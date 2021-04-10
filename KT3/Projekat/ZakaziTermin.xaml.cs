using System;
using System.Collections.Generic;
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
using static Model.Termin;

namespace Projekat
{
    // TODO
    public partial class ZakaziTermin : Window
    {
        public Lekar lekarr;
        public Termin noviTermin;
        //
        public IObservable<string> sviSlobodni;
        public List<Sala> slobodneSale;
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
                } catch (Exception)
                {
                    MessageBox.Show("Izaberite lekara kod kog želite da zakažete termin", "Greška", MessageBoxButton.OK);
                }
                /*else
                {
                    // TODO: optimizovati!
                    s.Lekar = lekarr;
                }*/


                foreach (Pacijent p in PacijentiMenadzer.pacijenti)
                {
                    if (p.IdPacijenta == 1)
                    {
                        s.Pacijent = p;
                    }
                }
                foreach (Sala sala in SaleMenadzer.sale)
                {
                    if (sala.Status.Equals(status.Slobodna))
                    {
                        s.Prostorija = sala;
                    }
                    /*try
                    {
                        if (sala.Status.Equals(status.Slobodna))
                        {
                            s.Prostorija = sala;  // kad naidje na prvu slobodnu

                            // TODO: ispraviti , NIJE DOBRO!!!
                            for(int i = 0; i < PrikaziSalu.Sale.Count(); i++) 
                            { 
                                if (PrikaziSalu.Sale[i].Id == sala.Id)
                                {

                                    //PrikaziSalu.Sale[i].Status = status.Zauzeta;
                                }
                            }
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ne postoji nijedna slobodna sala", "Greška", MessageBoxButton.OK);
                    }*/
                }
                TerminMenadzer.ZakaziTermin(s);
                this.Close();

            } catch(System.Exception)
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

        private void vpp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            slobodneSale = new List<Sala>();
            string tip = combo.SelectedValue.ToString().Split(' ')[1];
            foreach(Sala s in SaleMenadzer.sale)
            {
                if(tip.Equals("Opeacija"))
                {
                    if(s.TipSale.Equals(tipSale.OperacionaSala) && s.Status.Equals(status.Slobodna))
                    {
                        slobodneSale.Add(s);
                        MessageBox.Show(s.Id.ToString());
                    }
                } else
                {
                    if (s.TipSale.Equals(tipSale.SalaZaPregled) && s.Status.Equals(status.Slobodna))
                    {
                        slobodneSale.Add(s);
                        MessageBox.Show(s.Id.ToString());
                    }
                }
            }


        }

        private void preferenca_Click(object sender, RoutedEventArgs e)
        {
            // prozor za odabir lekara po preferenci
            ZakaziTerminPreferenca ztp = new ZakaziTerminPreferenca();
            ztp.Show();
        }
    }
}
