using Projekat.Model;
using Projekat.Servis;
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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for ObradiZahtevZaLek.xaml
    /// </summary>
    public partial class ObradiZahtevZaLek : Window
    {
        ZahtevZaLekove zahtev;
        public ObradiZahtevZaLek(ZahtevZaLekove izabraniZahtev)
        {
            InitializeComponent();
            this.DataContext = this;
            this.zahtev = izabraniZahtev;
            this.spisakSastojaka.ItemsSource = LekoviServis.nadjiSastojke(izabraniZahtev);
            this.datum.SelectedDate = DateTime.Parse(izabraniZahtev.datumSlanjaZahteva);
            this.naziv.Text = izabraniZahtev.nazivLeka;
            this.sifra.Text = izabraniZahtev.sifraLeka;

        }

        private void Button_Odbij(object sender, RoutedEventArgs e)
        {
            OdbijZahtevZaLek odbijZahtev = new OdbijZahtevZaLek(zahtev);
            odbijZahtev.Show();
            this.Close();
        }

        private void Button_Odobri(object sender, RoutedEventArgs e)
        {
            zahtev.odobrenZahtev = true;
            zahtev.obradjenZahtev = true;
            Lek lek = new Lek(LekoviServis.GenerisanjeIdLijeka(), zahtev.nazivLeka,zahtev.sifraLeka, zahtev.lek.zamenskiLekovi, zahtev.lek.sastojci);
            LekoviServis.DodajLijek(lek);
            LekoviServis.izmeniZahtev(zahtev);
            LekoviServis.sacuvajIzmeneZahteva();
            this.Close();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Odbij(sender, e);
            }
            else if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Odobri(sender, e);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
