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
using Projekat.Model;
using Projekat.Pomoc;
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for OglasnaTabla.xaml
    /// </summary>
    public partial class OglasnaTabla : Window
    {
        private bool flag = false;

        public static ObservableCollection<Obavestenja> oglasnaTabla { get; set; }

        public OglasnaTabla()
        {
            InitializeComponent();
            oglasnaTabla = new ObservableCollection<Obavestenja>();
            listView.ItemsSource = oglasnaTabla;

            foreach (Obavestenja obavestenje in ObavestenjaMenadzer.obavestenja)
            { 
                if (obavestenje.Notifikacija == false)
                {
                    oglasnaTabla.Add(obavestenje);
                }
            }
        }

        private void Pacijenti_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziPacijenta p = new PrikaziPacijenta();
            p.Show();
        }

        private void Termini_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziTerminSekretar p = new PrikaziTerminSekretar();
            p.Show();
        }

        private void Lekari_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziLekare prikaz = new PrikaziLekare();
            prikaz.Show();
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Sekretar s = new Sekretar();
            s.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ObavestenjaServis.sacuvajIzmene();
        }

        private void Napusti_uvid_Click(object sender, RoutedEventArgs e)
        {
            canvas.Visibility = Visibility.Hidden;
            okvir.Visibility = Visibility.Hidden;
        }

        private void Obavestenja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flag == false)
            {
                canvas.Visibility = Visibility.Visible;
                okvir.Visibility = Visibility.Visible;
            }

            Obavestenja selektovanoObavestenje = (Obavestenja)listView.SelectedItem;
            if (selektovanoObavestenje != null)
            {
                naslov.Text = selektovanoObavestenje.TipObavestenja;
                datum.Text = selektovanoObavestenje.Datum;
                sadrzaj.Text = selektovanoObavestenje.SadrzajObavestenja;
                namena.Text = ObavestenjaServis.PopuniNamenuObavestenja(selektovanoObavestenje);
            }
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            DodajObavestenje dodavanje = new DodajObavestenje();
            dodavanje.Show();
        }

        private void Izmeni_Click(object sender, RoutedEventArgs e)
        {
            Obavestenja selektovanoObavestenje = (Obavestenja)listView.SelectedItem;

            if (selektovanoObavestenje != null)
            {
                IzmeniObavestenje izmena = new IzmeniObavestenje(selektovanoObavestenje);
                izmena.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali obavestenje koje zelite da izmenite!");
            }
        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
            flag = true;
            canvas.Visibility = Visibility.Hidden;

            Obavestenja selektovanoObavestenje = (Obavestenja)listView.SelectedItem;
            if (selektovanoObavestenje != null)
            {
                ObrisiObavestenjeSekretar brisanje = new ObrisiObavestenjeSekretar(selektovanoObavestenje);
                brisanje.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali obavestenje koje zelite da obrisete!");
            }

            flag = false;
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            OglasnaTablaPomoc pomoc = new OglasnaTablaPomoc();
            pomoc.Show();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // dodaj
            if (e.Key == Key.D && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Dodaj_Click(sender, e);
            }
            else if (e.Key == Key.D && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Dodaj_Click(sender, e);
            }
            // izmeni
            else if (e.Key == Key.I && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Izmeni_Click(sender, e);
            }
            else if (e.Key == Key.I && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Izmeni_Click(sender, e);
            }
            // otkazi
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Obrisi_Click(sender, e);
            }
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Obrisi_Click(sender, e);
            }
            // X na detaljnom prikazu termina
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Napusti_uvid_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Napusti_uvid_Click(sender, e);
            }
            // izadji iz ovog prozora
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Nazad_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Nazad_Click(sender, e);
            }
        }

    }
}
