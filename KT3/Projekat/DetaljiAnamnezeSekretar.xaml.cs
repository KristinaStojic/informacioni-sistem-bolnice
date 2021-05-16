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
using Model;
using Projekat.Model;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for DetaljiAnamnezeSekretar.xaml
    /// </summary>
    public partial class DetaljiAnamnezeSekretar : Window
    {
        Pacijent pacijent;
        Anamneza anamneza;

        public DetaljiAnamnezeSekretar(Anamneza izabranaAnamneza, Pacijent izabraniPacijent)
        {
            InitializeComponent();
            this.pacijent = izabraniPacijent;
            PopuniPodatke(izabranaAnamneza);
        }

        private void PopuniPodatke(Anamneza izabranaAnamneza)
        {
            this.anamneza = izabranaAnamneza;

            foreach (Pacijent pac in PacijentiMenadzer.pacijenti)
            {
                if (pac.IdPacijenta == izabranaAnamneza.IdPacijenta)
                {
                    this.datum.SelectedDate = DateTime.Parse(izabranaAnamneza.Datum);
                    this.lekar.Text = anamneza.ImePrezimeLekara;
                    this.bolest.Text = izabranaAnamneza.OpisBolesti;
                    this.terapija.Text = izabranaAnamneza.Terapija;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
