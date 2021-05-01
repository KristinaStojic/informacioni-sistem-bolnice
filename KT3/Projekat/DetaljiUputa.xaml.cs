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
    /// Interaction logic for DetaljiUputa.xaml
    /// </summary>
    public partial class DetaljiUputa : Window
    {
        Uput uput;
        Termin termin;
        public DetaljiUputa(Uput izabraniUput, Termin izabraniTermin)
        {
            InitializeComponent();
            this.uput = izabraniUput;
            this.termin = izabraniTermin;
            if(izabraniUput.TipUputa == tipUputa.SpecijallistickiPregled)
            {
                specijalistickiTab.IsSelected = true;
            }

            popuniPodatke();

            
        }
        private void popuniPodatke()
        {
            nadjiPacijenta(uput.idPacijenta);
            nadjiLekaraKojiIzdajeUput(uput);
            nadjiLekaraSpecijalistu(uput);
            this.datum.SelectedDate = DateTime.Parse(uput.datumIzdavanja);
            this.napomena.Text = uput.opisPregleda;
        }
        private void nadjiPacijenta(int idPacijenta)
        {
            foreach(Pacijent pacijent in PacijentiMenadzer.pacijenti)
            {
                if(pacijent.IdPacijenta == idPacijenta)
                {
                    this.ime.Text = pacijent.ImePacijenta;
                    this.prezime.Text = pacijent.PrezimePacijenta;
                    this.jmbg.Text = pacijent.Jmbg.ToString();
                }
            }
        }

        private void nadjiLekaraKojiIzdajeUput(Uput izabraniUput)
        {
            foreach(Lekar lekar in MainWindow.lekari)
            {
                if(lekar.IdLekara == izabraniUput.IdLekaraKojiIzdajeUput)
                {
                    this.lekar.Text = lekar.ImeLek + " " + lekar.PrezimeLek;
                }
            }
        }
        private void nadjiLekaraSpecijalistu(Uput izabraniUput)
        {
            foreach(Lekar lekar in MainWindow.lekari)
            {
                if(lekar.IdLekara == izabraniUput.IdLekaraKodKogSeUpucuje)
                {
                    this.specijalista.Text = lekar.ImeLek + " " + lekar.PrezimeLek;
                }
            }
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
