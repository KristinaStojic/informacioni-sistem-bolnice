using Model;
using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for LicniPodaciPacijenta.xaml
    /// </summary>
    public partial class LicniPodaciPacijenta : Page
    {
        private static int idPacijent;
        private static Pacijent prijavljeniPacijent;
        public LicniPodaciPacijenta(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            this.sacuvajIzmene.Visibility = Visibility.Hidden;
            this.odustani.Visibility = Visibility.Hidden;
            idPacijent = idPrijavljenogPacijenta;
            prijavljeniPacijent = PacijentiServis.PronadjiPoId(idPrijavljenogPacijenta);
            /* LEKARI OPSTE PRAKSE */
            this.lekar.ItemsSource = LekariServis.PronadjiLekarePoSpecijalizaciji(Specijalizacija.Opsta_praksa);
            InicijalizujLicnePodatke();
            PrikaziTermin.AktivnaTemaPagea(this.zaglavlje, this.SvetlaTema, this.tamnaTema);

        }

        private void InicijalizujLicnePodatke()
        {
            this.ime.Text = prijavljeniPacijent.ImePacijenta;
            this.prezime.Text = prijavljeniPacijent.PrezimePacijenta;
            this.jmbg.Text = prijavljeniPacijent.Jmbg.ToString();

            this.poltxt.Text = PacijentiServis.OdrediPolPacijenta(prijavljeniPacijent);
            this.brojTel.Text = prijavljeniPacijent.BrojTelefona.ToString();
            this.email.Text = prijavljeniPacijent.Email;
            this.adresa.Text = prijavljeniPacijent.AdresaStanovanja;
            this.bracStanje.Text = prijavljeniPacijent.BracnoStanje.ToString();
            this.zanimanje.Text = prijavljeniPacijent.Zanimanje;
            if (prijavljeniPacijent.IzabraniLekar != null)
            {
                this.lekar.Text = prijavljeniPacijent.IzabraniLekar.ToString();
            }
            this.pacijent.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
        }

        private void izmeniBtn_Click(object sender, RoutedEventArgs e)
        {
            PromeniVidljivostKomponentiPreIzmene();
        }

        private void odustani_Click(object sender, RoutedEventArgs e)
        {
            PromeniVidljivostKomponentiPosleIzmene();
        }

        private void sacuvajIzmene_Click(object sender, RoutedEventArgs e)
        {

            string ime = this.ime.Text;
            string prezime = this.prezime.Text;
            int jmbg = int.Parse(this.jmbg.Text);
            pol polPacijenta = PacijentiServis.IzmeniPolPacijenta(this.poltxt.Text);
            long brTel = long.Parse(this.brojTel.Text);
            string eMail = this.email.Text;
            string adresa = this.adresa.Text;
            bracnoStanje brStanje = PacijentiServis.OdrediBracnoStanjePacijenta(polPacijenta, this.bracStanje.Text);
            string zanimanje = this.zanimanje.Text;
            Lekar l = null;
            if (this.lekar != null)
            {
                l = (Lekar)this.lekar.SelectedItem;
            }
            Pacijent izmenjenPacijent = new Pacijent(prijavljeniPacijent.IdPacijenta, ime, prezime, jmbg, polPacijenta, brTel, eMail, adresa, statusNaloga.Stalni, zanimanje, brStanje);
            izmenjenPacijent.IzabraniLekar = l;
            PacijentiServis.IzmeniNalogPacijent(prijavljeniPacijent, izmenjenPacijent);
            PacijentiServis.SacuvajIzmenePacijenta(); 
            PromeniVidljivostKomponentiPosleIzmene();
        }

        private void PromeniVidljivostKomponentiPosleIzmene()
        {
            this.ime.IsEnabled = false;
            this.prezime.IsEnabled = false;
            this.jmbg.IsEnabled = false;
            this.poltxt.IsEnabled = false;
            this.brojTel.IsEnabled = false;
            this.email.IsEnabled = false;
            this.adresa.IsEnabled = false;
            this.bracStanje.IsEnabled = false;
            this.zanimanje.IsEnabled = false;
            this.lekar.IsEnabled = false;

            this.sacuvajIzmene.Visibility = Visibility.Hidden;
            this.odustani.Visibility = Visibility.Hidden;
            this.izmeniBtn.Visibility = Visibility.Visible;
        }

        private void PromeniVidljivostKomponentiPreIzmene()
        {
            this.izmeniBtn.Visibility = Visibility.Hidden;
            this.sacuvajIzmene.Visibility = Visibility.Visible;
            this.odustani.Visibility = Visibility.Visible;

            this.ime.IsEnabled = true;
            this.prezime.IsEnabled = true;
            this.jmbg.IsEnabled = true;
            this.poltxt.IsEnabled = true;
            this.brojTel.IsEnabled = true;
            this.email.IsEnabled = true;
            this.adresa.IsEnabled = true;
            this.bracStanje.IsEnabled = true;
            this.zanimanje.IsEnabled = true;
            this.lekar.IsEnabled = true;
        }

        private void odjava_Click(object sender, RoutedEventArgs e)
        {
            Page odjava = new PrijavaPacijent();
            this.NavigationService.Navigate(odjava);
        }

        public void karton_Click(object sender, RoutedEventArgs e)
        {
            Page karton = new ZdravstveniKartonPacijent(idPacijent);
            this.NavigationService.Navigate(karton);
        }

        public void zakazi_Click(object sender, RoutedEventArgs e)
        {
            if (MalicioznoPonasanjeServis.DetektujMalicioznoPonasanje(idPacijent))
            {
                MessageBox.Show("Nije Vam omoguceno zakazivanje termina jer ste prekoracili dnevni limit modifikacije termina.", "Upozorenje", MessageBoxButton.OK);
                return;
            }
            Page zakaziTermin = new ZakaziTermin(idPacijent);
            this.NavigationService.Navigate(zakaziTermin);
        }

        public void uvid_Click(object sender, RoutedEventArgs e)
        {
            Page uvid = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvid);
        }

        private void pocetna_Click(object sender, RoutedEventArgs e)
        {
            Page pocetna = new PrikaziTermin(idPacijent);
            this.NavigationService.Navigate(pocetna);
        }

        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            Page prikaziAnkete = new PrikaziAnkete(idPacijent);
            this.NavigationService.Navigate(prikaziAnkete);
        }
      
        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            Page podaci = new LicniPodaciPacijenta(idPacijent);
            this.NavigationService.Navigate(podaci);
        }

        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
            MenuItem mi = (MenuItem)sender;
            if (mi.Header.Equals("Svetla") || mi.Header.Equals("Light"))
            {
                //mi.Header = "Tamna";
                SvetlaTema.IsEnabled = false;
                tamnaTema.IsEnabled = true;
                app.ChangeTheme(new Uri("Teme/Svetla.xaml", UriKind.Relative));
            }
            else
            {
                //mi.Header = "Svetla";
                tamnaTema.IsEnabled = false;
                SvetlaTema.IsEnabled = true;
                app.ChangeTheme(new Uri("Teme/Tamna.xaml", UriKind.Relative));
            }
        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
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
            }
        }

    }
}
