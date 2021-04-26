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
using Model;
using Projekat.Model;

namespace Projekat
{
    public partial class Recept : Page
    {
        public Pacijent prijavljeniPacijent;
        public LekarskiRecept lekRec;
        public static int idPacijent;
        public Recept(LekarskiRecept lp, Pacijent izabraniPacijent)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = izabraniPacijent.IdPacijenta;

            this.lekRec = lp;
            this.naziv.Text = lp.NazivLeka;
            this.datum.Text = lp.DatumPropisivanjaLeka;
            this.dani.Text = lp.BrojDanaKoriscenja.ToString();
            this.brojUzimanja.Text = lp.BrojDanaKoriscenja.ToString();
            this.sati.Text = lp.PocetakKoriscenja.Substring(0, 2);
            this.min.Text = lp.PocetakKoriscenja.Substring(3);

            this.naziv.IsEnabled = false;
            this.datum.IsEnabled = false;
            this.dani.IsEnabled = false;
            this.brojUzimanja.IsEnabled = false;
            this.sati.IsEnabled = false;
            this.min.IsEnabled = false;

            this.prijavljeniPacijent = izabraniPacijent;
            ime.Text = izabraniPacijent.ImePacijenta;
            prezime.Text = izabraniPacijent.PrezimePacijenta;
            id.Text = izabraniPacijent.IdPacijenta.ToString();

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
