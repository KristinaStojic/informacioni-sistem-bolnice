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
        public DodajLijek()
        {
            InitializeComponent();
            this.Potvrdi.IsEnabled = false;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            string sifraLijeka = this.sifra.Text;
            string nazivLijeka = this.naziv.Text;
            dodajLijek(sifraLijeka, nazivLijeka);
        }

        private void dodajLijek(string sifraLijeka, string nazivLijeka)
        {
            Lek lijek = new Lek(LekoviMenadzer.GenerisanjeIdLijeka(), nazivLijeka, sifraLijeka);
            LekoviMenadzer.DodajLijek(lijek);
            this.Close();
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
                this.Potvrdi.IsEnabled = false;
            }else if(!this.sifra.Text.Trim().Equals("") && !this.naziv.Text.Trim().Equals("") && !postojiSifraLijeka())
            {
                this.Potvrdi.IsEnabled = true;
            }
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
    }
}
