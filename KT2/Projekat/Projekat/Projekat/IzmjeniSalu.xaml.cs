using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for IzmjeniSalu.xaml
    /// </summary>
    public partial class IzmjeniSalu : Window
    {
        public Sala sala;
        public IzmjeniSalu(Sala izabranaSala)
        {
            InitializeComponent();
            this.sala = izabranaSala;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
         
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
