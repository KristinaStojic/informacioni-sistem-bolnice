using Projekat.Model;
using Projekat.Servis;
using System.Windows;
using System.Windows.Controls;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmjeniSastojak.xaml
    /// </summary>
    public partial class IzmjeniSastojak : Window
    {
        //public Sastojak izabraniSastojak;
        //public Lek izabraniLijek;

        public IzmjeniSastojak()
        {
            InitializeComponent();
            //inicijalizujElemente(izabraniSastojak, izabraniLijek);
           // postaviElemente();
        }

        /*private void inicijalizujElemente(Sastojak izabraniSastojak, Lek izabraniLijek)
        {
            this.izabraniSastojak = izabraniSastojak;
            this.izabraniLijek = izabraniLijek;
        }

        private void postaviElemente()
        {
            this.naziv.Text = izabraniSastojak.naziv;
            this.kolicina.Text = izabraniSastojak.kolicina.ToString();
        }*/

        /*private void naziv_TextChanged(object sender, TextChangedEventArgs e)
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
        }*/

        /*private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            Sastojak sastojak = napraviSastojak();
            LekoviServis.izmjeniSastojakLijeka(izabraniLijek, izabraniSastojak, sastojak);
            int idx = Sastojci.SastojciLijeka.IndexOf(izabraniSastojak);
            Sastojci.SastojciLijeka.RemoveAt(idx);
            Sastojci.SastojciLijeka.Insert(idx, sastojak);
            this.Close();
        }*/

        /*private Sastojak napraviSastojak()
        {
            string naziv = this.naziv.Text;
            double kolicina = double.Parse(this.kolicina.Text);
            return new Sastojak(naziv, kolicina);
        }*/

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }*/
    }
}
