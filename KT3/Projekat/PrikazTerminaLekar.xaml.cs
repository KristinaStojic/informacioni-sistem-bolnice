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

        public PrikazTerminaLekar()
        {
            InitializeComponent();
            this.DataContext = this;
            Termini = new ObservableCollection<Termin>();
            foreach (Termin t in TerminMenadzer.termini)
            {
                /*foreach(Lekar l in MainWindow.lekari)
                {
                    if(t.Lekar.IdLekara == l.IdLekara)
                    {
                        Termini.Add(t);
                    }
                }*/


                /*if (t.Lekar.IdLekara == 1)
                {
                    Termini.Add(t);
                }*/
                /*if (t.Lekar.IdLekara == 2)
                {
                    Termini.Add(t);
                }*/
                /*if (t.Lekar.IdLekara == 3)
                {
                    Termini.Add(t);
                } */
               /* if (t.Lekar.IdLekara == 4)
                {
                    Termini.Add(t);
                }*/
                if (t.Lekar.IdLekara == 5)
                {
                    Termini.Add(t);
                }

                //Termini.Add(t);
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
            ZakaziTerminLekar zt = new ZakaziTerminLekar();
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
            PacijentiMenadzer.SacuvajIzmenePacijenta();
            this.Close();
            PocetnaStrana ps = new PocetnaStrana();
            ps.Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            //obrisi
            Termin zaBrisanje = (Termin)dataGridTermini.SelectedItem;
            if (zaBrisanje != null)
            {

                TerminMenadzer.OtkaziTerminLekar(zaBrisanje);
            }
            else
            {
                MessageBox.Show("Niste selektovali termin koji zelite da otkazete!");
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
                    UvidZdravstveniKartonLekar karton = new UvidZdravstveniKartonLekar(izabraniTermin.Pacijent, izabraniTermin);
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
            PacijentiMenadzer.SacuvajIzmenePacijenta();
            SaleMenadzer.sacuvajIzmjene();
            PocetnaStrana ps = new PocetnaStrana();
            //ps.Show();   /*ISPRAVITI*/
        }
    }
}
