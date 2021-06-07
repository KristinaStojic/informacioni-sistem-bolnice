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
    /// Interaction logic for DetaljiAnamneze.xaml
    /// </summary>
    public partial class DetaljiAnamneze : Window
    {
        Pacijent pacijent;
        Anamneza stara;
        Termin termin;
        bool popunjeno = false;
        public DetaljiAnamneze(Anamneza izabranaAnamneza, Termin Izabranitermin)
        {
            InitializeComponent();
            this.termin = Izabranitermin;
            PopuniPodatke(izabranaAnamneza);
            
        }

        private void PopuniPodatke(Anamneza izabranaAnamneza)
        {
            this.stara = izabranaAnamneza;
        
            foreach (Pacijent pac in PacijentiServis.pacijenti())
            {
                if (pac.IdPacijenta == izabranaAnamneza.IdPacijenta)
                {
                    this.datum.SelectedDate = DateTime.Parse(izabranaAnamneza.Datum);
                    this.lekar.Text = termin.Lekar.ImeLek + " " + termin.Lekar.PrezimeLek;
                    this.bol.Text = izabranaAnamneza.OpisBolesti;
                    this.terap.Text = izabranaAnamneza.Terapija;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //sacuvaj
            if (popunjeno == true)
            {
                string terapijaPacijenta = terap.Text;
                string bolestPacijenta = bol.Text;
                String datumPregleda = null;
                DateTime selectedDate = (DateTime)datum.SelectedDate;
                datumPregleda = selectedDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                Anamneza nova = new Anamneza(stara.IdAnamneze, stara.IdPacijenta, datumPregleda, bolestPacijenta, terapijaPacijenta, termin.Lekar.IdLekara, termin.IdTermin);
                ZdravstveniKartonServis.IzmeniAnamnezu(stara, nova);

                TerminServisLekar.sacuvajIzmene();
                PacijentiServis.SacuvajIzmenePacijenta();
                SaleServis.sacuvajIzmjene();
                this.Close();
            }
            else
            {
                MessageBox.Show("Popunite sva polja!");
            }

            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //odustani
            this.Close();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl)) //Sacuvaj
            {
                Button_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) //Nazad
            {
                Button_Click_1(sender, e);
            }
        }


        private void terap_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void bol_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void postaviDugme()
        {
            if (this.terap.Text != null)
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
            if (this.terap.Text.Trim().Equals("") || this.bol.Text.Trim().Equals(""))
            {
                this.potvrdi.IsEnabled = false;
                popunjeno = false;
            }
            else if (!this.terap.Text.Trim().Equals("") && !this.bol.Text.Trim().Equals(""))
            {
                this.potvrdi.IsEnabled = true;
                popunjeno = true;
            }
        }
    }
}
