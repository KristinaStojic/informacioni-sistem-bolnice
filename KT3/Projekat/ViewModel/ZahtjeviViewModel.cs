using Projekat.Model;
using Projekat.Pomoc;
using Projekat.Servis;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Projekat.ViewModel
{
    public class ZahtjeviViewModel : BindableBase
    {
        #region Promjenljive
        public MyICommand LijekoviProzor { get; set; }
        public MyICommand SkladisteKomanda { get; set; }
        public MyICommand ZatvoriZahtjeveKomanda { get; set; }
        public MyICommand OtvoriSale { get; set; }
        public MyICommand OtvoriIzvjestaj { get; set; }
        public MyICommand ObracunPlate { get; set; }
        public MyICommand ZatvoriFormulu { get; set; }
        public MyICommand UnesenaFormula { get; set; }
        public MyICommand ZatvoriObracunPlate { get; set; }
        public MyICommand OtvoriProstorije { get; set; }
        public MyICommand OtvoriKomunikacijuPlata { get; set; }
        public MyICommand OtvoriIzvjestajPlate { get; set; }
        public MyICommand OtvoriOpis { get; set; }
        public MyICommand OtvoriPomocPlate { get; set; }
        public MyICommand OtvoriKomunikaciju { get; set; }
        public MyICommand OtvoriOAplikaciji { get; set; }
        public MyICommand OtvoriEvidenciju { get; set; }
        public MyICommand Evidentiraj { get; set; }
        public MyICommand PrikaziSaleEvidencija { get; set; }
        public MyICommand PrikaziKomunikacijuEvidencija { get; set; }
        public MyICommand PrikaziIzvjestajEvidencija { get; set; }
        public MyICommand OtvoriOpisEvidencija { get; set; }
        public MyICommand OtvoriPomocEvidencija { get; set; }
        public MyICommand OtvoriPomoc { get; set; }
        public MyICommand ZatvoriEvidenciju { get; set; }
        public static Window ZahtjeviProzor { get; set; }
        public static Window PomocProzor { get; set; }
        public Window FormulaProzor { get; set; }
        public Window EvidencijaProzor { get; set; }
        public Window PomocEvidencijaProzor { get; set; }
        public Window PlateProzor { get; set; }
        public Window PlatePomocProzor { get; set; }

        private string formulaPlate;
        public string FormulaPlate { get { return formulaPlate; } set { formulaPlate = value; OnPropertyChanged("FormulaPlate"); UnesenaFormula.RaiseCanExecuteChanged(); } }

        private Oprema izabranaOprema;
        public Oprema IzabranaOprema { get { return izabranaOprema; } set { izabranaOprema = value; OnPropertyChanged("IzabranaOprema"); } }

        private ObservableCollection<Plata> plate;
        public ObservableCollection<Plata> Plate { get { return plate; } set { plate = value; OnPropertyChanged("Plate"); } }
       
        private ObservableCollection<Oprema> utrosenaOprema;
        public ObservableCollection<Oprema> UtrosenaOprema { get { return utrosenaOprema; } set { utrosenaOprema = value; OnPropertyChanged("UtrosenaOprema"); } }

        #endregion

        #region Konstruktor
        public ZahtjeviViewModel()
        {
            LijekoviProzor = new MyICommand(OtvoriLijekove);
            SkladisteKomanda = new MyICommand(OtvoriSkladiste);
            ZatvoriZahtjeveKomanda = new MyICommand(ZatvoriZahtjeve);
            OtvoriSale = new MyICommand(PrikaziSale);
            OtvoriKomunikaciju = new MyICommand(PrikaziKomunikaciju);
            OtvoriOAplikaciji = new MyICommand(PrikaziOpis);
            OtvoriIzvjestaj = new MyICommand(OtvaranjeIzvjestaja);
            ObracunPlate = new MyICommand(OtvoriObracunPlate);
            ZatvoriFormulu = new MyICommand(ZatvoriUnosFormule);
            UnesenaFormula = new MyICommand(PrikaziPlate, ValidnoUnesenaFormula);
            ZatvoriObracunPlate = new MyICommand(ZatvoriPlate);
            OtvoriProstorije = new MyICommand(PrikazProstorija);
            OtvoriKomunikacijuPlata = new MyICommand(PrikaziKomunikacijuPlate);
            OtvoriIzvjestajPlate = new MyICommand(IzvjestajPrikaz);
            OtvoriOpis = new MyICommand(PrikaziOpisAplikacije);
            ZatvoriEvidenciju = new MyICommand(ZatvoriIzmjenuSkladista);
            OtvoriEvidenciju = new MyICommand(PrikaziUtroseno);
            Evidentiraj = new MyICommand(EvidentirajUtroseno);
            PrikaziSaleEvidencija = new MyICommand(OtvoriSaleEvidencija);
            PrikaziKomunikacijuEvidencija = new MyICommand(OtvoriKomunikacijuEvidencija);
            PrikaziIzvjestajEvidencija = new MyICommand(OtvoriIzvjestajEvidencija);
            OtvoriOpisEvidencija = new MyICommand(OtvoriOAplikacijiEvidencija);
            OtvoriPomocEvidencija = new MyICommand(PomocEvidencija);
            OtvoriPomoc = new MyICommand(PrikaziPomoc);
            OtvoriPomocPlate = new MyICommand(PrikaziPomocPlate);
        }
        #endregion

        #region ZahtjeviViewModel

        private void PrikaziPomoc()
        {
            PomocProzor = new ZahtjeviPomoc();
            PomocProzor.Show();
            PomocProzor.DataContext = this;
        }
        private void PrikaziUtroseno()
        {
            EvidencijaProzor = new EvidencijaSkladista();
            EvidencijaProzor.Show();
            UtrosenaOprema = new ObservableCollection<Oprema>();
            UtrosenaOprema.Add(new Oprema("flaster", 10, false));
            UtrosenaOprema.Add(new Oprema("maska", 30, false));
            UtrosenaOprema.Add(new Oprema("rukavice", 20, false));
            EvidencijaProzor.DataContext = this;
            ZahtjeviProzor.Close();
        }
        private void PrikaziOpis()
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
            ZahtjeviProzor.Close();
        }
        private void PrikaziSale()
        {
            SaleViewModel.SaleProzor = new PrikaziSalu();
            SaleViewModel.SaleProzor.Show();
            SaleViewModel.SaleProzor.DataContext = new SaleViewModel();
            ZahtjeviProzor.Close();
        }
        private void ZatvoriZahtjeve()
        {
            UpravnikViewModel.UpravnikProzor = new Upravnik();
            UpravnikViewModel.UpravnikProzor.Show();
            UpravnikViewModel.UpravnikProzor.DataContext = new UpravnikViewModel();
            ZahtjeviProzor.Close();
        }
        #endregion

        #region LijekoviViewModel
        private void OtvoriLijekove()
        {
            LijekoviViewModel.LijekoviProzor = new Lijekovi();
            LijekoviViewModel.LijekoviProzor.Show();
            LijekoviViewModel.LijekoviProzor.DataContext = new LijekoviViewModel();
            ZahtjeviProzor.Close();
        }
        #endregion

        #region SkladisteViewModel
        private void OtvoriSkladiste()
        {
            try
            {
                SkladisteViewModel.SkladisteProzor = new Skladiste();
                SkladisteViewModel.SkladisteProzor.Show();
                SkladisteViewModel.otvoren = true;
                PremjestajServis.odradiZakazanePremjestaje();
                SkladisteViewModel.SkladisteProzor.DataContext = new SkladisteViewModel();
                ZahtjeviProzor.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Data); }
        }
        #endregion

        #region OtvoriIzvjestajViewModel

        private void OtvaranjeIzvjestaja()
        {
            IzvjestajViewModel.IzvjestajProzor = new Izvjestaj();
            IzvjestajViewModel.IzvjestajProzor.Show();
            IzvjestajViewModel.IzvjestajProzor.DataContext = new IzvjestajViewModel();
            ZahtjeviProzor.Close();
        }

        #endregion

        #region ObracunPlateViewModel
        private void PrikaziPomocPlate()
        {
            PlatePomocProzor = new PlatePomoc();
            PlatePomocProzor.Show();
            PlatePomocProzor.DataContext = new PomocViewModel();
        }
        private void OtvoriObracunPlate()
        {
            FormulaProzor = new FormulaZaObracunPlate();
            FormulaProzor.Show();
            FormulaProzor.DataContext = this;
        }
        private void IzvjestajPrikaz()
        {
            IzvjestajViewModel.IzvjestajProzor = new Izvjestaj();
            IzvjestajViewModel.IzvjestajProzor.Show();
            IzvjestajViewModel.IzvjestajProzor.DataContext = new IzvjestajViewModel();
            PlateProzor.Close();
        }
        private void ZatvoriUnosFormule()
        {
            FormulaProzor.Close();
        }
        private void PrikaziKomunikacijuPlate()
        {
            KomunikacijaViewModel.KomunikacijaProzor = new Komunikacija();
            KomunikacijaViewModel.KomunikacijaProzor.Show();
            KomunikacijaViewModel.KomunikacijaProzor.DataContext = new KomunikacijaViewModel();
            PlateProzor.Close();
        }
        private void PrikaziOpisAplikacije()
        {
            OAplikacijiViewModel.OAplikacijiProzor = new OAplikaciji();
            OAplikacijiViewModel.OAplikacijiProzor.Show();
            OAplikacijiViewModel.OAplikacijiProzor.DataContext = new OAplikacijiViewModel();
        }
        private void PrikazProstorija()
        {
            SaleViewModel.SaleProzor = new PrikaziSalu();
            SaleViewModel.SaleProzor.Show();
            SaleViewModel.SaleProzor.DataContext = new SaleViewModel();
            PlateProzor.Close();
        }
        private void PrikaziPlate()
        {
            PlateProzor = new ObracunataPlata();
            PlateProzor.Show();
            Plate = new ObservableCollection<Plata>();
            Plate.Add(new Plata("Marko", "Markovic", 100000));
            Plate.Add(new Plata("Ana", "Maric", 150000));
            Plate.Add(new Plata("Dimitrije", "Dimitrijevic", 150000));
            PlateProzor.DataContext = this;
            FormulaProzor.Close();
            ZahtjeviProzor.Close();
        }
        private void ZatvoriPlate()
        {
            PlateProzor.Close();
            ZahtjeviProzor = new Zahtjevi();
            ZahtjeviProzor.Show();
            ZahtjeviProzor.DataContext = this;
        }

        private bool ValidnoUnesenaFormula()
        {
            if (formulaPlate != null)
            {
                if (formulaPlate.Trim().Equals(""))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region EvidencijaUtrosenogMaterijalaViewModel
         
        private void OtvoriKomunikacijuEvidencija()
        {
            KomunikacijaViewModel.KomunikacijaProzor = new Komunikacija();
            KomunikacijaViewModel.KomunikacijaProzor.Show();
            KomunikacijaViewModel.KomunikacijaProzor.DataContext = new KomunikacijaViewModel();
            EvidencijaProzor.Close();
        }
        private void PomocEvidencija()
        {
            PomocEvidencijaProzor = new EvidencijaPomoc();
            PomocEvidencijaProzor.Show();
            PomocEvidencijaProzor.DataContext = new PomocViewModel();
        }
        private void OtvoriOAplikacijiEvidencija()
        {
            OAplikacijiViewModel.OAplikacijiProzor = new OAplikaciji();
            OAplikacijiViewModel.OAplikacijiProzor.Show();
            OAplikacijiViewModel.OAplikacijiProzor.DataContext = new OAplikacijiViewModel();
        }
        private void ZatvoriIzmjenuSkladista()
        {
            EvidencijaProzor.Close();
            ZahtjeviProzor = new Zahtjevi();
            ZahtjeviProzor.Show();
            ZahtjeviProzor.DataContext = this;
        }
        private void OtvoriIzvjestajEvidencija()
        {
            IzvjestajViewModel.IzvjestajProzor = new Izvjestaj();
            IzvjestajViewModel.IzvjestajProzor.Show();
            IzvjestajViewModel.IzvjestajProzor.DataContext = new IzvjestajViewModel();
            EvidencijaProzor.Close();
        }
        private void OtvoriSaleEvidencija()
        {
            SaleViewModel.SaleProzor = new PrikaziSalu();
            SaleViewModel.SaleProzor.Show();
            SaleViewModel.SaleProzor.DataContext = new SaleViewModel();
            EvidencijaProzor.Close();
        }
        private void EvidentirajUtroseno()
        {
            if(izabranaOprema != null)
            {
                SkladisteServis.EvidentirajUtrosenuOpremu(izabranaOprema);
                UtrosenaOprema.Remove(IzabranaOprema);
            }
            else
            {
                MessageBox.Show("Morate izabrati opremu!");
            }
        }
        #endregion

    }
    #region PlatePodaci
    public class Plata
    {
        public string ime { get; set; }
        public string prezime { get; set; }
        public double plata { get; set; }
        public Plata(){ }
        public Plata(string ime, string prezime, double plata){
            this.ime = ime;
            this.prezime = prezime;
            this.plata = plata;
        }
    }
    #endregion
}
