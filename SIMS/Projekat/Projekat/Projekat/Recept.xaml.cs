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
using Model;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for Recept.xaml
    /// </summary>
    public partial class Recept : Window
    {
        public Pacijent pacijent;

        public Recept(/*Pacijent izabraniNalog*/)
        {
            InitializeComponent();
            /*this.pacijent = izabraniNalog;

            ime.Text = izabraniNalog.ImePacijenta;
            prezime.Text = izabraniNalog.PrezimePacijenta;
            id.Text = izabraniNalog.IdPacijenta.ToString();*/
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
