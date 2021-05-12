﻿using Projekat.Model;
using System.Windows;

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

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
           napraviNoviZahtjev();
           sacuvajIzmjene();
           this.Close();
        }

        private void sacuvajIzmjene()
        {
            LekoviMenadzer.sacuvajIzmeneZahteva();
            LekoviMenadzer.sacuvajIzmjene();
            OdbijeniLijekovi.azurirajPrikaz();
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

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
