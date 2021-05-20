using Model;
using Projekat.Model;
using Projekat.Servis;
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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmeniTermin.xaml
    /// </summary>
    public partial class IzmeniTermin : Page
    {
        private Termin termin;
        private static int idPacijent;
        private List<Sala> SaleZaPreglede;
        private static ObservableCollection<string> PomocnaSviSlobodniSlotovi { get; set; }
        private Sala prvaSlobodnaSala;
        private static Pacijent prijavljeniPacijent;
        // TODO: !! 
        private static List<string> SviZauzetiZaSelektovaniDatum { get; set; }

        public IzmeniTermin(Termin izabraniTermin)
        {
            InitializeComponent();
            this.DataContext = this;
            this.termin = izabraniTermin;
            PomocnaSviSlobodniSlotovi = SaleServis.InicijalizujSveSlotove();
            idPacijent = izabraniTermin.Pacijent.IdPacijenta;
            OgraniciIzborNovogDatuma(izabraniTermin);
            InicijalizujPodatkeZaIzabraniTermin(izabraniTermin);
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dgSearch.ItemsSource);
            view.Filter = UserFilter;
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PrikaziTermin.AktivnaTemaPagea(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
        }

        private void InicijalizujPodatkeZaIzabraniTermin(Termin izabraniTermin)
        {
            if (izabraniTermin != null)
            {
                TipTermina tp;
                if (izabraniTermin.tipTermina.Equals(TipTermina.Operacija))
                {
                    this.combo.SelectedIndex = 0;
                }
                else if (izabraniTermin.tipTermina.Equals(TipTermina.Pregled))
                {
                    this.combo.SelectedIndex = 1;
                }
                tp = izabraniTermin.tipTermina;
                this.imePrz.Text = izabraniTermin.Lekar.ImeLek + " " + izabraniTermin.Lekar.PrezimeLek;
                prijavljeniPacijent = PacijentiServis.PronadjiPoId(idPacijent);
                this.datum.DisplayDate = DateTime.Parse(izabraniTermin.Datum);
                InicijalizujSelektovanogLekara(izabraniTermin);
            }
        }

        private void InicijalizujSelektovanogLekara(Termin izabraniTermin)
        {
            int brojac = 0;
            this.dgSearch.ItemsSource = LekariServis.NadjiSveLekare();
            foreach (Lekar lekar in LekariServis.NadjiSveLekare())
            {
                brojac++;
                if (lekar.IdLekara.Equals(izabraniTermin.Lekar.IdLekara))
                {
                    this.dgSearch.SelectedItem = izabraniTermin.Lekar;
                    return;
                }
            }
        }

        private void OgraniciIzborNovogDatuma(Termin izabraniTermin)
        {
           /* CalendarDateRange daniPreTermina = new CalendarDateRange();
            daniPreTermina.Start = DateTime.Parse(izabraniTermin.Datum).AddDays(-1000);//DateTime.Parse(izabraniTermin.Datum).AddDays(3);
            daniPreTermina.End = DateTime.Parse(izabraniTermin.Datum).AddDays(-3);
            datum.BlackoutDates.Add(daniPreTermina);
            CalendarDateRange daniPosleTermina = new CalendarDateRange();
            daniPosleTermina.Start = DateTime.Parse(izabraniTermin.Datum).AddDays(3);
            daniPosleTermina.End = DateTime.Parse(izabraniTermin.Datum).AddDays(1000);
            datum.BlackoutDates.Add(daniPosleTermina);*/
            // TODO: ograniciti pomeranje samo za termine koji su u buducnosti
            datum.BlackoutDates.AddDatesInPast(); 
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as Lekar).PrezimeLek.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0) 
                        || ((item as Lekar).ImeLek.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                        || ((item as Lekar).specijalizacija.ToString().IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgSearch.ItemsSource).Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // btn odustani
            Page uvidZakazaniTermini = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvidZakazaniTermini);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IzmeniIzabraniTermin();
        }

        private void IzmeniIzabraniTermin()
        {
           try
            {
                string datumTermina = TerminServis.FormatirajSelektovaniDatum(datum.SelectedDate.Value);
                string vremePocetka = vpp.Text;
                string vremeKraja = TerminServis.IzracunajVremeKrajaPregleda(vremePocetka);
                TipTermina tipTermina;
                if (combo.Text.Equals("Pregled"))
                {
                    tipTermina = TipTermina.Pregled;
                }
                else
                {
                    tipTermina = TipTermina.Operacija;
                }
                Termin noviTermin = new Termin(termin.IdTermin, datumTermina, vremePocetka, vremeKraja, tipTermina);
                noviTermin.Pacijent = prijavljeniPacijent;
                noviTermin.Pomeren = true;

                SaleServis.ObrisiZauzeceSale(termin.Prostorija.Id, termin.IdTermin);
                ZauzeceSale zs = new ZauzeceSale(vremePocetka, vremeKraja, datumTermina, noviTermin.IdTermin);
                prvaSlobodnaSala.zauzetiTermini.Add(zs);
                noviTermin.Prostorija = prvaSlobodnaSala;
                PostaviLekaraZaNoviTermin(noviTermin);
                TerminServis.IzmeniTermin(termin, noviTermin);
                Page uvidZakazaniTermini = new ZakazaniTerminiPacijent(idPacijent);
                this.NavigationService.Navigate(uvidZakazaniTermini);
            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PostaviLekaraZaNoviTermin(Termin noviTermin)
        {
            if (dgSearch.SelectedItems.Count > 0)
            {
                Lekar selLekar = (Lekar)dgSearch.SelectedItems[0];
                noviTermin.Lekar = selLekar;
            }
        }

        private void dgSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSearch.SelectedItems.Count > 0)
            {
                Lekar item = (Lekar)dgSearch.SelectedItems[0];
                imePrz.Text = item.ToString();
            }
        }

        private void imePrz_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {


        }
        #region Pomeri termin
        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaleZaPreglede = TerminServis.combo_SelectionChanged(this.combo, null, null, idPacijent);
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
            if (prvaSlobodnaSala == null) // ovo se nikad nece dogoditi
            {
                MessageBox.Show("Ne postoji slobodan termin", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

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
