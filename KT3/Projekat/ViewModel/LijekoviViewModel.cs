using Projekat.Model;
using Projekat.Pomoc;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Projekat.ViewModel
{

    public class LijekoviViewModel : BindableBase  
    {
        #region Promjenljive
        public Window PomocProzor { get; set; }
        public static Window DodavanjeLijekaProzor { get; set; }
        public static Window DodavanjeSastojkaProzor { get; set; }
        public static Window DodavanjeNovogSastojkaProzor { get; set; }
        public static Window BrisanjeLijekaProzor { get; set; }
        public static Window ZahtjeviProzor { get; set; }
        public static Window IzmjenaLijekaProzor { get; set; }
        public static Window ZamjenskiLijekoviProzor { get; set; }
        public static Window IzmjeniZamjenskiProzor { get; set; }
        public static Window SastojciProzor { get; set; }
        public static Window DodajSastojakProzor { get; set; }
        public static Window IzmjeniSastojakProzor { get; set; }
        public static Window OdbijeniLijekoviProzor { get; set; }
        public static Window ObrazlozenjeProzor { get; set; }
        public static Window IzmjeniOdbijeniProzor { get; set; }
        public static Window IzmjeniSastojakOdbijenogProzor { get; set; }
        public static Window IzmjenaSastojkaOdbijenogProzor { get; set; }
        public static Window BrisanjeOdbijenogLijekaProzor { get; set; }
        public static Window PonovnoSlanjeZahtjevaProzor { get; set; }
        public static Window DodavanjeZamjenskogLijekaProzor { get; set; }
        public static Window LijekoviProzor { get; set; }
        public MyICommand OtvoriOAplikaciji { get; set; }
        public MyICommand OtvoriPomoc { get; set; }
        public MyICommand OtvoriSale { get; set; }
        public MyICommand OtvoriKomunikaciju { get; set; }
        public MyICommand OtvoriIzvjestaj { get; set; }
        public MyICommand ObrisiLijekKomanda { get; set; }
        public MyICommand ObrisiLijekProzor { get; set; }
        public MyICommand ZatvoriBrisanjeLijekaKomanda { get; set; }
        
        public MyICommand DodajLijekKomanda { get; set; }
        public MyICommand DodajLijekProzor { get; set; }
        public MyICommand DodajSastojakLijekuProzorKomanda { get; set; }
        public MyICommand ZatvoriDodavanjeLijekaKomanda { get; set; }
        public MyICommand DodajSastojakKomanda { get; set; }
        public MyICommand ZatvoriDodavanjeSastojakaKomanda { get; set; }
        public MyICommand DodajNoviSastojak { get; set; }
        public MyICommand DodajSastojakZatvori { get; set; }

        private static Lek uneseniLijek;
        private string pretragaLijekova;
        private static string nazivLijeka;
        private static string sifraLijeka;
        private static string nazivSastojka;
        private static string kolicinaSastojka;
        public string NazivLijeka{get { return nazivLijeka; }set { nazivLijeka = value; DodajLijekKomanda.RaiseCanExecuteChanged(); DodajSastojakLijekuProzorKomanda.RaiseCanExecuteChanged(); }}
        public string SifraLijeka{get { return sifraLijeka; }set { sifraLijeka = value; DodajLijekKomanda.RaiseCanExecuteChanged(); DodajSastojakLijekuProzorKomanda.RaiseCanExecuteChanged(); }}
        public string NazivSastojka{get { return nazivSastojka; }set { nazivSastojka = value; DodajNoviSastojak.RaiseCanExecuteChanged(); }}
        public string KolicinaSastojka{get { return kolicinaSastojka; }set { kolicinaSastojka = value; DodajNoviSastojak.RaiseCanExecuteChanged(); }}
        public string PretragaLijekova { get { return pretragaLijekova; } set { pretragaLijekova = value; OnPropertyChanged("PretragaLijekova"); NadjiLijekove(); } }

        private static Lek izabraniLijek;
        public Lek IzabraniLijek {get { return izabraniLijek; }set{izabraniLijek = value;ObrisiLijekKomanda.RaiseCanExecuteChanged();}}

        private ObservableCollection<Lek> lekovi;
        public ObservableCollection<Lek> Lekovi { get { return lekovi; } set { lekovi = value; OnPropertyChanged("Lekovi"); } }

        private ObservableCollection<Lek> odbijeniLekovi;
        public ObservableCollection<Lek> OdbijeniLekovi { get { return odbijeniLekovi; } set { odbijeniLekovi = value; OnPropertyChanged("OdbijeniLekovi"); } }

        private ObservableCollection<Sastojak> sastojciLeka;
        public ObservableCollection<Sastojak> SastojciLeka { get { return sastojciLeka; } set { sastojciLeka = value; OnPropertyChanged("SastojciLeka"); } }

        private ObservableCollection<Sastojak> sastojciOdbijenog;
        public ObservableCollection<Sastojak> SastojciOdbijenog { get { return sastojciOdbijenog; } set { sastojciOdbijenog = value; OnPropertyChanged("SastojciOdbijenog"); } }

        private ObservableCollection<Lek> zamjenskiLekovi;

        private ObservableCollection<Lek> zamjenskiLijekovi;
        public ObservableCollection<Lek> ZamjenskiLekovi { get { return zamjenskiLekovi; } set { zamjenskiLekovi = value; OnPropertyChanged("ZamjenskiLekovi"); } }
        public ObservableCollection<Lek> ZamjenskiLijekovi { get { return zamjenskiLijekovi; } set { zamjenskiLijekovi = value; OnPropertyChanged("ZamjenskiLijekovi"); } }

        private ObservableCollection<Sastojak> sastojciLijeka;
        public ObservableCollection<Sastojak> SastojciLijeka { get { return sastojciLijeka; } set { sastojciLijeka = value; OnPropertyChanged("SastojciLijeka"); } }


        #endregion

        #region Konstruktor

        public LijekoviViewModel()
        {
            inicijalizujElemente(); 
            DodajLijekKomanda = new MyICommand(DodajLijek, ValidnaPoljaZaDodavanjeLijeka);
            DodajSastojakLijekuProzorKomanda = new MyICommand(DodajSastojakLijekuProzor, ValidnaPoljaZaDodavanjeLijeka);
            DodajLijekProzor = new MyICommand(DodajLijekProzorKomanda);
            ObrisiLijekProzor = new MyICommand(ProzorZaBrisanje);
            ObrisiLijekKomanda = new MyICommand(Obrisi);
            ZatvoriBrisanjeLijekaKomanda = new MyICommand(ZatvoriBrisanjeLijeka);
            ZatvoriDodavanjeLijekaKomanda = new MyICommand(ZatvoriDodavanjeLijeka);
            DodajSastojakKomanda = new MyICommand(DodajSastojak);
            ZatvoriDodavanjeSastojakaKomanda = new MyICommand(ZatvoriDodavanjeSastojaka);
            DodajNoviSastojak = new MyICommand(DodajNoviSastojakKomanda, ValidnaPoljaZaDodavanjeSastojka);
            DodajSastojakZatvori = new MyICommand(ZatvoriDodavanjeNovogSastojkaKomanda);
            ZahtjeviKomanda = new MyICommand(OtvoriZahtjeve);
            IzmjeniLijekKomanda = new MyICommand(IzmjeniLijekProzor);
            PotvrdiIzmjenuLijekaKomanda = new MyICommand(IzmjenaLijeka, ValidnaPoljaZaIzmjenuLijeka);
            ZatvoriIzmjenuLijekaKomanda = new MyICommand(ZatvoriIzmjenuLijeka);
            ZamjenskiLijekoviProzorKomanda = new MyICommand(OtvoriZamjenskeLijekove);
            ZatvoriZamjenskeKomanda = new MyICommand(ZatvoriZamjenske);
            ObrisiZamjenskiKomanda = new MyICommand(ObrisiZamjenski);
            IzmjeniZamjenskiLijekKomanda = new MyICommand(IzmjeniZamjenski);
            OdustaniOdIzmjeneZamjenskog = new MyICommand(ZatvoriIzmjenuZamjenskog);
            PotvrdiIzmjenuZamjenskog = new MyICommand(IzmjeniZamjenskiLijek, ValidnaPoljaZaIzmjenuZamjenskog);
            SastojciProzorKomanda = new MyICommand(OtvoriSastojkeLijeka);
            ZatvoriSastojkeKomanda = new MyICommand(ZatvoriSastojke);
            DodajSastojakProzorKomanda = new MyICommand(OtvoriDodavanjeSastojaka);
            ZatvoriDodavanjeSastojka = new MyICommand(ZatvoriDodavanjaSastojka);
            PotvrdiDodavanjeSastojka = new MyICommand(DodajSastojakLijeku, ValidnaPoljaZaUnosSastojka);
            ObrisiSastojak = new MyICommand(ObrisiSastojakLijeka);
            IzmjeniSastojakKomanda = new MyICommand(IzmjenaSastojakaProzor);
            OdustaniOdIzmjeneSastojka = new MyICommand(ZatvoriDodavanjaSastojaka);
            IzmjeniSastojak = new MyICommand(PotvrdiIzmjenuSastojka, ValidnaPoljaZaIzmjenuSastojka);
            OdbijeniLijekovi = new MyICommand(PrikaziOdbijeneLijekove);
            ZatvoriOdbijene = new MyICommand(ZatvoriOdbijeneProzor);
            ObrazlozenjeKomanda = new MyICommand(PrikaziObrazlozenje);
            ZatvoriObrazlozenje = new MyICommand(ZatvoriObrazlozenjeOdbijanja);
            IzmjeniOdbijeniLijek = new MyICommand(IzmjeniOdbijeni);
            OdustaniOdIzmjeneOdbijenog = new MyICommand(OdustaniOdIzmjeneOdbijenogLijeka);
            PotvrdiIzmjenuOdbijenog = new MyICommand(PotvrdiIzmjenuOdbijenogLijeka, ValidnaPoljaZaImjenuOdbijenog);
            IzmjeniSastojakOdbijenog = new MyICommand(OtvoriIzmjenuSastojkaOdbijenog);
            ZatvoriSastojkeOdbijenog = new MyICommand(ZatvoriSastojkeOdbijene);
            IzmjeniSastojkeOdbijenogProzor = new MyICommand(OtvoriIzmjenuSastojka);
            ZatvoriIzmjenuOdbijenogSastojka = new MyICommand(ZatvoriIzmjenuOdbijenog);
            IzmjeniSastojakOdbijenogLijeka = new MyICommand(PotvrdiIzmjenuSastojkaOdbijenog, ValidnaPoljaZaIzmjenuOdbijenog);
            BrisanjeOdbijenogLijeka = new MyICommand(PotvrdiBrisanjeOdbijenogZahtjeva);
            ObrisiOdbijeniLijek = new MyICommand(ObrisiOdbijeni);
            OdustaniOdBrisanjaOdbijenog = new MyICommand(ZatvoriBrisanjeOdbijenog);
            PonovnoSlanjeZahtjeva = new MyICommand(PonovoPosaljiZahtjev);
            OdustaniOdPonovnogSlanjaZahtjeva = new MyICommand(OdustaniOdSlanja);
            PonovoPosaljiLijek = new MyICommand(PosaljiZahtjev);
            DodajZamjenskiProzor = new MyICommand(OtvoriDodavanjeZamjenskog);
            NapustiDodavanjeZamjenskih = new MyICommand(NapustiZamjenske);
            DodajZamjenski = new MyICommand(DodavanjeZamjenskog);
            OtvoriSale = new MyICommand(PrikaziSale);
            OtvoriKomunikaciju = new MyICommand(PrikaziKomunikaciju);
            OtvoriOAplikaciji = new MyICommand(OtvoriOpis);
            OtvoriIzvjestaj = new MyICommand(PrikaziIzvjestaj);
            OtvoriPomoc = new MyICommand(Pomoc);
        }
        #endregion

        #region LijekoviViewModel
        private void Pomoc()
        {
            PomocProzor = new LijekoviPomoc();
            PomocProzor.Show();
            PomocProzor.DataContext = this;
        }

        private void PrikaziIzvjestaj()
        {
            IzvjestajViewModel.IzvjestajProzor = new Izvjestaj();
            IzvjestajViewModel.IzvjestajProzor.Show();
            IzvjestajViewModel.IzvjestajProzor.DataContext = new IzvjestajViewModel();
            LijekoviProzor.Close();
        }
        private void OtvoriOpis()
        {
            OAplikacijiViewModel.OAplikacijiProzor = new OAplikaciji();
            OAplikacijiViewModel.OAplikacijiProzor.Show();
            OAplikacijiViewModel.OAplikacijiProzor.DataContext = new OAplikacijiViewModel();
        }
        
        private void PrikaziSale()
        {
            SaleViewModel.SaleProzor = new PrikaziSalu();
            SaleViewModel.SaleProzor.Show();
            SaleViewModel.SaleProzor.DataContext = new SaleViewModel();
            LijekoviProzor.Close();
        }
        private void PrikaziKomunikaciju()
        {
            KomunikacijaViewModel.KomunikacijaProzor = new Komunikacija();
            KomunikacijaViewModel.KomunikacijaProzor.Show();
            KomunikacijaViewModel.KomunikacijaProzor.DataContext = new KomunikacijaViewModel();
            LijekoviProzor.Close();
        }
        private void NadjiLijekove()
        {
            Lekovi.Clear();
            foreach (Lek lijek in LekoviMenadzer.lijekovi)
            {
                if (lijek.nazivLeka.StartsWith(PretragaLijekova))
                {
                    Lekovi.Add(lijek);
                }
            }
        }

        private void inicijalizujElemente()
        {
            Lekovi = new ObservableCollection<Lek>();
            SastojciLijeka = new ObservableCollection<Sastojak>();
            ZamjenskiLekovi = new ObservableCollection<Lek>();
            OdbijeniLekovi = new ObservableCollection<Lek>();
            foreach (Lek lijek in LekoviServis.Lijekovi())
            {
                Lekovi.Add(lijek);
            }
        }
        #endregion

        #region BrisanjeLijekaViewModel

        private void Obrisi()
        {
            LekoviServis.obrisiLijek(izabraniLijek);
            Lekovi.Remove(izabraniLijek);
            BrisanjeLijekaProzor.Close();
        }

        private void ProzorZaBrisanje()
        {
            if (izabraniLijek == null)
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
            else
            {
                BrisanjeLijekaProzor = new BrisanjeLijeka();
                BrisanjeLijekaProzor.Show();
                BrisanjeLijekaProzor.DataContext = this;
            }
        }

        private void ZatvoriBrisanjeLijeka()
        {
            BrisanjeLijekaProzor.Close();
        }
        #endregion

        #region DodajLijekViewModel
        
        private bool ValidnaPoljaZaDodavanjeLijeka()
        {
            if (sifraLijeka != null && nazivLijeka != null)
            {
                if (sifraLijeka.Trim().Equals("") || nazivLijeka.Trim().Equals("") || postojiSifraLijeka())
                {
                    return false;
                }
                else if (!sifraLijeka.Trim().Equals("") && !nazivLijeka.Trim().Equals("") && !postojiSifraLijeka())
                {
                    return true;
                }
            }
            return false;
        }

        private bool postojiSifraLijeka()
        {
            foreach (Lek lijek in LekoviServis.Lijekovi())
            {
                if (lijek.sifraLeka == sifraLijeka)
                {
                    return true;
                }
            }
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (zahtjev.lek.sifraLeka.Equals(sifraLijeka))
                {
                    return true;
                }
            }
            return false;
        }

        private void ZatvoriDodavanjeLijeka()
        {
            nazivLijeka = "";
            sifraLijeka = "";
            DodavanjeLijekaProzor.Close();
        }

        private void DodajLijek()
        {
            uneseniLijek.nazivLeka = nazivLijeka;
            uneseniLijek.sifraLeka = sifraLijeka;
            uneseniLijek.idLeka = LekoviServis.GenerisanjeIdLijeka();
            LekoviServis.dodajZahtjev(uneseniLijek);
            nazivLijeka = "";
            sifraLijeka = "";
            DodavanjeLijekaProzor.Close();
        }

        private void DodajLijekProzorKomanda()
        {
            uneseniLijek = new Lek();
            DodavanjeLijekaProzor = new DodajLijek();
            DodavanjeLijekaProzor.Show();
            DodavanjeLijekaProzor.DataContext = this;
        }

        private void DodajSastojakLijekuProzor()
        {
            DodavanjeSastojkaProzor = new SastojciDodavanje();
            DodavanjeSastojkaProzor.Show();
            dodajSastojke();
            DodavanjeSastojkaProzor.DataContext = this;
            
        }

        private void dodajSastojke()
        {
            SastojciLijeka.Clear();
            foreach (Sastojak sastojak in uneseniLijek.sastojci)
            {
                SastojciLijeka.Add(sastojak);
            }
        }

        private void ZatvoriDodavanjeSastojaka()
        {
            DodavanjeSastojkaProzor.Close();
        }

        private void DodajSastojak()
        {
            kolicinaSastojka = "";
            nazivSastojka = "";
            DodavanjeNovogSastojkaProzor = new SastojciDodaj();
            DodavanjeNovogSastojkaProzor.Show();
            DodavanjeNovogSastojkaProzor.DataContext = this;
        }

        private void DodajNoviSastojakKomanda()
        {
            Sastojak sastojak = new Sastojak(nazivSastojka, double.Parse(kolicinaSastojka));
            SastojciLijeka.Add(sastojak);
            uneseniLijek.sastojci.Add(sastojak);
            DodavanjeNovogSastojkaProzor.Close();
        }

        private void ZatvoriDodavanjeNovogSastojkaKomanda()
        {
            DodavanjeNovogSastojkaProzor.Close();
        }

        private bool ValidnaPoljaZaDodavanjeSastojka()
        {
            if (kolicinaSastojka != null && nazivSastojka != null)
            {
                if (jeBroj(kolicinaSastojka))
                {
                    if (kolicinaSastojka.Trim().Equals("") || nazivSastojka.Trim().Equals(""))
                    {
                        return false;
                    }
                    else if (!kolicinaSastojka.Trim().Equals("") && !nazivSastojka.Trim().Equals(""))
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            return false;

        }

        public bool jeBroj(string tekst)
        {
            double test;
            return double.TryParse(tekst, out test);
        }

        #endregion

        #region ZahtjeviViewModel
        public MyICommand ZahtjeviKomanda { get; set; }

        private void OtvoriZahtjeve()
        {
            LijekoviProzor.Close();
            ZahtjeviViewModel.ZahtjeviProzor = new Zahtjevi();
            ZahtjeviViewModel.ZahtjeviProzor.Show();
            ZahtjeviViewModel.ZahtjeviProzor.DataContext = new ZahtjeviViewModel();
        }
        #endregion

        #region IzmjeniLIjekViewModel
        public MyICommand IzmjeniLijekKomanda { get; set; }
        public MyICommand PotvrdiIzmjenuLijekaKomanda { get; set; }
        public MyICommand ZatvoriIzmjenuLijekaKomanda { get; set; }
        private void IzmjeniLijekProzor()
        {
            if (izabraniLijek == null)
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
            else
            {
                IzmjenaLijekaProzor = new IzmjeniLijek();
                sifraNovogLijeka = izabraniLijek.sifraLeka;
                nazivNovogLijeka = izabraniLijek.nazivLeka;
                IzmjenaLijekaProzor.Show();
                IzmjenaLijekaProzor.DataContext = this;
            }
        }


        private static string nazivNovogLijeka;
        private static string sifraNovogLijeka;
        public string NazivNovogLijeka
        {
            get { return nazivNovogLijeka; }
            set { nazivNovogLijeka = value; PotvrdiIzmjenuLijekaKomanda.RaiseCanExecuteChanged(); }
        }

        public string SifraNovogLijeka
        {
            get { return sifraNovogLijeka; }
            set { sifraNovogLijeka = value; PotvrdiIzmjenuLijekaKomanda.RaiseCanExecuteChanged(); }
        }
        private bool ValidnaPoljaZaIzmjenuLijeka()
        {
            if(sifraNovogLijeka != null && nazivNovogLijeka != null){
                if (sifraNovogLijeka.Trim().Equals("") || nazivNovogLijeka.Trim().Equals("") || postojiSifraNovogLijeka())
                {
                    return false;
                }
                else if (!sifraNovogLijeka.Trim().Equals("") && !nazivNovogLijeka.Trim().Equals("") && !postojiSifraNovogLijeka())
                {
                    return true;
                }
            }
            return false;
        }

        private bool postojiSifraNovogLijeka()
        {
            foreach (Lek lijek in LekoviServis.Lijekovi())
            {
                if (lijek.sifraLeka.Equals(sifraNovogLijeka) && lijek.idLeka != izabraniLijek.idLeka)
                {
                    return true;
                }
            }
            return false;
        }

        private void ZatvoriIzmjenuLijeka()
        {
            IzmjenaLijekaProzor.Close();
        }

        private void IzmjenaLijeka()
        {
            Lek lijek = new Lek(izabraniLijek.idLeka, nazivNovogLijeka, sifraNovogLijeka);
            LekoviServis.izmjeniLijek(izabraniLijek, lijek);
            int idx = Lekovi.IndexOf(izabraniLijek);
            Lekovi.RemoveAt(idx);
            Lekovi.Insert(idx, lijek);
            if (ZamjenskiLekovi.Count != 0)
            {
                int idx1 = ZamjenskiLekovi.IndexOf(izabraniLijek);
                ZamjenskiLekovi.RemoveAt(idx1);
                ZamjenskiLekovi.Insert(idx1, lijek);
            }
            IzmjenaLijekaProzor.Close();
        }
        #endregion

        #region ZamjenskiLijekoviViewModel

        public MyICommand ZamjenskiLijekoviProzorKomanda { get; set; }
        public MyICommand ZatvoriZamjenskeKomanda { get; set; }
        public MyICommand ObrisiZamjenskiKomanda { get; set; }
        public MyICommand PretragaZamjenskihKomanda { get; set; }

        private static Lek izabraniZamjenskiLijek;
        public Lek IzabraniZamjenskiLijek
        {
            get { return izabraniZamjenskiLijek; }
            set { izabraniZamjenskiLijek = value; }
        }

        private string pretragaZamjenskih;
        public string PretragaZamjenskih
        {
            get { return pretragaZamjenskih; }
            set { pretragaZamjenskih = value; OnPropertyChanged("PretragaZamjenskih"); PretraziZamjenske(); }
        }

        private string tekstZamjenski;
        public string TekstZamjenski
        {
            get { return tekstZamjenski; }
            set { tekstZamjenski = value; OnPropertyChanged("TekstZamjenski"); }
        }
        private void OtvoriZamjenskeLijekove()
        {
            if (izabraniLijek != null)
            {
                ZamjenskiLijekoviProzor = new ZamjenskiLijekovi();
                ZamjenskiLijekoviProzor.Show();
                TekstZamjenski = "Zamjenski lijekovi za lijek: " + izabraniLijek.nazivLeka;
                dodajLijekove();
                ZamjenskiLijekoviProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }
        private void ZatvoriZamjenske()
        {
            ZamjenskiLijekoviProzor.Close();
        }
        private void dodajLijekove()
        {
            ZamjenskiLekovi = new ObservableCollection<Lek>();
            foreach (Lek lijek in LekoviServis.Lijekovi())
            {
                if (izabraniLijek.idLeka == lijek.idLeka)
                {
                    dodajZamjenskiLijek(lijek);
                }
            }
        }

        private void dodajZamjenskiLijek(Lek lijek)
        {
            if (lijek.zamenskiLekovi != null)
            {
                dodajZamjenski(lijek);
            }
        }

        private void dodajZamjenski(Lek lijek)
        {
            foreach (int zamjenskiLijek in lijek.zamenskiLekovi)
            {
                foreach (Lek zamjenski in LekoviServis.Lijekovi())
                {
                    if (zamjenski.idLeka == zamjenskiLijek)
                    {
                        ZamjenskiLekovi.Add(zamjenski);
                    }
                }
            }
        }

        private void ObrisiZamjenski()
        {
            if (izabraniZamjenskiLijek != null)
            {
                LekoviServis.obrisiZamjenski(izabraniLijek, izabraniZamjenskiLijek);
                ZamjenskiLekovi.Remove(IzabraniZamjenskiLijek);
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }

        private void PretraziZamjenske()
        {
            ZamjenskiLekovi.Clear();
            if (izabraniLijek.zamenskiLekovi != null)
            {
                foreach (int zamjenskiLijek in izabraniLijek.zamenskiLekovi)
                {
                    pretraziZamjenskeLijekove(zamjenskiLijek);
                }
            }
        }

        private void pretraziZamjenskeLijekove(int zamjenskiLijek)
        {
            foreach (Lek zamjenski in LekoviMenadzer.lijekovi)
            {
                if (zamjenski.idLeka == zamjenskiLijek && zamjenski.nazivLeka.StartsWith(pretragaZamjenskih))
                {
                    ZamjenskiLekovi.Add(zamjenski);
                }
            }
        }

        #endregion

        #region IzmjenaZamjenskogLijekaViewModel
        public MyICommand IzmjeniZamjenskiLijekKomanda { get; set; }
        public MyICommand OdustaniOdIzmjeneZamjenskog { get; set; }
        public MyICommand PotvrdiIzmjenuZamjenskog { get; set; }

        private static string nazivZamjenskog;
        private static string sifraZamjenskog;
        public string NazivZamjenskog
        {
            get { return nazivZamjenskog; }
            set { nazivZamjenskog = value; OnPropertyChanged("NazivZamjenskog"); PotvrdiIzmjenuZamjenskog.RaiseCanExecuteChanged(); }
        }

        public string SifraZamjenskog
        {
            get { return sifraZamjenskog; }
            set { sifraZamjenskog = value; OnPropertyChanged("SifraZamjenskog"); PotvrdiIzmjenuZamjenskog.RaiseCanExecuteChanged(); }
        }

        private void IzmjeniZamjenski()
        {
            if (izabraniZamjenskiLijek != null)
            {
                IzmjeniZamjenskiProzor = new IzmjeniZamjenskiLijek();
                IzmjeniZamjenskiProzor.Show();
                sifraZamjenskog = izabraniZamjenskiLijek.sifraLeka;
                nazivZamjenskog = izabraniZamjenskiLijek.nazivLeka;
                IzmjeniZamjenskiProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }

        private void ZatvoriIzmjenuZamjenskog()
        {
            IzmjeniZamjenskiProzor.Close();
        }

        private bool ValidnaPoljaZaIzmjenuZamjenskog()
        {
            if (sifraZamjenskog != null && NazivZamjenskog != null)
            {
                if (sifraZamjenskog.Trim().Equals("") || NazivZamjenskog.Trim().Equals("") || postojiSifraNovogZamjenskogLijeka())
                {
                    return false;
                }
                else if (!sifraZamjenskog.Trim().Equals("") && !NazivZamjenskog.Trim().Equals("") && !postojiSifraNovogZamjenskogLijeka())
                {
                    return true;
                }
            }
            return false;
        }

        private bool postojiSifraNovogZamjenskogLijeka()
        {
            foreach (Lek lijek in LekoviServis.Lijekovi())
            {
                if (lijek.sifraLeka.Equals(sifraZamjenskog) && lijek.idLeka != izabraniZamjenskiLijek.idLeka)
                {
                    return true;
                }
            }
            return false;
        }

        private void IzmjeniZamjenskiLijek()
        {
            Lek noviLijek = new Lek(izabraniZamjenskiLijek.idLeka, nazivZamjenskog, sifraZamjenskog);
            LekoviServis.izmjeniLijek(izabraniZamjenskiLijek, noviLijek);
            foreach (Lek lijek in LekoviServis.Lijekovi())
            {
                if(lijek.idLeka == izabraniZamjenskiLijek.idLeka)
                {
                    int idx = Lekovi.IndexOf(lijek);
                    Lekovi.RemoveAt(idx);
                    Lekovi.Insert(idx, noviLijek);
                }
            }
            
            int idx1 = ZamjenskiLekovi.IndexOf(izabraniZamjenskiLijek);
            ZamjenskiLekovi.RemoveAt(idx1);
            ZamjenskiLekovi.Insert(idx1, noviLijek);
            
            IzmjeniZamjenskiProzor.Close();
        }

        #endregion

        #region SastojciViewModel

        public MyICommand SastojciProzorKomanda { get; set; }
        public MyICommand ZatvoriSastojkeKomanda { get; set; }

        private string tekstSastojci;
        public string TekstSastojci
        {
            get { return tekstSastojci; }
            set { tekstSastojci = value; OnPropertyChanged("TekstSastojci"); }
        }

        private string sastojciPretraga;
        public string SastojciPretraga
        {
            get { return sastojciPretraga; }
            set { sastojciPretraga = value; OnPropertyChanged("SastojciPretraga"); PretragaSastojaka(); }
        }

        private void OtvoriSastojkeLijeka()
        {
            if (izabraniLijek != null)
            {
                SastojciProzor = new Sastojci();
                tekstSastojci = "Sastojci za lijek: " + izabraniLijek.nazivLeka;
                dodajSastojkeLijeka();
                SastojciProzor.DataContext = this;
                SastojciProzor.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }

        private void dodajSastojkeLijeka()
        {
            SastojciLeka = new ObservableCollection<Sastojak>();
            foreach (Lek lijek in LekoviMenadzer.lijekovi)
            {
                if (lijek.idLeka == izabraniLijek.idLeka)
                {
                    dodajSastojakLijeka(lijek);
                }
            }
        }

        private void dodajSastojakLijeka(Lek lijek)
        {
            foreach (Sastojak sastojak in lijek.sastojci)
            {
                SastojciLeka.Add(sastojak);
            }
        }

        private void ZatvoriSastojke()
        {
            SastojciProzor.Close();
        }

        private void PretragaSastojaka()
        {
            SastojciLeka.Clear();
            if (izabraniLijek.sastojci != null)
            {
                foreach (Sastojak sastojak in izabraniLijek.sastojci)
                {
                    if (sastojak.naziv.StartsWith(SastojciPretraga))
                    {
                        SastojciLeka.Add(sastojak);
                    }
                }
            }
        }

        #endregion

        #region DodajSastojak
        public MyICommand DodajSastojakProzorKomanda { get; set; }
        public MyICommand ZatvoriDodavanjeSastojka { get; set; }
        public MyICommand PotvrdiDodavanjeSastojka { get; set; }

        private string sastojakNaziv;
        private string sastojakKolicina;

        public string SastojakNaziv
        {
            get { return sastojakNaziv; }
            set { sastojakNaziv = value; PotvrdiDodavanjeSastojka.RaiseCanExecuteChanged(); }
        }

        public string SastojakKolicina
        {
            get { return sastojakKolicina; }
            set { sastojakKolicina = value; PotvrdiDodavanjeSastojka.RaiseCanExecuteChanged(); }
        }

        private void OtvoriDodavanjeSastojaka()
        {
            DodajSastojakProzor = new DodajSastojak();
            DodajSastojakProzor.Show();
            DodajSastojakProzor.DataContext = this;
        }

        private void ZatvoriDodavanjaSastojka()
        {
            DodajSastojakProzor.Close();
        }

        private bool ValidnaPoljaZaUnosSastojka()
        {
            if (sastojakKolicina != null && sastojakNaziv != null)
            {
                if (jeBroj(sastojakKolicina))
                {

                    if (sastojakKolicina.Trim().Equals("") || sastojakNaziv.Trim().Equals(""))
                    {
                        return false;
                    }
                    else if (!sastojakKolicina.Trim().Equals("") && !sastojakNaziv.Trim().Equals(""))
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private void DodajSastojakLijeku()
        {
            Sastojak sastojak = new Sastojak(sastojakNaziv, double.Parse(sastojakKolicina));
            LekoviServis.dodajSastojak(sastojak, izabraniLijek);
            SastojciLeka.Add(sastojak);
            DodajSastojakProzor.Close();

        }
        #endregion

        #region ObrisiSastojakViewModel
        public MyICommand ObrisiSastojak { get; set; }

        private static Sastojak izabraniSastojak;
        public Sastojak IzabraniSastojak
        {
            get { return izabraniSastojak; }
            set
            {
                izabraniSastojak = value;
            }
        }

        private void ObrisiSastojakLijeka()
        {
            if(izabraniSastojak != null)
            {
                LekoviServis.obrisiSastojakLijeka(izabraniLijek, izabraniSastojak);
                SastojciLeka.Remove(izabraniSastojak);
            }
            else
            {
                MessageBox.Show("Morate izabrati sastojak!");
            }
        }

        #endregion

        #region IzmjeniSastojakViewModel
        public MyICommand IzmjeniSastojakKomanda { get; set; }
        public MyICommand OdustaniOdIzmjeneSastojka { get; set; }
        public MyICommand IzmjeniSastojak { get; set; }

        private string nazivSastojkaIzmjena;
        private string kolicinaSastojkaIzmjena;
        
        public string NazivSastojkaIzmjena
        {
            get { return nazivSastojkaIzmjena; }
            set { nazivSastojkaIzmjena = value;  OnPropertyChanged("NazivSastojkaIzmjena"); IzmjeniSastojak.RaiseCanExecuteChanged(); }
        }

        public string KolicinaSastojkaIzmjena
        {
            get { return kolicinaSastojkaIzmjena; }
            set { kolicinaSastojkaIzmjena = value; OnPropertyChanged("KolicinaSastojkaIzmjena"); IzmjeniSastojak.RaiseCanExecuteChanged(); }
        }

        private void IzmjenaSastojakaProzor()
        {
            if (izabraniSastojak != null)
            {
                IzmjeniSastojakProzor = new IzmjeniSastojak();
                IzmjeniSastojakProzor.Show();
                nazivSastojkaIzmjena = izabraniSastojak.naziv;;
                kolicinaSastojkaIzmjena = izabraniSastojak.kolicina.ToString();//ne prikaze u text box
                IzmjeniSastojakProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati sastojak!");
            }
        }

        private void PotvrdiIzmjenuSastojka()
        {
            Sastojak sastojak = new Sastojak(nazivSastojkaIzmjena, double.Parse(kolicinaSastojkaIzmjena));
            LekoviServis.izmjeniSastojakLijeka(izabraniLijek, izabraniSastojak, sastojak);
            int idx = SastojciLeka.IndexOf(izabraniSastojak);
            SastojciLeka.RemoveAt(idx);
            SastojciLeka.Insert(idx, sastojak);
            IzmjeniSastojakProzor.Close();
        }

        private void ZatvoriDodavanjaSastojaka()
        {
            IzmjeniSastojakProzor.Close();
        }

        private bool ValidnaPoljaZaIzmjenuSastojka()
        {
            if (kolicinaSastojkaIzmjena != null && nazivSastojkaIzmjena != null)
            {
                if (jeBroj(kolicinaSastojkaIzmjena))
                {
                    if (kolicinaSastojkaIzmjena.Trim().Equals("") || nazivSastojkaIzmjena.Trim().Equals(""))
                    {
                        return false;
                    }
                    else if (!kolicinaSastojkaIzmjena.Trim().Equals("") && !nazivSastojkaIzmjena.Trim().Equals(""))
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        #endregion

        #region OdbijeniLijekovi
        private string pretragaOdbijenih;
        private static Lek izabraniOdbijeniLijek;
        private string obrazlozenjeTekst;
        public string ObrazlozenjeTekst
        {
            get { return obrazlozenjeTekst; }
            set { obrazlozenjeTekst = value; OnPropertyChanged("ObrazlozenjeTekst"); }
        }
        public Lek IzabraniOdbijeniLijek
        {
            get { return izabraniOdbijeniLijek;  }
            set { izabraniOdbijeniLijek = value; }
        }
        public string PretragaOdbijenih
        {
            get { return pretragaOdbijenih; }
            set { pretragaOdbijenih = value; OnPropertyChanged("PretragaOdbijenih"); PretraziOdbijene(); }
        }
        public MyICommand OdbijeniLijekovi { get; set; }
        public MyICommand ZatvoriOdbijene { get; set; }
        public MyICommand ObrazlozenjeKomanda { get; set; }
        public MyICommand ZatvoriObrazlozenje { get; set; }
        private void PrikaziOdbijeneLijekove()
        {
            OdbijeniLijekoviProzor = new OdbijeniLijekovi();
            OdbijeniLijekoviProzor.Show();
            OdbijeniLijekoviProzor.DataContext = this;
            dodajOdbijene();
        }

        private void ZatvoriOdbijeneProzor()
        {
            OdbijeniLijekoviProzor.Close();
        }

        private void dodajOdbijene()
        {
            OdbijeniLekovi.Clear();
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (!zahtjev.odobrenZahtev && zahtjev.obradjenZahtev)
                {
                    OdbijeniLekovi.Add(zahtjev.lek);
                }
            }
        }

        private void PretraziOdbijene()
        {
            OdbijeniLekovi.Clear();
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (!zahtjev.odobrenZahtev && zahtjev.obradjenZahtev && zahtjev.lek.nazivLeka.StartsWith(PretragaOdbijenih))
                {
                    OdbijeniLekovi.Add(zahtjev.lek);
                }
            }
        }

        private void PrikaziObrazlozenje()
        {
            if(izabraniOdbijeniLijek != null)
            {
                ObrazlozenjeProzor = new Obrazlozenje();
                ObrazlozenjeProzor.Show();
                foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
                {
                    if (zahtjev.lek.idLeka == izabraniOdbijeniLijek.idLeka)
                    {
                        obrazlozenjeTekst = zahtjev.obrazlozenjeOdbijanja;
                    }
                }
                ObrazlozenjeProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }
        private void ZatvoriObrazlozenjeOdbijanja()
        {
            ObrazlozenjeProzor.Close();
        }
        #endregion

        #region IzmjenaOdbijenihLijekova
        public MyICommand IzmjeniOdbijeniLijek { get; set; }
        public MyICommand OdustaniOdIzmjeneOdbijenog { get; set; }
        public MyICommand PotvrdiIzmjenuOdbijenog { get; set; }
        public MyICommand IzmjeniSastojakOdbijenog { get; set; }
        public MyICommand ZatvoriSastojkeOdbijenog { get; set; }
        public MyICommand IzmjeniSastojkeOdbijenogProzor { get; set; }
        public MyICommand ZatvoriIzmjenuOdbijenogSastojka { get; set; }
        public MyICommand IzmjeniSastojakOdbijenogLijeka { get; set; }


        private Sastojak sastojakZaIzmjenu;
        private string sifraOdbijenog;
        private string nazivOdbijenog;
        private string tekstOdbijeni;
        private string nazivOdbijenogSastojak;
        private string kolicinaOdbijenogSastojak;
        public string NazivOdbijenogSastojak
        {
            get { return nazivOdbijenogSastojak; }
            set { nazivOdbijenogSastojak = value; OnPropertyChanged("NazivOdbijenogSastojak"); IzmjeniSastojakOdbijenogLijeka.RaiseCanExecuteChanged(); }
        }
        public string KolicinaOdbijenogSastojak
        {
            get { return kolicinaOdbijenogSastojak; }
            set { kolicinaOdbijenogSastojak = value; OnPropertyChanged("KolicinaOdbijenogSastojak"); IzmjeniSastojakOdbijenogLijeka.RaiseCanExecuteChanged(); }
        }
        public Sastojak SastojakZaIzmjenu
        {
            get { return sastojakZaIzmjenu; }
            set { sastojakZaIzmjenu = value; OnPropertyChanged("SastojakZaIzmjenu"); }
        }
        public string TeksOdbijeni
        {
            get { return tekstOdbijeni; }
            set { tekstOdbijeni = value; OnPropertyChanged("TekstOdbijeni"); }
        }
        public string SifraOdbijenog
        {
            get { return sifraOdbijenog; }
            set { sifraOdbijenog = value; OnPropertyChanged("SifraOdbijenog"); PotvrdiIzmjenuOdbijenog.RaiseCanExecuteChanged(); }
        }
        public string NazivOdbijenog
        {
            get { return nazivOdbijenog; }
            set { nazivOdbijenog = value; OnPropertyChanged("NazivOdbijenog"); PotvrdiIzmjenuOdbijenog.RaiseCanExecuteChanged(); }
        }
        private void IzmjeniOdbijeni()
        {
            if(izabraniOdbijeniLijek != null)
            {
                IzmjeniOdbijeniProzor = new IzmjeniOdbijeniLijek();
                IzmjeniOdbijeniProzor.Show();
                nazivOdbijenog = izabraniOdbijeniLijek.nazivLeka;
                sifraOdbijenog = izabraniOdbijeniLijek.sifraLeka;
                IzmjeniOdbijeniProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }

        private void OdustaniOdIzmjeneOdbijenogLijeka()
        {
            IzmjeniOdbijeniProzor.Close();
        }

        private void PotvrdiIzmjenuOdbijenogLijeka()
        {
            Lek lijek = new Lek(izabraniOdbijeniLijek.idLeka, nazivOdbijenog, sifraOdbijenog);
            LekoviServis.IzmjeniOdbijeniLijek(izabraniOdbijeniLijek, lijek);
            LekoviMenadzer.sacuvajIzmeneZahteva();
            int idx = OdbijeniLekovi.IndexOf(izabraniOdbijeniLijek);
            OdbijeniLekovi.RemoveAt(idx);
            OdbijeniLekovi.Insert(idx, lijek);

            IzmjeniOdbijeniProzor.Close();
        }

        private bool ValidnaPoljaZaImjenuOdbijenog()
        {
            if (sifraOdbijenog != null && nazivOdbijenog != null)
            {
                if (sifraOdbijenog.Trim().Equals("") || nazivOdbijenog.Trim().Equals("") || postojiSifraOdbijenogLijeka())
                {
                    return false;
                }
                else if (!sifraOdbijenog.Trim().Equals("") && !nazivOdbijenog.Trim().Equals("") && !postojiSifraOdbijenogLijeka())
                {
                    return true;
                }
            }
            return false;
        }

        private bool postojiSifraOdbijenogLijeka()
        {
            foreach (Lek lijek in LekoviServis.Lijekovi())
            {
                if (lijek.sifraLeka.Equals(sifraOdbijenog) && lijek.idLeka != izabraniOdbijeniLijek.idLeka)
                {
                    return true;
                }
            }
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)//mogu izmjeniti u sifru postojeceg obijenog???
            {
                if (zahtjev.lek.sifraLeka.Equals(sifraOdbijenog) && zahtjev.lek.idLeka != izabraniOdbijeniLijek.idLeka)
                {
                    return true;
                }
            }
            return false;
        }

        private void OtvoriIzmjenuSastojkaOdbijenog()
        {
            IzmjeniSastojakOdbijenogProzor = new IzmjeniSastojkeOdbijenog();
            IzmjeniSastojakOdbijenogProzor.Show();
            tekstOdbijeni = "Sastojci za lijek: " + izabraniOdbijeniLijek.nazivLeka;
            IzmjeniSastojakOdbijenogProzor.DataContext = this;
            dodajSastojkeOdbijenog();
        }

        private void dodajSastojkeOdbijenog()
        {
            SastojciLijeka = new ObservableCollection<Sastojak>();

            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (zahtjev.lek.sifraLeka == izabraniOdbijeniLijek.sifraLeka)
                {
                    dodajSastojkeZahtjevu(zahtjev);
                }
            }
        }

        private void dodajSastojkeZahtjevu(ZahtevZaLekove zahtjev)
        {
            SastojciOdbijenog = new ObservableCollection<Sastojak>();
            foreach (Sastojak sastojak in zahtjev.lek.sastojci)
            {
                SastojciOdbijenog.Add(sastojak);
            }
        }

        private void OtvoriIzmjenuSastojka()
        {
            if (sastojakZaIzmjenu != null) {
                IzmjenaSastojkaOdbijenogProzor = new IzmjeniSastojakOdbijenog();
                IzmjenaSastojkaOdbijenogProzor.Show();
                nazivOdbijenogSastojak = sastojakZaIzmjenu.naziv;
                kolicinaOdbijenogSastojak = sastojakZaIzmjenu.kolicina.ToString();
                IzmjenaSastojkaOdbijenogProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati sastojak!");
            }
        }

        private void PotvrdiIzmjenuSastojkaOdbijenog()
        {
            Sastojak sastojak = new Sastojak(nazivOdbijenogSastojak, double.Parse(kolicinaOdbijenogSastojak));
            LekoviServis.izmjeniSastojakOdbijenogLijeka(izabraniOdbijeniLijek, sastojakZaIzmjenu, sastojak);
            int idx = SastojciOdbijenog.IndexOf(sastojakZaIzmjenu);
            SastojciOdbijenog.RemoveAt(idx);
            SastojciOdbijenog.Insert(idx, sastojak);
            IzmjenaSastojkaOdbijenogProzor.Close();
        }

        private bool ValidnaPoljaZaIzmjenuOdbijenog()
        {
            if (kolicinaOdbijenogSastojak != null && nazivOdbijenogSastojak != null)
            {
                if (jeBroj(kolicinaOdbijenogSastojak))
                {
                    if (kolicinaOdbijenogSastojak.Trim().Equals("") || nazivOdbijenogSastojak.Trim().Equals(""))
                    {
                        return false;
                    }
                    else if (!kolicinaOdbijenogSastojak.Trim().Equals("") && !nazivOdbijenogSastojak.Trim().Equals(""))
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private void ZatvoriIzmjenuOdbijenog()
        {
            IzmjenaSastojkaOdbijenogProzor.Close();
        }

        private void ZatvoriSastojkeOdbijene()
        {
            IzmjeniSastojakOdbijenogProzor.Close();
        }
        #endregion

        #region BrisanjeOdbijenogZahtjeva
        public MyICommand BrisanjeOdbijenogLijeka { get; set; }
        public MyICommand ObrisiOdbijeniLijek { get; set; }
        public MyICommand OdustaniOdBrisanjaOdbijenog { get; set; }


        private void PotvrdiBrisanjeOdbijenogZahtjeva()
        {
            if(izabraniOdbijeniLijek != null)
            {
                BrisanjeOdbijenogLijekaProzor = new BrisanjeOdbijenogLijeka();
                BrisanjeOdbijenogLijekaProzor.Show();
                BrisanjeOdbijenogLijekaProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }

        private void ObrisiOdbijeni()
        {
            ukloniZahtjevZaLijek(nadjiIzabraniZahtjev());
            BrisanjeOdbijenogLijekaProzor.Close();
        }

        private ZahtevZaLekove nadjiIzabraniZahtjev()
        {
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (zahtjev.lek.sifraLeka.Equals(izabraniOdbijeniLijek.sifraLeka))
                {
                    return zahtjev;
                }
            }
            return null;
        }

        private void ukloniZahtjevZaLijek(ZahtevZaLekove izabraniZahtjev)
        {
            LekoviMenadzer.zahteviZaLekove.Remove(izabraniZahtjev);
            OdbijeniLekovi.Remove(IzabraniOdbijeniLijek);
            LekoviMenadzer.sacuvajIzmeneZahteva();
        }

        private void ZatvoriBrisanjeOdbijenog()
        {
            BrisanjeOdbijenogLijekaProzor.Close();
        }

        #endregion

        #region PonovnoSlanjeZahtjeva
        public MyICommand PonovnoSlanjeZahtjeva { get; set; }
        public MyICommand OdustaniOdPonovnogSlanjaZahtjeva { get; set; }
        public MyICommand PonovoPosaljiLijek { get; set; }

        private void PonovoPosaljiZahtjev()
        {
            if(izabraniOdbijeniLijek != null)
            {
                PonovnoSlanjeZahtjevaProzor = new PotvrdaSlanjaZahtjeva();
                PonovnoSlanjeZahtjevaProzor.Show();
                PonovnoSlanjeZahtjevaProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }

        private void PosaljiZahtjev()
        {
            napraviNoviZahtjev();
            sacuvajIzmjene();
            PonovnoSlanjeZahtjevaProzor.Close();
        }

        private void sacuvajIzmjene()
        {
            LekoviMenadzer.sacuvajIzmeneZahteva();
            LekoviServis.sacuvajIzmjene();
            OdbijeniLekovi.Remove(izabraniOdbijeniLijek);
        }

        private void napraviNoviZahtjev()
        {
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (zahtjev.lek.sifraLeka == izabraniOdbijeniLijek.sifraLeka)
                {
                    zahtjev.obradjenZahtev = false;
                    zahtjev.odobrenZahtev = false;
                }
            }
        }

        private void OdustaniOdSlanja()
        {
            PonovnoSlanjeZahtjevaProzor.Close();
        }

        #endregion

        #region DodajZamjenskiViewModel
        public MyICommand DodajZamjenskiProzor { get; set; }
        public MyICommand NapustiDodavanjeZamjenskih { get; set; }
        public MyICommand DodajZamjenski { get; set; }
        private string tekstZamjenskiLijek;
        private Lek izabraniZamjenski;
        public Lek IzabraniZamjenski {
            get { return izabraniZamjenski; }
            set { izabraniZamjenski = value; OnPropertyChanged("IzabraniZamjenski"); }
        }
        public string TekstZamjenskiLijek
        {
            get { return tekstZamjenskiLijek; }
            set
            {
                tekstZamjenskiLijek = value; OnPropertyChanged("TekstZamjenskiLijek");
            }
        }
        private void OtvoriDodavanjeZamjenskog()
        {
            DodavanjeZamjenskogLijekaProzor = new DodajZamjenskiLijek();
            DodavanjeZamjenskogLijekaProzor.Show();
            tekstZamjenskiLijek = izabraniLijek.nazivLeka;
            DodavanjeZamjenskogLijekaProzor.DataContext = this;
            dodajZamjenskeLijekove();
        }

        private void DodavanjeZamjenskog()
        {
            LekoviServis.dodajZamjenskeLijekove(izabraniLijek, izabraniZamjenski);
            ZamjenskiLekovi.Add(izabraniZamjenski);
            DodavanjeZamjenskogLijekaProzor.Close();
        }

        private void NapustiZamjenske()
        {
            DodavanjeZamjenskogLijekaProzor.Close();
        }
        private void dodajZamjenskeLijekove()
        {
            ZamjenskiLijekovi = new ObservableCollection<Lek>();
            foreach (Lek lijek in LekoviServis.Lijekovi())
            {
                if (lijek.idLeka != izabraniLijek.idLeka && !postojiZamjenski(lijek))
                {
                    ZamjenskiLijekovi.Add(lijek);
                }
            }
        }

        private bool postojiZamjenski(Lek lijek)
        {
            foreach (int zamjenski in izabraniLijek.zamenskiLekovi)
            {
                if (zamjenski == lijek.idLeka)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

    }



}