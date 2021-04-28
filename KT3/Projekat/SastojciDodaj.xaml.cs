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
    /// Interaction logic for SastojciDodaj.xaml
    /// </summary>
    public partial class SastojciDodaj : Window
    {
        Lek uneseniLijek;
        public SastojciDodaj(Lek uneseniLijek)
        {
            InitializeComponent();
            this.uneseniLijek = uneseniLijek;
            this.Potvrdi.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            string naziv = this.naziv.Text;
            double kolicina = double.Parse(this.kolicina.Text);
            Sastojak sastojak = new Sastojak(naziv, kolicina);
            uneseniLijek.sastojci.Add(sastojak);
            SastojciDodavanje.SastojciLijeka.Add(sastojak);
            this.Close();
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
    }
}
