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
    /// Interaction logic for Upravnik.xaml
    /// </summary>
    public partial class Upravnik : Window
    {
        public Upravnik()
        {
            InitializeComponent();
        }

        private void Prostorije_Click(object sender, RoutedEventArgs e)
        {
            PrikaziSalu w1 = new PrikaziSalu();
            w1.ShowDialog();
        }

        private void Zahtjevi_Click(object sender, RoutedEventArgs e)
        {
            Zahtjevi w2 = new Zahtjevi();
            w2.ShowDialog();
        }

        private void Odjava_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
