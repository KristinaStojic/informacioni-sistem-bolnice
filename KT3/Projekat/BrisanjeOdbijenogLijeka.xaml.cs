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
    /// Interaction logic for BrisanjeOdbijenogLijeka.xaml
    /// </summary>
    public partial class BrisanjeOdbijenogLijeka : Window
    {
        public Lek izabraniLijek;
        public BrisanjeOdbijenogLijeka(Lek izabraniLijek)
        {
            InitializeComponent();
            this.izabraniLijek = izabraniLijek;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ZahtevZaLekove izabraniZahtjev = null;
            foreach(ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if(zahtjev.lek.sifraLeka.Equals(izabraniLijek.sifraLeka))
                {
                    izabraniZahtjev = zahtjev;   
                }
            }
            LekoviMenadzer.zahteviZaLekove.Remove(izabraniZahtjev);
            OdbijeniLijekovi.azurirajPrikaz();
            LekoviMenadzer.sacuvajIzmeneZahteva();
            this.Close();
        }
    }
}
