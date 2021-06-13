using Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat
{

    public partial class DetaljiSpecijalistickogUputa : Page
    {
        private static int idPacijent;
        private static Pacijent prijavljeniPacijent;
        PacijentiServis servis = new PacijentiServis();
        public DetaljiSpecijalistickogUputa(int idPrijavljenogPacijenta, Uput stacionarnoLecenje)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            prijavljeniPacijent = servis.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);

            Lekar lekarKojiIzdajeUput = LekariServis.NadjiPoId(stacionarnoLecenje.IdLekaraKojiIzdajeUput);
            this.podaciLekara.Text = lekarKojiIzdajeUput.ToString();
            this.ime.Text = prijavljeniPacijent.ImePacijenta;
            this.prezime.Text = prijavljeniPacijent.PrezimePacijenta;
            this.jmbg.Text = prijavljeniPacijent.Jmbg.ToString();

            this.datum.Text = stacionarnoLecenje.datumPocetkaLecenja;
            this.datumKraj.Text = stacionarnoLecenje.datumKrajaLecenja;
            this.Napomena.Text = stacionarnoLecenje.opisPregleda;
            this.soba.Text = stacionarnoLecenje.brojSobe.ToString();
            this.krevet.Text = stacionarnoLecenje.brojKreveta.ToString();
        }

        private void odjava_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.odjava_Click(this);
        }

        public void karton_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.karton_Click(this, idPacijent);
        }

        public void zakazi_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.zakazi_Click(this, idPacijent);
        }
        public void uvid_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.uvid_Click(this, idPacijent);
        }

        private void pocetna_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.pocetna_Click(this, idPacijent);
        }

        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.anketa_Click(this, idPacijent);
        }

        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Korisnik_Click(this, idPacijent);
        }
        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.PromeniTemu(SvetlaTema, tamnaTema);
        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Jezik_Click(Jezik);

        }
    }
}
