using Model;
using Projekat.Model;
using Projekat.Servis;
using Projekat.ViewModel;
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
        private static ObservableCollection<Anketa> AnketePacijenta { get; set; }
        PacijentiServis servis = new PacijentiServis();

        public PrikaziAnkete(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            Pacijent prijavljeniPacijent = servis.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
            this.potvrdi.IsEnabled = false;
            AnketePacijenta = new ObservableCollection<Anketa>();
            AnketaServis anketaServis = new AnketaServis();
            AnketePacijenta = anketaServis.PrikaziSveAnketeZaProsleTermine(AnketePacijenta, idPacijent);
            listaAnketi.ItemsSource = AnketePacijenta;
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
        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.anketa_Click(this, idPacijent);
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

        private void Podsetnik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Podsetnik_Click(this, idPacijent);
        }

        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.PromeniTemu(SvetlaTema, tamnaTema);
        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Jezik_Click(Jezik);

        }

        // mvvm
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new AnketeViewModel(this.NavigationService, idPacijent);
        }
    }
}
