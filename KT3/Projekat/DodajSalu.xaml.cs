using Model;
using Projekat.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        { 
            SaleMenadzer.DodajSalu(napraviSalu());
            this.Close();
        }

        private Sala napraviSalu()
        {
            int brojSale = int.Parse(this.brojSale.Text);
            string namjenaSale = this.namjenaSale.Text;
            tipSale TipSale = nadjiTipSale();
            Sala sala = new Sala(SaleMenadzer.GenerisanjeIdSale(), brojSale, namjenaSale, TipSale);
            sala.Oprema = new List<Oprema>();
            return sala;
        }

        private tipSale nadjiTipSale()
        {
            if (combo.Text.Equals("Sala za preglede"))
            {
                return tipSale.SalaZaPregled;
            }
            else if (combo.Text.Equals("Sala za operacije"))
            {
                return tipSale.OperacionaSala;
            }
            else
            {
                return tipSale.SalaZaOdmor;
            }
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

        private void postaviDugme()
        {
            if(this.brojSale.Text.Trim().Equals("") || this.namjenaSale.Text.Trim().Equals("") || !jeBroj(this.brojSale.Text) || jeBroj(this.namjenaSale.Text) || postojiBrojSale())
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
    }
}
