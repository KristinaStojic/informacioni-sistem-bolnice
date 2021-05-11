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
            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            Termini = new ObservableCollection<Termin>();
            ObavestenjaPacijent = new ObservableCollection<Obavestenja>();

            pacijentProzor = true;
            thread = new Thread(izvrsiNit);
            thread.Start();
            DodajTerminePacijenta();
            DodajObavestenja();
            AktivnaTema(this.zaglavlje, this.svetlaTema);
        }

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

        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            Page prikaziAnkete = new PrikaziAnkete(idPacijent);
            this.NavigationService.Navigate(prikaziAnkete);
        }

        private static void DodajTerminePacijenta()
        {
            foreach (Termin t in TerminMenadzer.termini)
            {
                if (t.Pacijent.IdPacijenta == idPacijent)
                {
                    Termini.Add(t);
                }
            }
        }

        private static void DodajObavestenja()
        {
            foreach (Obavestenja o in ObavestenjaMenadzer.obavestenja)
            {
                if (o.ListaIdPacijenata.Contains(idPacijent))
                {
                    if (o.TipObavestenja.Equals("Terapija"))
                    {
                        DodajStaraObavestenjaZaTerapije(o);
                    }
                }
                if (!o.TipObavestenja.Equals("Terapija"))
                {
                    if (o.ListaIdPacijenata.Contains(prijavljeniPacijent.IdPacijenta) || o.Oznaka.Equals("pacijenti") || o.Oznaka.Equals("svi"))
                    {
                        ObavestenjaPacijent.Add(o);
                    }
                }
            }
        }

        private static void DodajStaraObavestenjaZaTerapije(Obavestenja o)
        {
            DateTime dt = DateTime.Parse(o.Datum);
            if (dt.Date <= DateTime.Now.Date)
            {
                if (dt.TimeOfDay <= DateTime.Now.TimeOfDay)
                {
                    ObavestenjaPacijent.Add(o);
                }
            }
        }

        public void izvrsiNit()
        {
            while (pacijentProzor == true)
            {
                Thread.Sleep(1000);  //30000
                ProveriRecepte();
            }
        }

        private static void ProveriRecepte()
        {
             App.Current.Dispatcher.Invoke((Action)delegate
             {
                 foreach (LekarskiRecept recept in prijavljeniPacijent.Karton.LekarskiRecepti)
                 {
                     foreach (DateTime datum in recept.UzimanjeTerapije)
                     {
                         if (datum.Date == DateTime.Now.Date)
                         {
                             bool postojiObavestenje = ProveriObjavljenaObavestenja(recept.NazivLeka, datum); 
                             string trenutnoVreme = DateTime.Now.TimeOfDay.ToString().Substring(0, 5); // HH:mm
                             string vremeZaTerapiju = datum.TimeOfDay.ToString().Substring(0, 5);
                             if (vremeZaTerapiju.Equals(trenutnoVreme))
                             {
                                 if (!postojiObavestenje)
                                 {
                                    //DateTime datum = DateTime.Now.Date;
                                    List<int> lista = new List<int>();
                                    lista.Add(prijavljeniPacijent.IdPacijenta);
                                    int id = ObavestenjaMenadzer.GenerisanjeIdObavestenja();
                                    Obavestenja obavestenje = PronadjiSledeceObavestenje(datum.ToString("MM/dd/yyyy HH:mm"));
                                    if (obavestenje != null)
                                    {
                                        ObavestenjaPacijent.Add(obavestenje);
                                        Console.Beep();
                                    }
                                 }
                            }
                        }
                    }
                }
            });
        }

        private static Obavestenja PronadjiSledeceObavestenje(string datum)
        {
            foreach (Obavestenja o in ObavestenjaMenadzer.obavestenja)
            {
                if (o.ListaIdPacijenata.Contains(idPacijent))
                {
                    if (o.TipObavestenja.Equals("Terapija") && o.Datum.Equals(datum) && !ObavestenjaPacijent.Any(x => x.IdObavestenja == o.IdObavestenja))
                    {
                        return o;
                    }
                }
            }
            return null;
        }

        private static bool ProveriObjavljenaObavestenja(string nazivLeka, DateTime datumUzimanjaTerapije) 
        {
            foreach(Obavestenja o in ObavestenjaPacijent)
            {
                if (o.TipObavestenja.Equals("Terapija"))
                {
                    string sadrzaj = "Uzmite terapiju: " + nazivLeka;
                    string datum = datumUzimanjaTerapije.ToString("dd/MM/yyyy HH:mm");
                    if (ObavestenjaPacijent.Any(x => x.SadrzajObavestenja == sadrzaj) && ObavestenjaPacijent.Any(y => y.Datum == datum))
                    {
                        return true;
                    }
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

        private void dataGridTermini_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void obavestenja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
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
            // TODO: ako je obavestenje za sve, ne moze pacijent da obrise obavestenje iz menadzera(obrisace svima)
            Obavestenja obavestenje = (Obavestenja)obavestenja.SelectedItem;
            if (obavestenje != null && obavestenje.TipObavestenja.Equals("Terapija"))
            {
                ObavestenjaMenadzer.ObrisiObavestenjePacijent(obavestenje);
                ObavestenjaPacijent.Remove(obavestenje);
            }
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

        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            Page podaci = new LicniPodaciPacijenta(idPacijent);
            this.NavigationService.Navigate(podaci);
        }
    }
}
