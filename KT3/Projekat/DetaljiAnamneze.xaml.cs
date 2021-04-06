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
        public DetaljiAnamneze(Pacijent izabraniPacijent, Anamneza izabranaAnamneza)
        {
            InitializeComponent();
            this.pacijent = izabraniPacijent;

            Console.WriteLine(pacijent.ImePacijenta + " " + pacijent.PrezimePacijenta);
            Console.WriteLine("Broj anamneza u kartonu: " + pacijent.Karton.Anamneze.Count);
            foreach (Anamneza anamneza in pacijent.Karton.Anamneze)
            {
                Console.WriteLine("Lista: " + anamneza.IdAnamneze + " Izabrana: " + izabranaAnamneza.IdAnamneze); 
                Console.WriteLine("Lista: " + anamneza.OpisBolesti + " Izabrana: " + izabranaAnamneza.OpisBolesti);
               if(anamneza.IdAnamneze == izabranaAnamneza.IdAnamneze)
                {
                    Console.WriteLine("Lista: " + anamneza.IdAnamneze + " Izabrana: " + izabranaAnamneza.IdAnamneze);
                    this.datum.SelectedDate = DateTime.Parse(izabranaAnamneza.Datum);
                    this.bolest.Text = izabranaAnamneza.OpisBolesti;
                    this.terapija.Text = izabranaAnamneza.Terapija;

                }
            }
    
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
