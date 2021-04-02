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
    /// Interaction logic for DodajOpremu.xaml
    /// </summary>
    public partial class DodajOpremu : Window
    {
        public bool staticka;
        public DodajOpremu(bool staticka)
        {
            InitializeComponent();
            this.staticka = staticka;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        { 
            string nazivOpreme = naziv.Text;
            int Kolicina = int.Parse(kolicina.Text);
            int idOpreme = OpremaMenadzer.GenerisanjeIdOpreme();
            Oprema o = new Oprema(nazivOpreme, Kolicina, staticka);
            OpremaMenadzer.DodajOpremu(o);
            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
