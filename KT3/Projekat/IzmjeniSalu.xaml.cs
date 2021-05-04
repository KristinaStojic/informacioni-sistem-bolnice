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
        public Sala izabranaSala;
        public IzmjeniSalu(Sala izabranaSala)
        {
            InitializeComponent();
            this.izabranaSala = izabranaSala;
            postaviElemente();
        }

        private void postaviElemente()
        {
            if (izabranaSala != null)
            {
                this.text1.Text = izabranaSala.brojSale.ToString();
                this.text2.Text = izabranaSala.Namjena;

                if (izabranaSala.TipSale.Equals(tipSale.SalaZaPregled))
                {
                    this.combo1.SelectedIndex = 1;
                }
                else if (izabranaSala.TipSale.Equals(tipSale.OperacionaSala))
                {
                    this.combo1.SelectedIndex = 0;
                }
                else
                {
                    this.combo1.SelectedIndex = 2;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaleMenadzer.IzmjeniSalu(izabranaSala, napraviSalu());
            this.Close();
        }

        private Sala napraviSalu()
        {
            int brojSale = int.Parse(this.text1.Text);
            string namjena = this.text2.Text;
            tipSale Tip = nadjiTipSale();
            return new Sala(izabranaSala.Id, brojSale, namjena, Tip);
        }

        private tipSale nadjiTipSale()
        {
            if (this.combo1.SelectedIndex == 1)
            {
                return tipSale.SalaZaPregled;
            }
            else if (this.combo1.SelectedIndex == 0)
            {
                return tipSale.OperacionaSala;
            }
            else
            {
                return tipSale.SalaZaOdmor;
            }
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
            if (this.text1.Text.Trim().Equals("") || this.text2.Text.Trim().Equals("") || !IsNumeric(this.text1.Text) || IsNumeric(this.text2.Text) || postojiBrojSale())
            {
                this.Potvrdi.IsEnabled = false;
            }
            else if (!this.text1.Text.Trim().Equals("") && !this.text2.Text.Trim().Equals("") && IsNumeric(this.text1.Text) && !IsNumeric(this.text2.Text) && !postojiBrojSale())
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
                    if (sala.brojSale == int.Parse(this.text1.Text) && sala.Id != this.izabranaSala.Id)
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
