using Model;
using Projekat.Model;
using Projekat.Servis;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat
{
    public partial class ZakazaniTerminiPacijent : Page
    {
        public static ObservableCollection<Termin> Termini { get; set; }
        public static int idPacijent;
        public static Pacijent prijavljeniPacijent;
        ObservableCollection<string> months;
        PacijentiServis servis = new PacijentiServis();


        public ZakazaniTerminiPacijent(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            Termini = TerminServis.PronadjiTerminPoIdPacijenta(idPacijent);
            prijavljeniPacijent = servis.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
            InicijalizujKalendar();
        }

        private void InicijalizujKalendar()
        {
            PostaviZaglavljeKalendara();
            cboMonth.SelectionChanged += (o, e) => RefreshCalendar();
            cboYear.SelectionChanged += (o, e) => RefreshCalendar();
            string datum = DateTime.Now.Date.ToString("MM/dd/yyyy").Split(' ')[0];
            int mesec = Int32.Parse(datum.Split('/')[0]);
            cboMonth.SelectedIndex = mesec - 1;
            DodajZakazaneTermineNaKalendar();
        }

        private void PostaviZaglavljeKalendara()
        {
            if (Jezik.Header.Equals("_en-US"))
            {
                Kalendar.DayNames = new ObservableCollection<string> { "Nedelja", "Ponedeljak", "Utorak", "Sreda", "Četvrtak", "Petak", "Subota" };
            }
            else
            {
                Kalendar.DayNames = new ObservableCollection<string> { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            }
        }

        private void DodajZakazaneTermineNaKalendar()
        {
            foreach (Termin termin in TerminMenadzer.PronadjiTerminPoIdPacijenta(idPacijent))
            {
                int mesec = Int32.Parse(termin.Datum.Split('/')[0].Substring(1,1));
                if ((cboMonth.SelectedIndex + 1) == mesec)
                {
                    int dan = Int32.Parse(termin.Datum.Split('/')[1]);
                    if (mesec == 1) // jan
                    {
                        dan += 4;
                        Kalendar.Days[dan].Notes = termin.tipTermina.ToString() + "\nLekar: " + termin.Lekar.ToString();
                    }
                    if (mesec == 2)  // feb
                    {
                        Kalendar.Days[dan].Notes = termin.tipTermina.ToString() + "\nLekar: " + termin.Lekar.ToString();
                    }
                    if (mesec == 3) // mart
                    {
                        Kalendar.Days[dan].Notes = termin.tipTermina.ToString() + "\nLekar: " + termin.Lekar.ToString();
                    }
                    if (mesec == 4) // apr
                    {
                        dan += 3;
                        Kalendar.Days[dan].Notes = termin.tipTermina.ToString() + "\nLekar: " + termin.Lekar.ToString();
                    }
                    if (mesec == 5) // maj
                    {
                        dan += 5; 
                        Kalendar.Days[dan].Notes = termin.tipTermina.ToString() + "\nLekar: " + termin.Lekar.ToString();
                    }
                    if(mesec == 6)  // jun
                    {
                        dan += 1;
                        Kalendar.Days[dan].Notes = termin.tipTermina.ToString() + "\nLekar: " + termin.Lekar.ToString();
                    }
                    if(mesec == 7)
                    {
                        dan += 3;
                        Kalendar.Days[dan].Notes = termin.tipTermina.ToString() + "\nLekar: " + termin.Lekar.ToString();
                    }
                    if (mesec == 8)
                    {
                        Kalendar.Days[dan].Notes = termin.tipTermina.ToString() + "\nLekar: " + termin.Lekar.ToString();
                    }
                    if (mesec == 9)
                    {
                        dan += 2;
                        Kalendar.Days[dan].Notes = termin.tipTermina.ToString() + "\nLekar: " + termin.Lekar.ToString();
                    }
                    if (mesec == 10)
                    {
                        dan += 4;
                        Kalendar.Days[dan].Notes = termin.tipTermina.ToString() + "\nLekar: " + termin.Lekar.ToString();
                    }
                    if (mesec == 11)
                    {
                        Kalendar.Days[dan].Notes = termin.tipTermina.ToString() + "\nLekar: " + termin.Lekar.ToString();
                    }
                    if (mesec == 12)
                    {
                        dan += 2;
                        Kalendar.Days[dan].Notes = termin.tipTermina.ToString() + "\nLekar: " + termin.Lekar.ToString();
                    }


                }
            }
        }

        /* Pomeri termin */
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Page izmeniTermin = new ZakazaniTerminiPacijentDatum(idPacijent);
            this.NavigationService.Navigate(izmeniTermin);
        }
        private void odjava_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.odjava_Click(this);
        }
        public void karton_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.karton_Click(this, idPacijent);
        }
        public void zakazi_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.zakazi_Click(this, idPacijent);
        }
        public void uvid_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.uvid_Click(this, idPacijent);
        }
        private void pocetna_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.pocetna_Click(this, idPacijent);
        }
        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.anketa_Click(this, idPacijent);
        }
        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.PromeniTemu(SvetlaTema, tamnaTema);
        }
        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Korisnik_Click(this, idPacijent);
        }
        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Jezik_Click(Jezik);
        }
        private void RefreshCalendar()
        {
            if (cboYear.SelectedItem == null) return;
            if (cboMonth.SelectedItem == null) return;

            int year = Int32.Parse(cboYear.SelectedValue.ToString().Split(' ')[1]);

            int month = Int32.Parse(cboMonth.SelectedValue.ToString().Split(' ')[1]);

            DateTime targetDate = new DateTime(year, month, 1);

            Kalendar.BuildCalendar(targetDate);

            DodajZakazaneTermineNaKalendar();

        }
        private void Kalendar_DayChanged(object sender, MyCalendar.Calendar.DayChangedEventArgs e)
        {
            //Console.WriteLine("Pozvana metoda daychanged");


        }
        private void Kalendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           // MessageBox.Show(Kalendar.Days[0].ToString());
        }
        private void cboMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DodajZakazaneTermineNaKalendar();
        }
    }
}
