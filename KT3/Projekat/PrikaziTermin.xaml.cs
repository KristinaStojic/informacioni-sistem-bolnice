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
        ObavestenjaServis servis = new ObavestenjaServis();
        PacijentiServis pacijentiServis = new PacijentiServis();

        public PrikaziTermin(int idPrijavljeniPacijent)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljeniPacijent;
            prijavljeniPacijent = pacijentiServis.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            Termini = new ObservableCollection<Termin>();
            ObavestenjaPacijent = new ObservableCollection<Obavestenja>();

            pacijentProzor = true;
            thread = new Thread(izvrsiNit);
            thread.Start();

            //Termini = TerminServis.DodajTerminePacijenta(idPacijent);
            ObavestenjaPacijent = servis.DodajObavestenja(idPacijent);

            this.SvetlaTema.IsEnabled = false;
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
        }

      

        public void izvrsiNit()
        {
            while (pacijentProzor == true)
            {
                Thread.Sleep(1000);  //30000
                servis.ProveriSvaObavestenja(idPacijent, ObavestenjaPacijent);
            }
        }

        private void obavestenja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void UvidObavestenje_Click(object sender, RoutedEventArgs e)
        {
            Obavestenja obavestenje = (Obavestenja)obavestenja.SelectedItem;
            if (obavestenja != null)
            {
                if (Jezik.Header.Equals("_en-US"))
                {
                    string informacijeObavestenjeSrp = "Sadržaj: " + obavestenje.SadrzajObavestenja + "\nDatum: " + obavestenje.Datum;
                    MessageBox.Show(informacijeObavestenjeSrp, obavestenje.TipObavestenja, MessageBoxButton.OKCancel, MessageBoxImage.Information);
                }
                else
                {
                    string informacijeObavestnjeEng = "Content: " + obavestenje.SadrzajObavestenja + "\nDate: " + obavestenje.Datum;
                    MessageBox.Show(informacijeObavestnjeEng, obavestenje.TipObavestenja, MessageBoxButton.OKCancel, MessageBoxImage.Information);
                }
            }
            else
            {
                if (Jezik.Header.Equals("_en-US"))
                { 
                    MessageBox.Show("Morate selektovati jedno obavestenje.", obavestenje.TipObavestenja, MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    MessageBox.Show("You must select one notification.", obavestenje.TipObavestenja, MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    return;
                }
            }
        }

        private void ObrisiObavestenje_Click(object sender, RoutedEventArgs e)
        {
            servis.ObrisiSelektovanoObavestenje((Obavestenja)obavestenja.SelectedItem, ObavestenjaPacijent, Jezik);
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
    }
}
