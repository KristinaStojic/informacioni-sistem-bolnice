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
    /// Interaction logic for DetaljiAlergena.xaml
    /// </summary>
    public partial class DetaljiAlergena : Window
    {
        Pacijent pacijent;
        Alergeni stariAlergen;
        Termin termin;
        public DetaljiAlergena(Alergeni izabraniAlergen, Termin termin)
        {
            InitializeComponent();
            this.stariAlergen = izabraniAlergen;
            this.termin = termin;
            foreach (Pacijent pac in PacijentiMenadzer.pacijenti)
            {
                if (pac.IdPacijenta == izabraniAlergen.IdPacijenta)
                {
                    this.naziv.Text = izabraniAlergen.NazivLeka;
                    this.sifra.Text = izabraniAlergen.SifraLeka;
                    this.nuspojava.Text = izabraniAlergen.NuspojavaNaLek;
                    this.vreme.Text = izabraniAlergen.VremeReakcije;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //sacuvaj
            string nazivLeka = naziv.Text;
            string sifraLeka = sifra.Text;
            string nuspojavaNaLek = nuspojava.Text;
            string vremeReakcije = vreme.Text;

            Alergeni noviAlergen = new Alergeni(stariAlergen.IdAlergena, stariAlergen.IdPacijenta, nazivLeka, sifraLeka, nuspojavaNaLek, vremeReakcije);
            ZdravstveniKartonMenadzer.IzmeniAlergen(stariAlergen, noviAlergen);

            TerminMenadzer.sacuvajIzmene();
            PacijentiMenadzer.SacuvajIzmenePacijenta();
            SaleMenadzer.sacuvajIzmjene();


            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //odustani
            this.Close();
        }
    }
}
