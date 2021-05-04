﻿using System;
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
using static Model.Termin;

namespace Projekat
{
    public partial class ZakaziTermin : Page
    {
        private static int maksimalniJednocifren = 9;
        private static int oznakaZaRenoviranje = 0;
        private static int idPacijent;
        public static Lekar izabraniLekar;
        private List<Sala> slobodneSaleZaPreglede;
        private Sala prvaSlobodnaSala;
        private int ukupanBrojSalaZaPregled;
        private static Pacijent prijavljeniPacijent;
        private static ObservableCollection<string> SviSlobodniSlotovi { get; set; }
        private static ObservableCollection<string> PomocnaSviSlobodniSlotovi { get; set; }
        private static List<string> SviZauzetiZaSelektovaniDatum { get; set; }
        public ZakaziTermin(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            InicijalizujPodatkeNaWpf(idPrijavljenogPacijenta);
            PomocnaSviSlobodniSlotovi = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30", "11:00", "11:30", "12:00", "12:30",
                                                                "13:00", "13:30", "14:00", "14:30","15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00"};
        }

        private void InicijalizujPodatkeNaWpf(int idPrijavljenogPacijenta)
        {
            datum.BlackoutDates.AddDatesInPast();
            idPacijent = idPrijavljenogPacijenta;
            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            InicijalizujPodatkeOLekaru();
        }

        private void InicijalizujPodatkeOLekaru()
        {
            if (izabraniLekar != null)
            {
                this.imePrz.Text = izabraniLekar.ToString();
            }
            else
            {
                izabraniLekar = prijavljeniPacijent.IzabraniLekar;
                this.imePrz.Text = izabraniLekar.ToString();
            }
        }

