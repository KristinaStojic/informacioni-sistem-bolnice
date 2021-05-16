using Model;
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
    /// Interaction logic for OdobravanjeGodisnjegOdmora.xaml
    /// </summary>
    public partial class OdobravanjeGodisnjegOdmora : Window
    {
        public static ObservableCollection<ZahtevZaGodisnji> TabelaZahteva
        {
            get;
            set;
        }
        public OdobravanjeGodisnjegOdmora()
        {
            InitializeComponent();
            this.DataContext = this;
            dodajZahteveUTabelu();
        }

        private void dodajZahteveUTabelu()
        {
            TabelaZahteva = new ObservableCollection<ZahtevZaGodisnji>();
            /*foreach (Lekar lekar in LekariMenadzer.lekari)
            {
                
                    foreach (ZahtevZaGodisnji zahtev in lekar.zahteviZaOdmor)
                    {
                        TabelaZahteva.Add(zahtev);

                    }
                
            }*/

            foreach(ZahtevZaGodisnji zahtev in LekariMenadzer.zahtevi)
            {
                TabelaZahteva.Add(zahtev);
            }
        }
        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TabelaLekara_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
