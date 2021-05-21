using Model;
using Projekat.Model;
using Projekat.Servis;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrebaciDinamicku.xaml
    /// </summary>
    public partial class PrebaciDinamicku : Window
    {
        //public ObservableCollection<Sala> Sale { get; set; }
        //Oprema opremaZaSlanje;
        //public static bool aktivan;
        //public static int dozvoljenaKolicina;
        /*public int validacija;

        public int Validacija
        {
            get { return validacija; }
            set
            {
                if (value != validacija)
                {
                    validacija = value;
                    OnPropertyChanged("Validacija");
                }
            }
        }*/

        public PrebaciDinamicku(Oprema oprema)
        {
            InitializeComponent();
            //inicijalizujElemente(oprema);
            //dodajSale();
        }

        /*private void inicijalizujElemente(Oprema oprema)
        {
           // this.opremaZaSlanje = oprema;
            //this.oprema.Text = opremaZaSlanje.NazivOpreme;
            //this.DataContext = this;
            //this.maks.Text = "MAX: " + opremaZaSlanje.Kolicina.ToString();
            //dozvoljenaKolicina = opremaZaSlanje.Kolicina;
        }*/

        /*private void dodajSale()
        {
            Sale = new ObservableCollection<Sala>();
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (!sala.Namjena.Equals("Skladiste"))
                {
                    Sale.Add(sala);
                }
            }
        }*/

        /*private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            aktivan = false;
        }*/

        /*private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            PremjestajServis.izvrsiPremjestanje((Sala)kombo.SelectedItem, int.Parse(Kolicina.Text), opremaZaSlanje);    
            Skladiste.azurirajOpremu();
            this.Close();
            aktivan = false;
        }*/

        /*public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }*/
         
        /*public bool jeBroj(string tekst)
        {
            int test;
            return int.TryParse(tekst, out test);
        }

        private void kombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            postaviDugme();
        }

        private void Kolicina_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }*/

        /*private void postaviDugme()
        {
            if (jeBroj(this.Kolicina.Text))
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
            if (int.Parse(this.Kolicina.Text) > dozvoljenaKolicina || int.Parse(this.Kolicina.Text) <= 0 || this.kombo.SelectedItem == null)
            {
                this.Potvrdi.IsEnabled = false;
            }
            else if(int.Parse(this.Kolicina.Text) <= dozvoljenaKolicina && int.Parse(this.Kolicina.Text) > 0 && this.kombo.SelectedItem != null)
            {
                this.Potvrdi.IsEnabled = true;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            aktivan = false;
        }*/

    }
}
