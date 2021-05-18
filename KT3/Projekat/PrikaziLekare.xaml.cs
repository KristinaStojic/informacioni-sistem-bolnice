using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Model;
using Projekat.Pomoc;
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikaziLekare.xaml
    /// </summary>
    public partial class PrikaziLekare : Window
    {
        private bool flag = false;

        public static ObservableCollection<Lekar> Lekari
        {
            get;
            set;
        }

        public PrikaziLekare()
        {
            InitializeComponent();
            this.DataContext = this;
            Lekari = new ObservableCollection<Lekar>();

            foreach (Lekar l in LekariMenadzer.lekari)
            {
                Lekari.Add(l);
            }

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Lekari);
            view.Filter = UserFilterLekari;
        }

        private bool UserFilterLekari(object item)
        {
            if (String.IsNullOrEmpty(pretraga.Text))
            {
                return true;
            }
            else
            {
                return ((item as Lekar).ImeLek.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Lekar).PrezimeLek.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                       || ((item as Lekar).Jmbg.ToString().IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                          || ((item as Lekar).specijalizacija.ToString().IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            DodajLekara lekar = new DodajLekara();
            lekar.Show();
        }

        private void Izmeni_Click(object sender, RoutedEventArgs e)
        {
            Lekar zaIzmenu = (Lekar)TabelaLekara.SelectedItem;

            if (zaIzmenu != null)
            {
                IzmeniLekara izmena = new IzmeniLekara(zaIzmenu);
                izmena.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali lekara kojeg zelite da izmenite!");
            }
        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
            flag = true;
            Lekar zaBrisanje = (Lekar)TabelaLekara.SelectedItem;
            canvas2.Visibility = Visibility.Hidden;

            if (zaBrisanje != null)
            {
                ObrisiLekara brisanje = new ObrisiLekara(zaBrisanje);
                brisanje.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali lekara kojeg zelite da obrisete!");
            }
            flag = false;
        }

        private void Pacijenti_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziPacijenta prikazPacijenata = new PrikaziPacijenta();
            prikazPacijenata.Show();
        }

        private void Termini_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziTerminSekretar prikazTermina = new PrikaziTerminSekretar();
            prikazTermina.Show();
        }

        private void Oglasna_tabla_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OglasnaTabla oglasnaTabla = new OglasnaTabla();
            oglasnaTabla.Show();
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            PrikaziLekarePomoc pomoc = new PrikaziLekarePomoc();
            pomoc.Show();
        }

        private void Napusti_uvid_Click(object sender, RoutedEventArgs e)
        {
            canvas2.Visibility = Visibility.Hidden;
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            LekariServis.SacuvajIzmeneLekara();
            this.Close();
            Sekretar pocetnaStrana = new Sekretar();
            pocetnaStrana.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // sacuvati sve
            LekariServis.SacuvajIzmeneLekara();
        }

        private void TabelaLekara_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flag == false)
            {
                canvas2.Visibility = Visibility.Visible;
            }

            Lekar selektovaniLekar = (Lekar)TabelaLekara.SelectedItem;

            if (selektovaniLekar != null)
            {
                ime.Text = selektovaniLekar.ImeLek;
                prezime.Text = selektovaniLekar.PrezimeLek;
                jmbg.Text = selektovaniLekar.Jmbg.ToString();
                telefon.Text = selektovaniLekar.BrojTelefona.ToString();
                email.Text = selektovaniLekar.Email;
                adresa.Text = selektovaniLekar.AdresaStanovanja;
                specijalizacija.Text = selektovaniLekar.specijalizacija.ToString();
            }
        }

        private void Pretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(Lekari).Refresh();
        }

        private void Radno_vreme_Click(object sender, RoutedEventArgs e)
        {
            Lekar selektovaniLekar = (Lekar)TabelaLekara.SelectedItem;

            if (selektovaniLekar == null)
            {
                MessageBox.Show("Izaberite lekara cije radno vreme zelite da odredite.");
            }
            else
            {
                OdrediRadnoVreme radnoVreme = new OdrediRadnoVreme(selektovaniLekar);
                radnoVreme.Show();
            }        
        }

        private void Godisnji_odmor_Click(object sender, RoutedEventArgs e)
        {
            OdobravanjeGodisnjegOdmora odobravanje = new OdobravanjeGodisnjegOdmora();
            odobravanje.Show();
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
            else if (e.Key == Key.Z && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Godisnji_odmor_Click(sender, e);
            }
            else if (e.Key == Key.Z && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Godisnji_odmor_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Napusti_uvid_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Napusti_uvid_Click(sender, e);
            }
            else if (e.Key == Key.R && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Radno_vreme_Click(sender, e);
            }
            else if (e.Key == Key.R && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Radno_vreme_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Nazad_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Nazad_Click(sender, e);
            }
            // TODO: tabela termina 
            else if (e.Key == Key.T && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
            }
            else if (e.Key == Key.T && Keyboard.IsKeyDown(Key.RightCtrl))
            {
            }
            // TODO: polje za pretragu
        }
    }
}
