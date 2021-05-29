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
    /// <summary>
    /// Interaction logic for PodsetnikPacijent.xaml
    /// </summary>
    public partial class PodsetnikPacijent : Page
    {
        private static int idPacijent;
        private static Pacijent prijavljeniPacijent;
        public PodsetnikPacijent(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            PacijentPagesServis.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
        }

        private void DodajPodsetnik_Click(object sender, RoutedEventArgs e)
        {
            string vremePodsetnika = Vreme.Text;
            string datumPodsetnika = Datum.SelectedDate.Value.ToString("MM/dd/yyyy") + " " + vremePodsetnika;
            string sadrzajPodsetnika = SadrzajPodsetnika.Text;

            List<int> pacijenti = new List<int>();
            pacijenti.Add(idPacijent);
            Obavestenja obavestenjeZaPodsetnik = new Obavestenja(ObavestenjaServis.GenerisanjeIdObavestenja(), datumPodsetnika, "Podsetnik", sadrzajPodsetnika, pacijenti, true);
            ObavestenjaServis.PronadjiSvaObavestenja().Add(obavestenjeZaPodsetnik);
            ObavestenjaServis.sacuvajIzmene();
            
            Vreme.Text = null;
            Datum.Text = null;
            SadrzajPodsetnika.Text = null;
        }
        private void odjava_Click(object sender, RoutedEventArgs e)
        {
            /*Page odjava = new PrijavaPacijent();
            this.NavigationService.Navigate(odjava);*/
            PacijentPagesServis.odjava_Click(this);
        }
        public void karton_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.karton_Click(this, idPacijent);
        }
        public void zakazi_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.zakazi_Click(this, idPacijent);
        }
        public void uvid_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.uvid_Click(this, idPacijent);
        }
        private void pocetna_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.pocetna_Click(this, idPacijent);
        }
        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.PromeniTemu(SvetlaTema, tamnaTema);
        }
        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.Korisnik_Click(this, idPacijent);
        }
        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.anketa_Click(this, idPacijent);
        }
        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            /* var app = (App)Application.Current;
             // TODO: proveriti
             string eng = "en-US";
             string srb = "sr-LATN";
             MenuItem mi = (MenuItem)sender;
             if (mi.Header.Equals("en-US"))
             {
                 mi.Header = "sr-LATN";
                 app.ChangeLanguage(eng);
             }
             else
             {
                 mi.Header = "en-US";
                 app.ChangeLanguage(srb);
             }*/
            PacijentPagesServis.Jezik_Click(Jezik);

        }

    }
}
