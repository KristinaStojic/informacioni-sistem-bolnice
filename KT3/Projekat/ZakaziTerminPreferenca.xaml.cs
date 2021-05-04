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
using System.Windows.Shapes;

namespace Projekat
{
    public partial class ZakaziTerminPreferenca : Page
    {
        private static int idPacijent;
        private static int maxBrojPreporucenihTermina = 3;
        private Termin preporuceniTermin;
        private static Pacijent prijavljeniPacijent { get; set; }
        private List<string> sviSlotovi { get; set; }
        private static ObservableCollection<string> pomocnaSviSlobodniSlotovi { get; set; }
        private ObservableCollection<Termin> Termini { get; set; }
        public ZakaziTerminPreferenca(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            this.nazad.Visibility = Visibility.Hidden;
            Termini = new ObservableCollection<Termin>();
            sviSlotovi = new List<string>() { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00"};
            pomocnaSviSlobodniSlotovi = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30", "11:00", "11:30", "12:00", "12:30",
                                                                "13:00", "13:30", "14:00", "14:30","15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30", "19:00", "19:30", "20:00"};
            this.preferencaGrid.Visibility = Visibility.Hidden;
            this.btnZakazi.Visibility = Visibility.Hidden;
            this.nazad.Visibility = Visibility.Hidden;
            this.grupa.Visibility = Visibility.Hidden;
            this.datagridLekari.Visibility = Visibility.Hidden;
            this.txtFilter.Visibility = Visibility.Hidden;
            this.zakaziLekar.Visibility = Visibility.Hidden;

            this.datagridLekari.ItemsSource = MainWindow.lekari;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(datagridLekari.ItemsSource);
            view.Filter = UserFilter;

            PronadjiPreporuceneTermine(prijavljeniPacijent);
            preferencaGrid.ItemsSource = Termini;
        }

        public void IzbaciProsleSlotoveZaDanasnjiDan()
        {
            foreach (string slot in pomocnaSviSlobodniSlotovi)
            {
                DateTime vreme = DateTime.Parse(slot);
                DateTime sada = DateTime.Now;
                if (vreme.TimeOfDay <= sada.TimeOfDay)
                {
                    sviSlotovi.Remove(slot);
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
                            foreach (string slot in sviSlotovi)
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

        private void DodajNoviPreporuceniTermin(Pacijent prijavljeniPacijent, Sala s, ZauzeceSale zs, string slot)
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
        }

        private void preferencaGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as Lekar).PrezimeLek.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0) || ((item as Lekar).ImeLek.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Lekar).specijalizacija.ToString().IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0); ;
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(datagridLekari.ItemsSource).Refresh();
        }

        private void nazad_Click(object sender, RoutedEventArgs e)
        {
            Page zakazivanje = new ZakaziTermin(idPacijent);
            this.NavigationService.Navigate(zakazivanje);
        }

        private void lekari_Click(object sender, RoutedEventArgs e)
        {
            this.grupa.Visibility = Visibility.Visible;
            this.datagridLekari.Visibility = Visibility.Visible;
            this.txtFilter.Visibility = Visibility.Visible;
            this.nazad.Visibility = Visibility.Visible;

            this.lekari.Visibility = Visibility.Hidden;
            this.preporuka.Visibility = Visibility.Hidden;
            this.zakaziLekar.Visibility = Visibility.Visible;

        }

        private void preporuka_Click(object sender, RoutedEventArgs e)
        {
            this.preferencaGrid.Visibility = Visibility.Visible;
            this.btnZakazi.Visibility = Visibility.Visible;
            this.nazad.Visibility = Visibility.Visible;

            this.lekari.Visibility = Visibility.Hidden;
            this.preporuka.Visibility = Visibility.Hidden;
            this.zakaziLekar.Visibility = Visibility.Hidden;

        }

        private void datagridLekari_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridLekari.SelectedItems.Count > 0)
            {
                Lekar item = (Lekar)datagridLekari.SelectedItem;
                //mePrz.Text = item.ToString();
            }

        }

        private void zakaziLekar_Click(object sender, RoutedEventArgs e)
        {
            Lekar l = null;
            if (datagridLekari.SelectedItems.Count > 0)
            {
                l = (Lekar)datagridLekari.SelectedItems[0];
            }
            ZakaziTermin.izabraniLekar = l;
            Page zt = new ZakaziTermin(idPacijent);
            this.NavigationService.Navigate(zt);
        }

        private void btnZakazi_Click(object sender, RoutedEventArgs e)
        {
            Termin t = (Termin)preferencaGrid.SelectedItem;
            // TODO: sacuvati u listu zauzetih termina, srediti id termina
            if (t != null)
            {
                TerminMenadzer.ZakaziTermin(t);

                // TODO: proveriti
                Sala sala = SaleMenadzer.NadjiSaluPoId(t.Prostorija.Id);
                ZauzeceSale novoZauzeceSale = new ZauzeceSale(t.VremePocetka, t.VremeKraja, t.Datum, t.IdTermin);
                sala.zauzetiTermini.Add(novoZauzeceSale);
                SaleMenadzer.sacuvajIzmjene();
                TerminMenadzer.sacuvajIzmene();

                Page prikaziTermin = new PrikaziTermin(t.Pacijent.IdPacijenta);
                this.NavigationService.Navigate(prikaziTermin);
            } else
            {
                MessageBox.Show("Oznacite termin koji zelite da zakazete", "Upozorenje", MessageBoxButton.OK);
            }
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
