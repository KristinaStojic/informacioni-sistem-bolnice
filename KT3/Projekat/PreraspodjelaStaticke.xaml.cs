﻿using Model;
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
            this.DataContext = this;
            staticka = new ObservableCollection<Oprema>();
            sale = new ObservableCollection<Sala>();
            this.salaDodavanje = izabranaSala;
            dodajStaticku();
            dodajTermine();
        }

        private void dodajTermine()
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
            if (kolicina == 0)
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
        private void azurirajKolicinu(Sala s)
        {
            int kolicina;
            foreach (Sala sal in SaleMenadzer.sale)
            {
                if (s.Id == sal.Id)
                {
                    foreach (Oprema o in sal.Oprema)
                    {
                        if (o.IdOpreme == izabranaOprema.IdOpreme)
                        {
                            kolicina = o.Kolicina;
                            foreach (Premjestaj pm in PremjestajMenadzer.premjestaji)
                            {
                                if (pm.izSale.Id == s.Id)
                                {
                                    kolicina -= pm.kolicina;
                                }
                            }
                            this.tekst.Text = "MAX:" + kolicina.ToString();
                            dozvoljenaKolicina = kolicina;
                        }
                    }
                }
            }
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
            int x = 0;
            PreraspodjelaStaticke.izabranaOprema = (Oprema)kombo.SelectedItem;
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
                        if (s.Id == izabranaSala.Id)
                        {
                            foreach (Oprema o in s.Oprema)
                            {
                                if (o.IdOpreme == izabranaOprema.IdOpreme)
                                {
                                    o.Kolicina -= kolicina;
                                    if (s.Namjena.Equals("Skladiste"))
                                    {
                                        if (o.Kolicina == 0)
                                        {
                                            if (Skladiste.OpremaStaticka != null)
                                            {
                                                s.Oprema.Remove(o);
                                                Skladiste.OpremaStaticka.Remove(o);
                                                break;
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
                                    else
                                    {
                                        if (o.Kolicina == 0)
                                        {
                                            /* if (PrikazStaticke.OpremaStaticka != null)
                                             {*/
                                            s.Oprema.Remove(o);
                                            if (PrikazStaticke.otvoren)
                                            {
                                                PrikazStaticke.azurirajPrikaz();
                                            }
                                            break;
                                            //}
                                        }
                                        if (PrikazStaticke.otvoren)
                                        {
                                            PrikazStaticke.azurirajPrikaz();
                                        }
                                    }
                                }
                            }
                        }
                        if (s.Id == salaDodavanje.Id)
                        {
                            foreach (Oprema o in s.Oprema)
                            {
                                if (o.IdOpreme == izabranaOprema.IdOpreme)
                                {
                                    o.Kolicina += kolicina;
                                    x += 1;
                                    if (s.Namjena.Equals("Skladiste"))
                                    {
                                        if (Skladiste.OpremaStaticka != null)
                                        {
                                            int idx = Skladiste.OpremaStaticka.IndexOf(o);
                                            Skladiste.OpremaStaticka.RemoveAt(idx);
                                            Skladiste.OpremaStaticka.Insert(idx, o);
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


                            }
                            if (x == 0)
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
                                    /*if (PrikazStaticke.OpremaStaticka != null)
                                    {*/
                                    if (PrikazStaticke.otvoren)
                                    {
                                        PrikazStaticke.azurirajPrikaz();
                                    }
                                    //}
                                }
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
                        if (s.Id == izabranaSala.Id)
                        {
                            zakazi.izSale = s;
                        }
                        if (s.Id == salaDodavanje.Id)
                        {
                            zakazi.uSalu = s;
                        }
                    }
                    foreach (Oprema o in OpremaMenadzer.oprema)
                    {
                        if (izabranaOprema.IdOpreme == o.IdOpreme)
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
                                if (izabranaOprema.IdOpreme == o.IdOpreme)
                                {
                                    zakazi.oprema = o;
                                }
                            }
                        }
                    }
                    //zakazi.salji = false;
                    zakazi.datumIVrijeme = datumIVrijeme;
                    PremjestajMenadzer.dodajPremjestaj(zakazi);
                }
            }
            else
            {
                Premjestaj zakazi = new Premjestaj();
                zakazi.kolicina = kolicina;
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
                foreach (Oprema o in OpremaMenadzer.oprema)
                {
                    if (izabranaOprema.IdOpreme == o.IdOpreme)
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
                            if (izabranaOprema.IdOpreme == o.IdOpreme)
                            {
                                zakazi.oprema = o;
                            }
                        }
                    }
                }
                //zakazi.salji = false;
                zakazi.datumIVrijeme = datumIVrijeme;
                PremjestajMenadzer.dodajPremjestaj(zakazi);
            }
            this.Close();
            aktivna = false;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (termini.Count != 0)
            {
                if (DatePicker.SelectedDate == DateTime.Now.Date)
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
                else
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