        // Zakazivanje termina
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                DetektujMalicioznoPonasanjePacijenta();
                Page uvid = new ZakazaniTerminiPacijent(idPacijent);
                this.NavigationService.Navigate(uvid);
            }
            catch (System.Exception)
            {
                MessageBox.Show("Morate popuniti sva polja kako biste zakazali termin", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DetektujMalicioznoPonasanjePacijenta()
        {
            if (!MalicioznoPonasanjeMenadzer.DetektujMalicioznoPonasanje(idPacijent))
            {
                PokupiPodatkeZaZakazivanjeTermina();
            }
            else
            {
                MessageBox.Show("Nije Vam omoguceno zakazivanje termina jer ste prekoracili maksimalni broj modifikacije termina u danu.", "Upozorenje", MessageBoxButton.OK);
            }
        }

        private void PokupiPodatkeZaZakazivanjeTermina()
        {
            int brojTermina = TerminMenadzer.GenerisanjeIdTermina();
            String datumTermina = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            String vremePocetka = vpp.Text;
            String vremeKraja = IzracunajVremeKrajaPregleda(vremePocetka);
            TipTermina tipTermina;
            if (combo.Text.Equals("Pregled"))
            {
                tipTermina = TipTermina.Pregled;
            }
            else
            {
                tipTermina = TipTermina.Operacija;
            }
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
                if (brojacTermina == PrikaziAnkete.minBrojTerminaZaAnketuKlinika)
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
            // prozor za odabir lekara po preferenci
            Page ztp = new ZakaziTerminPreferenca(idPacijent);
            this.NavigationService.Navigate(ztp);
        }

        /*  -------- ZAKAZIVANJE TERMINA ---------- */
        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            slobodneSaleZaPreglede = new List<Sala>();
            ukupanBrojSalaZaPregled = 0;
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.TipSale.Equals(tipSale.SalaZaPregled))
                {
                    slobodneSaleZaPreglede.Add(s);
                    ukupanBrojSalaZaPregled++;
                }
            }
        }

        public void IzbaciProsleSlotoveZaDanasnjiDan()
        {
            if (datum.SelectedDate == DateTime.Now.Date)
            {
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


       

        /* pacijent ne moze imati dva ili vise termina u isto vreme */
        private void UkloniZauzecaPacijenta(string selektovaniDatum)
        {
            foreach(Termin termin in TerminMenadzer.PronadjiTerminPoIdPacijenta(idPacijent))
            {
                if (termin.Datum.Equals(selektovaniDatum)) 
                {
                    foreach(string slot in PomocnaSviSlobodniSlotovi)
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
            string selektovaniDatum = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            SviSlobodniSlotovi = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30","11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00"};

            IzbaciProsleSlotoveZaDanasnjiDan();
            UkloniZauzecaPacijenta(selektovaniDatum);
            if (slobodneSaleZaPreglede != null)
            {
                PronadjiSvaZauzecaZaSelektovaniDatum();
            }
            else
            {
                MessageBox.Show("Prvo izberite tip termina", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            UkloniZauzeteSlotove();
            vpp.ItemsSource = SviSlobodniSlotovi;
            /*if (!sviSlobodniSlotovi.Any()) // ovo mozda i ne moram --> web
            {
                MessageBox.Show("Ne postoji nijedan slobodan termin za izabrani datum, molimo Vas izaberite drugi datum", "Upozorenje");
            }*/
        }

        private void PronadjiSvaZauzecaZaSelektovaniDatum()
        {
            SviZauzetiZaSelektovaniDatum = new List<string>();
            foreach (Sala s in slobodneSaleZaPreglede)
            {
                foreach (ZauzeceSale zs in s.zauzetiTermini)
                {
                    DateTime datumPocetkaZauzeca = DateTime.Parse(zs.datumPocetkaTermina);
                    DateTime datumKrajaZauzeca = DateTime.Parse(zs.datumKrajaTermina);
                    int satiVremePocetka = ParsirajSateVremenskogSlota(zs.pocetakTermina);
                    int minVremePocetka = ParsirajMinuteVremenskogSlota(zs.pocetakTermina);
                    int satiVremeKraja = ParsirajSateVremenskogSlota(zs.krajTermina);
                    /* provera za termine i renoviranje(u periodu jednog dana - nekoliko sati) */
                    if (datumPocetkaZauzeca.Equals(datum.SelectedDate) && datumKrajaZauzeca.Equals(datum.SelectedDate))
                    {
                        foreach (string slot in PomocnaSviSlobodniSlotovi)
                        {
                            int satiVreme = ParsirajSateVremenskogSlota(slot);
                            int minVreme = ParsirajMinuteVremenskogSlota(slot);
                            /* provera u slucaju da renoviranje traje jedan dan */
                            if (zs.idTermina == oznakaZaRenoviranje)
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
                    /* ukoliko je selektovani datum u periodu renoviranja sale */
                    else if (datumPocetkaZauzeca < datum.SelectedDate && datum.SelectedDate < datumKrajaZauzeca)
                    {
                        foreach (string slot in PomocnaSviSlobodniSlotovi)
                        {
                            SviZauzetiZaSelektovaniDatum.Add(slot);
                        }
                    }
                    /* provera da li se selektovani datum poklapa sa pocetkom renoviranja sale - slobodni termini pre renoviranja */
                    else if (datumPocetkaZauzeca == datum.SelectedDate)
                    {
                        foreach (string slot in PomocnaSviSlobodniSlotovi)
                        {
                            int satiVreme = ParsirajSateVremenskogSlota(slot);
                            if (satiVreme >= satiVremePocetka)
                            {
                                SviZauzetiZaSelektovaniDatum.Add(slot);
                            }
                        }
                    }
                    /* provera da li se selektovani datum poklapa sa krajem renoviranja sale - slobodni termini posle renoviranja */
                    else if (datumKrajaZauzeca == datum.SelectedDate)
                    {
                        foreach (string slot in PomocnaSviSlobodniSlotovi)
                        {
                            int satiVreme = ParsirajSateVremenskogSlota(slot);
                            if (satiVreme < satiVremeKraja)
                            {
                                SviZauzetiZaSelektovaniDatum.Add(slot);
                            }
                        }
                    }
                }
            }
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

        private bool ProveriVremeZauzecaZaRenoviranje(string selektovaniDatum, int satiVreme, Sala sala)
        {
            foreach (ZauzeceSale zauzece in sala.zauzetiTermini)
            {
                if (prvaSlobodnaSala != null) break;

                if (zauzece.idTermina == oznakaZaRenoviranje)
                {
                    int satiVremePocetka = ParsirajSateVremenskogSlota(zauzece.pocetakTermina);
                    int satiVremeKraja = ParsirajSateVremenskogSlota(zauzece.krajTermina);
                    /* ukoliko renoviranje traje tokom jednog dana */
                    if (selektovaniDatum.Equals(zauzece.datumPocetkaTermina) && selektovaniDatum.Equals(zauzece.datumKrajaTermina))
                    {
                        if (satiVremePocetka <= satiVreme && satiVreme < satiVremeKraja)
                        {
                            return true;
                        }
                    }
                    /* ukoliko renoviranje traje vise dana, a termin se poklapa sa pocetkom zauzeca sale */
                    else if (selektovaniDatum.Equals(zauzece.datumPocetkaTermina))
                    {
                        if (satiVremePocetka <= satiVreme)
                        {
                            return true;
                        }
                    }
                    /* ukoliko renoviranje traje vise dana, a termin se poklapa sa krajem zauzeca sale */
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

        private void vpp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selektovaniDatum = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string selektovaniSlot = vpp.SelectedValue.ToString();
            int satiVreme = ParsirajSateVremenskogSlota(selektovaniSlot);
            /* Pronalazenje sale za koju je slobodan izabrani slot*/
            foreach (Sala sala in slobodneSaleZaPreglede)
            {
                bool postojiZauzece = ProveriVremeZauzecaZaTermine(selektovaniDatum, selektovaniSlot, sala) || ProveriVremeZauzecaZaRenoviranje(selektovaniDatum, satiVreme, sala);
                if (!postojiZauzece)
                {
                    prvaSlobodnaSala = sala;
                    return;
                }
            }
        }
        /*  --------------------------- */

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
    }
}
