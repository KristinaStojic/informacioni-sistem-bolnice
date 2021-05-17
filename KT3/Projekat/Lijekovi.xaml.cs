using Projekat.Model;
using Projekat.Pomoc;
using Projekat.Servis;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for Lijekovi.xaml
    /// </summary>
    public partial class Lijekovi : Window
    {

        private int colNum = 0;

        public static ObservableCollection<Lek> Lekovi
        {
            get;
            set;
        }

        public Lijekovi()
        {
            InitializeComponent();
            inicijalizujElemente();
            dodajLijekove();
        }

        private void inicijalizujElemente()
        {
            this.DataContext = this;
            Lekovi = new ObservableCollection<Lek>();
        }

        private void dodajLijekove()
        {
            foreach(Lek lijek in LekoviServis.Lijekovi())
            {
                Lekovi.Add(lijek);
            }
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void DodajLijek_Click(object sender, RoutedEventArgs e)
        {
            DodajLijek dodajLijek = new DodajLijek();
            dodajLijek.Show();
        }

        private void IzmjeniLijek_Click(object sender, RoutedEventArgs e)
        {
            Lek izabraniLijek = (Lek)dataGridLijekovi.SelectedItem;
            if(izabraniLijek != null)
            {
                IzmjeniLijek izmjeniLijek = new IzmjeniLijek(izabraniLijek);
                izmjeniLijek.ShowDialog();
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }

        private void Zahtjevi_Click(object sender, RoutedEventArgs e)
        {
            Zahtjevi zahtjevi = new Zahtjevi();
            this.Close();
            zahtjevi.Show();
        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
            Lek izabraniLijek = (Lek)dataGridLijekovi.SelectedItem;
            if(izabraniLijek != null)
            {
                BrisanjeLijeka brisanjeLijeka = new BrisanjeLijeka(izabraniLijek);
                brisanjeLijeka.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }

        private void ZamjenskiLijekovi_Click(object sender, RoutedEventArgs e)
        {
            Lek izabraniLijek = (Lek)dataGridLijekovi.SelectedItem;
            if (izabraniLijek != null)
            {
                ZamjenskiLijekovi zamjenskiLijekovi = new ZamjenskiLijekovi(izabraniLijek);
                zamjenskiLijekovi.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }

        private void Sastojci_Click(object sender, RoutedEventArgs e)
        {
            Lek izabraniLijek = (Lek)dataGridLijekovi.SelectedItem;
            if (izabraniLijek != null)
            {
                Sastojci sastojci = new Sastojci(izabraniLijek);
                sastojci.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.N || e.Key == Key.Z)
                {
                    Zahtjevi_Click(sender, e);
                }else if(e.Key == Key.D)
                {
                    DodajLijek_Click(sender, e);
                }
                else if (e.Key == Key.I)
                {
                    IzmjeniLijek_Click(sender, e);
                }
                else if (e.Key == Key.O)
                {
                    Obrisi_Click(sender, e);
                }
                else if (e.Key == Key.T)
                {
                    Sale_Click(sender, e);
                }else if(e.Key == Key.P)
                {
                    this.Pretraga.Focus();
                }else if(e.Key == Key.B)
                {
                    OdbijeniLijekovi_Click(sender, e);
                }
                else if (e.Key == Key.H)
                {
                    Pomoc_Click(sender, e);
                }
                else if (e.Key == Key.K)
                {
                    Komunikacija_Click(sender, e);
                }
            }
        }

        private void Sale_Click(object sender, RoutedEventArgs e)
        {
            PrikaziSalu ps = new PrikaziSalu();
            this.Hide();
            ps.Show();
        }

        private void Pretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            Lekovi.Clear();
            foreach (Lek lijek in LekoviMenadzer.lijekovi)
            {
                if (lijek.nazivLeka.StartsWith(this.Pretraga.Text))
                {
                    Lekovi.Add(lijek);
                }
            }
        }

        private void OdbijeniLijekovi_Click(object sender, RoutedEventArgs e)
        {
            OdbijeniLijekovi odbijeniLijekovi = new OdbijeniLijekovi();
            odbijeniLijekovi.Show();
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            LijekoviPomoc lijekoviPomoc = new LijekoviPomoc();
            lijekoviPomoc.Show();
        }

        private void Komunikacija_Click(object sender, RoutedEventArgs e)
        {
            Komunikacija komunikacija = new Komunikacija();
            komunikacija.Show();
            this.Close();
        }
    }
}
