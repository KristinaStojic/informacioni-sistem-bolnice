using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Projekat.ViewModel
{
    public class KomunikacijaViewModel : BindableBase
    {
        #region KomunikacijaViewModel
        public MyICommand UviduUZahtjeveKomanda { get; set; }
        public MyICommand ZatvoriKomunikaciju { get; set; }
        public MyICommand ZatvoriOsoblje { get; set; }
        public MyICommand ZatvoriZahtjev { get; set; }
        private ZahtjevZaKomunikaciju izabraniZahtjev;
        private string textOprema;
        private string textOsoblje;
        public string TextOprema { get { return textOprema; } set { textOprema = value; OnPropertyChanged("TextOprema"); } }
        public string TextOsoblje{ get { return textOsoblje; } set { textOsoblje = value; OnPropertyChanged("TextOsoblje"); } }
        public ZahtjevZaKomunikaciju IzabraniZahtjev { get { return izabraniZahtjev; } set { izabraniZahtjev = value; OnPropertyChanged("IzabraniZahtjev"); } }
        private ObservableCollection<Oprema> zahtjeviOprema;
        private ObservableCollection<Osoblje> zahtjeviOsoblje;
        public ObservableCollection<Oprema> ZahtjeviOprema { get { return zahtjeviOprema; } set { zahtjeviOprema = value;OnPropertyChanged("ZahtjeviOprema"); } }
        public ObservableCollection<Osoblje> ZahtjeviOsoblje{ get { return zahtjeviOsoblje; } set { zahtjeviOsoblje = value;OnPropertyChanged("ZahtjeviOsoblje"); } }
        public Window ZahtjeviProzor { get; set; }
        public Window ZahtjevOpremaProzor { get; set; }
        public Window ZahtjevOsobljeProzor { get; set; }
        public static Window KomunikacijaProzor { get; set; }
        public KomunikacijaViewModel()
        {
            UviduUZahtjeveKomanda = new MyICommand(UvidUZahtjeve);
            ZatvoriZahtjeve = new MyICommand(ZatvoriZahtjeveKomunikacije);
            UvidUZahtjev = new MyICommand(PrikaziZahtjev);
            ZatvoriOpremu = new MyICommand(ZatvoriZahtjeveOpreme);
            UkloniZahtjev = new MyICommand(ObrisiOprema);
            ZatvoriOsoblje = new MyICommand(ZatvoriZahtjeveOsoblje);
            ZatvoriZahtjev = new MyICommand(UkloniOsoblje);
            ZatvoriKomunikaciju = new MyICommand(ZatvoriProzor);
            OtvoriSale = new MyICommand(PrikaziSale);
            OtvoriZahtjeve = new MyICommand(PrikaziZahtjeve);
        }
        public MyICommand OtvoriSale { get; set; }
        public MyICommand OtvoriZahtjeve { get; set; }
        private void PrikaziZahtjeve()
        {
            ZahtjeviViewModel.ZahtjeviProzor = new Zahtjevi();
            ZahtjeviViewModel.ZahtjeviProzor.Show();
            ZahtjeviViewModel.ZahtjeviProzor.DataContext = new ZahtjeviViewModel();
            KomunikacijaProzor.Close();
        }
        private void PrikaziSale()
        {
            SaleViewModel.SaleProzor = new PrikaziSalu();
            SaleViewModel.SaleProzor.Show();
            SaleViewModel.SaleProzor.DataContext = new SaleViewModel();
            KomunikacijaProzor.Close();
        }
        private void ZatvoriProzor()
        {
            UpravnikViewModel.UpravnikProzor = new Upravnik();
            UpravnikViewModel.UpravnikProzor.Show();
            UpravnikViewModel.UpravnikProzor.DataContext = new UpravnikViewModel();
            KomunikacijaProzor.Close();
        }
        private void UvidUZahtjeve()
        {
            ZahtjeviProzor = new ZahtjeviZaKomunikaciju();
            ZahtjeviProzor.Show();
            dodajZahtjeve();
            ZahtjeviProzor.DataContext = this;
        }
        #endregion
        #region ZahtjeviZaKomunikacijuViewModel
        public MyICommand ZatvoriZahtjeve { get; set; }

        private ObservableCollection<ZahtjevZaKomunikaciju> zahtjeviZaKomunikaciju;
        public ObservableCollection<ZahtjevZaKomunikaciju> ZahtjeviZaKomunikaciju { get { return zahtjeviZaKomunikaciju; } set { zahtjeviZaKomunikaciju = value; OnPropertyChanged("ZahtjeviZaKomunikaciju"); } }
        private void dodajZahtjeve()
        {
            ZahtjeviZaKomunikaciju = new ObservableCollection<ZahtjevZaKomunikaciju>();
            ZahtjeviZaKomunikaciju.Add(new ZahtjevZaKomunikaciju("ZDRAVO", "Beograd", "Transfer materijala", dodajOpremu(), null));
            ZahtjeviZaKomunikaciju.Add(new ZahtjevZaKomunikaciju("ZDRAVO", "Beograd", "Transfer osoblja", null, dodajOsoblje()));
            ZahtjeviZaKomunikaciju.Add(new ZahtjevZaKomunikaciju("ZDRAVO", "Sarajevo", "Transfer materijala", dodajOpremu(), null));
        }
        private List<Osoblje> dodajOsoblje()
        {
            List<Osoblje> osoblje = new List<Osoblje>();
            osoblje.Add(new Osoblje("ljekar", 3, "opsta praksa"));
            osoblje.Add(new Osoblje("ljekar", 3, "hirurg"));
            osoblje.Add(new Osoblje("ljekar", 3, "hirurg"));
            return osoblje;
        }

        private List<Oprema> dodajOpremu()
        {
            List<Oprema> oprema = new List<Oprema>();
            oprema.Add(new Oprema("operacioni sto", 2, true));
            oprema.Add(new Oprema("monitor vitalnih funkcija", 3, true));
            oprema.Add(new Oprema("maska", 100, false));
            return oprema;
        }

        private void ZatvoriZahtjeveKomunikacije()
        {
            ZahtjeviProzor.Close();
        }

        #endregion
        #region  UvidUZahtjevViewModel
        public MyICommand UvidUZahtjev { get; set; }
        public MyICommand ZatvoriOpremu { get; set; }
        public MyICommand UkloniZahtjev { get; set; }

        private void PrikaziZahtjev()
        {
            if (izabraniZahtjev == null)
            {
                MessageBox.Show("Morate izabrati zahtjev!");
            }
            else if (izabraniZahtjev.oprema != null)
            {
                ZahtjevOpremaProzor = new UvidUZahtjevKomunikacija();
                ZahtjevOpremaProzor.Show();
                textOprema = "Zahtjev ustanove " + izabraniZahtjev.nazivUstanove + ", " + izabraniZahtjev.sjedisteUstanove;
                dodajZahtjeveOprema();
                ZahtjevOpremaProzor.DataContext = this;
            }
            else if (izabraniZahtjev.osoblje != null)
            {
                ZahtjevOsobljeProzor = new UvidUZahtjevOsoblje();
                ZahtjevOsobljeProzor.Show();
                textOsoblje = "Zahtjev ustanove " + izabraniZahtjev.nazivUstanove + ", " + izabraniZahtjev.sjedisteUstanove;
                dodajZahtjeveOsoblje();
                ZahtjevOsobljeProzor.DataContext = this;
            }
        }
        private void dodajZahtjeveOsoblje()
        {
            ZahtjeviOsoblje = new ObservableCollection<Osoblje>();
            foreach (Osoblje osoblje in izabraniZahtjev.osoblje)
            {
                ZahtjeviOsoblje.Add(osoblje);
            }
        }
        private void UkloniOsoblje()
        {
            ZahtjevOsobljeProzor.Close();
            ZahtjeviZaKomunikaciju.Remove(IzabraniZahtjev);
        }
        private void ZatvoriZahtjeveOsoblje()
        {
            ZahtjevOsobljeProzor.Close();
        }
        private void dodajZahtjeveOprema()
        {
            ZahtjeviOprema = new ObservableCollection<Oprema>();
            if (izabraniZahtjev.oprema != null)
            {
                foreach (Oprema oprema in izabraniZahtjev.oprema)
                {
                    ZahtjeviOprema.Add(oprema);
                }
            }
        }
        private void ObrisiOprema()
        {
            ZahtjeviZaKomunikaciju.Remove(IzabraniZahtjev);
            ZahtjevOpremaProzor.Close();
        }
        private void ZatvoriZahtjeveOpreme()
        {
            ZahtjevOpremaProzor.Close();
        }
        #endregion
    }

    #region ZahtjeviKlasa
    public class ZahtjevZaKomunikaciju
    {
        public string nazivUstanove { get; set; }
        public string sjedisteUstanove { get; set; }
        public string tipZahtjeva { get; set; }
        public List<Oprema> oprema { get; set; }
        public List<Osoblje> osoblje { get; set; }

        public ZahtjevZaKomunikaciju(string nazivUstanove, string sjedisteUstanove, string tipZahtjeva, List<Oprema> oprema, List<Osoblje> osoblje)
        {
            this.nazivUstanove = nazivUstanove;
            this.sjedisteUstanove = sjedisteUstanove;
            this.tipZahtjeva = tipZahtjeva;
            this.oprema = oprema;
            this.osoblje = osoblje;
        }
    }

    public class Osoblje
    {
        public string nazivOsoblja { get; set; }
        public int kolicina { get; set; }
        public string specijalizacija { get; set; }

        public Osoblje(string nazivOsoblja, int kolicina, string specijalizacija)
        {
            this.nazivOsoblja = nazivOsoblja;
            this.kolicina = kolicina;
            this.specijalizacija = specijalizacija;
        }
    }
    #endregion
}
