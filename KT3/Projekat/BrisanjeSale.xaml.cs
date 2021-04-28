using Model;
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
    /// Interaction logic for BrisanjeSale.xaml
    /// </summary>
    public partial class BrisanjeSale : Window
    {

        Sala izabranaSala;

        public BrisanjeSale(Sala izabranaSala)
        {
            InitializeComponent();
            this.izabranaSala = izabranaSala;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaleMenadzer.ObrisiSalu((Sala)izabranaSala);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
