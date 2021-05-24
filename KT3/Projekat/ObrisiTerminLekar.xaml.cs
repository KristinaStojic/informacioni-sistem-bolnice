using Model;
using Projekat.Servis;
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
    /// Interaction logic for ObrisiTerminLekar.xaml
    /// </summary>
    public partial class ObrisiTerminLekar : Window
    { 
        Termin TerminzaBrisanje;
        public ObrisiTerminLekar(Termin izabranitermin)
        {
            InitializeComponent();
            this.TerminzaBrisanje = izabranitermin;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            
            if (TerminzaBrisanje != null)
            {

                TerminServisLekar.OtkaziTerminLekar(TerminzaBrisanje);
                this.Close();
            }
            else
            {
                MessageBox.Show("Niste selektovali termin koji zelite da otkazete!");
            }
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Odustani_Click(sender, e);
            }
            else if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Potvrdi_Click(sender, e);
            }
        }
    }
}
