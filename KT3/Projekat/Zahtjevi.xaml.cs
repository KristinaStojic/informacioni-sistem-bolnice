using Projekat.Model;
using Projekat.Pomoc;
using Projekat.Servis;
using System;
using System.Windows;
using System.Windows.Input;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for Zahtjevi.xaml
    /// </summary>
    public partial class Zahtjevi : Window
    {
        public Zahtjevi()
        {
            InitializeComponent();
        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Skladiste.otvoren = true;
                Skladiste skladiste = new Skladiste();
                PremjestajServis.odradiZakazanePremjestaje();
                this.Close();
                skladiste.ShowDialog();
            }
            catch(Exception ex) { Console.WriteLine(ex.Data); }
        }*/

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            Upravnik u = new Upravnik();
            this.Close();
            u.Show();
        }

        private void Osoblje_Click(object sender, RoutedEventArgs e)
        {
            //Osoblje
        }

        private void Sale_Click(object sender, RoutedEventArgs e)
        {
            PrikaziSalu ps = new PrikaziSalu();
            this.Close();
            ps.Show();
        }

        private void Komunikacija_Click(object sender, RoutedEventArgs e)
        {
            Komunikacija komunikacija = new Komunikacija();
            komunikacija.Show();
            this.Close();
        }

        private void Izvjestaj_Click(object sender, RoutedEventArgs e)
        {
            //Izvjestaj
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            ZahtjeviPomoc zahtjeviPomoc = new ZahtjeviPomoc();
            zahtjeviPomoc.Show();
        }

        private void OAplikaciji_Click(object sender, RoutedEventArgs e)
        {
            //O aplikaciji
        }

        /*private void Lijekovi_Click(object sender, RoutedEventArgs e)
        {
            Lijekovi lijekoviProzor = new Lijekovi();
            lijekoviProzor.Show();
            this.Close();
        }*/

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.N)
                {
                    Odustani_Click(sender, e);
                }else if(e.Key == Key.T)
                {
                    Sale_Click(sender, e);
                }
                else if (e.Key == Key.H)
                {
                    Pomoc_Click(sender, e);
                }
                else if(e.Key == Key.K)
                {
                    Komunikacija_Click(sender, e);
                }
            }
        }
    }
}
