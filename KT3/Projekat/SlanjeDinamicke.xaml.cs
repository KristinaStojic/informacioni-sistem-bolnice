using Model;
using Projekat.Model;
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
            foreach(Sala sala in SaleMenadzer.sale)
            {
                posaljiIzSale(sala);
                dodajUSalu(sala, (Sala)kombo.SelectedItem);
            }
            this.Close();
            aktivan = false;
        }

        private void dodajUSalu(Sala sala, Sala salaUKojuSaljem)
        {
            if (sala.Id == salaUKojuSaljem.Id)
            {
                dodajOpremuUSalu(sala, int.Parse(KOlicina.Text));
            }
        }

        private void posaljiIzSale(Sala sala)
        {
            if (sala.Id == salaIzKojeSaljem.Id)
            {
                foreach (Oprema o in sala.Oprema.ToArray())
                {
                    prebaciOpremuIzSale(o, int.Parse(KOlicina.Text), sala);
                }
            }
        }

        private void dodajOpremuUSalu(Sala sala, int kolicina)
        {
            if (!postojiOprema(sala, kolicina))
            {
                Oprema oprema = new Oprema(opremaZaSlanje.NazivOpreme, kolicina, false);
                oprema.IdOpreme = opremaZaSlanje.IdOpreme;
                sala.Oprema.Add(oprema);
            }
        }

        private bool postojiOprema(Sala sala, int kolicina)
        {
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    oprema.Kolicina += kolicina;
                    return true;
                }
            }
            return false;
        }

        private void prebaciOpremuIzSale(Oprema oprema, int kolicina, Sala sala)
        {
            if (oprema.IdOpreme == opremaZaSlanje.IdOpreme)
            {
                if (oprema.Kolicina - kolicina == 0)
                {
                    ukloniOpremu(sala, oprema);
                }
                else
                {
                    prebaciOpremu(oprema, kolicina);
                }

            }
        }

        private void prebaciOpremu(Oprema oprema, int kolicina)
        {
            oprema.Kolicina -= kolicina;
            int idx = PrikazDinamicke.OpremaDinamicka.IndexOf(oprema);
            PrikazDinamicke.OpremaDinamicka.RemoveAt(idx);
            PrikazDinamicke.OpremaDinamicka.Insert(idx, oprema);
        }

        private void ukloniOpremu(Sala sala, Oprema oprema)
        {
            sala.Oprema.Remove(oprema);
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
