using System;
using System.Collections.Generic;
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
using Projekat.Model;
using Projekat.Pomoc;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for KomunikacijaSekretar.xaml
    /// </summary>
    public partial class KomunikacijaSekretar : Window
    {
        public KomunikacijaSekretar()
        {
            InitializeComponent();

            List<Obavestenja> komunikacija = new List<Obavestenja>();
            string datum = DateTime.Now.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            Obavestenja mejl1 = new Obavestenja("Mejl za Kliniku u Beogradu", datum, "Da li postoje slobodni lezaji za transfer pacijenata?");
            Obavestenja mejl2 = new Obavestenja("Za Kliniku u Sarajevu", datum, "Odobren je transfer pacijenata u nasu kliniku u Novom Sadu.");
            Obavestenja mejl3 = new Obavestenja("Za Kliniku u Beogradu", datum, "Transfer pacijenata u nasu kliniku u Novom Sadu je odbijen, jer je kapacitet popunjen.");

            komunikacija.Add(mejl1);
            komunikacija.Add(mejl2);
            komunikacija.Add(mejl3);

            listView.ItemsSource = komunikacija;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Button_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Nazad_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Nazad_Click(sender, e);
            }
        }

        private void Pacijenti_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziPacijenta p = new PrikaziPacijenta();
            p.Show();
        }

        private void Lekari_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziLekare prikaz = new PrikaziLekare();
            prikaz.Show();
        }

        private void Termini_Click(object sender, RoutedEventArgs e)
        {
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

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Sekretar s = new Sekretar();
            s.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            canvas.Visibility = Visibility.Hidden;
            okvir.Visibility = Visibility.Hidden;
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            KomunikacijaSekretarPomoc k = new KomunikacijaSekretarPomoc();
            k.Show();
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            canvas.Visibility = Visibility.Visible;
            okvir.Visibility = Visibility.Visible;

            Obavestenja selektovanoObavestenje = (Obavestenja)listView.SelectedItem;
            if (selektovanoObavestenje != null)
            {
                naslov.Text = selektovanoObavestenje.TipObavestenja;
                datum.Text = selektovanoObavestenje.Datum;
                sadrzaj.Text = selektovanoObavestenje.SadrzajObavestenja;
            }
        }

    }
}
