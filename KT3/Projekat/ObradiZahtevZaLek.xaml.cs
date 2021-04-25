using Projekat.Model;
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
            
            this.spisakSastojaka.ItemsSource = LekoviMenadzer.nadjiSastojke(izabraniZahtev.sifraLeka);
            this.datum.SelectedDate = DateTime.Parse(izabraniZahtev.datumSlanjaZahteva);
            this.naziv.Text = izabraniZahtev.nazivLeka;
            this.sifra.Text = izabraniZahtev.sifraLeka;

        }

        private void Button_Odbij(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button_Odobri(object sender, RoutedEventArgs e)
        {
            zahtev.odobrenZahtev = true;
            this.Close();
        }
    }
}
