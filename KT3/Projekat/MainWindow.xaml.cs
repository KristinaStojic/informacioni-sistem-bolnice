using Model;
using Projekat.Model;
using Projekat.Servis;
using Projekat.ViewModel;
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
       // LekariServis lekariServis = new LekariServis();
        PacijentiServis pacijentiServis = new PacijentiServis();
       // ZahteviZaGodisnjiMenadzer menadzer = new ZahteviZaGodisnjiMenadzer();
        ObavestenjaServis servis = new ObavestenjaServis();


        public MainWindow()
        {
            InitializeComponent();
            TerminServis.NadjiSveTermine();
            SaleServis.NadjiSveSale();
            OpremaServis.NadjiSvuOpremu();
            LekoviServis.NadjiSveLijekove();
            PremjestajServis.NadjiSvePremjestaje();
            LekoviServis.NadjiSveZahteve();
           
            //lekariServis.NadjiSveLekare();
            pacijentiServis.pacijenti();
            //menadzer.NadjiSveZahteve();
            servis.NadjiSvaObavestenja();

            lekovi = new ObservableCollection<Lek>();
            lekovi.Add(new Lek(1, "Paracetamol", "P2L"));
            lekovi.Add(new Lek(2, "Brufen", "B1E"));
            lekovi.Add(new Lek(3, "Pentraxil", "R24"));
            lekovi.Add(new Lek(4, "Andol", "M4M"));
            lekovi.Add(new Lek(5, "Sterpsils", "K5S"));

            zahtevi = new ObservableCollection<ZahtevZaLekove>();
            zahtevi.Add(new ZahtevZaLekove(1, "Tylolhot", "T32", "11/04/2021", false));
            zahtevi.Add(new ZahtevZaLekove(2, "Vitamic C", "VC4", "10/04/2021", false));
            zahtevi.Add(new ZahtevZaLekove(3, "Panklav", "PKL", "12/04/2021", false));

            kreveti = new ObservableCollection<Krevet>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*UpravnikViewModel.UpravnikProzor = new Upravnik();
            UpravnikViewModel.UpravnikProzor.Show();
            UpravnikViewModel.UpravnikProzor.DataContext = new UpravnikViewModel();*/
            UpravnikViewModel.PrijavaProzor = new UpravnikPrijava();
            UpravnikViewModel.PrijavaProzor.Show();
            UpravnikViewModel.PrijavaProzor.DataContext = new UpravnikViewModel();
            this.Hide();

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
             SekretarPrijava s = new SekretarPrijava();
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
            else if ( (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl)) || (e.Key == Key.S && Keyboard.IsKeyDown(Key.RightCtrl)) ) //sekretar
            {
                Button_Click_3(sender, e);
            }
        }
    }
}
