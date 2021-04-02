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
    /// Interaction logic for DodajSalu.xaml
    /// </summary>
    public partial class DodajSalu : Window
    {
        public DodajSalu()
        {
            InitializeComponent();
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int brojSale = int.Parse(text1.Text);
            string namjenaSale = text2.Text;
            tipSale TipSale;
            if (combo.Text.Equals("Sala za preglede"))
            {
                TipSale = tipSale.SalaZaPregled;
            }
            else
            {
                TipSale = tipSale.OperacionaSala;
            }
            Sala s = new Sala(SaleMenadzer.GenerisanjeIdSale(), brojSale, namjenaSale, TipSale);
            SaleMenadzer.DodajSalu(s);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
