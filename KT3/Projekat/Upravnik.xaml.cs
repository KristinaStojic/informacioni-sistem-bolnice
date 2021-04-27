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
using Projekat.Model;

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

            foreach (Obavestenja o in ObavestenjaMenadzer.obavestenja)
            {
                if (o.Oznaka.Equals("svi") || o.Oznaka.Equals("upravnici"))
                {
                    obavestenjaUpravnik.Items.Add(o);
                }
            }
        }

        private void Prostorije_Click(object sender, RoutedEventArgs e)
        {
            PrikaziSalu w1 = new PrikaziSalu();
            this.Close();
            w1.ShowDialog();
        }

        private void Zahtjevi_Click(object sender, RoutedEventArgs e)
        {
            Zahtjevi w2 = new Zahtjevi();
            this.Close();
            w2.ShowDialog();
        }

        private void Odjava_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
