using Projekat.Model;
using Projekat.Servis;
using System.Windows;
using System.Windows.Controls;

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
            inicijalizujElemente(izabraniLijek);
        }

        private void inicijalizujElemente(Lek izabraniLijek)
        {
            this.Potvrdi.IsEnabled = false;
            this.izabraniLijek = izabraniLijek;
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            Sastojak sastojak = napraviSastojak();
            LekoviServis.dodajSastojak(sastojak, izabraniLijek);
            Sastojci.SastojciLijeka.Add(sastojak);
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
            if (jeBroj(this.kolicina.Text))
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
            }
            else if (!this.kolicina.Text.Trim().Equals("") && !this.naziv.Text.Trim().Equals(""))
            {
                this.Potvrdi.IsEnabled = true;
            }
        }

        public bool jeBroj(string tekst)
        {
            double test;
            return double.TryParse(tekst, out test);
        }
    }
}
