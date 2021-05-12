using Projekat.Model;
using System.Windows;
using System.Windows.Input;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for Upravnik.xaml
    /// </summary>
    public partial class Upravnik : Window
    {
        public Upravnik()
        {
            InitializeComponent();
            dodajObavjestenja();
        }

        private void dodajObavjestenja()
        {
            foreach (Obavestenja o in ObavestenjaMenadzer.obavestenja)
            {
                if (o.Oznaka.Equals("svi") || o.Oznaka.Equals("upravnici"))
                {
                    obavestenjaUpravnik.Items.Add(o);
                }
            }
        }

        private void Prostorije_Click(object sender, RoutedEventArgs e)
        {
            PrikaziSalu w1 = new PrikaziSalu();
            this.Close();
            w1.ShowDialog();
        }

        private void Zahtjevi_Click(object sender, RoutedEventArgs e)
        {
            Zahtjevi w2 = new Zahtjevi();
            this.Close();
            w2.ShowDialog();
        }

        private void Odjava_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Obavjestenja_Click(object sender, RoutedEventArgs e)
        {
            Obavestenja izabranoObavjestenje = (Obavestenja)this.obavestenjaUpravnik.SelectedItem;
            if (izabranoObavjestenje != null)
            {
                PrikazObavjestenja prikazObavjestenja = new PrikazObavjestenja(izabranoObavjestenje);
                prikazObavjestenja.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati obavjestenje!");
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.O)
                {
                    Odjava_Click(sender, e);
                }
            }
        }

        private void Komunikacija_Click(object sender, RoutedEventArgs e)
        {
            Komunikacija komunikacija = new Komunikacija();
            komunikacija.Show();
            this.Close();
        }
    }
}
