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
            this.sacuvajIzmene.Visibility = Visibility.Hidden;
            this.odustani.Visibility = Visibility.Hidden;
            this.ime.IsEnabled = false;
            this.prezime.IsEnabled = false;
            this.jmbg.IsEnabled = false;
            this.pol.IsEnabled = false;
            this.brojTel.IsEnabled = false;
            this.email.IsEnabled = false;
            this.adresa.IsEnabled = false;
            this.bracnoStanje.IsEnabled = false;
            this.zanimanje.IsEnabled = false;
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
            this.zanimanje.Text = izabraniPacijent.Zanimanje;
            if (izabraniPacijent.IzabraniLekar != null)
            {
                this.lekar.Text = izabraniPacijent.IzabraniLekar.ToString();
            }
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // lekarski recepti
            MessageBox.Show(sender.ToString(), e.ToString());
            
        }

        private void izmeniBtn_Click(object sender, RoutedEventArgs e)
        {
            this.izmeniBtn.Visibility = Visibility.Hidden;
            this.sacuvajIzmene.Visibility = Visibility.Visible;
            this.odustani.Visibility = Visibility.Visible;

            this.ime.IsEnabled = true;
            this.prezime.IsEnabled = true;
            this.jmbg.IsEnabled = true;
            this.pol.IsEnabled = true;
            this.brojTel.IsEnabled = true;
            this.email.IsEnabled = true;
            this.adresa.IsEnabled = true;
            this.bracnoStanje.IsEnabled = true;
            this.zanimanje.IsEnabled = true;
        }

        private void odustani_Click(object sender, RoutedEventArgs e)
        {
            this.sacuvajIzmene.Visibility = Visibility.Hidden;
            this.odustani.Visibility = Visibility.Hidden;
            this.izmeniBtn.Visibility = Visibility.Visible;

            this.ime.IsEnabled = false;
            this.prezime.IsEnabled = false;
            this.jmbg.IsEnabled = false;
            this.pol.IsEnabled = false;
            this.brojTel.IsEnabled = false;
            this.email.IsEnabled = false;
            this.adresa.IsEnabled = false;
            this.bracnoStanje.IsEnabled = false;
            this.zanimanje.IsEnabled = false;
        }
    }
}
