using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Projekat.ViewModel
{
    public class UpravnikViewModel : BindableBase
    {
        #region Promjenljive
        public MyICommand OdjavaKomanda { get; set; }
        public MyICommand PrikazObavjestenja { get; set; }
        public MyICommand ProstorijeProzor { get; set; }
        public MyICommand NapustiRegistraciju { get; set; }
        public MyICommand ZahtjeviProzor { get; set; }
        public MyICommand KomunikacijProzor { get; set; }
        public MyICommand ZatvoriObavjestenje { get; set; }
        public MyICommand PrijaviSeKomanda { get; set; }
        public MyICommand Registracija { get; set; }
        public MyICommand Upravnik { get; set; }
        public MyICommand UgasiAplikaciju { get; set; }
        public MyICommand RegistracijaKlik { get; set; }
        public MyICommand Logovanje { get; set; }
        public MyICommand IzvjestajProzor { get; set; }
        public Window ObavjestenjeProzor { get; set; }
        public static Window PrijavaProzor { get; set; }
        public static Window UpravnikProzor { get; set; }
        public static Window UpravnikRegistracijaProzor { get; set; }
        public static Window UspjesnaRegistracijaProzor { get; set; }

        private string korisnickoIme;

        private string lozinka;

        private string ime;

        private string prezime;

        private string korisnickoImeRegistracija;

        private string lozinkaRegistracija;
        public string Ime { get { return ime; } set { ime = value; OnPropertyChanged("Ime"); Registracija.RaiseCanExecuteChanged(); } }
        public string Prezime { get { return prezime; } set { prezime = value; OnPropertyChanged("Prezime"); Registracija.RaiseCanExecuteChanged(); } }
        public string KorisnickoImeRegistracija { get { return korisnickoImeRegistracija; } set { korisnickoImeRegistracija = value; OnPropertyChanged("KorisnickoImeRegistracija"); Registracija.RaiseCanExecuteChanged(); } }
        public string LozinkaRegistracija { get { return lozinkaRegistracija; } set { lozinkaRegistracija = value; OnPropertyChanged("LozinkaRegistracija"); Registracija.RaiseCanExecuteChanged(); } }
        public string KorisnickoIme { get { return korisnickoIme; } set { korisnickoIme = value; OnPropertyChanged("KorisnickoIme"); } }
        public string Lozinka { get { return lozinka; } set { lozinka = value; OnPropertyChanged("Lozinka"); } }

        private string obavjestenje;
        public string Obavjestenje { get { return obavjestenje; } set { obavjestenje = value; OnPropertyChanged("Obavjestenje"); } }
        
        private Obavestenja izabranoObavjestenje;
        public Obavestenja IzabranoObavjestenje { get { return izabranoObavjestenje; } set { izabranoObavjestenje = value; OnPropertyChanged("IzabranoObavjestenj"); } }
        
        private ObservableCollection<Obavestenja> obavestenja;
        public ObservableCollection<Obavestenja> Obavestenja { get { return obavestenja; } set { obavestenja = value; OnPropertyChanged("Obavestenja"); } }
        #endregion

        #region Konstruktor
        public UpravnikViewModel()
        {
            korisnickoImeRegistracija = "";
            lozinkaRegistracija = "";
            OdjavaKomanda = new MyICommand(ZatvoriAplikaciju);
            ProstorijeProzor = new MyICommand(OtvoriProstorije);
            ZahtjeviProzor = new MyICommand(OtvoriZahtjeve);
            KomunikacijProzor = new MyICommand(OtvoriKomunikaciju);
            PrikazObavjestenja = new MyICommand(ObavjestenjeDetaljnije);
            ZatvoriObavjestenje = new MyICommand(ZatvoriObavjestenja);
            PrijaviSeKomanda = new MyICommand(Prijava);
            Registracija = new MyICommand(RegistrujSe, ValidnaRegistracija);
            RegistracijaKlik = new MyICommand(OtvoriRegistraciju);
            Logovanje = new MyICommand(UlogujSe);
            Upravnik = new MyICommand(OtvoriUpravnika);
            IzvjestajProzor = new MyICommand(PrikaziIzvjestaj);
            UgasiAplikaciju = new MyICommand(Ugasi);
            NapustiRegistraciju = new MyICommand(OtvoriPrijavu);
            dodajObavjestenja();
        }

        private void dodajObavjestenja()
        {
            ObavestenjaServis servis = new ObavestenjaServis(); 
            Obavestenja = new ObservableCollection<Obavestenja>();
            foreach (Obavestenja obavjestenja in servis.NadjiSvaObavestenja())
            {
                if (obavjestenja.Oznaka.Equals("svi") || obavjestenja.Oznaka.Equals("upravnici"))
                {
                    Obavestenja.Add(obavjestenja);
                }
            }
        }
        #endregion

        #region UpravnikViewModel
        private void ZatvoriObavjestenja()
        {
            ObavjestenjeProzor.Close();
        }
        private void ObavjestenjeDetaljnije()
        {
            if(izabranoObavjestenje != null)
            {
                ObavjestenjeProzor = new PrikazObavjestenja();
                ObavjestenjeProzor.Show();
                Obavjestenje = izabranoObavjestenje.SadrzajObavestenja;
                ObavjestenjeProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati obavjestenje!");
            }
        }

        private void Ugasi()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OtvoriKomunikaciju()
        {
            KomunikacijaViewModel.KomunikacijaProzor = new Komunikacija();
            KomunikacijaViewModel.KomunikacijaProzor.Show();
            KomunikacijaViewModel.KomunikacijaProzor.DataContext = new KomunikacijaViewModel();
            UpravnikProzor.Close();
        }
        private void OtvoriZahtjeve()
        {
            ZahtjeviViewModel.ZahtjeviProzor = new Zahtjevi();
            ZahtjeviViewModel.ZahtjeviProzor.Show();
            ZahtjeviViewModel.ZahtjeviProzor.DataContext = new ZahtjeviViewModel();
            UpravnikProzor.Close();
        }
        private void OtvoriProstorije()
        {
            SaleViewModel.SaleProzor = new PrikaziSalu();
            SaleViewModel.SaleProzor.Show();
            SaleViewModel.SaleProzor.DataContext = new SaleViewModel();
            UpravnikProzor.Close();
        }
        private void ZatvoriAplikaciju()
        {
            PrijavaProzor.Show();
            KorisnickoIme = "";
            Lozinka = "";
            UpravnikProzor.Close();
        }
        #endregion

        #region PrijavaUpravnika
        private void Prijava()
        {
            foreach(UpravnikModel upravnik in UpravnikServis.NadjiSveUpravnike())
            {
                if(upravnik.KorisnickoIme.Equals(KorisnickoIme) && upravnik.Lozinka.Equals(Lozinka))
                {
                    UpravnikProzor = new Upravnik();
                    UpravnikProzor.Show();
                    UpravnikProzor.DataContext = this;
                    PrijavaProzor.Hide();
                    return;
                }
            }
            Console.WriteLine("DSadasdasdas");
            MessageBox.Show("Neispravno korisnicko ime i / ili lozinka");
            KorisnickoIme = "";
            Lozinka = "";
        }

        #endregion

        #region RegistracijaUpravnika
        private void RegistrujSe()
        {
            UpravnikModel upravnik = new UpravnikModel(korisnickoImeRegistracija, lozinkaRegistracija);
            UpravnikServis.DodajUpravnika(upravnik);
            UpravnikRegistracijaProzor.Close();
            UspjesnaRegistracijaProzor = new UspjesnaRegistracijaUpravnik();
            UspjesnaRegistracijaProzor.Show();
            UspjesnaRegistracijaProzor.DataContext = this;
        }
        private void UlogujSe()
        {
            PrijavaProzor = new UpravnikPrijava();
            PrijavaProzor.Show();
            korisnickoIme = "";
            lozinka = "";
            PrijavaProzor.DataContext = this;
            UspjesnaRegistracijaProzor.Close();
        }
        private void OtvoriUpravnika()
        {
            UpravnikProzor = new Upravnik();
            UpravnikProzor.Show();
            UpravnikProzor.DataContext = this;
            UspjesnaRegistracijaProzor.Close();
        }
        private bool ValidnaRegistracija()
        {
            if (ime != null && prezime != null && korisnickoImeRegistracija != null && lozinkaRegistracija != null)
            {
                if (!ime.Trim().Equals("") && !prezime.Trim().Equals("") && !korisnickoImeRegistracija.Trim().Equals("") && !lozinkaRegistracija.Trim().Equals(""))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private void OtvoriPrijavu()
        {
            PrijavaProzor = new UpravnikPrijava();
            PrijavaProzor.Show();
            PrijavaProzor.DataContext = this;
            UpravnikRegistracijaProzor.Close();
        }
        private void OtvoriRegistraciju()
        {
            UpravnikRegistracijaProzor = new UpravnikRegistracija();
            UpravnikRegistracijaProzor.Show();
            korisnickoImeRegistracija = "";
            lozinkaRegistracija = "";
            ime = "";
            prezime = "";
            UpravnikRegistracijaProzor.DataContext = this;
            PrijavaProzor.Hide();
        }

        #endregion

        #region IzvjestajViewModel
        private void PrikaziIzvjestaj()
        {
            IzvjestajViewModel.IzvjestajProzor = new Izvjestaj();
            IzvjestajViewModel.IzvjestajProzor.Show();
            IzvjestajViewModel.IzvjestajProzor.DataContext = new IzvjestajViewModel();
            UpravnikProzor.Close();
        }
        #endregion
    }
}
