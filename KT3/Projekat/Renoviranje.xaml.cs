using Model;
using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for Renoviranje.xaml
    /// </summary>
    public partial class Renoviranje : Window
    {
        
        public ObservableCollection<string> terminiPocetak { get; set; }

        public ObservableCollection<string> terminiKraj { get; set; }
        
        public Sala izabranaSala;
        public static Sala novaSala;
        public static Sala salaZaSpajanje;
        public static bool spajanje;
        public static List<Oprema> opremaZaPrebacivanje;
        
        public Renoviranje(Sala izabranaSala)
        {
            InitializeComponent();
            inicijalizujElemente(izabranaSala);
            postaviTerminePocetka();
            postaviTermineKraja();
        }
       
        private void inicijalizujElemente(Sala izabranaSala)
        {
            this.DataContext = this;
            this.izabranaSala = izabranaSala;
            terminiPocetak = new ObservableCollection<string>();
            terminiKraj = new ObservableCollection<string>();
            this.Potvrdi.IsEnabled = false;
        }

        private void DatePickerPocetak_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            azurirajDatumKraja();
            postaviTerminePocetka();
        }

        private void DatePickerKraj_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            azurirajVrijemeKraja();
        }

        private void vrijemePocetka_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            azurirajVrijemeKraja();
            postaviDugme();
        }
        
        private void vrijemeKraja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            postaviDugme();
        }

        private void postaviTerminePocetka()
        {
            if(DatePickerPocetak.SelectedDate == DateTime.Now.Date)
            {
                azurirajVrijemePocetkaIstiDatum();
            }
            else
            {
                azurirajVrijemePocetkaDrugiDatum();
            }
        }

        private void azurirajVrijemeKraja()
        {
            if(DatePickerKraj.SelectedDate == DatePickerPocetak.SelectedDate)
            {
                azurirajVrijemeKrajaIstiDatum();
            }
            else
            {
                azurirajVrijemeKrajaDrugiDatum();
            }
        }

        private void azurirajVrijemePocetkaIstiDatum()
        {
            string datumPocetka = ((DateTime)DatePickerPocetak.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (terminiPocetak != null)
            {
                terminiPocetak.Clear();
                for (int termin = (int)DateTime.Now.Hour + 1; termin <= 23; termin++)
                {
                    dodajTerminPocetka(datumPocetka, termin);
                }
            }

        }

        private void dodajTerminPocetka(string datumPocetka, int termin)
        {
            if (!zauzecaZaDatum(datumPocetka).Contains(termin))
            {
                terminiPocetak.Add(termin + ":00");
            }
        }

        private void dodajTerminKraja(string datumPocetka, int termin)
        {
            if (!zauzecaZaDatum(datumPocetka).Contains(termin))
            {
                terminiKraj.Add(termin + ":00");
            }
        }

        private void azurirajVrijemePocetkaDrugiDatum()
        {
            string datumPocetka = ((DateTime)DatePickerPocetak.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (terminiPocetak != null)
            {
                terminiPocetak.Clear();
                for (int termin = 1; termin <= 23; termin++)
                {
                    dodajTerminPocetka(datumPocetka, termin);
                }
            }
        }

        private void azurirajVrijemeKrajaIstiDatum()
        {
            string datumPocetka = ((DateTime)DatePickerPocetak.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (terminiKraj != null && vrijemePocetka.SelectedItem != null)
            {
                terminiKraj.Clear();
                for (int termin = int.Parse(vrijemePocetka.SelectedItem.ToString().Split(':')[0]) + 1; termin <= prvoSledeceZauzece(vrijemePocetka.SelectedItem.ToString().Split(':')[0], datumPocetka); termin++)
                {
                    dodajTerminKraja(datumPocetka, termin);
                }
            }
        }

        private void azurirajVrijemeKrajaDrugiDatum()
        {
            string datumPocetka = ((DateTime)DatePickerPocetak.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string datumKraja = ((DateTime)DatePickerKraj.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            
            if (terminiKraj != null)
            {
                terminiKraj.Clear();
                if (vrijemePocetka.SelectedItem != null)
                {
                    if (prvoSledeceZauzece(vrijemePocetka.SelectedItem.ToString().Split(':')[0], datumPocetka) == 24 && slobodniTerminiIzmedju(datumPocetka, datumKraja))//Termini i pregledi ne traju vise dana...
                    {
                        for (int termin = 1; termin <= prvoSledeceZauzece("1", datumKraja); termin++)
                        {
                            dodajTerminKraja(datumPocetka, termin);
                        }
                    }
                }
            }
        }

        private bool slobodniTerminiIzmedju(string datumPocetka, string datumKraja)
        {
            DateTime? pocetakRenoviranja = (DateTime?)DateTime.Parse(datumPocetka);
            DateTime? krajRenoviranja = (DateTime?)DateTime.Parse(datumKraja);
            for (var datum = pocetakRenoviranja; datum < krajRenoviranja; datum = datum.Value.AddDays(1))
            {
                string datumTrenutni = ((DateTime)datum).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if (zauzecaZaDatum(datumTrenutni).Count != 0)
                {
                    return false;
                }
            }
            return true;
        }


        private int prvoSledeceZauzece(string vrijeme, string datum)
        {
            foreach(int zauzetiTermin in zauzecaZaDatum(datum))
            {
                if(zauzetiTermin > int.Parse(vrijeme))
                {
                    return zauzetiTermin;
                }
            }
            return 24;
        }
        
        private void postaviTermineKraja()
        {
            string datumKraja = ((DateTime)DatePickerKraj.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (terminiKraj != null)
            {
                terminiKraj.Clear();
                for (int termin = (int)DateTime.Now.Hour + 1; termin <= 23; termin++)
                {
                    dodajTerminKraja(datumKraja, termin);
                }
            }
        }
        

        private void azurirajDatumKraja()
        {
            if (DatePickerKraj != null && DatePickerPocetak != null)
            {
                this.DatePickerKraj.DisplayDateStart = DatePickerPocetak.SelectedDate;
                this.DatePickerKraj.SelectedDate = DatePickerPocetak.SelectedDate;
            }
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (salaZaSpajanje != null)
            {
                spojiSale();
                ZauzeceSale zauzeceSale = napraviZauzece();
                SaleServis.zauzmiSalu(zauzeceSale, izabranaSala);
                SaleServis.zauzmiSalu(zauzeceSale, salaZaSpajanje);
                SaleServis.sacuvajIzmjene();
            }
            else if (opremaZaPrebacivanje != null)
            {
                ZauzeceSale zauzeceSale = napraviZauzece();
                SaleServis.prebaciOpremuIzStareSale(izabranaSala, opremaZaPrebacivanje);
                napraviNovuSalu();
                SaleServis.zauzmiSalu(zauzeceSale, izabranaSala);
                SaleServis.zauzmiSalu(zauzeceSale, novaSala);
                SaleServis.sacuvajIzmjene();
            }
            else
            {
                ZauzeceSale zauzeceSale = napraviZauzece();
                SaleServis.zauzmiSalu(zauzeceSale, izabranaSala);
                SaleServis.sacuvajIzmjene();
            }
            this.Close();
        }
   
        private void napraviNovuSalu()
        {
            novaSala.Oprema = opremaZaPrebacivanje;
            SaleServis.DodajSalu(novaSala);
            PrikaziSalu.Sale.Add(novaSala);
        }

        private void spojiSale()
        {
            OpremaServis.dodajOpremuIzSaleZaDodavanje(izabranaSala, salaZaSpajanje);
            SaleServis.ObrisiSalu(salaZaSpajanje);
            PrikaziSalu.Sale.Remove(salaZaSpajanje);
            SaleServis.sacuvajIzmjene();
        }

        private ZauzeceSale napraviZauzece()
        {
            string datumPocetka = ((DateTime)DatePickerPocetak.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string datumKraja = ((DateTime)DatePickerKraj.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            return new ZauzeceSale(this.vrijemePocetka.Text, this.vrijemeKraja.Text, datumPocetka, datumKraja, 0);
        }

        private List<int> zauzecaZaDatum(string datum)
        {
            List<int> zauzeca = new List<int>();
            foreach (ZauzeceSale zauzece in izabranaSala.zauzetiTermini)
            {
                if(zauzece.datumPocetkaTermina == datum)
                {
                    for (int i = int.Parse(zauzece.pocetakTermina.Split(':')[0]); i <= int.Parse(zauzece.krajTermina.Split(':')[0]); i++) {
                        zauzeca.Add(i);
                    }
                }
            }
            return zauzeca;
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void postaviDugme()
        {
            if(this.vrijemeKraja.SelectedItem != null && this.vrijemePocetka.SelectedItem != null)
            {
                this.Potvrdi.IsEnabled = true;
            }else
            {
                this.Potvrdi.IsEnabled = false;
            }
        }

        private void Spoji_Click(object sender, RoutedEventArgs e)
        {
            SpajanjeSala spajanjeSala = new SpajanjeSala(izabranaSala);
            spajanjeSala.Show();
        }

        private void Podijeli_Click(object sender, RoutedEventArgs e)
        {
            NovaSala novaSala = new NovaSala(izabranaSala);
            novaSala.Show();
        }

        private void Window_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (salaZaSpajanje != null)
            {
                this.tekst.Text = "Spajanje sa salom: " + salaZaSpajanje.Namjena + ", br. " + salaZaSpajanje.brojSale;
            }
            else if (opremaZaPrebacivanje != null)
            {
                this.tekst.Text = "Podjela sale na 2";
            }
            else
            {
                this.tekst.Text = "";
            }
        }
    }
}
