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
    /// Interaction logic for SlanjeDinamicke.xaml
    /// </summary>
    public partial class SlanjeDinamicke : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Sala> sale { get; set; }
        Oprema opremaZaSlanje;
        Sala salaIzKojeSaljem;
        public static bool aktivan;
        public static int dozvoljenaKolicina;
        public int validacija;

        public int Validacija
        {
            get{return validacija;}
            set
            {
                if (value != validacija)
                {
                    validacija = value;
                    OnPropertyChanged("Validacija");
                }
            }
        }

        public SlanjeDinamicke(Sala izabranaSala, Oprema kojuSaljem)
        {
            InitializeComponent();
            inicijalizujElemente(izabranaSala, kojuSaljem);
            dodajSale(izabranaSala);
        }

        private void inicijalizujElemente(Sala izabranaSala, Oprema kojuSaljem) 
        {
            this.opremaZaSlanje = kojuSaljem;
            this.salaIzKojeSaljem = izabranaSala;
            this.tekst.Text = kojuSaljem.NazivOpreme;
            this.DataContext = this;
            sale = new ObservableCollection<Sala>();
            this.maks.Text = "MAX: " + kojuSaljem.Kolicina.ToString();
            dozvoljenaKolicina = kojuSaljem.Kolicina;
            this.Potvrdi.IsEnabled = false;
        }

        private void dodajSale(Sala izabranaSala)
        {
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.Id != izabranaSala.Id)
                {
                    sale.Add(s);
                }
            }
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            aktivan = false;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            
            PremjestajServis.prebaciDinamickuOpremu((Sala)kombo.SelectedItem, int.Parse(KOlicina.Text), salaIzKojeSaljem, opremaZaSlanje);
            this.Close();
            aktivan = false;
        }
        public static void prebaciOpremu(Oprema oprema, int kolicina)
        {
            
            int idx = PrikazDinamicke.OpremaDinamicka.IndexOf(oprema);
            PrikazDinamicke.OpremaDinamicka.RemoveAt(idx);
            PrikazDinamicke.OpremaDinamicka.Insert(idx, oprema);
        }

        public static void ukloniOpremu(Sala sala, Oprema oprema)
        {
            PrikazDinamicke.OpremaDinamicka.Remove(oprema);
        }
            
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            aktivan = false;
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

        private void podesiDugme()
        {
            if (jeBroj(this.KOlicina.Text))
            {
                izvrsiPodesavanje();
            }
            else
            {
                this.Potvrdi.IsEnabled = false;
            }
        }

        private void izvrsiPodesavanje()
        {
            if (int.Parse(this.KOlicina.Text) > dozvoljenaKolicina || int.Parse(this.KOlicina.Text) <= 0 || this.kombo.SelectedItem == null)
            {
                this.Potvrdi.IsEnabled = false;
            }else if (int.Parse(this.KOlicina.Text) <= dozvoljenaKolicina && int.Parse(this.KOlicina.Text) > 0 && this.kombo.SelectedItem != null)
            {
                this.Potvrdi.IsEnabled = true;
            }
        }

        private void kombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            podesiDugme();
        }

        private void KOlicina_TextChanged(object sender, TextChangedEventArgs e)
        {
            podesiDugme();
        }
    }
}
