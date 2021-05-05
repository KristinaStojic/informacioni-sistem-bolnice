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
        private static int oznakaAnketeZaKliniku = 0;
        public PrikaziAnkete(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            this.potvrdi.IsEnabled = false;
            AnketePacijenta = new ObservableCollection<Anketa>();
            PrikaziAnketeZaProsleTermine();
            listaAnketi.ItemsSource = AnketePacijenta;
            Pacijent prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PrikaziTermin.AktivnaTema(this.zaglavlje, this.svetlaTema);
        }

        private void PrikaziAnketeZaProsleTermine()
        {
            brojacProslihTermina = 0;
            foreach (Anketa anketa in AnketaMenadzer.ankete)
            {
                foreach (Termin termin in TerminMenadzer.PronadjiTerminPoIdPacijenta(idPacijent))
                {
                    DateTime datumTermina = DateTime.Parse(termin.Datum);
                    TimeSpan vremeKrajaTermina = TimeSpan.Parse(termin.VremeKraja);
                    if ((datumTermina == DateTime.Now.Date && vremeKrajaTermina <= DateTime.Now.TimeOfDay) || datumTermina < DateTime.Now.Date)
                    {
                        if (anketa.idTermina == oznakaAnketeZaKliniku) 
                        {
                            if (brojacProslihTermina == minBrojTerminaZaAnketuKlinika) /* posle 3. termina - anketa o radu klinike */
                            {
                                AnketePacijenta.Add(anketa);
                                break; 
                            }
                        }
                        if (anketa.idTermina == termin.IdTermin)
                        {
                            brojacProslihTermina++;
                            AnketePacijenta.Add(anketa);
                        }
                    }
                }
            }
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

        private void GridViewColumn_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void listaAnketi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Anketa anketa = (Anketa)listaAnketi.SelectedItem;
            if (anketa != null && !anketa.popunjenaAnketa)
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

        private void popunjenCheckBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {

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
