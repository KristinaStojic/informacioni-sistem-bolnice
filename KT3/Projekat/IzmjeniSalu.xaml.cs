using Model;
using System.Windows;
using System.Windows.Controls;

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
            postaviElemente(izabranaSala);
        }

        private void postaviElemente(Sala izabranaSala)
        {
            this.izabranaSala = izabranaSala;
            if (izabranaSala != null)
            {
                this.brojSale.Text = izabranaSala.brojSale.ToString();
                this.namjenaSale.Text = izabranaSala.Namjena;
                postaviTipSale();
            }
        }

        private void postaviTipSale()
        {
            if (izabranaSala.TipSale.Equals(global::Model.tipSale.SalaZaPregled))
            {
                this.tipSale.SelectedIndex = 1;
            }
            else if (izabranaSala.TipSale.Equals(global::Model.tipSale.OperacionaSala))
            {
                this.tipSale.SelectedIndex = 0;
            }
            else
            {
                this.tipSale.SelectedIndex = 2;
            }
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            SaleMenadzer.IzmjeniSalu(izabranaSala, napraviSalu());
            this.Close();
        }

        private Sala napraviSalu()
        {
            int brojSale = int.Parse(this.brojSale.Text);
            string namjena = this.namjenaSale.Text;
            tipSale Tip = nadjiTipSale();
            return new Sala(izabranaSala.Id, brojSale, namjena, Tip);
        }

        private tipSale nadjiTipSale()
        {
            if (this.tipSale.SelectedIndex == 1)
            {
                return global::Model.tipSale.SalaZaPregled;
            }
            else if (this.tipSale.SelectedIndex == 0)
            {
                return global::Model.tipSale.OperacionaSala;
            }
            else
            {
                return global::Model.tipSale.SalaZaOdmor;
            }
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                    if (sala.brojSale == int.Parse(this.brojSale.Text) && sala.Id != this.izabranaSala.Id)
                    {
                        return true;
                    }
                }
            }
            return false;
           
        }
    }
}
