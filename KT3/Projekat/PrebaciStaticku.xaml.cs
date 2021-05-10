using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
            get
            {
                return validacija;
            }
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

            termini = new ObservableCollection<string>();
            InitializeComponent();
            this.opremaZaSlanje = oprema;
            this.oprema.Text = opremaZaSlanje.NazivOpreme;
            this.DataContext = this;
            dodajSale();
            dodajTerminePocetak();
            postaviMax();
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
            for (int i = (int)DateTime.Now.Hour + 1; i <= 23; i++)
            {
                if (!zauzetTermin(i))
                {
                    termini.Add(i + ":00");
                }
            }
            termini.Add("12:45");
        }

        private bool zauzetTermin(int termin)
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

        private void postaviMax()
        {
            dozvoljenaKolicina = nadjidozvoljenuKolicinu();
            this.maks.Text = "MAX: " + dozvoljenaKolicina.ToString();
        }

        private int nadjidozvoljenuKolicinu()
        {
            int kolicina = opremaZaSlanje.Kolicina;
            foreach (Premjestaj pm in PremjestajMenadzer.premjestaji)
            {
                if (pm.izSale.Id == 4 && pm.oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    kolicina -= pm.kolicina;
                }
            }
            return kolicina;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            aktivan = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Sala salaUKojuSaljem = (Sala)sale.SelectedItem;
            int kolicina = int.Parse(Kolicina.Text);

            DateTime datumIVrijeme = napraviTerminPremjestaja();
          
            if (datumIVrijeme.Date.ToString().Equals(DateTime.Now.Date.ToString()))
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
            else
            {
                zakaziPremjestaj(salaUKojuSaljem, datumIVrijeme, kolicina);
            }
            zavrsiPremjestaj();
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
            foreach (Sala s in SaleMenadzer.sale)
            {
                definisiSale(zakazi, s, salaUKojuSaljem);
            }
            foreach (Oprema o in OpremaMenadzer.oprema)
            {
                definisiOpremu(zakazi, o);
            }
            if (zakazi.oprema == null)
            {
                nadjiOpremuUSalama(zakazi);
            }
            dodajPremjestaj(zakazi, datumIVrijeme);
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
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    if (oprema.Kolicina - kolicina == 0)
                    {
                        sala.Oprema.Remove(oprema);
                        Skladiste.OpremaStaticka.Remove(oprema);
                        break;
                    }
                    else
                    {
                        oprema.Kolicina -= kolicina;
                        int idx = Skladiste.OpremaStaticka.IndexOf(oprema);
                        Skladiste.OpremaStaticka.RemoveAt(idx);
                        Skladiste.OpremaStaticka.Insert(idx, oprema);
                    }

                }
            }
        }

        private void dodajOpremuUSalu(Sala s, int kolicina)
        {
            int x = 0;
            foreach (Oprema o in s.Oprema)
            {
                if (o.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    o.Kolicina += kolicina;
                    x += 1;
                }
            }
            if (x == 0)
            {
                Oprema op = new Oprema(opremaZaSlanje.NazivOpreme, kolicina, true);
                op.IdOpreme = opremaZaSlanje.IdOpreme;
                s.Oprema.Add(op);
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (termini.Count != 0)
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

        private void dodajTermine()
        {
            int x = 0;
            string[] t = termini[0].Split(':');
            string prvi = t[0];
            for (int i = int.Parse(prvi) - 1; i > 0; i--)
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

        public bool IsNumeric(string input)
        {
            int test;
            return int.TryParse(input, out test);
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
            if (IsNumeric(this.Kolicina.Text))
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
