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
using Model;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for ObrisiNalogPacijenta.xaml
    /// </summary>
    public partial class ObrisiNalogPacijenta : Window
    {
        Pacijent pacijent;
        public ObrisiNalogPacijenta(Pacijent zaBrisanje)
        {
            InitializeComponent();
            pacijent = zaBrisanje;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PacijentiMenadzer.ObrisiNalog(pacijent);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
