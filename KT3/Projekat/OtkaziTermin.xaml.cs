using Model;
using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public partial class OtkaziTermin : Page
    {
        public static Pacijent prijavljeniPacijent;
        public static int idPacijent;
        public static Termin terminZaBrisanje;
        public OtkaziTermin(Termin zaBrisanje)
        {
            InitializeComponent();
            this.DataContext = this;
            terminZaBrisanje = zaBrisanje;
            idPacijent = zaBrisanje.Pacijent.IdPacijenta;
            prijavljeniPacijent = PacijentiServis.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            InicijalizujPodatkeOTerminuZaBrisanje(zaBrisanje);
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
        }

        private void InicijalizujPodatkeOTerminuZaBrisanje(Termin zaBrisanje)
        {
            this.tipTermina.Text = zaBrisanje.tipTermina.ToString();
            this.datum.Text = zaBrisanje.Datum;
            this.vreme.Text = zaBrisanje.VremePocetka;
            this.lekar.Text = zaBrisanje.Lekar.ToString();
            this.sala.Text = zaBrisanje.Prostorija.Id.ToString();
        }

        #region Otkazivanje termina
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OtkaziOdabraniTermin();
            Page uvid = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvid);
        }

        private static void OtkaziOdabraniTermin()
        {
            TerminServis.OtkaziTermin(terminZaBrisanje);
            MalicioznoPonasanjeServis.DodajMalicioznoPonasanje(idPacijent); 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Page uvid = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvid);
        }
        #endregion

        private void odjava_Click(object sender, RoutedEventArgs e)
        {
            /*Page odjava = new PrijavaPacijent();
            this.NavigationService.Navigate(odjava);*/
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
            PacijentWebStranice.Jezik_Click(Jezik);

        }

    }
}
