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
        public PreraspodjelaStaticke(Sala izabranaSala)
        {
            termini = new ObservableCollection<string>();
            InitializeComponent();
            this.salaDodavanje = izabranaSala;
            inicijalizujElemente();
            dodajStaticku();
            dodajTermine();
        }

        private void inicijalizujElemente()
        {
            this.DataContext = this;
            staticka = new ObservableCollection<Oprema>();
            sale = new ObservableCollection<Sala>();
        }

        private void dodajTermine()
        {
            bool postojiTermin;
            for (int i = (int)DateTime.Now.Hour + 1; i <= 23; i++)
            {
                postojiTermin = false;
                foreach (Premjestaj p in PremjestajMenadzer.premjestaji)
                {
                    if (p.datumIVrijeme.Hour.ToString().Equals(i.ToString()))
                    {
                        postojiTermin = true;
                    }
                }
                if (!postojiTermin)
                {
                    termini.Add(i + ":00");
                }
            }
            termini.Add("18:58");
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
            bool ima = false;
            foreach (Sala s in SaleMenadzer.sale)
            {
                foreach (Oprema op in s.Oprema)
                {
                    ima = false;
                    if (op.Staticka)
                    {
                        foreach (Oprema ops in staticka)
                        {
                            if (ops.IdOpreme == op.IdOpreme)
                            {
                                ima = true;
                            }
                        }
                        if (!ima)
                        {
                            staticka.Add(op);
                        }
                    }
                }
            }
        }

        private void kombo_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            PreraspodjelaStaticke.izabranaOprema = (Oprema)kombo.SelectedItem;
            azurirajSale(izabranaOprema);
            podesiDugme();
        }

        private void azurirajSale(Oprema izabranaOprema)
        {
            sale.Clear();
            foreach (Sala s in SaleMenadzer.sale)
            {
                foreach (Oprema o in s.Oprema)
                {
                    if (o.IdOpreme == izabranaOprema.IdOpreme)
                    {
                        if (s.Id != salaDodavanje.Id && provjeriPreostalo(o, s))
                        {
                            sale.Add(s);
                        }

                    }
                }
            }

        }

        private bool provjeriPreostalo(Oprema opremaZaSlanje, Sala izabranaSala)
        {
            int kolicina = opremaZaSlanje.Kolicina;
            foreach (Premjestaj pm in PremjestajMenadzer.premjestaji)
            {
                if (pm.izSale.Id == izabranaSala.Id && pm.oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    kolicina -= pm.kolicina;
                }
            }
            if (kolicina <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
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
                if (pm.izSale.Id == sala.Id)
                {
                    kolicina -= pm.kolicina;
                }
            }
            return kolicina;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            aktivna = false;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            Sala izabranaSala = (Sala)komboSale.SelectedItem;
            int kolicina = int.Parse(Kolicina.Text);
            PreraspodjelaStaticke.izabranaOprema = (Oprema)kombo.SelectedItem;
            DateTime datumIVrijeme = napraviTerminPremjestaja();
            if (datumIVrijeme.Date.ToString().Equals(DateTime.Now.Date.ToString()))
            {
                if (datumIVrijeme.TimeOfDay <= DateTime.Now.TimeOfDay)
                {
                    premjestiOpremu(izabranaSala, kolicina);
                }
                else
                {
                    zakaziPremjestaj(izabranaSala, kolicina, datumIVrijeme);
                }
            }
            else
            {
                zakaziPremjestaj(izabranaSala, kolicina, datumIVrijeme);
            }
            zavrsiPremjestaj();
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
                foreach (Sala s in SaleMenadzer.sale)
                {
                    foreach (Oprema o in s.Oprema)
                    {
                        if (izabranaOprema.IdOpreme == o.IdOpreme)
                        {
                            zakazi.oprema = o;
                        }
                    }
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

        private void dodajOpremu(Sala s, int kolicina)
        {
            Oprema op = new Oprema(izabranaOprema.NazivOpreme, kolicina, true);
            op.IdOpreme = izabranaOprema.IdOpreme;
            s.Oprema.Add(op);
            if (salaDodavanje.Namjena.Equals("Skladiste"))
            {
                if (Skladiste.OpremaStaticka != null)
                {
                    Skladiste.OpremaStaticka.Add(op);
                }
            }
            else
            {
                if (PrikazStaticke.otvoren)
                {
                    PrikazStaticke.azurirajPrikaz();
                }
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
                if (PrikazStaticke.otvoren)
                {
                    PrikazStaticke.azurirajPrikaz();
                }
            }
        }

        private void ukloniOpremuIzSale(Sala s, int kolicina)
        {
            foreach (Oprema o in s.Oprema)
            {
                if (o.IdOpreme == izabranaOprema.IdOpreme)
                {
                    o.Kolicina -= kolicina;
                    if (s.Namjena.Equals("Skladiste"))
                    {
                        prebaciOpremuIzSkladista(o, s);
                    }
                    else
                    {
                        azurirajPrikazStaticke(o, s);
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

        private void prebaciOpremuIzSkladista(Oprema o, Sala s)
        {
            if (o.Kolicina == 0)
            {
                if (Skladiste.OpremaStaticka != null)
                {
                    s.Oprema.Remove(o);
                    Skladiste.OpremaStaticka.Remove(o);
                }
            }
            else
            {
                if (Skladiste.OpremaStaticka != null)
                {
                    int idx = Skladiste.OpremaStaticka.IndexOf(o);
                    Skladiste.OpremaStaticka.RemoveAt(idx);
                    Skladiste.OpremaStaticka.Insert(idx, o);
                }
            }
        }

        private void dodajTermineZaDanas()
        {
            termini.Clear();
            for (int i = (int)DateTime.Now.Hour + 1; i <= 23; i++)
            {
                int x = 0;
                foreach (Premjestaj p in PremjestajMenadzer.premjestaji)
                {
                    if (p.datumIVrijeme.Hour.ToString().Equals(i.ToString()))
                    {
                        x += 1;
                    }
                }
                if (x == 0)
                {
                    termini.Add(i + ":00");
                }
            }
        }

        private void dodajTermineDrugiDan()
        {
            int x = 0;
            string[] t = termini[0].Split(':');
            string prvi = t[0];
            for (int i = int.Parse(prvi); i > 0; i--)
            {
                x = 0;
                foreach (Premjestaj p in PremjestajMenadzer.premjestaji)
                {
                    if (p.datumIVrijeme.Hour.ToString().Equals(i.ToString()))
                    {
                        x += 1;
                    }
                }
                if (x == 0)
                {
                    termini.Insert(0, i + ":00");
                }
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (termini.Count != 0)
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

        public bool IsNumeric(string input)
        {
            int test;
            return int.TryParse(input, out test);
        }

        private void podesiDugme()
        {
            if (this.Kolicina != null)
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
