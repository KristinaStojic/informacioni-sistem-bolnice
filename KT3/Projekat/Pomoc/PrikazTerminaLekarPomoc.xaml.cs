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

namespace Projekat.Pomoc
{
    /// <summary>
    /// Interaction logic for PrikazTerminaLekarPomoc.xaml
    /// </summary>
    public partial class PrikazTerminaLekarPomoc : Window
    {
        public PrikazTerminaLekarPomoc()
        {
            InitializeComponent();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBlock_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
