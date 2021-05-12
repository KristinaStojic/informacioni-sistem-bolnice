using Projekat.Model;
using System.Windows;
using System.Windows.Controls;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmjeniOpremu.xaml
    /// </summary>
    public partial class IzmjeniOpremu : Window
    {
        public Oprema izabranaOprema;

        public IzmjeniOpremu(Oprema izabranaOprema)
        {
            InitializeComponent();
            inicijalizujElemente(izabranaOprema);
        }

        private void inicijalizujElemente(Oprema izabranaOprema)
        {
            this.izabranaOprema = izabranaOprema;
            if (izabranaOprema != null)
            {
                this.naziv.Text = izabranaOprema.NazivOpreme;
                this.kolicina.Text = izabranaOprema.Kolicina.ToString();
            }
        }

        public void Potvrdi_Click(object sender, RoutedEventArgs e)
        { 
            OpremaMenadzer.izmjeniOpremu(izabranaOprema, napraviOpremu());
            this.Close();
        }

        private Oprema napraviOpremu()
        {
            string naziv = this.naziv.Text;
            int kolicina = int.Parse(this.kolicina.Text);
            Oprema uOpremu = new Oprema(naziv, kolicina, izabranaOprema.Staticka);
            uOpremu.IdOpreme = izabranaOprema.IdOpreme;
            return uOpremu;
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
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
            if (this.naziv.Text.Trim().Equals("") || jeBroj(this.naziv.Text) || !jeBroj(this.kolicina.Text) || this.kolicina.Text.Trim().Equals(""))
            {
                this.Potvrdi.IsEnabled = false;
            }
            else if(!this.naziv.Text.Trim().Equals("") && !jeBroj(this.naziv.Text) && jeBroj(this.kolicina.Text) && !this.kolicina.Text.Trim().Equals(""))
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
