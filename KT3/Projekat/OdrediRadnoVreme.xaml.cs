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
using Model;
using Projekat.Model;
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for OdrediRadnoVreme.xaml
    /// </summary>
    public partial class OdrediRadnoVreme : Window
    {
        public Lekar lekar;
        public ObservableCollection<string> PocetakRadnogVremena = new ObservableCollection<string>()
                                                             { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30" };
        public ObservableCollection<string> KrajRadnogVremena = new ObservableCollection<string>()
                                                             { "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00" };
        LekariServis servis = new LekariServis();

        public OdrediRadnoVreme(Lekar selektovaniLekar)
        {
            InitializeComponent();
            this.lekar = selektovaniLekar;
            kalendar.DisplayDateStart = DateTime.Now;
            kalendar.DisplayDateEnd = DateTime.Now.AddDays(7);
            pocetak.ItemsSource = PocetakRadnogVremena;
            kraj.ItemsSource = KrajRadnogVremena;

            potvrdi.IsEnabled = false;
            pocetak.IsEnabled = false;
            kraj.IsEnabled = false;

            if (lekar.RadniDani.Count > 0)
            {
                datumi.Text = lekar.RadniDani[0].Datum;
                dodeljenPocetak.Text = lekar.RadniDani[0].VremePocetka;
                dodeljenKraj.Text = lekar.RadniDani[0].VremeKraja;
            }
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            List<RadniDan> radniDani = servis.NapraviListuRadnogVremena(pocetak.Text, kraj.Text, kalendar, lekar);
            List<Lekar> lekari = servis.NadjiSveLekare();

            foreach (Lekar l in lekari)
            {
                if (l.IdLekara == lekar.IdLekara)
                {
                    l.RadniDani = radniDani;
                    //LekariServis.SacuvajIzmeneLekara();
                }
            }

            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Vreme_pocetka_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            kraj.Text = "";

            foreach (string slot in PocetakRadnogVremena)
            {
                if (DateTime.Parse((string)pocetak.SelectedItem) >= DateTime.Parse(slot))
                {
                    KrajRadnogVremena.Remove(slot);
                }
            }

            if (kalendar.SelectedDate.HasValue && pocetak.SelectedIndex != -1)
            {
                kraj.IsEnabled = true;
            }
            else
            {
                kraj.IsEnabled = false;
            }
        }

        private void Vreme_kraja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (kraj.SelectedIndex != -1 && pocetak.SelectedIndex != -1 && kalendar.SelectedDate.HasValue)
            {
                potvrdi.IsEnabled = true;
            }
            else
            {
                potvrdi.IsEnabled = false;
            }
        }

        private void Kalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (kalendar.SelectedDate.HasValue)
            {
                pocetak.IsEnabled = true;
            }
            else
            {
                pocetak.IsEnabled = false;
            }
        }

        private void Pocetak_LostFocus(object sender, RoutedEventArgs e)
        {
            dodeljenPocetak.Text = pocetak.Text;
        }

        private void Kraj_LostFocus(object sender, RoutedEventArgs e)
        {
            dodeljenKraj.Text = kraj.Text;
        }

        private void Kalendar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (kalendar.SelectedDate != null)
            {
                DateTime datum = (DateTime)kalendar.SelectedDate;
                datumi.Text = datum.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.O && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Odustani_Click(sender, e);
            }
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Odustani_Click(sender, e);
            }
        }
    }
}
