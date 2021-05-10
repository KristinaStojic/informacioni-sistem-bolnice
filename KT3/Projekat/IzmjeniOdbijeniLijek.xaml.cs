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
    /// Interaction logic for IzmjeniOdbijeniLijek.xaml
    /// </summary>
    public partial class IzmjeniOdbijeniLijek : Window
    {
        public Lek izabraniLijek;
        public IzmjeniOdbijeniLijek(Lek izabraniLijek)
        {
            InitializeComponent();
            this.izabraniLijek = izabraniLijek;
            this.Potvrdi.IsEnabled = false;
            postaviElementeProzora();
        }

        private void postaviElementeProzora()
        {
            if (izabraniLijek != null)
            {
                this.sifra.Text = izabraniLijek.sifraLeka;
                this.naziv.Text = izabraniLijek.nazivLeka;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string sifraLijeka = this.sifra.Text;
            string nazivLijeka = this.naziv.Text;
            Lek lijek = new Lek(izabraniLijek.idLeka, nazivLijeka, sifraLijeka);
            //LekoviMenadzer.izmjeniZahtjev(izabraniLijek, lijek);
            this.Close();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            string sifraLijeka = this.sifra.Text;
            string nazivLijeka = this.naziv.Text;
            Lek lijek = new Lek(izabraniLijek.idLeka, nazivLijeka, sifraLijeka);
            LekoviMenadzer.IzmjeniOdbijeniLijek(izabraniLijek, lijek);
            LekoviMenadzer.sacuvajIzmeneZahteva();
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
            if (this.sifra.Text.Trim().Equals("") || this.naziv.Text.Trim().Equals("") || postojiSifraLijeka())
            {
                this.Potvrdi.IsEnabled = false;
            }
            else if (!this.sifra.Text.Trim().Equals("") && !this.naziv.Text.Trim().Equals("") && !postojiSifraLijeka())
            {
                this.Potvrdi.IsEnabled = true;
            }
        }

        private bool postojiSifraLijeka()
        {
            foreach (Lek lijek in LekoviMenadzer.lijekovi)
            {
                if (lijek.sifraLeka == this.sifra.Text && lijek.idLeka != this.izabraniLijek.idLeka)
                {
                    return true;
                }
            }
            return false;
        }

        private void Sastojci_Click(object sender, RoutedEventArgs e)
        {
            IzmjeniSastojkeOdbijenog izmjeniSastojkeOsbijenog = new IzmjeniSastojkeOdbijenog(izabraniLijek);
            izmjeniSastojkeOsbijenog.Show();
        }
    }

}
