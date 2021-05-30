using Model;
using Projekat.Model;
using Projekat.Servis;
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
            LekariServis.DodajZahteveUTabelu();
        }

        private void Odobri_Click(object sender, RoutedEventArgs e)
        {
            ZahtevZaGodisnji izabraniZahtev = (ZahtevZaGodisnji)TabelaLekara.SelectedItem;
            int indeks = TabelaLekara.SelectedIndex;

            if (izabraniZahtev == null)
            {
                MessageBox.Show("Izaberite zahtev koji zelite da odobrite.");
            }
            else
            {
                LekariServis.OdobriZahtevZaGodisnji(izabraniZahtev, indeks);
            }
        }
       
        private void Odbij_Click(object sender, RoutedEventArgs e)
        {
            ZahtevZaGodisnji izabraniZahtev = (ZahtevZaGodisnji)TabelaLekara.SelectedItem;
            int indeks = TabelaLekara.SelectedIndex;

            if (izabraniZahtev == null)
            {
                MessageBox.Show("Izaberite zahtev koji zelite da odbijete.");
            }
            else
            {
                LekariServis.OdbijZahtevZaGodisnji(izabraniZahtev, indeks);
            }
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
