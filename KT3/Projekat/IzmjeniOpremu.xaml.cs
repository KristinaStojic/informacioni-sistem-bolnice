using Projekat.Model;
using Projekat.Servis;
using System.Windows;
using System.Windows.Controls;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmjeniOpremu.xaml
    /// </summary>
    public partial class IzmjeniOpremu : Window
    {
        //public Oprema izabranaOprema;

        public IzmjeniOpremu()
        {
            InitializeComponent();
            //inicijalizujElemente(izabranaOprema);
        }

        /*private void inicijalizujElemente(Oprema izabranaOprema)
        {
            this.izabranaOprema = izabranaOprema;
            if (izabranaOprema != null)
            {
                this.naziv.Text = izabranaOprema.NazivOpreme;
                this.kolicina.Text = izabranaOprema.Kolicina.ToString();
            }
        }*/

        /*public void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            Oprema oprema = napraviOpremu();
            OpremaServis.izmjeniOpremu(izabranaOprema, oprema);
            izmjeniPrikazOpreme(izabranaOprema, oprema);
            this.Close();
        }*/

        /*private static void izmjeniPrikazOpreme(Oprema izOpreme, Oprema uOpremu)
        {
            foreach (Oprema oprema in OpremaMenadzer.oprema)
            {
                if (oprema.IdOpreme == izOpreme.IdOpreme)
                {
                    oprema.NazivOpreme = uOpremu.NazivOpreme;
                    oprema.Kolicina = uOpremu.Kolicina;
                    zamjeniOpremu(uOpremu, izOpreme, oprema);
                }
            }
        }

        private static void zamjeniOpremu(Oprema uOpremu, Oprema izOpreme, Oprema oprema)
        {
            if (uOpremu.Staticka)
            {
                zamjeniStatickuOpremuUSkladistu(izOpreme, oprema);
            }
            else
            {
                zamjeniDinamickuOpremuUSkladistu(izOpreme, oprema);
            }
        }

        private static void zamjeniDinamickuOpremuUSkladistu(Oprema izOpreme, Oprema oprema)
        {
            int idx = Skladiste.OpremaDinamicka.IndexOf(izOpreme);
            Skladiste.OpremaDinamicka.RemoveAt(idx);
            Skladiste.OpremaDinamicka.Insert(idx, oprema);
        }

        private static void zamjeniStatickuOpremuUSkladistu(Oprema izOpreme, Oprema oprema)
        {
            int idx = Skladiste.OpremaStaticka.IndexOf(izOpreme);
            Skladiste.OpremaStaticka.RemoveAt(idx);
            Skladiste.OpremaStaticka.Insert(idx, oprema);
        }*/

        /*private Oprema napraviOpremu()
        {
            string naziv = this.naziv.Text;
            int kolicina = int.Parse(this.kolicina.Text);
            Oprema uOpremu = new Oprema(naziv, kolicina, izabranaOprema.Staticka);
            uOpremu.IdOpreme = izabranaOprema.IdOpreme;
            return uOpremu;
        }*/

        /*private void Odustani_Click(object sender, RoutedEventArgs e)
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
        }*/
    }
}
