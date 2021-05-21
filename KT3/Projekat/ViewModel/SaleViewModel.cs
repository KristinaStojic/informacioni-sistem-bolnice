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
    public class SaleViewModel : BindableBase
    {
        public Window DodajSaluProzor { get; set; }
        public Window IzmjeniSaluProzor { get; set; }
        public Window RenoviranjeProzor { get; set; }
        public Window SpajanjeSalaProzor { get; set; }
        public Window PodijeliSaluProzor { get; set; }
        public Window DefinisiPodjeluProzor { get; set; }
        public Window BrisanjeSaleProzor { get; set; }
        public Window PrikazDinamickeProzor { get; set; }
        public Window PremjestanjeDinamickeProzor { get; set; }

        public static Sala novaSala;
        public static Sala salaZaSpajanje;
        public static bool spajanje;
        public static List<Oprema> opremaZaPrebacivanje;

        private ObservableCollection<Sala> sale;
        private ObservableCollection<Sala> saleSpajanje;
        private ObservableCollection<string> terminiPocetak;
        private ObservableCollection<string> terminiKraj;
        private ObservableCollection<Oprema> opremaStaraSala;
        private ObservableCollection<Oprema> opremaNovaSala;
        private ObservableCollection<Oprema> opremaDinamicka;
        private string tekstRenoviranja;
        public string TekstRenoviranja
        {
            get { return tekstRenoviranja; }
            set
            {
                tekstRenoviranja = value;
                OnPropertyChanged("TekstRenoviranja");
            }
        }
        public ObservableCollection<Oprema> OpremaStaraSala { get { return opremaStaraSala; } set { opremaStaraSala = value; OnPropertyChanged("OpremaStaraSala"); } }
        public ObservableCollection<Oprema> OpremaNovaSala { get { return opremaNovaSala; } set { opremaNovaSala = value; OnPropertyChanged("OpremaNovaSala"); } }
        public ObservableCollection<Oprema> OpremaDinamicka{ get { return opremaDinamicka; } set { opremaDinamicka = value; OnPropertyChanged("OpremaDinamicka"); } }
        public ObservableCollection<string> TerminiPocetak { get { return terminiPocetak; } set { terminiPocetak = value; OnPropertyChanged("TerminiPocetak"); } }
        public ObservableCollection<string> TerminiKraj { get { return terminiKraj; } set { terminiKraj = value; OnPropertyChanged("TerminiKraj"); } }
        
        public ObservableCollection<Sala> Sale { get { return sale; } set { sale = value; OnPropertyChanged("Sale"); } }
        public ObservableCollection<Sala> SaleSpajanje { get { return saleSpajanje; } set { saleSpajanje = value; OnPropertyChanged("SaleSpajanje"); } }
        public SaleViewModel()
        {
            ZatvoriPrikazSala = new MyICommand(ZatvoriSale);
            DodajSaluKomanda = new MyICommand(OtvoriDodavanjeSale);
            OdustaniOdDodavanjaSale = new MyICommand(ZatvoriDodavanjeSale);
            PotvrdiDodavanjeSale = new MyICommand(DodajNovuSalu, ValidnaPoljaZaDodavanjeSale);
            IzmjeniSaluKomanda = new MyICommand(OtvoriIzmjenuSale);
            OdustaniOdIzmjeneSale = new MyICommand(ZatvoriIzmjenuSale);
            PotvrdiIzmjenuSale = new MyICommand(IzmjeniIzabranuSalu, ValidnaPoljaZaIzmjenuSale);
            RenoviranjeKomanda = new MyICommand(OtvoriRenoviranje);
            OdustaniOdRenoviranja = new MyICommand(ZatvoriRenoviranje);
            PotvrdiRenoviranje = new MyICommand(IzvrsiRenoviranje, ValidnaPoljaZaRenoviranje);
            SpojiSaluKomanda = new MyICommand(SpojiSaleProzor);
            OdustaniOdSpajanjaSala = new MyICommand(ZatvoriSpajanje);
            PotvrdiSpajanjeSala = new MyICommand(SpojiSale);
            PodijeliSaluKomanda = new MyICommand(PodjelaSaleProzor);
            OdustaniOdPodjeleSale = new MyICommand(ZatvoriPodjelu);
            PotvrdiPodjeluSale = new MyICommand(NovaSala, ValidanUnosNoveSale);
            OdustaniOdPodjeleOpreme = new MyICommand(ZatvoriPodjeluSale);
            PotvrdiPodjeluOpreme = new MyICommand(PodijeliOpremu, ValidnaPoljaZaPodjelu);
            KrevetKomanda = new MyICommand(DodajKrevet);
            ObrisiSaluKomanda = new MyICommand(ObrisiSalu);
            PotvrdiBrisanjeSale = new MyICommand(ObrisiIzabranuSalu);
            OdustaniOdBrisanjaSale = new MyICommand(ZatvoriBrisanjeSale);
            PregledDinamickeKomanda = new MyICommand(PrikaziDinamicku);
            NapustiDinamicku = new MyICommand(ZatvoriDinamicku);
            PremjestiDinamickuKomanda = new MyICommand(OtvoriPremjestanjeDinamicke);
            OdustaniOdSlanjaDinamicke = new MyICommand(ZatvoriSlanjeDinamicke);
            PotvrdiSlanjeDinamicke = new MyICommand(PosaljiDinamicku, ValidnaPoljaZaSlanjeDinamicke);
            DodavanjeDinamickeKomanda = new MyICommand(OtvoriDodavanjeDinamicke);
            DodajSale();
        }
        private void DodajSale()
        {
            Sale = new ObservableCollection<Sala>();
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (!sala.Namjena.Equals("Skladiste"))
                {
                    Sale.Add(sala);
                }
            }
        }

        #region ZatvoriPrikazSalaViewModel
        public MyICommand ZatvoriPrikazSala { get; set; }
        private void ZatvoriSale()
        {
            //dio iz upravnik viewMODEL za zatvaranje...
        }
        #endregion
        #region DodajSaluViewModel
        private string brojSaleDodavanje;
        private string namjenaSaleDodavanje;
        private string tipSaleDodavanje;
        public string BrojSaleDodavanje { get { return brojSaleDodavanje; } set { brojSaleDodavanje = value; OnPropertyChanged("BrojSaleDodavanje"); PotvrdiDodavanjeSale.RaiseCanExecuteChanged(); } }
        public string NamjenaSaleDodavanje { get { return namjenaSaleDodavanje; } set { namjenaSaleDodavanje = value; OnPropertyChanged("NamjenaSaleDodavanje"); PotvrdiDodavanjeSale.RaiseCanExecuteChanged(); } }
        public string TipSaleDodavanje { get { return tipSaleDodavanje; } set { tipSaleDodavanje = value; OnPropertyChanged("TipSaleDodavanje"); PotvrdiDodavanjeSale.RaiseCanExecuteChanged(); } }
        public MyICommand DodajSaluKomanda { get; set; }
        public MyICommand OdustaniOdDodavanjaSale { get; set; }
        public MyICommand PotvrdiDodavanjeSale { get; set; }

        private void OtvoriDodavanjeSale()
        {
            DodajSaluProzor = new DodajSalu();
            DodajSaluProzor.Show();
            brojSaleDodavanje = "";
            namjenaSaleDodavanje = "";
            DodajSaluProzor.DataContext = this;
        }

        private void DodajNovuSalu()
        {
            Sala sala = new Sala(SaleServis.GenerisanjeIdSale(), int.Parse(brojSaleDodavanje), namjenaSaleDodavanje, nadjiTipSale());
            sala.Oprema = new List<Oprema>();
            SaleServis.DodajSalu(sala);
            Sale.Add(sala);
            DodajSaluProzor.Close();
        }

        private tipSale nadjiTipSale()
        {
            Console.WriteLine(tipSaleDodavanje);
            if (tipSaleDodavanje.Contains("Sala za preglede"))
            {
                return tipSale.SalaZaPregled;
            }
            else if (tipSaleDodavanje.Contains("Sala za operacije"))
            {
                return tipSale.OperacionaSala;
            }
            else
            {
                return tipSale.SalaZaLezanje;
            }
        }

        private bool ValidnaPoljaZaDodavanjeSale()
        {
            if (brojSaleDodavanje != null && namjenaSaleDodavanje != null && tipSaleDodavanje != null)
            {
                if (brojSaleDodavanje.Trim().Equals("") || namjenaSaleDodavanje.Trim().Equals("") || !jeBroj(brojSaleDodavanje) || jeBroj(namjenaSaleDodavanje) || postojiBrojSale())
                {
                    return false;
                }
                else if (!brojSaleDodavanje.Trim().Equals("") && !namjenaSaleDodavanje.Trim().Equals("") && jeBroj(brojSaleDodavanje) && !jeBroj(namjenaSaleDodavanje) && !postojiBrojSale())
                {
                    return true;
                }
            }
            return false;
        }

        public bool jeBroj(string tekst)
        {
            int test;
            return int.TryParse(tekst, out test);
        }

        private bool postojiBrojSale()
        {
            if (jeBroj(brojSaleDodavanje))
            {
                foreach (Sala sala in SaleServis.Sale())
                {
                    if (sala.brojSale == int.Parse(brojSaleDodavanje))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void ZatvoriDodavanjeSale()
        {
            DodajSaluProzor.Close();
        }

        #endregion
        #region IzmjeniSaluViewModel
        public MyICommand IzmjeniSaluKomanda { get; set; }
        public MyICommand OdustaniOdIzmjeneSale { get; set; }
        public MyICommand PotvrdiIzmjenuSale { get; set; }

        private string brojSaleIzmjena;
        private string namjenaSaleIzmjena;
        private int tipSaleIzmjena;
        public string BrojSaleIzmjena { get { return brojSaleIzmjena; } set { brojSaleIzmjena = value; OnPropertyChanged("BrojSaleIzmjena"); PotvrdiIzmjenuSale.RaiseCanExecuteChanged(); } }
        public string NamjenaSaleIzmjena { get { return namjenaSaleIzmjena; } set { namjenaSaleIzmjena = value; OnPropertyChanged("NamjenaSaleIzmjena"); PotvrdiIzmjenuSale.RaiseCanExecuteChanged(); } }
        public int TipSaleIzmjena { get { return tipSaleIzmjena; } set { tipSaleIzmjena = value; OnPropertyChanged("TipSaleIzmjena"); PotvrdiIzmjenuSale.RaiseCanExecuteChanged(); } }

        private Sala izabranaSala;
        public Sala IzabranaSala
        {
            get { return izabranaSala; }
            set { izabranaSala = value; OnPropertyChanged("IzabranaSala"); }
        }
        private void OtvoriIzmjenuSale()
        {
            if (izabranaSala != null)
            {
                IzmjeniSaluProzor = new IzmjeniSalu();
                IzmjeniSaluProzor.Show();
                brojSaleIzmjena = izabranaSala.brojSale.ToString();
                namjenaSaleIzmjena = izabranaSala.Namjena;
                postaviTipSale();
                IzmjeniSaluProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
        }

        private void IzmjeniIzabranuSalu(){
            Sala sala = new Sala(izabranaSala.Id, int.Parse(brojSaleIzmjena), namjenaSaleIzmjena, nadjiTipIzmjenjeneSale());
            SaleServis.IzmjeniSalu(izabranaSala, sala);
            int idx = Sale.IndexOf(izabranaSala);
            Sale.RemoveAt(idx);
            Sale.Insert(idx, sala);
            IzmjeniSaluProzor.Close();
        }

        private tipSale nadjiTipIzmjenjeneSale()
        {
            if (tipSaleIzmjena == 1)
            {
                return tipSale.SalaZaPregled;
            }
            else if (tipSaleIzmjena == 0)
            {
                return tipSale.OperacionaSala;
            }
            else
            {
                return tipSale.SalaZaLezanje;
            }
        }

        private void postaviTipSale()
        {
            if (izabranaSala.TipSale.Equals(tipSale.SalaZaPregled))
            {
                tipSaleIzmjena = 1;
            }
            else if (izabranaSala.TipSale.Equals(tipSale.OperacionaSala))
            {
                tipSaleIzmjena = 0;
            }
            else
            {
                tipSaleIzmjena = 2;
            }
        }

        private bool ValidnaPoljaZaIzmjenuSale()
        {
            if (brojSaleIzmjena != null && namjenaSaleIzmjena != null && tipSaleIzmjena != 0)
            {
                if (brojSaleIzmjena.Trim().Equals("") || namjenaSaleIzmjena.Trim().Equals("") || !jeBroj(brojSaleIzmjena) || jeBroj(namjenaSaleIzmjena) || postojiBrojNoveSale())
                {
                    return false;
                }
                else if (!brojSaleIzmjena.Trim().Equals("") && !namjenaSaleIzmjena.Trim().Equals("") && jeBroj(brojSaleIzmjena) && !jeBroj(namjenaSaleIzmjena) && !postojiBrojNoveSale())
                {
                    return true;
                }
            }
            return false;
        }

        private bool postojiBrojNoveSale()
        {
            if (jeBroj(brojSaleIzmjena))
            {
                foreach (Sala sala in SaleServis.Sale())
                {
                    if (sala.brojSale == int.Parse(brojSaleIzmjena) && sala.Id != this.izabranaSala.Id)
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        private void ZatvoriIzmjenuSale()
        {
            IzmjeniSaluProzor.Close();
        }

        #endregion
        #region RenoviranjeSaleViewModel
        public MyICommand RenoviranjeKomanda { get; set; }
        public MyICommand OdustaniOdRenoviranja { get; set; }
        public MyICommand PotvrdiRenoviranje { get; set; }
        public MyICommand OdustaniOdSpajanjaSala { get; set; }
        public MyICommand SpojiSaluKomanda { get; set; }
        public MyICommand PotvrdiSpajanjeSala { get; set; }
        public MyICommand PodijeliSaluKomanda { get; set; }

        private DateTime datumPocetka;
        private DateTime datumKraja;
        private DateTime pocetakKraja;
        private string vrijemePocetka;
        private string vrijemeKraja;
        private Sala izabranaSalaZaSpajanje;
        public Sala IzabranaSalaZaSpajanje { get { return izabranaSalaZaSpajanje; } set { izabranaSalaZaSpajanje = value; OnPropertyChanged("IzabranaSalaZaSpajanje"); } }
        public string VrijemePocetka { get { return vrijemePocetka; } set { vrijemePocetka = value; OnPropertyChanged("VrijemePocetka"); PromijenjenoVrijemePocetka(); PotvrdiRenoviranje.RaiseCanExecuteChanged(); } }
        public string VrijemeKraja { get { return vrijemeKraja; } set { vrijemeKraja = value; OnPropertyChanged("VrijemeKraja"); PotvrdiRenoviranje.RaiseCanExecuteChanged(); } }
        public DateTime DatumPocetka { get { return datumPocetka; } set { datumPocetka = value; OnPropertyChanged("DatumPocetka"); PromjenjenDatumPocetka(); PotvrdiRenoviranje.RaiseCanExecuteChanged(); } }
        public DateTime DatumKraja { get { return datumKraja; } set { datumKraja = value; OnPropertyChanged("DatumKraja"); PromjenjenDatumKraja(); PotvrdiRenoviranje.RaiseCanExecuteChanged(); } }
        public DateTime PocetakKraja{ get { return pocetakKraja; } set { pocetakKraja = value; OnPropertyChanged("PocetakKraja"); } }

        private void OtvoriRenoviranje()
        {
            if (izabranaSala != null && SaleServis.salaZakazanaZaRenoviranje(izabranaSala)) 
            {
                MessageBox.Show("Izabrana sala je vec zakazana za renoviranje!");
            }
            else if (izabranaSala != null)
            {
                RenoviranjeProzor = new Renoviranje();
                RenoviranjeProzor.Show();
                TerminiPocetak = new ObservableCollection<string>();
                TerminiKraj = new ObservableCollection<string>();
                datumPocetka = DateTime.Now.Date;
                datumKraja = DateTime.Now.Date;
                pocetakKraja = DateTime.Now.Date;
                TekstRenoviranja = "";
                postaviTerminePocetka();
                postaviTermineKraja();
                RenoviranjeProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
        }

        private void PodjelaSaleProzor()
        {
            PodijeliSaluProzor = new NovaSala();
            PodijeliSaluProzor.Show();
            NazivNoveSale = "";
            BrojNoveSale = "";
            PodijeliSaluProzor.DataContext = this;
        }

        private bool ValidnaPoljaZaRenoviranje()
        {
            if (VrijemeKraja != null && VrijemePocetka != null && DatumPocetka != null && DatumKraja != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SpojiSale()
        {
            if (izabranaSalaZaSpajanje != null)
            {
                TekstRenoviranja = "Spajanje sa salom: " + izabranaSalaZaSpajanje.Namjena + ", br. " + izabranaSalaZaSpajanje.brojSale;
                salaZaSpajanje = izabranaSalaZaSpajanje;
                opremaZaPrebacivanje = null;
                SpajanjeSalaProzor.Close();
            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
        }

        private void ZatvoriSpajanje()
        {
            SpajanjeSalaProzor.Close();
        }

        private void SpojiSaleProzor()
        {
            SpajanjeSalaProzor = new SpajanjeSala();
            SpajanjeSalaProzor.Show();
            dodajSaleSpajanje();
            SpajanjeSalaProzor.DataContext = this;
        }

        private void dodajSaleSpajanje()
        {
            SaleSpajanje = new ObservableCollection<Sala>();
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (!sala.Namjena.Equals("Skladiste") && sala.Id != izabranaSala.Id && sala.TipSale.Equals(izabranaSala.TipSale))
                {
                    SaleSpajanje.Add(sala);
                }
            }   
        }
        private void spojiSale()
        {
            OpremaServis.dodajOpremuIzSaleZaDodavanje(izabranaSala, izabranaSalaZaSpajanje);
            SaleServis.ObrisiSalu(IzabranaSalaZaSpajanje);
            Sale.Remove(IzabranaSalaZaSpajanje);
            SaleServis.sacuvajIzmjene();
        }
        private void IzvrsiRenoviranje()
        {
            if (salaZaSpajanje != null)
            {
                spojiSale();
                ZauzeceSale zauzeceSale = napraviZauzece();
                SaleServis.zauzmiSalu(zauzeceSale, izabranaSala);
                SaleServis.zauzmiSalu(zauzeceSale, salaZaSpajanje);
                SaleServis.sacuvajIzmjene();
            }
            else if (opremaZaPrebacivanje != null)
            {
                ZauzeceSale zauzeceSale = napraviZauzece();
                SaleServis.prebaciOpremuIzStareSale(izabranaSala, opremaZaPrebacivanje);
                napraviNovuSalu();
                SaleServis.zauzmiSalu(zauzeceSale, izabranaSala);
                SaleServis.zauzmiSalu(zauzeceSale, novaSala);
                SaleServis.sacuvajIzmjene();
            }
            else
            {
                ZauzeceSale zauzeceSale = napraviZauzece();
                SaleServis.zauzmiSalu(zauzeceSale, izabranaSala);
                SaleServis.sacuvajIzmjene();
            }
            RenoviranjeProzor.Close();
        }

        private void napraviNovuSalu()
        {
            novaSala.Oprema = opremaZaPrebacivanje;
            SaleServis.DodajSalu(novaSala);
            Sale.Add(novaSala);
        }

        private void postaviTermineKraja()
        {
            string datumKrajaRenoviranja = (DatumKraja.Date).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (TerminiKraj != null)
            {
                TerminiKraj.Clear();
                for (int termin = (int)DateTime.Now.Hour + 1; termin <= 23; termin++)
                {
                    dodajTerminKraja(datumKrajaRenoviranja, termin);
                }
            }
        }
        private void postaviTerminePocetka()
        {
            if (DatumPocetka.Date == DateTime.Now.Date)
            {
                azurirajVrijemePocetkaIstiDatum();
            }
            else
            {
                azurirajVrijemePocetkaDrugiDatum();
            }
        }

        

        private ZauzeceSale napraviZauzece()
        {
            string datumPocetka = (DatumPocetka).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string datumKraja = (DatumKraja).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            return new ZauzeceSale(VrijemePocetka, VrijemeKraja, datumPocetka, datumKraja, 0);
        }

        private void azurirajVrijemePocetkaDrugiDatum()
        {
            string datumPocetkaRenoviranja = (DatumPocetka.Date).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (TerminiPocetak != null)
            {
                TerminiPocetak.Clear();
                for (int termin = 1; termin <= 23; termin++)
                {
                    dodajTerminPocetka(datumPocetkaRenoviranja, termin);
                }
            }
        }

        private void azurirajVrijemePocetkaIstiDatum()
        {
            string datumPocetkaRenoviranja = DatumPocetka.Date.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (TerminiPocetak != null)
            {
                TerminiPocetak.Clear();
                for (int termin = (int)DateTime.Now.Hour + 1; termin <= 23; termin++)
                {
                    dodajTerminPocetka(datumPocetkaRenoviranja, termin);
                }
            }

        }

        private void dodajTerminPocetka(string datumPocetka, int termin)
        {
            if (!zauzecaZaDatum(datumPocetka).Contains(termin))
            {
                TerminiPocetak.Add(termin + ":00");
            }
        }

        private List<int> zauzecaZaDatum(string datum)
        {
            List<int> zauzeca = new List<int>();
            foreach (ZauzeceSale zauzece in izabranaSala.zauzetiTermini)
            {
                if (zauzece.datumPocetkaTermina == datum)
                {
                    for (int i = int.Parse(zauzece.pocetakTermina.Split(':')[0]); i <= int.Parse(zauzece.krajTermina.Split(':')[0]); i++)
                    {
                        zauzeca.Add(i);
                    }
                }
            }
            return zauzeca;
        }

        private void PromjenjenDatumPocetka() {
            azurirajDatumKraja();
            postaviTerminePocetka();
        }
        private void azurirajVrijemeKraja()
        {
            if (DatumKraja.Date == DatumPocetka.Date)
            {
                azurirajVrijemeKrajaIstiDatum();
            }
            else
            {
                azurirajVrijemeKrajaDrugiDatum();
            }
        }
        private void azurirajVrijemeKrajaDrugiDatum()
        {
            string datumPocetkaRenoviranja = (DatumPocetka.Date).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string datumKrajaRenoviranja = (DatumKraja.Date).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            if (TerminiKraj != null)
            {
                TerminiKraj.Clear();
                if (VrijemePocetka != null)
                {
                    if (prvoSledeceZauzece(VrijemePocetka.Split(':')[0], datumPocetkaRenoviranja) == 24 && slobodniTerminiIzmedju(datumPocetkaRenoviranja, datumKrajaRenoviranja))//Termini i pregledi ne traju vise dana...
                    {
                        for (int termin = 1; termin <= prvoSledeceZauzece("1", datumKrajaRenoviranja); termin++)
                        {
                            dodajTerminKraja(datumPocetkaRenoviranja, termin);
                        }
                    }
                }
            }
        }

        private bool slobodniTerminiIzmedju(string datumPocetka, string datumKraja)
        {
            DateTime? pocetakRenoviranja = (DateTime?)DateTime.Parse(datumPocetka);
            DateTime? krajRenoviranja = (DateTime?)DateTime.Parse(datumKraja);
            for (var datum = pocetakRenoviranja; datum < krajRenoviranja; datum = datum.Value.AddDays(1))
            {
                string datumTrenutni = ((DateTime)datum).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if (zauzecaZaDatum(datumTrenutni).Count != 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void azurirajVrijemeKrajaIstiDatum()
        {
            string datumPocetkaRenoviranja = (datumPocetka).ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (TerminiKraj != null && VrijemePocetka != null)
            {
                TerminiKraj.Clear();
                for (int termin = int.Parse(VrijemePocetka.Split(':')[0]) + 1; termin <= prvoSledeceZauzece(VrijemePocetka.Split(':')[0], datumPocetkaRenoviranja); termin++)
                {
                    dodajTerminKraja(datumPocetkaRenoviranja, termin);
                }
            }
        }

        private void dodajTerminKraja(string datumPocetka, int termin)
        {
            if (!zauzecaZaDatum(datumPocetka).Contains(termin))
            {
                TerminiKraj.Add(termin + ":00");
            }
        }

        private int prvoSledeceZauzece(string vrijeme, string datum)
        {
            foreach (int zauzetiTermin in zauzecaZaDatum(datum))
            {
                if (zauzetiTermin > int.Parse(vrijeme))
                {
                    return zauzetiTermin;
                }
            }
            return 24;
        }

        private void azurirajDatumKraja()
        {
            if (DatumKraja != null && DatumPocetka != null)
            {
                PocetakKraja = datumPocetka.Date;
                DatumKraja = datumPocetka.Date;
            }
        }

        private void PromjenjenDatumKraja() {
            azurirajVrijemeKraja();
        }

        private void ZatvoriRenoviranje()
        {
            RenoviranjeProzor.Close();
        }

        private void PromijenjenoVrijemePocetka()
        {
            azurirajVrijemeKraja();
        }
        #endregion
        #region PodjelaSaleViewModel
        private string nazivNoveSale;
        private string brojNoveSale;
        public string NazivNoveSale { get { return nazivNoveSale; } set { nazivNoveSale = value; OnPropertyChanged("NazivNoveSale"); PotvrdiPodjeluSale.RaiseCanExecuteChanged(); } }
        public string BrojNoveSale { get { return brojNoveSale; } set { brojNoveSale = value; OnPropertyChanged("BrojNoveSale"); PotvrdiPodjeluSale.RaiseCanExecuteChanged(); } }
        public MyICommand OdustaniOdPodjeleSale { get; set; }
        public MyICommand PotvrdiPodjeluSale { get; set; }
        private void ZatvoriPodjelu()
        {
            PodijeliSaluProzor.Close();
        }

        private bool ValidanUnosNoveSale()
        {
            if (BrojNoveSale != null && NazivNoveSale != null)
            {
                if (brojNoveSale.Trim().Equals("") || nazivNoveSale.Trim().Equals("") || !jeBroj(brojNoveSale) || jeBroj(nazivNoveSale) || postojiBrojPodjeljeneSale())
                {
                    return false;
                }
                else if (!brojNoveSale.Trim().Equals("") && !nazivNoveSale.Trim().Equals("") && jeBroj(brojNoveSale) && !jeBroj(nazivNoveSale) && !postojiBrojPodjeljeneSale())
                {
                    return true;
                }
            }
            return false;
        }

        private bool postojiBrojPodjeljeneSale()
        {

            if (jeBroj(BrojNoveSale))
            {
                foreach (Sala sala in SaleMenadzer.sale)
                {
                    if (sala.brojSale == int.Parse(BrojNoveSale))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void NovaSala()
        {
            novaSala = new Sala();
            novaSala.TipSale = izabranaSala.TipSale;
            novaSala.Namjena = NazivNoveSale;
            novaSala.brojSale = int.Parse(BrojNoveSale);
            novaSala.Oprema = new List<Oprema>();
            novaSala.zauzetiTermini = new List<ZauzeceSale>();
            otvoriPremjestajOpreme();
        }

        private void otvoriPremjestajOpreme()
        {
            //PodjelaSale()...
            DefinisiPodjeluProzor = new PodjelaSale(izabranaSala, novaSala);
            DefinisiPodjeluProzor.Show();
            StaraSalaString = "Sala " + izabranaSala.Namjena + ", br. " + izabranaSala.brojSale.ToString();
            NovaSalaString = "Sala " + novaSala.Namjena + ", br. " + novaSala.brojSale.ToString();
            dodajOpremuPodjela();
            opremaZaPrebacivanje = new List<Oprema>();
            DefinisiPodjeluProzor.DataContext = this;
            PodijeliSaluProzor.Close();
        }

        #endregion
        #region PodjelaSaleViewModel
        public MyICommand OdustaniOdPodjeleOpreme { get; set; }
        public MyICommand PotvrdiPodjeluOpreme { get; set; }

        private string staraSalaString;
        private Oprema opremaZaPodjelu;
        public Oprema OpremaZaPodjelu { get { return opremaZaPodjelu; } set { opremaZaPodjelu = value; OnPropertyChanged("OpremaZaPodjelu");  } }
        private string unijetaKolicina;
        public string UnijetaKolicina { get { return unijetaKolicina; } set { unijetaKolicina = value; OnPropertyChanged("UnijetaKolicina"); } }

        private string novaSalaString;
        public string StaraSalaString { get { return staraSalaString; } set { staraSalaString = value; OnPropertyChanged("StaraSalaString"); } }
        public string NovaSalaString { get { return novaSalaString; } set { novaSalaString = value; OnPropertyChanged("NovaSalaString"); } }
        private bool ValidnaPoljaZaPodjelu()
        {
            if (OpremaNovaSala != null)
            {
                foreach (Oprema oprema in OpremaStaraSala)
                {
                    foreach (Oprema opremaSlanje in opremaZaPrebacivanje)
                    {
                        if (oprema.IdOpreme == opremaSlanje.IdOpreme)
                        {
                            if (oprema.Kolicina < opremaSlanje.Kolicina)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        private void dodajOpremuPodjela()
        {
            OpremaStaraSala = new ObservableCollection<Oprema>();
            OpremaNovaSala = new ObservableCollection<Oprema>();
            foreach (Oprema oprema in izabranaSala.Oprema)
            {
                OpremaStaraSala.Add(oprema);
                Oprema nova = new Oprema(oprema.NazivOpreme, 0, oprema.Staticka);
                nova.IdOpreme = oprema.IdOpreme;
                OpremaNovaSala.Add(nova);
            }
        }

        private void ZatvoriPodjeluSale()
        {
            DefinisiPodjeluProzor.Close();
        }

        private void PodijeliOpremu()
        {
            foreach (Oprema oprema in OpremaNovaSala)
            {
                if (oprema.Kolicina != 0)
                {
                    opremaZaPrebacivanje.Add(oprema);
                }
            }
            if (!ValidnaPoljaZaPodjelu())
            {
                MessageBox.Show("Morate unijeti ispravne kolicine!");
            }
            else
            {
                TekstRenoviranja = "Podjela sale na 2";
                salaZaSpajanje = null;
                DefinisiPodjeluProzor.Close();
            }
        }


        #endregion
        #region KrevetViewModel
        public MyICommand KrevetKomanda { get; set; }
        private void DodajKrevet()
        {
            if (izabranaSala != null)
            {
                Krevet krevet = new Krevet(izabranaSala.Id, false);
                izabranaSala.Kreveti.Add(krevet);
                SaleServis.sacuvajIzmjene();

            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
        }
        #endregion
        #region ObrisiSaluViewModel
        public MyICommand ObrisiSaluKomanda { get; set; }
        public MyICommand PotvrdiBrisanjeSale { get; set; }
        public MyICommand OdustaniOdBrisanjaSale { get; set; }
        private void ObrisiSalu()
        {
            if (izabranaSala != null)
            {
                BrisanjeSaleProzor = new BrisanjeSale();
                BrisanjeSaleProzor.Show();
                BrisanjeSaleProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
        }
        private void ObrisiIzabranuSalu()
        {
            SaleServis.ObrisiSalu(izabranaSala);
            Sale.Remove(izabranaSala);
            BrisanjeSaleProzor.Close();
        }

        private void ZatvoriBrisanjeSale()
        {
            BrisanjeSaleProzor.Close();
        }
        #endregion
        #region PretragaSalaViewModel
        private string pretragaSala;
        public string PretragaSala { get { return pretragaSala; } set { pretragaSala = value;OnPropertyChanged("PretragaSala"); promjenjenTekstPretrage(); } }
        private void promjenjenTekstPretrage()
        {
            Sale.Clear();
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (sala.Namjena.Equals("Skladiste"))
                {
                    continue;
                }
                if (sala.Namjena.StartsWith(pretragaSala))
                {
                    Sale.Add(sala);
                }
            }
        }
        #endregion
        #region DinamickaOpremaViewModel
        public MyICommand PregledDinamickeKomanda { get; set; }
        public MyICommand NapustiDinamicku { get; set; }
        private string tekstDinamicke;
        private string pretragaDinamicke;
        private string tekstDinamicka;
        public string PretragaDinamicke { get { return pretragaDinamicke; } set { pretragaDinamicke = value; OnPropertyChanged("PretragaDinamicke"); PromijenjenTekstDinamicke(); } }
        public string TekstDinamicka { get { return tekstDinamicka; } set { tekstDinamicka = value; OnPropertyChanged("TekstDinamicka"); } }
        private void PrikaziDinamicku()
        {
            if (izabranaSala != null)
            {
                prikaziDinamicku(izabranaSala);
            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
        }

        private void prikaziDinamicku(Sala izabranaSala)
        {
            try
            {
                PrikazDinamickeProzor = new PrikazDinamicke(izabranaSala);
                PrikazDinamickeProzor.Show();
                postaviTekst();
                dodajDinamickuOpremu();
                pretragaDinamicke = "";
                PrikazDinamickeProzor.DataContext = this;
            }
            catch (Exception ex) { Console.WriteLine(ex.Data); }
        }

        private void dodajDinamickuOpremu()
        {
            OpremaDinamicka = new ObservableCollection<Oprema>();
            if (izabranaSala.Oprema != null)
            {
                foreach (Oprema oprema in izabranaSala.Oprema)
                {
                    if (!oprema.Staticka)
                    {
                        OpremaDinamicka.Add(oprema);
                    }
                }
            }
        }

        private void PromijenjenTekstDinamicke()
        {
            OpremaDinamicka.Clear();
            foreach (Oprema oprema in izabranaSala.Oprema)
            {
                if (oprema.NazivOpreme.StartsWith(pretragaDinamicke) && !oprema.Staticka)
                {
                    OpremaDinamicka.Add(oprema);
                }
            }
        }


        private void ZatvoriDinamicku()
        {
            PrikazDinamickeProzor.Close();
        }

        private void postaviTekst()
        {
            if (izabranaSala != null)
            {
                if (izabranaSala.TipSale == tipSale.SalaZaPregled)
                {
                    tekstDinamicka = "Sala za pregled (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
                else if (izabranaSala.TipSale == tipSale.OperacionaSala)
                {
                    tekstDinamicka = "Operaciona sala (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
                else
                {
                    tekstDinamicka = "Sala za lezanje (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
            }
        }
        #endregion
        #region PremjestiDinamickuViewModel
        private Oprema izabranaDinamicka;
        private string maxDinamickeTekst;
        private string nazivDinamicke;
        private Sala izabranaSalaDinamicka;
        private string kolicinaSlanjeDinamicke;
        private string upozorenjeSlanjeDinamicke;
        public string UpozorenjeSlanjeDinamicke { get { return upozorenjeSlanjeDinamicke; } set { upozorenjeSlanjeDinamicke = value; OnPropertyChanged("UpozorenjeSlanjeDinamicke"); } }
        public string KolicinaSlanjeDinamicke { get { return kolicinaSlanjeDinamicke; } set { kolicinaSlanjeDinamicke = value; OnPropertyChanged("KolicinaSlanjeDinamicke"); PotvrdiSlanjeDinamicke.RaiseCanExecuteChanged(); } }
        public Sala IzabranaSalaDinamicka { get { return izabranaSalaDinamicka; } set { izabranaSalaDinamicka = value; OnPropertyChanged("IzabranaSalaDinamicka"); PotvrdiSlanjeDinamicke.RaiseCanExecuteChanged(); } }
        public string NazivDinamicke { get { return nazivDinamicke; } set { nazivDinamicke = value; OnPropertyChanged("NazivDinamicke"); } }
        public string MaxDinamickeTekst { get { return maxDinamickeTekst; } set { maxDinamickeTekst = value; OnPropertyChanged("MaxDinamickeTekst"); } }
        private ObservableCollection<Sala> saleZaSlanje;
        public ObservableCollection<Sala> SaleZaSlanje { get { return saleZaSlanje; } set { saleZaSlanje = value; OnPropertyChanged("SaleZaSlanje");} }
        public Oprema IzabranaDinamicka { get { return izabranaDinamicka; } set { izabranaDinamicka = value; OnPropertyChanged("IzabranaDinamicka"); } }
        public MyICommand PremjestiDinamickuKomanda { get; set; }
        public MyICommand OdustaniOdSlanjaDinamicke { get; set; }
        public MyICommand PotvrdiSlanjeDinamicke { get; set; }
        private void OtvoriPremjestanjeDinamicke()
        {
            if (izabranaDinamicka != null)
            {
                PremjestanjeDinamickeProzor = new SlanjeDinamicke();
                PremjestanjeDinamickeProzor.Show();
                saleZaSlanje = new ObservableCollection<Sala>();
                maxDinamickeTekst = "MAX: " + izabranaDinamicka.Kolicina.ToString() ;
                NazivDinamicke = izabranaDinamicka.NazivOpreme;
                dodajSaleZaSlanje();
                PremjestanjeDinamickeProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati opremu");
            }
        }
        private void dodajSaleZaSlanje()
        {
            foreach (Sala sala in SaleServis.Sale())
            {
                if (sala.Id != izabranaSala.Id)
                {
                    saleZaSlanje.Add(sala);
                }
            }
        }
        private void ZatvoriSlanjeDinamicke()
        {
            PremjestanjeDinamickeProzor.Close();
        }
        private void azurirajPrikazDinamicke()
        {
            OpremaDinamicka.Clear();
            if (izabranaSala.Oprema != null)
            {
                foreach (Oprema oprema in izabranaSala.Oprema)
                {
                    if (!oprema.Staticka)
                    {
                        OpremaDinamicka.Add(oprema);
                    }
                }
            }
        }
        private bool ValidnaPoljaZaSlanjeDinamicke()
        {
            if (kolicinaSlanjeDinamicke != null && izabranaSalaDinamicka != null)
            {
                if (jeBroj(kolicinaSlanjeDinamicke))
                {
                    if (int.Parse(kolicinaSlanjeDinamicke) > izabranaDinamicka.Kolicina || int.Parse(kolicinaSlanjeDinamicke) <= 0 || izabranaSalaDinamicka == null)
                    {
                        if(int.Parse(kolicinaSlanjeDinamicke) > izabranaDinamicka.Kolicina)
                        {
                            UpozorenjeSlanjeDinamicke = "Morate unijeti manju vrijednost";
                        }else if (int.Parse(kolicinaSlanjeDinamicke) <= 0)
                        {
                            UpozorenjeSlanjeDinamicke = "Morate unijeti vecu vrijednost";
                        }
                        return false;
                    }
                    else if (int.Parse(kolicinaSlanjeDinamicke) <= izabranaDinamicka.Kolicina && int.Parse(kolicinaSlanjeDinamicke) > 0 && izabranaSalaDinamicka != null)
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
        private void PosaljiDinamicku()
        {
            PremjestajServis.prebaciDinamickuOpremu(IzabranaSalaDinamicka, int.Parse(kolicinaSlanjeDinamicke), izabranaSala, IzabranaDinamicka);
            PremjestanjeDinamickeProzor.Close();
            azurirajPrikazDinamicke();
        }
        #endregion
        #region DodavanjeDinamickeViewModel
        public MyICommand DodavanjeDinamickeKomanda { get; set; }
        private void OtvoriDodavanjeDinamicke()
        {

        }
        #endregion
    }
}
