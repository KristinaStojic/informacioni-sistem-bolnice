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
    /// Interaction logic for PrikazAnamnezePacijent.xaml
    /// </summary>
    public partial class PrikazAnamnezePacijent : Window
    {

        public Pacijent pacijent;
        public Anamneza anamneza;
        public PrikazAnamnezePacijent(Pacijent izabraniPacijent, Anamneza izabranaAnamneza)
        {
            InitializeComponent();
            pacijent = izabraniPacijent;
            anamneza = izabranaAnamneza;


            this.datumAnamneze.Text = anamneza.Datum;
            this.podaciLekar.Text = anamneza.ImePrezimeLekara;
            this.opisBolesti.Text = anamneza.OpisBolesti;
            this.terpaija.Text = anamneza.Terapija;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // nazad
            this.Close();
        }
    }
}
