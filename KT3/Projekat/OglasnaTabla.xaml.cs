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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziPacijenta p = new PrikaziPacijenta();
            p.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziTerminSekretar p = new PrikaziTerminSekretar();
            p.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Sekretar s = new Sekretar();
            s.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ObavestenjaMenadzer.sacuvajIzmene();
        }

        // X na detaljnom uvid u selektovano obavestenje
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            canvas2.Visibility = Visibility.Hidden;
        }

        private void obavestenja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flag == false)
            {
                canvas2.Visibility = Visibility.Visible;
            }

            Obavestenja selektovanoObavestenje = (Obavestenja)listView.SelectedItem;

            if (selektovanoObavestenje != null)
            {
                naslov.Text = selektovanoObavestenje.TipObavestenja;
                datum.Text = selektovanoObavestenje.Datum;
                sadrzaj.Text = selektovanoObavestenje.SadrzajObavestenja;

                if (selektovanoObavestenje.Oznaka.Equals("svi"))
                {
                    namena.Text = "sve";
                }
                else if (selektovanoObavestenje.Oznaka.Equals("lekari"))
                {
                    namena.Text = "lekare";
                }
                else if (selektovanoObavestenje.Oznaka.Equals("upravnici"))
                {
                    namena.Text = "upravnike";
                }
                else if (selektovanoObavestenje.Oznaka.Equals("pacijenti"))
                {
                    namena.Text = "sve pacijente";
                }
                else if (selektovanoObavestenje.Oznaka.Equals("specificni pacijenti"))
                {
                    namena.Text = "";
                    foreach (int id in selektovanoObavestenje.ListaIdPacijenata)
                    {
                        Pacijent pacijent = PacijentiMenadzer.PronadjiPoId(id);
                        namena.Text += pacijent.ImePacijenta + " " + pacijent.PrezimePacijenta + " \n";
                    }
                }
            }
        }

        // dodaj
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DodajObavestenje dodavanje = new DodajObavestenje();
            dodavanje.Show();
        }

        // izmeni
        private void Button_Click_2(object sender, RoutedEventArgs e)
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

        // obrisi
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            flag = true;
            canvas2.Visibility = Visibility.Hidden;

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
                Button_Click_4(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.RightCtrl))
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
        }
    }
}
