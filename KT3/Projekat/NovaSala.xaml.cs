using Model;
using Projekat.Model;
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
    /// Interaction logic for NovaSala.xaml
    /// </summary>
    public partial class NovaSala : Window
    {
        Sala izabranaSala;
        Sala novaSala;

        public NovaSala(Sala izabranaSala)
        {
            InitializeComponent();
            inicijalizujElemente(izabranaSala);
        }

        private void inicijalizujElemente(Sala izabranaSala)
        {
            this.Potvrdi.IsEnabled = false;
            this.izabranaSala = izabranaSala;
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            novaSala = new Sala();
            novaSala.TipSale = izabranaSala.TipSale;
            novaSala.Namjena = this.namjenaSale.Text;
            novaSala.brojSale = int.Parse(this.brojSale.Text);
            novaSala.Oprema = new List<Oprema>();
            novaSala.zauzetiTermini = new List<ZauzeceSale>();
            otvoriPremjestajOpreme();
        }

        private void otvoriPremjestajOpreme()
        {
            PodjelaSale podjelaSale = new PodjelaSale(izabranaSala, novaSala);
            podjelaSale.Show();
            this.Close();
        }

        private void postaviDugme()
        {
            if (this.brojSale.Text.Trim().Equals("") || this.namjenaSale.Text.Trim().Equals("") || !jeBroj(this.brojSale.Text) || jeBroj(this.namjenaSale.Text) || postojiBrojSale())
            {
                this.Potvrdi.IsEnabled = false;
            }
            else if (!this.brojSale.Text.Trim().Equals("") && !this.namjenaSale.Text.Trim().Equals("") && jeBroj(this.brojSale.Text) && !jeBroj(this.namjenaSale.Text) && !postojiBrojSale())
            {
                this.Potvrdi.IsEnabled = true;
            }
        }

        private bool postojiBrojSale()
        {
            if (jeBroj(this.brojSale.Text))
            {
                foreach (Sala sala in SaleMenadzer.sale)
                {
                    if (sala.brojSale == int.Parse(this.brojSale.Text))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool jeBroj(string tekst)
        {
            int test;
            return int.TryParse(tekst, out test);
        }

        private void brojSale_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void namjenaSale_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }
    }
}
