using Model;
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
            terminZaBrisanje = zaBrisanje;
            idPacijent = zaBrisanje.Pacijent.IdPacijenta;
            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            //if (zaBrisanje.tipTermina.Equals(TipTermina.Pregled))
            this.tipTermina.Text = zaBrisanje.tipTermina.ToString();
            this.datum.Text = zaBrisanje.Datum;
            this.vreme.Text = zaBrisanje.VremePocetka;
            this.lekar.Text = zaBrisanje.Lekar.ToString();
            this.sala.Text = zaBrisanje.Prostorija.Id.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
            Page uvid = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvid);
        }

        private void odjava_Click(object sender, RoutedEventArgs e)
        {
            // TODO: prijava 
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // brisanje termina
            if (terminZaBrisanje != null)
            {
                TerminMenadzer.OtkaziTermin(terminZaBrisanje);
                //TerminMenadzer.sacuvajIzmene();
                Sala s = SaleMenadzer.NadjiSaluPoId(terminZaBrisanje.Prostorija.Id);
                foreach (ZauzeceSale zs in s.zauzetiTermini)
                {
                    if (zs.idTermina.Equals(terminZaBrisanje.IdTermin))
                    {
                        s.zauzetiTermini.Remove(zs);
                        break;
                    }
                }
            }
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
