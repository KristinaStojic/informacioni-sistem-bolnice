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
            if (izabranaSala != null)
            {
                this.text1.Text = izabranaSala.Id.ToString();
                this.text2.Text = izabranaSala.Namjena;
                if (izabranaSala.Status.Equals(status.Slobodna))
                {
                    this.combo.SelectedIndex = 0;
                }
                else if (izabranaSala.Status.Equals(status.Zauzeta))
                {
                    this.combo.SelectedIndex = 1;
                }
                else
                {
                    this.combo.SelectedIndex = 2;
                }
                if (izabranaSala.TipSale.Equals(tipSale.SalaZaPregled))
                {
                    this.combo1.SelectedIndex = 1;
                }
                else
                {
                    this.combo1.SelectedIndex = 0;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(this.text1.Text);
            string namjena = this.text2.Text;
            tipSale Tip;
            status Status;
            if (this.combo.SelectedIndex == 0)
            {
                Status = status.Slobodna;
            }
            else if (this.combo.SelectedIndex == 1)
            {
                Status = status.Zauzeta;
            }
            else
            {
                Status = status.Renoviranje;
            }
            if (this.combo1.SelectedIndex == 1)
            {
                Tip = tipSale.SalaZaPregled;
            }
            else
            {
                Tip = tipSale.OperacionaSala;
            }
            Sala s = new Sala(id, namjena, Tip);
            s.Status = Status;
            SaleMenadzer.IzmjeniSalu(sala, s);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
