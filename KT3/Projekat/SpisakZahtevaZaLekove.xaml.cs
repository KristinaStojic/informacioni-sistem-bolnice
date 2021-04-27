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
        public static ObservableCollection<Lek> TabelaLekova
        {
            get;
            set;
        }

        public SpisakZahtevaZaLekove()
        {
            InitializeComponent();
            this.DataContext = this;
            dodajZahteveUTabelu();
            dodajLekoveUTabelu();

        }

        private void dodajZahteveUTabelu()
        {
            TabelaZahteva = new ObservableCollection<ZahtevZaLekove>();
            foreach (ZahtevZaLekove zahtev in LekoviMenadzer.zahteviZaLekove)
            {
                TabelaZahteva.Add(zahtev);
            }
        }

        private void dodajLekoveUTabelu()
        {
            TabelaLekova = new ObservableCollection<Lek>();
            foreach (Lek lek in LekoviMenadzer.lijekovi)
            {
                TabelaLekova.Add(lek);
            }
        }

        private void Button_Obradi(object sender, RoutedEventArgs e)
        {
            ZahtevZaLekove izabraniZahtev = (ZahtevZaLekove)dataGridZahtevi.SelectedItem;
 
            if(izabraniZahtev == null)
            {
                MessageBox.Show("Niste selektovali zahtev koji zelite da obradite!");
            }
            else if(izabraniZahtev.obradjenZahtev == true)
            {
                MessageBox.Show("Izabrani zahtev je vec obradjen!");
            }
            else if (izabraniZahtev != null && izabraniZahtev.obradjenZahtev == false)
            {
                ObradiZahtevZaLek obradiZahtev = new ObradiZahtevZaLek(izabraniZahtev);
                obradiZahtev.Show();
            }

        }
        
        private void Button_Nazad(object sender, RoutedEventArgs e)
        {
            this.Close();
        } 
        
        private void Button_Izmeni(object sender, RoutedEventArgs e)
        {
            
            Lek izabraniLek = (Lek)dataGridLekovi.SelectedItem;

            if (izabraniLek != null)
            {

                IzmeniLekLekar izmeniLek = new IzmeniLekLekar(izabraniLek);
                izmeniLek.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali nijedan lek!");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Lek izabraniLek = (Lek)dataGridLekovi.SelectedItem;

            if (izabraniLek != null)
            {
                PrikazSastojakaLekar sastojci = new PrikazSastojakaLekar(izabraniLek);
                sastojci.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali nijedan lek!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
