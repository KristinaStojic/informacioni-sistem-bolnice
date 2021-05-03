﻿using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for OtkaziTermin.xaml
    /// </summary>
    public partial class OtkaziTermin : Page
    {
        public static Pacijent prijavljeniPacijent;
        public static int idPacijent;
        public static Termin terminZaBrisanje;
        public OtkaziTermin(Termin zaBrisanje)
        {
            InitializeComponent();
            this.DataContext = this;
            terminZaBrisanje = zaBrisanje;
            idPacijent = zaBrisanje.Pacijent.IdPacijenta;
            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            this.tipTermina.Text = zaBrisanje.tipTermina.ToString();
            this.datum.Text = zaBrisanje.Datum;
            this.vreme.Text = zaBrisanje.VremePocetka;
            this.lekar.Text = zaBrisanje.Lekar.ToString();
            this.sala.Text = zaBrisanje.Prostorija.Id.ToString();
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
        }

        // BRISANJE TERMINA
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!MalicioznoPonasanjeMenadzer.DetektujMalicioznoPonasanje(idPacijent))
            {
                OtkaziOdabraniTermin();
            }
            else
            {
                MessageBox.Show("Nije Vam omoguceno otkazivanje termina jer ste prekoracili maksimalni broj modifikacije termina u danu.", "Upozorenje", MessageBoxButton.OK);
            }
            Page uvid = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvid);
        }

        private static void OtkaziOdabraniTermin()
        {
            TerminMenadzer.OtkaziTermin(terminZaBrisanje);
            MalicioznoPonasanjeMenadzer.DodajMalicioznoPonasanje(idPacijent);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Page uvid = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvid);
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

        }
    }
}
