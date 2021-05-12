using Model;
using Projekat.Model;
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
            foreach (Sala s in SaleMenadzer.sale)
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
            foreach (Premjestaj premjestaj in PremjestajMenadzer.premjestaji)
            {
                if (premjestaj.datumIVrijeme.Hour.ToString().Equals(termin.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        private void postaviMax()
        {
            dozvoljenaKolicina = nadjidozvoljenuKolicinu();
            this.maks.Text = "MAX: " + dozvoljenaKolicina.ToString();
        }

        private int nadjidozvoljenuKolicinu()
        {
            int kolicina = opremaZaSlanje.Kolicina;
            foreach (Premjestaj premjestaj in PremjestajMenadzer.premjestaji)
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

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            Sala salaUKojuSaljem = (Sala)sale.SelectedItem;
            int kolicina = int.Parse(Kolicina.Text);
            DateTime datumIVrijeme = napraviTerminPremjestaja();
            odradiPremjestaj(datumIVrijeme, salaUKojuSaljem, kolicina);
        }

        private void odradiPremjestaj(DateTime datumIVrijeme, Sala salaUKojuSaljem, int kolicina)
        {
            if (datumIVrijeme.Date.ToString().Equals(DateTime.Now.Date.ToString()))
            {
                odradiPremjestajSada(datumIVrijeme, salaUKojuSaljem, kolicina);
            }
            else
            {
                zakaziPremjestaj(salaUKojuSaljem, datumIVrijeme, kolicina);
            }
            zavrsiPremjestaj();
        }

        private void odradiPremjestajSada(DateTime datumIVrijeme, Sala salaUKojuSaljem, int kolicina)
        {
            if (datumIVrijeme.TimeOfDay <= DateTime.Now.TimeOfDay)
            {
                odradiPremjestaj(salaUKojuSaljem, kolicina);
            }
            else
            {
                zakaziPremjestaj(salaUKojuSaljem, datumIVrijeme, kolicina);
            }
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

        private void zavrsiPremjestaj()
        {
            this.Close();
            aktivan = false;
            SaleMenadzer.sacuvajIzmjene();
            PremjestajMenadzer.sacuvajIzmjene();
        }

        private void zakaziPremjestaj(Sala salaUKojuSaljem, DateTime datumIVrijeme,int kolicina)
        {
            Premjestaj zakazi = new Premjestaj();
            zakazi.kolicina = kolicina;
            definisiElemente(zakazi, salaUKojuSaljem);
            if (zakazi.oprema == null)
            {
                nadjiOpremuUSalama(zakazi);
            }
            dodajPremjestaj(zakazi, datumIVrijeme);
        }

        private void definisiElemente(Premjestaj zakazi, Sala salaUKojuSaljem)
        {
            foreach (Sala sala in SaleMenadzer.sale)
            {
                definisiSale(zakazi, sala, salaUKojuSaljem);
            }
            foreach (Oprema oprema in OpremaMenadzer.oprema)
            {
                definisiOpremu(zakazi, oprema);
            }
        }

        private void dodajPremjestaj(Premjestaj zakazi, DateTime datumIVrijeme)
        {
            zakazi.datumIVrijeme = datumIVrijeme;
            PremjestajMenadzer.dodajPremjestaj(zakazi);
        }

        private void nadjiOpremuUSalama(Premjestaj zakazi)
        {
            foreach (Sala s in SaleMenadzer.sale)
            {
                foreach (Oprema o in s.Oprema)
                {
                    if (opremaZaSlanje.IdOpreme == o.IdOpreme)
                    {
                        zakazi.oprema = o;
                    }
                }
            }
        }

        private void definisiOpremu(Premjestaj zakazi, Oprema o)
        {
            if (opremaZaSlanje.IdOpreme == o.IdOpreme)
            {
                zakazi.oprema = o;
            }
        }

        private void definisiSale(Premjestaj zakazi, Sala s, Sala salaUKojuSaljem)
        {
            if (s.Namjena.Equals("Skladiste"))
            {
                zakazi.izSale = s;
            }
            if (s.Id == salaUKojuSaljem.Id)
            {
                zakazi.uSalu = s;
            }
        }

        private void odradiPremjestaj(Sala salaUKojuSaljem, int kolicina)
        {
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.Namjena.Equals("Skladiste"))
                {
                    ukloniOpremuIzSale(s, kolicina);
                }
                if (s.Id == salaUKojuSaljem.Id)
                {
                    dodajOpremuUSalu(s, kolicina);
                }
            }
        }

        private void ukloniOpremuIzSale(Sala sala, int kolicina)
        {
            foreach (Oprema oprema in sala.Oprema.ToArray())
            {
                if (oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    smanjiKolicinuOpreme(kolicina, oprema, sala);
                }
            }
        }

        private void smanjiKolicinuOpreme(int kolicina, Oprema oprema, Sala sala)
        {
            if (oprema.Kolicina - kolicina == 0)
            {
                ukloniOpremu(sala, oprema);
            }
            else
            {
                smanjiKolicinu(oprema, kolicina);
            }
        }

        private void smanjiKolicinu(Oprema oprema, int kolicina)
        {
            oprema.Kolicina -= kolicina;
            int idx = Skladiste.OpremaStaticka.IndexOf(oprema);
            Skladiste.OpremaStaticka.RemoveAt(idx);
            Skladiste.OpremaStaticka.Insert(idx, oprema);
        }

        private void ukloniOpremu(Sala sala, Oprema oprema)
        {
            sala.Oprema.Remove(oprema);
            Skladiste.OpremaStaticka.Remove(oprema);
        }

        private void dodajOpremuUSalu(Sala s, int kolicina)
        {
            napraviNovuOpremu(dodajOpremu(s, kolicina), kolicina, s);
        }

        private bool dodajOpremu(Sala sala, int kolicina)
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

        private void napraviNovuOpremu(bool postojiOprema, int kolicina, Sala sala)
        {
            if (!postojiOprema)
            {
                Oprema oprema = new Oprema(opremaZaSlanje.NazivOpreme, kolicina, true);
                oprema.IdOpreme = opremaZaSlanje.IdOpreme;
                sala.Oprema.Add(oprema);
            }
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
            PremjestajMenadzer.sacuvajIzmjene();
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
