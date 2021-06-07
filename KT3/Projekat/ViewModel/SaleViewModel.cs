using Model;
using Projekat.Model;
using Projekat.Pomoc;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace Projekat.ViewModel
{
    public class SaleViewModel : BindableBase
    {
        #region PromjenljiveViewModel
        public Window DodajSaluProzor { get; set; }
        public Window IzmjeniSaluProzor { get; set; }
        public Window RenoviranjeProzor { get; set; }
        public Window SpajanjeSalaProzor { get; set; }
        public Window PodijeliSaluProzor { get; set; }
        public Window DefinisiPodjeluProzor { get; set; }
        public Window BrisanjeSaleProzor { get; set; }
        public Window PrikazDinamickeProzor { get; set; }
        public Window PremjestanjeDinamickeProzor { get; set; }
        public Window DodavanjeDinamickeProzor { get; set; }
        public Window PrikazStatickeProzor { get; set; }
        public Window SlanjeStatickeProzor { get; set; }
        public Window DodavanjeStatickeProzor { get; set; }
        public static Window SaleProzor { get; set; }
        public static Window PomocSaleProzor { get; set; }
        public MyICommand ZatvoriProzorKomanda { get; set; }
        public MyICommand ZatvoriSalu { get; set; }
        public MyICommand OtvoriZahtjeve { get; set; }
        public MyICommand PomocSale { get; set; }
        public MyICommand OtvoriKomunikaciju { get; set; }
        public MyICommand OAplikacijiKomanda { get; set; }
        public MyICommand DodajSaluKomanda { get; set; }
        public MyICommand OdustaniOdDodavanjaSale { get; set; }
        public MyICommand PotvrdiDodavanjeSale { get; set; }
        public MyICommand IzmjeniSaluKomanda { get; set; }
        public MyICommand OdustaniOdIzmjeneSale { get; set; }
        public MyICommand PotvrdiIzmjenuSale { get; set; }
        public MyICommand RenoviranjeKomanda { get; set; }
        public MyICommand OdustaniOdRenoviranja { get; set; }
        public MyICommand ObrisiSaluKomanda { get; set; }
        public MyICommand PotvrdiBrisanjeSale { get; set; }
        public MyICommand PremjestiDinamickuKomanda { get; set; }
        public MyICommand OdustaniOdSlanjaDinamicke { get; set; }
        public MyICommand PotvrdiSlanjeDinamicke { get; set; }
        public MyICommand OdustaniOdBrisanjaSale { get; set; }
        public MyICommand DodavanjeDinamickeKomanda { get; set; }
        public MyICommand PregledStatickeKomanda { get; set; }
        public MyICommand OdustaniOdDodavanjaDinamicke { get; set; }
        public MyICommand PotvrdiDodavanjeDinamicke { get; set; }
        public MyICommand PotvrdiRenoviranje { get; set; }
        public MyICommand OdustaniOdSpajanjaSala { get; set; }
        public MyICommand SpojiSaluKomanda { get; set; }
        public MyICommand PregledDinamickeKomanda { get; set; }
        public MyICommand NapustiDinamicku { get; set; }
        public MyICommand PotvrdiSpajanjeSala { get; set; }
        public MyICommand PodijeliSaluKomanda { get; set; }
        public MyICommand OdustaniOdPodjeleOpreme { get; set; }
        public MyICommand OtvoriIzvjestaj { get; set; }
        public MyICommand PotvrdiPodjeluOpreme { get; set; }
        public MyICommand KrevetKomanda { get; set; }
        public MyICommand PrebaciStatickuKomanda { get; set; }
        public MyICommand OdustaniOdSlanjaStaticke { get; set; }
        public MyICommand PotvrdiSlanjeStaticke { get; set; }
        public MyICommand ZatvoriStaticku { get; set; }
        public MyICommand ZatvoriStatickuPrikaz { get; set; }
        public MyICommand DodajStatickuKomanda { get; set; }
        public MyICommand OdustaniOdDodavanjaStaticke { get; set; }
        public MyICommand DodajStaticku { get; set; }
        public MyICommand PotvrdiPodjeluSale { get; set; }
        public MyICommand ZatvoriPrikazSala { get; set; }
        public MyICommand OdustaniOdPodjeleSale { get; set; }

        public static Sala novaSala;
        
        public static Sala salaZaSpajanje;
        
        public static bool spajanje;
        
        public static List<Oprema> opremaZaPrebacivanje;
        
        public static bool aktivnaDinamicka;
        
        public static bool aktivnaStaticka;
        
        private string brojSaleDodavanje;
        
        private string namjenaSaleDodavanje;
        
        private string tipSaleDodavanje;

        private string tekstRenoviranja;

        private string brojSaleIzmjena;

        private string namjenaSaleIzmjena;

        private int tipSaleIzmjena;
        public string BrojSaleIzmjena { get { return brojSaleIzmjena; } set { brojSaleIzmjena = value; OnPropertyChanged("BrojSaleIzmjena"); PotvrdiIzmjenuSale.RaiseCanExecuteChanged(); } }
        public string NamjenaSaleIzmjena { get { return namjenaSaleIzmjena; } set { namjenaSaleIzmjena = value; OnPropertyChanged("NamjenaSaleIzmjena"); PotvrdiIzmjenuSale.RaiseCanExecuteChanged(); } }
        public int TipSaleIzmjena { get { return tipSaleIzmjena; } set { tipSaleIzmjena = value; OnPropertyChanged("TipSaleIzmjena"); PotvrdiIzmjenuSale.RaiseCanExecuteChanged(); } }

        private DateTime datumPocetka;

        private DateTime datumKraja;

        private DateTime pocetakKraja;

        private string vrijemePocetka;

        private string vrijemeKraja;

        private string maxDinamickaText;

        private Oprema izabranaDinamickaDodavanje;

        private Sala izabranaSalaDodavanje;

        public static int dozvoljenaKolicinaDinamicka;

        public string kolicinaDodavanjeDinamicke;

        public static Oprema opremaZaDodavanje;
        public string KolicinaDodavanjeDinamicke { get { return kolicinaDodavanjeDinamicke; } set { kolicinaDodavanjeDinamicke = value; OnPropertyChanged("KolicinaDodavanjeDinamicke"); PotvrdiDodavanjeDinamicke.RaiseCanExecuteChanged(); } }
        public Oprema IzabranaDinamickaDodavanje { get { return izabranaDinamickaDodavanje; } set { izabranaDinamickaDodavanje = value; OnPropertyChanged("IzabranaDinamickaDodavanje"); promjenjenaOpremaDinamicka(); PotvrdiDodavanjeDinamicke.RaiseCanExecuteChanged(); } }
        public Sala IzabranaSalaDodavanje { get { return izabranaSalaDodavanje; } set { izabranaSalaDodavanje = value; OnPropertyChanged("IzabranaSalaDodavanje"); promjenjenaSalaDinamicka(); PotvrdiDodavanjeDinamicke.RaiseCanExecuteChanged(); } }
        public string MaxDinamickaText { get { return maxDinamickaText; } set { maxDinamickaText = value; OnPropertyChanged("MaxDinamickaText"); } }

        private Sala izabranaSalaZaSpajanje;
        public Sala IzabranaSalaZaSpajanje { get { return izabranaSalaZaSpajanje; } set { izabranaSalaZaSpajanje = value; OnPropertyChanged("IzabranaSalaZaSpajanje"); } }
        public string VrijemePocetka { get { return vrijemePocetka; } set { vrijemePocetka = value; OnPropertyChanged("VrijemePocetka"); PromijenjenoVrijemePocetka(); PotvrdiRenoviranje.RaiseCanExecuteChanged(); } }
        public string VrijemeKraja { get { return vrijemeKraja; } set { vrijemeKraja = value; OnPropertyChanged("VrijemeKraja"); PotvrdiRenoviranje.RaiseCanExecuteChanged(); } }
        public DateTime DatumPocetka { get { return datumPocetka; } set { datumPocetka = value; OnPropertyChanged("DatumPocetka"); PromjenjenDatumPocetka(); PotvrdiRenoviranje.RaiseCanExecuteChanged(); } }
        public DateTime DatumKraja { get { return datumKraja; } set { datumKraja = value; OnPropertyChanged("DatumKraja"); PromjenjenDatumKraja(); PotvrdiRenoviranje.RaiseCanExecuteChanged(); } }
        public DateTime PocetakKraja { get { return pocetakKraja; } set { pocetakKraja = value; OnPropertyChanged("PocetakKraja"); } }

        private string pretragaSala;
        public string PretragaSala { get { return pretragaSala; } set { pretragaSala = value; OnPropertyChanged("PretragaSala"); promjenjenTekstPretrage(); } }

        private string nazivNoveSale;

        private string brojNoveSale;
        public string NazivNoveSale { get { return nazivNoveSale; } set { nazivNoveSale = value; OnPropertyChanged("NazivNoveSale"); PotvrdiPodjeluSale.RaiseCanExecuteChanged(); } }
        public string BrojNoveSale { get { return brojNoveSale; } set { brojNoveSale = value; OnPropertyChanged("BrojNoveSale"); PotvrdiPodjeluSale.RaiseCanExecuteChanged(); } }
        
        private string staraSalaString;

        private Oprema opremaZaPodjelu;
        public Oprema OpremaZaPodjelu { get { return opremaZaPodjelu; } set { opremaZaPodjelu = value; OnPropertyChanged("OpremaZaPodjelu"); } }
        
        private string unijetaKolicina;
        public string UnijetaKolicina { get { return unijetaKolicina; } set { unijetaKolicina = value; OnPropertyChanged("UnijetaKolicina"); } }

        private string novaSalaString;
        public string StaraSalaString { get { return staraSalaString; } set { staraSalaString = value; OnPropertyChanged("StaraSalaString"); } }
        public string NovaSalaString { get { return novaSalaString; } set { novaSalaString = value; OnPropertyChanged("NovaSalaString"); } }
        
        private string pretragaDinamicke;
        
        private string tekstDinamicka;
        public string PretragaDinamicke { get { return pretragaDinamicke; } set { pretragaDinamicke = value; OnPropertyChanged("PretragaDinamicke"); PromijenjenTekstDinamicke(); } }
        public string TekstDinamicka { get { return tekstDinamicka; } set { tekstDinamicka = value; OnPropertyChanged("TekstDinamicka"); } }
        
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
        public Oprema IzabranaDinamicka { get { return izabranaDinamicka; } set { izabranaDinamicka = value; OnPropertyChanged("IzabranaDinamicka"); } }
        
        public static bool azuriraj;
        
        private string tekstStaticka;
        public string TekstStaticka { get { return tekstStaticka; } set { tekstStaticka = value; OnPropertyChanged("TekstStaticka"); } }
        
        public static bool otvoren;

        private string pretragaStaticke;
        public string PretragaStaticke { get { return pretragaStaticke; } set { pretragaStaticke = value; OnPropertyChanged("PretragaStaticke"); pretraziStaticku(); } }

        private Sala izabranaSala;
        public Sala IzabranaSala {get { return izabranaSala; }set { izabranaSala = value; OnPropertyChanged("IzabranaSala"); }}
        public string TekstRenoviranja { get { return tekstRenoviranja; } set { tekstRenoviranja = value; OnPropertyChanged("TekstRenoviranja"); } }
        public string BrojSaleDodavanje { get { return brojSaleDodavanje; } set { brojSaleDodavanje = value; OnPropertyChanged("BrojSaleDodavanje"); PotvrdiDodavanjeSale.RaiseCanExecuteChanged(); } }
        public string NamjenaSaleDodavanje { get { return namjenaSaleDodavanje; } set { namjenaSaleDodavanje = value; OnPropertyChanged("NamjenaSaleDodavanje"); PotvrdiDodavanjeSale.RaiseCanExecuteChanged(); } }
        public string TipSaleDodavanje { get { return tipSaleDodavanje; } set { tipSaleDodavanje = value; OnPropertyChanged("TipSaleDodavanje"); PotvrdiDodavanjeSale.RaiseCanExecuteChanged(); } }
        
        private string statickaZaSlanje;
        
        private string maxStaticka;

        private Oprema izabranaStatickaDodavanje;
        
        public static Oprema izabranaStat;
        
        private Sala izabranaSalaZaDodavanje;
        
        public static int dozvoljenaKolicinaDodavanjeStaticke;
        
        private string tekstDodavanjaStaticke;
        
        private DateTime datumPrebacivanja;
        
        private string kolicinaDodavanjaStaticke;
        
        private string vrijemeDodavanja;
        public string VrijemeDodavanja { get { return vrijemeDodavanja; } set { vrijemeDodavanja = value; OnPropertyChanged("VrijemeDodavanja"); DodajStaticku.RaiseCanExecuteChanged(); } }
        public string KolicinaDodavanjaStaticke { get { return kolicinaDodavanjaStaticke; } set { kolicinaDodavanjaStaticke = value; OnPropertyChanged("KolicinaDodavanjaStaticke"); DodajStaticku.RaiseCanExecuteChanged(); } }
        public DateTime DatumPrebacivanja { get { return datumPrebacivanja; } set { datumPrebacivanja = value; OnPropertyChanged("DatumPrebacivanja"); promijenjenDatum(); DodajStaticku.RaiseCanExecuteChanged(); } }
        public string TekstDodavanjaStaticke { get { return tekstDodavanjaStaticke; } set { tekstDodavanjaStaticke = value; OnPropertyChanged("TekstDodavanjaStaticke"); } }
        public Oprema IzabranaStatickaDodavanje { get { return izabranaStatickaDodavanje; } set { izabranaStatickaDodavanje = value; OnPropertyChanged("IzabranaStatickaDodavanje"); promjenjenaOprema(); izabranaStat = izabranaStatickaDodavanje; DodajStaticku.RaiseCanExecuteChanged(); } }
        public Sala IzabranaSalaZaDodavanje { get { return izabranaSalaZaDodavanje; } set { izabranaSalaZaDodavanje = value; OnPropertyChanged("IzabranaSalaZaDodavanje"); promjenjenaSala(); DodajStaticku.RaiseCanExecuteChanged(); } }

        private DateTime datumSlanjaStaticke;
        public DateTime DatumSlanjaStaticke { get { return datumSlanjaStaticke; } set { datumSlanjaStaticke = value; OnPropertyChanged("DatumSlanjaStaticke"); promijenjenDatumSlanja(); PotvrdiSlanjeStaticke.RaiseCanExecuteChanged(); } }
        public string StatickaZaSlanje { get { return statickaZaSlanje; } set { statickaZaSlanje = value; OnPropertyChanged("StatickaZaSlanje"); } }
        public string MaxStaticka { get { return maxStaticka; } set { maxStaticka = value; OnPropertyChanged("MaxStaticka"); } }
        
        private Oprema izabranaStaticka;
        public Oprema IzabranaStaticka { get { return izabranaStaticka; } set { izabranaStaticka = value; OnPropertyChanged("IzabranaStaticka"); } }
        
        public static int dozvoljenaKolicinaStaticke;
        
        private Sala salaZaSlanjeStaticke;
        public Sala SalaZaSlanjeStaticke { get { return salaZaSlanjeStaticke; } set { salaZaSlanjeStaticke = value; OnPropertyChanged("SalaZaSlanjeStaticke"); PotvrdiSlanjeStaticke.RaiseCanExecuteChanged(); } }
        
        private string vrijemeSlanjaStaticke;
        public string VrijemeSlanjaStaticke { get { return vrijemeSlanjaStaticke; } set { vrijemeSlanjaStaticke = value; OnPropertyChanged("VrijemeSlanjaStaticke"); PotvrdiSlanjeStaticke.RaiseCanExecuteChanged(); } }
        
        private string kolicinaSlanjaStaticke;
        public string KolicinaSlanjaStaticke { get { return kolicinaSlanjaStaticke; } set { kolicinaSlanjaStaticke = value; OnPropertyChanged("KolicinaSlanjaStaticke"); PotvrdiSlanjeStaticke.RaiseCanExecuteChanged(); } }

        private ObservableCollection<Sala> sale;
        
        private ObservableCollection<Sala> saleDinamicka;
        
        private ObservableCollection<Sala> saleSpajanje;
        
        private ObservableCollection<string> terminiPocetak;
        
        private ObservableCollection<string> terminiKraj;
        
        private ObservableCollection<Oprema> opremaStaraSala;
        
        private ObservableCollection<Oprema> opremaNovaSala;
        
        private ObservableCollection<Oprema> opremaDinamicka;
        
        private ObservableCollectionEx<Oprema> opremaStaticka;
        
        private ObservableCollection<Oprema> dodavanjeDinamicka;

        private ObservableCollection<string> terminiDodavanjaStaticke;
        
        private ObservableCollection<Sala> saleZaDodavanjeStaticke;
        
        private ObservableCollection<Oprema> statickaZaDodavanje;
        
        public static ObservableCollection<Oprema> OpremaStatickaZaDodavanje;
        public ObservableCollection<Sala> SaleZaDodavanjeStaticke { get { return saleZaDodavanjeStaticke; } set { saleZaDodavanjeStaticke = value; OnPropertyChanged("SaleZaDodavanjeStaticke"); } }
        public ObservableCollection<Oprema> StatickaZaDodavanje { get { return statickaZaDodavanje; } set { statickaZaDodavanje = value; OnPropertyChanged("StatickaZaDodavanje"); } }
        public ObservableCollection<string> TerminiDodavanjaStaticke { get { return terminiDodavanjaStaticke; } set { terminiDodavanjaStaticke = value; OnPropertyChanged("TerminiDodavanjaStaticke"); } }

        public ObservableCollection<Oprema> OpremaStaraSala { get { return opremaStaraSala; } set { opremaStaraSala = value; OnPropertyChanged("OpremaStaraSala"); } }
        public ObservableCollection<Oprema> OpremaNovaSala { get { return opremaNovaSala; } set { opremaNovaSala = value; OnPropertyChanged("OpremaNovaSala"); } }
        public ObservableCollection<Oprema> DodavanjeDinamicka { get { return dodavanjeDinamicka; } set { dodavanjeDinamicka = value; OnPropertyChanged("DodavanjeDinamicka"); } }
        public ObservableCollection<Oprema> OpremaDinamicka { get { return opremaDinamicka; } set { opremaDinamicka = value; OnPropertyChanged("OpremaDinamicka"); } }
        public ObservableCollectionEx<Oprema> OpremaStaticka { get { return opremaStaticka; } set { opremaStaticka = value; OnPropertyChanged("OpremaStaticka"); } }
        public ObservableCollection<string> TerminiPocetak { get { return terminiPocetak; } set { terminiPocetak = value; OnPropertyChanged("TerminiPocetak"); } }
        public ObservableCollection<string> TerminiKraj { get { return terminiKraj; } set { terminiKraj = value; OnPropertyChanged("TerminiKraj"); } }
        public ObservableCollection<Sala> Sale { get { return sale; } set { sale = value; OnPropertyChanged("Sale"); } }
        public ObservableCollection<Sala> SaleDinamicka { get { return saleDinamicka; } set { saleDinamicka = value; OnPropertyChanged("SaleDinamicka "); } }
        public ObservableCollection<Sala> SaleSpajanje { get { return saleSpajanje; } set { saleSpajanje = value; OnPropertyChanged("SaleSpajanje"); } }
        
        private ObservableCollection<Sala> saleZaSlanjeStaticke;
        public ObservableCollection<Sala> SaleZaSlanjeStaticke { get { return saleZaSlanjeStaticke; } set { saleZaSlanjeStaticke = value; OnPropertyChanged("SaleZaSlanjeStaticke"); } }

        private ObservableCollection<string> terminiStaticke;
        public ObservableCollection<string> TerminiStaticke { get { return terminiStaticke; } set { terminiStaticke = value; OnPropertyChanged("TerminiStaticke"); } }
        
        private ObservableCollection<Sala> saleZaSlanje;
        public ObservableCollection<Sala> SaleZaSlanje { get { return saleZaSlanje; } set { saleZaSlanje = value; OnPropertyChanged("SaleZaSlanje"); } }

        #endregion

        #region Konstruktor

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
            OdustaniOdDodavanjaDinamicke = new MyICommand(ZatvoriDodavanjeDinamicke);
            PotvrdiDodavanjeDinamicke = new MyICommand(DodavanjeDinamicke, ValidnaPoljaZaDodavanjeDinamicke);
            PregledStatickeKomanda = new MyICommand(PrikaziStaticku);
            ZatvoriStaticku = new MyICommand(ZatvaranjeStaticke);
            ZatvoriStatickuPrikaz = new MyICommand(ZatvaranjeStatickePrikaz);
            PrebaciStatickuKomanda = new MyICommand(OtvoriPrebacivanjeStaticke);
            OdustaniOdSlanjaStaticke = new MyICommand(ZatvoriSlanjeStaticke);
            PotvrdiSlanjeStaticke = new MyICommand(PosaljiStaticku, ValidnaPoljaZaSlanjeStaticke);
            DodajStatickuKomanda = new MyICommand(OtvoriDodavanjeStaticke);
            OdustaniOdDodavanjaStaticke = new MyICommand(ZatvoriDodavanjeStaticke);
            DodajStaticku = new MyICommand(DodavanjeStaticke, ValidnaPoljaZaDodavanjeStaticke);
            ZatvoriProzorKomanda = new MyICommand(ZatvoriSaleProzor);
            ZatvoriSalu = new MyICommand(Zatvori);
            OtvoriZahtjeve = new MyICommand(PrikaziZahtjeve);
            OtvoriKomunikaciju = new MyICommand(PrikaziKomunikaciju);
            OAplikacijiKomanda = new MyICommand(OtvoriOpis);
            PomocSale = new MyICommand(OtvoriPomoc);
            OtvoriIzvjestaj = new MyICommand(OtvaranjeIzvjestaja);
            DodajSale();
        }
        private void DodajSale()
        {
            Sale = new ObservableCollection<Sala>();
            foreach (Sala sala in SaleServis.Sale())
            {
                if (!sala.Namjena.Equals("Skladiste"))
                {
                    Sale.Add(sala);
                }
            }
        }
        #endregion

        #region ZatvoriPrikazSalaViewModel
        private void ZatvoriSale()
        {
            //dio iz upravnik viewMODEL za zatvaranje...
        }
        #endregion

        #region SaleViewModel

        private void OtvoriOpis()
        {
            OAplikacijiViewModel.OAplikacijiProzor = new OAplikaciji();
            OAplikacijiViewModel.OAplikacijiProzor.Show();
            OAplikacijiViewModel.OAplikacijiProzor.DataContext = new OAplikacijiViewModel();
        }
        
        private void PrikaziKomunikaciju()
        {
            KomunikacijaViewModel.KomunikacijaProzor = new Komunikacija();
            KomunikacijaViewModel.KomunikacijaProzor.Show();
            KomunikacijaViewModel.KomunikacijaProzor.DataContext = new KomunikacijaViewModel();
            SaleProzor.Close();
        }
        
        private void PrikaziZahtjeve()
        {
            ZahtjeviViewModel.ZahtjeviProzor = new Zahtjevi();
            ZahtjeviViewModel.ZahtjeviProzor.Show();
            ZahtjeviViewModel.ZahtjeviProzor.DataContext = new ZahtjeviViewModel();
            SaleProzor.Close();
        }
        
        private void Zatvori()
        {
            SaleServis.sacuvajIzmjene();
        }
        
        private void ZatvoriSaleProzor()
        {
            UpravnikViewModel.UpravnikProzor = new Upravnik();
            UpravnikViewModel.UpravnikProzor.Show();
            UpravnikViewModel.UpravnikProzor.DataContext = new UpravnikViewModel();
            SaleProzor.Close();
        }
        
        #endregion

        #region DodajSaluViewModel
        
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

        private void IzmjeniIzabranuSalu() {
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
            if (brojSaleIzmjena != null && namjenaSaleIzmjena != null)
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
            foreach (Sala sala in SaleServis.Sale())
            {
                if (!sala.Namjena.Equals("Skladiste") && sala.Id != izabranaSala.Id && sala.TipSale.Equals(izabranaSala.TipSale))
                {
                    SaleSpajanje.Add(sala);
                }
            }
        }
        
        private void spojiSale()
        {
            SaleServis.dodajOpremuIzSaleZaDodavanje(izabranaSala, izabranaSalaZaSpajanje);
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
                napraviNovuSalu();
                SaleServis.prebaciOpremuIzStareSale(izabranaSala, opremaZaPrebacivanje);
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
                foreach (Sala sala in SaleServis.Sale())
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
                if (oprema.Kolicina != 0 && !postojiOpremaZaPrebacivanje(oprema))
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

        private bool postojiOpremaZaPrebacivanje(Oprema oprema)
        {
            foreach(Oprema opremaZaPrebacivanje in opremaZaPrebacivanje)
            {
                if (opremaZaPrebacivanje.NazivOpreme.Equals(oprema.NazivOpreme))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region KrevetViewModel
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
        private void promjenjenTekstPretrage()
        {
            Sale.Clear();
            foreach (Sala sala in SaleServis.Sale())
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
                PrikazDinamickeProzor = new PrikazDinamicke();
                PrikazDinamickeProzor.Show();
                postaviTekst();
                dodajDinamickuOpremu();
                pretragaDinamicke = "";
                aktivnaDinamicka = true;
                aktivnaStaticka = false;
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
            aktivnaDinamicka = false;
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
         
        private void OtvoriPremjestanjeDinamicke()
        {
            if (izabranaDinamicka != null)
            {
                PremjestanjeDinamickeProzor = new SlanjeDinamicke();
                PremjestanjeDinamickeProzor.Show();
                saleZaSlanje = new ObservableCollection<Sala>();
                maxDinamickeTekst = "MAX: " + izabranaDinamicka.Kolicina.ToString();
                NazivDinamicke = izabranaDinamicka.NazivOpreme;
                KolicinaSlanjeDinamicke = "";
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
        
        private void OtvoriDodavanjeDinamicke()
        {
            DodavanjeDinamickeProzor = new PreraspodjelaDinamicke();
            DodavanjeDinamickeProzor.Show();
            DodavanjeDinamicka = new ObservableCollection<Oprema>();
            SaleDinamicka = new ObservableCollection<Sala>();
            dozvoljenaKolicinaDinamicka = 0;
            MaxDinamickaText = "";
            KolicinaDodavanjeDinamicke = "";
            dodajDinamicku();
            DodavanjeDinamickeProzor.DataContext = this;
        }
        private void ZatvoriDodavanjeDinamicke()
        {
            DodavanjeDinamickeProzor.Close();
        }
        private void dodajDinamicku()
        {
            dodajIzSkladista();
            dodajIzSala();
        }

        private void DodavanjeDinamicke()
        {
            PremjestajServis.prebaciOpremu(IzabranaSalaDodavanje, int.Parse(KolicinaDodavanjeDinamicke), IzabranaDinamickaDodavanje, IzabranaSala);
            azurirajPrikazDinamicke();
            DodavanjeDinamickeProzor.Close();
        }

        private void dodajIzSkladista()
        {
            foreach (Oprema oprema in OpremaServis.Oprema())
            {
                if (!oprema.Staticka)
                {
                    DodavanjeDinamicka.Add(oprema);
                }
            }
        }
        private void dodajIzSala()
        {
            foreach (Sala sala in SaleServis.Sale())
            {
                foreach (Oprema oprema in sala.Oprema)
                {
                    dodajDinamickuOpremu(oprema);
                }
            }
        }
        private void dodajDinamickuOpremu(Oprema oprema)
        {
            if (!oprema.Staticka)
            {

                if (!postojiOprema(oprema))
                {
                    DodavanjeDinamicka.Add(oprema);
                }
            }
        }

        private void promjenjenaOpremaDinamicka()
        {
            SaleDinamicka.Clear();
            opremaZaDodavanje = IzabranaDinamickaDodavanje;
            foreach (Sala sala in SaleServis.Sale())
            {
                foreach (Oprema oprema in sala.Oprema)
                {
                    dodajSalu(oprema, sala);
                }
            }
        }

        private bool ValidnaPoljaZaDodavanjeDinamicke()
        {
            if (izabranaDinamickaDodavanje != null & IzabranaSalaDodavanje != null && kolicinaDodavanjeDinamicke != null)
            {
                if (jeBroj(KolicinaDodavanjeDinamicke.ToString()))
                {

                    if (int.Parse(KolicinaDodavanjeDinamicke) > dozvoljenaKolicinaDinamicka || int.Parse(KolicinaDodavanjeDinamicke) <= 0 || IzabranaSalaDodavanje == null || izabranaDinamickaDodavanje == null)
                    {
                        return false;
                    }
                    else if (int.Parse(kolicinaDodavanjeDinamicke) <= dozvoljenaKolicinaDinamicka && int.Parse(KolicinaDodavanjeDinamicke) > 0 && IzabranaSalaDodavanje != null && IzabranaDinamickaDodavanje != null)
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

        private void dodajSalu(Oprema oprema, Sala sala)
        {
            if (IzabranaDinamickaDodavanje != null)
            {
                if (oprema.IdOpreme == IzabranaDinamickaDodavanje.IdOpreme)
                {
                    if (sala.Id != IzabranaSala.Id)
                    {
                        SaleDinamicka.Add(sala);
                    }

                }
            }
        }

        private void promjenjenaSalaDinamicka()
        {
            if (IzabranaSalaDodavanje != null)
            {

                foreach (Sala sala in SaleServis.Sale())
                {
                    if (IzabranaSalaDodavanje.Id == sala.Id)
                    {
                        postaviTekstDinamicka(sala);
                    }
                }
            }
        }

        private void postaviTekstDinamicka(Sala sala)
        {
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == izabranaDinamickaDodavanje.IdOpreme)
                {
                    MaxDinamickaText = "MAX:" + oprema.Kolicina.ToString();
                    dozvoljenaKolicinaDinamicka = oprema.Kolicina;
                }
            }
        }

        private bool postojiOprema(Oprema oprema)
        {
            foreach (Oprema dinamickaOprema in DodavanjeDinamicka)
            {
                if (dinamickaOprema.IdOpreme == oprema.IdOpreme)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region SlanjeStatickeViewModel
        
        private void PrikaziStaticku()
        {
            if (izabranaSala != null)
            {
                try
                {
                    aktivnaStaticka = true;
                    aktivnaDinamicka = false;
                    PrikazStatickeProzor = new PrikazStaticke();
                    PremjestajServis.odradiZakazanePremjestaje();
                    PrikazStatickeProzor.Show();
                    postaviTekstStaticka();
                    dodajStatickuOpremu();
                    otvoren = true;
                    Thread th = new Thread(izvrsi);
                    Thread th1 = new Thread(azurirajPrikaz);
                    th.Start();
                    th1.Start();
                    PrikazStatickeProzor.DataContext = this;
                }
                catch (Exception ex) { Console.WriteLine(ex.Data); }
            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
        }
        private void azurirajPrikaz()
        {
            while (otvoren)
            {
                if (azuriraj)
                {
                    Thread.Sleep(1000);
                    azurirajPrikazStaticke();
                    azuriraj = false;
                }
            }
        }

        private void azurirajPrikazStaticke()
        {
            OpremaStaticka.Clear();
            List<Oprema> opremaStaticka1 = new List<Oprema>();
            if (izabranaSala.Oprema != null)
            {
                foreach (Sala sala in SaleServis.Sale())
                {
                    if (sala.Id == izabranaSala.Id)
                    {
                        foreach (Oprema oprema in sala.Oprema)
                        {
                            if (oprema.Staticka)
                            {
                                opremaStaticka1.Add(oprema);
                            }
                        }
                    }
                }
            }
            OpremaStaticka = new ObservableCollectionEx<Oprema>(opremaStaticka1);
        }
        private void ZatvaranjeStatickePrikaz()
        {
            aktivnaStaticka = false;
            otvoren = false;
            PrikazStatickeProzor.Close();
        }
        private void ZatvaranjeStaticke()
        {
            aktivnaStaticka = false;
            otvoren = false;
        }
        public void izvrsi()
        {
            while (otvoren)
            {
                Thread.Sleep(10);
                PremjestajServis.odradiZakazanePremjestaje();
            }
        }
        public void dodajStatickuOpremu()
        {
            List<Oprema> opremaStaticka1 = new List<Oprema>();
            if (izabranaSala.Oprema != null)
            {
                foreach (Sala s in SaleServis.Sale())
                {
                    if (s.Id == izabranaSala.Id)
                    {
                        dodajOpremu(s, opremaStaticka1);
                    }
                }
            }
            OpremaStaticka = new ObservableCollectionEx<Oprema>(opremaStaticka1);
        }

        private void dodajOpremu(Sala sala, List<Oprema> opremaStaticka1)
        {
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.Staticka)
                {
                    opremaStaticka1.Add(oprema);
                }
            }
        }
        private void postaviTekstStaticka()
        {
            if (izabranaSala != null)
            {
                if (izabranaSala.TipSale == tipSale.SalaZaPregled)
                {
                    TekstStaticka = "Sala za pregled (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
                else if (izabranaSala.TipSale == tipSale.OperacionaSala)
                {
                    TekstStaticka = "Operaciona sala (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
                else
                {
                    TekstStaticka = "Sala za odmor (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
            }
        }
        #endregion

        #region PretragaStatickeViewModel
       
        private void pretraziStaticku()
        {
            OpremaStaticka.Clear();
            foreach (Oprema oprema in izabranaSala.Oprema)
            {
                if (oprema.NazivOpreme.StartsWith(PretragaStaticke) && oprema.Staticka)
                {
                    OpremaStaticka.Add(oprema);
                }
            }
        }
        #endregion

        #region PrebaciStatickuViewModel
        private void OtvoriPrebacivanjeStaticke()
        {
            if (IzabranaStaticka != null)
            {
                if (provjeriPreostalo(izabranaStaticka))
                {
                    SlanjeStatickeProzor = new SlanjeStaticke(izabranaSala, izabranaStaticka);
                    SlanjeStatickeProzor.Show();
                    statickaZaSlanje = izabranaStaticka.NazivOpreme;
                    SaleZaSlanjeStaticke = new ObservableCollection<Sala>();
                    TerminiStaticke = new ObservableCollection<string>();
                    dodajSale(IzabranaSala);
                    datumSlanjaStaticke = DateTime.Now.Date;
                    dodajTermine();
                    postaviDozvoljenuKolicinu();
                    kolicinaSlanjaStaticke = "";
                    SlanjeStatickeProzor.DataContext = this;
                }
                else
                {
                    MessageBox.Show("Preostala oprema je vec zakazana za transfer");
                }
            }
            else
            {
                MessageBox.Show("Morate izabrati opremu");
            }
        }

        private bool ValidnaPoljaZaSlanjeStaticke()
        {
            if (kolicinaSlanjaStaticke != null && VrijemeSlanjaStaticke != null && DatumSlanjaStaticke != null && salaZaSlanjeStaticke != null)
            {
                if (jeBroj(kolicinaSlanjaStaticke))
                {
                    if (int.Parse(kolicinaSlanjaStaticke) > dozvoljenaKolicinaStaticke || int.Parse(kolicinaSlanjaStaticke) <= 0 || salaZaSlanjeStaticke == null || VrijemeSlanjaStaticke == null)
                    {
                        return false;
                    }
                    else if (int.Parse(kolicinaSlanjaStaticke) <= dozvoljenaKolicinaStaticke && int.Parse(kolicinaSlanjaStaticke) > 0 && salaZaSlanjeStaticke != null && VrijemeSlanjaStaticke != null)
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
        private DateTime nadjiTerminPremjestaja()
        {
            DateTime? datumSlanja = DatumSlanjaStaticke;
            string vrijemeSlanja = vrijemeSlanjaStaticke;
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
        private void PosaljiStaticku()
        {
            PremjestajServis.prebaciStatickuOpremu(nadjiTerminPremjestaja(), int.Parse(kolicinaSlanjaStaticke), SalaZaSlanjeStaticke, izabranaSala, izabranaStaticka);
            SlanjeStatickeProzor.Close();
        }
        private void ZatvoriSlanjeStaticke()
        {
            SlanjeStatickeProzor.Close();
        }
        private void postaviDozvoljenuKolicinu()
        {
            int kolicina = izabranaStaticka.Kolicina;
            foreach (Premjestaj premjestaj in PremjestajServis.Premjestaji())
            {
                if (premjestaj.izSale.Id == izabranaSala.Id && premjestaj.oprema.IdOpreme == izabranaStaticka.IdOpreme)
                {
                    kolicina -= premjestaj.kolicina;
                }
            }
            MaxStaticka = "MAX: " + kolicina.ToString();
            dozvoljenaKolicinaStaticke = kolicina;
        }
        private void dodajTermine()
        {
            for (int termin = (int)DateTime.Now.Hour + 1; termin <= 23; termin++)
            {
                if (!postojiTermin(termin))
                {
                    TerminiStaticke.Add(termin + ":00");
                }
            }
        }

        private void promijenjenDatumSlanja(){
            if (TerminiStaticke.Count != 0)
            {
                azurirajTermine();
            }
        }

        private void azurirajTermine()
        {
            if (DatumSlanjaStaticke == DateTime.Now.Date)
            {
                TerminiStaticke.Clear();
                dodajTermine();
            }
            else
            {
                dodajTermineDrugiDatum();
            }
        }

        private void dodajTermineDrugiDatum()
        {
            string[] prviTermin = TerminiStaticke[0].Split(':');
            string prvi = prviTermin[0];
            for (int termin = int.Parse(prvi); termin > 0; termin--)
            {
                if (!postojiTermin(termin))
                {
                    TerminiStaticke.Insert(0, termin + ":00");
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
        private void dodajSale(Sala izabranaSala)
        {
            foreach (Sala sala in SaleServis.Sale())
            {
                if (sala.Id != izabranaSala.Id)
                {
                    SaleZaSlanjeStaticke.Add(sala);
                }
            }
        }
        private bool provjeriPreostalo(Oprema opremaZaSlanje)
        {
            int kolicina = opremaZaSlanje.Kolicina;
            foreach (Premjestaj premjestaj in PremjestajServis.Premjestaji())
            {
                if (premjestaj.izSale.Id == izabranaSala.Id && premjestaj.oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    kolicina -= premjestaj.kolicina;
                }
            }
            if (kolicina <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region DodajStatickuViewModel
        private void OtvoriDodavanjeStaticke()
        {
            try
            {
                DodavanjeStatickeProzor = new PreraspodjelaStaticke();
                DodavanjeStatickeProzor.Show();
                TerminiDodavanjaStaticke = new ObservableCollection<string>();
                SaleZaDodavanjeStaticke = new ObservableCollection<Sala>();
                StatickaZaDodavanje = new ObservableCollection<Oprema>();
                dozvoljenaKolicinaDodavanjeStaticke = 0;
                OpremaStatickaZaDodavanje = statickaZaDodavanje;
                tekstDodavanjaStaticke = "";
                KolicinaDodavanjaStaticke = "";
                DatumPrebacivanja = DateTime.Now.Date;
                dodajTermineDodavanjaStaticke();
                dodajStaticku();
                DodavanjeStatickeProzor.DataContext = this;
            }
            catch (Exception ex) { Console.WriteLine(ex.Data); }
        }
        private bool ValidnaPoljaZaDodavanjeStaticke()
        {
            if (kolicinaDodavanjaStaticke != null && IzabranaStatickaDodavanje != null && IzabranaSalaZaDodavanje != null && vrijemeDodavanja != null)
            {
                if (jeBroj(kolicinaDodavanjaStaticke))
                {
                    if (int.Parse(kolicinaDodavanjaStaticke) > dozvoljenaKolicinaDodavanjeStaticke || int.Parse(kolicinaDodavanjaStaticke) <= 0 || IzabranaStatickaDodavanje == null || IzabranaSalaZaDodavanje == null || vrijemeDodavanja == null)
                    {
                        return false;
                    }
                    else if (int.Parse(kolicinaDodavanjaStaticke) <= dozvoljenaKolicinaDodavanjeStaticke && int.Parse(kolicinaDodavanjaStaticke) > 0 && IzabranaStatickaDodavanje != null && IzabranaSalaZaDodavanje != null && vrijemeDodavanja != null)
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
        private void ZatvoriDodavanjeStaticke()
        {
            DodavanjeStatickeProzor.Close();
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
            PremjestajServis.dodajStatickuOpremu(IzabranaSalaZaDodavanje, int.Parse(kolicinaDodavanjaStaticke), napraviTerminPremjestaja(), izabranaSala, IzabranaStatickaDodavanje);
            DodavanjeStatickeProzor.Close();
        }
        private void izracunajDozvoljenuKolicinu(Sala sala, Sala izabranaSala)
        {
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == izabranaStatickaDodavanje.IdOpreme)
                {
                    dozvoljenaKolicinaDodavanjeStaticke = nadjiDozvoljenuKolicinuDodavanja(oprema, izabranaSala);
                    TekstDodavanjaStaticke = "MAX:" + dozvoljenaKolicinaDodavanjeStaticke;
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
                if (!postojiTerminDodavanja(termin))
                {
                    TerminiDodavanjaStaticke.Insert(0, termin + ":00");
                }
            }
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
                if (pm.izSale.Id == IzabranaSala.Id && pm.oprema.IdOpreme == IzabranaStatickaDodavanje.IdOpreme)
                {
                    kolicina -= pm.kolicina;
                }
            }
            return kolicina;
        }

        private void dodajTermineDodavanjaStaticke()
        {
            for (int termin = (int)DateTime.Now.Hour + 1; termin <= 23; termin++)
            {
                if (!postojiTerminDodavanja(termin))
                {
                    TerminiDodavanjaStaticke.Add(termin + ":00");
                }
            }
        }

        private bool postojiTerminDodavanja(int termin)
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
        }
        private bool postojiStatickaOprema(Oprema oprema)
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

        #endregion

        #region PomocViewModel
        private void OtvoriPomoc()
        {
            PomocSaleProzor = new SalePomoc();
            PomocSaleProzor.Show();
            PomocSaleProzor.DataContext = this;
        }

        #endregion

        #region OtvoriIzvjestajViewModel

        private void OtvaranjeIzvjestaja()
        {
            IzvjestajViewModel.IzvjestajProzor = new Izvjestaj();
            IzvjestajViewModel.IzvjestajProzor.Show();
            IzvjestajViewModel.IzvjestajProzor.DataContext = new IzvjestajViewModel();
            SaleProzor.Close();
        }

        #endregion
    }
}
