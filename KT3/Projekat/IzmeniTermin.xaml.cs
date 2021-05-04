using Model;
using Projekat.Model;
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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmeniTermin.xaml
    /// </summary>
    public partial class IzmeniTermin : Page
    {
        public Termin termin;
        private Lekar idLek;
        public static int idPacijent;
        //
        public List<Sala> slobodneSale;
        public static ObservableCollection<string> sviSlobodni { get; set; }
        public static ObservableCollection<string> pomocnaSviSlobodniSlotovi { get; set; }
        public Sala prvaSlobodnaSala;
        public List<string> vremeSala;
        public int ukupanBrojSalaZaPregled;
        public static Pacijent prijavljeniPacijent;
        public static List<string> sviZauzetiZaSelektovaniDatum { get; set; }
        public static int oznakaZaRenoviranje = 0;

        public IzmeniTermin(Termin izabraniTermin)
        {
            InitializeComponent();
            this.DataContext = this;
            pomocnaSviSlobodniSlotovi = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00"};
            idPacijent = izabraniTermin.Pacijent.IdPacijenta;
            OgraniciIzborNovogDatuma(izabraniTermin);
            this.termin = izabraniTermin;
            if (izabraniTermin != null)
            {
                TipTermina tp;
                if (izabraniTermin.tipTermina.Equals(TipTermina.Operacija))
                {
                    this.combo.SelectedIndex = 0;
                }
                else if (izabraniTermin.tipTermina.Equals(TipTermina.Pregled))
                {
                    this.combo.SelectedIndex = 1;
                }
                tp = izabraniTermin.tipTermina;
                this.imePrz.Text = izabraniTermin.Lekar.ImeLek + " " + izabraniTermin.Lekar.PrezimeLek;
                prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
                idLek = izabraniTermin.Lekar;
                this.datum.DisplayDate = DateTime.Parse(izabraniTermin.Datum);
                int brojac = 0;
                this.dgSearch.ItemsSource = MainWindow.lekari;
                // TODO: ispraviti selektovanog lekara: upotrebiti dgsearch
                foreach (Lekar lekar in MainWindow.lekari)
                {
                    brojac++;
                    if (lekar.IdLekara.Equals(izabraniTermin.Lekar.IdLekara))
                    {
                        this.dgSearch.SelectedIndex = brojac;
                        break;
                    }
                }
                this.vpp.SelectedItem = izabraniTermin.VremePocetka;
            }
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dgSearch.ItemsSource);
            view.Filter = UserFilter;
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
        }

        private void OgraniciIzborNovogDatuma(Termin izabraniTermin)
        {
            // TODO: provera da li su  ti datumi u proslosti
            CalendarDateRange daniPreTermina = new CalendarDateRange();
            daniPreTermina.Start = DateTime.Parse(izabraniTermin.Datum).AddDays(-1000);//DateTime.Parse(izabraniTermin.Datum).AddDays(3);
            daniPreTermina.End = DateTime.Parse(izabraniTermin.Datum).AddDays(-3);
            datum.BlackoutDates.Add(daniPreTermina);
            CalendarDateRange daniPosleTermina = new CalendarDateRange();
            daniPosleTermina.Start = DateTime.Parse(izabraniTermin.Datum).AddDays(3);
            daniPosleTermina.End = DateTime.Parse(izabraniTermin.Datum).AddDays(1000);
            datum.BlackoutDates.Add(daniPosleTermina);
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as Lekar).PrezimeLek.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0) 
                        || ((item as Lekar).ImeLek.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                        || ((item as Lekar).specijalizacija.ToString().IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgSearch.ItemsSource).Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // btn odustani
            Page uvidZakazaniTermini = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvidZakazaniTermini);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (termin.Pomeren == false)
            {
                IzmeniIzabraniTermin();
            }
            else
            {
                MessageBox.Show("Nemoguce je pomeriti ovaj termin jer je vec jednom pomeren", "Upozorenje");
            }
        }

        private void IzmeniIzabraniTermin()
        {
            try
            {
                String datumTermina = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                /*if (selectedDate.HasValue)
                {
                    formatted = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                }*/

                String vp = vpp.Text;
                String vk = ZakaziTermin.IzracunajVremeKrajaPregleda(vp);

                TipTermina tp;
                if (combo.Text.Equals("Pregled"))
                {
                    tp = TipTermina.Pregled;
                }
                else
                {
                    tp = TipTermina.Operacija;
                }
                Termin t = new Termin(termin.IdTermin, datumTermina, vp, vk, tp);
                t.Pacijent = prijavljeniPacijent;
                t.Pomeren = true;

                SaleMenadzer.ObrisiZauzeceSale(termin.Prostorija.Id, termin.IdTermin);
                ZauzeceSale zs = new ZauzeceSale(vp, vk, datumTermina, t.IdTermin);
                prvaSlobodnaSala.zauzetiTermini.Add(zs);
                t.Prostorija = prvaSlobodnaSala;
                if (dgSearch.SelectedItems.Count > 0)
                {
                    Lekar selLekar = (Lekar)dgSearch.SelectedItem;
                    t.Lekar = selLekar;
                }
                TerminMenadzer.IzmeniTermin(termin, t);
                Page uvidZakazaniTermini = new ZakazaniTerminiPacijent(idPacijent);
                this.NavigationService.Navigate(uvidZakazaniTermini);
            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

      

        private void dgSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSearch.SelectedItems.Count > 0)
            {
                Lekar item = (Lekar)dgSearch.SelectedItems[0];
                imePrz.Text = item.ToString();
            }
        }

        private void imePrz_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {


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
        private static int ParsiranjeSataVremenskogSlota(string vreme)
        {
            string sat = vreme.Split(':')[0];
            return Convert.ToInt32(sat);
        }

        private static int ParsiranjeMinutaVremenskogSlota(string vreme)
        {
            string minuti = vreme.Split(':')[1];
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
            sviSlobodni = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30", "11:00", "11:30", "12:00", "12:30",
                                                               "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00"};
            string selectDatum = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (datum.SelectedDate == DateTime.Now.Date)
            {
                foreach (string slot in pomocnaSviSlobodniSlotovi)
                {
                    DateTime dt = DateTime.Parse(slot);
                    DateTime sada = DateTime.Now;
                    if (dt.TimeOfDay <= sada.TimeOfDay)
                    {
                        //MessageBox.Show(dt.TimeOfDay.ToString() + " " + sada.TimeOfDay.ToString());
                        sviSlobodni.Remove(slot);
                    }
                }
            }
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
                        //int minVremeKraja = Convert.ToInt32(zs.krajTermina.Split(':')[1]);


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
            UkloniZauzeteSlotove();
            this.vpp.ItemsSource = sviSlobodni;
        }

        private bool ProveriVremeZauzecaZaTermine(string selektovaniDatum, string selektovaniSlot, Sala sala)
        {
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
                    int minVremePocetka = ParsiranjeMinutaVremenskogSlota(zauzece.pocetakTermina);
                    int satiVremeKraja = ParsiranjeSataVremenskogSlota(zauzece.krajTermina);
                    int minVremeKraja = ParsiranjeMinutaVremenskogSlota(zauzece.krajTermina);

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
                bool postojiZauzece = ProveriVremeZauzecaZaTermine(selektovaniDatum, selektovaniSlot, sala) || ProveriVremeZauzecaZaRenoviranje(selektovaniDatum, satiVreme, sala);
                //postojiZauzece = ProveriVremeZauzecaZaRenoviranje(selektovaniDatum, satiVreme, sala);
                if (!postojiZauzece)
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

        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            Page prikaziAnkete = new PrikaziAnkete(idPacijent);
            this.NavigationService.Navigate(prikaziAnkete);
        }
    }
}
