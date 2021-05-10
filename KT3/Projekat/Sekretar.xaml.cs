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
using Projekat.Pomoc;

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
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PrikaziTerminSekretar s = new PrikaziTerminSekretar();
            s.Show();
            this.Close();
        }

        // nazad
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        // otvori meni
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            menu.Visibility = Visibility.Visible;
        }

        // zatvori meni
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            menu.Visibility = Visibility.Hidden;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            OglasnaTabla o = new OglasnaTabla();
            o.Show();
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.M && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_3(sender, e);
            }
            else if (e.Key == Key.M && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Button_Click_3(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_4(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Button_Click_4(sender, e);
            }
            else if (e.Key == Key.P && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Hyperlink_Click(sender, e);
            }
            else if (e.Key == Key.P && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Hyperlink_Click(sender, e);
            }
            else if (e.Key == Key.L && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                this.Close();
            }
            else if (e.Key == Key.L && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                this.Close();
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            SekretarPomoc pomoc = new SekretarPomoc();
            pomoc.Show();
        }
    }
}
