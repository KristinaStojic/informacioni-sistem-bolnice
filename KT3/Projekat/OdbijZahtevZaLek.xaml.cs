using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for OdbijZahtevZaLek.xaml
    /// </summary>
    public partial class OdbijZahtevZaLek : Window
    {
        ZahtevZaLekove zahtev;
        public bool popunjeno = false;
        public OdbijZahtevZaLek(ZahtevZaLekove izabraniZahtev)
        {
            InitializeComponent();
            /*this.zahtev = izabraniZahtev;
            this.naziv.Text = izabraniZahtev.nazivLeka;
            this.potvrdi.IsEnabled = false;*/
        }

        private void Button_Odustani(object sender, RoutedEventArgs e)
        {
            //odustani
            this.Close();
            /*ObradiZahtevZaLek oz = new ObradiZahtevZaLek(zahtev);
            oz.Show();*/
        }

        private void Button_Sacuvaj(object sender, RoutedEventArgs e)
        {
            //sacuvaj
            if(popunjeno == true)
            {

                LekoviServis.odbijaZahtev(zahtev, this.razlogOdbijanja.Text);
                this.Close();

            }
            else
            {
                MessageBox.Show("Niste popunili sve podatke!");
            }




        }

        /*private void Grid_KeyDown(object sender, KeyEventArgs e)
            
        {
           
            
                if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    Button_Odustani(sender, e);
                }
                else if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    Button_Sacuvaj(sender, e);
                }
                
            

        }*/

        private void razlogOdbijanja_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }


        private void postaviDugme()
        {
            if (this.razlogOdbijanja.Text != null)
            {
                izvrsiPostavljanje();
            }
            else
            {
                this.potvrdi.IsEnabled = false;
            }
        }
        private void izvrsiPostavljanje()
        {
            if (this.razlogOdbijanja.Text.Trim().Equals(""))
            {
                this.potvrdi.IsEnabled = false;
                popunjeno = false;
            }
            else if (!this.razlogOdbijanja.Text.Trim().Equals(""))
            {
                this.potvrdi.IsEnabled = true;
                popunjeno = true;
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
