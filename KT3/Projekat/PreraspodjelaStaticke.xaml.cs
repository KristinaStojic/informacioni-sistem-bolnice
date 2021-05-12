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
    /// Interaction logic for PreraspodjelaStaticke.xaml
    /// </summary>
    public partial class PreraspodjelaStaticke : Window, INotifyPropertyChanged
    {
        public static Oprema izabranaOprema;
        public Sala salaDodavanje;
        public static bool aktivna;
        public ObservableCollection<Sala> sale { get; set; }
        public ObservableCollection<Oprema> staticka { get; set; }
        public ObservableCollection<string> termini { get; set; }
        public int validacija;
        public static int dozvoljenaKolicina;

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

        public PreraspodjelaStaticke(Sala izabranaSala)
        {
            InitializeComponent();
            inicijalizujElemente(izabranaSala);
            dodajStaticku();
            dodajTermine();
        }

        private void inicijalizujElemente(Sala izabranaSala)
        {
            termini = new ObservableCollection<string>();
            this.salaDodavanje = izabranaSala;
            this.DataContext = this;
            staticka = new ObservableCollection<Oprema>();
            sale = new ObservableCollection<Sala>();
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
            foreach (Premjestaj p in PremjestajMenadzer.premjestaji)
            {
                if (p.datumIVrijeme.Hour.ToString().Equals(termin.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        private void dodajStaticku()
        {
            dodajStatickuIzSkladista();
            dodajStatickuIzSala();
        }

        private void dodajStatickuIzSkladista()
        {
            foreach (Oprema o in OpremaMenadzer.oprema)
            {
                if (o.Staticka)
                {
                    staticka.Add(o);
                }
            }
        }

        private void dodajStatickuIzSala()
        {
            foreach (Sala sala in SaleMenadzer.sale)
            {
                foreach (Oprema oprema in sala.Oprema)
                {
                    dodajOpremu(oprema);
                }
            }
        }

        private void dodajOpremu(Oprema oprema)
        {
            if (oprema.Staticka)
            {
                if (!postojiOprema(oprema))
                {
                    staticka.Add(oprema);
                }
            }
        }

        private bool postojiOprema(Oprema oprema)
        {
            foreach (Oprema statickaOprema in staticka)
            {
                if (statickaOprema.IdOpreme == oprema.IdOpreme)
                {
                    return true;
                }
            }
            return false;
        }

        private void oprema_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PreraspodjelaStaticke.izabranaOprema = (Oprema)kombo.SelectedItem;
            azurirajSale(izabranaOprema);
            podesiDugme();
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
                if (sala.Id != salaDodavanje.Id && provjeriPreostalo(oprema, sala))
                {
                    sale.Add(sala);
                }
            }
        }

        private bool provjeriPreostalo(Oprema opremaZaSlanje, Sala izabranaSala)
        {
            if (nadjiDozvoljenuKolicinu(opremaZaSlanje, izabranaSala) <= 0) {return false;}
            else {return true;}
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

        private void azurirajKolicinu(Sala izabranaSala)
        {
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (izabranaSala.Id == sala.Id)
                {
                    izracunajDozvoljenuKolicinu(sala, izabranaSala);
                }
            }
        }

        private void izracunajDozvoljenuKolicinu(Sala sala, Sala izabranaSala)
        {
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    dozvoljenaKolicina = nadjiDozvoljenuKolicinu(oprema, izabranaSala);
                    this.tekst.Text = "MAX:" + dozvoljenaKolicina;

                }
            }
        }

        private int nadjiDozvoljenuKolicinu(Oprema oprema, Sala sala)
        {
            int kolicina = oprema.Kolicina;
            foreach (Premjestaj pm in PremjestajMenadzer.premjestaji)
            {
                if (pm.izSale.Id == sala.Id && pm.oprema.IdOpreme == oprema.IdOpreme)
                {
                    kolicina -= pm.kolicina;
                }
            }
            return kolicina;
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            aktivna = false;
            this.Close();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            PreraspodjelaStaticke.izabranaOprema = (Oprema)kombo.SelectedItem;
            if (napraviTerminPremjestaja().Date.ToString().Equals(DateTime.Now.Date.ToString()))
            {
                provjeriPremjestaj();
            }
            else
            {
                zakaziPremjestaj((Sala)komboSale.SelectedItem, int.Parse(Kolicina.Text), napraviTerminPremjestaja());
            }
            zavrsiPremjestaj();
        }

        private void provjeriPremjestaj()
        {
            if (napraviTerminPremjestaja().TimeOfDay <= DateTime.Now.TimeOfDay)
            {
                premjestiOpremu((Sala)komboSale.SelectedItem, int.Parse(Kolicina.Text));
            }
            else
            {
                zakaziPremjestaj((Sala)komboSale.SelectedItem, int.Parse(Kolicina.Text), napraviTerminPremjestaja());
            }
        }

        private void zavrsiPremjestaj()
        {
            this.Close();
            aktivna = false;
            PremjestajMenadzer.sacuvajIzmjene();
            SaleMenadzer.sacuvajIzmjene();
        }

        private void premjestiOpremu(Sala izabranaSala, int kolicina)
        {
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.Id == izabranaSala.Id)
                {
                    ukloniOpremuIzSale(s, kolicina);
                }
                if (s.Id == salaDodavanje.Id)
                {
                    dodajOpremuUSalu(s, kolicina);

                }
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
            return  new DateTime(int.Parse(godina), int.Parse(mjesec), int.Parse(dan), int.Parse(sat), int.Parse(minuti), 0);
        }

        private void zakaziPremjestaj(Sala izabranaSala, int kolicina, DateTime datumIVrijeme)
        {
            Premjestaj zakazi = new Premjestaj();
            zakazi.kolicina = kolicina;
            definisiSale(izabranaSala, zakazi);
            definisiOpremu(zakazi);
            dodajOpremuIzSala(zakazi);
            dodajPremjestaj(zakazi, datumIVrijeme);
        }

        private void dodajPremjestaj(Premjestaj zakazi, DateTime datumIVrijeme)
        {
            zakazi.datumIVrijeme = datumIVrijeme;
            PremjestajMenadzer.dodajPremjestaj(zakazi);
        }

        private void dodajOpremuIzSala(Premjestaj zakazi)
        {
            if (zakazi.oprema == null)
            {
                foreach (Sala sala in SaleMenadzer.sale)
                {
                    dodajOpremu(sala, zakazi);
                }
            }
        }

        private void dodajOpremu(Sala sala, Premjestaj zakazi)
        {
            foreach (Oprema oprema in sala.Oprema)
            {
                if (izabranaOprema.IdOpreme == oprema.IdOpreme)
                {
                    zakazi.oprema = oprema;
                }
            }
        }

        private void definisiOpremu(Premjestaj zakazi)
        {
            foreach (Oprema o in OpremaMenadzer.oprema)
            {
                if (izabranaOprema.IdOpreme == o.IdOpreme)
                {
                    zakazi.oprema = o;
                }
            }
        }

        private void definisiSale(Sala izabranaSala, Premjestaj zakazi)
        {
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.Id == izabranaSala.Id)
                {
                    zakazi.izSale = s;
                }
                if (s.Id == salaDodavanje.Id)
                {
                    zakazi.uSalu = s;
                }
            }
        }
    
        private void dodajOpremuUSalu(Sala s, int kolicina)
        {
            if (prebaciOpremu(s, kolicina) == 0)
            {
                dodajOpremu(s, kolicina);
            }
        }

        private void dodajOpremu(Sala sala, int kolicina)
        {
            Oprema oprema = new Oprema(izabranaOprema.NazivOpreme, kolicina, true);
            oprema.IdOpreme = izabranaOprema.IdOpreme;
            sala.Oprema.Add(oprema);
            if (salaDodavanje.Namjena.Equals("Skladiste"))
            {
                dodajStaticku(oprema);
            }
            else
            {
                azurirajPrikaz();
            }
        }

        private void dodajStaticku(Oprema oprema)
        {
            if (Skladiste.OpremaStaticka != null)
            {
                Skladiste.OpremaStaticka.Add(oprema);
            }
        }

        private void azurirajPrikaz()
        {
            if (PrikazStaticke.otvoren)
            {
                PrikazStaticke.azurirajPrikaz();
            }
        }

        private int prebaciOpremu(Sala s, int kolicina)
        {
            int x = 0;
            foreach (Oprema o in s.Oprema)
            {
                if (o.IdOpreme == izabranaOprema.IdOpreme)
                {
                    o.Kolicina += kolicina;
                    x += 1;
                    ukloniOpremu(s, o);
                }
            }
            return x;
        }

        private void prebaciUSkladiste(Oprema oprema)
        {
            int idx = Skladiste.OpremaStaticka.IndexOf(oprema);
            Skladiste.OpremaStaticka.RemoveAt(idx);
            Skladiste.OpremaStaticka.Insert(idx, oprema);
        }

        private void ukloniOpremu(Sala sala, Oprema oprema)
        {
            if (sala.Namjena.Equals("Skladiste"))
            {
                if (Skladiste.OpremaStaticka != null)
                {
                    prebaciUSkladiste(oprema);
                }
            }
            else
            {
                azurirajPrikaz();
            }
        }

        private void ukloniOpremuIzSale(Sala s, int kolicina)
        {
            foreach (Oprema oprema in s.Oprema.ToArray())
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    oprema.Kolicina -= kolicina;
                    if (s.Namjena.Equals("Skladiste"))
                    {
                        prebaciOpremuIzSkladista(oprema, s);
                    }
                    else
                    {
                        azurirajPrikazStaticke(oprema, s);
                    }
                }
            }
        }

        private void azurirajPrikazStaticke(Oprema o, Sala s)
        {
            if (o.Kolicina == 0)
            {
                s.Oprema.Remove(o);
                if (PrikazStaticke.otvoren)
                {
                    PrikazStaticke.azurirajPrikaz();
                }
            }
            if (PrikazStaticke.otvoren)
            {
                PrikazStaticke.azurirajPrikaz();
            }
        }

        private void prebaciOpremuIzSkladista(Oprema oprema, Sala sala)
        {
            if (oprema.Kolicina == 0)
            {
                if (Skladiste.OpremaStaticka != null)
                {
                    sala.Oprema.Remove(oprema);
                    Skladiste.OpremaStaticka.Remove(oprema);
                }
            }
            else
            {
                if (Skladiste.OpremaStaticka != null)
                {
                    prebaciUSkladiste(oprema);
                }
            }
        }

        private void dodajTermineZaDanas()
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

        private void dodajTermineDrugiDan()
        {
            string[] terminPremjestaja = termini[0].Split(':');
            string prviTermin = terminPremjestaja[0];
            for (int termin = int.Parse(prviTermin); termin > 0; termin--)
            {
                if (!postojiTermin(termin))
                {
                    termini.Insert(0, termin + ":00");
                }
            }
        }

        private void DatumPremjestaja_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (termini != null)
            {
                if (DatePicker.SelectedDate == DateTime.Now.Date)
                {
                    dodajTermineZaDanas();
                }
                else
                {
                    dodajTermineDrugiDan(); 
                }
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
            if (this.Kolicina != null)
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
        }

        private void postaviDugme()
        {
            if (int.Parse(this.Kolicina.Text) > dozvoljenaKolicina || int.Parse(this.Kolicina.Text) <= 0 || this.kombo.SelectedItem == null || this.komboSale.SelectedItem == null || this.vrijeme.SelectedItem == null)
            {
                this.Potvrdi.IsEnabled = false;
            }
            else if (int.Parse(this.Kolicina.Text) <= dozvoljenaKolicina && int.Parse(this.Kolicina.Text) > 0 && this.kombo.SelectedItem != null && this.komboSale.SelectedItem != null && this.vrijeme.SelectedItem != null)
            {
                this.Potvrdi.IsEnabled = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            aktivna = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
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
