using Model;
using Projekat.Model;
using Projekat.Servis;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat
{
    public partial class PodsetnikPacijent : Page
    {
        private static int idPacijent;
        private static Pacijent prijavljeniPacijent;
        ObavestenjaServis servis = new ObavestenjaServis();
        PacijentiServis pacijentiServis = new PacijentiServis();
        public PodsetnikPacijent(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
            prijavljeniPacijent = pacijentiServis.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            Datum.BlackoutDates.AddDatesInPast();
        }

        private void DodajPodsetnik_Click(object sender, RoutedEventArgs e)
        {
            valVreme.Visibility = Visibility.Hidden;
            valDatum.Visibility = Visibility.Hidden;
            valSadrzaj.Visibility = Visibility.Hidden;
            try
            {

                string vremePodsetnika = Vreme.Text;
                string datumPodsetnika = Datum.SelectedDate.Value.ToString("MM/dd/yyyy") + " " + vremePodsetnika;
                string sadrzajPodsetnika = SadrzajPodsetnika.Text;
                try
                {
                    DateTime formatiranoVreme = DateTime.Parse(Vreme.Text); 
                } catch
                {
                    if (Jezik.Header.Equals("_en-US"))
                    {
                        MessageBox.Show("Neispravan format vremena(format: HH:mm)");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Invalid time format (format HH:mm)");
                        return;
                    }
                    
                }
               
                List<int> pacijenti = new List<int>();
                pacijenti.Add(idPacijent);
                if (SadrzajPodsetnika.Text == "")
                {
                    valSadrzaj.Visibility = Visibility.Visible;
                    return;
                }
                if (vremePodsetnika == "")
                {
                    valSadrzaj.Visibility = Visibility.Visible;
                    return;
                }

                if (valDatum.Visibility == Visibility.Visible || valVreme.Visibility == Visibility.Visible 
                    || !ProveriIspravnostZaDatum(Datum) || valSadrzaj.Visibility == Visibility.Visible)
                {
                    return;
                }
                Obavestenja obavestenjeZaPodsetnik = new Obavestenja(servis.GenerisanjeIdObavestenja(), datumPodsetnika, "Podsetnik", sadrzajPodsetnika, pacijenti, true);
                servis.PronadjiSvaObavestenja().Add(obavestenjeZaPodsetnik);
                //ObavestenjaServis.sacuvajIzmene();

                Vreme.Text = null;
                Datum.Text = null;
                SadrzajPodsetnika.Text = null;

                Page pocetna = new PrikaziTermin(idPacijent);
                this.NavigationService.Navigate(pocetna);
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    valVreme.Visibility = Visibility.Visible;
                }
                if (ex is InvalidOperationException)
                {
                    valDatum.Visibility = Visibility.Visible;
                }
                if(SadrzajPodsetnika.Text == "")
                {
                    valSadrzaj.Visibility = Visibility.Visible;
                }
                if (Vreme.Text == "")
                {
                    valVreme.Visibility = Visibility.Visible;
                }
            }
        }

        private bool ProveriIspravnostZaDatum(DatePicker datum)
        {
            if(datum.SelectedDate.Value == null)
            {
                return false;
            }
            else
            {
                return true;
            }  
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
        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.PromeniTemu(SvetlaTema, tamnaTema);
        }
        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Korisnik_Click(this, idPacijent);
        }
        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.anketa_Click(this, idPacijent);
        }
        private void Jezik_Click(object sender, RoutedEventArgs e)
        { 
            PacijentWebStranice.Jezik_Click(Jezik);
        }

    }
}
