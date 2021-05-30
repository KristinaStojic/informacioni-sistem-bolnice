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
        public ZakaziTermin(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            InicijalizujPodatkeNaWpf(idPrijavljenogPacijenta);
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
            this.combo.SelectedIndex = 0;
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
        }

        private void InicijalizujPodatkeNaWpf(int idPrijavljenogPacijenta)
        {
            datum.BlackoutDates.AddDatesInPast();
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (comboUputi.Text.Equals("Specijalistički pregled") && !jeSelektovanUput)
                {
                    MessageBox.Show("Izaberite uput za koji želite da zakažene specijalistički pregled", "Uput", MessageBoxButton.OK, MessageBoxImage.Information);
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
            SaleServis.DodajZauzeceSale(termin, prvaSlobodnaSala);
            termin.Prostorija = prvaSlobodnaSala;
            TerminServis.ZakaziTermin(termin);

            AnketaServis.DodajAnketuZaLekara(termin, idPacijent);
            AnketaServis.ProveriAnketuZaKliniku(idPacijent);
            MalicioznoPonasanjeServis.DodajMalicioznoPonasanje(idPacijent);
        }
         
        #region Zakazivanje termina
        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaleZaPreglede = TerminServis.combo_SelectionChanged(this.combo, this.comboUputi, this.preferenca, idPacijent);
        }

        private void datum_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SaleZaPreglede == null)
            {
                MessageBox.Show("Izaberite tip termina", "Upozorenje", MessageBoxButton.OK);
                return;
            }
            vpp.ItemsSource = TerminServis.datum_SelectedDatesChanged(datum);

        }

        private void vpp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            prvaSlobodnaSala = TerminServis.Vpp_SelectionChanged(vpp, datum);
            if (prvaSlobodnaSala == null)
            {
                MessageBox.Show("Ne postoji slobodan termin", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
            // TODO: odraditi i preko validacije
            catch (System.Exception)
            {
                MessageBox.Show("Morate popuniti sva polja kako biste zakazali termin", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

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
            Page podaci = new LicniPodaciPacijenta(idPacijent);
            this.NavigationService.Navigate(podaci);
        }

        private void comboUputi_SelectionChanged(object sender, SelectionChangedEventArgs e)
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