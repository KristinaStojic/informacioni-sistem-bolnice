using Model;
using Projekat.Model;
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
            foreach (Premjestaj pm in PremjestajMenadzer.premjestaji)
            {
                if (pm.izSale.Id == 4 && pm.oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    dozvoljenaKolicina -= pm.kolicina;
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
            foreach(Sala sala in SaleMenadzer.sale)
            {
                if (sala.Namjena.Equals("Skladiste"))
                {
                    ukloniOpremuIzSale(sala);
                }
            }
            Skladiste.azurirajOpremu();
            this.Close();
        }

        private void ukloniOpremuIzSale(Sala sala)
        {
            foreach (Oprema oprema in sala.Oprema.ToList())
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    oprema.Kolicina -= int.Parse(kolicina.Text);
                    if (oprema.Kolicina == 0)
                    {
                        sala.Oprema.Remove(oprema);
                    }
                }
            }
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
