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
    /// Interaction logic for Sekretar.xaml
    /// </summary>
    public partial class Sekretar : Window
    {
        public Sekretar()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrikaziPacijenta p = new PrikaziPacijenta();
            p.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PrikaziTerminSekretar s = new PrikaziTerminSekretar();
            s.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
