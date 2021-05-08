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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikaziAntekuZaLekare.xaml
    /// </summary>
    public partial class PrikaziAntekuZaLekare : Page
    {
        private static string prvoPitanje = null;
        private static string drugoPitanje = null;
        private static string trecePitanje = null;
        private static string cetvrtoPitanje = null;
        private static string petoPitanje = null;
        private static int idPacijent;
        private static int idAnkete;
        public PrikaziAntekuZaLekare(int idPrijavljenogPacijenta, int idSelektovaneAnkete)
        {
            InitializeComponent();
            idPacijent = idPrijavljenogPacijenta;
            idAnkete = idSelektovaneAnkete;
            Lekar lekar = pronadjiLekaraZaAnketu(idAnkete);
            this.lekar.Content = "Anketa o radu lekara (" + lekar.ImeLek + " " + lekar.PrezimeLek + ")";
            Pacijent prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PrikaziTermin.AktivnaTema(this.zaglavlje, this.svetlaTema);
        }

        private static Lekar pronadjiLekaraZaAnketu(int idAnkete)
        {
            Anketa anketa = AnketaMenadzer.NadjiAnketuPoId(idAnkete);
            Termin termin = TerminMenadzer.NadjiTerminPoId(anketa.IdTermina);
            return termin.Lekar;
        }

        public void jedan1_Click(object sender, RoutedEventArgs e)
        {
            // brPitanja = odgovor ; ......
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
            if (MalicioznoPonasanjeMenadzer.DetektujMalicioznoPonasanje(idPacijent))
            {
                MessageBox.Show("Nije Vam omoguceno zakazivanje termina jer ste prekoracili dnevni limit modifikacije termina.", "Upozorenje", MessageBoxButton.OK);
                return;
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string odgovoriPacijenta = prvoPitanje + ";" + drugoPitanje + ";" + trecePitanje + ";" + cetvrtoPitanje + ";" + petoPitanje;
            Anketa anketa = AnketaMenadzer.NadjiAnketuPoId(idAnkete);
            anketa.Odgovori = odgovoriPacijenta;
            anketa.PopunjenaAnketa = true;

            Page prikaziAnkete = new PrikaziAnkete(idPacijent);
            this.NavigationService.Navigate(prikaziAnkete);
        }

        private void odgovorenoNaSvaPitanja()
        {
            if (prvoPitanje != null && drugoPitanje != null && trecePitanje != null && cetvrtoPitanje != null && petoPitanje != null)
            {
                this.potvrdi.IsEnabled = true;
            }
        }

        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            Page prikaziAnkete = new PrikaziAnkete(idPacijent);
            this.NavigationService.Navigate(prikaziAnkete);
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
    }

}
