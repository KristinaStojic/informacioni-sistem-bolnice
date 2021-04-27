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
    /// Interaction logic for IzmeniSastojakLekar.xaml
    /// </summary>
    public partial class IzmeniSastojakLekar : Window
    {
        public Sastojak stariSastojak;
        public Lek lek;
        public IzmeniSastojakLekar(Lek izabraniLek, Sastojak izabraniSastojak)
        {
            InitializeComponent();
            this.stariSastojak = izabraniSastojak;
            this.lek = izabraniLek;
            postaviElemente();
        }

        private void postaviElemente()
        {
            this.naziv.Text = stariSastojak.naziv;
            this.kolicina.Text = stariSastojak.kolicina.ToString();
        }
        private void naziv_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void kolicina_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void postaviDugme()
        {
            if (IsNumeric(this.kolicina.Text))
            {
                izvrsiPostavljanje();
            }
            else
            {
                this.Potvrdi.IsEnabled = false;
            }
        }
        private void izvrsiPostavljanje()
        {
            if (this.kolicina.Text.Trim().Equals("") || this.naziv.Text.Trim().Equals(""))
            {
                this.Potvrdi.IsEnabled = false;
            }
            else if (!this.kolicina.Text.Trim().Equals("") && !this.naziv.Text.Trim().Equals(""))
            {
                this.Potvrdi.IsEnabled = true;
            }
        }
        public bool IsNumeric(string input)
        {
            double test;
            return double.TryParse(input, out test);
        }


        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            string naziv = this.naziv.Text;
            double kolicina = double.Parse(this.kolicina.Text);
            Sastojak noviSastojak = new Sastojak(naziv, kolicina);
            LekoviMenadzer.izmeniSastojakLekaLekar(lek, stariSastojak, noviSastojak);
            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
