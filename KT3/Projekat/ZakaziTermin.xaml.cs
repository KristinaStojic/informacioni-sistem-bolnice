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
        private static ObservableCollection<string> PomocnaSviSlobodniSlotovi { get; set; }
        private static ObservableCollection<Uput> UputiPacijenta { get; set; }
        private static bool selektovanUput;
        public ZakaziTermin(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            InicijalizujPodatkeNaWpf(idPrijavljenogPacijenta);
            PomocnaSviSlobodniSlotovi = SaleServis.InicijalizujSveSlotove();
            PrikaziTermin.AktivnaTemaPagea(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
            this.combo.SelectedIndex = 0;
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
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
            selektovanUput = false;
        }

        private static void DodajUputePacijenta(ObservableCollection<Uput> uputiPacijenta, int idPacijent)
        {
            // TODO: ispraviti na ZdravstveniKartonServis
            foreach (Uput uput in ZdravstveniKartonMenadzer.PronadjiSveSpecijalistickeUputePacijenta(idPacijent))
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
                if (comboUputi.Text.Equals("Specijalistički pregled") && !selektovanUput)
                {
                    MessageBox.Show("Izaberite uput za koji želite da zakažene specijalistički pregled", "Uput", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                PokupiPodatkeZaZakazivanjeTermina();
                Page uvid = new ZakazaniTerminiPacijent(idPacijent);
                this.NavigationService.Navigate(uvid);
            }
            // TODO: odraditi i preko validacije
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
            Termin termin = new Termin(brojTermina, datumTermina, vremePocetka, vremeKraja, tipTermina);
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
            // elektronsko placanje
            MessageBox.Show("Elektronsko placanje ce uskoro biti implementirano", "Obavestenje");
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
        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.anketa_Click(this, idPacijent);
        }

        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.PromeniTemu(SvetlaTema, tamnaTema);
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
            PacijentPagesServis.Jezik_Click(Jezik);

        }

    }
}