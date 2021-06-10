using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Model;
using Projekat.Model;
using Projekat.Servis;
using static Model.Termin;

namespace Projekat
{
    public partial class ZakaziTermin : Page
    {
        private static int idPacijent;
        public static Lekar izabraniLekar { get; set; }
        private List<Sala> SaleZaPreglede;
        private Sala prvaSlobodnaSala;
        private static Pacijent prijavljeniPacijent;
        private static ObservableCollection<Uput> UputiPacijenta { get; set; }
        private static bool jeSelektovanUput;
        private static Termin termin;
        private AnketeZaKlinikuServis anketeZaKlinikuServis;
        private AnketeZaLekaraServis anketeZaLekaraServis;
        public ZakaziTermin(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            InicijalizujPodatkeNaWpf(idPrijavljenogPacijenta);
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
            this.combo.SelectedIndex = 0;
            this.podaci.Header = PacijentWebStranice.podaciPacijenta(prijavljeniPacijent);
            anketeZaKlinikuServis = new AnketeZaKlinikuServis();
            anketeZaLekaraServis = new AnketeZaLekaraServis();
        }

        private void InicijalizujPodatkeNaWpf(int idPrijavljenogPacijenta)
        {
            //datum.BlackoutDates.AddDatesInPast();
            idPacijent = idPrijavljenogPacijenta;
            prijavljeniPacijent = PacijentiServis.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            InicijalizujPodatkeOLekaru(prijavljeniPacijent);
            UputiPacijenta = new ObservableCollection<Uput>();
            DodajUputePacijenta(UputiPacijenta, idPacijent);
            comboUputi.ItemsSource = UputiPacijenta;
            jeSelektovanUput = false;
        }

        private static void DodajUputePacijenta(ObservableCollection<Uput> uputiPacijenta, int idPacijent)
        {
            foreach (Uput uput in ZdravstveniKartonServis.PronadjiSveSpecijalistickeUputePacijenta(idPacijent))
            {
                uputiPacijenta.Add(uput);
            }
        }

        private void InicijalizujPodatkeOLekaru(Pacijent prijavljeniPacijent)
        {
            if (izabraniLekar == null)
            {
                izabraniLekar = prijavljeniPacijent.IzabraniLekar;
            }
            this.imePrz.Text = izabraniLekar.ToString();
        }

        private void PromenaSpecijalistickogUputa(object sender, RoutedEventArgs e)
        {
            try
            {
                if (comboUputi.Text.Equals("Specijalistički pregled") && !jeSelektovanUput)
                {
                    MessageBox.Show("Izaberite uput za koji želite da zakažene specijalistički pregled", "Uput", MessageBoxButton.OK);
                    return;
                }
                if (datum.SelectedDate == null)
                {
                    valDatum.Visibility = Visibility.Visible;
                    return;
                }
                if(vpp.SelectedItem == null)
                {
                    valVreme.Visibility = Visibility.Visible;
                    return;
                }
                if (combo.SelectedItem == null)
                {
                    valTipTermina.Visibility = Visibility.Visible;
                    return;
                }
                PokupiPodatkeZaZakazivanjeTermina();
                Page uvid = new ZakazaniTerminiPacijent(idPacijent);
                this.NavigationService.Navigate(uvid);
            }
            catch (System.Exception)
            {
                MessageBox.Show("Morate popuniti sva polja kako biste zakazali termin", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PokupiPodatkeZaZakazivanjeTermina()
        {
            int brojTermina = TerminServis.GenerisanjeIdTermina();
            String datumTermina = TerminServis.FormatirajSelektovaniDatum(datum.SelectedDate.Value);
            String vremePocetka = vpp.Text;
            String vremeKraja = TerminServis.IzracunajVremeKrajaPregleda(vremePocetka);
            TipTermina tipTermina = TipTermina.Pregled;
            
            termin = new Termin(brojTermina, datumTermina, vremePocetka, vremeKraja, tipTermina);
            Pacijent pacijent = PacijentiServis.PronadjiPoId(idPacijent);
            termin.Pacijent = pacijent;
            termin.Lekar = izabraniLekar;
            if (tipTermina.Equals(TipTermina.Pregled))
            {
                termin.Lekar.BrojPregleda++;
            }
            else if (tipTermina.Equals(TipTermina.Operacija))
            {
                termin.Lekar.BrojOperacija++;
            }
            SaleServis.DodajZauzeceSale(termin, prvaSlobodnaSala);
            SaleServis.sacuvajIzmjene();
            termin.Prostorija = prvaSlobodnaSala;
            TerminServis.ZakaziTermin(termin);

            anketeZaLekaraServis.DodajAnketuZaLekara(termin, idPacijent);
            anketeZaKlinikuServis.ProveriAnketuZaKliniku(idPacijent);
            ProxyMalicioznoPonasanjeServis proxy = new ProxyMalicioznoPonasanjeServis();
            proxy.DodajMalicioznoPonasanje(idPacijent);
        }
         
        #region Zakazivanje termina
        private void FiltritajTipTermina(object sender, SelectionChangedEventArgs e)
        {
            SaleZaPreglede = TerminServis.FiltrirajTipTermina(this.combo, this.comboUputi, this.preferenca, idPacijent);
        }

        private void FiltrirajDatum(object sender, SelectionChangedEventArgs e)
        {
            if (SaleZaPreglede == null)
            {
                MessageBox.Show("Izaberite tip termina", "Upozorenje", MessageBoxButton.OK);
                return;
            }
            vpp.ItemsSource = TerminServis.FiltrirajDatum(datum);

        }

        private void FiltrirajVremePocetka(object sender, SelectionChangedEventArgs e)
        {
            prvaSlobodnaSala = TerminServis.FiltritajVremePocetka(vpp, datum);
            if (prvaSlobodnaSala == null)
            {
                MessageBox.Show("Ne postoji slobodan termin", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            potvrdi.IsEnabled = true;
        }

        #endregion

        private void preferenca_Click(object sender, RoutedEventArgs e)
        {
            Page ztp = new ZakaziTerminPreferenca(idPacijent);
            this.NavigationService.Navigate(ztp);
        }

        private void ElektronskoPlacanje(object sender, RoutedEventArgs e)
        {
            try
            {
                if (comboUputi.Text.Equals("Specijalistički pregled") && !jeSelektovanUput)
                {
                    MessageBox.Show("Izaberite uput za koji želite da zakažene specijalistički pregled", "Uput", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                PokupiPodatkeZaZakazivanjeTermina();
                Page elektronsko = new ElektronskoPlacanjePacijent(idPacijent, termin.tipTermina);
                this.NavigationService.Navigate(elektronsko);
            }
            catch (System.Exception)
            {
                MessageBox.Show("Morate popuniti sva polja kako biste zakazali termin", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
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
            Page podaci = new LicniPodaciPacijenta(idPacijent);
            this.NavigationService.Navigate(podaci);
        }

        private void IzborUputa(object sender, SelectionChangedEventArgs e)
        {
            if (combo.Text.Equals("Pregled"))
            {
                comboUputi.IsEnabled = false;
                return;
            }
            Uput izabraniUput = (Uput)comboUputi.SelectedItem;
            izabraniLekar = LekariServis.NadjiPoId(izabraniUput.IdLekaraKodKogSeUpucuje);
            this.imePrz.Text = izabraniLekar.ToString();
        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Jezik_Click(Jezik);
        }

    }
}