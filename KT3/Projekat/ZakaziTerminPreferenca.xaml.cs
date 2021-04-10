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
    /// Interaction logic for ZakaziTerminPreferenca.xaml
    /// </summary>
    public partial class ZakaziTerminPreferenca : Window
    {
        public ZakaziTerminPreferenca()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // preporuci prva tri slobodna termina
        }

        private void termin1_Click(object sender, RoutedEventArgs e)
        {
            // termin 1
            MessageBox.Show("termin1");
        }

        private void termin2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void termin3_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
