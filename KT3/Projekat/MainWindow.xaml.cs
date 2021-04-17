using Model;
using Projekat.Model;
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
        public MainWindow()
        {
            InitializeComponent();
            SaleMenadzer.NadjiSveSale();
            OpremaMenadzer.NadjiSvuOpremu();

            PremjestajMenadzer.NadjiSvePremjestaje();

            TerminMenadzer.NadjiSveTermine();
            PacijentiMenadzer.PronadjiSve();
            ObavestenjaMenadzer.NadjiSvaObavestenja();

            lekari = new ObservableCollection<Lekar>();
            lekari.Add(new Lekar() {IdLekara = 1, ImeLek = "Petar", PrezimeLek = "Nebojsic", specijalizacija = Specijalizacija.Opsta_praksa }) ;
            lekari.Add(new Lekar() {IdLekara = 2, ImeLek = "Milos", PrezimeLek = "Dragojevic", specijalizacija = Specijalizacija.Opsta_praksa });
            lekari.Add(new Lekar() {IdLekara = 3, ImeLek = "Petar", PrezimeLek = "Milosevic", specijalizacija = Specijalizacija.Specijalista });
            lekari.Add(new Lekar() {IdLekara = 4, ImeLek = "Dejan", PrezimeLek = "Milosevic", specijalizacija = Specijalizacija.Specijalista });
            lekari.Add(new Lekar() {IdLekara = 5, ImeLek = "Isidora", PrezimeLek = "Isidorovic", specijalizacija = Specijalizacija.Specijalista });

        }
    
        // dok nemamo lekari menazder
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Upravnik w1 = new Upravnik();
            w1.Show();
            //this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //PrikaziTermin w1 = new PrikaziTermin();
            PrikazTerminaLekar w1 = new PrikazTerminaLekar();
            w1.Show();
            //this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            PrikaziTermin w1 = new PrikaziTermin();
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
    }
}
