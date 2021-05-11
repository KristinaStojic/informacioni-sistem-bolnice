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
    /// Interaction logic for Komunikacija.xaml
    /// </summary>
    public partial class Komunikacija : Window
    {
        public Komunikacija()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Upravnik upravnik = new Upravnik();
            upravnik.Show();
            this.Close();
        }

        private void Button_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.N)
                {
                    Button_Click(sender, e);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ZahtjeviZaKomunikaciju zahtjeviZaKomunikaciju = new ZahtjeviZaKomunikaciju();
            zahtjeviZaKomunikaciju.Show();
        }
    }
}
