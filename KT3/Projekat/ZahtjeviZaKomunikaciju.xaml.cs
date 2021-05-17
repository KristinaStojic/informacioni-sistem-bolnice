using Model;
using Projekat.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for ZahtjeviZaKomunikaciju.xaml
    /// </summary>
    public partial class ZahtjeviZaKomunikaciju : Window
    {
        private int colNum = 0;
        public static ObservableCollection<ZahtjevZaKomunikaciju> Zahtjevi{get; set;}

        public ZahtjeviZaKomunikaciju()
        {
            InitializeComponent();
            inicijalizujZahtjeve();
        }

        private void inicijalizujZahtjeve()
        {
            this.DataContext = this;
            napraviZahtjeve();
        }

        private void napraviZahtjeve()
        {
            Zahtjevi = new ObservableCollection<ZahtjevZaKomunikaciju>();
            Zahtjevi.Add(new ZahtjevZaKomunikaciju("ZDRAVO", "Beograd", "Transfer materijala", dodajOpremu(), null));
            Zahtjevi.Add(new ZahtjevZaKomunikaciju("ZDRAVO", "Beograd", "Transfer osoblja", null, dodajOsoblje()));
            Zahtjevi.Add(new ZahtjevZaKomunikaciju("ZDRAVO", "Sarajevo", "Transfer materijala", dodajOpremu(), null));
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

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.N)
                {
                    Odustani_Click(sender, e);
                }
            }
        }

        private void UvidUZahtjeve_Click(object sender, RoutedEventArgs e)
        {
            ZahtjevZaKomunikaciju zahtjev = (ZahtjevZaKomunikaciju)dataGridKomunikacija.SelectedItem;
            if(zahtjev == null)
            {
                MessageBox.Show("Morate izabrati zahtjev!");    
            }
            else if(zahtjev.oprema != null)
            {
                UvidUZahtjevKomunikacija uvidUZahtjev = new UvidUZahtjevKomunikacija(zahtjev);
                uvidUZahtjev.Show();
            }else if(zahtjev.osoblje != null)
            {
                UvidUZahtjevOsoblje uvidUZahtjev = new UvidUZahtjevOsoblje(zahtjev);
                uvidUZahtjev.Show();
            }
        }
    }

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
}

