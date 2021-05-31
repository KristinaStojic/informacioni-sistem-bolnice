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
    /// Interaction logic for PrijavaLekar.xaml
    /// </summary>
    public partial class PrijavaLekar : Window
    {
        public PrijavaLekar()
        {
            InitializeComponent();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            int idLekara = Int32.Parse(ime.Text.ToString());
            PocetnaStrana ps = new PocetnaStrana(idLekara);
            ps.Show();
            this.Close();
        }

        private void ime_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
