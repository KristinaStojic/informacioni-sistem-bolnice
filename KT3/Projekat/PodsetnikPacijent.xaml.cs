﻿using Model;
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
    /// Interaction logic for PodsetnikPacijent.xaml
    /// </summary>
    public partial class PodsetnikPacijent : Page
    {
        private static int idPacijent;
        private static Pacijent prijavljeniPacijent;
        private static ObservableCollection<Obavestenja> obavestenjaPodsetnici;
        public PodsetnikPacijent(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            obavestenjaPodsetnici = new ObservableCollection<Obavestenja>();
            PrikaziTermin.AktivnaTema(this.zaglavlje, this.svetlaTema);
            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            InicijalizujPodsetnikePacijenta(obavestenjaPodsetnici);
            Podsetnici.ItemsSource = obavestenjaPodsetnici;
        }

        private static void InicijalizujPodsetnikePacijenta(ObservableCollection<Obavestenja> obavestenjaPodsetnici)
        {
            foreach (Obavestenja obavestenje in ObavestenjaMenadzer.PronadjiObavestenjaPoIdPacijenta(idPacijent))
            {
                if (obavestenje.TipObavestenja.Equals("Podsetnik"))
                {
                    obavestenjaPodsetnici.Add(obavestenje);
                }
            }
        }

        private void DodajPodsetnik_Click(object sender, RoutedEventArgs e)
        {
            string vremePodsetnika = Vreme.Text;
            string datumPodsetnika = Datum.SelectedDate.Value.ToString("MM/dd/yyyy") + " " + vremePodsetnika;
            string sadrzajPodsetnika = SadrzajPodsetnika.Text;

            List<int> pacijenti = new List<int>();
            pacijenti.Add(idPacijent);
            Obavestenja obavestenjeZaPodsetnik = new Obavestenja(ObavestenjaMenadzer.GenerisanjeIdObavestenja(), datumPodsetnika, "Podsetnik", sadrzajPodsetnika, pacijenti, true);
            ObavestenjaMenadzer.obavestenja.Add(obavestenjeZaPodsetnik);
            obavestenjaPodsetnici.Add(obavestenjeZaPodsetnik);

            Vreme.Text = null;
            Datum.Text = null;
            SadrzajPodsetnika.Text = null;
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

        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            Page prikaziAnkete = new PrikaziAnkete(idPacijent);
            this.NavigationService.Navigate(prikaziAnkete);
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
    }
}