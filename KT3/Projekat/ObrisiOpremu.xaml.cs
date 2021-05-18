using Model;
using Projekat.Model;
using Projekat.Servis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for ObrisiOpremu.xaml
    /// </summary>
    public partial class ObrisiOpremu : Window
    {
        public Oprema izabranaOprema;
        public int dozvoljenaKolicina;

        public ObrisiOpremu(Oprema izabranaOprema)
        {
            InitializeComponent();
            inicijalizujElemente(izabranaOprema);
            postaviMaksimalnuKolicinu();
        }

        private void inicijalizujElemente(Oprema izabranaOprema)
        {
            this.izabranaOprema = izabranaOprema;
            this.Potvrdi.IsEnabled = false;
        }

        private void postaviMaksimalnuKolicinu()
        {
            this.maks.Text = "MAX: " + nadjiDozvoljenuKolicinu();
        }

        private string nadjiDozvoljenuKolicinu()
        {
            dozvoljenaKolicina = izabranaOprema.Kolicina;
            foreach (Premjestaj premjestaj in PremjestajServis.Premjestaji())
            {
                if (premjestaj.izSale.Id == 4 && premjestaj.oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    dozvoljenaKolicina -= premjestaj.kolicina;
                }
            }
            return dozvoljenaKolicina.ToString();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            SaleServis.ukloniOpremuIzSale(izabranaOprema, int.Parse(kolicina.Text));
            Skladiste.azurirajOpremu();
            this.Close();
        }

        private void kolicina_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (jeBroj(this.kolicina.Text)) {
                postaviDugme();
            }else{
                this.Potvrdi.IsEnabled = false;
            }
        }

        private void postaviDugme()
        {
            if (int.Parse(this.kolicina.Text) > dozvoljenaKolicina || int.Parse(this.kolicina.Text) <= 0 || this.kolicina.Text.Trim().Equals(""))
            {
                this.Potvrdi.IsEnabled = false;
            }
            else if (int.Parse(this.kolicina.Text) <= dozvoljenaKolicina && int.Parse(this.kolicina.Text) > 0 && !this.kolicina.Text.Trim().Equals(""))
            {
                this.Potvrdi.IsEnabled = true;
            }
        }

        public bool jeBroj(string tekst)
        {
            int test;
            return int.TryParse(tekst, out test);
        }
    }
}
