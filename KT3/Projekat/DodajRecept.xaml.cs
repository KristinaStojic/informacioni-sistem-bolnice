using Model;
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
    /// Interaction logic for DodajRecept.xaml
    /// </summary>
    public partial class DodajRecept : Window
    {
        public DodajRecept()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nadjiLek.SelectedItems.Count > 0)
            {
                Lekar item = (Lekar)nadjiLek.SelectedItems[0];
                nazivSifra.Text = item.ToString();
            }
        }


        private void pretraga_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(nadjiLek.ItemsSource).Refresh();
        }
    }
}
