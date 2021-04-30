using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Model;
using Projekat.Model;
using static Model.Termin;

namespace Projekat
{
    // TODO
    public partial class ZakaziTermin : Page
    {
        private static int maksimalniJednocifren = 9;
        private static int oznakaZaRenoviranje = 0;
        private static int idPacijent;
        public static Lekar izabraniLekar;
        private List<Sala> slobodneSale;
        private static ObservableCollection<string> sviSlobodni { get; set; }
        private static ObservableCollection<string> pomocnaSviSlobodniSlotovi { get; set; }
        private Sala prvaSlobodnaSala;
        private int ukupanBrojSalaZaPregled;
        private static Pacijent prijavljeniPacijent;
        public static List<string> sviZauzetiZaSelektovaniDatum { get; set; }

        public ZakaziTermin(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            datum.BlackoutDates.AddDatesInPast();
            idPacijent = idPrijavljenogPacijenta;
            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            if (izabraniLekar != null)
            {
                this.imePrz.Text = izabraniLekar.ToString();
            }
            else
            {
                izabraniLekar = prijavljeniPacijent.IzabraniLekar;
                this.imePrz.Text = izabraniLekar.ToString();
            }
            pomocnaSviSlobodniSlotovi = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30", "11:00", "11:30", "12:00", "12:30",
                                                                "13:00", "13:30", "14:00", "14:30","15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                                "19:00", "19:30", "20:00"};
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                int brojTermina = TerminMenadzer.GenerisanjeIdTermina();
                String datumTermina = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture); 
                String vp = vpp.Text;
                String vk = IzracunajVremeKrajaPregleda(vp);

                TipTermina tp;
                if (combo.Text.Equals("Pregled"))
                {
                    tp = TipTermina.Pregled;
                }
                else
                {
                    tp = TipTermina.Operacija;
                }

                Termin termin = new Termin(brojTermina, datumTermina, vp, vk, tp);
                Pacijent p = PacijentiMenadzer.PronadjiPoId(idPacijent);
                termin.Pacijent = p;
                termin.Lekar = izabraniLekar;

                ZauzeceSale zs = new ZauzeceSale(vp, vk, datumTermina, termin.IdTermin);
                prvaSlobodnaSala.zauzetiTermini.Add(zs);
                termin.Prostorija = prvaSlobodnaSala;
                TerminMenadzer.ZakaziTermin(termin);
                Anketa anketa = new Anketa(AnketaMenadzer.GenerisanjeIdAnkete(), VrstaAnkete.ZaLekare, idPacijent, termin.IdTermin);
                AnketaMenadzer.ankete.Add(anketa);
                ProveriAnketuZaKliniku();

                Page uvid = new ZakazaniTerminiPacijent(idPacijent);
                this.NavigationService.Navigate(uvid);
            }
            catch (System.Exception)
            {
                MessageBox.Show("Morate popuniti sva polja kako biste zakazali termin", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void ProveriAnketuZaKliniku()
        {
            int brojacTermina = 0;
            foreach(Termin termin in TerminMenadzer.termini)
            {
                if (termin.Pacijent.IdPacijenta == idPacijent)
                {
                    brojacTermina++;
                    if (brojacTermina == PrikaziAnkete.minBrojTerminaZaAnketuKlinika)
                    {
                        Anketa anketa = new Anketa(AnketaMenadzer.GenerisanjeIdAnkete(), VrstaAnkete.ZaKliniku, idPacijent, 0);
                        AnketaMenadzer.ankete.Add(anketa);
                        return;
                    }
                }
            }
        }

        public static string IzracunajVremeKrajaPregleda(string vp)
        {
            string hh = vp.Substring(0, 2);
            string mm = vp.Substring(3);
            if (mm == "30")
            {
                int jednocifrenSat = int.Parse(hh);
                jednocifrenSat++;
                if (jednocifrenSat <= maksimalniJednocifren)
                {
                    return "0" + jednocifrenSat.ToString() + ":00";
                }
                else
                {
                    return jednocifrenSat.ToString() + ":00";
                }
            }
            else
            {
                return hh + ":30";
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // elektronsko placanje
            MessageBox.Show("Elektronsko placanje ce uskoro biti implementirano", "Obavestenje");
        }

        private void odustani_Click(object sender, RoutedEventArgs e)
        {
           // this.Close();
        }
     
        private void preferenca_Click(object sender, RoutedEventArgs e)
        {
            // prozor za odabir lekara po preferenci
            Page ztp = new ZakaziTerminPreferenca(idPacijent);
            this.NavigationService.Navigate(ztp);
        }

        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /* tip termina*/
            slobodneSale = new List<Sala>();
            ukupanBrojSalaZaPregled = 0;
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.TipSale.Equals(tipSale.SalaZaPregled))
                {
                    slobodneSale.Add(s);
                    ukupanBrojSalaZaPregled++;
                }
            }
        }

