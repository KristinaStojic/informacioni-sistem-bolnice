using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for PreferencaTermini.xaml
    /// </summary>
    public partial class PreferencaTermini : Page
    {
        private static int idPacijent;
        private static Pacijent prijavljeniPacijent;
        private static int maxBrojPreporucenihTermina = 3;
        private Termin preporuceniTermin;
        private static ObservableCollection<string> SviSlobodniSlotovi { get; set; }
        private static ObservableCollection<string> PomocnaSviSlobodniSlotovi { get; set; }
        private ObservableCollection<Termin> Termini { get; set; }
        public PreferencaTermini(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPrijavljenogPacijenta);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PrikaziTermin.AktivnaTema(this.zaglavlje, this.svetlaTema);
            idPacijent = idPrijavljenogPacijenta;
            SviSlobodniSlotovi = SaleMenadzer.InicijalizujSveSlotove();
            PomocnaSviSlobodniSlotovi = SaleMenadzer.InicijalizujSveSlotove();
            Termini = new ObservableCollection<Termin>();
            PronadjiPreporuceneTermine(prijavljeniPacijent);
            preferencaGrid.ItemsSource = Termini;
        }

        private void preferencaGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void IzbaciProsleSlotoveZaDanasnjiDan()
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

        private void PronadjiPreporuceneTermine(Pacijent prijavljeniPacijent)
        {
            int brojacPreporucenihTermina = 0;
            bool jeTri = false;
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.TipSale.Equals(tipSale.SalaZaPregled))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        DateTime noviDatum = DateTime.Now.Date.AddDays(i); // tri dana unapred
                        if (i == 0)
                        {
                            IzbaciProsleSlotoveZaDanasnjiDan();
                        }
                        foreach (ZauzeceSale zs in s.zauzetiTermini)
                        {
                            DateTime zsDatum = DateTime.Parse(zs.datumPocetkaTermina);
                            foreach (string slot in SviSlobodniSlotovi)
                            {
                                if (!s.zauzetiTermini.Exists(x => x.datumPocetkaTermina.Equals(noviDatum)) && zs.idTermina != 0)
                                {
                                    preporuceniTermin = new Termin();
                                    preporuceniTermin.IdTermin = TerminMenadzer.GenerisanjeIdTermina();
                                    preporuceniTermin.Datum = noviDatum.ToString("MM/dd/yyyy");
                                    preporuceniTermin.VremePocetka = slot;
                                    preporuceniTermin.VremeKraja = ZakaziTermin.IzracunajVremeKrajaPregleda(slot);
                                    preporuceniTermin.Prostorija = s;
                                    preporuceniTermin.tipTermina = TipTermina.Pregled;
                                    // TODO: ispraviti kada dobijemo raspored radnog vremena
                                    preporuceniTermin.Lekar = prijavljeniPacijent.IzabraniLekar;
                                    preporuceniTermin.Pacijent = prijavljeniPacijent;

                                    Termini.Add(preporuceniTermin);
                                    brojacPreporucenihTermina++;
                                    if (brojacPreporucenihTermina == maxBrojPreporucenihTermina)
                                    {
                                        jeTri = true;
                                        return;
                                    }
                                }
                                else
                                {
                                    if (!s.zauzetiTermini.Exists(x => x.pocetakTermina.Equals(slot)) && zs.idTermina != 0)
                                    {
                                        preporuceniTermin = new Termin();
                                        preporuceniTermin.IdTermin = TerminMenadzer.GenerisanjeIdTermina();
                                        preporuceniTermin.Datum = zs.datumPocetkaTermina;
                                        preporuceniTermin.VremePocetka = slot;
                                        preporuceniTermin.VremeKraja = ZakaziTermin.IzracunajVremeKrajaPregleda(slot);
                                        preporuceniTermin.Prostorija = s;
                                        preporuceniTermin.tipTermina = TipTermina.Pregled;
                                        // TODO: ispraviti kada dobijemo raspored radnog vremena
                                        preporuceniTermin.Lekar = prijavljeniPacijent.IzabraniLekar;
                                        preporuceniTermin.Pacijent = prijavljeniPacijent;

                                        Termini.Add(preporuceniTermin);
                                        //DodajNoviPreporuceniTermin(prijavljeniPacijent, s, zs, slot);
                                        brojacPreporucenihTermina++;
                                        if (brojacPreporucenihTermina == maxBrojPreporucenihTermina)
                                        {
                                            jeTri = true;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnZakazi_Click(object sender, RoutedEventArgs e)
        {
            Termin termin = (Termin)preferencaGrid.SelectedItem;
            // TODO: sacuvati u listu zauzetih termina, srediti id termina
            if (termin == null)
            {
                MessageBox.Show("Oznacite termin koji zelite da zakazete", "Upozorenje", MessageBoxButton.OK);
                return;
            }
            TerminMenadzer.ZakaziTermin(termin);

            // TODO: proveriti
            Sala sala = SaleMenadzer.NadjiSaluPoId(termin.Prostorija.Id);
            ZauzeceSale novoZauzeceSale = new ZauzeceSale(termin.VremePocetka, termin.VremeKraja, termin.Datum, termin.IdTermin);
            sala.zauzetiTermini.Add(novoZauzeceSale);
            SaleMenadzer.sacuvajIzmjene(); // ? 
            TerminMenadzer.sacuvajIzmene(); // ?

            Page prikaziTermin = new PrikaziTermin(termin.Pacijent.IdPacijenta);
            this.NavigationService.Navigate(prikaziTermin);
            
        }
        private void nazad_Click(object sender, RoutedEventArgs e)
        {
            Page zakazivanje = new ZakaziTermin(idPacijent);
            this.NavigationService.Navigate(zakazivanje);
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
    }

}
