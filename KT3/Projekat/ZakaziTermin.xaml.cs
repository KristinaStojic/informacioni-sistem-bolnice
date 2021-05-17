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
        private static int maksimalniJednocifren = 9;
        private static int oznakaZaRenoviranje = 0;
        private static int idPacijent;
        public static Lekar izabraniLekar { get; set; }
        private List<Sala> SaleZaPreglede;
        private Sala prvaSlobodnaSala;
        private int ukupanBrojSalaZaPregled;
        private static Pacijent prijavljeniPacijent;
        private static ObservableCollection<string> SviSlobodniSlotovi { get; set; }
        private static ObservableCollection<string> PomocnaSviSlobodniSlotovi { get; set; }
        private static List<string> SviZauzetiZaSelektovaniDatum { get; set; }
        private static ObservableCollection<Uput> UputiPacijenta { get; set; }
        private static bool selektovanUput;
        public ZakaziTermin(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            InicijalizujPodatkeNaWpf(idPrijavljenogPacijenta);
            PomocnaSviSlobodniSlotovi = SaleServis.InicijalizujSveSlotove();
            PrikaziTermin.AktivnaTema(this.zaglavlje, this.svetlaTema);
            this.combo.SelectedIndex = 0;
        }

        private void InicijalizujPodatkeNaWpf(int idPrijavljenogPacijenta)
        {
            //datum.BlackoutDates.AddDatesInPast();
            idPacijent = idPrijavljenogPacijenta;
            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            InicijalizujPodatkeOLekaru();
            UputiPacijenta = new ObservableCollection<Uput>();
            DodajUputePacijenta(UputiPacijenta, idPacijent);
            // TODO: dodati naziv uputa u klasu!
            comboUputi.ItemsSource = UputiPacijenta;
            selektovanUput = false;
        }

        private static void DodajUputePacijenta(ObservableCollection<Uput> uputiPacijenta, int idPacijent)
        {
            foreach (Uput uput in ZdravstveniKartonMenadzer.PronadjiSveSpecijalistickeUputePacijenta(idPacijent))
            {
                uputiPacijenta.Add(uput);
            }
        }

        private void InicijalizujPodatkeOLekaru()
        {
            if (izabraniLekar == null )
            {
                izabraniLekar = prijavljeniPacijent.IzabraniLekar;
            }
            this.imePrz.Text = izabraniLekar.ToString();
        }

        // Zakazivanje termina - button click
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
            catch (System.Exception)
            {
                MessageBox.Show("Morate popuniti sva polja kako biste zakazali termin", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PokupiPodatkeZaZakazivanjeTermina()
        {
            int brojTermina = TerminMenadzer.GenerisanjeIdTermina();
            String datumTermina = FormatirajSelektovaniDatum(datum.SelectedDate.Value);
            String vremePocetka = vpp.Text;
            String vremeKraja = IzracunajVremeKrajaPregleda(vremePocetka);
            TipTermina tipTermina = TipTermina.Pregled;
            /*if (combo.Text.Equals("Pregled"))
            {
                tipTermina = TipTermina.Pregled;
            }
            else
            {
                tipTermina = TipTermina.Operacija;
            }*/
            Termin termin = new Termin(brojTermina, datumTermina, vremePocetka, vremeKraja, tipTermina);
            Pacijent pacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            termin.Pacijent = pacijent;
            termin.Lekar = izabraniLekar;
            DodajZauzeceSale(termin);
            termin.Prostorija = prvaSlobodnaSala;
            TerminMenadzer.ZakaziTermin(termin);

            AnketaMenadzer.DodajAnketuZaLekara(termin, idPacijent);
            ProveriAnketuZaKliniku();
            MalicioznoPonasanjeMenadzer.DodajMalicioznoPonasanje(idPacijent);
        }

        private void DodajZauzeceSale(Termin termin)
        {
            ZauzeceSale zs = new ZauzeceSale(termin.VremePocetka, termin.VremeKraja, termin.Datum, termin.IdTermin);
            prvaSlobodnaSala.zauzetiTermini.Add(zs);
        }

        private static void ProveriAnketuZaKliniku()
        {
            int brojacTermina = 0;
            foreach(Termin termin in TerminMenadzer.PronadjiTerminPoIdPacijenta(idPacijent)) 
            {
                brojacTermina++;
                if (brojacTermina == PrikaziAnkete.minBrojTerminaZaAnketuKlinika && !AnketaMenadzer.SveAnketePacijenta(idPacijent).Exists(x => x.IdTermina == AnketaMenadzer.oznakaAnketeZaKliniku))
                {
                    AnketaMenadzer.DodajAnketuZaKliniku(idPacijent);
                    return;
                }
            }
        }

        public static string IzracunajVremeKrajaPregleda(string vp)
        {
            string hh = vp.Substring(0, 2);
            string mm = vp.Substring(3);
            if (mm == "30")
            {
                int jednocifrenSat = int.Parse(hh);
                jednocifrenSat++;
                if (jednocifrenSat <= maksimalniJednocifren)
                {
                    return "0" + jednocifrenSat.ToString() + ":00";
                }
                else
                {
                    return jednocifrenSat.ToString() + ":00";
                }
            }
            else
            {
                return hh + ":30";
            }
        }

        private void ElektronskoPlacanje(object sender, RoutedEventArgs e)
        {
            // elektronsko placanje
            MessageBox.Show("Elektronsko placanje ce uskoro biti implementirano", "Obavestenje");
        }

        private void preferenca_Click(object sender, RoutedEventArgs e)
        {
            Page ztp = new ZakaziTerminPreferenca(idPacijent);
            this.NavigationService.Navigate(ztp);
        }

        /*  ---------------------- ZAKAZIVANJE TERMINA ---------------------- */
        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaleZaPreglede = SaleServis.PronadjiSaleZaPregled();
            ukupanBrojSalaZaPregled = SaleZaPreglede.Count();
            if (combo.Text.Equals("Pregled"))
            {
                this.comboUputi.IsEnabled = true;
                this.preferenca.IsEnabled = false;
                selektovanUput = true;
            }
            else if (combo.Text.Equals("Specijalistički pregled"))
            {
                this.comboUputi.IsEnabled = false;
                this.preferenca.IsEnabled = true;
                selektovanUput = false;
            }
        }

        private static int ParsirajSateVremenskogSlota(String vreme)
        {
            String sat = vreme.Split(':')[0];
            return Convert.ToInt32(sat);
        }

        private static int ParsirajMinuteVremenskogSlota(String vreme)
        {
            String minuti = vreme.Split(':')[1];
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

        /* pacijent ne moze imati dva ili vise termina u istovremeno */
        private void UkloniZauzecaPacijentaZaSelektovaniDatum(string selektovaniDatum, ObservableCollection<string> PomocnaSviSlobodniSlotovi)
        {
            List<Termin> termini = TerminMenadzer.PronadjiSveTerminePacijentaZaSelektovaniDatum(idPacijent, selektovaniDatum);
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
            if (SaleZaPreglede == null)
            {
                MessageBox.Show("Izaberite tip termina", "Upozorenje", MessageBoxButton.OK);
                return;
            }
            string selektovaniDatum = FormatirajSelektovaniDatum(datum.SelectedDate.Value);
            SviSlobodniSlotovi = SaleServis.InicijalizujSveSlotove();
            UkoloniProsleSlotoveZaDanasnjiDatum(PomocnaSviSlobodniSlotovi);
            UkloniZauzecaPacijentaZaSelektovaniDatum(selektovaniDatum, PomocnaSviSlobodniSlotovi);
            UkolniSlotoveZauzeteUSvimSalama(PomocnaSviSlobodniSlotovi);
            vpp.ItemsSource = SviSlobodniSlotovi;
        }

        public static string FormatirajSelektovaniDatum(DateTime selektovaniDatum)
        {
            return selektovaniDatum.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }

        private void UkolniSlotoveZauzeteUSvimSalama(ObservableCollection<string> PomocnaSviSlobodniSlotovi)
        {
            SviZauzetiZaSelektovaniDatum = PronadjiSvaZauzecaZaSelektovaniDatum(SaleZaPreglede);
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

        private List<string> PronadjiSvaZauzecaZaSelektovaniDatum(List<Sala> SaleZaPreglede)
        {
            SviZauzetiZaSelektovaniDatum = new List<string>();
            foreach (Sala sala in SaleZaPreglede)
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
                /* provera da se selektovani datum poklapa sa nekim zakazanim terminom */
                else if (satiVreme == satiVremePocetka && minVreme == minVremePocetka)
                {
                    SviZauzetiZaSelektovaniDatum.Add(slot);
                }
            }
        }

        private void vpp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selektovaniDatum = FormatirajSelektovaniDatum(datum.SelectedDate.Value);
            string selektovaniSlot = vpp.SelectedValue.ToString();
            int satiVremeSelektovanogSlota = ParsirajSateVremenskogSlota(selektovaniSlot);
            /* Pronalazenje sale za koju je slobodan izabrani slot*/
            foreach (Sala sala in SaleZaPreglede)
            {
                bool postojiZauzece = ProveriVremeZauzecaZaTermine(selektovaniDatum, selektovaniSlot, sala) || ProveriVremeSvihZauzecaZaRenoviranje(selektovaniDatum, satiVremeSelektovanogSlota, sala);
                if (!postojiZauzece)
                {
                    prvaSlobodnaSala = sala;
                    return;
                }
            }
        }

        private bool ProveriVremeZauzecaZaTermine(string selektovaniDatum, string selektovaniSlot, Sala sala)
        {
            foreach (ZauzeceSale zauzece in sala.zauzetiTermini)
            {
                if (prvaSlobodnaSala != null) break;
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

        private bool ProveriVremeSvihZauzecaZaRenoviranje(string selektovaniDatum, int satiVreme, Sala sala)
        {
            foreach (ZauzeceSale zauzece in sala.zauzetiTermini)
            {
                if (zauzece.idTermina == oznakaZaRenoviranje)
                {
                    /* ukoliko renoviranje traje tokom jednog dana */
                    if (selektovaniDatum.Equals(zauzece.datumPocetkaTermina) && selektovaniDatum.Equals(zauzece.datumKrajaTermina))
                    {
                        int satiVremePocetka = ParsirajSateVremenskogSlota(zauzece.pocetakTermina);
                        int satiVremeKraja = ParsirajSateVremenskogSlota(zauzece.krajTermina);
                        if (satiVremePocetka <= satiVreme && satiVreme < satiVremeKraja)
                        {
                            return true;
                        }
                    }
                    /* ukoliko renoviranje traje vise dana, a termin se poklapa sa pocetkom zauzeca sale */
                    else if (selektovaniDatum.Equals(zauzece.datumPocetkaTermina))
                    {
                        int satiVremePocetka = ParsirajSateVremenskogSlota(zauzece.pocetakTermina);
                        if (satiVremePocetka <= satiVreme)
                        {
                            return true;
                        }
                    }
                    /* ukoliko renoviranje traje vise dana, a termin se poklapa sa krajem zauzeca sale */
                    else if (selektovaniDatum.Equals(zauzece.datumKrajaTermina))
                    {
                        int satiVremeKraja = ParsirajSateVremenskogSlota(zauzece.krajTermina);
                        if (satiVreme < satiVremeKraja)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /*  ------------------------------------------------------------------ */

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
        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            Page podaci = new LicniPodaciPacijenta(idPacijent);
            this.NavigationService.Navigate(podaci);
        }
        private void comboUputi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(combo.Text.Equals("Pregled"))
            {
                comboUputi.IsEnabled = false;
                return;
            }
            Uput izabraniUput = (Uput)comboUputi.SelectedItem;
            izabraniLekar = LekariMenadzer.NadjiPoId(izabraniUput.IdLekaraKodKogSeUpucuje); 
            this.imePrz.Text = izabraniLekar.ToString();
        }
    }

}
