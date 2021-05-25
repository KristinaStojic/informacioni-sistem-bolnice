using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Projekat.ViewModel
{
    public class AnketeViewModel : BindableBase
    {
        private int idPacijent;
        private NavigationService navService;
        private ObservableCollection<Anketa> ankete;

        #region Pages
        public Page PrikaziAnketePage { get; set; }
        public Page PrikaziAnketeZaLekaraPage { get; set; }
        public Page PrikaziAnketuZaKlinikuPage { get; set; }

        // isto za svaki page
        public Page PrijavaPage { get; set; }
        public Page ZakaziTerminPage { get; set; }
        public Page PocetnaStranicaPage { get; set; }
        public Page UvidUZakazaneTerminePage { get; set; }
        public Page KartonPage { get; set; }
        public Page ProfilPage { get; set; }
        public Page PopuniAnketuPage { get; set; }
        #endregion

        #region Komande
        public MyICommand OdjavaKomanda { get; set; }
        public MyICommand PromeniTemuKomanda { get; set; }
        public MyICommand PromeniJezikKomanda { get; set; }
        public MyICommand ZakaziTerminKomanada { get; set; }
        public MyICommand PocetnaStranicaKomanda { get; set; }
        public MyICommand UvidUZakazaneTermineKomanda { get; set; }
        public MyICommand KartonKomanda { get; set; }
        public MyICommand ProfilKomanda { get; set; }
        public MyICommand PopuniAnketuKomanda { get; set; }
        public MyICommand PromeniJezikKomandaSR { get; set; }
        public MyICommand PromeniTemuKomandaTamna { get; set; }

        #endregion
        public ObservableCollection<Anketa> Ankete { get { return ankete; } set { ankete = value; OnPropertyChanged("Ankete"); } }

        public MyICommand PrikaziAnketuZaLekara { get; set; }
        public MyICommand PrikaziAnketuZaKlinku { get; set; }
        public MyICommand PrikaziAnkete { get; set; }
        public MyICommand OtvoriAnketuKomanda { get; set; }

        private StackPanel zaglavlje ;
        public StackPanel Zaglavlje { get { return zaglavlje; } set { zaglavlje = value; OnPropertyChanged("Zaglavlje"); } }
        private MenuItem tamnaTema;
        public MenuItem Tamna { get { return tamnaTema; } set { tamnaTema = value; OnPropertyChanged("Tamna"); } }
        private MenuItem svetlaTema;
        public MenuItem Svetla { get { return svetlaTema; } set { svetlaTema = value; OnPropertyChanged("Svetla"); } }

        private MenuItem aktivniJezik;
        public MenuItem AktivniJezik { get { return aktivniJezik; } set { aktivniJezik = value; OnPropertyChanged("AktivniJezik"); } }

        // *
        private Anketa izabranaAnketa;
        public Anketa IzabranaAnketa { get { return izabranaAnketa; } set { izabranaAnketa = value; OnPropertyChanged("IzabranaAnketa"); } }

        public AnketeViewModel(NavigationService navigation, int idPrijavljenogPacijenta)
        {
            OtvoriAnketuKomanda = new MyICommand(OtvoriAnketu);
            this.navService = navigation;
            this.idPacijent = idPrijavljenogPacijenta;

            ProfilKomanda = new MyICommand(Profil);
            PopuniAnketuKomanda = new MyICommand(PopuniAnketu);
            PromeniJezikKomanda = new MyICommand(PromeniJezik);
            PromeniJezikKomandaSR = new MyICommand(PromeniJezikSR);
            PromeniTemuKomanda = new MyICommand(PromeniTemu);
            PromeniTemuKomandaTamna = new MyICommand(PromeniTemuTamna);
            OdjavaKomanda = new MyICommand(Odjava);
            PocetnaStranicaKomanda = new MyICommand(OtvoriPocetnu);
            ZakaziTerminKomanada = new MyICommand(ZakaziTerminClick);
            UvidUZakazaneTermineKomanda = new MyICommand(UvidUZakazaneTermine);
            KartonKomanda = new MyICommand(KartonClick);

            
            PrikaziAnketePacijenta();
            
        }

        private void KartonClick()
        {
            KartonPage = new ZdravstveniKartonPacijent(idPacijent);
            this.navService.Navigate(KartonPage);
        }

        private void UvidUZakazaneTermine()
        {
            UvidUZakazaneTerminePage = new ZakazaniTerminiPacijent(idPacijent);
            this.navService.Navigate(UvidUZakazaneTerminePage);
        }

        private void ZakaziTerminClick()
        {
            if (MalicioznoPonasanjeServis.DetektujMalicioznoPonasanje(idPacijent))
            {
                MessageBox.Show("Nije Vam omoguceno zakazivanje termina jer ste prekoracili dnevni limit modifikacije termina.", "Upozorenje", MessageBoxButton.OK);
                return;
            }
            ZakaziTerminPage = new ZakaziTermin(idPacijent);
            this.navService.Navigate(ZakaziTerminPage);
        }

        private void OtvoriPocetnu()
        {
            PocetnaStranicaPage = new PrikaziTermin(idPacijent);
            this.navService.Navigate(PocetnaStranicaPage);
        }

        private void Profil()
        {
            ProfilPage = new LicniPodaciPacijenta(idPacijent);
            this.navService.Navigate(ProfilPage);
        }

        private void PopuniAnketu()
        {
            PopuniAnketuPage = new PrikaziAnkete(idPacijent);
            this.navService.Navigate(PopuniAnketuPage);
        }

        private void PromeniJezik()
        {
            var app = (App)Application.Current;
            string eng = "en-US";
            app.ChangeLanguage(eng);
        }

        private void PromeniJezikSR()
        {
            var app = (App)Application.Current;
            string srb = "sr-LATN";
            app.ChangeLanguage(srb);
           
        }

        private void PromeniTemu()
        {
            var app = (App)Application.Current;
            app.ChangeTheme(new Uri("Teme/Svetla.xaml", UriKind.Relative));
        }
            
        private void PromeniTemuTamna()
        {
            var app = (App)Application.Current;
            app.ChangeTheme(new Uri("Teme/Tamna.xaml", UriKind.Relative));
        }

        private void Odjava()
        {
            PrijavaPage = new PrijavaPacijent();
            this.navService.Navigate(PrijavaPage);
        }

        private void OtvoriAnketu()
        {
            if (izabranaAnketa == null)
            {
                return;
            }
            if (!izabranaAnketa.PopunjenaAnketa)
            {
                Console.WriteLine("Izabrana anketa " + izabranaAnketa.NazivAnkete);
                if (izabranaAnketa.VrstaAnkete.Equals(VrstaAnkete.ZaKliniku))
                {
                    PrikaziAnketuZaKlinikuPage = new PrikaziAnketuZaKliniku(idPacijent, izabranaAnketa.IdAnkete);
                    this.navService.Navigate(PrikaziAnketuZaKlinikuPage);
                }
                else
                {
                    PrikaziAnketeZaLekaraPage = new PrikaziAntekuZaLekare(idPacijent, izabranaAnketa.IdAnkete);
                    this.navService.Navigate(PrikaziAnketeZaLekaraPage);
                }
            }
        }


        private void PrikaziAnketePacijenta()
        {
            Ankete = new ObservableCollection<Anketa>();
            foreach (Anketa anketa in AnketaServis.NadjiSveAnkete())
            {
                if (anketa.IdPacijent == idPacijent)
                    Ankete.Add(anketa);
            }

        }
    }
}
