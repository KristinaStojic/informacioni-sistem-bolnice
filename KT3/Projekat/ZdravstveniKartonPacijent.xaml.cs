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
using System.Windows.Shapes;

namespace Projekat
{
    public partial class ZdravstveniKartonPacijent : Page
    {
        public List<LekarskiRecept> tempRecepti;
        public List<Anamneza> tempAnamneze;
        public static int idPacijent;
        public static Pacijent prijavljeniPacijent;
        public ZdravstveniKartonPacijent(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            prijavljeniPacijent = PacijentiServis.PronadjiPoId(idPrijavljenogPacijenta);
            this.tabelaRecepata.ItemsSource = DodajLekarskeReceptePacijenta();
            this.prikazAnamnezi.ItemsSource = DodajAnamnezePacijenta();
            this.prikazUputa.ItemsSource = DodajUputePacijenta();
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PrikaziTermin.AktivnaTemaPagea(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
        }

        // TODO: dodati ove metode u Zdravstveni karton Servis
        private static ObservableCollection<Uput> DodajUputePacijenta()
        {
            ObservableCollection<Uput> uputiPacijenta = new ObservableCollection<Uput>();
            if (prijavljeniPacijent.Karton.Uputi.Count != 0)
            {
                foreach (Uput uput in prijavljeniPacijent.Karton.Uputi)
                {
                    uputiPacijenta.Add(uput);
                    
                }
            }
            return uputiPacijenta;
        }

        private List<LekarskiRecept> DodajLekarskeReceptePacijenta()
        {
            tempRecepti = new List<LekarskiRecept>();
            if (prijavljeniPacijent.Karton.LekarskiRecepti.Count != 0)
            {
                foreach (LekarskiRecept lekRecepti in prijavljeniPacijent.Karton.LekarskiRecepti)
                {
                    tempRecepti.Add(lekRecepti);
                }
            }
            return tempRecepti;
        }

        private List<Anamneza> DodajAnamnezePacijenta()
        {
            tempAnamneze = new List<Anamneza>();
            if (prijavljeniPacijent.Karton.Anamneze.Count != 0)
            {
                foreach (Anamneza anamneza in prijavljeniPacijent.Karton.Anamneze)
                {
                    tempAnamneze.Add(anamneza);
                }
            }
            return tempAnamneze;
        }

        private void tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /* LEKARSKI RECEPTI */
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // lekarski recepti - prikazi informacije
            if (tabelaRecepata.SelectedItems.Count > 0)
            {
                LekarskiRecept lp = (LekarskiRecept)tabelaRecepata.SelectedItem;
                Page recept = new Recept(lp, prijavljeniPacijent);
                this.NavigationService.Navigate(recept);
            }
            else
            {
                MessageBox.Show("Selektujte recept za koji želite da prikažete informacije", "Upozorenje", MessageBoxButton.OK);
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LekarskiRecept lp = (LekarskiRecept)tabelaRecepata.SelectedItem;
            Page recept = new Recept(lp, prijavljeniPacijent);
            this.NavigationService.Navigate(recept);
        }


        /* ANAMNEZE */
        private void infoAnamneza_Click(object sender, RoutedEventArgs e)
        {
            if (prikazAnamnezi.SelectedItems.Count > 0)
            {
                Anamneza anamneza = (Anamneza)prikazAnamnezi.SelectedItem;
                PrikazAnamnezePacijent anamnezaPrikaz = new PrikazAnamnezePacijent(prijavljeniPacijent, anamneza);
                this.NavigationService.Navigate(anamnezaPrikaz);
            }
            else
            {
                MessageBox.Show("Selektujte anamnezu za koju želite da prikažete informacije", "Upozorenje", MessageBoxButton.OK);
            }
        }

        private void prikazAnamnezi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Anamneza anamneza = (Anamneza)prikazAnamnezi.SelectedItem;
            PrikazAnamnezePacijent anamnezaPrikaz = new PrikazAnamnezePacijent(prijavljeniPacijent, anamneza);
            this.NavigationService.Navigate(anamnezaPrikaz);
        }


        private void prikazUputa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO: uput za stacionarno lecenje i uput za labratoriju
            Uput uput = (Uput)prikazUputa.SelectedItem;
            if (uput.TipUputa.Equals(tipUputa.SpecijallistickiPregled))
            {
                Page detaljiUputa = new DetaljiUputaPacijent(idPacijent, uput);
                this.NavigationService.Navigate(detaljiUputa);
            }
            
        }

        private void odjava_Click(object sender, RoutedEventArgs e)
        {
            /*Page odjava = new PrijavaPacijent();
            this.NavigationService.Navigate(odjava);*/
            PacijentPagesServis.odjava_Click(this);
        }

        public void karton_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.karton_Click(this, idPacijent);
        }

        public void zakazi_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.zakazi_Click(this, idPacijent);
        }
        public void uvid_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.uvid_Click(this, idPacijent);
        }

        private void pocetna_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.pocetna_Click(this, idPacijent);
        }
        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.anketa_Click(this, idPacijent);
        }

        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.PromeniTemu(SvetlaTema, tamnaTema);
        }

        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.Podsetnik_Click(this, idPacijent);
        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.Jezik_Click(Jezik);
        }

    }
}
