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
using Projekat.Pomoc;
using LiveCharts;
using LiveCharts.Wpf;
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikazTerminaLekar.xaml
    /// </summary>
    public partial class PrikazTerminaLekar : Window
    {
        private int colNum = 0;
        int idLekara;
        public static ObservableCollection<Termin> Termini
        {
            get;
            set;
        }


        public ChartValues<int> ukupnoPregleda
        {
            get;set;
        }

        public ChartValues<int> ukupnoOperacija
        {
            get;set;
        }

        public Func<ChartPoint, string> LabelPoint { get; set; }
        public PrikazTerminaLekar(int id)
        {
            InitializeComponent();
            LabelPoint = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            this.idLekara = id;
            this.DataContext = this;
            NadjiUlogovanogLekara();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Termini);
            view.Filter = UserFilter;
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(datumFilter.Text))
            {
                return true;
            }
            else
            {
                return ((item as Termin).Datum.IndexOf(datumFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void NadjiUlogovanogLekara()
        {
            Termini = new ObservableCollection<Termin>();
            foreach (Termin t in TerminMenadzer.termini)
            {


                if (t.Lekar.IdLekara == idLekara) //Petar Nebojsic
                {
                    Termini.Add(t);
                    int brPregleda = t.Lekar.BrojPregleda; 
                    int brOperacija = t.Lekar.BrojOperacija; 
                    //int br = 10;
                    this.ukupnoPregleda = new ChartValues<int>() { brPregleda };
                    this.ukupnoOperacija = new ChartValues<int>() { brOperacija };
                }
                /*if (t.Lekar.IdLekara == 2) //Milos Dragojevic
                {
                    Termini.Add(t);
                }*/
                /*if (t.Lekar.IdLekara == 3) //Petar Milosevic
                {
                    Termini.Add(t);
                } */
                /*if (t.Lekar.IdLekara == 4) //Dejan Milosevic
                {
                    Termini.Add(t);
                }*/
                /*if (t.Lekar.IdLekara == 5) //Isidora Isidorovic
                {
                    Termini.Add(t);
                }*/


            }



        }
        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 9)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //zakazi

            ZakaziTerminLekar2 zt = new ZakaziTerminLekar2(idLekara);
            zt.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //izmjena
            Termin izabraniTermin = (Termin)dataGridTermini.SelectedItem;
            if (izabraniTermin != null)
            {
                IzmeniTerminLekara it = new IzmeniTerminLekara(izabraniTermin);
                it.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali termin koji zelite da izmenite!");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // nazad

            TerminMenadzer.sacuvajIzmene();
            PacijentiServis.SacuvajIzmenePacijenta();
            this.Close();
            //PocetnaStrana ps = new PocetnaStrana();
            //ps.Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            //obrisi
            Termin izabraniTermin = (Termin)dataGridTermini.SelectedItem;
            if (izabraniTermin == null)
            {
                MessageBox.Show("Niste selektovali termin koji zelite da obrišete!");
            }
            else
            {
                ObrisiTerminLekar ot = new ObrisiTerminLekar(izabraniTermin);
                ot.Show();
            }
               
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Termin izabraniTermin = (Termin)dataGridTermini.SelectedItem;
           
            if (izabraniTermin != null)
            {
                if (izabraniTermin.Pacijent.StatusNaloga.Equals(statusNaloga.Guest))
                {
                    MessageBox.Show("Pacijenti koji imaju guest nalog nemaju zdravstveni karton.");
                }
                else
                {
                    this.Close();
                    //UvidZdravstveniKartonLekar karton = new UvidZdravstveniKartonLekar(izabraniTermin.Pacijent, izabraniTermin);
                    ZdravstveniKartonLekar karton = new ZdravstveniKartonLekar(izabraniTermin.Pacijent, izabraniTermin);
                    karton.Show();
                }
            }
            else
            {
                MessageBox.Show("Niste selektovali pacijenta ciji karton zelite da vidite!");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TerminMenadzer.sacuvajIzmene();
            PacijentiServis.SacuvajIzmenePacijenta();
            SaleServis.sacuvajIzmjene();
            //PocetnaStrana ps = new PocetnaStrana();
            //ps.Show();   /*ISPRAVITI*/
        }

        // obavestenja lekara
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ObavestenjaLekar o = new ObavestenjaLekar();
            o.Show();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_1(sender, e);
            }
            else if (e.Key == Key.I && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_2(sender, e);
            }
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_5(sender, e);
            }
            else if (e.Key == Key.K && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_6(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_4(sender, e);
            }
            else if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Hyperlink_Click(sender, e);
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            PrikazTerminaLekarPomoc pomoc = new PrikazTerminaLekarPomoc();
            pomoc.Show();
        }

        private void datumFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dataGridTermini.ItemsSource).Refresh();
        }
    }
}
