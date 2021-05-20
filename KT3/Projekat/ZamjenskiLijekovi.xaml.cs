using Projekat.Model;
using Projekat.Servis;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for ZamjenskiLijekovi.xaml
    /// </summary>
    public partial class ZamjenskiLijekovi : Window
    {
        public Lek izabraniLijek;
        //private int colNum = 0;
        //public static ObservableCollection<Lek> ZamjenskiLekovi {get; set;}
        
        public ZamjenskiLijekovi()
        {
            InitializeComponent();
            //inicijalizujElemente(izabraniLijek);
            //postaviTekst();
            //dodajLijekove();
        }

        /*private void inicijalizujElemente(Lek izabraniLijek)
        {
            this.izabraniLijek = izabraniLijek;
            //this.DataContext = this;
        }*/

        /*private void postaviTekst()
        {
            this.tekst.Text = "Zamjenski lijekovi za lijek: " + izabraniLijek.nazivLeka;
        }*/

        /*private void dodajLijekove()
        {
            ZamjenskiLekovi = new ObservableCollection<Lek>();
            foreach(Lek lijek in LekoviMenadzer.lijekovi)
            {
                if(izabraniLijek.idLeka == lijek.idLeka)
                {
                    dodajZamjenskiLijek(lijek);
                }
            }
        }

        private void dodajZamjenskiLijek(Lek lijek)
        {
            if (lijek.zamenskiLekovi != null)
            {
                dodajZamjenski(lijek);
            }
        }

        private void dodajZamjenski(Lek lijek)
        {
            foreach (int zamjenskiLijek in lijek.zamenskiLekovi)
            {
                foreach (Lek zamjenski in LekoviMenadzer.lijekovi)
                {
                    if (zamjenski.idLeka == zamjenskiLijek)
                    {
                        ZamjenskiLekovi.Add(zamjenski);
                    }
                }
            }
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }*/
       
        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            DodajZamjenskiLijek dodajZamjenskiLijek = new DodajZamjenskiLijek(izabraniLijek);
            dodajZamjenskiLijek.Show();
        }

        /*private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }*/

        /*private void Izmjeni_Click(object sender, RoutedEventArgs e)
        {
            Lek izabraniLijek = (Lek)dataGridLijekovi.SelectedItem;
            if(izabraniLijek != null)
            {
                IzmjeniLijek izmjeniLijek = new IzmjeniLijek();
                izmjeniLijek.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }
        */
        /*private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
            Lek zamjenskiLijek = (Lek)dataGridLijekovi.SelectedItem;
            if(zamjenskiLijek != null)
            {
                LekoviServis.obrisiZamjenski(izabraniLijek, zamjenskiLijek);
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
                if (e.Key == Key.D)
                {
                    Dodaj_Click(sender, e);
                }else if(e.Key == Key.D || e.Key == Key.N)
                {
                    //Odustani_Click(sender, e);
                }
                else if (e.Key == Key.I)
                {
                    //Izmjeni_Click(sender, e);
                }
                else if (e.Key == Key.O)
                {
                    //Obrisi_Click(sender, e);
                }
                else if (e.Key == Key.P)
                {
                    //this.Pretraga.Focus();
                }

            }
        }*/

       /* private void Pretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            ZamjenskiLekovi.Clear();
            if (izabraniLijek.zamenskiLekovi != null)
            {
                foreach (int zamjenskiLijek in izabraniLijek.zamenskiLekovi)
                {
                    pretraziZamjenskeLijekove(zamjenskiLijek);
                }
            }            
        }

        private void pretraziZamjenskeLijekove(int zamjenskiLijek)
        {
            foreach (Lek zamjenski in LekoviMenadzer.lijekovi)
            {
                if (zamjenski.idLeka == zamjenskiLijek && zamjenski.nazivLeka.StartsWith(this.Pretraga.Text))
                {
                    ZamjenskiLekovi.Add(zamjenski);
                }
            }
        }*/
    }
}
