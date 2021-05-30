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
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikaziPacijenta.xaml
    /// </summary>
    public partial class PrikaziPacijenta : Window
    {
        private bool flag = false;
        public bool zatvoreno = false;
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

            List<Pacijent> pacijenti = PacijentiServis.PronadjiSve();
            foreach (Pacijent p in pacijenti)
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

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            PacijentiServis.SacuvajIzmenePacijenta();
            SaleServis.sacuvajIzmjene();
            this.Close();
            Sekretar s = new Sekretar();
            s.Show();
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            DodajPacijenta dodavanje = new DodajPacijenta();
            dodavanje.Show();
        }

        private void Izmeni_Click(object sender, RoutedEventArgs e)
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
        
        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
            //flag = true;
            Pacijent zaBrisanje = (Pacijent)TabelaPacijenata.SelectedItem;
            informacijePacijenta.Visibility = Visibility.Hidden;

            if (zaBrisanje != null)
            {
                ObrisiNalogPacijenta brisanje = new ObrisiNalogPacijenta(zaBrisanje, this);
                brisanje.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali pacijenta kojeg zelite da obrisete!");
            }
            
            if (PacijentiTabela.Count != 0 && zatvoreno == true)
            {
                TabelaPacijenata.SelectedIndex = 0;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PacijentiServis.SacuvajIzmenePacijenta();
            SaleServis.sacuvajIzmjene();
        }

        private void Zdravstveni_karton_Click(object sender, RoutedEventArgs e)
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

        private void Napusti_uvid_Click(object sender, RoutedEventArgs e)
        {
            informacijePacijenta.Visibility = Visibility.Hidden;
        }

        private void TabelaPacijenata_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (flag == false)
           // {
           //     informacijePacijenta.Visibility = Visibility.Visible;
           // }
            flag = false;
            
            Pacijent p = (Pacijent)TabelaPacijenata.SelectedItem;
            informacijePacijenta.Visibility = Visibility.Visible;
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

        private void Termini_Click(object sender, RoutedEventArgs e)
        {
            PacijentiServis.SacuvajIzmenePacijenta();
            SaleServis.sacuvajIzmjene();

            this.Close();
            PrikaziTerminSekretar p = new PrikaziTerminSekretar();
            p.Show();
        }

        private void Oglasna_tabla_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OglasnaTabla o = new OglasnaTabla();
            o.Show();
        }

        private void Lekari_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziLekare prikaz = new PrikaziLekare();
            prikaz.Show();
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            PrikaziPacijentaPomoc pomoc = new PrikaziPacijentaPomoc();
            pomoc.Show();
        }

        private void Pretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(PacijentiTabela).Refresh();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Dodaj_Click(sender, e);
            }
            else if (e.Key == Key.D && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Dodaj_Click(sender, e);
            }
            else if (e.Key == Key.I && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Izmeni_Click(sender, e);
            }
            else if (e.Key == Key.I && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Izmeni_Click(sender, e);
            }
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Obrisi_Click(sender, e);
            }
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Obrisi_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Napusti_uvid_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Napusti_uvid_Click(sender, e);
            }
            else if (e.Key == Key.U && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Zdravstveni_karton_Click(sender, e);
            }
            else if (e.Key == Key.U && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Zdravstveni_karton_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Nazad_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Nazad_Click(sender, e);
            }
            else if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                //this.MoveFocus();
            }
            else if (e.Key == Key.S && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                pretraga.Focusable = true;
            }
            else if (e.Key == Key.P && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Pomoc_Click(sender, e);
            }
            else if (e.Key == Key.P && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Pomoc_Click(sender, e);
            }

        }

    }
}
