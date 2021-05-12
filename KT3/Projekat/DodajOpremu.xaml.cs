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
            this.Potvrdi.IsEnabled = false;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            Oprema o = napraviOpremu();
            OpremaMenadzer.DodajOpremu(o);
            this.Close();
        }

        private Oprema napraviOpremu()
        {
            string nazivOpreme = naziv.Text;
            int Kolicina = int.Parse(kolicina.Text);
            int idOpreme = OpremaMenadzer.GenerisanjeIdOpreme();
            Oprema o = new Oprema(nazivOpreme, Kolicina, staticka);
            o.IdOpreme = idOpreme;
            return o;
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void naziv_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void kolicina_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();   
        }

        private void postaviDugme()
        {
            if (!jeBroj(this.kolicina.Text) || this.kolicina.Text.Trim().Equals("") || jeBroj(this.naziv.Text) || this.naziv.Text.Trim().Equals(""))
            {
                this.Potvrdi.IsEnabled = false;
            }
            else if (jeBroj(this.kolicina.Text) && !this.kolicina.Text.Trim().Equals("") && !jeBroj(this.naziv.Text) && !this.naziv.Text.Trim().Equals(""))
            {
                this.Potvrdi.IsEnabled = true;
            }
        }

        public bool jeBroj(string input)
        {
            int test;
            return int.TryParse(input, out test);
        }
    }
}
