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
    /// Interaction logic for ObrisiSastojakLekar.xaml
    /// </summary>
    public partial class ObrisiSastojakLekar : Window
    {
        Sastojak sastojak;
        Lek lek;
        public ObrisiSastojakLekar(Sastojak izabraniSastojak, Lek izabraniLek)
        {
            InitializeComponent();
           /* this.sastojak = izabraniSastojak;
            this.lek = izabraniLek;*/
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            LekoviServis.obrisiSastojakLekaLekar(lek, sastojak);
            this.Close();

        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl)) //Sacuvaj
            {
                Potvrdi_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) //Nazad
            {
                this.Close();
            }*/
        }
    }
}