        private void IzbaciProsleSlotoveZaDanasnjiDan()
        {
            if (datum.SelectedDate == DateTime.Now.Date)
            {
                foreach (string slot in pomocnaSviSlobodniSlotovi)
                {
                    DateTime vreme = DateTime.Parse(slot);
                    DateTime sada = DateTime.Now;
                    if (vreme.TimeOfDay <= sada.TimeOfDay)
                    {
                        sviSlobodni.Remove(slot);
                    }

                }
            }
        }

        private static int ParsiranjeSataVremenskogSlota(String vreme)
        {
            String sat = vreme.Split(':')[0]; 
            return Convert.ToInt32(sat);
        }

        private static int ParsiranjeMinutaVremenskogSlota(String vreme)
        {
            String minuti = vreme.Split(':')[1];
            return Convert.ToInt32(minuti);
        }


        private void UkloniZauzeteSlotove()
        {
            int brojacZauzetihSala;
            foreach (string slot in pomocnaSviSlobodniSlotovi)
            {
                brojacZauzetihSala = 0;
                foreach (string zauzeti in sviZauzetiZaSelektovaniDatum)
                {
                    if (slot.Equals(zauzeti))
                    {
                        brojacZauzetihSala++;
                        if (brojacZauzetihSala == ukupanBrojSalaZaPregled)
                        {
                            sviSlobodni.Remove(slot);
                            break;
                        }
                    }
                }
            }
        }

        private void datum_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            string selektovaniDatum = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            sviSlobodni = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00"};

            IzbaciProsleSlotoveZaDanasnjiDan();

            if (slobodneSale == null)
            {
                MessageBox.Show("Prvo izberite tip termina", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                sviZauzetiZaSelektovaniDatum = new List<string>();
                foreach (Sala s in slobodneSale)
                {
                    foreach (ZauzeceSale zs in s.zauzetiTermini)
                    {
                        DateTime datumPocetkaZauzeca = DateTime.Parse(zs.datumPocetkaTermina);
                        DateTime datumKrajaZauzeca = DateTime.Parse(zs.datumKrajaTermina);
                        int satiVremePocetka = ParsiranjeSataVremenskogSlota(zs.pocetakTermina);
                        int minVremePocetka = ParsiranjeMinutaVremenskogSlota(zs.pocetakTermina);
                        int satiVremeKraja = ParsiranjeSataVremenskogSlota(zs.krajTermina);
                        if (datumPocetkaZauzeca.Equals(datum.SelectedDate) && datumKrajaZauzeca.Equals(datum.SelectedDate))
                        {
                            foreach (string slot in pomocnaSviSlobodniSlotovi)
                            {
                                int satiVreme = ParsiranjeSataVremenskogSlota(slot);
                                int minVreme = ParsiranjeMinutaVremenskogSlota(slot);
                                /* provera u slucaju da renoviranje traje jedan dan */
                                if (zs.idTermina == oznakaZaRenoviranje)
                                {
                                    if (satiVreme >= satiVremePocetka && satiVreme < satiVremeKraja)
                                    {
                                        sviZauzetiZaSelektovaniDatum.Add(slot);
                                    }
                                }
                                /* provera da se selektovani datum poklapa sa nekim vec zakazanim terminom */
                                else if (satiVreme == satiVremePocetka && minVreme == minVremePocetka)
                                {
                                    MessageBox.Show(slot);
                                    sviZauzetiZaSelektovaniDatum.Add(slot);
                                }
                            }
                        }
                        /* ukoliko je selektovani datum u periodu renoviranja sale */
                        else if (datumPocetkaZauzeca < datum.SelectedDate && datum.SelectedDate < datumKrajaZauzeca)
                        {
                            foreach (string slot in pomocnaSviSlobodniSlotovi)
                            {
                                sviZauzetiZaSelektovaniDatum.Add(slot);
                            }
                        }
                        /* provera da li se selektovani datum poklapa sa pocetkom renoviranja sale - slobodni termini pre renoviranja */
                        else if (datumPocetkaZauzeca == datum.SelectedDate)
                        {
                            foreach (string slot in pomocnaSviSlobodniSlotovi)  // mozda ce morati sviSlobodni2
                            {
                                int satiVreme = ParsiranjeSataVremenskogSlota(slot);
                                if (satiVreme >= satiVremePocetka)
                                {
                                    sviZauzetiZaSelektovaniDatum.Add(slot);
                                }
                            }
                        }
                        /* provera da li se selektovani datum poklapa sa krajem renoviranja sale - slobodni termini posle renoviranja */
                        else if (datumKrajaZauzeca == datum.SelectedDate)
                        {
                            foreach (string slot in pomocnaSviSlobodniSlotovi)
                            {
                                int satiVreme = ParsiranjeSataVremenskogSlota(slot);
                                if (satiVreme < satiVremeKraja)
                                {
                                    sviZauzetiZaSelektovaniDatum.Add(slot);
                                }
                            }
                        }
                    }
                }
            }
            //string x = null;
            // if (sviZauzetiZaSelektovaniDatum != null)
            //{
            /*foreach (string s in sviZauzetiZaSelektovaniDatum)
            {
                x += s + " ";
            }
            MessageBox.Show(x);*/
            UkloniZauzeteSlotove();
            
            vpp.ItemsSource = sviSlobodni;
            /* if (!sviSlobodni.Any()) // ovo mozda i ne moram --> web
             {
                 MessageBox.Show("Ne postoji nijedan slobodan termin za izabrani datum, molimo Vas izaberite drugi datum", "Upozorenje");
             }*/
        }

