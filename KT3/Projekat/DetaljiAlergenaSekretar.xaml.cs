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
    /// Interaction logic for DetaljiAlergenaSekretar.xaml
    /// </summary>
    public partial class DetaljiAlergenaSekretar : Window
    {
        Pacijent pacijent;
        Alergeni stariAlergen;

        public DetaljiAlergenaSekretar(Alergeni izabraniAlergen, Pacijent selektovaniPacijent)
        {
            InitializeComponent();
            this.stariAlergen = izabraniAlergen;
            this.pacijent = selektovaniPacijent;

            PopuniPodatkeOAlergenu(izabraniAlergen);
        }
        private void PopuniPodatkeOAlergenu(Alergeni izabraniAlergen)
        {
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                if (p.IdPacijenta == izabraniAlergen.IdPacijenta)
                {
                    this.naziv.Text = izabraniAlergen.NazivSastojka;
                    this.nuspojava.Text = izabraniAlergen.NuspojavaNaNastojak;
                    this.vreme.Text = izabraniAlergen.VremeReakcije;
                }
            }
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
