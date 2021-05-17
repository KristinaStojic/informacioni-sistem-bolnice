using Projekat.Model;
using Projekat.Servis;
using System;
using System.Windows;
using System.Windows.Controls;

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
            uneseniLijek = new Lek();
            inicijalizujDugmad();
        }

        private void inicijalizujDugmad()
        {
            this.Potvrdi.IsEnabled = false;
            this.Sastojci.IsEnabled = false;
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            definisiLijek();
            LekoviServis.dodajZahtjev(uneseniLijek);
            this.Close();
        }

        private void definisiLijek()
        {
            uneseniLijek.nazivLeka = this.naziv.Text;
            uneseniLijek.sifraLeka = this.sifra.Text;
            uneseniLijek.idLeka = LekoviServis.GenerisanjeIdLijeka();
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
            foreach(Lek lijek in LekoviServis.Lijekovi())
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
                uneseniLijek = new Lek(LekoviServis.GenerisanjeIdLijeka(), this.naziv.Text, this.sifra.Text);
            }
            SastojciDodavanje sastojciDodavanje = new SastojciDodavanje(uneseniLijek);
            sastojciDodavanje.Show();
        }
    }
}
