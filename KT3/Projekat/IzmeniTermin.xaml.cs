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
        private List<Sala> SaleZaPregled;
        private static ObservableCollection<string> SviSlobodniSlotovi { get; set; }
        private static ObservableCollection<string> PomocnaSviSlobodniSlotovi { get; set; }
        private Sala prvaSlobodnaSala;
        private int ukupanBrojSalaZaPregled;
        private static Pacijent prijavljeniPacijent;
        private static List<string> SviZauzetiZaSelektovaniDatum { get; set; }
        private static int oznakaZaRenoviranje = 0;

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
                prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
                this.datum.DisplayDate = DateTime.Parse(izabraniTermin.Datum);
                InicijalizujSelektovanogLekara(izabraniTermin);
            }
        }

        private void InicijalizujSelektovanogLekara(Termin izabraniTermin)
        {
            int brojac = 0;
            this.dgSearch.ItemsSource = LekariMenadzer.lekari;
            foreach (Lekar lekar in LekariMenadzer.lekari)
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
            CalendarDateRange daniPreTermina = new CalendarDateRange();
            daniPreTermina.Start = DateTime.Parse(izabraniTermin.Datum).AddDays(-1000);//DateTime.Parse(izabraniTermin.Datum).AddDays(3);
            daniPreTermina.End = DateTime.Parse(izabraniTermin.Datum).AddDays(-3);
            datum.BlackoutDates.Add(daniPreTermina);
            CalendarDateRange daniPosleTermina = new CalendarDateRange();
            daniPosleTermina.Start = DateTime.Parse(izabraniTermin.Datum).AddDays(3);
            daniPosleTermina.End = DateTime.Parse(izabraniTermin.Datum).AddDays(1000);
            datum.BlackoutDates.Add(daniPosleTermina);
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

        /*  IZMENI TERMIN */
        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaleZaPregled = SaleServis.PronadjiSaleZaPregled();
            ukupanBrojSalaZaPregled = SaleZaPregled.Count();
        }

        private static int ParsirajSateVremenskogSlota(string vreme)
        {
            string sat = vreme.Split(':')[0];
            return Convert.ToInt32(sat);
        }

        private static int ParsirajMinuteVremenskogSlota(string vreme)
        {
            string minuti = vreme.Split(':')[1];
            return Convert.ToInt32(minuti);
        }

        public void UkoloniProsleSlotoveZaDanasnjiDatum(ObservableCollection<string> PomocnaSviSlobodniSlotovi)
        {
            if (datum.SelectedDate != DateTime.Now.Date)
                return;
            foreach (string slot in PomocnaSviSlobodniSlotovi)
            {
                DateTime vreme = DateTime.Parse(slot);
                DateTime sada = DateTime.Now;
                if (vreme.TimeOfDay <= sada.TimeOfDay)
                {
                    SviSlobodniSlotovi.Remove(slot);
                }
            }
        }

        /* pacijent ne moze imati dva ili vise termina u isto vreme */
        private void UkloniZauzecaPacijentaZaSelektovaniDatum(string selektovaniDatum, ObservableCollection<string> PomocnaSviSlobodniSlotovi)
        {
            List<Termin> termini = TerminServis.PronadjiSveTerminePacijentaZaSelektovaniDatum(idPacijent, selektovaniDatum);
            foreach (Termin termin in termini)
            {
                foreach (string slot in PomocnaSviSlobodniSlotovi)
                {
                    if (termin.VremePocetka.Equals(slot))
                    {
                        SviSlobodniSlotovi.Remove(slot);
                    }
                }
            }
        }

        private void datum_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SaleZaPregled == null)
            {
                MessageBox.Show("Izaberite tip termina", "Upozorenje", MessageBoxButton.OK);
                return;
            }
            string selektovaniDatum = TerminServis.FormatirajSelektovaniDatum(datum.SelectedDate.Value);
            SviSlobodniSlotovi = SaleMenadzer.InicijalizujSveSlotove();
            string selektovaniDatum = ZakaziTermin.FormatirajSelektovaniDatum(datum.SelectedDate.Value);
            SviSlobodniSlotovi = SaleServis.InicijalizujSveSlotove();
            UkoloniProsleSlotoveZaDanasnjiDatum(PomocnaSviSlobodniSlotovi);
            UkloniZauzecaPacijentaZaSelektovaniDatum(selektovaniDatum, PomocnaSviSlobodniSlotovi);
            UkolniSlotoveZauzeteUSvimSalama(PomocnaSviSlobodniSlotovi);
            vpp.ItemsSource = SviSlobodniSlotovi;
        }


        private void UkolniSlotoveZauzeteUSvimSalama(ObservableCollection<string> PomocnaSviSlobodniSlotovi)
        {
            SviZauzetiZaSelektovaniDatum = PronadjiSvaZauzecaZaSelektovaniDatum();
            int brojacZauzetihSala;
            foreach (string slot in PomocnaSviSlobodniSlotovi)
            {
                brojacZauzetihSala = 0;
                foreach (string zauzeti in SviZauzetiZaSelektovaniDatum)
                {
                    if (slot.Equals(zauzeti))
                    {
                        brojacZauzetihSala++;
                        if (brojacZauzetihSala == ukupanBrojSalaZaPregled)
                        {
                            SviSlobodniSlotovi.Remove(slot);
                            break;
                        }
                    }
                }
            }
        }

        private List<string> PronadjiSvaZauzecaZaSelektovaniDatum()
        {
            SviZauzetiZaSelektovaniDatum = new List<string>();
            foreach (Sala sala in SaleZaPregled)
            {
                foreach (ZauzeceSale zauzeceSale in sala.zauzetiTermini)
                {
                    DodajZauzeceZaSelektovaniDatum(zauzeceSale);
                }
            }
            return SviZauzetiZaSelektovaniDatum;
        }

        private void DodajZauzeceZaSelektovaniDatum(ZauzeceSale zauzeceSale)
        {
            DateTime datumPocetkaZauzeca = DateTime.Parse(zauzeceSale.datumPocetkaTermina);
            DateTime datumKrajaZauzeca = DateTime.Parse(zauzeceSale.datumKrajaTermina);
            /* provera za termine i renoviranje(u periodu jednog dana - nekoliko sati) */
            if (datumPocetkaZauzeca.Equals(datum.SelectedDate) && datumKrajaZauzeca.Equals(datum.SelectedDate))
            {
                DodajZauzecaSaleZaTermine(zauzeceSale);
            }
            /* ukoliko je selektovani datum u periodu renoviranja sale */
            else if (datumPocetkaZauzeca < datum.SelectedDate && datum.SelectedDate < datumKrajaZauzeca)
            {
                DodajZauzecaSaleZaVremeRenoviranja();
            }
            /* provera da li se selektovani datum poklapa sa pocetkom renoviranja sale - slobodni termini pre renoviranja */
            else if (datumPocetkaZauzeca == datum.SelectedDate)
            {
                DodajZauzecaSaleZaPocetakRenoviranja(zauzeceSale);
            }
            /* provera da li se selektovani datum poklapa sa krajem renoviranja sale - slobodni termini posle renoviranja */
            else if (datumKrajaZauzeca == datum.SelectedDate)
            {
                DodajZauzecaSaleZaKrajRenoviranja(zauzeceSale);
            }
        }

        private static void DodajZauzecaSaleZaKrajRenoviranja(ZauzeceSale zauzeceSale)
        {
            foreach (string slot in PomocnaSviSlobodniSlotovi)
            {
                int satiVreme = ParsirajSateVremenskogSlota(slot);
                int satiVremeKraja = ParsirajSateVremenskogSlota(zauzeceSale.krajTermina);
                if (satiVreme < satiVremeKraja)
                {
                    SviZauzetiZaSelektovaniDatum.Add(slot);
                }
            }
        }

        private static void DodajZauzecaSaleZaPocetakRenoviranja(ZauzeceSale zauzeceSale)
        {
            foreach (string slot in PomocnaSviSlobodniSlotovi)
            {
                int satiVreme = ParsirajSateVremenskogSlota(slot);
                int satiVremePocetka = ParsirajSateVremenskogSlota(zauzeceSale.pocetakTermina);
                if (satiVreme >= satiVremePocetka)
                {
                    SviZauzetiZaSelektovaniDatum.Add(slot);
                }
            }
        }

        private static void DodajZauzecaSaleZaVremeRenoviranja()
        {
            /* ukoliko je selektovani datum u periodu renoviranja sale - ceo dan sala je zauzeta */
            foreach (string slot in PomocnaSviSlobodniSlotovi)
            {
                SviZauzetiZaSelektovaniDatum.Add(slot);
            }
        }

        private static void DodajZauzecaSaleZaTermine(ZauzeceSale zauzeceSale)
        {
            /* provera za termine i renoviranje(u periodu jednog dana - nekoliko sati) */
            foreach (string slot in PomocnaSviSlobodniSlotovi)
            {
                int satiVreme = ParsirajSateVremenskogSlota(slot);
                int minVreme = ParsirajMinuteVremenskogSlota(slot);
                int satiVremePocetka = ParsirajSateVremenskogSlota(zauzeceSale.pocetakTermina);
                int minVremePocetka = ParsirajMinuteVremenskogSlota(zauzeceSale.pocetakTermina);
                int satiVremeKraja = ParsirajSateVremenskogSlota(zauzeceSale.krajTermina);
                /* provera u slucaju da renoviranje traje jedan dan */
                if (zauzeceSale.idTermina == oznakaZaRenoviranje)
                {
                    if (satiVreme >= satiVremePocetka && satiVreme < satiVremeKraja)
                    {
                        SviZauzetiZaSelektovaniDatum.Add(slot);
                    }
                }
                /* provera da se selektovani datum poklapa sa nekim vec zakazanim terminom */
                else if (satiVreme == satiVremePocetka && minVreme == minVremePocetka)
                {
                    SviZauzetiZaSelektovaniDatum.Add(slot);
                }
            }
        }

         /* Vreme pocetka*/
        private void vpp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selektovaniDatum = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string selektovaniSlot = vpp.SelectedValue.ToString();
            int satiVreme = ParsirajSateVremenskogSlota(selektovaniSlot);

            /* Pronalazenje sale za koju je slobodan izabrani slot*/
            foreach (Sala sala in SaleZaPregled)
            {
                bool postojiZauzece = ProveriVremeZauzecaZaTermine(selektovaniDatum, selektovaniSlot, sala) || ProveriVremeZauzecaZaRenoviranje(selektovaniDatum, satiVreme, sala);
                //postojiZauzece = ProveriVremeZauzecaZaRenoviranje(selektovaniDatum, satiVreme, sala);
                if (!postojiZauzece)
                {
                    prvaSlobodnaSala = sala;
                    break;
                }
            }
        }

        private bool ProveriVremeZauzecaZaTermine(string selektovaniDatum, string selektovaniSlot, Sala sala)
        {
            foreach (ZauzeceSale zauzece in sala.zauzetiTermini)
            {
                if (prvaSlobodnaSala != null) break;
                // provera da li se poklapa sa terminom
                if (zauzece.idTermina != oznakaZaRenoviranje && zauzece.datumPocetkaTermina.Equals(selektovaniDatum))
                {
                    if (zauzece.pocetakTermina.Equals(selektovaniSlot))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ProveriVremeZauzecaZaRenoviranje(string selektovaniDatum, int satiVreme, Sala sala)
        {
            foreach (ZauzeceSale zauzece in sala.zauzetiTermini)
            {
                if (prvaSlobodnaSala != null) break;
                if (zauzece.idTermina == oznakaZaRenoviranje)
                {
                    int satiVremePocetka = ParsirajSateVremenskogSlota(zauzece.pocetakTermina);
                    int satiVremeKraja = ParsirajSateVremenskogSlota(zauzece.krajTermina);

                    if (selektovaniDatum.Equals(zauzece.datumPocetkaTermina) && selektovaniDatum.Equals(zauzece.datumKrajaTermina))
                    {
                        if (satiVremePocetka <= satiVreme && satiVreme < satiVremeKraja)
                        {
                            return true;
                        }
                    }
                    else if (selektovaniDatum.Equals(zauzece.datumPocetkaTermina))
                    {
                        if (satiVremePocetka <= satiVreme)
                        {
                            return true;
                        }
                    }
                    else if (selektovaniDatum.Equals(zauzece.datumKrajaTermina))
                    {
                        if (satiVreme < satiVremeKraja)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /*  -----------------------------------  */

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
            if (MalicioznoPonasanjeMenadzer.DetektujMalicioznoPonasanje(idPacijent))
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
