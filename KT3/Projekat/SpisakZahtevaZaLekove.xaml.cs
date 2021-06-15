using Projekat.Model;
using Projekat.Pomoc;
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
            //this.DataContext = this;
            //dodajZahteveUTabelu();
            //dodajLekoveUTabelu();

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
            foreach (Lek lek in LekoviServis.Lijekovi())
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            Lek izabraniLek = (Lek)dataGridLekovi.SelectedItem;

            if (izabraniLek != null)
            {
                PrikazZamenskihLekovaLekar zamenskiLekovi = new PrikazZamenskihLekovaLekar(izabraniLek);
                zamenskiLekovi.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali nijedan lek!");
            }

        }

        #region Precice
        /*private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            tabZahtevi(sender,e);
            tabLekovi(sender, e);
            otvoriTabove(sender,e);
           
        }

        private void tabZahtevi(object sender, KeyEventArgs e)
        {
            if (zahtevi.IsSelected == true)
            {

                if (e.Key == Key.Z && Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    Button_Obradi(sender, e);
                }
                else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    Button_Click_1(sender, e);
                }
                else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    Button_Obrisi(sender, e);
                }
                else if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    Hyperlink_Click(sender, e);
                }
            }
        }

        private void otvoriTabove(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.L && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                sviLekovi.IsSelected = true;
            }
            else if (e.Key == Key.V && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                zahtevi.IsSelected = true;
            }
        }

        private void tabLekovi(object sender, KeyEventArgs e)
        {
            if (sviLekovi.IsSelected == true)
            {
                if (e.Key == Key.I && Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    Button_Izmeni(sender, e);
                }
                else if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    Button_Click(sender, e);
                }
                else if (e.Key == Key.Z && Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    Button_Click_2(sender, e);
                }
                else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    Button_Click_1(sender, e);
                }
                else if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    Lekovi_Pomoc(sender, e);
                }
               
            }
        }*/

        #endregion



        private void Button_Obrisi(object sender, RoutedEventArgs e)
        {
            /*ZahtevZaLekove zaBrisanje = (ZahtevZaLekove)dataGridZahtevi.SelectedItem;
            
            ZahtevZaLekove izabraniZahtjev = null;
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (zahtjev.lek.sifraLeka.Equals(zaBrisanje.sifraLeka))
                {
                    izabraniZahtjev = zahtjev;
                }
            }

            LekoviMenadzer.zahteviZaLekove.Remove(izabraniZahtjev);
            TabelaZahteva.Remove(zaBrisanje);
            LekoviServis.sacuvajIzmeneZahteva();*/
            ZahtevZaLekove izabraniZahtev = (ZahtevZaLekove)dataGridZahtevi.SelectedItem;

            ObrisiZahtevLekar oz = new ObrisiZahtevLekar(izabraniZahtev);
            oz.Show();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {        
            PomocZahteviZaLekove pomoc = new PomocZahteviZaLekove();
            pomoc.Show();       
        }

        private void Lekovi_Pomoc(object sender, RoutedEventArgs e)
        {
            LekoviPomocLekar pomoc = new LekoviPomocLekar();
            pomoc.Show();
        }
    }
}
