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
    /// Interaction logic for IzmjeniOpremu.xaml
    /// </summary>
    public partial class IzmjeniOpremu : Window
    {
        public Oprema oprema;
        public IzmjeniOpremu(Oprema izabranaOprema)
        {
            InitializeComponent();
            this.oprema = izabranaOprema;
            if(izabranaOprema != null)
            {
                this.naziv.Text = izabranaOprema.NazivOpreme;
                this.kolicina.Text = izabranaOprema.Kolicina.ToString();
            }
        }

        public void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            string naziv = this.naziv.Text;
            int kolicina = int.Parse(this.kolicina.Text);
            Oprema o = new Oprema(naziv, kolicina, oprema.Staticka);
            o.IdOpreme = oprema.IdOpreme;
            OpremaMenadzer.izmjeniOpremu(oprema, o);
            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
