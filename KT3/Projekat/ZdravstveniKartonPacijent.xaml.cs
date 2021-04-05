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
    /// Interaction logic for ZdravstveniKartonPacijent.xaml
    /// </summary>
    public partial class ZdravstveniKartonPacijent : Window
    {
        public Pacijent pacijent;
        public List<LekarskiRecept> tempRecepti;
        public ZdravstveniKartonPacijent(Pacijent izabraniPacijent)
        {
            InitializeComponent();
            this.pacijent = izabraniPacijent;
            /* LEKARSKI RECEPTI */
            tempRecepti = new List<LekarskiRecept>();
            foreach(Pacijent p in PacijentiMenadzer.pacijenti)
            {
                if (p.IdPacijenta == pacijent.IdPacijenta)
                {
                    foreach (LekarskiRecept lr in p.Karton.LekarskiRecepti)
                    {
                        tempRecepti.Add(lr);
                    }
                }
            }
            this.tabelaRecepata.ItemsSource = tempRecepti;

            /* LICNI PODACI */
            this.ime.Text = izabraniPacijent.ImePacijenta;
            this.prezime.Text = izabraniPacijent.PrezimePacijenta;
            this.jmbg.Text = izabraniPacijent.Jmbg.ToString();
            if (izabraniPacijent.Pol.Equals("M"))
                this.pol.Text = "M";
            else
                this.pol.Text = "Z";
            this.brojTel.Text = izabraniPacijent.BrojTelefona.ToString(); 
            this.email.Text = izabraniPacijent.Email;
            this.adresa.Text = izabraniPacijent.AdresaStanovanja;
            this.bracnoStanje.Text = izabraniPacijent.BracnoStanje.ToString();
            //this.lekar.Text = izabraniPacijent.IzabraniLekar.ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // nazad
            this.Close();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
