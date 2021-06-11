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
using System.Windows.Shapes;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for DodajZahtevZaGodisnji.xaml
    /// </summary>
    public partial class DodajZahtevZaGodisnji : Window
    {
        int idLekara;
        public bool popunjeno = false;
        LekariServis lekariServis = new LekariServis();
        public DodajZahtevZaGodisnji(int id)
        {
            InitializeComponent();
            this.idLekara = id;
            this.potvrdi.IsEnabled = false;
            popuniPodatke();
        }

        private void popuniPodatke()
        {
            foreach(Lekar lekar in lekariServis.NadjiSveLekare())
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
            try
            {
                DateTime kraj = (DateTime)this.kraj.SelectedDate;
                DateTime pocetak = (DateTime)this.pocetak.SelectedDate;

                if (pocetak >= kraj || pocetak < DateTime.Now.Date)
                {
                    MessageBox.Show("Niste uneli ispravne datume!");
                }
                else if (popunjeno)
                {
                    int idZahteva = lekariServis.GenerisanjeIdZahtevaZaOdmor(idLekara);
                    string napomena = this.napomena.Text;
                    /*pocetak*/
                    string pocetakOdmora = NadjiDatumPocetkaOdmora();

                    /*kraj*/
                    string krajOdmora = NadjiDatumKrajaOdmora();


                    /*broj dana*/
                    int brojDanaOdmora = OdrediBrojDanaOdmora();

                    /*lekar*/
                    Lekar lekar = NadjiLekara();

                    ZahtevZaGodisnji zahtev = new ZahtevZaGodisnji(idZahteva, lekar, pocetakOdmora, krajOdmora, brojDanaOdmora, napomena);
                    lekariServis.DodajZahtev(zahtev);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Popunite sva polja!");
                }
            }
            catch
            {
                MessageBox.Show("Popunite sva polja!");
            }
            
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
            int brojDanaOdmora = 0;

            foreach (Lekar lekar in lekariServis.NadjiSveLekare())
            {
                if (lekar.IdLekara == idLekara)
                {
                    foreach (RadniDan dan in lekar.RadniDani)
                    {
                        if ( (pocetak <= DateTime.Parse(dan.Datum)) &&
                             (kraj >= DateTime.Parse(dan.Datum)) )
                        {
                            brojDanaOdmora++;
                        }
                    }
                }
            }
            
            return brojDanaOdmora;
        }

        private Lekar NadjiLekara()
        {
            Lekar lekar = null;
            foreach (Lekar l in lekariServis.NadjiSveLekare())
            {
                if (l.IdLekara == idLekara)
                {
                    lekar = l;
                }
            }
            return lekar;
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_2(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_3(sender, e);
            }
        }

        private void pocetak_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            postaviDugme();
        }

        private void kraj_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            postaviDugme();
        }

        private void napomena_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }


        private void postaviDugme()
        {
            if (this.napomena.Text != null && this.kraj.Text.Length > 0  && this.pocetak.Text.Length > 0)
            {
                izvrsiPostavljanje();
            }
            else
            {
                this.potvrdi.IsEnabled = false;
            }
        }
        private void izvrsiPostavljanje()
        {
            if (this.napomena.Text.Trim().Equals("") || this.kraj.Text.Trim().Equals("") || this.pocetak.Text.Trim().Equals(""))
            {
                this.potvrdi.IsEnabled = false;
                popunjeno = false;
            }
            else if (!this.napomena.Text.Trim().Equals("") && !this.kraj.Text.Trim().Equals("") && !this.pocetak.Text.Trim().Equals(""))
            {
                this.potvrdi.IsEnabled = true;
                popunjeno = true;
            }
        }
    }
}
