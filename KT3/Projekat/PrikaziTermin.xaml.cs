using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Model;
using Projekat.Model;
using System.Threading;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using Projekat.Lokalizacija;
using Projekat.Servis;

namespace Projekat
{
    public partial class PrikaziTermin : Page
    {
        public static Pacijent prijavljeniPacijent;
        private static int idPacijent;
        public static bool pacijentProzor;
        private int colNum = 0;
        public static ObservableCollection<Termin> Termini { get; set; }
        public static ObservableCollection<Obavestenja> ObavestenjaPacijent { get; set; }
        public Thread thread;
        public PrikaziTermin(int idPrijavljeniPacijent)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljeniPacijent;
            prijavljeniPacijent = PacijentiServis.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            Termini = new ObservableCollection<Termin>();
            ObavestenjaPacijent = new ObservableCollection<Obavestenja>();

            pacijentProzor = true;
            thread = new Thread(izvrsiNit);
            thread.Start();
            Termini = TerminServis.DodajTerminePacijenta(idPacijent);
            ObavestenjaPacijent = ObavestenjaServis.DodajObavestenja(idPacijent);
            this.SvetlaTema.IsEnabled = false;
            AktivnaTemaPagea(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
        }

       
        public static void AktivnaTemaPagea(StackPanel PanelZaglavlja, MenuItem SvetlaTema, MenuItem TamnaTema)
        {
            if (PanelZaglavlja.Background.ToString().Equals("#FF112D4E"))
            {
                //AktivnaTema.Header = "Svetla";
                TamnaTema.IsEnabled = false;

            }
            else if (PanelZaglavlja.Background.ToString().Equals("#e8f1f5"))
            {
                //AktivnaTema.Header = "Tamna";
                SvetlaTema.IsEnabled = false;
            }
        }

        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.anketa_Click(this, idPacijent);
        }

        public void izvrsiNit()
        {
            while (pacijentProzor == true)
            {
                Thread.Sleep(1000);  //30000
                ObavestenjaServis.ProveriSvaObavestenja(idPacijent, ObavestenjaPacijent);
            }
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 8) // **
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void obavestenja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        // obavestenja servis
        private void UvidObavestenje_Click(object sender, RoutedEventArgs e)
        {
            Obavestenja obavestenje = (Obavestenja)obavestenja.SelectedItem;
            if(obavestenje != null)
            {
                MessageBox.Show("Bice implementirano");
            }
        }

        private void ObrisiObavestenje_Click(object sender, RoutedEventArgs e)
        {
            ObavestenjaServis.ObrisiSelektovanoObavestenje((Obavestenja)obavestenja.SelectedItem, ObavestenjaPacijent);
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

        private void Podsetnik_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.Podsetnik_Click(this, idPacijent);
        }

        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            /* var app = (App)Application.Current;
             MenuItem mi = (MenuItem)sender;
             if (mi.Header.Equals("Svetla") || mi.Header.Equals("Light"))
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
             }*/
            PacijentPagesServis.PromeniTemu(SvetlaTema, tamnaTema);
        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            /* var app = (App)Application.Current;
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
