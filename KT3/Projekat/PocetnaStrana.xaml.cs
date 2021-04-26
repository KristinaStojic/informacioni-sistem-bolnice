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
    /// Interaction logic for PocetnaStrana.xaml
    /// </summary>
    public partial class PocetnaStrana : Window
    {
        public PocetnaStrana()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrikazTerminaLekar pl = new PrikazTerminaLekar();
            pl.Show();
            this.Close();
        }

        private void Button_Zahtevi(object sender, RoutedEventArgs e)
        {


            SpisakZahtevaZaLekove zahtevi = new SpisakZahtevaZaLekove();
            zahtevi.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ObavestenjaLekar o = new ObavestenjaLekar();
            o.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
