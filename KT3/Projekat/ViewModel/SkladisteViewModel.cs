using Model;
using Projekat.Model;
using Projekat.Pomoc;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Projekat.ViewModel
{
    class SkladisteViewModel : BindableBase
    {
        #region Promjenljive
        public Window DodajOpremuProzor { get; set; }
        public Window IzmjeniOpremuProzor { get; set; }
        public Window PrebaciStatickuProzor { get; set; }
        public Window PrebaciDinamickuProzor { get; set; }
        public Window ObrisiOpremuProzor { get; set; }
        public Window PomocProzor { get; set; }
        public static Window SkladisteProzor { get; set; }
        public MyICommand OtvoriIzvjestaj { get; set; }
        public MyICommand OtvoriOAplikaciji { get; set; }
        public MyICommand OtvoriPomoc { get; set; }
        public MyICommand PrikaziSale { get; set; }
        public MyICommand PrikaziKomunikaciju { get; set; }
        public MyICommand NapustiSkladiste { get; set; }
        public MyICommand DodajOpremu { get; set; }
        public MyICommand ZatvoriDodavanjeOpreme { get; set; }
        public MyICommand ZatvoriSkladisteProzor { get; set; }
        public MyICommand DodavanjeOpremeKomanda { get; set; }
        public MyICommand IzmjeniOpremu { get; set; }
        public MyICommand OdustaniOdIzmjeneOpreme { get; set; }
        public MyICommand PotvrdiIzmjenuOpreme { get; set; }
        public MyICommand OdustaniOdSlanjaStaticke { get; set; }
        public MyICommand PotvrdiSlanjeStaticke { get; set; }
        public MyICommand ObrisiOpremu { get; set; }
        public MyICommand OdustaniOdBrisanjaOpreme { get; set; }
        public MyICommand PotvrdiBrisanjeOpreme { get; set; }
        public MyICommand ZatvoriPrebacivanjaStaticke { get; set; }
        public MyICommand PrebaciOpremu { get; set; }
        public MyICommand OdustaniOdPrebacivanjaDinamicke { get; set; }
        public MyICommand PrebaciDinamickuKomanda { get; set; }
        public MyICommand ZatvoriPrebacivanjaDinamicke { get; set; }

        public static bool otvoren;
        
        private int izabraniTab;
        
        public static int dozvoljenaKolicina;
        
        public static int dozvoljenaKolicinaStaticke;
        
        public static bool azuriraj = false;
        
        private string tekstPretrage;

        private string nazivOpreme;

        private string kolicinaOpreme;
        
        private Oprema izabranaStaticka;
        
        private Oprema izabranaDinamicka;
        
        Oprema izabranaOprema;

        private string izmjenaOpremeNaziv;
        
        private string izmjenaOpremeKolicina;
        
        public static bool aktivnaDinamicka;
        
        private string dinamicka;
        
        private string maxDinamicka;
        
        private string nazivStaticke;
        
        private string maxStaticka;
        
        public static bool aktivnaStaticka;
        
        private DateTime izabraniDatum;
        
        private Sala salaStaticka;
        
        private string termin;
        
        private string kolicinaZaBrisanje;
        
        private string unesenaKolicinaZaBrisanje;
        public string UnesenaKolicinaZaBrisanje{get { return unesenaKolicinaZaBrisanje; }set{unesenaKolicinaZaBrisanje = value;OnPropertyChanged("UnesenaKolicinaZaBrisanje");PotvrdiBrisanjeOpreme.RaiseCanExecuteChanged();}}
        public string KolicinaZaBrisanje{get { return kolicinaZaBrisanje; }set { kolicinaZaBrisanje = value; OnPropertyChanged("KolicinaZaBrisanje"); }}
        public string Termin{get { return termin; }set { termin = value; OnPropertyChanged("Termin"); PotvrdiSlanjeStaticke.RaiseCanExecuteChanged(); }}
        public Sala SalaStaticka{get { return salaStaticka; }set { salaStaticka = value; OnPropertyChanged("SaleStaticka"); PotvrdiSlanjeStaticke.RaiseCanExecuteChanged(); }}
        public DateTime IzabraniDatum{get { return izabraniDatum; }set { izabraniDatum = value; OnPropertyChanged("IzabraniDatum"); PromijenioSeDatum(); }}
        public string MaxStaticka{get { return maxStaticka; }set { maxStaticka = value; OnPropertyChanged("MaxStaticka"); }}
        public string NazivStaticke{get { return nazivStaticke; }set { nazivStaticke = value; OnPropertyChanged("NazivStaticke"); }}
        public string Dinamicka { get { return dinamicka; } set { dinamicka = value; OnPropertyChanged("Dinamicka"); } }
        public string MaxDinamicka { get { return maxDinamicka; } set { maxDinamicka = value; OnPropertyChanged("MaxDinamicka"); } }
        
        private Sala salaDinamicka;
        public Sala SalaDinamicka{get { return salaDinamicka; }set { salaDinamicka = value; OnPropertyChanged("SalaDinamicka"); PrebaciDinamickuKomanda.RaiseCanExecuteChanged(); }}
        public string IzmjenaOpremeNaziv{get { return izmjenaOpremeNaziv; }set { izmjenaOpremeNaziv = value; OnPropertyChanged("IzmjenaOpremeNaziv"); PotvrdiIzmjenuOpreme.RaiseCanExecuteChanged(); }}
        public string IzmjenaOpremeKolicina{get { return izmjenaOpremeKolicina; }set { izmjenaOpremeKolicina = value; OnPropertyChanged("IzmjenaOpremeKolicina"); PotvrdiIzmjenuOpreme.RaiseCanExecuteChanged(); }}
        public Oprema IzabranaStaticka{get { return izabranaStaticka; }set { izabranaStaticka = value; OnPropertyChanged("IzabranaStaticka"); }}
        public Oprema IzabranaDinamicka{get { return izabranaDinamicka; }set { izabranaDinamicka = value; OnPropertyChanged("IzabranaDinamicka"); }}
        public string NazivOpreme{get { return nazivOpreme; }set { nazivOpreme = value; OnPropertyChanged("NazivOpreme"); DodavanjeOpremeKomanda.RaiseCanExecuteChanged(); }}
        public string KolicinaOpreme{get { return kolicinaOpreme; }set { kolicinaOpreme = value; OnPropertyChanged("KolicinaOpreme"); DodavanjeOpremeKomanda.RaiseCanExecuteChanged(); }}
        public string TekstPretrage{get { return tekstPretrage; }set { tekstPretrage = value; OnPropertyChanged("TekstPretrage"); PretraziSkladiste(); }}
        public int IzabraniTab { get { return izabraniTab; } set { izabraniTab = value; OnPropertyChanged("IzabraniTab"); } }
        
        public string validacija;
        public string Validacija { get { return validacija; } set { if (value != validacija) { validacija = value; OnPropertyChanged("Validacija"); PrebaciDinamickuKomanda.RaiseCanExecuteChanged(); PotvrdiSlanjeStaticke.RaiseCanExecuteChanged(); } } }
        
        List<Oprema> opremaStaticka1;
        
        private ObservableCollection<Sala> sale;
        public ObservableCollection<Sala> Sale { get { return sale; } set { sale = value; OnPropertyChanged("Sale"); } }

        private ObservableCollection<string> termini;
        public ObservableCollection<string> Termini { get { return termini; } set { termini = value; OnPropertyChanged("Termini"); } }

        public ObservableCollectionEx<Oprema> opremaStaticka;

        private ObservableCollection<Oprema> opremaDinamicka;
        public ObservableCollectionEx<Oprema> OpremaStaticka { get { return opremaStaticka; } set { opremaStaticka = value; OnPropertyChanged("OpremaStaticka"); } }
        public ObservableCollection<Oprema> OpremaDinamicka { get { return opremaDinamicka; } set { opremaDinamicka = value; OnPropertyChanged("OpremaDinamicka"); } }

        #endregion

        #region Konstruktor
        public SkladisteViewModel()
        {
            OpremaDinamicka = new ObservableCollection<Oprema>();
            opremaStaticka1 = new List<Oprema>();
            dodajOpremu();
            OpremaStaticka = new ObservableCollectionEx<Oprema>(opremaStaticka1);
            Thread th = new Thread(izvrsi);
            Thread th1 = new Thread(Azuriraj);
            th.Start();
            th1.Start();
            NapustiSkladiste = new MyICommand(ZatvoriSkladiste);
            DodajOpremu = new MyICommand(DodavanjeOpreme);
            ZatvoriDodavanjeOpreme = new MyICommand(ZatvoriDodavanje);
            DodavanjeOpremeKomanda = new MyICommand(DodajOpremuUSkladiste, ValidnaPoljaZaDodavanjeOpreme);
            IzmjeniOpremu = new MyICommand(IzmjenaOpreme);
            OdustaniOdIzmjeneOpreme = new MyICommand(ZatvoriIzmjenuOpreme);
            PotvrdiIzmjenuOpreme = new MyICommand(IzvrsiIzmjenuOpreme, ValidnaPoljaZaIzmjenuOpreme);
            PrebaciOpremu = new MyICommand(OtvoriPrebacivanjeOpreme);
            PrebaciDinamickuKomanda = new MyICommand(PrebaciDinamickuOpremu, ValidnaPoljaZaPrebacivanjeDinamicke);
            OdustaniOdPrebacivanjaDinamicke = new MyICommand(ZatvoriPrebacivanjeDinamicke);
            ZatvoriPrebacivanjaDinamicke = new MyICommand(DeaktivirajDinamicku);
            OdustaniOdSlanjaStaticke = new MyICommand(ZatvoriSlanjeStaticke);
            ZatvoriPrebacivanjaStaticke = new MyICommand(ZatvoriSlanjeDinamicke);
            PotvrdiSlanjeStaticke = new MyICommand(PosaljiStaticku, ValidnaPoljaZaSlanjeStaticke);
            ZatvoriSkladisteProzor = new MyICommand(DeaktivirajSkladiste);
            ObrisiOpremu = new MyICommand(BrisanjeOpreme);
            OdustaniOdBrisanjaOpreme = new MyICommand(OdustaniOdBrisanja);
            PotvrdiBrisanjeOpreme = new MyICommand(ObrisiIzabranuOpremu, ValidnaPoljaZaBrisanjeOpreme);
            PrikaziSale = new MyICommand(OtvoriSale);
            PrikaziKomunikaciju = new MyICommand(OtvoriKomunikaciju);
            OtvoriOAplikaciji = new MyICommand(OtvoriOpis);
            OtvoriIzvjestaj = new MyICommand(PrikaziIzvjestaj);
            OtvoriPomoc = new MyICommand(Pomoc);
        }
        public void izvrsi()
        {
            while (otvoren)
            {
                Thread.Sleep(2000);
                PremjestajServis.odradiZakazanePremjestaje();
            }
        }

        private void dodajOpremu()
        {
            foreach (Sala sala in SaleServis.Sale())
            {
                if (sala.Namjena.Equals("Skladiste"))
                {
                    pronadjiOpremuZaDodavanje();
                }
            }
        }
        #endregion

        #region SkladisteViewModel
        private void Pomoc()
        {
            PomocProzor = new SkladistePomoc();
            PomocProzor.Show();
            PomocProzor.DataContext = this;
        }
        private void PrikaziIzvjestaj()
        {
            IzvjestajViewModel.IzvjestajProzor = new Izvjestaj();
            IzvjestajViewModel.IzvjestajProzor.Show();
            IzvjestajViewModel.IzvjestajProzor.DataContext = new IzvjestajViewModel();
            SkladisteProzor.Close();
        }
        private void OtvoriOpis()
        {
            OAplikacijiViewModel.OAplikacijiProzor = new OAplikaciji();
            OAplikacijiViewModel.OAplikacijiProzor.Show();
            OAplikacijiViewModel.OAplikacijiProzor.DataContext = new OAplikacijiViewModel();
        }
        private void OtvoriKomunikaciju()
        {
            KomunikacijaViewModel.KomunikacijaProzor = new Komunikacija();
            KomunikacijaViewModel.KomunikacijaProzor.Show();
            KomunikacijaViewModel.KomunikacijaProzor.DataContext = new KomunikacijaViewModel();
            SkladisteProzor.Close();
        }
        private void OtvoriSale()
        {
            SaleViewModel.SaleProzor = new PrikaziSalu();
            SaleViewModel.SaleProzor.Show();
            SaleViewModel.SaleProzor.DataContext = new SaleViewModel();
            SkladisteProzor.Close();
        }
        
        #endregion

        #region PretragaSkladistaViewModel
        
        private void PretraziSkladiste()
        {
            if (izabraniTab == 0)
            {
                nadjiStaticku();
            }
            else if (izabraniTab == 1)
            {
                nadjiDinamicku();
            }
        }
        private void nadjiStaticku()
        {
            OpremaStaticka.Clear();
            foreach (Oprema oprema in OpremaServis.Oprema())
            {
                if (oprema.NazivOpreme.StartsWith(tekstPretrage) && oprema.Staticka)
                {
                    OpremaStaticka.Add(oprema);
                }
            }
        }

        private void nadjiDinamicku()
        {
            OpremaDinamicka.Clear();
            foreach (Oprema oprema in OpremaServis.Oprema())
            {
                if (oprema.NazivOpreme.StartsWith(tekstPretrage) && !oprema.Staticka)
                {
                    OpremaDinamicka.Add(oprema);
                }
            }
        }
        #endregion

        #region NapustiSkladisteViewModel
        private void ZatvoriSkladiste()
        {
            SkladisteViewModel.SkladisteProzor.Close();
            ZahtjeviViewModel.ZahtjeviProzor = new Zahtjevi();
            ZahtjeviViewModel.ZahtjeviProzor.Show();
            ZahtjeviViewModel.ZahtjeviProzor.DataContext = new ZahtjeviViewModel();
            otvoren = false;
        }
        #endregion

        #region DodajOpremuViewModel

        private void pronadjiOpremuZaDodavanje()
        {
            foreach (Oprema oprema in OpremaServis.Oprema())
            {
                if (oprema.Staticka)
                {
                    opremaStaticka1.Add(oprema);
                }
                else
                {
                    OpremaDinamicka.Add(oprema);
                }
            }
        }
        private bool ValidnaPoljaZaDodavanjeOpreme()
        {
            if (kolicinaOpreme != null && nazivOpreme != null)
            {
                if (!jeBroj(kolicinaOpreme) || kolicinaOpreme.Trim().Equals("") || jeBroj(nazivOpreme) || nazivOpreme.Trim().Equals(""))
                {
                    return false;
                }
                else if (jeBroj(kolicinaOpreme) && !kolicinaOpreme.Trim().Equals("") && !jeBroj(nazivOpreme) && !nazivOpreme.Trim().Equals(""))
                {
                    return true;
                }
            }
            return false;
        }

        public bool jeBroj(string input)
        {
            int test;
            return int.TryParse(input, out test);
        }

        private void DodajOpremuUSkladiste()
        {
            Oprema oprema = napraviOpremu();
            OpremaServis.DodajOpremu(oprema);
            if (izabraniTab == 0)
            {
                OpremaStaticka.Add(oprema);
            }
            else if (izabraniTab == 1)
            {
                OpremaDinamicka.Add(oprema);
            }
            DodajOpremuProzor.Close();
        }
        private Oprema napraviOpremu()
        {
            bool staticka = true;
            if (izabraniTab == 0)
            {
                staticka = true;
            } else if (izabraniTab == 1)
            {
                staticka = false;
            }
            int idOpreme = SkladisteServis.GenerisanjeIdOpreme();
            Oprema oprema = new Oprema(nazivOpreme, int.Parse(kolicinaOpreme), staticka);
            oprema.IdOpreme = idOpreme;
            return oprema;
        }

        private void DodavanjeOpreme()
        {
            DodajOpremuProzor = new DodajOpremu();
            DodajOpremuProzor.Show();
            kolicinaOpreme = "";
            nazivOpreme = "";
            DodajOpremuProzor.DataContext = this;
        }
        private void ZatvoriDodavanje()
        {
            DodajOpremuProzor.Close();
        }
        #endregion

        #region IzmjeniOpremuViewModel

        private void IzmjenaOpreme()
        {
            if (izabraniTab == 0)
            {
                izabranaOprema = izabranaStaticka;
                IzmjeniOpremuUSkladistu(izabranaStaticka);
            } else if (izabraniTab == 1)
            {
                izabranaOprema = izabranaDinamicka;
                IzmjeniOpremuUSkladistu(izabranaDinamicka);
            }
        }

        private bool ValidnaPoljaZaIzmjenuOpreme()
        {
            if (izmjenaOpremeNaziv != null && izmjenaOpremeKolicina != null)
            {
                if (izmjenaOpremeNaziv.Trim().Equals("") || jeBroj(IzmjenaOpremeNaziv) || !jeBroj(izmjenaOpremeKolicina) || izmjenaOpremeKolicina.Trim().Equals(""))
                {
                    return false;
                }
                else if (!izmjenaOpremeNaziv.Trim().Equals("") && !jeBroj(izmjenaOpremeNaziv) && jeBroj(izmjenaOpremeKolicina) && !IzmjenaOpremeKolicina.Trim().Equals(""))
                {
                    return true;
                }
            }
            return false;
        }

        private void IzmjeniOpremuUSkladistu(Oprema oprema)
        {
            if (oprema != null)
            {
                IzmjeniOpremuProzor = new IzmjeniOpremu();
                IzmjeniOpremuProzor.Show();
                izmjenaOpremeNaziv = oprema.NazivOpreme;
                izmjenaOpremeKolicina = oprema.Kolicina.ToString();
                IzmjeniOpremuProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati opremu!");
            }
        }

        private void IzvrsiIzmjenuOpreme()
        {
            bool staticka = false;
            if (izabraniTab == 0)
            {
                staticka = true;
            }
            Oprema oprema = new Oprema(izmjenaOpremeNaziv, int.Parse(izmjenaOpremeKolicina), staticka);
            oprema.IdOpreme = izabranaOprema.IdOpreme;
            OpremaServis.izmjeniOpremu(izabranaOprema, oprema);
            izmjeniPrikazOpreme(izabranaOprema, oprema);
            IzmjeniOpremuProzor.Close();
        }

        private void izmjeniPrikazOpreme(Oprema izOpreme, Oprema uOpremu)
        {
            foreach (Oprema oprema in OpremaServis.Oprema())
            {
                if (oprema.IdOpreme == izOpreme.IdOpreme)
                {
                    //oprema.NazivOpreme = uOpremu.NazivOpreme;
                    //oprema.Kolicina = uOpremu.Kolicina;
                    zamjeniOpremu(izOpreme, oprema);
                }
            }
        }

        private void zamjeniOpremu(Oprema izOpreme, Oprema oprema)
        {
            if (izabraniTab == 0)
            {
                zamjeniStatickuOpremuUSkladistu(izOpreme, oprema);
            }
            else
            {
                zamjeniDinamickuOpremuUSkladistu(izOpreme, oprema);
            }
        }

        private void zamjeniDinamickuOpremuUSkladistu(Oprema izOpreme, Oprema oprema)
        {
            int idx = OpremaDinamicka.IndexOf(izOpreme);
            OpremaDinamicka.RemoveAt(idx);
            OpremaDinamicka.Insert(idx, oprema);
        }

        private void zamjeniStatickuOpremuUSkladistu(Oprema izOpreme, Oprema oprema)
        {
            int idx = OpremaStaticka.IndexOf(izOpreme);
            OpremaStaticka.RemoveAt(idx);
            OpremaStaticka.Insert(idx, oprema);
        }

        private void ZatvoriIzmjenuOpreme()
        {
            IzmjeniOpremuProzor.Close();
        }
        public void azurirajOpremu()
        {
            OpremaDinamicka.Clear();
            OpremaStaticka.Clear();
            foreach (Sala sala in SaleServis.Sale())
            {
                if (sala.Namjena.Equals("Skladiste"))
                {
                    azurirajPrikazOpreme();
                }
            }
        }

        public static void azurirajPrikaz()
        {
            azuriraj = true;
        }

        private void Azuriraj()
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
            foreach (Sala sala in SaleServis.Sale())
            {
                if (sala.Namjena.Equals("Skladiste"))
                {
                    foreach (Oprema oprema in OpremaServis.Oprema())
                    {
                        if (oprema.Staticka && oprema.Kolicina != 0)
                        {
                            OpremaStaticka.Add(oprema);
                        }
                    }
                }
            }
        }

        private void azurirajPrikazOpreme()
        {
            foreach (Oprema oprema in OpremaServis.Oprema())
            {
                if (oprema.Staticka)
                {
                    OpremaStaticka.Add(oprema);
                }
                else
                {
                    OpremaDinamicka.Add(oprema);
                }
            }
        }
        #endregion

        #region PrebaciOpremuViewModel
        
        private void OtvoriPrebacivanjeOpreme()
        {
            if (izabraniTab == 0)
            {
                izabranaOprema = IzabranaStaticka;
                prebaciStaticku(izabranaOprema);
            }
            else
            {
                izabranaOprema = IzabranaDinamicka;
                prebaciDinamicku(izabranaOprema);
            }
        }

        private bool ValidnaPoljaZaPrebacivanjeDinamicke()
        {
            if (validacija != null && salaDinamicka != null)
            {
                if (jeBroj(validacija))
                {
                    if (int.Parse(validacija) > dozvoljenaKolicina || int.Parse(validacija) <= 0 || salaDinamicka == null)
                    {
                        return false;
                    }
                    else if (int.Parse(validacija) <= dozvoljenaKolicina && int.Parse(validacija) > 0 && salaDinamicka != null)
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
        private void prebaciStaticku(Oprema izabranaOprema)
        {
            if (izabranaOprema != null)
            {
                prebaciStatickuOpremu(izabranaOprema);
            }
            else
            {
                MessageBox.Show("Morate izabrati opremu!");
            }
        }
        private void prebaciDinamicku(Oprema izabranaOprema)
        {
            if (izabranaOprema != null)
            {
                PrebaciDinamickuProzor = new PrebaciDinamicku(izabranaOprema);
                aktivnaDinamicka = true;
                PrebaciDinamickuProzor.Show();
                dinamicka = izabranaOprema.NazivOpreme;
                MaxDinamicka = "MAX: " + izabranaOprema.Kolicina.ToString();
                dozvoljenaKolicina = izabranaOprema.Kolicina;
                validacija = "";
                dodajSale();
                PrebaciDinamickuProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati opremu!");
            }
        }

        private void ZatvoriPrebacivanjeDinamicke()
        {
            PrebaciDinamickuProzor.Close();
            aktivnaDinamicka = false;
        }

        private void PrebaciDinamickuOpremu()
        {
            PremjestajServis.izvrsiPremjestanje(SalaDinamicka, int.Parse(validacija), izabranaOprema);
            azurirajOpremu();
            aktivnaDinamicka = false;
            PrebaciDinamickuProzor.Close();
        }
        private void DeaktivirajDinamicku()
        {
            aktivnaDinamicka = false;
        }


        private void dodajSale()
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

        #region PrebaciStatickuViewModel
        
        private bool provjeriPreostalo(Oprema izabranaOprema)
        {
            if (nadjiDozvoljenuKolicinu(izabranaOprema) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int nadjiDozvoljenuKolicinu(Oprema izabranaOprema)
        {
            int dozvoljenaKolicina = izabranaOprema.Kolicina;
            foreach (Premjestaj premjestaj in PremjestajServis.Premjestaji())
            {
                if (premjestaj.izSale.Id == 4 && premjestaj.oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    dozvoljenaKolicina -= premjestaj.kolicina;
                }
            }
            return dozvoljenaKolicina;
        }

        private void prebaciStatickuOpremu(Oprema izabranaOprema)
        {
            if (provjeriPreostalo(izabranaOprema))
            {
                PrebaciStatickuProzor = new PrebaciStaticku(izabranaOprema);
                aktivnaStaticka = true;
                PrebaciStatickuProzor.Show();
                nazivStaticke = izabranaOprema.NazivOpreme;
                dozvoljenaKolicinaStaticke = nadjiDozvoljenuKolicinuStaticke();
                MaxStaticka = "MAX: " + dozvoljenaKolicinaStaticke.ToString();
                dodajTerminePocetak();
                dodajSale();
                izabraniDatum = DateTime.Now.Date;
                PrebaciStatickuProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Sva preostala oprema je vec zakazana za transfer");
            }
        }

        private void PromijenioSeDatum()
        {
            if (termini != null)
            {
                if (IzabraniDatum == DateTime.Now.Date)
                {
                    dodajTermineDanas();
                }
                else
                {
                    dodajTermine();
                }
            }
        }

        private void dodajTermineDanas()
        {
            termini.Clear();
            for (int termin = (int)DateTime.Now.Hour + 1; termin <= 23; termin++)
            {
                if (!postojiTermin(termin))
                {
                    termini.Add(termin + ":00");
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

        private void dodajTermine()
        {
            int x = 0;
            string[] t = termini[0].Split(':');
            string prvi = t[0];
            for (int termin = int.Parse(prvi) - 1; termin > 0; termin--)
            {
                if (!postojiTermin(termin))
                {
                    termini.Insert(0, termin + ":00");
                }
            }
        }

        private int nadjiDozvoljenuKolicinuStaticke()
        {
            int kolicina = izabranaOprema.Kolicina;
            foreach (Premjestaj premjestaj in PremjestajServis.Premjestaji())
            {
                if (premjestaj.izSale.Id == 4 && premjestaj.oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    kolicina -= premjestaj.kolicina;
                }
            }
            return kolicina;
        }

        private void dodajTerminePocetak()
        {
            termini = new ObservableCollection<string>();
            for (int termin = (int)DateTime.Now.Hour + 1; termin <= 23; termin++)
            {
                if (!zauzetTermin(termin))
                {
                    termini.Add(termin + ":00");
                }
            }
        }

        private bool zauzetTermin(int termin)
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
        private void PosaljiStaticku() {
            DateTime datumIVrijeme = napraviTerminPremjestaja();
            PremjestajServis.odradiPremjestaj(datumIVrijeme, salaStaticka, int.Parse(validacija), izabranaOprema);
            PrebaciStatickuProzor.Close();
            aktivnaStaticka = false;
        }

        private DateTime napraviTerminPremjestaja()
        {
            DateTime? datumSlanja = izabraniDatum;
            string vrijemeSlanja = termin.ToString();
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

        public void ukloniOpremuIzSkladista(Sala sala, Oprema oprema)
        {
            if (OpremaStaticka != null)
            {
                sala.Oprema.Remove(oprema);
                OpremaStaticka.Remove(oprema);
            }
        }

        public void zamjeniOpremuUSkladistu(Oprema oprema)
        {
            if (OpremaStaticka != null)
            {
                int idx = OpremaStaticka.IndexOf(oprema);
                OpremaStaticka.RemoveAt(idx);
                OpremaStaticka.Insert(idx, oprema);
            }
        }

        public void dodajOpremuUSkladiste(Oprema oprema)
        {
            if (OpremaStaticka != null)
            {
                OpremaStaticka.Add(oprema);
            }
        }

        private bool ValidnaPoljaZaSlanjeStaticke()
        {
            if (validacija != null && salaStaticka != null)
            {
                if (jeBroj(validacija))
                {
                    if (int.Parse(validacija) > dozvoljenaKolicinaStaticke || int.Parse(validacija) <= 0 || salaStaticka == null)
                    {
                        return false;
                    }
                    else if (int.Parse(validacija) <= dozvoljenaKolicinaStaticke && int.Parse(validacija) > 0 && salaStaticka != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void ZatvoriSlanjeStaticke()
        {
            aktivnaStaticka = false;
            PrebaciStatickuProzor.Close();
        }
        private void ZatvoriSlanjeDinamicke()
        {
            aktivnaStaticka = false;
        }
        #endregion 

        #region ObrisiOpremuViewModel
        
        private bool ValidnaPoljaZaBrisanjeOpreme()
        {
            if (unesenaKolicinaZaBrisanje != null)
            {
                if (jeBroj(unesenaKolicinaZaBrisanje))
                {
                    if (int.Parse(unesenaKolicinaZaBrisanje) > int.Parse(nadjiDozvoljenuKolicinuZaBrisanje()) || int.Parse(unesenaKolicinaZaBrisanje) <= 0 || unesenaKolicinaZaBrisanje.Trim().Equals(""))
                    {
                        return false;
                    }
                    else if (int.Parse(unesenaKolicinaZaBrisanje) <= int.Parse(nadjiDozvoljenuKolicinuZaBrisanje()) && int.Parse(unesenaKolicinaZaBrisanje) > 0 && !unesenaKolicinaZaBrisanje.Trim().Equals(""))
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

        private void OdustaniOdBrisanja()
        {
            ObrisiOpremuProzor.Close();
        }

        private void BrisanjeOpreme()
        {
            if(izabraniTab == 0)
            {
                izabranaOprema = izabranaStaticka;
            }
            else if (izabraniTab == 1)
            {
                izabranaOprema = izabranaDinamicka;
            }
            if (izabranaOprema != null)
            {
                obrisiOpremuSkladista(izabranaOprema);
            }
            else
            {
                MessageBox.Show("Morate izabrati opremu!");
            }
        }

        private void obrisiOpremuSkladista(Oprema izabranaOprema)
        {
            if (provjeriPreostaloBrisanje(izabranaOprema))
            {
                ObrisiOpremuProzor = new ObrisiOpremu();
                ObrisiOpremuProzor.Show();
                kolicinaZaBrisanje = "MAX: " + nadjiDozvoljenuKolicinuZaBrisanje();
                ObrisiOpremuProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Nije moguce obrisati opremu, oprema je vec zakazana za transfer");
            }
        }

        private string nadjiDozvoljenuKolicinuZaBrisanje()
        {
            dozvoljenaKolicina = izabranaOprema.Kolicina;
            foreach (Premjestaj premjestaj in PremjestajServis.Premjestaji())
            {
                if (premjestaj.izSale.Id == 4 && premjestaj.oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    dozvoljenaKolicina -= premjestaj.kolicina;
                }
            }
            return dozvoljenaKolicina.ToString();
        }

        private bool provjeriPreostaloBrisanje(Oprema izabranaOprema)
        {
            if (nadjiDozvoljenuKolicinuBrisanje(izabranaOprema) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ObrisiIzabranuOpremu()
        {
            SaleServis.ukloniOpremuIzSale(izabranaOprema, int.Parse(unesenaKolicinaZaBrisanje));
            azurirajOpremu();
            ObrisiOpremuProzor.Close();
        }

        private int nadjiDozvoljenuKolicinuBrisanje(Oprema izabranaOprema)
        {
            int dozvoljenaKolicina = izabranaOprema.Kolicina;
            foreach (Premjestaj pm in PremjestajServis.Premjestaji())
            {
                if (pm.izSale.Id == 4 && pm.oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    dozvoljenaKolicina -= pm.kolicina;
                }
            }
            return dozvoljenaKolicina;
        }
        #endregion

        #region ZatvoriSkladisteViewModel
        private void DeaktivirajSkladiste()
        {
            OpremaServis.sacuvajIzmjene();
            otvoren = false;
        }
        #endregion
    }

    #region ObservableCollectionEx
    public class ObservableCollectionEx<t> : ObservableCollection<t>
    {
        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        public ObservableCollectionEx(IEnumerable<t> collection) : base(collection) { }
        public ObservableCollectionEx(List<t> collection) : base(collection) { }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            using (BlockReentrancy())
            {
                var eventHandler = CollectionChanged;
                if (eventHandler != null)
                {
                    Delegate[] delegates = eventHandler.GetInvocationList();

                    foreach (NotifyCollectionChangedEventHandler handler in delegates)
                    {
                        var dispatcherObject = handler.Target as DispatcherObject;

                        if (dispatcherObject != null && dispatcherObject.CheckAccess() == false)

                            dispatcherObject.Dispatcher.Invoke(DispatcherPriority.DataBind,
                                          handler, this, e);
                        else
                            handler(this, e);
                    }
                }
            }
        }
    }
    #endregion
}
