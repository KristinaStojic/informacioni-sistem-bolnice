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
    public partial class ZdravstveniKartonPacijent : Page
    {
        public Pacijent prijavljeniPacijent;
        public List<LekarskiRecept> tempRecepti;
        public List<Anamneza> tempAnamneze;
        public ZdravstveniKartonPacijent(Pacijent izabraniPacijent)
        {
            InitializeComponent();
            this.DataContext = this;
            this.prijavljeniPacijent = izabraniPacijent;
            this.sacuvajIzmene.Visibility = Visibility.Hidden;
            this.odustani.Visibility = Visibility.Hidden;
           /* this.ime.IsEnabled = false;
            this.prezime.IsEnabled = false;
            this.jmbg.IsEnabled = false;
            this.pol.IsEnabled = false;
            this.brojTel.IsEnabled = false;
            this.email.IsEnabled = false;
            this.adresa.IsEnabled = false;
            this.bracnoStanje.IsEnabled = false;
            this.zanimanje.IsEnabled = false;
            this.lekar.IsEnabled = false;*/
            /* LEKARI OPSTE PRAKSE */
            List<Lekar> opstaPraksa = new List<Lekar>();
            foreach (Lekar l in MainWindow.lekari)
            {
                if (l.specijalizacija.Equals(Specijalizacija.Opsta_praksa))
                {
                    opstaPraksa.Add(l);
                }
            }
            this.lekar.ItemsSource = opstaPraksa;
            /* LEKARSKI RECEPTI */
            tempRecepti = new List<LekarskiRecept>();
            tempAnamneze = new List<Anamneza>();
           /* foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                if (p.IdPacijenta == pacijentt.IdPacijenta)
                {*/
           
                    foreach (LekarskiRecept lekRecepti in prijavljeniPacijent.Karton.LekarskiRecepti)
                    {
                        tempRecepti.Add(lekRecepti);
                    }
                    foreach (Anamneza anamneza in prijavljeniPacijent.Karton.Anamneze)
                    {
                        tempAnamneze.Add(anamneza);
                    }
               // }
            //}
            this.tabelaRecepata.ItemsSource = tempRecepti;
            this.prikazAnamnezi.ItemsSource = tempAnamneze;
            /* LICNI PODACI */
            this.ime.Text = izabraniPacijent.ImePacijenta;
            this.prezime.Text = izabraniPacijent.PrezimePacijenta;
            this.jmbg.Text = izabraniPacijent.Jmbg.ToString();
            if (izabraniPacijent.Pol.Equals("M"))
                this.poltxt.Text = "M";
            else
                this.poltxt.Text = "Z";
            this.brojTel.Text = izabraniPacijent.BrojTelefona.ToString(); 
            this.email.Text = izabraniPacijent.Email;
            this.adresa.Text = izabraniPacijent.AdresaStanovanja;
            this.bracStanje.Text = izabraniPacijent.BracnoStanje.ToString();
            this.zanimanje.Text = izabraniPacijent.Zanimanje;
            if (izabraniPacijent.IzabraniLekar != null)
            {
                this.lekar.Text = izabraniPacijent.IzabraniLekar.ToString();
            }
            
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // nazad
            //this.Close();
        }

        

        private void tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /* LEKARSKI RECEPTI */
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // lekarski recepti - prikazi informacije
            if (tabelaRecepata.SelectedItems.Count > 0)
            {
                LekarskiRecept lp = (LekarskiRecept)tabelaRecepata.SelectedItem;
                Page recept = new Recept(lp, prijavljeniPacijent);
                this.NavigationService.Navigate(recept);
            }
            else
            {
                MessageBox.Show("Selektujte recept za koji želite da prikažete informacije", "Upozorenje", MessageBoxButton.OK);
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LekarskiRecept lp = (LekarskiRecept)tabelaRecepata.SelectedItem;
            Page recept = new Recept(lp, prijavljeniPacijent);
            this.NavigationService.Navigate(recept);
        }
        // **********

        private void izmeniBtn_Click(object sender, RoutedEventArgs e)
        {
            this.izmeniBtn.Visibility = Visibility.Hidden;
            this.sacuvajIzmene.Visibility = Visibility.Visible;
            this.odustani.Visibility = Visibility.Visible;

            this.ime.IsEnabled = true;
            this.prezime.IsEnabled = true;
            this.jmbg.IsEnabled = true;
            this.poltxt.IsEnabled = true;
            this.brojTel.IsEnabled = true;
            this.email.IsEnabled = true;
            this.adresa.IsEnabled = true;
            this.bracStanje.IsEnabled = true;
            this.zanimanje.IsEnabled = true;
            this.lekar.IsEnabled = true;
        }

        private void odustani_Click(object sender, RoutedEventArgs e)
        {
            this.sacuvajIzmene.Visibility = Visibility.Hidden;
            this.odustani.Visibility = Visibility.Hidden;
            this.izmeniBtn.Visibility = Visibility.Visible;

            this.ime.IsEnabled = false;
            this.prezime.IsEnabled = false;
            this.jmbg.IsEnabled = false;
            this.poltxt.IsEnabled = false;
            this.brojTel.IsEnabled = false;
            this.email.IsEnabled = false;
            this.adresa.IsEnabled = false;
            this.bracStanje.IsEnabled = false;
            this.zanimanje.IsEnabled = false;
            this.lekar.IsEnabled = false;
        }

        private void sacuvajIzmene_Click(object sender, RoutedEventArgs e)
        {

            string ime = this.ime.Text;
            string prezime = this.prezime.Text;
            int jmbg = int.Parse(this.jmbg.Text);
            pol poll;
            if (this.poltxt.Equals(pol.M))
            {
                poll = pol.M;
            } else
            {
                poll = pol.Z;
            }
            long brTel = long.Parse(this.brojTel.Text);
            string eMail = this.email.Text;
            string adresa = this.adresa.Text;
            bracnoStanje brStanje = bracnoStanje.Neodredjeno;
            if (this.poltxt.Text.Equals("M"))
            {
                poll = pol.M;
                if (this.bracStanje.Text.Equals("Ozenjen"))
                {
                    brStanje = bracnoStanje.Ozenjen;
                }
                else if (this.bracStanje.Text.Equals("Neoznjen"))
                {
                    brStanje = bracnoStanje.Neozenjen;
                }
                else if (this.bracStanje.Text.Equals("Udovac"))
                {
                    brStanje = bracnoStanje.Udovac;
                }
                else if (this.bracStanje.Text.Equals("Razveden"))
                {
                    brStanje = bracnoStanje.Razveden;
                }
                else if (this.bracStanje.Text.Equals("Neodredjeno"))
                {
                    brStanje = bracnoStanje.Neodredjeno;
                }
            }
            else
            {
                poll = pol.Z;
                if (this.bracStanje.Text.Equals("Udata"))
                {
                    brStanje = bracnoStanje.Udata;
                }
                else if (this.bracStanje.Text.Equals("Neudata"))
                {
                    brStanje = bracnoStanje.Neudata;
                }
                else if (this.bracStanje.Text.Equals("Udovica"))
                {
                    brStanje = bracnoStanje.Udovica;
                }
                else if (this.bracStanje.Text.Equals("Razvedena"))
                {
                    brStanje = bracnoStanje.Razvedena;
                }
                else if (this.bracStanje.Text.Equals("Neodredjeno"))
                {
                    brStanje = bracnoStanje.Neodredjeno;
                }
            }

            string zanimanje = this.zanimanje.Text;
            Lekar l = null;
            if (this.lekar != null )
            {
                l = (Lekar)this.lekar.SelectedItem;
                //MessageBox.Show(l.ImeLek + " " + l.PrezimeLek, prijavljeniPacijent.ImePacijenta + " " + prijavljeniPacijent.PrezimePacijenta);
            }
          
            Pacijent novi = new Pacijent(prijavljeniPacijent.IdPacijenta, ime, prezime, jmbg, poll, brTel, eMail, adresa, statusNaloga.Stalni, zanimanje, brStanje);
            novi.IzabraniLekar = l; 
            PacijentiMenadzer.IzmeniNalogPacijent(prijavljeniPacijent, novi);
            PacijentiMenadzer.SacuvajIzmenePacijenta(); // ?

            this.ime.IsEnabled = false;
            this.prezime.IsEnabled = false;
            this.jmbg.IsEnabled = false;
            this.poltxt.IsEnabled = false;
            this.brojTel.IsEnabled = false;
            this.email.IsEnabled = false;
            this.adresa.IsEnabled = false;
            this.bracStanje.IsEnabled = false;
            this.zanimanje.IsEnabled = false;
            this.lekar.IsEnabled = false;

            this.sacuvajIzmene.Visibility = Visibility.Hidden;
            this.odustani.Visibility = Visibility.Hidden;
            this.izmeniBtn.Visibility = Visibility.Visible;
        }

        /* ANAMNEZE */
        private void infoAnamneza_Click(object sender, RoutedEventArgs e)
        {
            if (prikazAnamnezi.SelectedItems.Count > 0)
            {
                Anamneza anamneza = (Anamneza)prikazAnamnezi.SelectedItem;
                PrikazAnamnezePacijent anamnezaPrikaz = new PrikazAnamnezePacijent(prijavljeniPacijent, anamneza);
                this.NavigationService.Navigate(anamnezaPrikaz);
            }
            else
            {
                MessageBox.Show("Selektujte anamnezu za koju želite da prikažete informacije", "Upozorenje", MessageBoxButton.OK);
            }
        }

        private void prikazAnamnezi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Anamneza anamneza = (Anamneza)prikazAnamnezi.SelectedItem;
            PrikazAnamnezePacijent anamnezaPrikaz = new PrikazAnamnezePacijent(prijavljeniPacijent, anamneza);
            this.NavigationService.Navigate(anamnezaPrikaz);
        }

        // *********

        private void prikazUputa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void odjava_Click(object sender, RoutedEventArgs e)
        {

        }

        public void karton_Click(object sender, RoutedEventArgs e)
        {
            Page karton = new ZdravstveniKartonPacijent(prijavljeniPacijent);
            this.NavigationService.Navigate(karton);
        }

        public void zakazi_Click(object sender, RoutedEventArgs e)
        {
            Lekar l = null;
            Page zakaziTermin = new ZakaziTermin(l);
            this.NavigationService.Navigate(zakaziTermin);
        }

        public void uvid_Click(object sender, RoutedEventArgs e)
        {
            // TODO ispraviti --> uvid u zakazane poseban page
            Page uvid = new PrikaziTermin();
            this.NavigationService.Navigate(uvid);
        }
    }
}
