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
    /// Interaction logic for Zahtjevi.xaml
    /// </summary>
    public partial class Zahtjevi : Window
    {
        public Zahtjevi()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Skladiste.otvoren = true;
            Skladiste w1 = new Skladiste();
            
            PremjestajMenadzer.odradiZakazano();
            w1.ShowDialog();
        }
    }
}
