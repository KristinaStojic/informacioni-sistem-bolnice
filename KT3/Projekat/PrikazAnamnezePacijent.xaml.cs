using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
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
    public partial class PrikazAnamnezePacijent : Page
    {

        public Pacijent prijavljeniPacijent;
        public Anamneza anamneza;
        public PrikazAnamnezePacijent(Pacijent izabraniPacijent, Anamneza izabranaAnamneza)
        {
            InitializeComponent();
            this.DataContext = this;
            prijavljeniPacijent = izabraniPacijent;
            anamneza = izabranaAnamneza;


            this.datumAnamneze.Text = anamneza.Datum;
            this.podaciLekar.Text = anamneza.ImePrezimeLekara;
            this.opisBolesti.Text = anamneza.OpisBolesti;
            this.terpaija.Text = anamneza.Terapija;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // nazad
            //this.Close();
        }

        private void odjava_Click(object sender, RoutedEventArgs e)
        {

        }

        public void karton_Click(object sender, RoutedEventArgs e)
        {
            Page karton = new ZdravstveniKartonPacijent(prijavljeniPacijent);
            this.NavigationService.Navigate(karton);
        }

        public void zakazi_Click(object sender, RoutedEventArgs e)
        {
            Lekar l = null;
            Page zakaziTermin = new ZakaziTermin(l);
            this.NavigationService.Navigate(zakaziTermin);
        }

        public void uvid_Click(object sender, RoutedEventArgs e)
        {
            // TODO ispraviti --> uvid u zakazane poseban page
            Page uvid = new PrikaziTermin();
            this.NavigationService.Navigate(uvid);
        }
    }
}
