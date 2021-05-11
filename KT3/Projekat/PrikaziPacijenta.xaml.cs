using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikaziPacijenta.xaml
    /// </summary>
    public partial class PrikaziPacijenta : Window
    {
        private bool flag = false;
        public static ObservableCollection<Pacijent> PacijentiTabela
        {
            get;
            set;
        }
        public PrikaziPacijenta()
        {
            InitializeComponent();
            this.DataContext = this;
            PacijentiTabela = new ObservableCollection<Pacijent>();

            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                PacijentiTabela.Add(p);
            }

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(PacijentiTabela);
            view.Filter = UserFilterPacijenti;
        }

        private bool UserFilterPacijenti(object item)
        {
            if (String.IsNullOrEmpty(pretraga.Text))
            {
                return true;
            }
            else
            {
                return ((item as Pacijent).ImePacijenta.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Pacijent).PrezimePacijenta.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                       || ((item as Pacijent).Jmbg.ToString().IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        // nazad
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PacijentiMenadzer.SacuvajIzmenePacijenta();
            SaleMenadzer.sacuvajIzmjene();
            this.Close();
            Sekretar s = new Sekretar();
            s.Show();
        }

        // dodavanje
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DodajPacijenta dodavanje = new DodajPacijenta();
            dodavanje.Show();
        }

        // izmena
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Pacijent zaIzmenu = (Pacijent)TabelaPacijenata.SelectedItem;

            if (zaIzmenu != null)
            {
                IzmeniPacijenta izmena = new IzmeniPacijenta(zaIzmenu);
                izmena.Show();
            }
            else 
            {
                MessageBox.Show("Niste selektovali pacijenta kojeg zelite da izmenite!");
            }
        }
        
        // brisanje
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            flag = true;
            Pacijent zaBrisanje = (Pacijent)TabelaPacijenata.SelectedItem;
            canvas2.Visibility = Visibility.Hidden;

            if (zaBrisanje != null)
            {
                ObrisiNalogPacijenta brisanje = new ObrisiNalogPacijenta(zaBrisanje);
                brisanje.Show();
            }
            else 
            {
                MessageBox.Show("Niste selektovali pacijenta kojeg zelite da obrisete!");
            }
            flag = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PacijentiMenadzer.SacuvajIzmenePacijenta();
            SaleMenadzer.sacuvajIzmjene();
        }

        // otvaranje zdravstvenog kartona pacijenta (uvid u zdravstveni karton)
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Pacijent p = (Pacijent)TabelaPacijenata.SelectedItem;

            if (p != null)
            {
                if (p.StatusNaloga.Equals(statusNaloga.Guest))
                {
                    MessageBox.Show("Guest nalozi nemaju zdravstveni karton.");
                }
                else
                {
                    UvidZdravstveniKarton karton = new UvidZdravstveniKarton(p);
                    karton.Show();
                }
            }
            else
            {
                MessageBox.Show("Niste selektovali pacijenta ciji karton zelite da vidite!");
            }
        }

        // X na prikazu naloga pacijenta
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            canvas2.Visibility = Visibility.Hidden;
        }

        private void TabelaPacijenata_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flag == false)
            {
                canvas2.Visibility = Visibility.Visible;
            }

            Pacijent p = (Pacijent)TabelaPacijenata.SelectedItem;

            if (p != null)
            {
                ime.Text = p.ImePacijenta;
                prezime.Text = p.PrezimePacijenta;
                jmbg.Text = p.Jmbg.ToString();
                pol.Text = p.Pol.ToString();
                status.Text = p.StatusNaloga.ToString();
                telefon.Text = p.BrojTelefona.ToString();
                email.Text = p.Email;
                adresa.Text = p.AdresaStanovanja;
                stanje.Text = p.BracnoStanje.ToString();
                zanimanje.Text = p.Zanimanje;
                
                if (p.Maloletnik == true)
                {
                    maloletnik.IsChecked = true;
                    jmbgStaratelj.Text = p.JmbgStaratelja.ToString();
                }
                else
                {
                    maloletnik.IsChecked = false;
                    jmbgStaratelj.Text = "";
                }
            }
        }

        // button termini
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            PacijentiMenadzer.SacuvajIzmenePacijenta();
            SaleMenadzer.sacuvajIzmjene();

            this.Close();
            PrikaziTerminSekretar p = new PrikaziTerminSekretar();
            p.Show();
        }

        // oglasna tabla
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            this.Close();
            OglasnaTabla o = new OglasnaTabla();
            o.Show();
        }

        private void pretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(PacijentiTabela).Refresh();
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            PrikaziPacijentaPomoc pomoc = new PrikaziPacijentaPomoc();
            pomoc.Show();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // zakazi
            if (e.Key == Key.D && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_1(sender, e);
            }
            else if (e.Key == Key.D && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Button_Click_1(sender, e);
            }
            // izmeni
            else if (e.Key == Key.I && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_2(sender, e);
            }
            else if (e.Key == Key.I && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Button_Click_2(sender, e);
            }
            // otkazi
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_3(sender, e);
            }
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Button_Click_3(sender, e);
            }
            // X na detaljnom prikazu termina
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_5(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Button_Click_5(sender, e);
            }
            // uvid u zdravstveni karton
            else if (e.Key == Key.U && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_4(sender, e);
            }
            else if (e.Key == Key.U && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Button_Click_4(sender, e);
            }
            // izadji iz ovog prozora
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Button_Click(sender, e);
            }
            // tabela termina
           
        }
    }
}
