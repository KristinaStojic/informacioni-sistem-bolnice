using Projekat.Model;
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
    /// Interaction logic for ObrisiZamenskiLekLekar.xaml
    /// </summary>
    public partial class ObrisiZamenskiLekLekar : Window
    {
        Lek lek;
        Lek zamenskiLek;
        public ObrisiZamenskiLekLekar(Lek izabraniLek, Lek izabraniZamenskiLek)
        {
            InitializeComponent();
            this.lek = izabraniLek;
            this.zamenskiLek = izabraniZamenskiLek;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            LekoviMenadzer.obrisiZamenskiLekLekar(lek,zamenskiLek);
            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
