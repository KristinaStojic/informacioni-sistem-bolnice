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
    /// Interaction logic for PotvrdaSlanjaZahtjeva.xaml
    /// </summary>
    public partial class PotvrdaSlanjaZahtjeva : Window
    {
        Lek izabraniLijek;
        public PotvrdaSlanjaZahtjeva(Lek izabraniLijek)
        {
            InitializeComponent();
            this.izabraniLijek = izabraniLijek;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           napraviNoviZahtjev();
           LekoviMenadzer.sacuvajIzmeneZahteva();
           LekoviMenadzer.sacuvajIzmjene();
           OdbijeniLijekovi.azurirajPrikaz();
           this.Close();
        }

        private void napraviNoviZahtjev()
        {
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (zahtjev.lek.sifraLeka == izabraniLijek.sifraLeka)
                {
                    zahtjev.obradjenZahtev = false;
                    zahtjev.odobrenZahtev = false;
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
