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
            Sale = new ObservableCollection<Sala>();
            dodajSale();
            dodajTerminePocetak();
            postaviMax();
        }

        private void dodajSale()
        {
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
            int x = 0;
            for (int i = (int)DateTime.Now.Hour + 1; i <= 23; i++)
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
                    termini.Add(i + ":00");
                }
            }
        }

        private void postaviMax()
        {
            int kolicina = opremaZaSlanje.Kolicina;
            foreach (Premjestaj pm in PremjestajMenadzer.premjestaji)
            {
                if (pm.izSale.Id == 4 && pm.oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    kolicina -= pm.kolicina;
                }
            }
            this.maks.Text = "MAX: " + kolicina.ToString();
            dozvoljenaKolicina = kolicina;
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
            int x = 0;
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
            DateTime datumIVrijeme = new DateTime(int.Parse(godina), int.Parse(mjesec), int.Parse(dan), int.Parse(sat), int.Parse(minuti), 0);
          
            if (datumIVrijeme.Date.ToString().Equals(DateTime.Now.Date.ToString()))
            {
                if (datumIVrijeme.TimeOfDay <= DateTime.Now.TimeOfDay)
                {
                    foreach (Sala s in SaleMenadzer.sale)
                    {
                        if (s.Namjena.Equals("Skladiste"))
                        {
                            foreach (Oprema o in s.Oprema)
                            {
                                if (o.IdOpreme == opremaZaSlanje.IdOpreme)
                                {
                                    if (o.Kolicina - kolicina == 0)
                                    {
                                        s.Oprema.Remove(o);
                                        Skladiste.OpremaStaticka.Remove(o);
                                        break;
                                    }
                                    else
                                    {
                                        o.Kolicina -= kolicina;
                                        int idx = Skladiste.OpremaStaticka.IndexOf(o);
                                        Skladiste.OpremaStaticka.RemoveAt(idx);
                                        Skladiste.OpremaStaticka.Insert(idx, o);
                                    }

                                }
                            }
                        }
                        if (s.Id == salaUKojuSaljem.Id)
                        {
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
                    }
                }
                else
                {
                    Premjestaj zakazi = new Premjestaj();
                    zakazi.kolicina = kolicina;
                    foreach (Sala s in SaleMenadzer.sale)
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
                    foreach (Oprema o in OpremaMenadzer.oprema)
                    {
                        if (opremaZaSlanje.IdOpreme == o.IdOpreme)
                        {
                            zakazi.oprema = o;
                        }
                    }
                    if(zakazi.oprema == null)
                    {
                        foreach(Sala s in SaleMenadzer.sale)
                        {
                            foreach(Oprema o in s.Oprema)
                            {
                                if (opremaZaSlanje.IdOpreme == o.IdOpreme)
                                {
                                    zakazi.oprema = o;
                                }
                            }
                        }
                    }
                    zakazi.datumIVrijeme = datumIVrijeme;
                    //zakazi.salji = true;
                    PremjestajMenadzer.dodajPremjestaj(zakazi);
                }
            }
            else
            {
                Premjestaj zakazi = new Premjestaj();
                zakazi.kolicina = kolicina;
                foreach (Sala s in SaleMenadzer.sale) {
                    if (s.Namjena.Equals("Skladiste")) {
                        zakazi.izSale = s;
                    }
                    if(s.Id == salaUKojuSaljem.Id)
                    {
                        zakazi.uSalu = s;
                    }
                }
                foreach(Oprema o in OpremaMenadzer.oprema)
                {
                    if(opremaZaSlanje.IdOpreme == o.IdOpreme)
                    {
                        zakazi.oprema = o;
                    }
                }
                if (zakazi.oprema == null)
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
                zakazi.datumIVrijeme = datumIVrijeme;
                //zakazi.salji = true;
                PremjestajMenadzer.dodajPremjestaj(zakazi);
            }
            this.Close();
            aktivan = false;
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
