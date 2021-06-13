using Model;
using Projekat.Interfejsi;
using Projekat.Model;
using Projekat.Servis;
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
        bool popunjeno = false;
        PacijentiServis servis = new PacijentiServis();
        List<Pacijent> pacijenti;
        ZdravstveniKartonMenadzer menadzer = new ZdravstveniKartonMenadzer();

        public DetaljiUputa(Uput izabraniUput)
        {
            InitializeComponent();
            this.uput = izabraniUput;

            this.potvrdi.Visibility = Visibility.Hidden;
            this.odustani.Visibility = Visibility.Hidden;

            pacijenti = servis.PronadjiSve(); 

            /*if(izabraniUput.TipUputa == tipUputa.SpecijalistickiPregled)
            {
                specijalistickiTab.IsSelected = true;
                PopuniPodatkeUputa();
            }
            else if(izabraniUput.TipUputa == tipUputa.StacionarnoLecenje)
            {
                stacinarnoTab.IsSelected = true;
                PopuniPodatkeBolnickoLecenje();
            }
            else if (izabraniUput.TipUputa == tipUputa.Laboratorija)
            {
                labTab.IsSelected = true;
                PopuniPodatkeLaboratorija();
            }*/

            this.potvrdi.IsEnabled = false;



        }

        private void PopuniPodatkeLaboratorija()
        {
            NadjiPacijentaLaboratorija(uput.idPacijenta);
            NadjiLekaraLaboratorija(uput);
            this.datumLab.SelectedDate = DateTime.Parse(uput.datumIzdavanja);
            this.napomenaLab.Text = uput.opisPregleda;
        }

        private void NadjiPacijentaLaboratorija(int idPacijenta)
        {
            foreach (Pacijent pacijent in pacijenti)
            {
                if (pacijent.IdPacijenta == idPacijenta)
                {
                    this.imeLab.Text = pacijent.ImePacijenta;
                    this.prezimeLab.Text = pacijent.PrezimePacijenta;
                    this.jmbgLab.Text = pacijent.Jmbg.ToString();
                }
            }
        }

        private void NadjiLekaraLaboratorija(Uput izabraniUput)
        {
            foreach (Lekar lekar in LekariMenadzer.lekari)
            {
                if (lekar.IdLekara == izabraniUput.IdLekaraKojiIzdajeUput)
                {
                    this.lekarLab.Text = lekar.ImeLek + " " + lekar.PrezimeLek;
                }
            }
        }
        
        private void PopuniPodatkeBolnickoLecenje()
        {
            NadjiPacijentaBolnickoLecenje(uput.idPacijenta);
            NadjiLekaraKojiIzdajeBolnickoLecenje(uput);
           // this.datumKraja.DisplayDateStart = datumPocetka.SelectedDate;

            this.datumPocetka.SelectedDate = DateTime.Parse(uput.datumPocetkaLecenja);
            this.datumKraja.SelectedDate = DateTime.Parse(uput.datumKrajaLecenja);
            this.napomenaPregelda.Text = uput.opisPregleda;
            this.brojKreveta.Text = uput.brojKreveta.ToString();
            this.brojSobe.Text = uput.brojSobe.ToString();

        }
        private void PopuniPodatkeUputa()
        {
            NadjiPacijenta(uput.idPacijenta);
            NadjiLekaraKojiIzdajeUput(uput);
            NadjiLekaraSpecijalistu(uput);
            this.datum.SelectedDate = DateTime.Parse(uput.datumIzdavanja);
            this.napomena.Text = uput.opisPregleda;
        }
        private void NadjiPacijenta(int idPacijenta)
        {
            foreach(Pacijent pacijent in pacijenti)
            {
                if(pacijent.IdPacijenta == idPacijenta)
                {
                    this.ime.Text = pacijent.ImePacijenta;
                    this.prezime.Text = pacijent.PrezimePacijenta;
                    this.jmbg.Text = pacijent.Jmbg.ToString();
                }
            }
        }private void NadjiPacijentaBolnickoLecenje(int idPacijenta)
        {
            foreach(Pacijent pacijent in pacijenti)
            {
                if(pacijent.IdPacijenta == idPacijenta)
                {
                    this.imePacijenta.Text = pacijent.ImePacijenta;
                    this.prezimePacijenta.Text = pacijent.PrezimePacijenta;
                    this.jmbgPacijenta.Text = pacijent.Jmbg.ToString();
                }
            }
        }

        private void NadjiLekaraKojiIzdajeUput(Uput izabraniUput)
        {
            foreach(Lekar lekar in LekariMenadzer.lekari)
            {
                if(lekar.IdLekara == izabraniUput.IdLekaraKojiIzdajeUput)
                {
                    this.lekar.Text = lekar.ImeLek + " " + lekar.PrezimeLek;
                }
            }
        }
        private void NadjiLekaraKojiIzdajeBolnickoLecenje(Uput izabraniUput)
        {
            foreach(Lekar lekar in LekariMenadzer.lekari)
            {
                if(lekar.IdLekara == izabraniUput.IdLekaraKojiIzdajeUput)
                {
                    this.Lekar.Text = lekar.ImeLek + " " + lekar.PrezimeLek;
                }
            }
        }
        private void NadjiLekaraSpecijalistu(Uput izabraniUput)
        {
            foreach(Lekar lekar in LekariMenadzer.lekari)
            {
                if(lekar.IdLekara == izabraniUput.IdLekaraKodKogSeUpucuje)
                {
                    this.specijalista.Text = lekar.ImeLek + " " + lekar.PrezimeLek + "-" + lekar.specijalizacija;
                }
            }
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PotvrdiLecenje_Click(object sender, RoutedEventArgs e)
        {
            if (popunjeno)
            {

                //Uput noviUput = new Uput(uput.IdUputa, uput.idPacijenta, uput.IdLekaraKojiIzdajeUput, uput.brojSobe, uput.brojKreveta, NadjiNoviDatumKraja(), uput.datumPocetkaLecenja, uput.datumIzdavanja, uput.opisPregleda, uput.TipUputa);
                Uput noviUput = new Uput(uput.IdUputa, uput.idPacijenta, uput.IdLekaraKojiIzdajeUput, uput.brojSobe, uput.brojKreveta, NadjiNoviDatumKraja(), uput.datumPocetkaLecenja, uput.datumIzdavanja, uput.opisPregleda);
                noviUput.TipUputa = new BolnickoLecenje();
                //ZdravstveniKartonMenadzer.IzmeniUput(uput, noviUput);
                PacijentiServis.SacuvajIzmenePacijenta();

                //Uput noviUput = new Uput(uput.IdUputa, uput.idPacijenta, uput.IdLekaraKojiIzdajeUput, uput.brojSobe, uput.brojKreveta, NadjiNoviDatumKraja(), uput.datumPocetkaLecenja, uput.datumIzdavanja, uput.opisPregleda, uput.TipUputa);
                menadzer.IzmeniUput(uput, noviUput);
                //pacijentiMenadzer.SacuvajIzmene();

                this.Close();
            }
            else
            {
                MessageBox.Show("Niste uneli ispravne podatke!");
            }
           
        }

        private string NadjiNoviDatumKraja()
        {
            String formatiranDatum = null;
            DateTime? selectedDate = datumKraja.SelectedDate;
            if (selectedDate.HasValue)
            {
                formatiranDatum = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            }
            return formatiranDatum;
        }

        private void stacinarnoTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl)) //Anamneza
            {
                PotvrdiLecenje_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) //Recepti
            {
                this.Close();
            }
        }

        private void specijalistickiTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) //Recepti
            {
                this.Close();
            }
        }

       

        private void datumPocetka_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void datumKraja_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datumKraja.Text.Length <= 0)
            {
                potvrdi.IsEnabled = false;
                popunjeno = false;

            }
            else
            {
                popunjeno = true;
                potvrdi.IsEnabled = true;
            }
        }

        private void labTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) 
            {
                this.Close();
            }
        }

        private void datumKraja_LostFocus(object sender, RoutedEventArgs e)
        {
            postaviDugme();
        }

        private void postaviDugme()
        {
            if (this.datumKraja.Text != null)
            {
                izvrsiPostavljanje();
            }
            else
            {
                this.potvrdi.IsEnabled = false;
                this.popunjeno = false;
            }
        }
        private void izvrsiPostavljanje()
        {
            if (this.datumKraja.Text.Trim().Equals(""))
            {
                this.potvrdi.IsEnabled = false;
                popunjeno = false;
            }
            else if (!this.datumKraja.Text.Trim().Equals(""))
            {
                this.potvrdi.IsEnabled = true;
                popunjeno = true;
            }
        }
    }
}
