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
    /// <summary>
    /// Interaction logic for ZakaziTerminPreferenca.xaml
    /// </summary>
    public partial class ZakaziTerminPreferenca : Page
    {
        public static int idPacijent;
        public static Pacijent prijavljeniPacijent;
        public static int idLekar = 1;
        public static int maxBrojPreporucenihTermina = 3;
        public List<string> sviSlobodni2 { get; set; }
        public ObservableCollection<Termin> Termini2 { get; set; }
        public static List<Termin> lista;
        public Termin t;
        public ZakaziTerminPreferenca(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            this.nazad.Visibility = Visibility.Hidden;
            Termini2 = new ObservableCollection<Termin>();
            sviSlobodni2 = new List<string>() { "07:00", "07:30", "08:00", "08:30",
                                                "09:00", "09:30",  "10:00", "10:30",
                                                "11:00", "11:30", "12:00", "12:30",
                                                "13:00", "13:30", "14:00", "14:30",
                                                "15:00", "15:30", "16:00", "16:30",
                                                "17:00", "17:30", "18:00", "18:30",
                                                "19:00", "19:30", "20:00"};
            // na startu
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
            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);

            int brojacPreporucenihTermina = 0;
            lista = new List<Termin>();
            bool jeTri = false;


            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.TipSale.Equals(tipSale.SalaZaPregled))
                {
                    foreach (ZauzeceSale zs in s.zauzetiTermini)
                    {
                        DateTime zsDatum = DateTime.Parse(zs.datumPocetkaTermina);
                        DateTime noviDatum = DateTime.Now.AddDays(3); // tri dana unapred
                       // MessageBox.Show("Novi datum: " + noviDatum.ToString() + " trenutni datum: " + DateTime.Now.ToString() );
                        if (DateTime.Compare(zsDatum, noviDatum) < 0 && jeTri == false)
                        {
                            foreach (string slot in sviSlobodni2)
                            {
                                if (!s.zauzetiTermini.Exists(x => x.pocetakTermina.Equals(slot)) && jeTri == false)
                                {
                                    t = new Termin();
                                    t.IdTermin = TerminMenadzer.GenerisanjeIdTermina();
                                    t.Datum = zs.datumPocetkaTermina;
                                    t.VremePocetka = slot;
                                    t.VremeKraja = ZakaziTermin.IzracunajVremeKraja(slot);
                                    t.Prostorija = s;
                                    t.tipTermina = TipTermina.Pregled;
                                    // TODO: ispraviti kada dobijemo raspored radnog vremena
                                    foreach (Lekar l in MainWindow.lekari)
                                    {
                                        if (l.IdLekara.Equals(idLekar))
                                        {
                                            t.Lekar = l ;
                                            break;
                                        }
                                        break;
                                    }

                                    // TODO: isparivi kada uradimo prijavljivanje
                                    //Pacijent p = PacijentiMenadzer.PronadjiPoId(idPacijent);
                                    t.Pacijent = prijavljeniPacijent;
                                    brojacPreporucenihTermina++;
                                    Termini2.Add(t);
                                    if (brojacPreporucenihTermina == maxBrojPreporucenihTermina)
                                    {
                                        jeTri = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
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
            //this.Close();
            
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
            // prosledjuje se lekar
            Lekar l = null;
            if (datagridLekari.SelectedItems.Count > 0)
            {
                l = (Lekar)datagridLekari.SelectedItems[0];
            }
            // TODO: kada se bude prosledjivao prijavljeni pacijent, samo set-ovati lekara
            ZakaziTermin zt = new ZakaziTermin(idPacijent);
            this.NavigationService.Navigate(zt);
        }

        public string imePrz_Changed()
        {
            if (datagridLekari.SelectedItems.Count > 0)
            {
                Lekar l = (Lekar)datagridLekari.SelectedItems[0];
                return l.ToString();
            }
            return null;
        }

        private void btnZakazi_Click(object sender, RoutedEventArgs e)
        {
            Termin t = (Termin)preferencaGrid.SelectedItem;
            //MessageBox.Show(t.Datum);
            // TODO: sacuvati u listu zauzetih termina, srediti id termina
            TerminMenadzer.ZakaziTermin(t);


            // TODO: proveriti
            Sala sala = SaleMenadzer.NadjiSaluPoId(t.Prostorija.Id);
            ZauzeceSale novoZauzeceSale = new ZauzeceSale(t.VremePocetka, t.VremeKraja, t.Datum, t.IdTermin);
            sala.zauzetiTermini.Add(novoZauzeceSale);
            SaleMenadzer.sacuvajIzmjene();
            TerminMenadzer.sacuvajIzmene();

            Page prikaziTermin = new PrikaziTermin(t.Pacijent.IdPacijenta);
            this.NavigationService.Navigate(prikaziTermin);
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
    }
}
