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
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for DetaljiUputaSekretar.xaml
    /// </summary>
    public partial class DetaljiUputaSekretar : Window
    {
        Uput uput;

        public DetaljiUputaSekretar(Uput izabraniUput)
        {
            InitializeComponent();
            this.uput = izabraniUput;
            PopuniPodatkeUputa();
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
            List<Pacijent> pacijenti = PacijentiServis.PronadjiSve();
            foreach (Pacijent pacijent in pacijenti)
            {
                if (pacijent.IdPacijenta == idPacijenta)
                {
                    this.ime.Text = pacijent.ImePacijenta;
                    this.prezime.Text = pacijent.PrezimePacijenta;
                    this.jmbg.Text = pacijent.Jmbg.ToString();
                }
            }
        }

        private void NadjiLekaraKojiIzdajeUput(Uput izabraniUput)
        {
            List<Lekar> lekari = LekariServis.NadjiSveLekare();
            foreach (Lekar lekar in lekari)
            {
                if (lekar.IdLekara == izabraniUput.IdLekaraKojiIzdajeUput)
                {
                    this.lekar.Text = lekar.ImeLek + " " + lekar.PrezimeLek;
                }
            }
        }

        private void NadjiLekaraSpecijalistu(Uput izabraniUput)
        {
            List<Lekar> lekari = LekariServis.NadjiSveLekare();
            foreach (Lekar lekar in lekari)
            {
                if (lekar.IdLekara == izabraniUput.IdLekaraKodKogSeUpucuje)
                {
                    this.specijalista.Text = lekar.ImeLek + " " + lekar.PrezimeLek + "-" + lekar.specijalizacija;
                }
            }
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
