using Model;
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
    /// <summary>
    /// Interaction logic for ElektronskoPlacanjePacijent.xaml
    /// </summary>
    public partial class ElektronskoPlacanjePacijent : Page
    {
        private static int idPacijent;
        public ElektronskoPlacanjePacijent(int idPrijavaljenogPacijenta, TipTermina tip)
        {
            InitializeComponent();
            idPacijent = idPrijavaljenogPacijenta;
            OdrediCenuPregleda(tip);

            Pacijent prijavljeniPacijent = PacijentiServis.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
        }

        private void OdrediCenuPregleda(TipTermina tip)
        {
            if (tip.Equals(TipTermina.Pregled))
            {
                if (Jezik.Header.Equals("_sr-LATN"))
                {
                    cena.Text = "10 €";
                }
                else
                {
                    cena.Text = "1170 DIN";
                }
            }
            else
            {
                if (Jezik.Header.Equals("_sr-LATN"))
                {
                    cena.Text = "50 €";
                }
                else
                {
                    cena.Text = "6000 DIN";
                }
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ElektronskoPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            Page uvid = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvid);
        }
    }
}
