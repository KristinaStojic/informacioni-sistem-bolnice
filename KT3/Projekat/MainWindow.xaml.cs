using Model;
using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ObservableCollection<Lekar> lekari;
        public static ObservableCollection<Lek> lekovi;
        public static ObservableCollection<Alergeni> alergeni;
        public static ObservableCollection<ZahtevZaLekove> zahtevi;
        public static ObservableCollection<Krevet> kreveti;
        public MainWindow()
        {
            InitializeComponent();
            SaleMenadzer.NadjiSveSale();
            OpremaMenadzer.NadjiSvuOpremu();
            LekoviMenadzer.NadjiSveLijekove();
            PremjestajMenadzer.NadjiSvePremjestaje();
            TerminServis.NadjiSveTermine();
            PacijentiMenadzer.PronadjiSve();
            ObavestenjaServis.NadjiSvaObavestenja();
            LekoviMenadzer.NadjiSveZahteve();
            LekariMenadzer.NadjiSveZahteve();
            LekariMenadzer.NadjiSveLekare();

            lekari = new ObservableCollection<Lekar>();
            lekari.Add(new Lekar() {IdLekara = 1, ImeLek = "Petar", PrezimeLek = "Nebojsic", specijalizacija = Specijalizacija.Opsta_praksa }) ;
            lekari.Add(new Lekar() {IdLekara = 2, ImeLek = "Milos", PrezimeLek = "Dragojevic", specijalizacija = Specijalizacija.Opsta_praksa });
            lekari.Add(new Lekar() {IdLekara = 3, ImeLek = "Petar", PrezimeLek = "Milosevic", specijalizacija = Specijalizacija.Specijalista });
            lekari.Add(new Lekar() {IdLekara = 4, ImeLek = "Dejan", PrezimeLek = "Milosevic", specijalizacija = Specijalizacija.Specijalista });
            lekari.Add(new Lekar() {IdLekara = 5, ImeLek = "Isidora", PrezimeLek = "Isidorovic", specijalizacija = Specijalizacija.Specijalista });
            lekari.Add(new Lekar() {IdLekara = 6, ImeLek = "Jagoda", PrezimeLek = "Jagodic", specijalizacija = Specijalizacija.Ortopedija });
            lekari.Add(new Lekar() {IdLekara = 7, ImeLek = "Jovana", PrezimeLek = "Jovanovic", specijalizacija = Specijalizacija.Akuserstvo });
            lekari.Add(new Lekar() { IdLekara = 8, ImeLek = "Ivan", PrezimeLek = "Ivanovic", specijalizacija = Specijalizacija.Hirurgija });
            lekari.Add(new Lekar() { IdLekara = 9, ImeLek = "Igor", PrezimeLek = "Ivanovic", specijalizacija = Specijalizacija.Opsta_praksa });


            lekovi = new ObservableCollection<Lek>();
            lekovi.Add(new Lek(1, "Paracetamol", "P2L"));
            lekovi.Add(new Lek(2, "Brufen", "B1E"));
            lekovi.Add(new Lek(3, "Pentraxil", "R24"));
            lekovi.Add(new Lek(4, "Andol", "M4M"));
            lekovi.Add(new Lek(5, "Sterpsils", "K5S"));

            alergeni = new ObservableCollection<Alergeni>();
            alergeni.Add(new Alergeni(1, "Paracetamol", "P2L"));
            alergeni.Add(new Alergeni(2, "Brufen", "B1E"));
            alergeni.Add(new Alergeni(3, "Pentraxil", "R24"));
            alergeni.Add(new Alergeni(4, "Andol", "M4M"));
            alergeni.Add(new Alergeni(5, "Sterpsils", "K5S"));

            zahtevi = new ObservableCollection<ZahtevZaLekove>();
            zahtevi.Add(new ZahtevZaLekove(1, "Tylolhot", "T32", "11/04/2021", false));
            zahtevi.Add(new ZahtevZaLekove(2, "Vitamic C", "VC4", "10/04/2021", false));
            zahtevi.Add(new ZahtevZaLekove(3, "Panklav", "PKL", "12/04/2021", false));

            kreveti = new ObservableCollection<Krevet>();
            kreveti.Add(new Krevet(1, 6, false));
            kreveti.Add(new Krevet(2, 6, true));
            kreveti.Add(new Krevet(3, 6, false));
            kreveti.Add(new Krevet(4, 6, false));
            kreveti.Add(new Krevet(5, 5, true));
            kreveti.Add(new Krevet(6, 5, true));
            kreveti.Add(new Krevet(7, 5, false));
            kreveti.Add(new Krevet(8, 5, false));

            foreach(Krevet k in kreveti)
            {
                Console.WriteLine(k.IdKreveta + " " + k.IdSobe + " " + k.Zauzet);
                foreach(Sala s in SaleMenadzer.sale)
                {
                    if(k.IdSobe == s.Id)
                    {
                        s.Kreveti.Add(k);
                    }
                }
            }

            

        }

        //TODO: prebaaci u LekariMenadzer-u
        public static Lekar PronadjiPoId(int id)
        {
            foreach (Lekar p in lekari)
            {
                if (p.IdLekara == id)
                {
                    return p;
                }
            }
            return null;

        }

        public static List<Lekar> PronadjiLekarePoSpecijalizaciji(Specijalizacija oblastSpecijalizacije)
        {
            List<Lekar> specijalizovaniLekari = new List<Lekar>();

            foreach (Lekar lekar in lekari)
            {
                if (lekar.specijalizacija.Equals(oblastSpecijalizacije))
                {
                    specijalizovaniLekari.Add(lekar);
                }
            }
            return specijalizovaniLekari;
        }
        //------------------------------------------------------------------------------------

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Upravnik w1 = new Upravnik();
            w1.Show();
            //this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            //PocetnaStrana w1 = new PocetnaStrana();
            PrijavaLekar w1 = new PrijavaLekar();
            w1.Show();
            //this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //PrikaziTermin w1 = new PrikaziTermin();
            MainWindowPacijent w1 = new MainWindowPacijent();
            w1.Show();
            //this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
             Sekretar s = new Sekretar();
             s.Show();
           // this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.U && Keyboard.IsKeyDown(Key.LeftCtrl)) //upravnik
            {
                Button_Click(sender, e);
            }
            else if (e.Key == Key.L && Keyboard.IsKeyDown(Key.LeftCtrl)) //lekar
            {
                Button_Click_1(sender, e);
            }
            else if (e.Key == Key.P && Keyboard.IsKeyDown(Key.LeftCtrl)) //pacijent
            {
                Button_Click_2(sender, e);
            }
            /*else if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl)) //sekretar
            {
                Button_Click_3(sender, e);
            }*/
        }
    }
}
