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
    public partial class PrikaziAnketuZaKliniku : Page
    {
        private static int idPacijent;
        private static int idAnkete;
        private static string prvoPitanje = null;
        private static string drugoPitanje = null;
        private static string trecePitanje = null;
        private static string cetvrtoPitanje = null;
        private static string petoPitanje = null;
        PacijentiServis servis = new PacijentiServis();
        public PrikaziAnketuZaKliniku(int idPrijavljenogPacijenta, int idSelektovaneAnkete)
        {
            InitializeComponent();
            this.potvrdi.IsEnabled = false;
            Pacijent prijavljeniPacijent = servis.PronadjiPoId(idPrijavljenogPacijenta);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
            idPacijent = idPrijavljenogPacijenta;
            idAnkete = idSelektovaneAnkete;
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
            PoveriOdgovoreNaSvaPitanja();
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
            PoveriOdgovoreNaSvaPitanja();
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
            PoveriOdgovoreNaSvaPitanja();
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
            PoveriOdgovoreNaSvaPitanja();
        }

        private void jedan5_Click(object sender, RoutedEventArgs e)
        {
            petoPitanje = "5=";
            if ((bool)jedan5.IsChecked)
            {
                petoPitanje += "1";
            }
            else if ((bool)dva5.IsChecked)
            {
                petoPitanje += "2";
            }
            else if ((bool)tri5.IsChecked)
            {
                petoPitanje += "3";
            }
            else if ((bool)cetiri5.IsChecked)
            {
                petoPitanje += "4";
            }
            else if ((bool)pet5.IsChecked)
            {
                petoPitanje += "5";
            }
            PoveriOdgovoreNaSvaPitanja();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string odgovoriPacijenta = prvoPitanje + ";" + drugoPitanje + ";" + trecePitanje + ";" + cetvrtoPitanje + ";" + petoPitanje;
            Anketa anketa = AnketaServis.NadjiAnketuPoId(idAnkete);
            anketa.Odgovori = odgovoriPacijenta;
            anketa.PopunjenaAnketa = true;

            Page prikaziAnkete = new PrikaziAnkete(idPacijent);
            this.NavigationService.Navigate(prikaziAnkete);
        }

        private void PoveriOdgovoreNaSvaPitanja()
        {
            if (prvoPitanje != null && drugoPitanje != null && trecePitanje != null && cetvrtoPitanje != null && petoPitanje != null)
            {
                this.potvrdi.IsEnabled = true;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new AnketeZaKlinikuViewModel(this.NavigationService, idPacijent, idAnkete);

        }
    }
}
