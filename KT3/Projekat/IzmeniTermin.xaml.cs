using Model;
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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmeniTermin.xaml
    /// </summary>
    public partial class IzmeniTermin : Window
    {
        public Termin termin;
        private Lekar idLek;
        private string stariDatum;
        public IzmeniTermin(Termin izabraniTermin)
        {
            InitializeComponent();
            //this.DataContext = this;
            datum.BlackoutDates.AddDatesInPast();
            CalendarDateRange cdr = new CalendarDateRange();
            cdr.Start = DateTime.Now.AddDays(3);
            cdr.End = DateTime.Now.AddDays(2000);
            datum.BlackoutDates.Add(cdr);
            //this.datum.SelectedDate = DateTime.Parse(izabraniTermin.Datum);

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
                this.dgSearch.SelectedValue = izabraniTermin.Lekar.IdLekara;
                idLek = izabraniTermin.Lekar;
                stariDatum = izabraniTermin.Datum;
                this.vpp.Text = izabraniTermin.VremePocetka;
                this.dgSearch.ItemsSource = MainWindow.lekari;

                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dgSearch.ItemsSource);
                view.Filter = UserFilter;
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //dugme odustani
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //dugme sacuvaj
           try
           {
            //int brojTermina = TerminMenadzer.GenerisanjeIdTermina();
            String formatted = null;
            DateTime? selectedDate = datum.SelectedDate;
            if (selectedDate.HasValue)
            {
                formatted = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            } else
            {
                formatted = stariDatum;
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
            Termin t = new Termin(termin.IdTermin, formatted, vp, vk, tp);
            // TODO: promeniti ovo na id pacijenta koji je prijavljen
            foreach (Pacijent p in PacijentiMenadzer.PronadjiSve())
            {
                if (p.IdPacijenta == 1)
                {
                    t.Pacijent = p;
                }
            }
            foreach (Sala sala in SaleMenadzer.NadjiSveSale())
            {
                try
                {
                    if (sala.Status == status.Slobodna)
                    {
                        t.Prostorija = sala;  // kad naidje na prvu slobodnu
                        //sala.Status = status.Zauzeta;
                        break;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Ne postoji nijedna slobodna sala", "Zauzete sale");
                }
            }

            if (dgSearch.SelectedItems.Count > 0)
            {
                Lekar selLekar = (Lekar)dgSearch.SelectedItem;
                t.Lekar = selLekar;
            } else
            {
                // TODO: optimizovati!
                t.Lekar = idLek; 
            }
            TerminMenadzer.IzmeniTermin(termin, t);
            this.Close();
         } catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void vpp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lvWithSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void vpp_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

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

    }
}
