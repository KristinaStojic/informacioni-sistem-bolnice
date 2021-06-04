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

        private void Pacijenti_Click(object sender, RoutedEventArgs e)
        {
            PrikaziPacijenta p = new PrikaziPacijenta();
            p.Show();
            this.Close();
        }

        private void Termini_Click(object sender, RoutedEventArgs e)
        {
            PrikaziTerminSekretar s = new PrikaziTerminSekretar();
            s.Show();
            this.Close();
        }

        private void Lekari_Click(object sender, RoutedEventArgs e)
        {
            PrikaziLekare lekari = new PrikaziLekare();
            lekari.Show();
            this.Close();
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Otvori_meni_Click(object sender, RoutedEventArgs e)
        {
            menu.Visibility = Visibility.Visible;
        }

        private void Zatvori_meni_Click(object sender, RoutedEventArgs e)
        {
            menu.Visibility = Visibility.Hidden;
        }

        private void Oglasna_tabla_Click(object sender, RoutedEventArgs e)
        {
            OglasnaTabla o = new OglasnaTabla();
            o.Show();
            this.Close();
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            SekretarPomoc pomoc = new SekretarPomoc();
            pomoc.Show();
        }

        private void Wizard_Click(object sender, RoutedEventArgs e)
        {
            HelpWizard help = new HelpWizard();
            help.Show();
        }

        private void Komunikacija_Click(object sender, RoutedEventArgs e)
        {
            KomunikacijaSekretar komunikacija = new KomunikacijaSekretar();
            komunikacija.Show();
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.M && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Otvori_meni_Click(sender, e);
            }
            else if (e.Key == Key.M && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Otvori_meni_Click(sender, e);
            } 
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Zatvori_meni_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Zatvori_meni_Click(sender, e);
            }
            else if (e.Key == Key.P && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Pomoc_Click(sender, e);
            }
            else if (e.Key == Key.P && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Pomoc_Click(sender, e);
            }
            else if (e.Key == Key.L && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                this.Close();
            }
            else if (e.Key == Key.L && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                this.Close();
            }
            else if (e.Key == Key.W && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Wizard_Click(sender, e);
            }
            else if (e.Key == Key.W && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Wizard_Click(sender, e);
            }
            else if (e.Key == Key.K && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Wizard_Click(sender, e);
            }
            else if (e.Key == Key.K && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Wizard_Click(sender, e);
            }
        }

    }
}
