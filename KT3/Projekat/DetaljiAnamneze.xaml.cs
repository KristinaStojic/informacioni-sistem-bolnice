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
    /// Interaction logic for DetaljiAnamneze.xaml
    /// </summary>
    public partial class DetaljiAnamneze : Window
    {
        Pacijent pacijent;
        Anamneza stara;
        public DetaljiAnamneze(Anamneza izabranaAnamneza)
        {
            InitializeComponent();
            this.stara = izabranaAnamneza;
            foreach(Pacijent pac in PacijentiMenadzer.pacijenti)
            {
                if(pac.IdPacijenta == izabranaAnamneza.IdPacijenta)
                {
                    this.datum.SelectedDate = DateTime.Parse(izabranaAnamneza.Datum);
                    this.bolest.Text = izabranaAnamneza.OpisBolesti;
                    this.terapija.Text = izabranaAnamneza.Terapija;
                }
            }
    
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //sacuvaj
            string ter = terapija.Text;
            string bol = bolest.Text;

            Anamneza nova = new Anamneza(ter, bol);
            ZdravstveniKartonMenadzer.IzmeniAnamnezu(stara, nova);

            
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //odustani
            this.Close();
        }
    }
}
