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
            this.lekar.ItemsSource = LekariServis.PronadjiLekarePoSpecijalizaciji(Specijalizacija.Opsta_praksa);
            InicijalizujLicnePodatke(prijavljeniPacijent);
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);

        }

        private void InicijalizujLicnePodatke(Pacijent prijavljeniPacijent)
        {
            this.Ime.Text = prijavljeniPacijent.ImePacijenta;
            this.prezime.Text = prijavljeniPacijent.PrezimePacijenta;
            this.jmbg.Text = prijavljeniPacijent.Jmbg.ToString();
            InicijalizujPolPacijenta(prijavljeniPacijent);
            this.brojTel.Text = prijavljeniPacijent.BrojTelefona.ToString();
            this.email.Text = prijavljeniPacijent.Email;
            this.adresa.Text = prijavljeniPacijent.AdresaStanovanja;
            InicijalizujBracnoStanjePacijenta(prijavljeniPacijent);
            this.zanimanje.Text = prijavljeniPacijent.Zanimanje;
            InicijalizujIzabranogLekaraPacijenta(prijavljeniPacijent);
            this.pacijent.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
        }

        private void InicijalizujBracnoStanjePacijenta(Pacijent prijavljeniPacijent)
        {
            string bracno = prijavljeniPacijent.BracnoStanje.ToString();
            if (bracno.Equals("Neozenjen") || bracno.Equals("Neudata"))
            {
                this.bracStanje.SelectedIndex = 0;
                return;
            }
            else if (bracno.Equals("Ozenjen") || bracno.Equals("Udata"))
            {
                this.bracStanje.SelectedIndex = 1;
                return;
            }
            else if (bracno.Equals("Razveden") || bracno.Equals("Razvedena"))
            {
                this.bracStanje.SelectedIndex = 2;
                return;
            }
            else if (bracno.Equals("Udovac") || bracno.Equals("Udovica"))
            {
                this.bracStanje.SelectedIndex = 3;
                return;
            }
            else
            {
                this.bracStanje.SelectedIndex = 4;
                return;
            }
        }

        private void InicijalizujIzabranogLekaraPacijenta(Pacijent prijavljeniPacijent)
        {
            if (prijavljeniPacijent.IzabraniLekar != null)
            {
                this.lekar.Text = prijavljeniPacijent.IzabraniLekar.ToString();
            }
        }

        private void InicijalizujPolPacijenta(Pacijent prijavljeniPacijent)
        {
            string polPacijent = PacijentiServis.OdrediPolPacijenta(prijavljeniPacijent);
            if (polPacijent.Equals("M"))
            {
                this.poltxt.SelectedIndex = 0;
            }
            else
            {
                this.poltxt.SelectedIndex = 1;
            }
        }

        private void izmeniBtn_Click(object sender, RoutedEventArgs e)
        {
            PromeniVidljivostKomponentiPreIzmene();
        }

        private void odustani_Click(object sender, RoutedEventArgs e)
        {
            PromeniVidljivostKomponentiPosleIzmene();
            InicijalizujLicnePodatke(prijavljeniPacijent);
        }

        private void sacuvajIzmene_Click(object sender, RoutedEventArgs e)
        {

            string ime = this.Ime.Text;
            string prezime = this.prezime.Text;
            long jmbg = long.Parse(this.jmbg.Text);
            //pol polPacijenta = PacijentiServis.IzmeniPolPacijenta(this.poltxt.Text);
            pol polPacijenta = prijavljeniPacijent.Pol;
            if (this.poltxt != null)
            {
                if (poltxt.SelectedItem.Equals("Muški"))
                {
                    polPacijenta = pol.M;
                }
                if (poltxt.SelectedItem.Equals("Ženski"))
                {
                    polPacijenta = pol.Z;
                }
            }
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
            this.Ime.IsEnabled = false;
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

            this.Ime.IsEnabled = true;
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
      
        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Korisnik_Click(this, idPacijent);
        }

        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.PromeniTemu(SvetlaTema, tamnaTema);

        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Jezik_Click(Jezik);
        }
        #region Validacija licnih podataka

        public bool ProveraCifara(string tekst)
        {
            foreach (char karakter in tekst)
            {
                if (!(karakter >= '0' && karakter <= '9'))
                {
                    return false;
                }
            }
            return true;
        }
        private void prezime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (prezime.Text == null || prezime.Text.Equals(""))
            {
                valPrezime.Visibility = Visibility.Visible;
                sacuvajIzmene.IsEnabled = false;
                return;
            }
            sacuvajIzmene.IsEnabled = true;
            valPrezime.Visibility = Visibility.Hidden;
        }
        private void Ime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Ime.Text == null || Ime.Text.Equals(""))
            {
                valIme.Visibility = Visibility.Visible;
               sacuvajIzmene.IsEnabled = false;
                return;
            }
            sacuvajIzmene.IsEnabled = true;
            valIme.Visibility = Visibility.Hidden;
        }

        #endregion
        private void jmbg_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!ProveraCifara(jmbg.Text) || jmbg.Text == "")
            {
                valJmbg.Visibility = Visibility.Visible;
                sacuvajIzmene.IsEnabled = false;
                if (jmbg.Text.Length < 9 || jmbg.Text.Length > 13)
                {
                    valJmbg.Visibility = Visibility.Hidden;
                    valJmbgDuzina.Visibility = Visibility.Visible;
                    sacuvajIzmene.IsEnabled = false;
                }
                return;
            }
            if (jmbg.Text.Length < 9 || jmbg.Text.Length > 13)
            {
                valJmbg.Visibility = Visibility.Hidden;
                valJmbgDuzina.Visibility = Visibility.Visible;
                sacuvajIzmene.IsEnabled = false;
                return;
            }
            sacuvajIzmene.IsEnabled = true;
            valJmbg.Visibility = Visibility.Hidden;
            valJmbgDuzina.Visibility = Visibility.Hidden;
        }

        private void brojTel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!ProveraCifara(brojTel.Text) || brojTel.Text == null)
            {
                valTelefon.Visibility = Visibility.Visible;
                sacuvajIzmene.IsEnabled = false;
                if (brojTel.Text.Length < 9 || brojTel.Text.Length > 13)
                {
                    valTelefon.Visibility = Visibility.Hidden;
                    valTelefonDuzina.Visibility = Visibility.Visible;
                    sacuvajIzmene.IsEnabled = false;
                }
                return;
            }
            if (brojTel.Text.Length < 9 || brojTel.Text.Length > 13)
            {
                valTelefon.Visibility = Visibility.Hidden;
                valTelefonDuzina.Visibility = Visibility.Visible;
                sacuvajIzmene.IsEnabled = false;
                return;
            }
            sacuvajIzmene.IsEnabled = true;
            valTelefon.Visibility = Visibility.Hidden;
            valTelefonDuzina.Visibility = Visibility.Hidden;
        }

        private void adresa_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (adresa.Text == null || adresa.Text.Equals(""))
            {
                valAdresa.Visibility = Visibility.Visible;
                sacuvajIzmene.IsEnabled = false;
                return;
            }
            sacuvajIzmene.IsEnabled = true;
            valAdresa.Visibility = Visibility.Hidden;
        }

        private void email_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (email.Text == null || email.Text.Equals(""))
            {
                valEmail.Visibility = Visibility.Visible;
                sacuvajIzmene.IsEnabled = false;
                return;
            }
            sacuvajIzmene.IsEnabled = true;
            valEmail.Visibility = Visibility.Hidden;
        }

        private void zanimanje_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (zanimanje.Text == null || zanimanje.Text.Equals(""))
            {
                valZanimanje.Visibility = Visibility.Visible;
                sacuvajIzmene.IsEnabled = false;
                return;
            }
            sacuvajIzmene.IsEnabled = true;
            valZanimanje.Visibility = Visibility.Hidden;
        }

    
    }
}
