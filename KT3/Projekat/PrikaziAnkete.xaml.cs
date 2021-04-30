using Model;
using Projekat.Model;
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
    /// <summary>
    /// Interaction logic for PrikaziAnkete.xaml
    /// </summary>
    public partial class PrikaziAnkete : Page
    {
        private static int idPacijent;
        public static ObservableCollection<Anketa> AnketePacijenta { get; set; }
        //private static System.Timers.Timer aTimer;
        private int brojacProslihTermina;
        public static int minBrojTerminaZaAnketuKlinika = 3;
        public PrikaziAnkete(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            this.potvrdi.IsEnabled = false;
            //PostaviTimer();
            AnketePacijenta = new ObservableCollection<Anketa>();
            PrikaziAnketeZaProsleTermine();
            OnemoguciVisestrukoPopunjavanjeIsteAnkete();
        }

        private void PrikaziAnketeZaProsleTermine()
        {
            foreach (Anketa anketa in AnketaMenadzer.ankete)
            {
                foreach (Termin termin in TerminMenadzer.pronadjiTerminPoIdPacijenta(idPacijent))
                {
                    DateTime datumTermina = DateTime.Parse(termin.Datum);
                    TimeSpan vremeKrajaTermina = TimeSpan.Parse(termin.VremeKraja);
                    if ((datumTermina == DateTime.Now.Date && vremeKrajaTermina <= DateTime.Now.TimeOfDay) || datumTermina < DateTime.Now.Date)
                    {
                        if (anketa.idTermina == termin.IdTermin)
                        {
                            brojacProslihTermina++;
                            AnketePacijenta.Add(anketa);
                        }
                        if (anketa.idTermina == 0) // TODO: magic number!
                        {
                            if (brojacProslihTermina == minBrojTerminaZaAnketuKlinika)
                            {
                                AnketePacijenta.Add(anketa);
                            }
                        }
                    }
                }
            }
        }

        private void OnemoguciVisestrukoPopunjavanjeIsteAnkete()
        {
            for(int i = 0; i < AnketePacijenta.Count(); i++)
            {
                if (AnketePacijenta[i].popunjenaAnketa == true)
                {
                    // TODO:
                }
            }
        }

        /*private static void PostaviTimer()
        {
            aTimer = new System.Timers.Timer(2000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            
        }*/

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

        private void datagridAnkete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Anketa anketa = (Anketa)datagridAnkete.SelectedItem;
            if (anketa != null)
            {
                if (anketa.vrstaAnkete.Equals(VrstaAnkete.ZaKliniku))
                {
                    PrikaziAnketuZaKliniku anketaZaKliniku = new PrikaziAnketuZaKliniku(idPacijent, anketa.idAnkete);
                    this.NavigationService.Navigate(anketaZaKliniku);
                }
                else
                {
                    PrikaziAntekuZaLekare anketaZaLekare = new PrikaziAntekuZaLekare(idPacijent, anketa.idAnkete);
                    this.NavigationService.Navigate(anketaZaLekare);
                }
            }
        }

        private void GridViewColumn_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

    }
}