        private bool ProveriVremeZauzecaZaTermine(string selektovaniDatum, string selektovaniSlot, Sala sala)
        {
            //bool postojiTermin = false;
            foreach (ZauzeceSale zauzece in sala.zauzetiTermini)
            {
                if (prvaSlobodnaSala != null) break;
                // provera da li se poklapa sa terminom
                if (zauzece.idTermina != oznakaZaRenoviranje && zauzece.datumPocetkaTermina.Equals(selektovaniDatum))
                {
                    if (zauzece.pocetakTermina.Equals(selektovaniSlot))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ProveriVremeZauzecaZaRenoviranje(string selektovaniDatum, int satiVreme, Sala sala)
        {
            foreach (ZauzeceSale zauzece in sala.zauzetiTermini)
            {
                if (prvaSlobodnaSala != null) break;

                if (zauzece.idTermina == oznakaZaRenoviranje)
                {
                    int satiVremePocetka = ParsiranjeSataVremenskogSlota(zauzece.pocetakTermina);
                    int satiVremeKraja = ParsiranjeSataVremenskogSlota(zauzece.krajTermina);

                    if (selektovaniDatum.Equals(zauzece.datumPocetkaTermina) && selektovaniDatum.Equals(zauzece.datumKrajaTermina))
                    {
                        if (satiVremePocetka <= satiVreme && satiVreme < satiVremeKraja)
                        {
                            return true;
                        }
                    }
                    else if (selektovaniDatum.Equals(zauzece.datumPocetkaTermina))
                    {
                        if (satiVremePocetka <= satiVreme)
                        {
                            return true;
                        }
                    }
                    else if (selektovaniDatum.Equals(zauzece.datumKrajaTermina))
                    {
                        if (satiVreme < satiVremeKraja)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void vpp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selektovaniDatum = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string selektovaniSlot = vpp.SelectedValue.ToString();
            int satiVreme = ParsiranjeSataVremenskogSlota(selektovaniSlot);
            /* Pronalazenje sale za koju je slobodan izabrani slot*/
            foreach (Sala sala in slobodneSale)
            {
                bool PostojiZauzece = ProveriVremeZauzecaZaTermine(selektovaniDatum, selektovaniSlot, sala);
                PostojiZauzece = ProveriVremeZauzecaZaRenoviranje(selektovaniDatum, satiVreme, sala);
                if (!PostojiZauzece)
                {
                    prvaSlobodnaSala = sala;
                    break;
                }
            }
        }

        private void odjava_Click(object sender, RoutedEventArgs e)
        {
            Page odjava = new PrijavaPacijent();
            this.NavigationService.Navigate(odjava);
        }

        public void karton_Click(object sender, RoutedEventArgs e)
        {
            Page karton = new ZdravstveniKartonPacijent(idPacijent);
            this.NavigationService.Navigate(karton);
        }

        public void zakazi_Click(object sender, RoutedEventArgs e)
        {
            Page zakaziTermin = new ZakaziTermin(idPacijent);
            this.NavigationService.Navigate(zakaziTermin);
        }

        public void uvid_Click(object sender, RoutedEventArgs e)
        {
            Page uvid = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvid);
        }

        private void pocetna_Click(object sender, RoutedEventArgs e)
        {
            Page pocetna = new PrikaziTermin(idPacijent);
            this.NavigationService.Navigate(pocetna);
        }
    }
}
