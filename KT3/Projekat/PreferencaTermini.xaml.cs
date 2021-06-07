using Model;
using Projekat.Model;
using Projekat.Servis;
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
    public partial class PreferencaTermini : Page
    {
        private static int idPacijent;
        private static Pacijent prijavljeniPacijent;
        private ObservableCollection<Termin> Termini { get; set; }
        public PreferencaTermini(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            prijavljeniPacijent = PacijentiServis.PronadjiPoId(idPrijavljenogPacijenta);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
            idPacijent = idPrijavljenogPacijenta;
            
            Termini = new ObservableCollection<Termin>();
            PronadjiPreporuceneTermine(prijavljeniPacijent);
            preferencaGrid.ItemsSource = Termini;
        }

        private void PronadjiPreporuceneTermine(Pacijent prijavljeniPacijent)
        {
            int brojacPreporucenihTermina = 0;
            bool jeMaksimum = false;
            foreach (Sala sala in SaleServis.NadjiSveSale())
            {
                if (sala.TipSale.Equals(tipSale.SalaZaPregled))
                {
                     Termini = TerminServis.PronadjiPreporuceneTermine(prijavljeniPacijent, sala, brojacPreporucenihTermina, jeMaksimum);
                }
            }
        }

        
        private void btnZakazi_Click(object sender, RoutedEventArgs e)
        {
            Termin termin = (Termin)preferencaGrid.SelectedItem;
            if (termin == null)
            {
                MessageBox.Show("Oznacite termin koji zelite da zakazete", "Upozorenje", MessageBoxButton.OK);
                return;
            }
            TerminServis.ZakaziTermin(termin);
            Sala sala = SaleServis.NadjiSaluPoId(termin.Prostorija.Id);
            SaleServis.DodajZauzeceSale(termin, sala);

            TerminServis.sacuvajIzmene(); 
            SaleServis.sacuvajIzmjene(); 

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

        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.PromeniTemu(SvetlaTema, tamnaTema);
        }

        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Korisnik_Click(this, idPacijent);
        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Jezik_Click(Jezik);
        }

    }

}
