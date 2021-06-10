using Model;
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
    class LekarLekoviViewModel : BindableBase
    {
        private ObservableCollection<ZahtevZaLekove> tabelaZahteva;
        public ObservableCollection<ZahtevZaLekove> TabelaZahteva { get { return tabelaZahteva; } set { tabelaZahteva = value; OnPropertyChanged("TabelaZahteva"); } }

        private ObservableCollection<Lek> tabelaLekova;
        public ObservableCollection<Lek> TabelaLekova { get { return tabelaLekova; } set { tabelaLekova = value; OnPropertyChanged("TabelaLekova"); } }

        private ObservableCollection<Sastojak> tabelaSastojaka;
        public ObservableCollection<Sastojak> TabelaSastojaka { get { return tabelaSastojaka; } set { tabelaSastojaka = value; OnPropertyChanged("TabelaSastojaka"); } }


        private ObservableCollection<Sastojak> sastojciZahteva;
        public ObservableCollection<Sastojak> SastojciZahteva { get { return sastojciZahteva; } set { sastojciZahteva = value; OnPropertyChanged("SastojciZahteva"); } }


        private ZahtevZaLekove izabraniZahtev;
        public ZahtevZaLekove IzabraniZahtev { get { return izabraniZahtev; } set { izabraniZahtev = value; OnPropertyChanged("IzabraniZahtev"); } }

        private Lek izabraniLek;
        public Lek IzabraniLek { get { return izabraniLek; } set { izabraniLek = value; OnPropertyChanged("IzabraniLek"); } }

        private Sastojak izabraniSastojak;
        public Sastojak IzabraniSastojak { get { return izabraniSastojak; } set { izabraniSastojak = value; OnPropertyChanged("IzabraniSastojak"); } }

        private Sastojak sastojakZahteva;
        public Sastojak SastojakZahteva { get { return sastojakZahteva; } set { sastojakZahteva = value; OnPropertyChanged("SastojakZahteva"); } }

        private string sifraLekaZahtev;
        public string SifraLekaZahtev { get { return sifraLekaZahtev; } set { sifraLekaZahtev = value; OnPropertyChanged("SifraLekaZahtev"); } }

        private string nazivLekaZahtev;
        public string NazivLekaZahtev { get { return nazivLekaZahtev; } set { nazivLekaZahtev = value; OnPropertyChanged("NazivLekaZahtev"); } }

        private string razlogOdbijanja;
        public string RazlogOdbijanja { get { return razlogOdbijanja; } set { razlogOdbijanja = value; OnPropertyChanged("RazlogOdbijanja"); } }


        private string datumLekaZahtev;
        public string DatumLekaZahtev { get { return datumLekaZahtev; } set { datumLekaZahtev = value; OnPropertyChanged("DatumLekaZahtev"); } }

        private string nazivLeka;
        public string NazivLeka { get { return nazivLeka; } set { nazivLeka = value; OnPropertyChanged("NazivLeka"); } }
        private string sifraLeka;
        public string SifraLeka { get { return sifraLeka; } set { sifraLeka = value; OnPropertyChanged("SifraLeka"); } }

        private string nazivSastojka;
        public string NazivSastojka { get { return nazivSastojka; } set { nazivSastojka = value; OnPropertyChanged("NazivSastojka"); } }

        private double kolicinaSastojka;
        public double KolicinaSastojka { get { return kolicinaSastojka; } set { kolicinaSastojka = value; OnPropertyChanged("KolicinaSastojka"); } }

        public MyICommand ObradiZahtevKomanda { get; set; }
        public MyICommand ObrisiZahtevKomanda { get; set; }
        public MyICommand NazadIzZahtevaKomanda { get; set; }
        public MyICommand IzmeniLekKomanda { get; set; }
        public MyICommand SastojciLekaKomanda { get; set; }
        public MyICommand ZamenskiLekoviKomanda { get; set; }
        public MyICommand PotvrdiOdobravanjeLekaKomanda { get; set; }
        public MyICommand OdbijLekKomanda { get; set; }
        public MyICommand PotvrdiOdbijanjeLeka { get; set; }
        public MyICommand OdustaniOdOdbijanjaLekaKomanda { get; set; }
        public MyICommand PotvrdiBrisanjeZahtevaKomanda { get; set; }
        public MyICommand OdustaniOdBrisanjaZahtevaKomanda { get; set; }
        public MyICommand PotvrdiIzmenuLekaKomanda { get; set; }
        public MyICommand OdustaniOdIzmeneLekaKomanda { get; set; }
        public MyICommand DodajSastojakKomanda { get; set; }
        public MyICommand PotvrdiDodavanjeSastojkaKomanda { get; set; }
        public MyICommand IzmeniSastojakKomanda { get; set; }
        public MyICommand PotvrdiIzmenuSastojkaKomanda { get; set; }
        public MyICommand OdustaniOdIzmeneSastojkaKomanda { get; set; }
        public MyICommand ObrisiSastojakKomanda { get; set; }
        public MyICommand PotvrdiBrisanjeSastojkaKomanda { get; set; }
        public MyICommand NazadIzSastojakaKomanda { get; set; }
        public static Window OtvoriObraduZahteva { get; set; }
        public static Window OtvoriBrisanjeZahteva { get; set; }
        public static Window OtvoriIzmenuLeka { get; set; }
        public static Window OtvoriSastojkeLeka { get; set; }
        public static Window OtvoriZamenskeLekove { get; set; }
        public static Window OtvoriOdbijanjeLeka { get; set; }
        public static Window DodajSastojakLeka { get; set; }
        public static Window IzmeniSastojakLeka { get; set; }
        public static Window ObrisiSastojakLeka { get; set; }

        public LekarLekoviViewModel()
        {
            DodajZahteve();
            DodajLekove();
            ObradiZahtevKomanda = new MyICommand(ObradiZahtev);
            ObrisiZahtevKomanda = new MyICommand(ObrisiZahtev);
            NazadIzZahtevaKomanda = new MyICommand(NazadIzZahteva);
            IzmeniLekKomanda = new MyICommand(IzmeniLek);
            SastojciLekaKomanda = new MyICommand(SastojciLeka);
            ZamenskiLekoviKomanda = new MyICommand(ZamenskiLekovi);
            PotvrdiOdobravanjeLekaKomanda = new MyICommand(PotvrdiOdobravanjeLeka);
            OdbijLekKomanda = new MyICommand(OdbijLek);
            PotvrdiOdbijanjeLeka = new MyICommand(PotvrdiOdbijanje);
            OdustaniOdOdbijanjaLekaKomanda = new MyICommand(OdustaniOdOdbijanjaLeka);
            PotvrdiBrisanjeZahtevaKomanda = new MyICommand(PotvrdiBrisanjeZahteva);
            OdustaniOdBrisanjaZahtevaKomanda = new MyICommand(OdustaniOdBrisanjaZahteva);
            PotvrdiIzmenuLekaKomanda = new MyICommand(PotvrdiIzmenuLeka);
            OdustaniOdIzmeneLekaKomanda = new MyICommand(OdustaniOdIzmeneLeka);
            DodajSastojakKomanda = new MyICommand(DodajSastojak);
            PotvrdiDodavanjeSastojkaKomanda = new MyICommand(PotvrdiDodavanjeSastojka);
            IzmeniSastojakKomanda = new MyICommand(IzmeniSastojak);
            PotvrdiIzmenuSastojkaKomanda = new MyICommand(PotvrdiIzmenuSastojka);
            OdustaniOdIzmeneSastojkaKomanda = new MyICommand(OdustaniOdIzmeneSastojka);
            ObrisiSastojakKomanda = new MyICommand(ObrisiSastojak);
            PotvrdiBrisanjeSastojkaKomanda = new MyICommand(PotvrdiBrisanjeSastojka);
            NazadIzSastojakaKomanda = new MyICommand(NazadIzSastojaka);
        }

        #region Zahtevi za lekove
        private void OdustaniOdBrisanjaZahteva()
        {
            OtvoriBrisanjeZahteva.Close();
        }

        private void PotvrdiBrisanjeZahteva()
        {
            ZahtevZaLekove izabraniZahtjev = null;
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (zahtjev.lek.sifraLeka.Equals(izabraniZahtev.sifraLeka))
                {
                    izabraniZahtjev = zahtjev;
                }
            }

            LekoviMenadzer.zahteviZaLekove.Remove(izabraniZahtjev);
            TabelaZahteva.Remove(izabraniZahtev);
            LekoviServis.sacuvajIzmeneZahteva();

            OtvoriBrisanjeZahteva.Close();
        }

        private void OdustaniOdOdbijanjaLeka()
        {
            OtvoriOdbijanjeLeka.Close();
        }

        private void PotvrdiOdbijanje()
        {
            LekoviServis.odbijaZahtev(izabraniZahtev, RazlogOdbijanja);
            ZahtevZaLekove z = izabraniZahtev;
            int idx = TabelaZahteva.IndexOf(izabraniZahtev);
            TabelaZahteva.RemoveAt(idx);
            TabelaZahteva.Insert(idx, z);
            OtvoriObraduZahteva.Close();
            OtvoriOdbijanjeLeka.Close();
        }

        private void OdbijLek()
        {
            OtvoriOdbijanjeLeka = new OdbijZahtevZaLek(IzabraniZahtev);
            OtvoriOdbijanjeLeka.Show();
            OtvoriOdbijanjeLeka.DataContext = this;
        }

        private void PotvrdiOdobravanjeLeka()
        {
            izabraniZahtev.odobrenZahtev = true;
            izabraniZahtev.obradjenZahtev = true;
            Lek lek = new Lek(LekoviServis.GenerisanjeIdLijeka(), izabraniZahtev.nazivLeka, izabraniZahtev.sifraLeka, izabraniZahtev.lek.zamenskiLekovi, izabraniZahtev.lek.sastojci);
            LekoviServis.DodajLijek(lek);
            LekoviServis.izmeniZahtev(izabraniZahtev);
            ZahtevZaLekove z = izabraniZahtev;
            int idx = TabelaZahteva.IndexOf(izabraniZahtev);
            TabelaZahteva.RemoveAt(idx);
            TabelaZahteva.Insert(idx, z);
            TabelaLekova.Add(lek);
            LekoviServis.sacuvajIzmeneZahteva();
            OtvoriObraduZahteva.Close();
        }


        private void ObradiZahtev()
        {
            if (izabraniZahtev == null)
            {
                MessageBox.Show("Niste selektovali zahtev koji zelite da obradite!");
            }
            else if (izabraniZahtev.obradjenZahtev == true)
            {
                MessageBox.Show("Izabrani zahtev je vec obradjen!");
            }
            else if (izabraniZahtev != null && izabraniZahtev.obradjenZahtev == false)
            {
                SastojciZahteva = new ObservableCollection<Sastojak>();
                //SastojciZahteva.Add = LekoviServis.nadjiSastojke(izabraniZahtev);

                DatumLekaZahtev = izabraniZahtev.datumSlanjaZahteva;
                NazivLekaZahtev = izabraniZahtev.nazivLeka;
                SifraLekaZahtev = izabraniZahtev.sifraLeka;

                foreach (Sastojak s in LekoviServis.nadjiSastojke(izabraniZahtev))
                {
                    SastojciZahteva.Add(s);
                }

                OtvoriObraduZahteva = new ObradiZahtevZaLek(izabraniZahtev);
                OtvoriObraduZahteva.Show();
                OtvoriObraduZahteva.DataContext = this;
            }


        }

        private void ObrisiZahtev()
        {
            if (izabraniZahtev == null)
            {
                MessageBox.Show("Niste selektovali zahtev koji zelite da obradite!");
            }
            else if (izabraniZahtev != null)
            {
                OtvoriBrisanjeZahteva = new ObrisiZahtevLekar(IzabraniZahtev);
                OtvoriBrisanjeZahteva.Show();
                OtvoriBrisanjeZahteva.DataContext = this;
            }
        }

        private void NazadIzZahteva()
        {

        }

        private void DodajZahteve()
        {
            TabelaZahteva = new ObservableCollection<ZahtevZaLekove>();
            foreach (ZahtevZaLekove zahtev in LekoviMenadzer.zahteviZaLekove)
            {
                TabelaZahteva.Add(zahtev);
            }
        }


        #endregion


        #region Lekovi

        private void NazadIzSastojaka()
        {
            OtvoriSastojkeLeka.Close();
        }

        private void PotvrdiBrisanjeSastojka()
        {
            LekoviServis.obrisiSastojakLekaLekar(IzabraniLek, izabraniSastojak);
            TabelaSastojaka.Remove(izabraniSastojak);
            ObrisiSastojakLeka.Close();
        }

        private void ObrisiSastojak()
        {
            if (izabraniSastojak != null)
            {
                ObrisiSastojakLeka = new ObrisiSastojakLekar(izabraniSastojak, izabraniLek);
                ObrisiSastojakLeka.DataContext = this;
                ObrisiSastojakLeka.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati sastojak!");
            }
        }

        private void OdustaniOdIzmeneSastojka()
        {
            IzmeniSastojakLeka.Close();
        }

        private void PotvrdiIzmenuSastojka()
        {
            Sastojak noviSastojak = new Sastojak(NazivSastojka, KolicinaSastojka);
            LekoviServis.izmeniSastojakLekaLekar(izabraniLek, izabraniSastojak, noviSastojak);
            int idx = TabelaSastojaka.IndexOf(izabraniSastojak);
            TabelaSastojaka.RemoveAt(idx);
            TabelaSastojaka.Insert(idx, noviSastojak);
            IzmeniSastojakLeka.Close();
        }

        private void IzmeniSastojak()
        {
            IzmeniSastojakLeka = new IzmeniSastojakLekar(izabraniLek,izabraniSastojak);
            NazivSastojka = izabraniSastojak.naziv;
            KolicinaSastojka = izabraniSastojak.kolicina;
            IzmeniSastojakLeka.DataContext = this;
            IzmeniSastojakLeka.Show();
        }

        private void PotvrdiDodavanjeSastojka()
        {
            
            Sastojak sastojak = new Sastojak(NazivSastojka, KolicinaSastojka);
            LekoviMenadzer.dodajSastojakLekar(sastojak, izabraniLek);
            TabelaSastojaka.Add(sastojak);

            DodajSastojakLeka.Close();
        }

        private void DodajSastojak()
        {
            DodajSastojakLeka = new DodajSastojakLekar(izabraniLek);
            DodajSastojakLeka.DataContext = this;
            DodajSastojakLeka.Show();
        }

        private void OdustaniOdIzmeneLeka()
        {

            OtvoriIzmenuLeka.Close();
        }

        private void DodajLekove()
        {
            TabelaLekova = new ObservableCollection<Lek>();
            foreach (Lek lek in LekoviServis.Lijekovi())
            {
                TabelaLekova.Add(lek);
            }
        }

        private void PotvrdiIzmenuLeka()
        {
            Lek noviLek = new Lek(izabraniLek.idLeka, NazivLeka, SifraLeka);
            LekoviServis.IzmeniLekoveLekar(izabraniLek, noviLek);
            Lek l = noviLek;
            int idx = TabelaLekova.IndexOf(izabraniLek);
            TabelaLekova.RemoveAt(idx);
            TabelaLekova.Insert(idx, noviLek);
            /*if (ZamenskiLekovi != null)
            {
                int idx1 = PrikazZamenskihLekovaLekar.TabelaZamenskihLekova.IndexOf(izabraniLek);
                PrikazZamenskihLekovaLekar.TabelaZamenskihLekova.RemoveAt(idx1);
                PrikazZamenskihLekovaLekar.TabelaZamenskihLekova.Insert(idx1, lek);
            }*/


            OtvoriIzmenuLeka.Close();
        }

        private void ZamenskiLekovi()
        {
            if (izabraniLek != null)
            {
                OtvoriZamenskeLekove = new PrikazZamenskihLekovaLekar(izabraniLek);
                OtvoriZamenskeLekove.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali nijedan lek!");
            }
        }
        private void SastojciLeka()
        {
            if (izabraniLek != null)
            {
                TabelaSastojaka = new ObservableCollection<Sastojak>();
                foreach (Sastojak sastojak in izabraniLek.sastojci)
                {
                    TabelaSastojaka.Add(sastojak);
                }
                OtvoriSastojkeLeka = new PrikazSastojakaLekar(izabraniLek);
                OtvoriSastojkeLeka.Show();
                OtvoriSastojkeLeka.DataContext = this;
            }
            else
            {
                MessageBox.Show("Niste selektovali nijedan lek!");
            }
        }
        private void IzmeniLek()
        {

            if (izabraniLek != null)
            {

                OtvoriIzmenuLeka = new IzmeniLekLekar(izabraniLek);
                NazivLeka = izabraniLek.nazivLeka;
                SifraLeka = izabraniLek.sifraLeka;
                OtvoriIzmenuLeka.Show();
                OtvoriIzmenuLeka.DataContext = this;
            }
            else
            {
                MessageBox.Show("Niste selektovali nijedan lek!");
            }
        }


        #endregion


    }
}
