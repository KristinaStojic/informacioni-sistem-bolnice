using Model;
using Projekat.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PreraspodjelaDinamicke.xaml
    /// </summary>
    public partial class PreraspodjelaDinamicke : Window, INotifyPropertyChanged
    {
        public static Oprema izabranaOprema;
        public Sala salaDodavanje;
        public static bool aktivna;
        public static int dozvoljenaKolicina;
        public ObservableCollection<Sala> sale { get; set; }
        public ObservableCollection<Oprema> dinamicka { get; set; }
        public int validacija;

        public int Validacija
        {
            get{ return validacija;}
            set
            {
                if (value != validacija)
                {
                    validacija = value;
                    OnPropertyChanged("Validacija");
                }
            }
        }
        public PreraspodjelaDinamicke(Sala izabranaSala)
        {
            InitializeComponent();
            inicijalizujElemente(izabranaSala);
            dodajDinamicku();
        }

        private void inicijalizujElemente(Sala izabranaSala)
        {
            this.salaDodavanje = izabranaSala;
            this.DataContext = this;
            dinamicka = new ObservableCollection<Oprema>();
            sale = new ObservableCollection<Sala>();
            this.Potvrdi.IsEnabled = false;
        }

        private void dodajDinamicku()
        {
            dodajIzSkladista();
            dodajIzSala();
        }

        private void dodajIzSkladista()
        {
            foreach (Oprema oprema in OpremaMenadzer.oprema)
            {
                if (!oprema.Staticka)
                {
                    dinamicka.Add(oprema);
                }
            }
        }

        private void dodajIzSala()
        {
            foreach (Sala sala in SaleMenadzer.sale)
            {
                foreach (Oprema oprema in sala.Oprema)
                {
                    dodajDinamickuOpremu(oprema);
                }
            }
        }

        private void dodajDinamickuOpremu(Oprema oprema)
        {
            if (!oprema.Staticka)
            {

                if (!postojiOprema(oprema))
                {
                    dinamicka.Add(oprema);
                }
            }
        }

        private bool postojiOprema(Oprema oprema)
        {
            foreach (Oprema dinamickaOprema in dinamicka)
            {
                if (dinamickaOprema.IdOpreme == oprema.IdOpreme)
                {
                    return true;
                }
            }
            return false;
        }

        private void azurirajSale(Oprema izabranaOprema)
        {
            sale.Clear();
            foreach (Sala sala in SaleMenadzer.sale)
            {
                foreach (Oprema oprema in sala.Oprema)
                {
                    dodajSalu(oprema, sala);
                }
            }
        }
        
        private void dodajSalu(Oprema oprema, Sala sala)
        {
            if (oprema.IdOpreme == izabranaOprema.IdOpreme)
            {
                if (sala.Id != salaDodavanje.Id)
                {
                    sale.Add(sala);
                }

            }
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            PreraspodjelaDinamicke.aktivna = false;
            this.Close();
            aktivna = false;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            PreraspodjelaDinamicke.izabranaOprema = (Oprema)kombo.SelectedItem;
            foreach(Sala sala in SaleMenadzer.sale)
            {
                prebaciOpremu(sala, (Sala)komboSale.SelectedItem, int.Parse(Kolicina.Text));
            }
            this.Close();
            aktivna = false;
        }

        private void prebaciOpremu(Sala sala, Sala izabranaSala, int kolicina)
        {
            if (sala.Id == izabranaSala.Id)
            {
                ukloniOpremuIzSale(sala, kolicina);
            }
            if (sala.Id == salaDodavanje.Id)
            {
                dodajOpremuUSalu(sala, kolicina);

            }
        }

        private void dodajOpremuUSalu(Sala sala, int kolicina)
        {
            if (prebaciDinamicku(sala, kolicina) == 0)
            {
                Oprema oprema = new Oprema(izabranaOprema.NazivOpreme, kolicina, false);
                oprema.IdOpreme = izabranaOprema.IdOpreme;
                PrikazDinamicke.OpremaDinamicka.Add(oprema);
                sala.Oprema.Add(oprema);
            }
        }

        private int prebaciDinamicku(Sala sala, int kolicina)
        {
            int x = 0;
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    oprema.Kolicina += kolicina;
                    x += 1;
                    azurirajPrikaz(oprema);
                }
            }
            return x;
        }

        private void azurirajPrikaz(Oprema oprema)
        {
            int idx = PrikazDinamicke.OpremaDinamicka.IndexOf(oprema);
            PrikazDinamicke.OpremaDinamicka.RemoveAt(idx);
            PrikazDinamicke.OpremaDinamicka.Insert(idx, oprema);
        }

        private void ukloniOpremuIzSale(Sala sala, int kolicina)
        {
            foreach (Oprema oprema in sala.Oprema.ToArray())
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    ukloniOpremu(oprema, kolicina, sala);
                }
            }
        }

        private void ukloniOpremu(Oprema oprema, int kolicina, Sala sala)
        {
            oprema.Kolicina -= kolicina;
            if (oprema.Kolicina == 0)
            {
                sala.Oprema.Remove(oprema);
            }
        }

        private void kombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PreraspodjelaDinamicke.izabranaOprema = (Oprema)kombo.SelectedItem;
            azurirajSale(izabranaOprema);
            podesiDugme();
        }

        private void komboSale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sala s = (Sala)komboSale.SelectedItem;
            if (s != null)
            {
                azurirajKolicinu(s);
            }
            podesiDugme();
        }
        
        public bool jeBroj(string tekst)
        {
            int test;
            return int.TryParse(tekst, out test);
        }

        private void podesiDugme()
        {
            if (jeBroj(this.Kolicina.Text))
            {
                postaviDugme();
            }
            else
            {
                this.Potvrdi.IsEnabled = false;
            }
        }

        private void postaviDugme()
        {
            if (int.Parse(this.Kolicina.Text) > dozvoljenaKolicina || int.Parse(this.Kolicina.Text) <= 0 || this.kombo.SelectedItem == null || this.komboSale.SelectedItem == null)
            {
                this.Potvrdi.IsEnabled = false;
            }else if (int.Parse(this.Kolicina.Text) <= dozvoljenaKolicina && int.Parse(this.Kolicina.Text) > 0 && this.kombo.SelectedItem != null && this.komboSale.SelectedItem != null)
            {
                this.Potvrdi.IsEnabled = true;
            }
        }

        private void azurirajKolicinu(Sala izmjenjenaSala)
        {
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (izmjenjenaSala.Id == sala.Id)
                {
                    postaviTekst(sala);
                }
            }
        }

        private void postaviTekst(Sala sala)
        {
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    this.tekst.Text = "MAX:" + oprema.Kolicina.ToString();
                    dozvoljenaKolicina = oprema.Kolicina;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PreraspodjelaDinamicke.aktivna = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void Kolicina_TextChanged(object sender, TextChangedEventArgs e)
        {
            podesiDugme();
        }
    }
}
