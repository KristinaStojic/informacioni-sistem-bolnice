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
    /// Interaction logic for OdobravanjeGodisnjegOdmora.xaml
    /// </summary>
    public partial class OdobravanjeGodisnjegOdmora : Window
    {
        public OdobravanjeGodisnjegOdmora()
        {
            InitializeComponent();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TabelaLekara_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
