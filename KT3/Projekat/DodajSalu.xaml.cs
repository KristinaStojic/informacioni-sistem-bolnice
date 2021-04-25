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
            this.Potvrdi.IsEnabled = false;
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

        public bool IsNumeric(string input)
        {
            int test;
            return int.TryParse(input, out test);
        }

        private void text1_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void text2_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void postaviDugme()
        {
            if(this.text1.Text.Trim().Equals("") || this.text2.Text.Trim().Equals("") || !IsNumeric(this.text1.Text) || IsNumeric(this.text2.Text) || postojiBrojSale())
            {
                this.Potvrdi.IsEnabled = false;
            }else if (!this.text1.Text.Trim().Equals("") && !this.text2.Text.Trim().Equals("") && IsNumeric(this.text1.Text) && !IsNumeric(this.text2.Text) && !postojiBrojSale())
            {
                this.Potvrdi.IsEnabled = true;
            }
        }
        private bool postojiBrojSale()
        {
            if (IsNumeric(this.text1.Text))
            {
                foreach (Sala sala in SaleMenadzer.sale)
                {
                    if (sala.brojSale == int.Parse(this.text1.Text))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
