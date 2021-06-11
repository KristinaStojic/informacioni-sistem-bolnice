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
    /// Interaction logic for ObrisiLekara.xaml
    /// </summary>
    public partial class ObrisiLekara : Window
    {
        Lekar lekar;
        LekariServis servis = new LekariServis();

        public ObrisiLekara(Lekar lekarZaBrisanje)
        {
            InitializeComponent();
            lekar = lekarZaBrisanje;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            servis.ObrisiLekara(lekar);
            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
