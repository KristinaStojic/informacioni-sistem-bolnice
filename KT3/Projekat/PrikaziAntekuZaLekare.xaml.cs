using Model;
using Projekat.Model;
using Projekat.Servis;
using Projekat.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat
{
    public partial class PrikaziAntekuZaLekare : Page
    {
        private static string prvoPitanje = null;
        private static string drugoPitanje = null;
        private static string trecePitanje = null;
        private static string cetvrtoPitanje = null;
        private static int idPacijent;
        private static int idAnkete;
        PacijentiServis servis = new PacijentiServis();

        public PrikaziAntekuZaLekare(int idPrijavljenogPacijenta, int idSelektovaneAnkete)
        {
            InitializeComponent();
            Pacijent prijavljeniPacijent = servis.PronadjiPoId(idPrijavljenogPacijenta);
            this.podaci.Header = PacijentWebStranice.podaciPacijenta(prijavljeniPacijent);
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
            this.potvrdi.IsEnabled = false;
            idPacijent = idPrijavljenogPacijenta;
            idAnkete = idSelektovaneAnkete;

            AnketeZaLekaraServis anketeZaLekaraServis = new AnketeZaLekaraServis();
            lekar.Content += anketeZaLekaraServis.PrikaziNaslovAnkete(idAnkete);
        }

        public void jedan1_Click(object sender, RoutedEventArgs e)
        {
            prvoPitanje = "1=";
            if ((bool)jedan1.IsChecked)
            {
                prvoPitanje += "1";
            }
            else if ((bool)dva1.IsChecked)
            {
                prvoPitanje += "2";
            }
            else if ((bool)tri1.IsChecked)
            {
                prvoPitanje += "3";
            }
            else if ((bool)cetiri1.IsChecked)
            {
                prvoPitanje += "4";
            }
            else if ((bool)pet1.IsChecked)
            {
                prvoPitanje += "5";
            }
            odgovorenoNaSvaPitanja();
        }

        private void jedan2_Click(object sender, RoutedEventArgs e)
        {
            drugoPitanje = "2=";
            if ((bool)jedan2.IsChecked)
            {
                drugoPitanje += "1";
            }
            else if ((bool)dva2.IsChecked)
            {
                drugoPitanje += "2";
            }
            else if ((bool)tri2.IsChecked)
            {
                drugoPitanje += "3";
            }
            else if ((bool)cetiri2.IsChecked)
            {
                drugoPitanje += "4";
            }
            else if ((bool)pet2.IsChecked)
            {
                drugoPitanje += "5";
            }
            odgovorenoNaSvaPitanja();
        }

        private void jedan3_Click(object sender, RoutedEventArgs e)
        {
            trecePitanje = "3=";
            if ((bool)jedan3.IsChecked)
            {
                trecePitanje += "1";
            }
            else if ((bool)dva3.IsChecked)
            {
                trecePitanje += "2";
            }
            else if ((bool)tri3.IsChecked)
            {
                trecePitanje += "3";
            }
            else if ((bool)cetiri3.IsChecked)
            {
                trecePitanje += "4";
            }
            else if ((bool)pet3.IsChecked)
            {
                trecePitanje += "5";
            }
            odgovorenoNaSvaPitanja();
        }

        private void jedan4_Click(object sender, RoutedEventArgs e)
        {
            cetvrtoPitanje = "4=";
            if ((bool)jedan4.IsChecked)
            {
                cetvrtoPitanje += "1";
            }
            else if ((bool)dva4.IsChecked)
            {
                cetvrtoPitanje += "2";
            }
            else if ((bool)tri4.IsChecked)
            {
                cetvrtoPitanje += "3";
            }
            else if ((bool)cetiri4.IsChecked)
            {
                cetvrtoPitanje += "4";
            }
            else if ((bool)pet4.IsChecked)
            {
                cetvrtoPitanje += "5";
            }
            odgovorenoNaSvaPitanja();
        }

        private void odgovorenoNaSvaPitanja()
        {
            if (prvoPitanje != null && drugoPitanje != null && trecePitanje != null && cetvrtoPitanje != null)
            {
                this.potvrdi.IsEnabled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string odgovoriPacijenta = prvoPitanje + ";" + drugoPitanje + ";" + trecePitanje + ";" + cetvrtoPitanje;
            AnketaServis anketaServis = new AnketaServis();
            Anketa anketa = anketaServis.NadjiAnketuPoId(idAnkete);
            anketa.Odgovori = odgovoriPacijenta;
            anketa.PopunjenaAnketa = true;
            anketaServis.sacuvajIzmene();

            Page prikaziAnkete = new PrikaziAnkete(idPacijent);
            this.NavigationService.Navigate(prikaziAnkete);
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

        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.anketa_Click(this, idPacijent);
        }

        // mvvm
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new AnketeZaLekaraViewModel(this.NavigationService, idPacijent, idAnkete);
        }
    }

}
