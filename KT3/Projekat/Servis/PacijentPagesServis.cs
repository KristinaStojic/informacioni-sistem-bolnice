using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Projekat.Servis
{
    public class PacijentPagesServis
    {
        public static void odjava_Click(Page nazivPagea)
        {
            Page odjava = new PrijavaPacijent();
            nazivPagea.NavigationService.Navigate(odjava);
        }

        public static void karton_Click(Page nazivPagea, int idPacijent)
        {
            Page karton = new ZdravstveniKartonPacijent(idPacijent);
            nazivPagea.NavigationService.Navigate(karton);
        }

        public static void zakazi_Click(Page nazivPagea, int idPacijent)
        {
            if (MalicioznoPonasanjeServis.DetektujMalicioznoPonasanje(idPacijent))
            {
                MessageBox.Show("Nije Vam omoguceno zakazivanje termina jer ste prekoracili dnevni limit modifikacije termina.", "Upozorenje", MessageBoxButton.OK);
                return;
            }
            Page zakaziTermin = new ZakaziTermin(idPacijent);
            nazivPagea.NavigationService.Navigate(zakaziTermin);
        }
        public static void uvid_Click(Page nazivPagea, int idPacijent)
        {
            Page uvid = new ZakazaniTerminiPacijent(idPacijent);
            nazivPagea.NavigationService.Navigate(uvid);
        }

        public static void pocetna_Click(Page nazivPagea, int idPacijent)
        {
            Page pocetna = new PrikaziTermin(idPacijent);
            nazivPagea.NavigationService.Navigate(pocetna);
        }

        public static void Korisnik_Click(Page nazivPagea, int idPacijent)
        {
            Page podaci = new LicniPodaciPacijenta(idPacijent);
            nazivPagea.NavigationService.Navigate(podaci);
        }

        public static void Podsetnik_Click(Page nazivPagea, int idPacijent)
        {
            Page dodajPodsetnik = new PodsetnikPacijent(idPacijent);
            nazivPagea.NavigationService.Navigate(dodajPodsetnik);
        }

        public static void PromeniTemu(MenuItem SvetlaTema, MenuItem tamnaTema)
        {
            var app = (App)Application.Current;
            if (SvetlaTema.IsEnabled)
            {
                //mi.Header = "Tamna";
                SvetlaTema.IsEnabled = false;
                tamnaTema.IsEnabled = true;
                app.ChangeTheme(new Uri("Teme/Svetla.xaml", UriKind.Relative));
            }
            else
            {
                //mi.Header = "Svetla";
                tamnaTema.IsEnabled = false;
                SvetlaTema.IsEnabled = true;
                app.ChangeTheme(new Uri("Teme/Tamna.xaml", UriKind.Relative));
            }
        }

        public static void Jezik_Click(MenuItem Jezik)
        {
            var app = (App)Application.Current;
            // TODO: proveriti
            string eng = "en-US";
            string srb = "sr-LATN";
            if (Jezik.Header.Equals("_en-US"))
            {
                Jezik.Header = "_sr-LATN";
                app.ChangeLanguage(eng);
            }
            else
            {
                Jezik.Header = "_en-US";
                app.ChangeLanguage(srb);
            }
        }

        public static void anketa_Click(Page nazivPagea, int idPacijent)
        {
            Page prikaziAnkete = new PrikaziAnkete(idPacijent);
            nazivPagea.NavigationService.Navigate(prikaziAnkete);
        }

    }
}
