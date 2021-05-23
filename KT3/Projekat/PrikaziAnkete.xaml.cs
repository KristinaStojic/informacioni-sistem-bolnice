using Model;
using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Timers;
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
    public partial class PrikaziAnkete : Page
    {
        private static int idPacijent;
        public static ObservableCollection<Anketa> AnketePacijenta { get; set; }
        public PrikaziAnkete(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            this.potvrdi.IsEnabled = false;
            AnketePacijenta = new ObservableCollection<Anketa>();
            AnketePacijenta = AnketaServis.PrikaziSveAnketeZaProsleTermine(AnketePacijenta, idPacijent);
            listaAnketi.ItemsSource = AnketePacijenta;
            Pacijent prijavljeniPacijent = PacijentiServis.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PrikaziTermin.AktivnaTemaPagea(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
        }
        private void listaAnketi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Anketa anketa = (Anketa)listaAnketi.SelectedItem;
            if (anketa != null && !anketa.PopunjenaAnketa)
            {
                if (anketa.VrstaAnkete.Equals(VrstaAnkete.ZaKliniku))
                {
                    PrikaziAnketuZaKliniku anketaZaKliniku = new PrikaziAnketuZaKliniku(idPacijent, anketa.IdAnkete);
                    this.NavigationService.Navigate(anketaZaKliniku);
                }
                else
                {
                    PrikaziAntekuZaLekare anketaZaLekare = new PrikaziAntekuZaLekare(idPacijent, anketa.IdAnkete);
                    this.NavigationService.Navigate(anketaZaLekare);
                }
            }
        }

        private void odjava_Click(object sender, RoutedEventArgs e)
        {
            /*Page odjava = new PrijavaPacijent();
            this.NavigationService.Navigate(odjava);*/
            PacijentPagesServis.odjava_Click(this);
        }

        public void karton_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.karton_Click(this, idPacijent);
        }

        public void zakazi_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.zakazi_Click(this, idPacijent);
        }
        public void uvid_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.uvid_Click(this, idPacijent);
        }

        private void pocetna_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.pocetna_Click(this, idPacijent);
        }

        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.Korisnik_Click(this, idPacijent);
        }

        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.anketa_Click(this, idPacijent);
        }

        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.PromeniTemu(SvetlaTema, tamnaTema);
        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            /*var app = (App)Application.Current;
             // TODO: proveriti
             string eng = "en-US";
             string srb = "sr-LATN";
             MenuItem mi = (MenuItem)sender;
             if (mi.Header.Equals("en-US"))
             {
                 mi.Header = "sr-LATN";
                 app.ChangeLanguage(eng);
             }
             else
             {
                 mi.Header = "en-US";
                 app.ChangeLanguage(srb);
             }*/
            PacijentPagesServis.Jezik_Click(Jezik);
        }


    }
}
