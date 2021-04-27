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
        public int idPacijent;
        public Lekar lekarr;
        //
        public List<Sala> slobodneSale;
        public static ObservableCollection<string> sviSlobodni { get; set; }
        public static ObservableCollection<string> pomocnaSviSlobodni { get; set; }
        public Sala prvaSlobodnaSala;
        public List<string> vremeSala;
        public int ukupanBrojSalaZaPregled;
        public static Pacijent prijavljeniPacijent;
        public static List<string> sviZauzetiZaSelektovaniDatum { get; set; }

        public ZakaziTermin(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            datum.BlackoutDates.AddDatesInPast();
            idPacijent = idPrijavljenogPacijenta;

            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            lekarr = prijavljeniPacijent.IzabraniLekar;
            if (lekarr != null)
            {
                this.imePrz.Text = prijavljeniPacijent.IzabraniLekar.ToString();
            }
            pomocnaSviSlobodni = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30",
                                                                "09:00", "09:30",  "10:00", "10:30",
                                                                "11:00", "11:30", "12:00", "12:30",
                                                                "13:00", "13:30", "14:00", "14:30",
                                                                "15:00", "15:30", "16:00", "16:30",
                                                                "17:00", "17:30", "18:00", "18:30",
                                                                "19:00", "19:30", "20:00"};
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                int brojTermina = TerminMenadzer.GenerisanjeIdTermina();
                String formatted = null;
                DateTime? selectedDate = datum.SelectedDate;
                if (selectedDate.HasValue)
                {
                    formatted = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                }
                String vp = vpp.Text;
                String vk = IzracunajVremeKraja(vp);

                TipTermina tp;
                if (combo.Text.Equals("Pregled"))
                {
                    tp = TipTermina.Pregled;
                }
                else
                {
                    tp = TipTermina.Operacija;
                }

                Termin s = new Termin(brojTermina, formatted, vp, vk, tp);
                Pacijent p = PacijentiMenadzer.PronadjiPoId(idPacijent);
                s.Pacijent = p;

                // TODO: da li ovo treba ispraviti?
                s.Lekar = lekarr;

                ZauzeceSale zs = new ZauzeceSale(vp, vk, formatted, s.IdTermin);
                prvaSlobodnaSala.zauzetiTermini.Add(zs);
                s.Prostorija = prvaSlobodnaSala;
                //SaleMenadzer.sacuvajIzmjene(); // ?
                TerminMenadzer.ZakaziTermin(s);

                Page uvid = new ZakazaniTerminiPacijent(idPacijent);
                this.NavigationService.Navigate(uvid);
            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli sve podatke", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        // vreme kraja pregleda
        public static string IzracunajVremeKraja(string vp)
        {
            String vk;
            String hh = vp.Substring(0, 2);
            String min = vp.Substring(3);
            if (min == "30")
            {
                int vkInt = int.Parse(hh);
                vkInt++;
                if (vkInt <= 9)
                {
                    vk = "0" + vkInt.ToString() + ":00";
                }
                else
                {
                    vk = vkInt.ToString() + ":00";
                }
            }
            else
            {
                vk = hh + ":30";
            }
            return vk;
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

        private void datum_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            string selektovaniDatum = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            sviSlobodni = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30",
                                                               "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30",
                                                               "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30",
                                                               "17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00"};

            if (datum.SelectedDate == DateTime.Now.Date)
            {
                MessageBox.Show("Odabrali ste danasnji dan");
                foreach(string slot in pomocnaSviSlobodni)
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
            
            int brojacZauzetihSala = 0;
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
                        DateTime pocetakZauzecaSale = DateTime.Parse(zs.datumPocetkaTermina);
                        DateTime krajZauzecaSale = DateTime.Parse(zs.datumKrajaTermina);

                        string[] vremePocetkaZauzecaSale = zs.pocetakTermina.Split(':');
                        int satiVremePocetka = Convert.ToInt32(vremePocetkaZauzecaSale[0]);
                        int minVremePocetka = Convert.ToInt32(vremePocetkaZauzecaSale[1]);

                        string[] vremeKrajaZauzecaSale = zs.krajTermina.Split(':');
                        int satiVremeKraja = Convert.ToInt32(vremeKrajaZauzecaSale[0]);
                        int minVremeKraja = Convert.ToInt32(vremeKrajaZauzecaSale[1]);

                        if (pocetakZauzecaSale == datum.SelectedDate && krajZauzecaSale == datum.SelectedDate)
                        {
                            foreach (string slot in pomocnaSviSlobodni)
                            {
                                string[] vreme = slot.Split(':');
                                int satiVreme = Convert.ToInt32(vreme[0]);
                                int minVreme = Convert.ToInt32(vreme[1]);

                                if (satiVreme > satiVremePocetka && satiVreme < satiVremeKraja) 
                                {
                                    sviZauzetiZaSelektovaniDatum.Add(slot);
                                }
                                else if (satiVreme == satiVremeKraja && minVreme == minVremeKraja && minVreme == 30)
                                {
                                    sviZauzetiZaSelektovaniDatum.Add(slot);
                                    //break;
                                }
                                else if (satiVreme == satiVremePocetka && minVreme == minVremePocetka)
                                {
                                    sviZauzetiZaSelektovaniDatum.Add(slot);
                                    //break;
                                }
                            }
                        }
                        else if (pocetakZauzecaSale < datum.SelectedDate && datum.SelectedDate < krajZauzecaSale)
                        {
                           
                            foreach(string slot in pomocnaSviSlobodni)
                            {
                                sviZauzetiZaSelektovaniDatum.Add(slot);
                            }
                        }
                        else if (pocetakZauzecaSale == datum.SelectedDate)
                        {
                            foreach (string slot in pomocnaSviSlobodni)  // mozda ce morati sviSlobodni2
                            {
                                string[] vreme = slot.Split(':');
                                int satiVreme = Convert.ToInt32(vreme[0]);
                                int minVreme = Convert.ToInt32(vreme[1]);

                                if (satiVreme >= satiVremePocetka)
                                {
                                    sviZauzetiZaSelektovaniDatum.Add(slot);
                                }
                            }
                        }
                        else if (krajZauzecaSale == datum.SelectedDate)
                        {
                            foreach (string slot in pomocnaSviSlobodni)
                            {
                                string[] vreme = slot.Split(':');
                                int satiVreme = Convert.ToInt32(vreme[0]);
                                int minVreme = Convert.ToInt32(vreme[1]);

                                if (satiVreme < satiVremeKraja) 
                                {
                                    sviZauzetiZaSelektovaniDatum.Add(slot);
                                }
                            }
                        }
                    }
                }
            }
            string x = null;
            foreach (string s in sviZauzetiZaSelektovaniDatum)
            {
                x += s + " ";
            }
            //MessageBox.Show(x);


            foreach (string slot in pomocnaSviSlobodni)
            {
                brojacZauzetihSala = 0;
                foreach(string zauzeti in sviZauzetiZaSelektovaniDatum)
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

            vpp.ItemsSource = sviSlobodni;
            if (!sviSlobodni.Any())
            {
                MessageBox.Show("Ne postoji nijedan slobodan termin za izabrani datum, molimo Vas izaberite drugi datum", "Upozorenje");
            }
        }

        private void vpp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selektDatum = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string selektovanoVreme = vpp.SelectedValue.ToString();
            string[] vreme = selektovanoVreme.Split(':');
            int satiVreme = Convert.ToInt32(vreme[0]);
            int minVreme = Convert.ToInt32(vreme[1]);

            foreach (Sala sala in slobodneSale)
            {
                foreach(ZauzeceSale zauzece in sala.zauzetiTermini)
                {
                    if (prvaSlobodnaSala != null)
                    {
                        break;
                    }
                    string[] vremePocetkaZauzecaSale = zauzece.pocetakTermina.Split(':');
                    int satiVremePocetka = Convert.ToInt32(vremePocetkaZauzecaSale[0]);
                    int minVremePocetka = Convert.ToInt32(vremePocetkaZauzecaSale[1]);

                    if (zauzece.datumPocetkaTermina.Equals(selektDatum))
                    {
                        if(satiVreme < satiVremePocetka)
                        {
                            prvaSlobodnaSala = SaleMenadzer.NadjiSaluPoId(sala.Id);
                            break;
                        } 
                        else if(satiVreme == satiVremePocetka && minVreme < minVremePocetka)
                        {
                            prvaSlobodnaSala = SaleMenadzer.NadjiSaluPoId(sala.Id);
                            break;
                        }
                    } 
                    else if (zauzece.datumKrajaTermina.Equals(selektDatum))
                    {
                        string[] vremeKrajaZauzecaSale = zauzece.krajTermina.Split(':');
                        int satiVremeKraja = Convert.ToInt32(vremeKrajaZauzecaSale[0]);
                        int minVremeKraja = Convert.ToInt32(vremeKrajaZauzecaSale[1]);

                        if (satiVreme > satiVremeKraja)
                        {
                            prvaSlobodnaSala = SaleMenadzer.NadjiSaluPoId(sala.Id);
                            break;
                        }
                        else if (satiVreme == satiVremeKraja && minVreme == minVremeKraja)
                        {
                            prvaSlobodnaSala = SaleMenadzer.NadjiSaluPoId(sala.Id);
                            break;
                        }
                    }
                    else if(!sala.zauzetiTermini.Exists(x => x.datumPocetkaTermina.Equals(selektDatum)) && !sala.zauzetiTermini.Exists(x => x.datumKrajaTermina.Equals(selektDatum)))
                    {
                        DateTime pocetakZauzecaSale = DateTime.Parse(zauzece.datumPocetkaTermina);
                        DateTime krajZauzecaSale = DateTime.Parse(zauzece.datumKrajaTermina);
                        if ((pocetakZauzecaSale < datum.SelectedDate) && (datum.SelectedDate < krajZauzecaSale))
                        {
                            break;
                        }
                        else
                        {
                            prvaSlobodnaSala = SaleMenadzer.NadjiSaluPoId(sala.Id);
                        }
                    }
                }
            }

            /*foreach (Sala s in slobodneSale)
            {
                if (!s.zauzetiTermini.Exists(x => x.pocetakTermina.Equals(vreme))) // ako ne postoji
                {
                    prvaSlobodnaSala = SaleMenadzer.NadjiSaluPoId(s.Id);
                    break;
                }
            }
            if(prvaSlobodnaSala == null)
            {
                MessageBox.Show("Ne postoji slobodna sala za odabrani datum i vreme");
            }*/
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
