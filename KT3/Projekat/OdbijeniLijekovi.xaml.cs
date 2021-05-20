using Projekat.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for OdbijeniLijekovi.xaml
    /// </summary>
    public partial class OdbijeniLijekovi : Window
    {
        /*private int colNum = 0;

        public static ObservableCollection<Lek> OdbijeniLekovi
        {
            get;
            set;
        }*/
        public OdbijeniLijekovi()
        {
            InitializeComponent();
            //inicijalizujElemente();
            //inicijalizujLijekove();
        }

        /*private void inicijalizujElemente()
        {
            OdbijeniLekovi = new ObservableCollection<Lek>();
            this.DataContext = this;
        }

        private void inicijalizujLijekove()
        {
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (!zahtjev.odobrenZahtev && zahtjev.obradjenZahtev)
                {
                    OdbijeniLekovi.Add(zahtjev.lek);
                }
            }
        }*/

       /* private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }
       */
        /*private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }*/

        /*private void Obrazlozenje_Click(object sender, RoutedEventArgs e)
        {
            Lek izabraniLijek = (Lek)this.dataGridOdbijeniLijekovi.SelectedItem;
            if (izabraniLijek != null)
            {
                Obrazlozenje obrazlozenje = new Obrazlozenje(izabraniLijek);
                obrazlozenje.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }*/

        /*private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.N)
                {
                    //Odustani_Click(sender, e);

                }else if(e.Key == Key.P)
                {
                    Potvrdi_Click(sender, e);
                }else if(e.Key == Key.I)
                {
                    //Izmjeni_Click(sender, e);
                }else if(e.Key == Key.O)
                {
                    //Obrisi_Click(sender, e);
                }
            }
        }*/

        /*private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            Lek izabraniLijek = (Lek)dataGridOdbijeniLijekovi.SelectedItem;
            if(izabraniLijek != null)
            {
                PotvrdaSlanjaZahtjeva potvrdaSlanja = new PotvrdaSlanjaZahtjeva(izabraniLijek);
                potvrdaSlanja.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }*/

        /*public static void azurirajPrikaz()
        {
            OdbijeniLekovi.Clear();
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (!zahtjev.odobrenZahtev && zahtjev.obradjenZahtev)
                {
                    OdbijeniLekovi.Add(zahtjev.lek);
                }
            }
        }*/

       /* private void Izmjeni_Click(object sender, RoutedEventArgs e)
        {
            Lek izabraniLijek = (Lek)dataGridOdbijeniLijekovi.SelectedItem;
            if(izabraniLijek != null)
            {
                IzmjeniOdbijeniLijek izmjeniOdbijeniLijek = new IzmjeniOdbijeniLijek(izabraniLijek);
                izmjeniOdbijeniLijek.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek");
            }
        }*/

        /*private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
            Lek izabraniLijek = (Lek)dataGridOdbijeniLijekovi.SelectedItem;
            if(izabraniLijek != null)
            {
                BrisanjeOdbijenogLijeka brisanjeLijeka = new BrisanjeOdbijenogLijeka(izabraniLijek);
                brisanjeLijeka.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }*/

       /* private void Pretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            OdbijeniLekovi.Clear();
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (!zahtjev.odobrenZahtev && zahtjev.obradjenZahtev && zahtjev.lek.nazivLeka.StartsWith(this.Pretraga.Text))
                {
                    OdbijeniLekovi.Add(zahtjev.lek);
                }
            }
        }*/
    }
}
