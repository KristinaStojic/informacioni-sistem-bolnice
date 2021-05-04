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
    /// Interaction logic for DodajSastojak.xaml
    /// </summary>
    public partial class DodajSastojak : Window
    {
        public Lek izabraniLijek;
        public DodajSastojak(Lek izabraniLijek)
        {
            InitializeComponent();
            this.Potvrdi.IsEnabled = false;
            this.izabraniLijek = izabraniLijek;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {   
            LekoviMenadzer.dodajSastojak(napraviSastojak(), izabraniLijek);
            this.Close();
        }

        private Sastojak napraviSastojak()
        {
            string naziv = this.naziv.Text;
            double kolicina = double.Parse(this.kolicina.Text);
            return new Sastojak(naziv, kolicina);
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
            if(this.kolicina.Text.Trim().Equals("") || this.naziv.Text.Trim().Equals(""))
            {
                this.Potvrdi.IsEnabled = false;
            }else if (!this.kolicina.Text.Trim().Equals("") && !this.naziv.Text.Trim().Equals(""))
            {
                this.Potvrdi.IsEnabled = true;
            }
        }
        public bool IsNumeric(string input)
        {
            double test;
            return double.TryParse(input, out test);
        }
    }
}
