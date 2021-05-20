using Projekat.Model;
using Projekat.Servis;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for Sastojci.xaml
    /// </summary>
    public partial class Sastojci : Window
    {
        public Lek izabraniLijek;
        //private int colNum = 0;

        public static ObservableCollection<Sastojak> SastojciLijeka{get; set;}
        
        public Sastojci(Lek izabraniLijek)
        {
            InitializeComponent();
            //inicijalizujElemente(izabraniLijek);
            //postaviTekst();
            //dodajSastojke();
        }

        /*private void inicijalizujElemente(Lek izabraniLijek)
        {
            this.izabraniLijek = izabraniLijek;
            //this.DataContext = this;
        }*/

       /* private void dodajSastojke()
        {
            SastojciLijeka = new ObservableCollection<Sastojak>();
            foreach(Lek lijek in LekoviMenadzer.lijekovi)
            {
                if(lijek.idLeka == izabraniLijek.idLeka)
                {
                    dodajSastojakLijeka(lijek);
                }
            }
        }

        private void dodajSastojakLijeka(Lek lijek)
        {
            foreach (Sastojak sastojak in lijek.sastojci)
            {
                SastojciLijeka.Add(sastojak);
            }
        }
       */
        /*private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }*/

       /* private void postaviTekst()
        {
            this.tekst.Text = "Sastojci za lijek: " + izabraniLijek.nazivLeka;
        }*/

       /* private void DodajSastojak_Click(object sender, RoutedEventArgs e)
        {
            DodajSastojak dodajSastojak = new DodajSastojak(izabraniLijek);
            dodajSastojak.Show();
        }*/

        /*private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }*/

        /*private void IzmjeniSastojak_Click(object sender, RoutedEventArgs e)
        {
            Sastojak izabraniSastojak = (Sastojak)dataGridSastojci.SelectedItem;
            if(izabraniSastojak != null)
            {
                IzmjeniSastojak izmjeniSastojak = new IzmjeniSastojak(izabraniSastojak, izabraniLijek);
                izmjeniSastojak.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati sastojak!");
            }
        }*/

        /*private void ObrisiSastojak_Click(object sender, RoutedEventArgs e)
        {
            Sastojak izabraniSastojak = (Sastojak)dataGridSastojci.SelectedItem;
            if (izabraniSastojak != null)
            {
                LekoviServis.obrisiSastojakLijeka(izabraniLijek, izabraniSastojak);
            }
            else
            {
                MessageBox.Show("Morate izabrati sastojak!");
            }
        }*/

        /*private void Pretraga_Click(object sender, TextChangedEventArgs e)
        {
            SastojciLijeka.Clear();
            if(izabraniLijek.sastojci != null)
            {
                foreach(Sastojak sastojak in izabraniLijek.sastojci)
                {
                    if (sastojak.naziv.StartsWith(this.Pretraga.Text))
                    {
                        SastojciLijeka.Add(sastojak);
                    }
                }
            }
        }*/

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.P)
                {
                    //this.Pretraga.Focus();
                }else if(e.Key == Key.N || e.Key == Key.Z)
                {
                    //Odustani_Click(sender, e);
                }
                else if (e.Key == Key.D)
                {
                    //DodajSastojak_Click(sender, e);
                }
                else if (e.Key == Key.I)
                {
                    //IzmjeniSastojak_Click(sender, e);
                }
                else if (e.Key == Key.O)
                {
                    //ObrisiSastojak_Click(sender, e);
                }
            }
        }

    }
}
