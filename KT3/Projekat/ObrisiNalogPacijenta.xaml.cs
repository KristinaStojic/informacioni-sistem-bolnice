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
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for ObrisiNalogPacijenta.xaml
    /// </summary>
    public partial class ObrisiNalogPacijenta : Window
    {
        Pacijent pacijent;
        PrikaziPacijenta prikaz;
        PacijentiServis servis = new PacijentiServis();
        public ObrisiNalogPacijenta(Pacijent zaBrisanje, PrikaziPacijenta p)
        {
            InitializeComponent();
            pacijent = zaBrisanje;
            prikaz = p;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            servis.ObrisiNalog(pacijent);
            this.Close();
            prikaz.zatvoreno = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
