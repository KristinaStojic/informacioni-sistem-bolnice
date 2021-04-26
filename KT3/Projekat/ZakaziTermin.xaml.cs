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
        public static ObservableCollection<string> sviSlobodni2 { get; set; }
        public Sala prvaSlobodnaSala;
        public List<string> vremeSala;
        public int brSala;
        public static Pacijent prijavljeniPacijent;

        public ZakaziTermin(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            datum.BlackoutDates.AddDatesInPast();
            idPacijent = idPrijavljenogPacijenta;

            // TODO: ????
            /*if(l != null)
            {
                //MessageBox.Show("Promenjen lekar");
                lekarr = l;
                this.imePrz.Text = l.ToString();
            } else
            {
                prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
                lekarr = prijavljeniPacijent.IzabraniLekar;
                this.imePrz.Text = prijavljeniPacijent.IzabraniLekar.ToString();
            }*/
           
            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            lekarr = prijavljeniPacijent.IzabraniLekar;
            if (lekarr != null)
            {
                this.imePrz.Text = prijavljeniPacijent.IzabraniLekar.ToString();
            }
            

            sviSlobodni2 = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30",
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
                Console.WriteLine(selectedDate);
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
                MessageBox.Show("Niste uneli sve podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

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
            string tip = combo.SelectedValue.ToString().Split(' ')[1];
            brSala = 0;
            if (tip.Equals("Operacija"))
            {
                foreach (Sala s in SaleMenadzer.sale)
                {
                    if (s.TipSale.Equals(tipSale.OperacionaSala))
                    {
                        slobodneSale.Add(s);
                        brSala++;
                    }
                }
            }
            else
            {
                foreach (Sala s in SaleMenadzer.sale)
                {
                    if (s.TipSale.Equals(tipSale.SalaZaPregled))
                    {
                        slobodneSale.Add(s);
                        brSala++;
                       // MessageBox.Show(s.Id.ToString() + " " + brSala);
                    }
                }
            }
        }

        // obrisati
        public ObservableCollection<string> danasnjiSlotovi()
        {
            sviSlobodni = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30",
                                                               "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30",
                                                               "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30",
                                                               "17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00"};


            return sviSlobodni;

        }

        private void datum_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectDatum = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            sviSlobodni = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30",
                                                               "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30",
                                                               "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30",
                                                               "17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00"};

            if (datum.SelectedDate == DateTime.Now.Date)
            {
               // MessageBox.Show("Odabrali ste danasnji dan");
                foreach(string slot in sviSlobodni2)
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
                foreach (Sala s in slobodneSale)
                {
                    foreach (ZauzeceSale zs in s.zauzetiTermini)
                    {
                        if (zs.datumTermina.Equals(selectDatum))
                        {
                            // TODO: cast u int i poredjenje
                            switch (zs.pocetakTermina)
                            {
                                case "07:00":
                                {
                                    brojacZauzetihSala++;
                                    if (brojacZauzetihSala >= brSala)
                                    {
                                        sviSlobodni.Remove("07:00");
                                        // MessageBox.Show("Postoji zauzeti termin u sali " + s.brojSale + ": " + zs.datumTermina + " " + zs.pocetakTermina);
                                    }
                                    break;
                                }
                                case "07:30":
                                {
                                    brojacZauzetihSala++;
                                    if (brojacZauzetihSala >= brSala)
                                    {
                                        sviSlobodni.Remove("07:30");
                                    }
                                    break;
                                }
                                case "08:00":
                                {
                                    brojacZauzetihSala++;
                                    if (brojacZauzetihSala >= brSala)
                                    {
                                        sviSlobodni.Remove("08:00");
                                    }
                                    break;
                                }
                                case "08:30":
                                {
                                    brojacZauzetihSala++;
                                    if (brojacZauzetihSala >= brSala)
                                    {
                                        sviSlobodni.Remove("08:30");
                                    }
                                    break;
                                }
                                case "09:00":
                                {
                                    brojacZauzetihSala++;
                                    if (brojacZauzetihSala >= brSala)
                                    {
                                        sviSlobodni.Remove("09:00");
                                    }
                                    break;
                                }
                                case "09:30":
                                {
                                    brojacZauzetihSala++;
                                    if (brojacZauzetihSala >= brSala)
                                    {
                                        sviSlobodni.Remove("09:30");
                                    }
                                    break;
                                }
                                case "10:00":
                                {
                                    brojacZauzetihSala++;
                                    if (brojacZauzetihSala >= brSala)
                                    {
                                        sviSlobodni.Remove("10:00");
                                    }
                                    break;
                                }
                                case "10:30":
                                {
                                    brojacZauzetihSala++;
                                    if (brojacZauzetihSala >= brSala)
                                    {
                                        sviSlobodni.Remove("10:30");
                                    }
                                    break;
                                }
                                case "11:00":
                                {
                                    brojacZauzetihSala++;
                                    if (brojacZauzetihSala >= brSala)
                                    {
                                        sviSlobodni.Remove("11:00");
                                    }
                                    break;
                                }
                                case "11:30":
                                {
                                    brojacZauzetihSala++;
                                    if (brojacZauzetihSala >= brSala)
                                    {
                                        sviSlobodni.Remove("11:30");
                                    }
                                    break;
                                    }
                                case "12:00":
                                {
                                    brojacZauzetihSala++;
                                    if (brojacZauzetihSala >= brSala)
                                    {
                                        sviSlobodni.Remove("12:00");
                                    }
                                    break;
                                }
                                case "12:30":
                                    {
                                        brojacZauzetihSala++;
                                        if (brojacZauzetihSala >= brSala)
                                        {
                                            sviSlobodni.Remove("12:30");
                                        }
                                        break;
                                    }
                                case "13:00":
                                    {
                                        brojacZauzetihSala++;
                                        if (brojacZauzetihSala >= brSala)
                                        {
                                            sviSlobodni.Remove("13:00");
                                        }
                                        break;
                                    }
                                case "13:30":
                                    {
                                        brojacZauzetihSala++;
                                        if (brojacZauzetihSala >= brSala)
                                        {
                                            sviSlobodni.Remove("13:30");
                                        }
                                        break;
                                    }
                                case "14:00":
                                    {
                                        brojacZauzetihSala++;
                                        if (brojacZauzetihSala >= brSala)
                                        {
                                            sviSlobodni.Remove("14:00");
                                        }
                                        break;
                                    }
                                case "14:30":
                                    {
                                        brojacZauzetihSala++;
                                        if (brojacZauzetihSala >= brSala)
                                        {
                                            sviSlobodni.Remove("14:30");
                                        }
                                        break;
                                    }
                                case "15:00":
                                    {
                                        brojacZauzetihSala++;
                                        if (brojacZauzetihSala >= brSala)
                                        {
                                            sviSlobodni.Remove("15:00");
                                        }
                                        break;
                                    }
                                case "15:30":
                                    {
                                        brojacZauzetihSala++;
                                        if (brojacZauzetihSala >= brSala)
                                        {
                                            sviSlobodni.Remove("15:30");
                                        }
                                        break;
                                    }
                                case "16:00":
                                    {
                                        brojacZauzetihSala++;
                                        if (brojacZauzetihSala >= brSala)
                                        {
                                            sviSlobodni.Remove("16:00");
                                        }
                                        break;
                                    }
                                case "16:30":
                                    {
                                        brojacZauzetihSala++;
                                        if (brojacZauzetihSala >= brSala)
                                        {
                                            sviSlobodni.Remove("16:30");
                                        }
                                        break;
                                    }
                                case "17:00":
                                    {
                                        brojacZauzetihSala++;
                                        if (brojacZauzetihSala >= brSala)
                                        {
                                            sviSlobodni.Remove("17:00");
                                        }
                                        break;
                                    }
                                case "17:30":
                                    {
                                        brojacZauzetihSala++;
                                        if (brojacZauzetihSala >= brSala)
                                        {
                                            sviSlobodni.Remove("17:30");
                                        }
                                        break;
                                    }
                                case "18:00":
                                    {
                                        brojacZauzetihSala++;
                                        if (brojacZauzetihSala >= brSala)
                                        {
                                            sviSlobodni.Remove("18:00");
                                        }
                                        break;
                                    }
                                case "18:30":
                                    {
                                        brojacZauzetihSala++;
                                        if (brojacZauzetihSala >= brSala)
                                        {
                                            sviSlobodni.Remove("18:30");
                                        }
                                        break;
                                    }
                                case "19:00":
                                    {
                                        brojacZauzetihSala++;
                                        if (brojacZauzetihSala >= brSala)
                                        {
                                            sviSlobodni.Remove("19:00");
                                        }
                                        break;
                                    }
                                case "19:30":
                                {
                                    brojacZauzetihSala++;
                                    if (brojacZauzetihSala >= brSala)
                                    {
                                        sviSlobodni.Remove("19:30");
                                    }
                                    break;
                                }
                                case "20:00":
                                {
                                    brojacZauzetihSala++;
                                    if (brojacZauzetihSala >= brSala)
                                    {
                                        sviSlobodni.Remove("20:00");
                                    }
                                    break;
                                }
                                default:
                                {
                                    Console.WriteLine("Greska");
                                    MessageBox.Show("Ne postoje slobodni slotovi za izabrani datum");
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            vpp.ItemsSource = sviSlobodni;
            if (!sviSlobodni.Any())
            {
                MessageBox.Show("Ne postoji nijedan slobodan temrin za izabrani datum", "Izaberite drugi datum");
            }
        }

        private void vpp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selektDatum = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string vreme = vpp.SelectedValue.ToString();

            foreach (Sala s in slobodneSale)
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
