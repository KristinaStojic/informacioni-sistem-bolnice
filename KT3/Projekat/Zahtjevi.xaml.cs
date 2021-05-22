using Projekat.Model;
using Projekat.Pomoc;
using Projekat.Servis;
using System;
using System.Windows;
using System.Windows.Input;

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
        
        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            ZahtjeviPomoc zahtjeviPomoc = new ZahtjeviPomoc();
            zahtjeviPomoc.Show();
        }

    }
}
