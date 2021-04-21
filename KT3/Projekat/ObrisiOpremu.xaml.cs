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
    /// Interaction logic for ObrisiOpremu.xaml
    /// </summary>
    public partial class ObrisiOpremu : Window
    {
        public Oprema izabranaOprema;
        public ObrisiOpremu(Oprema izabranaOprema)
        {
            InitializeComponent();
            this.izabranaOprema = izabranaOprema;
            postaviMax();
        }

        private void postaviMax()
        {
            bool postoji = false;
            int kolicina = izabranaOprema.Kolicina;
            foreach(Premjestaj pm in PremjestajMenadzer.premjestaji)
            {
                if(pm.izSale.Id == 4 && pm.oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    kolicina -= pm.kolicina;
                }
            }
            this.maks.Text = "MAX: " + kolicina.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach(Sala s in SaleMenadzer.sale)
            {
                if (s.Namjena.Equals("Skladiste"))
                {
                    foreach(Oprema o in s.Oprema.ToList())
                    {
                        if(o.IdOpreme == izabranaOprema.IdOpreme)
                        {
                            o.Kolicina -= int.Parse(kolicina.Text);
                            if(o.Kolicina == 0)
                            {
                                s.Oprema.Remove(o);
                            }
                        }
                    }
                }
            }
            Skladiste.azurirajOpremu();
            this.Close();
        }
    }
}
