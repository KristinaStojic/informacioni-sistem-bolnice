﻿using Model;
using Projekat.Model;
using Projekat.Servis;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
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
        PacijentiServis servis = new PacijentiServis();

        public ZdravstveniKartonPacijent(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            prijavljeniPacijent = servis.PronadjiPoId(idPrijavljenogPacijenta);
            this.tabelaRecepata.ItemsSource = DodajLekarskeReceptePacijenta();
            this.prikazAnamnezi.ItemsSource = DodajAnamnezePacijenta();
            this.prikazUputa.ItemsSource = ZdravstveniKartonServis.DodajUputePacijenta(prijavljeniPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
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
            Uput uput = (Uput)prikazUputa.SelectedItem;
            if (uput.TipUputa.Equals(tipUputa.SpecijalistickiPregled))
            {
                Page detaljiUputa = new DetaljiUputaPacijent(idPacijent, uput);
                this.NavigationService.Navigate(detaljiUputa);
            }
            else if (uput.TipUputa.Equals(tipUputa.StacionarnoLecenje))
            {
                Page detaljiUputa = new DetaljiSpecijalistickogUputa(idPacijent, uput);  // page -  detalji stac uputa 
                this.NavigationService.Navigate(detaljiUputa);
            }
            else if (uput.TipUputa.Equals(tipUputa.Laboratorija))
            {
                Page detaljiLabUputa = new DetaljiLabUputaPacijent(uput, idPacijent);
                this.NavigationService.Navigate(detaljiLabUputa);
            }
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
