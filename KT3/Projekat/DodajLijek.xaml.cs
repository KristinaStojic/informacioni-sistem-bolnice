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
    /// Interaction logic for DodajLijek.xaml
    /// </summary>
    public partial class DodajLijek : Window
    {
        Lek uneseniLijek;
        public DodajLijek()
        {
            InitializeComponent();
            inicijalizujDugmad();
        }

        private void inicijalizujDugmad()
        {
            uneseniLijek = new Lek();
            this.Potvrdi.IsEnabled = false;
            this.Sastojci.IsEnabled = false;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            definisiLijek();
            dodajZahtjev(uneseniLijek);
            this.Close();
        }

        private void definisiLijek()
        {
            uneseniLijek.nazivLeka = this.naziv.Text;
            uneseniLijek.sifraLeka = this.sifra.Text;
            uneseniLijek.idLeka = LekoviMenadzer.GenerisanjeIdLijeka();
        }

        private void dodajZahtjev(Lek lijek)
        {
            ZahtevZaLekove zahtjev = new ZahtevZaLekove(LekoviMenadzer.GenerisanjeIdZahtjeva(), lijek, DateTime.Now.Date.ToString("d"), false);
            LekoviMenadzer.zahteviZaLekove.Add(zahtjev);
            LekoviMenadzer.sacuvajIzmeneZahteva();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void sifra_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void naziv_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void postaviDugme()
        {
            if(this.sifra.Text.Trim().Equals("") || this.naziv.Text.Trim().Equals("") || postojiSifraLijeka())
            {
                inicijalizujDugmad();
            }
            else if(!this.sifra.Text.Trim().Equals("") && !this.naziv.Text.Trim().Equals("") && !postojiSifraLijeka())
            {
                aktivirajDugmad();
            }
        }

        private void aktivirajDugmad()
        {
            this.Potvrdi.IsEnabled = true;
            this.Sastojci.IsEnabled = true;
        }

        private bool postojiSifraLijeka()
        {
            foreach(Lek lijek in LekoviMenadzer.lijekovi)
            {
                if(lijek.sifraLeka == this.sifra.Text)
                {
                    return true;
                }
            }
            return false;
        }


        private void Sastojci_Click(object sender, RoutedEventArgs e)
        {
            if (uneseniLijek == null)
            {
                uneseniLijek = new Lek(LekoviMenadzer.GenerisanjeIdLijeka(), this.naziv.Text, this.sifra.Text);
            }
            SastojciDodavanje sastojciDodavanje = new SastojciDodavanje(uneseniLijek);
            sastojciDodavanje.Show();
        }
    }
}
