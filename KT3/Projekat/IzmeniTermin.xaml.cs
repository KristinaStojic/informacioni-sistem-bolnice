using Model;
using Projekat.Model;
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
        private List<Sala> slobodneSaleZaPregled;
        private static ObservableCollection<string> SviSlobodniSlotovi { get; set; }
        private static ObservableCollection<string> PomocnaSviSlobodniSlotovi { get; set; }
        private Sala prvaSlobodnaSala;
        private List<string> vremeSala;
        private int ukupanBrojSalaZaPregled;
        private static Pacijent prijavljeniPacijent;
        private static List<string> SviZauzetiZaSelektovaniDatum { get; set; }
        private static int oznakaZaRenoviranje = 0;

        public IzmeniTermin(Termin izabraniTermin)
        {
            InitializeComponent();
            this.DataContext = this;
            this.termin = izabraniTermin;
            PomocnaSviSlobodniSlotovi = SaleMenadzer.sviSlotovi;
            idPacijent = izabraniTermin.Pacijent.IdPacijenta;
            OgraniciIzborNovogDatuma(izabraniTermin);
            InicijalizujPodatkeZaIzabraniTermin(izabraniTermin);
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dgSearch.ItemsSource);
            view.Filter = UserFilter;
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PrikaziTermin.AktivnaTema(this.zaglavlje, this.svetlaTema);
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
                //idLek = izabraniTermin.Lekar;
                this.datum.DisplayDate = DateTime.Parse(izabraniTermin.Datum);
                InicijalizujSelektovanogLekara(izabraniTermin);
                //this.vpp.SelectedItem = izabraniTermin.VremePocetka;
            }
        }

        // TODO: ispraviti
        private void InicijalizujSelektovanogLekara(Termin izabraniTermin)
        {
            int brojac = 0;
            this.dgSearch.ItemsSource = MainWindow.lekari;
           
            foreach (Lekar lekar in MainWindow.lekari)
            {
                brojac++;
                if (lekar.IdLekara.Equals(izabraniTermin.Lekar.IdLekara))
                {
                    this.dgSearch.SelectedItems[0] = izabraniTermin.Lekar;
                    return;
                }
            }
        }

        private void OgraniciIzborNovogDatuma(Termin izabraniTermin)
        {
            // TODO: provera da li su  ti datumi u proslosti
            CalendarDateRange daniPreTermina = new CalendarDateRange();
            daniPreTermina.Start = DateTime.Parse(izabraniTermin.Datum).AddDays(-1000);//DateTime.Parse(izabraniTermin.Datum).AddDays(3);
            daniPreTermina.End = DateTime.Parse(izabraniTermin.Datum).AddDays(-3);
            datum.BlackoutDates.Add(daniPreTermina);
            CalendarDateRange daniPosleTermina = new CalendarDateRange();
            daniPosleTermina.Start = DateTime.Parse(izabraniTermin.Datum).AddDays(3);
            daniPosleTermina.End = DateTime.Parse(izabraniTermin.Datum).AddDays(1000);
            datum.BlackoutDates.Add(daniPosleTermina);
            //datum.BlackoutDates.AddDatesInPast();
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
            if (termin.Pomeren == false)
            {
                IzmeniIzabraniTermin();
            }
            else
            {
                MessageBox.Show("Nemoguce je pomeriti ovaj termin, dozvoljeni broj pomeranja termina je jedan", "Upozorenje");
            }
            Page uvid = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvid);
        }

        private void IzmeniIzabraniTermin()
        {
           try
            {
                string datumTermina = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string vremePocetka = vpp.Text;
                string vremeKraja = ZakaziTermin.IzracunajVremeKrajaPregleda(vremePocetka);
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

                SaleMenadzer.ObrisiZauzeceSale(termin.Prostorija.Id, termin.IdTermin);
                ZauzeceSale zs = new ZauzeceSale(vremePocetka, vremeKraja, datumTermina, noviTermin.IdTermin);
                prvaSlobodnaSala.zauzetiTermini.Add(zs);
                noviTermin.Prostorija = prvaSlobodnaSala;
                if (dgSearch.SelectedItems.Count > 0)
                {
                    Lekar selLekar = (Lekar)dgSearch.SelectedItem;
                    noviTermin.Lekar = selLekar;
                }
                TerminMenadzer.IzmeniTermin(termin, noviTermin);
                Page uvidZakazaniTermini = new ZakazaniTerminiPacijent(idPacijent);
                this.NavigationService.Navigate(uvidZakazaniTermini);
            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            slobodneSaleZaPregled = SaleMenadzer.PronadjiSaleZaPregled();
            ukupanBrojSalaZaPregled = SaleMenadzer.UkupanBrojSalaZaPregled();
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


        private void UkloniZauzeteSlotove()
        {
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

        /* pacijent ne moze imati dva ili vise termina u isto vreme */
        private void UkloniZauzecaPacijentaZaSelektovaniDatum(string selektovaniDatum)
        {
            foreach (Termin termin in TerminMenadzer.PronadjiTerminPoIdPacijenta(idPacijent))
            {
                if (termin.Datum.Equals(selektovaniDatum))
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
        }

        private void datum_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            SviSlobodniSlotovi = SaleMenadzer.sviSlotovi;
            string selektovaniDatum = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            IzbaciProsleSlotoveZaDanasnjiDan();
            UkloniZauzecaPacijentaZaSelektovaniDatum(selektovaniDatum);
            if (slobodneSaleZaPregled != null)
            {
                PronadjiSvaZauzecaZaSelektovaniDatum();
            }
            else
            {
                MessageBox.Show("Prvo izberite tip termina", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            UkolniSlotoveZauzeteUSvimSalama();
            this.vpp.ItemsSource = SviSlobodniSlotovi;
        }

        private void PronadjiSvaZauzecaZaSelektovaniDatum()
        {
            SviZauzetiZaSelektovaniDatum = new List<string>();
            foreach (Sala sala in slobodneSaleZaPregled)
            {
                foreach (ZauzeceSale zauzeceSale in sala.zauzetiTermini)
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

        private void UkolniSlotoveZauzeteUSvimSalama()
        {
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

        private void IzbaciProsleSlotoveZaDanasnjiDan()
        {
            if (datum.SelectedDate == DateTime.Now.Date)
            {
                foreach (string slot in PomocnaSviSlobodniSlotovi)
                {
                    DateTime dt = DateTime.Parse(slot);
                    DateTime sada = DateTime.Now;
                    if (dt.TimeOfDay <= sada.TimeOfDay)
                    {
                        SviSlobodniSlotovi.Remove(slot);
                    }
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
            foreach (Sala sala in slobodneSaleZaPregled)
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

        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
            MenuItem mi = (MenuItem)sender;
            if (mi.Header.Equals("Svetla"))
            {
                mi.Header = "Tamna";
                app.ChangeTheme(new Uri("Teme/Svetla.xaml", UriKind.Relative));
            }
            else
            {
                mi.Header = "Svetla";
                app.ChangeTheme(new Uri("Teme/Tamna.xaml", UriKind.Relative));
            }
        }
    }
}
