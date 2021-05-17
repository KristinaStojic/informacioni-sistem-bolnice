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
    /// Interaction logic for DodajZahtevZaGodisnji.xaml
    /// </summary>
    public partial class DodajZahtevZaGodisnji : Window
    {
        int idLekara;
        public DodajZahtevZaGodisnji(int id)
        {
            InitializeComponent();
            this.idLekara = id;
            popuniPodatke();
        }

        private void popuniPodatke()
        {
            foreach(Lekar lekar in LekariMenadzer.lekari)
            {
                if(lekar.IdLekara == idLekara)
                {
                    this.ime.Text = lekar.ImeLek;
                    this.prezime.Text = lekar.PrezimeLek; 
                }
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int idZahteva = LekariMenadzer.GenerisanjeIdZahtevaZaOdmor(idLekara);
            string napomena = this.napomena.Text;
            /*pocetak*/
            string pocetakOdmora = NadjiDatumPocetkaOdmora();
            
            /*kraj*/
            string krajOdmora = NadjiDatumKrajaOdmora();
            

            /*broj dana*/
            int brojDanaOdmora = OdrediBrojDanaOdmora();

            /*lekar*/
            Lekar lekar = NadjiLekara();

            ZahtevZaGodisnji zahtev = new ZahtevZaGodisnji(idZahteva,lekar,pocetakOdmora,krajOdmora,brojDanaOdmora,napomena);
            LekariMenadzer.DodajZahtev(zahtev);
            this.Close();
        }

        private string NadjiDatumPocetkaOdmora()
        {
            string pocetakOdmora = null;
            DateTime? datumPocetka = this.pocetak.SelectedDate;
            if (datumPocetka.HasValue)
            {
                pocetakOdmora = datumPocetka.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }

            return pocetakOdmora;
        }


        private string NadjiDatumKrajaOdmora()
        {
            string krajOdmora = null;
            DateTime? datumKraja = this.kraj.SelectedDate;
            if (datumKraja.HasValue)
            {
                krajOdmora = datumKraja.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }

            return krajOdmora;
        }

        private int OdrediBrojDanaOdmora()
        {
            DateTime kraj = (DateTime)this.kraj.SelectedDate;
            DateTime pocetak = (DateTime)this.pocetak.SelectedDate;

            TimeSpan brojDana = kraj.Subtract(pocetak);
            int brojDanaOdmora = brojDana.Days;

            return brojDanaOdmora;
        }

        private Lekar NadjiLekara()
        {
            Lekar lekar = null;
            foreach (Lekar l in LekariMenadzer.lekari)
            {
                if (l.IdLekara == idLekara)
                {
                    lekar = l;
                }
            }
            return lekar;
        }
    }
}
