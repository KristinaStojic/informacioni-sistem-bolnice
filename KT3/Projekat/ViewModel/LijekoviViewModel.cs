using Projekat.Model;
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
        #region LijekoviViewModel
        private ObservableCollection<Lek> lekovi;
        public ObservableCollection<Lek> Lekovi { get { return lekovi; } set { lekovi = value; OnPropertyChanged("Lekovi"); } }

        private ObservableCollection<Lek> zamjenskiLekovi;
        private ObservableCollection<Lek> zamjenskiLekoviDodavanje;
        public ObservableCollection<Lek> ZamjenskiLekovi { get { return zamjenskiLekovi; } set { zamjenskiLekovi = value; OnPropertyChanged("ZamjenskiLekovi"); } }
        public ObservableCollection<Lek> ZamjenskiLekoviDodavanje { get { return zamjenskiLekoviDodavanje; } set { zamjenskiLekoviDodavanje = value; OnPropertyChanged("ZamjenskiLekoviDodavanje"); } }

        private ObservableCollection<Sastojak> sastojciLijeka;
        public ObservableCollection<Sastojak> SastojciLijeka { get { return sastojciLijeka; } set { sastojciLijeka = value; OnPropertyChanged("SastojciLijeka"); } }

        public static Window DodavanjeLijekaProzor{ get; set; }
        public static Window DodavanjeSastojkaProzor { get; set; }
        public static Window DodavanjeNovogSastojkaProzor { get; set; }
        public static Window BrisanjeLijekaProzor { get; set; }
        public static Window ZahtjeviProzor { get; set; }
        public static Window IzmjenaLijekaProzor { get; set; }
        public static Window ZamjenskiLijekoviProzor{ get; set; }
        public static Window DodajZamjenskiProzor{ get; set; }

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
            ZamjenskiLijekoviKomanda = new MyICommand(OtvoriZamjenskeLijekove);
            NapustiZamjenskeKomanda = new MyICommand(ZatvoriZamjenske);
            DodajZamjenskiLijekKomanda = new MyICommand(DodajZamjenskiLijekProzor);
            ZatvoriDodavanjeZamjenskogKomanda = new MyICommand(ZatvoriDodavanjeZamjenskog);
            DodajZamjenskiProzorKomanda = new MyICommand(DodajZamjenskeProzor);
        }

        private void inicijalizujElemente()
        {
            Lekovi = new ObservableCollection<Lek>();
            SastojciLijeka = new ObservableCollection<Sastojak>();
            ZamjenskiLekovi = new ObservableCollection<Lek>();
            ZamjenskiLekoviDodavanje = new ObservableCollection<Lek>();
            foreach (Lek lijek in LekoviServis.Lijekovi())
            {
                Lekovi.Add(lijek);
            }
        }
        #endregion

        #region BrisanjeLijekaViewModel
        public MyICommand ObrisiLijekKomanda { get; set; }
        public MyICommand ObrisiLijekProzor { get; set; }
        public MyICommand ZatvoriBrisanjeLijekaKomanda { get; set; }

        private static Lek izabraniLijek;
        public Lek IzabraniLijek
        {
            get { return izabraniLijek; }
            set
            {
                izabraniLijek = value;
                ObrisiLijekKomanda.RaiseCanExecuteChanged();
            }
        }
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
        private static Lek uneseniLijek = new Lek();
        public MyICommand DodajLijekKomanda { get; set; }
        public MyICommand DodajLijekProzor { get; set; }
        public MyICommand DodajSastojakLijekuProzorKomanda { get; set; }
        public MyICommand ZatvoriDodavanjeLijekaKomanda { get; set; }
        public MyICommand DodajSastojakKomanda { get; set; }
        public MyICommand ZatvoriDodavanjeSastojakaKomanda { get; set; }
        public MyICommand DodajNoviSastojak { get; set; }
        public MyICommand DodajSastojakZatvori { get; set; }


        
        private static string nazivLijeka;
        private static string sifraLijeka;
        private static string nazivSastojka;
        private static string kolicinaSastojka;

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
            return false;
        }

        public string NazivLijeka
        {
            get { return nazivLijeka; }
            set { nazivLijeka = value; DodajLijekKomanda.RaiseCanExecuteChanged(); DodajSastojakLijekuProzorKomanda.RaiseCanExecuteChanged(); }
        }

        public string SifraLijeka
        {
            get { return sifraLijeka; }
            set { sifraLijeka = value; DodajLijekKomanda.RaiseCanExecuteChanged(); DodajSastojakLijekuProzorKomanda.RaiseCanExecuteChanged(); }
        }

        public string NazivSastojka
        {
            get { return nazivSastojka; }
            set { nazivSastojka = value; DodajNoviSastojak.RaiseCanExecuteChanged(); }
        }

        public string KolicinaSastojka
        {
            get { return kolicinaSastojka; }
            set { kolicinaSastojka = value; DodajNoviSastojak.RaiseCanExecuteChanged(); }
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
            DodavanjeLijekaProzor = new DodajLijek();
            DodavanjeLijekaProzor.Show();
            DodavanjeLijekaProzor.DataContext = this;
        }

        private void DodajSastojakLijekuProzor()
        {
            DodavanjeSastojkaProzor = new SastojciDodavanje();
            DodavanjeSastojkaProzor.Show();
            DodavanjeSastojkaProzor.DataContext = this;
            dodajSastojke();
        }

        private void dodajSastojke()
        {
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

        #region Zahtjevi
        public MyICommand ZahtjeviKomanda { get; set; }

        private void OtvoriZahtjeve()
        {
            if(BrisanjeLijekaProzor != null)
            {
                BrisanjeLijekaProzor.Close();
            }
            if(DodavanjeLijekaProzor != null)
            {
                DodavanjeLijekaProzor.Close();
            }
            if(DodavanjeSastojkaProzor != null)
            {
                DodavanjeSastojkaProzor.Close();
            }
            if(DodavanjeNovogSastojkaProzor != null)
            {
                DodavanjeNovogSastojkaProzor.Close();
            }
            if(ZahtjeviViewModel.LijekProzor != null)
            {
                ZahtjeviViewModel.LijekProzor.Close();
            }
            ZahtjeviProzor = new Zahtjevi();
            ZahtjeviProzor.Show();
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
            if (ZamjenskiLijekovi.ZamjenskiLekovi != null)
            {
                int idx1 = ZamjenskiLijekovi.ZamjenskiLekovi.IndexOf(izabraniLijek);
                ZamjenskiLijekovi.ZamjenskiLekovi.RemoveAt(idx1);
                ZamjenskiLijekovi.ZamjenskiLekovi.Insert(idx1, lijek);
            }
            IzmjenaLijekaProzor.Close();
        }
        #endregion

        #region ZamjenskiLijekovi
        public MyICommand ZamjenskiLijekoviKomanda { get; set; }
        public MyICommand NapustiZamjenskeKomanda { get; set; }
        public MyICommand DodajZamjenskiLijekKomanda { get; set; }
        public MyICommand ZatvoriDodavanjeZamjenskogKomanda { get; set; }
        public MyICommand DodajZamjenskiProzorKomanda { get; set; }
        private void OtvoriZamjenskeLijekove()
        {
            if (izabraniLijek == null)
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
            else
            {
                ZamjenskiLijekoviProzor = new ZamjenskiLijekovi();
                dodajZamjenskeLijekove();
                tekstZamjenskog = "Zamjenski lijekovi za lijek: " + izabraniLijek.nazivLeka;
                ZamjenskiLijekoviProzor.Show();
                ZamjenskiLijekoviProzor.DataContext = this;
            }
        }
        private void dodajZamjenskeLijekove()
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
                foreach (Lek zamjenski in LekoviMenadzer.lijekovi)
                {
                    if (zamjenski.idLeka == zamjenskiLijek)
                    {
                        ZamjenskiLekovi.Add(zamjenski);
                    }
                }
            }
        }

        private static string tekstZamjenskog;
        public string TekstZamjenskog
        {
            get { return tekstZamjenskog; }
            set { tekstZamjenskog = value;}
        }

        private void DodajZamjenskiLijekProzor()
        {

            DodajZamjenskiProzor = new DodajZamjenskiLijek();
            DodajZamjenskiProzor.Show();
            DodajZamjenskiProzor.DataContext = this;
            dodajMoguceZamjenske();
        }

        private void dodajMoguceZamjenske()
        {
            zamjenskiLekoviDodavanje.Clear();
            foreach (Lek lijek in LekoviServis.Lijekovi())
            {
                if (lijek.idLeka != izabraniLijek.idLeka && !postojiZamjenski(lijek))
                {
                    zamjenskiLekoviDodavanje.Add(lijek);
                }
            }
        }

        private bool postojiZamjenski(Lek lijek)
        {
            foreach(int zamjenski in izabraniLijek.zamenskiLekovi)
            {
                if(zamjenski == lijek.idLeka)
                {
                    return true;
                }
            }
            return false;
        }

        private void ZatvoriZamjenske()
        {
            ZamjenskiLijekoviProzor.Close();
        }
        private void ZatvoriDodavanjeZamjenskog()
        {
            DodajZamjenskiProzor.Close();
        }

        private void DodajZamjenskeProzor()
        {

        }
        #endregion
    }


}
