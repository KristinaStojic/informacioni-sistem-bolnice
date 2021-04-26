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
            postaviVrijemePocetka();
            postaviVrijemeKraja();
        }

        private void postaviVrijemePocetka()
        {
            if (terminiPocetak != null)
            {
                terminiPocetak.Clear();
                terminiPocetak.Add("07:00");
                if (DateTime.Now.Date == DatePickerPocetak.SelectedDate)
                {
                    for (int i = (int)DateTime.Now.Hour + 1; i <= 23; i++)
                    {
                        terminiPocetak.Add(i + ":00");
                    }
                }
                else
                {
                    for (int i = 1; i <= 23; i++)
                    {
                        terminiPocetak.Add(i + ":00");
                    }
                }
            }
            
        }

        private void postaviVrijemeKraja()
        {
            if (terminiKraj != null)
            {
                terminiKraj.Clear();
                
                terminiKraj.Add("08:00");
                if (DateTime.Now.Date == DatePickerKraj.SelectedDate)
                {
                    for (int i = (int)DateTime.Now.Hour + 1; i <= 23; i++)
                    {
                        terminiKraj.Add(i + ":00");
                    }
                }
                else
                {
                    for (int i = 1; i <= 23; i++)
                    {
                        terminiKraj.Add(i + ":00");
                    }
                }
            }
        }

        private void DatePickerKraj_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            postaviVrijemeKraja();
        }

        private void DatePickerPocetak_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            postaviVrijemePocetka();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime pocetak = nadjiDatumIVrijeme(DatePickerPocetak.SelectedDate, vrijemePocetka.SelectedItem.ToString());
            DateTime kraj = nadjiDatumIVrijeme(DatePickerKraj.SelectedDate, vrijemeKraja.SelectedItem.ToString());
            string datum = ((DateTime)DatePickerPocetak.SelectedDate).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            ZauzeceSale zauzeceSale = new ZauzeceSale(this.vrijemePocetka.Text, this.vrijemeKraja.Text, datum, 000);
            zauzmiSalu(zauzeceSale);
            SaleMenadzer.sacuvajIzmjene();
            this.Close();
        }
        private void zauzmiSalu(ZauzeceSale zauzeceSale)
        {
            foreach(Sala sala in SaleMenadzer.sale)
            {
                if(sala.Id == izabranaSala.Id)
                {
                    sala.zauzetiTermini.Add(zauzeceSale);
                }
            }
        }
        private DateTime nadjiDatumIVrijeme(DateTime? dateTime, string vrijeme)
        {
            DateTime? datumPocetka = dateTime;
            string vrijemeSlanja = vrijeme;
            string datum = datumPocetka.Value.ToString("dd.MM.yyy", System.Globalization.CultureInfo.InvariantCulture);
            string[] datumi = datum.Split('.');
            string dan = datumi[0];
            string mjesec = datumi[1];
            string godina = datumi[2];
            string[] sati = vrijemeSlanja.Split(':');
            string sat = sati[0];
            string minuti = sati[1];
            DateTime datumIVrijeme = new DateTime(int.Parse(godina), int.Parse(mjesec), int.Parse(dan), int.Parse(sat), int.Parse(minuti), 0);
            return datumIVrijeme;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
