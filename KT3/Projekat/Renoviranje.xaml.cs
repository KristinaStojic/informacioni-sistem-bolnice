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
    /// Interaction logic for Renoviranje.xaml
    /// </summary>
    public partial class Renoviranje : Window
    {
        public ObservableCollection<string> terminiPocetak { get; set; }
        public ObservableCollection<string> terminiKraj { get; set; }
        public Sala izabranaSala;
        public Renoviranje(Sala izabranaSala)
        {
            InitializeComponent();
            this.DataContext = this;
            this.izabranaSala = izabranaSala;
            terminiPocetak = new ObservableCollection<string>();
            terminiKraj = new ObservableCollection<string>();
            this.Potvrdi.IsEnabled = false;
            postaviVrijemePocetka();
            postaviVrijemeKraja();
        }
       
        private void DatePickerPocetak_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            azurirajDatumKraja();
            azurirajVrijemePocetka();
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
        private void azurirajVrijemePocetka()
        {
            if(DatePickerPocetak.SelectedDate == DateTime.Now.Date)
            {
                postaviVrijemePocetka();
            }
            else
            {
                postaviVrijemePocetkaDrugiDatum();
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

        private void azurirajVrijemeKrajaIstiDatum()
        {
            string datumKraja = ((DateTime)DatePickerPocetak.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (terminiKraj != null && vrijemePocetka.SelectedItem != null)
            {
                terminiKraj.Clear();
                for (int i = int.Parse(vrijemePocetka.SelectedItem.ToString().Split(':')[0]) + 1; i <= prvoSledeceZauzece(vrijemePocetka.SelectedItem.ToString().Split(':')[0], datumKraja); i++)
                {
                    if (!zauzecaZaDatum(datumKraja).Contains(i))
                    {
                        terminiKraj.Add(i + ":00");
                    }
                }
            }
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
            return 23;
        }
        private void azurirajVrijemeKrajaDrugiDatum()
        {
            string datumKraja = ((DateTime)DatePickerPocetak.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (terminiKraj != null)
            {
                terminiKraj.Clear();
                if (prvoSledeceZauzece(vrijemePocetka.SelectedItem.ToString().Split(':')[0], datumKraja) == 23) {
                    for (int i = 1; i <= 23; i++)
                    {
                        if (!zauzecaZaDatum(datumKraja).Contains(i))
                        {
                            terminiKraj.Add(i + ":00");
                        }
                    }
                }
            }
        }

        private void postaviVrijemePocetka()
        {
            string datumPocetka = ((DateTime)DatePickerPocetak.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (terminiPocetak != null)
            {
                terminiPocetak.Clear();
                for (int i = (int)DateTime.Now.Hour + 1; i <= 23; i++)
                {
                    if (!zauzecaZaDatum(datumPocetka).Contains(i))
                    {
                        terminiPocetak.Add(i + ":00");
                    }
                }
            }

        }

        private void postaviVrijemeKraja()
        {
            string datumKraja = ((DateTime)DatePickerKraj.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (terminiKraj != null)
            {
                terminiKraj.Clear();
                for (int i = (int)DateTime.Now.Hour + 1; i <= 23; i++)
                {
                    if (!zauzecaZaDatum(datumKraja).Contains(i))
                    {
                        terminiKraj.Add(i + ":00");
                    }
                }
            }
        }
        private void postaviVrijemePocetkaDrugiDatum()
        {
            string datumPocetka = ((DateTime)DatePickerPocetak.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (terminiPocetak != null)
            {
                terminiPocetak.Clear();
                for (int i = 1; i <= 23; i++)
                {
                    if (!zauzecaZaDatum(datumPocetka).Contains(i))
                    {
                        terminiPocetak.Add(i + ":00");
                    }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string datumPocetka = ((DateTime)DatePickerPocetak.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string datumKraja = ((DateTime)DatePickerKraj.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            ZauzeceSale zauzeceSale = new ZauzeceSale(this.vrijemePocetka.Text, this.vrijemeKraja.Text, datumPocetka, datumKraja, 0);
            zauzmiSalu(zauzeceSale);
            SaleMenadzer.sacuvajIzmjene();
            this.Close();
        }
        private void zauzmiSalu(ZauzeceSale zauzeceSale)
        {
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (sala.Id == izabranaSala.Id)
                {
                    sala.zauzetiTermini.Add(zauzeceSale);
                }
            }
        }
        private List<int> zauzecaZaDatum(string datum)
        {
            List<int> zauzeca = new List<int>();
            foreach (ZauzeceSale zauzece in izabranaSala.zauzetiTermini)
            {
                if(zauzece.datumPocetkaTermina == datum)
                {
                    for (int i = int.Parse(zauzece.pocetakTermina.Split(':')[0]); i < int.Parse(zauzece.krajTermina.Split(':')[0]); i++) {
                        zauzeca.Add(i);
                    }
                }
            }
            return zauzeca;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void vrijemeKraja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            postaviDugme();
        }
        private void postaviDugme()
        {
            if(this.vrijemeKraja.SelectedItem != null && this.vrijemePocetka.SelectedItem != null)
            {
                this.Potvrdi.IsEnabled = true;
            }else if(this.vrijemeKraja.SelectedItem == null || this.vrijemePocetka.SelectedItem == null)
            {
                this.Potvrdi.IsEnabled = false;
            }
        }
    }
}
