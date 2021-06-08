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
    /// Interaction logic for ObrisiZahtevLekar.xaml
    /// </summary>
    public partial class ObrisiZahtevLekar : Window
    {
        ZahtevZaLekove Zahtev;
        public ObrisiZahtevLekar(ZahtevZaLekove izabraniZahtev)
        {
            InitializeComponent();
            this.Zahtev = izabraniZahtev;
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            ZahtevZaLekove izabraniZahtjev = null;
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (zahtjev.lek.sifraLeka.Equals(Zahtev.sifraLeka))
                {
                    izabraniZahtjev = zahtjev;
                }
            }

            LekoviMenadzer.zahteviZaLekove.Remove(izabraniZahtjev);
            //SpisakZahtevaZaLekove.TabelaZahteva.Remove(Zahtev);
            LekoviServis.sacuvajIzmeneZahteva();

            this.Close();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl)) //Sacuvaj
            {
                Potvrdi_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) //Nazad
            {
                this.Close();
            }
        }
    }
}
