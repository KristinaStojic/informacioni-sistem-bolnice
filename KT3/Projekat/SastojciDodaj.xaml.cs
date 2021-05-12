using Projekat.Model;
using System.Windows;
using System.Windows.Controls;

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
            inicijalizujElemente(uneseniLijek);
        }

        private void inicijalizujElemente(Lek uneseniLijek)
        {
            this.uneseniLijek = uneseniLijek;
            this.Potvrdi.IsEnabled = false;
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            
            uneseniLijek.sastojci.Add(napraviSastojak());
            SastojciDodavanje.SastojciLijeka.Add(napraviSastojak());
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
            if (this.kolicina.Text.Trim().Equals("") || this.naziv.Text.Trim().Equals(""))
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
