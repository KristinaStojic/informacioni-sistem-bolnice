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
    /// Interaction logic for SekretarPrijava.xaml
    /// </summary>
    public partial class SekretarPrijava : Window
    {
        public SekretarPrijava()
        {
            InitializeComponent();
        }

        private void Prijava_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            if (ProveriIdentitet())
            {
                Sekretar pocetnaStrana = new Sekretar();
                pocetnaStrana.Show();
            }
            else
            {
                MessageBox.Show("Pogresno korisničko ime ili lozinka!");
            }
        }

        private bool ProveriIdentitet()
        {
            if (ime.Text.Equals("teodora") && sifra.Password.Equals("teodora"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
