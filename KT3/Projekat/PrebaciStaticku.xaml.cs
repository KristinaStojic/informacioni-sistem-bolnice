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
    /// Interaction logic for PrebaciStaticku.xaml
    /// </summary>
    public partial class PrebaciStaticku : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Sala> Sale { get; set; }
        public ObservableCollection<string> termini { get; set; }
        public static bool aktivan;
        Oprema opremaZaSlanje;
        public DateTime datumIVrijemeSlanja;
        public int validacija;
        public static int dozvoljenaKolicina;

        public int Validacija
        {
            get{return validacija;}
            set
            {
                if(value != validacija)
                {
                    validacija = value;
                    OnPropertyChanged("Validacija");
                }
            }
        }

        public PrebaciStaticku(Oprema oprema)
        {
            InitializeComponent();
            inicijalizujElemente(oprema);
            dodajSale();
            dodajTerminePocetak();
            postaviMax();
        }

        private void inicijalizujElemente(Oprema oprema)
        {
            termini = new ObservableCollection<string>();
            this.opremaZaSlanje = oprema;
            this.oprema.Text = opremaZaSlanje.NazivOpreme;
            this.DataContext = this;
        }

        private void dodajSale()
        {
            Sale = new ObservableCollection<Sala>();
            foreach (Sala s in SaleServis.Sale())
            {
                if (!s.Namjena.Equals("Skladiste"))
                {
                    Sale.Add(s);
                }
            }
        }

        private void dodajTerminePocetak()
        {
            for (int termin = (int)DateTime.Now.Hour + 1; termin <= 23; termin++)
            {
                if (!zauzetTermin(termin))
                {
                    termini.Add(termin + ":00");
                }
            }
        }

        private bool zauzetTermin(int termin)
        {
            foreach (Premjestaj premjestaj in PremjestajServis.Premjestaji())
            {
                if (premjestaj.datumIVrijeme.Hour.ToString().Equals(termin.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        public static void ukloniOpremuIzSkladista(Sala sala, Oprema oprema)
        {
            if (Skladiste.OpremaStaticka != null)
            {
                sala.Oprema.Remove(oprema);
                Skladiste.OpremaStaticka.Remove(oprema);
            }
        }

        public static void zamjeniOpremuUSkladistu(Oprema oprema)
        {
            if (Skladiste.OpremaStaticka != null)
            {
                int idx = Skladiste.OpremaStaticka.IndexOf(oprema);
                Skladiste.OpremaStaticka.RemoveAt(idx);
                Skladiste.OpremaStaticka.Insert(idx, oprema);
            }
        }

        public static void dodajOpremuUSkladiste(Oprema oprema)
        {
            if (Skladiste.OpremaStaticka != null)
            {
                Skladiste.OpremaStaticka.Add(oprema);
            }
        }

        private void postaviMax()
        {
            dozvoljenaKolicina = nadjidozvoljenuKolicinu();
            this.maks.Text = "MAX: " + dozvoljenaKolicina.ToString();
        }

        private int nadjidozvoljenuKolicinu()
        {
            int kolicina = opremaZaSlanje.Kolicina;
            foreach (Premjestaj premjestaj in PremjestajServis.Premjestaji())
            {
                if (premjestaj.izSale.Id == 4 && premjestaj.oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    kolicina -= premjestaj.kolicina;
                }
            }
            return kolicina;
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            aktivan = false;
        }

        private DateTime napraviTerminPremjestaja()
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
            Sala salaUKojuSaljem = (Sala)sale.SelectedItem;
            int kolicina = int.Parse(Kolicina.Text);
            DateTime datumIVrijeme = napraviTerminPremjestaja();
            PremjestajServis.odradiPremjestaj(datumIVrijeme, salaUKojuSaljem, kolicina, opremaZaSlanje);
            this.Close();
            aktivan = false;
        }

        private void datumPremjestaja_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (termini != null)
            {
                if (DatePicker.SelectedDate == DateTime.Now.Date)
                {
                    dodajTermineDanas();
                }
                else
                {
                    dodajTermine();
                }
            }
        }

        private void dodajTermineDanas()
        {
            termini.Clear();
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

        private void dodajTermine()
        {
            int x = 0;
            string[] t = termini[0].Split(':');
            string prvi = t[0];
            for (int termin = int.Parse(prvi) - 1; termin > 0; termin--)
            {
                if (!postojiTermin(termin))
                {
                    termini.Insert(0, termin + ":00");
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PremjestajServis.sacuvajIzmjene();
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

        private void Kolicina_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void sale_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                this.potvrda.IsEnabled = false;
            }
        }

        private void izvrsiPostavljanje()
        {
            if (int.Parse(this.Kolicina.Text) > dozvoljenaKolicina || int.Parse(this.Kolicina.Text) <= 0 || this.sale.SelectedItem == null)
            {
                this.potvrda.IsEnabled = false;
            }
            else if (int.Parse(this.Kolicina.Text) <= dozvoljenaKolicina && int.Parse(this.Kolicina.Text) > 0 && this.sale.SelectedItem != null)
            {
                this.potvrda.IsEnabled = true;
            }
        }
    }
}
