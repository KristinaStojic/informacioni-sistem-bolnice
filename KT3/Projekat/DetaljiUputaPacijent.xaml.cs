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
    public partial class DetaljiUputaPacijent : Page
    {
        private static int idPacijent;
        public DetaljiUputaPacijent(int idPrijavljenogPacijenta, Uput izabraniUput)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            Pacijent prijavljeniPacijent = PacijentiServis.PronadjiPoId(idPacijent);
            this.ime.Text = prijavljeniPacijent.ImePacijenta;
            this.prezime.Text = prijavljeniPacijent.PrezimePacijenta;
            this.jmbg.Text = prijavljeniPacijent.Jmbg.ToString();

            this.datum.Text = izabraniUput.datumIzdavanja;
            Lekar lekarKodKogSeUpucuje =  PronadjiLekaraPoId(izabraniUput.IdLekaraKodKogSeUpucuje);
            this.LekarKodKogSeUpucuje.Text = lekarKodKogSeUpucuje.ToString();
            Lekar lekarKojiIzdajeUput = PronadjiLekaraPoId(izabraniUput.IdLekaraKojiIzdajeUput);
            this.podaciLekara.Text = lekarKojiIzdajeUput.ToString();
            this.Napomena.Text = izabraniUput.opisPregleda;

            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PrikaziTermin.AktivnaTemaPagea(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
        }

        private static Lekar PronadjiLekaraPoId(int idLekara)
        {
            foreach (Lekar lekar in LekariServis.NadjiSveLekare())
            {
                if (lekar.IdLekara == idLekara)
                {
                    return lekar;
                }
            }
            return null;
        }

        private void odjava_Click(object sender, RoutedEventArgs e)
        {
            /*Page odjava = new PrijavaPacijent();
            this.NavigationService.Navigate(odjava);*/
            PacijentPagesServis.odjava_Click(this);
        }

        public void karton_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.karton_Click(this, idPacijent);
        }

        public void zakazi_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.zakazi_Click(this, idPacijent);
        }
        public void uvid_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.uvid_Click(this, idPacijent);
        }

        private void pocetna_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.pocetna_Click(this, idPacijent);
        }

        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.anketa_Click(this, idPacijent);
        }
      
        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.Korisnik_Click(this, idPacijent);
        }
        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.PromeniTemu(SvetlaTema, tamnaTema);
        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            /*var app = (App)Application.Current;
            string eng = "en-US";
            string srb = "sr-LATN";
            MenuItem mi = (MenuItem)sender;
            if (mi.Header.Equals("en-US"))
            {
                mi.Header = "sr-LATN";
                app.ChangeLanguage(eng);
            }
            else
            {
                mi.Header = "en-US";
                app.ChangeLanguage(srb);
            }*/
            PacijentPagesServis.Jezik_Click(Jezik);

        }
    }
}
