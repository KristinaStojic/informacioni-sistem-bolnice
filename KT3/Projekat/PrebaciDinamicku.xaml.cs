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

        public PrebaciDinamicku(Oprema oprema)
        {
            InitializeComponent();
            this.opremaZaSlanje = oprema;
            this.oprema.Text = opremaZaSlanje.NazivOpreme;
            this.DataContext = this;
            dodajSale();
            this.maks.Text = "MAX: " + opremaZaSlanje.Kolicina.ToString();
            dozvoljenaKolicina = opremaZaSlanje.Kolicina;
        }

        private void dodajSale()
        {
            Sale = new ObservableCollection<Sala>();
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (!s.Namjena.Equals("Skladiste"))
                {
                    Sale.Add(s);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            aktivan = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Sala salaUKojuSaljem = (Sala)kombo.SelectedItem;
            
            foreach (Sala sala in SaleMenadzer.sale)
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
            this.Close();
            aktivan = false;
        }

        private void dodajOpremuUSalu(Sala sala, int kolicina)
        {
            bool postojiOprema = false;
            foreach (Oprema o in sala.Oprema)
            {
                if (o.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    o.Kolicina += kolicina;
                    postojiOprema = true;
                }
            }
            if (!postojiOprema)
            {
                Oprema op = new Oprema(opremaZaSlanje.NazivOpreme, kolicina, false);
                op.IdOpreme = opremaZaSlanje.IdOpreme;
                sala.Oprema.Add(op);
            }
        }

        public void ukloniOpremuIzSale(Sala s, int kolicina)
        {
            foreach (Oprema o in s.Oprema)
            {
                if (o.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    if (o.Kolicina - kolicina == 0)
                    {
                        s.Oprema.Remove(o);
                        Skladiste.OpremaDinamicka.Remove(o);
                        break;
                    }
                    else
                    {
                        o.Kolicina -= kolicina;
                        int idx = Skladiste.OpremaDinamicka.IndexOf(o);
                        Skladiste.OpremaDinamicka.RemoveAt(idx);
                        Skladiste.OpremaDinamicka.Insert(idx, o);
                    }

                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public bool IsNumeric(string input)
        {
            int test;
            return int.TryParse(input, out test);
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
            if (IsNumeric(this.Kolicina.Text))
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
