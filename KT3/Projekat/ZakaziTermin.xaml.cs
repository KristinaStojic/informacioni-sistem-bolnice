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
    public partial class ZakaziTermin : Window
    {
        public Lekar lekarr;
        public Termin noviTermin;
        //
        public List<Sala> slobodneSale;
        public static ObservableCollection<string> sviSlobodni { get; set; }
        public Sala _sala;
        public List<string> vremeSala;
        public int brSala;

        public ZakaziTermin()
        {
            InitializeComponent();
            noviTermin = new Termin();
            datum.BlackoutDates.AddDatesInPast();
            //TODO: lekarr je izabrani lekar prijavljenog pacijenta
            //lekarr = 

            this.dgSearch.ItemsSource = MainWindow.lekari;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dgSearch.ItemsSource);
            view.Filter = UserFilter;
            /* Kada promenim samo zakazivanje pregleda */
            //combo.SelectedIndex = 1;
            //combo.IsEnabled = false;

            /*Sala s = SaleMenadzer.NadjiSaluPoId(2);
            s.zauzetiTermini = new List<ZauzeceSale>();
            ZauzeceSale zs = new ZauzeceSale("07:30", "08:00", "04/14/2021", 2);
            s.zauzetiTermini.Add(zs);*/

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // PrikaziTermin pt = new PrikaziTermin();
            //pt.Show();
            this.Close();
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
                try
                {
                    if (dgSearch.SelectedItems.Count > 0)
                    {
                        Lekar selLekar = (Lekar)dgSearch.SelectedItem;
                        s.Lekar = selLekar;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Izaberite lekara kod kog želite da zakažete termin", "Greška", MessageBoxButton.OK);
                }

                foreach (Pacijent p in PacijentiMenadzer.pacijenti)
                {
                    if (p.IdPacijenta == 1)  // STATICKO, dok ne bude prijavljivanje na sistem
                    {
                        s.Pacijent = p;
                    }
                }

                ZauzeceSale zs = new ZauzeceSale(vp, vk, formatted);
                _sala.zauzetiTermini.Add(zs);
                s.Prostorija = _sala;
                SaleMenadzer.sacuvajIzmjene(); // ?

                TerminMenadzer.ZakaziTermin(s);
                this.Close();

            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli sve podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void zdravstevniKarton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // elektronsko placanje
        }

        private void odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as Lekar).PrezimeLek.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgSearch.ItemsSource).Refresh();
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

        private void preferenca_Click(object sender, RoutedEventArgs e)
        {
            // prozor za odabir lekara po preferenci
            ZakaziTerminPreferenca ztp = new ZakaziTerminPreferenca();
            ztp.Show();
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
                        MessageBox.Show(s.Id.ToString());
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
                        MessageBox.Show(s.Id.ToString() + " " + brSala);
                    }
                }
            }
        }

        private void datum_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            sviSlobodni = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30",
                                                               "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30",
                                                               "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30",
                                                               "17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00"};
            ObservableCollection<string> sviSlobodni2 = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30",
                                                                                           "09:00", "09:30",  "10:00", "10:30",
                                                                                           "11:00", "11:30", "12:00", "12:30",
                                                                                           "13:00", "13:30", "14:00", "14:30",
                                                                                           "15:00", "15:30", "16:00", "16:30",
                                                                                           "17:00", "17:30", "18:00", "18:30",
                                                                                           "19:00", "19:30", "20:00"};
            string selectDatum = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            vremeSala = new List<string>();
            int i = 0;
            foreach (Sala s in slobodneSale)
            {
                foreach (ZauzeceSale zs in s.zauzetiTermini)
                {

                    /* foreach(string slot in sviSlobodni2)
                     {
                         if (zs.datumTermina.Equals(selectDatum) && zs.pocetakTermina.Equals(slot))
                         {
                             i++;
                             if (i == brSala)
                             {
                                 sviSlobodni.Remove(slot);
                             }
                         }
                         else
                         {
                             MessageBox.Show("OVDEE : " + s.Id + "-" + slot);
                         }
                     } */

                    //zs.idSale = s.Id;
                    if (zs.datumTermina.Equals(selectDatum))
                    {
                        // int i = 0;
                        //foreach (string slot in sviSlobodni2)
                        //{
                        switch (zs.pocetakTermina)
                        {
                            case "07:00":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("07:00");
                                        MessageBox.Show("Postoji zauzeti termin u sali " + s.brojSale + ": " + zs.datumTermina + " " + zs.pocetakTermina);
                                    }
                                    break;
                                }
                            case "07:30":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("07:30");
                                    }
                                    break;
                                }
                            case "08:00":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("08:00");
                                    }
                                    break;
                                }
                            case "08:30":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("08:30");
                                    }
                                    break;
                                }
                            case "09:00":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("09:00");
                                    }
                                    break;
                                }
                            case "09:30":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("09:30");
                                    }
                                    break;
                                }
                            case "10:00":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("10:00");
                                    }
                                    break;
                                }
                            case "10:30":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("10:30");
                                    }
                                    break;
                                }
                            case "11:00":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("11:00");
                                    }
                                    break;
                                }
                            case "11:30":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("11:30");
                                    }
                                    break;
                                }
                            case "12:00":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("12:00");
                                    }
                                    break;
                                }
                            case "12:30":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("12:30");
                                    }
                                    break;
                                }
                            case "13:00":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("13:00");
                                    }
                                    break;
                                }
                            case "13:30":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("13:30");
                                    }
                                    break;
                                }
                            case "14:00":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("14:00");
                                    }
                                    break;
                                }
                            case "14:30":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("14:30");
                                    }
                                    break;
                                }
                            case "15:00":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("15:00");
                                    }
                                    break;
                                }
                            case "15:30":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("15:30");
                                    }
                                    break;
                                }
                            case "16:00":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("16:00");
                                    }
                                    break;
                                }
                            case "16:30":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("16:30");
                                    }
                                    break;
                                }
                            case "17:00":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("17:00");
                                    }
                                    break;
                                }
                            case "17:30":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("17:30");
                                    }
                                    break;
                                }
                            case "18:00":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("18:00");
                                    }
                                    break;
                                }
                            case "18:30":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("18:30");
                                    }
                                    break;
                                }
                            case "19:00":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("19:00");
                                    }
                                    break;
                                }
                            case "19:30":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("19:30");
                                    }
                                    break;
                                }
                            case "20:00":
                                {
                                    i++;
                                    if (i == brSala)
                                    {
                                        sviSlobodni.Remove("20:00");
                                    }
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("Greska");
                                    break;
                                }
                        }
                        //break;
                    }
                }
            }
            vpp.ItemsSource = sviSlobodni;
            /*if (!sviSlobodni.Any())
            {
                MessageBox.Show("Ne postoji nijedan slobodan temrin za izabrani datum", "Izaberite drugi datum");
            }*/
            Console.WriteLine("----- Pre distincta -------");
            for (int j = 0; j < vremeSala.Count(); j++)
            {
                Console.WriteLine(vremeSala[j]);
            }
            Console.WriteLine("----- Posle distincta -------");
            for (int j = 0; j < vremeSala.Count(); j++)
            {
                vremeSala[i].Split('?');
                Console.WriteLine(vremeSala[j]);
            }
        }

        private void vpp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // novo zauzece, koje treba dodati u listu zauzeca ma sacuvaj
            string selektDatum = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string vreme = vpp.SelectedValue.ToString();

            /* foreach (string s in vremeSala)
              {
                  string[] split = s.Split('?');
                  if (vreme.Equals(split[0]))
                  {
                      _sala = SaleMenadzer.NadjiSaluPoId(int.Parse(split[1]));
                      MessageBox.Show("Sala u kojoj je zakazan pregled je: " + _sala.Id + " " + int.Parse(split[1])) ;
                      break;
                  }
              } */

            foreach (Sala s in slobodneSale)
            {
                if (!s.zauzetiTermini.Exists(x => x.pocetakTermina.Equals(vreme))) // ako ne postoji
                {
                    _sala = SaleMenadzer.NadjiSaluPoId(s.Id);
                    break;
                }
            }
        }
    }
}
