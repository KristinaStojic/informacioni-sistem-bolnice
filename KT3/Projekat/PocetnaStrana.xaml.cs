using Projekat.Pomoc;
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
        int IDLekara;
        public PocetnaStrana(int idLekara)
        {
            InitializeComponent();
            this.IDLekara = idLekara;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrikazTerminaLekar pl = new PrikazTerminaLekar(IDLekara);
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
            //godisnji odmor
            ZahteviZaGodisnjiLekar zahtev = new ZahteviZaGodisnjiLekar(IDLekara);
            zahtev.Show();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.P && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click(sender, e);
            }
            else if (e.Key == Key.L && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Zahtevi(sender, e);
            }
            else if (e.Key == Key.G && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_3(sender, e);
            } 
            else if (e.Key == Key.M && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_3(sender, e);
            }
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_2(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                this.Close();
            }
            else if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Pomoc_Click(sender, e);
            }
            


        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            PocetnaStranaLekarPomoc pomoc = new PocetnaStranaLekarPomoc();
            pomoc.Show();
        }
    }
}
