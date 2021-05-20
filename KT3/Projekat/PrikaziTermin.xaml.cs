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
            this.Jezik.Header = "en-US";
            var app = (App)Application.Current;
            app.ChangeLanguage("sr-LATN");

        }


        // TODO: ispraviti na svim Page-vima
        public static void AktivnaTema(StackPanel PanelZaglavlja, MenuItem AktivnaTema)
        {
            if (PanelZaglavlja.Background.ToString().Equals("#FF112D4E"))
            {
                AktivnaTema.Header = "Svetla";
                
            }
            else if(PanelZaglavlja.Background.ToString().Equals("#e8f1f5"))
            {
                AktivnaTema.Header = "Tamna";
            }
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
            Page prikaziAnkete = new PrikaziAnkete(idPacijent);
            this.NavigationService.Navigate(prikaziAnkete);
        }

        public void izvrsiNit()
        {
            while (pacijentProzor == true)
            {
                Thread.Sleep(1000);  //30000
                ProveriSvaObavestenja();
            }
        }

        // obavestenja servis
        private static void ProveriSvaObavestenja()
        {
            App.Current.Dispatcher.Invoke((Action)delegate
             {
                 // TODO: CLEAN CODE i ispraviti
                 foreach (Obavestenja obavestenje in ObavestenjaServis.PronadjiObavestenjaPoIdPacijenta(idPacijent))
                 {
                     DateTime datumObavestenja = DateTime.Parse(obavestenje.Datum);
                     if (datumObavestenja == DateTime.Now.Date)
                     {
                        bool postojiObavestenjeZaRecept = ProveriObjavljenaObavestenjaZaRecepte(obavestenje);
                        string trenutnoVreme = DateTime.Now.TimeOfDay.ToString().Substring(0, 5); // HH:mm
                        string vremeZaTerapiju = datumObavestenja.ToString("MM/dd/yyyy HH:mm").Substring(0, 5);
                        if (vremeZaTerapiju.Equals(trenutnoVreme))
                        {
                            if (!postojiObavestenjeZaRecept)
                            {
                                Obavestenja novoObavestenje = PronadjiSledeceObavestenje(datumObavestenja.ToString("MM/dd/yyyy HH:mm"));
                                if (novoObavestenje != null)
                                {
                                    ObavestenjaPacijent.Add(novoObavestenje);
                                    if (novoObavestenje.TipObavestenja.Equals("Terapija"))
                                    {
                                        string sadrzajObavestenja = novoObavestenje.SadrzajObavestenja;
                                        MessageBox.Show(sadrzajObavestenja, "Novo obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                            }
                        }
                     }
                 }
                 // obavestenja za podsetnike
                 ObavestenjaZaPodsetnike();
             });
        }

        // obavestenja servis
        // TODO
        private static void ObavestenjaZaPodsetnike()
        {
            foreach (Obavestenja obavestenje in ObavestenjaServis.PronadjiObavestenjaPoIdPacijenta(idPacijent))
            {
                if (obavestenje.TipObavestenja.Equals("Podsetnik"))
                {
                    DateTime datumPodsetnika = DateTime.Parse(obavestenje.Datum.Split(' ')[0]);
                    string trenutnoVreme = DateTime.Now.TimeOfDay.ToString().Substring(0, 5); // HH:mm
                    string vremeZaPodsetnik = obavestenje.Datum.Split(' ')[1];
                    if (trenutnoVreme.Equals(vremeZaPodsetnik))
                    {
                        if (ProveriObjavljenaObavestenjaZaPodsetnike(obavestenje))
                        {
                            return;
                        }
                        ObavestenjaPacijent.Add(obavestenje);
                        string nazivObavestenja = obavestenje.SadrzajObavestenja;
                        MessageBox.Show(nazivObavestenja, "Podsetnik", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
        // obavestenja servis
        private static bool ProveriObjavljenaObavestenjaZaPodsetnike(Obavestenja obavestenje)
        {
            foreach(Obavestenja o in ObavestenjaPacijent)
            {
                if(o.IdObavestenja == obavestenje.IdObavestenja)
                {
                    return true;
                }
            }
            return false;
        }
        // obavestenja servis
        private static Obavestenja PronadjiSledeceObavestenje(string datum)
        {
            foreach (Obavestenja o in ObavestenjaServis.NadjiSvaObavestenja())
            {
                if (o.ListaIdPacijenata.Contains(idPacijent))
                {
                    if ((o.TipObavestenja.Equals("Terapija") || o.TipObavestenja.Equals("Podsetnik")) && o.Datum.Equals(datum) && !ObavestenjaPacijent.Any(x => x.IdObavestenja == o.IdObavestenja))
                    {
                        return o;
                    }
                }
            }
            return null;
        }
        // obavestenja servis
        private static bool ProveriObjavljenaObavestenjaZaRecepte(Obavestenja obavestenje) 
        {
            foreach(Obavestenja o in ObavestenjaPacijent)
            {
                /*if (o.TipObavestenja.Equals("Terapija") )
                {
                    string sadrzaj = "Uzmite terapiju: " + nazivLeka;
                    string datum = datumUzimanjaTerapije.ToString("dd/MM/yyyy HH:mm");
                    if (ObavestenjaPacijent.Any(x => x.SadrzajObavestenja == sadrzaj) && ObavestenjaPacijent.Any(y => y.Datum == datum))
                    {
                        return true;
                    }
                }*/
                if (o.IdObavestenja == obavestenje.IdObavestenja)
                {
                    return true;
                }
            }
            return false;
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

        // obavestenja servis
        private void ObrisiObavestenje_Click(object sender, RoutedEventArgs e)
        {
            // TODO: ako je obavestenje za sve, ne moze pacijent da obrise obavestenje iz menadzera(obrisace svima)
            Obavestenja obavestenje = (Obavestenja)obavestenja.SelectedItem;
            if (obavestenje != null && obavestenje.TipObavestenja.Equals("Terapija"))
            {
                ObavestenjaServis.ObrisiObavestenjePacijent(obavestenje);
                ObavestenjaPacijent.Remove(obavestenje);
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
            if (MalicioznoPonasanjeServis.DetektujMalicioznoPonasanje(idPacijent))
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

        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            Page podaci = new LicniPodaciPacijenta(idPacijent);
            this.NavigationService.Navigate(podaci);
        }

        private void Podsetnik_Click(object sender, RoutedEventArgs e)
        {
            Page dodajPodsetnik = new PodsetnikPacijent(idPacijent);
            this.NavigationService.Navigate(dodajPodsetnik);
        }

        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
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
            }
        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
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
            }
            
        }
    }
}
