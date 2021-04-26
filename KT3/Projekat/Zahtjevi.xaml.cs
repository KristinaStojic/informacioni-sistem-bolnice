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
            try
            {
                Skladiste.otvoren = true;
                Skladiste w1 = new Skladiste();

                PremjestajMenadzer.odradiZakazano();
                this.Close();
                w1.ShowDialog();
            }catch(Exception ex) { }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Upravnik u = new Upravnik();
            this.Close();
            u.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Osoblje
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            PrikaziSalu ps = new PrikaziSalu();
            this.Close();
            ps.Show();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //Komunikacija
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            //Izvjestaj
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            //Pomoc
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            //O aplikaciji
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Lijekovi lijekoviProzor = new Lijekovi();
            lijekoviProzor.Show();
            this.Hide();
        }
    }
}
