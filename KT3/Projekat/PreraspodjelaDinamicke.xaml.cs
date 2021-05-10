using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            get
            {
                return validacija;
            }
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
            this.salaDodavanje = izabranaSala;
            dodajDinamicku();
        }

        private void inicijalizujElemente()
        {
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
            bool postojiOprema = false;
            foreach (Sala sala in SaleMenadzer.sale)
            {
                foreach (Oprema oprema in sala.Oprema)
                {
                    postojiOprema = false;
                    if (!oprema.Staticka)
                    {
                        foreach (Oprema dinamickaOprema in dinamicka)
                        {
                            if (dinamickaOprema.IdOpreme == oprema.IdOpreme)
                            {
                                postojiOprema = true;
                            }
                        }
                        if (!postojiOprema)
                        {
                            dinamicka.Add(oprema);
                        }
                    }
                }
            }
        }

        private void azurirajSale(Oprema izabranaOprema)
        {
            sale.Clear();
            foreach (Sala sala in SaleMenadzer.sale)
            {
                foreach (Oprema oprema in sala.Oprema)
                {
                    if(oprema.IdOpreme == izabranaOprema.IdOpreme)
                    {
                        if (sala.Id != salaDodavanje.Id)
                        {
                            sale.Add(sala);
                        }
                        
                    }
                }
            }

        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PreraspodjelaDinamicke.aktivna = false;
            this.Close();
            aktivna = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Sala izabranaSala = (Sala)komboSale.SelectedItem;
            int kolicina = int.Parse(Kolicina.Text);
            PreraspodjelaDinamicke.izabranaOprema = (Oprema)kombo.SelectedItem;
            foreach(Sala sala in SaleMenadzer.sale)
            {
                if(sala.Id == izabranaSala.Id)
                {
                    ukloniOpremuIzSale(sala, kolicina);
                }
                if(sala.Id == salaDodavanje.Id)
                {
                    dodajOpremuUSalu(sala, kolicina);
                    
                }
            }
            this.Close();
            aktivna = false;
        }

        private void dodajOpremuUSalu(Sala sala, int kolicina)
        {
            int x = 0;
            foreach (Oprema o in sala.Oprema)
            {
                if (o.IdOpreme == izabranaOprema.IdOpreme)
                {
                    o.Kolicina += kolicina;
                    x += 1;
                    int idx = PrikazDinamicke.OpremaDinamicka.IndexOf(o);
                    PrikazDinamicke.OpremaDinamicka.RemoveAt(idx);
                    PrikazDinamicke.OpremaDinamicka.Insert(idx, o);
                }


            }
            if (x == 0)
            {
                Oprema oprema = new Oprema(izabranaOprema.NazivOpreme, kolicina, false);
                oprema.IdOpreme = izabranaOprema.IdOpreme;
                PrikazDinamicke.OpremaDinamicka.Add(oprema);
                sala.Oprema.Add(oprema);
            }
        }

        private void ukloniOpremuIzSale(Sala sala, int kolicina)
        {
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    oprema.Kolicina -= kolicina;
                    if (oprema.Kolicina == 0)
                    {
                        sala.Oprema.Remove(oprema);
                        break;
                    }
                }
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
        
        public bool IsNumeric(string input)
        {
            int test;
            return int.TryParse(input, out test);
        }

        private void podesiDugme()
        {
            if (IsNumeric(this.Kolicina.Text))
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

        private void azurirajKolicinu(Sala s)
        {
            foreach (Sala sal in SaleMenadzer.sale)
            {
                if (s.Id == sal.Id)
                {
                    foreach (Oprema o in sal.Oprema)
                    {
                        if (o.IdOpreme == izabranaOprema.IdOpreme)
                        {
                            this.tekst.Text = "MAX:" + o.Kolicina.ToString();
                            dozvoljenaKolicina = o.Kolicina;
                        }
                    }
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
