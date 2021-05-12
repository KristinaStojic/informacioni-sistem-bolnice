using Model;
using Projekat.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrebaciDinamicku.xaml
    /// </summary>
    public partial class PrebaciDinamicku : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Sala> Sale { get; set; }
        Oprema opremaZaSlanje;
        public static bool aktivan;
        public static int dozvoljenaKolicina;
        public int validacija;

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
        }

        public PrebaciDinamicku(Oprema oprema)
        {
            InitializeComponent();
            inicijalizujElemente(oprema);
            dodajSale();
        }

        private void inicijalizujElemente(Oprema oprema)
        {
            this.opremaZaSlanje = oprema;
            this.oprema.Text = opremaZaSlanje.NazivOpreme;
            this.DataContext = this;
            this.maks.Text = "MAX: " + opremaZaSlanje.Kolicina.ToString();
            dozvoljenaKolicina = opremaZaSlanje.Kolicina;
        }

        private void dodajSale()
        {
            Sale = new ObservableCollection<Sala>();
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (!sala.Namjena.Equals("Skladiste"))
                {
                    Sale.Add(sala);
                }
            }
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            aktivan = false;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            foreach (Sala sala in SaleMenadzer.sale)
            {
                izvrsiPremjestanje(sala, (Sala)kombo.SelectedItem);
            }
            this.Close();
            aktivan = false;
        }

        private void izvrsiPremjestanje(Sala sala, Sala salaUKojuSaljem)
        {
            if (sala.Namjena.Equals("Skladiste"))
            {
                ukloniOpremuIzSale(sala, int.Parse(Kolicina.Text));
            }
            if (sala.Id == salaUKojuSaljem.Id)
            {
                dodajOpremuUSalu(sala, int.Parse(Kolicina.Text));
            }
        }

        private void dodajOpremuUSalu(Sala sala, int kolicina)
        {
            bool postojiOprema = false;
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    oprema.Kolicina += kolicina;
                    postojiOprema = true;
                }
            }
            dodajNovuOpremu(postojiOprema, kolicina, sala);
        }

        private void dodajNovuOpremu(bool postojiOprema, int kolicina, Sala sala)
        {
            if (!postojiOprema)
            {
                Oprema oprema = new Oprema(opremaZaSlanje.NazivOpreme, kolicina, false);
                oprema.IdOpreme = opremaZaSlanje.IdOpreme;
                sala.Oprema.Add(oprema);
            }
        }

        public void ukloniOpremuIzSale(Sala sala, int kolicina)
        {
            foreach (Oprema oprema in sala.Oprema.ToArray())
            {
                if (oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    prebaciOpremu(oprema, kolicina, sala);
                }
            }
        }

        private void prebaciOpremu(Oprema oprema, int kolicina, Sala sala)
        {
            if (oprema.Kolicina - kolicina == 0)
            {
                ukloniSvuOpremu(oprema, sala);
            }
            else
            {
                smanjiKolicinuOpreme(oprema, kolicina);
            }
        }

        private void smanjiKolicinuOpreme(Oprema oprema, int kolicina)
        {
            oprema.Kolicina -= kolicina;
            int idx = Skladiste.OpremaDinamicka.IndexOf(oprema);
            Skladiste.OpremaDinamicka.RemoveAt(idx);
            Skladiste.OpremaDinamicka.Insert(idx, oprema);
        }

        private void ukloniSvuOpremu(Oprema oprema, Sala sala)
        {
            sala.Oprema.Remove(oprema);
            Skladiste.OpremaDinamicka.Remove(oprema);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
         
        public bool jeBroj(string tekst)
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
        }

        private void postaviDugme()
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
        }

    }
}
