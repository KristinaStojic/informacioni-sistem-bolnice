using Model;
using Projekat.Model;
using Projekat.Servis;
using Projekat.ViewModel;
using System;
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
            PremjestajServis.prebaciOpremu((Sala)komboSale.SelectedItem, int.Parse(Kolicina.Text), izabranaOprema, salaDodavanje);    
            this.Close();
            aktivna = false;
        }


        public static void azurirajPrikaz(Oprema oprema)
        {
            int idx = PrikazDinamicke.OpremaDinamicka.IndexOf(oprema);
            PrikazDinamicke.OpremaDinamicka.RemoveAt(idx);
            PrikazDinamicke.OpremaDinamicka.Insert(idx, oprema);
        }

        public static void smanjiKolicinuOpreme(int kolicina, Oprema oprema, Sala sala)
        {
            if (oprema.Kolicina - kolicina == 0)
            {
                sala.Oprema.Remove(oprema);
                //SkladisteViewModel.OpremaStaticka.Remove(oprema);
                SkladisteViewModel.azurirajPrikaz();
            }
            else
            {
                oprema.Kolicina -= kolicina;
                SkladisteViewModel.azurirajPrikaz();
                /*int idx = SkladisteViewModel.OpremaStaticka.IndexOf(oprema);
                SkladisteViewModel.OpremaStaticka.RemoveAt(idx);
                SkladisteViewModel.OpremaStaticka.Insert(idx, oprema);*/
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
