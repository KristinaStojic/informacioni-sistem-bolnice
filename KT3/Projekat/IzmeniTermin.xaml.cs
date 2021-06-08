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
    public partial class IzmeniTermin : Page
    {
        private Termin termin;
        private static int idPacijent;
        private List<Sala> SaleZaPreglede;
        private Sala prvaSlobodnaSala;
        private static Pacijent prijavljeniPacijent;
        public IzmeniTermin(Termin izabraniTermin)
        {
            InitializeComponent();
            this.DataContext = this;
            this.termin = izabraniTermin;
            idPacijent = izabraniTermin.Pacijent.IdPacijenta;
            OgraniciIzborNovogDatuma(izabraniTermin);
            InicijalizujPodatkeZaIzabraniTermin(izabraniTermin);
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dgSearch.ItemsSource);
            view.Filter = UserFilter;
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
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
            Page uvidZakazaniTermini = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvidZakazaniTermini);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IzmeniIzabraniTermin();
        }

        private void IzmeniIzabraniTermin()
        {
            try {
                string datumTermina = TerminServis.FormatirajSelektovaniDatum(datum.SelectedDate.Value);
                string vremePocetka = vpp.Text;
                string vremeKraja = TerminServis.IzracunajVremeKrajaPregleda(vremePocetka);
                TipTermina tipTermina = OdrediTipTermina();
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
            catch(Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private TipTermina OdrediTipTermina()
        {
            TipTermina tipTermina;
            if (combo.Text.Equals("Pregled"))
            {
                tipTermina = TipTermina.Pregled;
            }
            else
            {
                tipTermina = TipTermina.Operacija;
            }

            return tipTermina;
        }

        private void PostaviLekaraZaNoviTermin(Termin noviTermin)
        {
            if (dgSearch.SelectedItems.Count > 0)
            {
                Lekar selLekar = (Lekar)dgSearch.SelectedItems[0];
                noviTermin.Lekar = selLekar;
            }
        }

        // tabela lekara
        private void dgSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSearch.SelectedItems.Count > 0)
            {
                Lekar item = (Lekar)dgSearch.SelectedItems[0];
                imePrz.Text = item.ToString();
            }
        }

        #region Pomeri termin
        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sacuvaj.IsEnabled = true;
            SaleZaPreglede = TerminServis.combo_SelectionChanged(this.combo, null, null, idPacijent);
        }

        private void datum_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            sacuvaj.IsEnabled = true;
            if (SaleZaPreglede == null)
            {
                MessageBox.Show("Izaberite tip termina", "Upozorenje", MessageBoxButton.OK);
                return;
            }
            vpp.ItemsSource = TerminServis.datum_SelectedDatesChanged(datum);

        }

        private void vpp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sacuvaj.IsEnabled = true;
            prvaSlobodnaSala = TerminServis.Vpp_SelectionChanged(vpp, datum);
            if (prvaSlobodnaSala == null) 
            {
                MessageBox.Show("Ne postoji slobodan termin", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        #region Pacijent web stanice
        private void odjava_Click(object sender, RoutedEventArgs e)
        {
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

        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Korisnik_Click(this, idPacijent);
        }

        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.PromeniTemu(SvetlaTema, tamnaTema);

        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Jezik_Click(Jezik);
        }
        #endregion
    }


}
