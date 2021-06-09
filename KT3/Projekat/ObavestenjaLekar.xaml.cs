using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for ObavestenjaLekar.xaml
    /// </summary>
    public partial class ObavestenjaLekar : Window
    {
        /* public static ObservableCollection<Obavestenja> obavestenjaLekar
        {
            get;
            set;
        }  */

        public ObavestenjaLekar()
        {
            InitializeComponent();
            //this.DataContext = this;
            //obavestenjaLekar = new ObservableCollection<Obavestenja>();
            foreach (Obavestenja o in LekariServis.NadjiPoId(1).obavestenja)
            {
                ObavestenjaLekara.Items.Add(o);
            }
        }

        private void Button_Nazad(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Nazad(sender, e);
            }
        }
    }
}
