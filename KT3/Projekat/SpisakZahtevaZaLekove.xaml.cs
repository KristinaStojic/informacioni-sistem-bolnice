using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for SpisakZahtevaZaLekove.xaml
    /// </summary>
    public partial class SpisakZahtevaZaLekove : Window
    {

        public static ObservableCollection<ZahtevZaLekove> TabelaZahteva
        {
            get;
            set;
        }
        public SpisakZahtevaZaLekove()
        {
            InitializeComponent();
            this.DataContext = this;
            TabelaZahteva = new ObservableCollection<ZahtevZaLekove>();
            TabelaZahteva = MainWindow.zahtevi;
            foreach (ZahtevZaLekove zahtev in LekoviMenadzer.zahteviZaLekove)
            {
                TabelaZahteva.Add(zahtev);
            }

           

        }

        private void Button_Obradi(object sender, RoutedEventArgs e)
        {
            ZahtevZaLekove izabraniZahtev = (ZahtevZaLekove)dataGridZahtevi.SelectedItem;
            if(izabraniZahtev != null)
            {
                ObradiZahtevZaLek obradiZahtev = new ObradiZahtevZaLek(izabraniZahtev);
                obradiZahtev.Show();

            }
            else
            {
                MessageBox.Show("Niste selektovali zahtev koji zelite da obradite!");
            }

        }
        
        private void Button_Nazad(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
