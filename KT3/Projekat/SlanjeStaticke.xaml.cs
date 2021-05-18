using Model;
using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for SlanjeStaticke.xaml
    /// </summary>
    public partial class SlanjeStaticke : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Sala> Sale { get; set; }
        public ObservableCollection<string> termini { get; set; }
        Oprema opremaZaSlanje;
        Sala salaIzKojeSaljem;
        public static int dozvoljenaKolicina;
        public int validacija;
        public static bool aktivan;

        public int Validacija
        {
            get {return validacija;}
            set
            {
                if (value != validacija)
                {
                    validacija = value;
                    OnPropertyChanged("Validacija");
                }
            }
        }

        public SlanjeStaticke(Sala izabranaSala, Oprema opremaZaSlanje)
        {
            termini = new ObservableCollection<string>();
            InitializeComponent();
            inicijalizujElemente(opremaZaSlanje, izabranaSala);
            dodajSale(izabranaSala);
            dodajTermine();
            postaviDozvoljenuKolicinu();            
        }

        private void postaviDozvoljenuKolicinu()
        {
            int kolicina = opremaZaSlanje.Kolicina;
            foreach (Premjestaj premjestaj in PremjestajMenadzer.premjestaji)
            {
                if (premjestaj.izSale.Id == salaIzKojeSaljem.Id && premjestaj.oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    kolicina -= premjestaj.kolicina;
                }
            }
            this.maks.Text = "MAX: " + kolicina.ToString();
            dozvoljenaKolicina = kolicina;
        }

        private void inicijalizujElemente(Oprema opremaZaSlanje, Sala izabranaSala)
        {
            this.opremaZaSlanje = opremaZaSlanje;
            this.salaIzKojeSaljem = izabranaSala;
            this.oprema.Text = opremaZaSlanje.NazivOpreme;
            this.DataContext = this;
            Sale = new ObservableCollection<Sala>();
            this.Potvrdi.IsEnabled = false;
        }

        private void dodajTermine()
        {
            for (int termin = (int)DateTime.Now.Hour + 1; termin <= 23; termin++)
            {
                if (!postojiTermin(termin))
                {
                    termini.Add(termin + ":00");
                }
            }
        }

        private bool postojiTermin(int termin)
        {
            foreach (Premjestaj premjestaj in PremjestajMenadzer.premjestaji)
            {
                if (premjestaj.datumIVrijeme.Hour.ToString().Equals(termin.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        private void dodajSale(Sala izabranaSala)
        {
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.Id != izabranaSala.Id)
                {
                    Sale.Add(s);
                }
            }
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            aktivan = false;
            this.Close();
        }

        private DateTime nadjiTerminPremjestaja()
        {
            DateTime? datumSlanja = DatePicker.SelectedDate;
            string vrijemeSlanja = vrijeme.SelectedItem.ToString();
            string datum = datumSlanja.Value.ToString("dd.MM.yyy", System.Globalization.CultureInfo.InvariantCulture);
            string[] datumi = datum.Split('.');
            string dan = datumi[0];
            string mjesec = datumi[1];
            string godina = datumi[2];
            string[] sati = vrijemeSlanja.Split(':');
            string sat = sati[0];
            string minuti = sati[1];
            return new DateTime(int.Parse(godina), int.Parse(mjesec), int.Parse(dan), int.Parse(sat), int.Parse(minuti), 0);
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            PremjestajServis.prebaciStatickuOpremu(nadjiTerminPremjestaja(), int.Parse(Kolicina.Text), (Sala)sale.SelectedItem, salaIzKojeSaljem, opremaZaSlanje);
            aktivan = false;
            this.Close();
        }

        public static void smanjiKolicinuOpreme(Oprema oprema, int kolicina)
        {
            
            int idx = PrikazStaticke.OpremaStaticka.IndexOf(oprema);
            PrikazStaticke.OpremaStaticka.RemoveAt(idx);
            PrikazStaticke.OpremaStaticka.Insert(idx, oprema);
        }

        public static void ukloniOpremu(Sala sala, Oprema oprema)
        {
            PrikazStaticke.OpremaStaticka.Remove(oprema);
        }

        private void DatumPremjestaja_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (termini.Count != 0)
            {
                azurirajTermine();
            }
            podesiDugme();
        }

        private void azurirajTermine()
        {
            if (DatePicker.SelectedDate == DateTime.Now.Date)
            {
                termini.Clear();
                dodajTermine();
            }
            else
            {
                dodajTermineDrugiDatum();
            }
        }

        private void dodajTermineDrugiDatum()
        {
            string[] prviTermin = termini[0].Split(':');
            string prvi = prviTermin[0];
            for (int termin = int.Parse(prvi); termin > 0; termin--)
            {
                if (!postojiTermin(termin))
                {
                    termini.Insert(0, termin + ":00");
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

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            aktivan = false;
        }

        public bool jeBroj(string tekst)
        {
            int test;
            return int.TryParse(tekst, out test);
        }

        private void podesiDugme()
        {
            if (this.Kolicina != null)
            {
                if (jeBroj(this.Kolicina.Text))
                {
                    izvrsiPodesavanje();
                }
                else
                {
                    this.Potvrdi.IsEnabled = false;
                }
            }
        }

        private void izvrsiPodesavanje()
        {
            if(int.Parse(this.Kolicina.Text) > dozvoljenaKolicina || int.Parse(this.Kolicina.Text) <= 0 || this.sale.SelectedItem == null || this.vrijeme.SelectedItem == null)
            {
                this.Potvrdi.IsEnabled = false;
            }
            else if (int.Parse(this.Kolicina.Text) <= dozvoljenaKolicina && int.Parse(this.Kolicina.Text) > 0 && this.sale.SelectedItem != null && this.vrijeme.SelectedItem != null)
            {
                this.Potvrdi.IsEnabled = true;
            }
        }

        private void sale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            podesiDugme();
        }

        private void vrijeme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            podesiDugme();
        }

        private void Kolicina_TextChanged(object sender, TextChangedEventArgs e)
        {
            podesiDugme();
        }
    }
}
