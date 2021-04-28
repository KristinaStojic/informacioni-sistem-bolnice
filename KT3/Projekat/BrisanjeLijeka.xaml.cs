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
    /// Interaction logic for BrisanjeLijeka.xaml
    /// </summary>
    public partial class BrisanjeLijeka : Window
    {
        Lek izabraniLijek;
        public BrisanjeLijeka(Lek izabraniLijek)
        {
            InitializeComponent();
            this.izabraniLijek = izabraniLijek;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LekoviMenadzer.obrisiLijek(izabraniLijek);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
