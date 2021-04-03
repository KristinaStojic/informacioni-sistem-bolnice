using Model;
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
    /// Interaction logic for PrikazDinamicke.xaml
    /// </summary>
    public partial class PrikazDinamicke : Window
    {
        Sala izabranaSala;
        public PrikazDinamicke(Sala izabranaSala)
        {
            InitializeComponent();
            this.izabranaSala = izabranaSala;
            if (izabranaSala != null)
            {
                if (izabranaSala.TipSale == tipSale.SalaZaPregled)
                {
                    this.tekst.Text = "Sala za pregled (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
                else
                {
                    this.tekst.Text = "Opreaciona sala (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PreraspodjelaDinamicke pd = new PreraspodjelaDinamicke();
            pd.Show();
        }
    }
}
