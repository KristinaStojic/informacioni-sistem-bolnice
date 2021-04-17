using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SlanjeStaticke.xaml
    /// </summary>
    public partial class SlanjeStaticke : Window
    {
        public ObservableCollection<Sala> Sale { get; set; }
        public ObservableCollection<string> termini { get; set; }
        Oprema opremaZaSlanje;
        Sala salaIzKojeSaljem;
        public SlanjeStaticke(Sala izabranaSala, Oprema opremaZaSlanje)
        {
            termini = new ObservableCollection<string>();
            InitializeComponent();
            this.opremaZaSlanje = opremaZaSlanje;
            this.salaIzKojeSaljem = izabranaSala;
            this.oprema.Text = opremaZaSlanje.NazivOpreme;
            this.DataContext = this;
            Sale = new ObservableCollection<Sala>();
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.Id != izabranaSala.Id)
                {
                    Sale.Add(s);
                }
            }
            int x = 0;
            for (int i = (int)DateTime.Now.Hour + 1; i <= 23; i++)
            {
                x = 0;
                foreach(Premjestaj p in PremjestajMenadzer.premjestaji)
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
            this.maks.Text = "MAX: " + opremaZaSlanje.Kolicina.ToString();
            termini.Add("12:30");
            termini.Add("12:31");
            termini.Add("12:32");
            termini.Add("12:33");
            termini.Add("12:34");
            termini.Add("12:35");
            termini.Add("12:36");
            termini.Add("12:37");
            termini.Add("12:38");
            termini.Add("12:39");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                        if (s.Id == salaIzKojeSaljem.Id)
                        {
                            foreach (Oprema o in s.Oprema)
                            {
                                if (o.IdOpreme == opremaZaSlanje.IdOpreme)
                                {
                                    if (o.Kolicina - kolicina == 0)
                                    {
                                        s.Oprema.Remove(o);
                                        PrikazStaticke.OpremaStaticka.Remove(o);
                                        break;
                                    }
                                    else
                                    {
                                        o.Kolicina -= kolicina;
                                        int idx = PrikazStaticke.OpremaStaticka.IndexOf(o);
                                        PrikazStaticke.OpremaStaticka.RemoveAt(idx);
                                        PrikazStaticke.OpremaStaticka.Insert(idx, o);
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
                        if (s.Id == salaIzKojeSaljem.Id)
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
                    zakazi.salji = true;
                    PremjestajMenadzer.dodajPremjestaj(zakazi);
                }
            }
            else
            {
                Premjestaj zakazi = new Premjestaj();
                zakazi.kolicina = kolicina;
                foreach (Sala s in SaleMenadzer.sale)
                {
                    if (s.Id == salaIzKojeSaljem.Id)
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
                zakazi.salji = true;
                PremjestajMenadzer.dodajPremjestaj(zakazi);
            }
            this.Close();
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
        }
    }
}
