using System.Windows;
using System.Windows.Input;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for Komunikacija.xaml
    /// </summary>
    public partial class Komunikacija : Window
    {
        public Komunikacija()
        {
            InitializeComponent();
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            Upravnik upravnik = new Upravnik();
            upravnik.Show();
            this.Close();
        }

        private void Button_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.N)
                {
                    Nazad_Click(sender, e);
                }
                else if(e.Key == Key.T)
                {
                    Prostorije_Click(sender, e);
                }
                else if (e.Key == Key.E)
                {
                    Zahtjevi_Click(sender, e);
                }
            }
        }

        private void UvidUZahtjev_Click(object sender, RoutedEventArgs e)
        {
            ZahtjeviZaKomunikaciju zahtjeviZaKomunikaciju = new ZahtjeviZaKomunikaciju();
            zahtjeviZaKomunikaciju.Show();
        }

        private void Osoblje_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Zahtjevi_Click(object sender, RoutedEventArgs e)
        {
            Zahtjevi zahtjevi = new Zahtjevi();
            zahtjevi.Show();
            this.Close();
        }

        private void Prostorije_Click(object sender, RoutedEventArgs e)
        {
            PrikaziSalu prikaziSalu = new PrikaziSalu();
            prikaziSalu.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Upravnik upravnik = new Upravnik();
            upravnik.Show();
            this.Close();
        }
    }
}
