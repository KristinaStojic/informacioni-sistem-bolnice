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
    class PremjestajOpremeViewModel : BindableBase
    {
        #region Promjenljive
        private string tekstDodavanjaStaticke;
        public static ObservableCollection<Oprema> OpremaStatickaZaDodavanje;
        public MyICommand OdustaniOdDodavanjaStaticke { get; set; }

        private ObservableCollection<Oprema> statickaZaDodavanje;
        private ObservableCollection<string> terminiDodavanjaStaticke;
        
        private Oprema izabranaStatickaDodavanje;
        private Sala izabranaSalaZaDodavanje;
        private DateTime datumPrebacivanja; private string vrijemeDodavanja;
        private string kolicinaDodavanjaStaticke;
        private ObservableCollection<Sala> saleZaDodavanjeStaticke;
        public string KolicinaDodavanjaStaticke { get { return kolicinaDodavanjaStaticke; } set { kolicinaDodavanjaStaticke = value; OnPropertyChanged("KolicinaDodavanjaStaticke"); } }//DodajStaticku.RaiseCanExecuteChanged(); } }
        public Sala IzabranaSalaZaDodavanje { get { return izabranaSalaZaDodavanje; } set { izabranaSalaZaDodavanje = value; OnPropertyChanged("IzabranaSalaZaDodavanje"); promjenjenaSala(); } }// DodajStaticku.RaiseCanExecuteChanged(); } }
        public string VrijemeDodavanja { get { return vrijemeDodavanja; } set { vrijemeDodavanja = value; OnPropertyChanged("VrijemeDodavanja"); } }// DodajStaticku.RaiseCanExecuteChanged(); } }
        public Oprema IzabranaStatickaDodavanje { get { return izabranaStatickaDodavanje; } set { izabranaStatickaDodavanje = value; OnPropertyChanged("IzabranaStatickaDodavanje"); promjenjenaOprema(); SaleViewModel.izabranaStat = izabranaStatickaDodavanje; } }//DodajStaticku.RaiseCanExecuteChanged(); } }
        public DateTime DatumPrebacivanja { get { return datumPrebacivanja; } set { datumPrebacivanja = value; OnPropertyChanged("DatumPrebacivanja"); promijenjenDatum(); } }// DodajStaticku.RaiseCanExecuteChanged(); } }
        public ObservableCollection<string> TerminiDodavanjaStaticke { get { return terminiDodavanjaStaticke; } set { terminiDodavanjaStaticke = value; OnPropertyChanged("TerminiDodavanjaStaticke"); } }
        public ObservableCollection<Oprema> StatickaZaDodavanje { get { return statickaZaDodavanje; } set { statickaZaDodavanje = value; OnPropertyChanged("StatickaZaDodavanje"); } }
        public string TekstDodavanjaStaticke { get { return tekstDodavanjaStaticke; } set { tekstDodavanjaStaticke = value; OnPropertyChanged("TekstDodavanjaStaticke"); } }
        public ObservableCollection<Sala> SaleZaDodavanjeStaticke { get { return saleZaDodavanjeStaticke; } set { saleZaDodavanjeStaticke = value; OnPropertyChanged("SaleZaDodavanjeStaticke"); } }
        public MyICommand DodajStaticku { get; set; }
        public Sala izabranaSala { get; set; }
        public PremjestajOpremeViewModel() { }
        public PremjestajOpremeViewModel(Sala izabranaSala) {
            TerminiDodavanjaStaticke = new ObservableCollection<string>();
            this.izabranaSala = izabranaSala;
            SaleZaDodavanjeStaticke = new ObservableCollection<Sala>();
            StatickaZaDodavanje = new ObservableCollection<Oprema>();
            SaleViewModel.dozvoljenaKolicinaDodavanjeStaticke = 0;
            OpremaStatickaZaDodavanje = statickaZaDodavanje;
            tekstDodavanjaStaticke = "";
            DodajStaticku = new MyICommand(DodavanjeStaticke);
            OdustaniOdDodavanjaStaticke = new MyICommand(ZatvoriDodavanjeStaticke);
            KolicinaDodavanjaStaticke = "";
            DatumPrebacivanja = DateTime.Now.Date;
            dodajTermineDodavanjaStaticke();
            dodajStaticku();
        }
        #endregion
        private void ZatvoriDodavanjeStaticke()
        {
            SaleViewModel.DodavanjeStatickeProzor.Close();
        }
        private DateTime napraviTerminPremjestaja()
        {
            DateTime? datumSlanja = DatumPrebacivanja;
            string vrijemeSlanja = VrijemeDodavanja;
            string datum = datumSlanja.Value.ToString("dd.MM.yyy", System.Globalization.CultureInfo.InvariantCulture);
            string[] datumi = datum.Split('.');
            string dan = datumi[0];
            string mjesec = datumi[1];
            string godina = datumi[2];
            string[] sati = vrijemeSlanja.Split(':');
            string sat = sati[0];
            string minuti = sati[1];
            return new DateTime(int.Parse(godina), int.Parse(mjesec), int.Parse(dan), int.Parse(sat), int.Parse(minuti), 0);
        }
        private void DodavanjeStaticke()
        {
            PremjestajOpremeServis.premjestajStatickeOpreme(IzabranaSalaZaDodavanje, int.Parse(kolicinaDodavanjaStaticke), napraviTerminPremjestaja(), izabranaSala, IzabranaStatickaDodavanje);
            SaleViewModel.DodavanjeStatickeProzor.Close();
            azurirajStaticku();
        }
        private static void azurirajStaticku()
        {
            if (SkladisteViewModel.otvoren)
            {
                SkladisteViewModel.azuriraj = true;
            }
            else
            {
                SaleViewModel.azuriraj = true;
            }
        }
        private void dodajTermineDodavanjaStaticke()
        {
            for (int termin = (int)DateTime.Now.Hour + 1; termin <= 23; termin++)
            {
                if (!postojiTermin(termin))
                {
                    TerminiDodavanjaStaticke.Add(termin + ":00");
                }
            }
        }
        private void dodajStaticku()
        {
            dodajStatickuIzSkladista();
            dodajStatickuIzSala();
        }

        private void dodajStatickuIzSkladista()
        {
            foreach (Oprema oprema in OpremaServis.Oprema())
            {
                if (oprema.Staticka)
                {
                    StatickaZaDodavanje.Add(oprema);
                }
            }
        }

        private void dodajStatickuIzSala()
        {
            foreach (Sala sala in SaleServis.Sale())
            {
                foreach (Oprema oprema in sala.Oprema)
                {
                    dodajOpremu(oprema);
                }
            }
        }

        private void dodajOpremu(Oprema oprema)
        {
            if (oprema.Staticka)
            {
                if (!postojiStatickaOprema(oprema))
                {
                    StatickaZaDodavanje.Add(oprema);
                }
            }
        }private bool postojiStatickaOprema(Oprema oprema)
        {
            foreach (Oprema statickaOprema in StatickaZaDodavanje)
            {
                if (statickaOprema.IdOpreme == oprema.IdOpreme)
                {
                    return true;
                }
            }
            return false;
        }
        private void promijenjenDatum()
        {
            if (TerminiDodavanjaStaticke != null)
            {
                if (datumPrebacivanja.Date == DateTime.Now.Date)
                {
                    dodajTermineZaDanas();
                }
                else
                {
                    dodajTermineDrugiDan();
                }
            }
        }
        private void dodajTermineZaDanas()
        {
            TerminiDodavanjaStaticke.Clear();
            for (int termin = (int)DateTime.Now.Hour + 1; termin <= 23; termin++)
            {
                if (!postojiTermin(termin))
                {
                    TerminiDodavanjaStaticke.Add(termin + ":00");
                }
            }
        }

        private void dodajTermineDrugiDan()
        {
            string[] terminPremjestaja = TerminiDodavanjaStaticke[0].Split(':');
            string prviTermin = terminPremjestaja[0];
            for (int termin = int.Parse(prviTermin); termin > 0; termin--)
            {
                if (!postojiTermin(termin))
                {
                    TerminiDodavanjaStaticke.Insert(0, termin + ":00");
                }
            }
        }
        private bool postojiTermin(int termin)
        {
            foreach (Premjestaj premjestaj in PremjestajServis.Premjestaji())
            {
                if (premjestaj.datumIVrijeme.Hour.ToString().Equals(termin.ToString()))
                {
                    return true;
                }
            }
            return false;
        }
        private void promjenjenaOprema()
        {
            SaleZaDodavanjeStaticke.Clear();
            foreach (Sala sala in SaleServis.Sale())
            {
                foreach (Oprema oprema in sala.Oprema)
                {
                    dodajSaluStaticka(oprema, sala);
                }
            }
        }

        private void dodajSaluStaticka(Oprema oprema, Sala sala)
        {
            if (izabranaStatickaDodavanje != null)
            {
                if (oprema.IdOpreme == IzabranaStatickaDodavanje.IdOpreme)
                {
                    if (sala.Id != izabranaSala.Id && provjeriPreostalo(oprema, sala))
                    {
                        SaleZaDodavanjeStaticke.Add(sala);
                    }
                }
            }
        }
        private bool provjeriPreostalo(Oprema opremaZaSlanje, Sala izabranaSala)
        {
            if (nadjiDozvoljenuKolicinu(opremaZaSlanje, izabranaSala) <= 0) { return false; }
            else { return true; }
        }
        private int nadjiDozvoljenuKolicinu(Oprema oprema, Sala sala)
        {
            int kolicina = oprema.Kolicina;
            foreach (Premjestaj pm in PremjestajServis.Premjestaji())
            {
                if (pm.izSale.Id == izabranaSala.Id && pm.oprema.IdOpreme == IzabranaStatickaDodavanje.IdOpreme)
                {
                    kolicina -= pm.kolicina;
                }
            }
            return kolicina;
        }
        private void promjenjenaSala()
        {
            if (IzabranaSalaZaDodavanje != null)
            {
                foreach (Sala sala in SaleServis.Sale())
                {
                    if (IzabranaSalaZaDodavanje.Id == sala.Id)
                    {
                        izracunajDozvoljenuKolicinu(sala, IzabranaSalaZaDodavanje);
                    }
                }
            }

        }
        private void izracunajDozvoljenuKolicinu(Sala sala, Sala izabranaSala)
        {
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == izabranaStatickaDodavanje.IdOpreme)
                {
                    SaleViewModel.dozvoljenaKolicinaDodavanjeStaticke = nadjiDozvoljenuKolicinuDodavanja(oprema, izabranaSala);
                    TekstDodavanjaStaticke = "MAX:" + SaleViewModel.dozvoljenaKolicinaDodavanjeStaticke;
                }
            }
        }
        private int nadjiDozvoljenuKolicinuDodavanja(Oprema oprema, Sala sala)
        {
            int kolicina = oprema.Kolicina;
            foreach (Premjestaj pm in PremjestajServis.Premjestaji())
            {
                if (pm.izSale.Id == sala.Id && pm.oprema.IdOpreme == oprema.IdOpreme)
                {
                    kolicina -= pm.kolicina;
                }
            }
            return kolicina;
        }

    }
}
