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
    /// Interaction logic for DodajSastojakLekar.xaml
    /// </summary>
    public partial class DodajSastojakLekar : Window
    {
        Lek lek;
        public bool popunjeno = false;
        public DodajSastojakLekar(Lek izabraniLek)
        {
            InitializeComponent();
            /*this.lek = izabraniLek;
            this.Potvrdi.IsEnabled = false;
            this.validacija.Visibility = Visibility.Hidden;*/

        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (popunjeno)
            {
                string naziv = this.naziv.Text;
                double kolicina = double.Parse(this.kolicina.Text);
                Sastojak sastojak = new Sastojak(naziv, kolicina);
                LekoviMenadzer.dodajSastojakLekar(sastojak, lek);
                this.Close();
            }
            else
            {
                MessageBox.Show("Niste uneli sve podatke!");
            }
            
        }


        private void naziv_TextChanged(object sender, TextChangedEventArgs e)
        {
            //postaviDugmeNaziv();
        }

        private void kolicina_TextChanged(object sender, TextChangedEventArgs e)
        {
            //postaviDugme();
        }
        /*private void postaviDugme()
        {
            if (IsNumeric(this.kolicina.Text))
            {
                izvrsiPostavljanje();
                this.validacija.Visibility = Visibility.Hidden;
            }
            else
            {
                this.validacija.Visibility = Visibility.Visible;
                this.Potvrdi.IsEnabled = false;
                popunjeno = false;
            }
        }*/

        /*private void postaviDugmeNaziv()
        {
            if (IsNumeric(this.kolicina.Text))
            {
                izvrsiPostavljanje();
            }
            else
            {
                this.Potvrdi.IsEnabled = false;
                popunjeno = false;
            }
        }


        private void izvrsiPostavljanje()
        {
            if (this.kolicina.Text.Trim().Equals("") || this.naziv.Text.Trim().Equals(""))
            {
                this.Potvrdi.IsEnabled = false;
                popunjeno = false;
            }
            else if (!this.kolicina.Text.Trim().Equals("") && !this.naziv.Text.Trim().Equals(""))
            {
                this.Potvrdi.IsEnabled = true;
                popunjeno = true;
            }
        }
        public bool IsNumeric(string input)
        {
            double test;
            return double.TryParse(input, out test);
        }
        */
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Potvrdi_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Odustani_Click(sender, e);
            }*/
        }
    }
}
