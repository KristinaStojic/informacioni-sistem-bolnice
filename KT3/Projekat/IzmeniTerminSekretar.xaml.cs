using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Model;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmeniTerminSekretar.xaml
    /// </summary>
    public partial class IzmeniTerminSekretar : Window
    {
        public Termin termin;

        public IzmeniTerminSekretar(Termin izabraniTermin)
        {
            InitializeComponent();
            this.termin = izabraniTermin;

            foreach (Sala s in SaleMenadzer.sale)
            {
                prostorije.Items.Add(s.Id);
            }

            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                pacijenti.Items.Add(p.ImePacijenta + " " + p.PrezimePacijenta + " " + p.Jmbg);
            }

            if (izabraniTermin != null)
            {
                termin.IdTermin = izabraniTermin.IdTermin;
                vremePocetka.Text = izabraniTermin.VremePocetka;
                vremeKraja.Text = izabraniTermin.VremeKraja;

                lekari.SelectedItem = izabraniTermin.Lekar;

                pacijenti.Text = izabraniTermin.Pacijent.ImePacijenta + " " + izabraniTermin.Pacijent.PrezimePacijenta + " " + izabraniTermin.Pacijent.Jmbg;
                prostorije.SelectedItem = izabraniTermin.Prostorija.Id.ToString();

                // Console.WriteLine(Convert.ToInt32(vremeKraja.Text));
                // Console.WriteLine(Int32.Parse(vremePocetka.Text));

                TipTermina tp;
                if (izabraniTermin.tipTermina.Equals(TipTermina.Operacija))
                {
                    tip.SelectedIndex = 1;
                }
                else if (izabraniTermin.tipTermina.Equals(TipTermina.Pregled))
                {
                    tip.SelectedIndex = 0;
                }

                tp = izabraniTermin.tipTermina;

                // NE RADI IZMENA DATUMA !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //string selektovano = izabraniTermin.Datum;
                //Console.WriteLine(selektovano);
                //datum.Text = DateTime.Parse(izabraniTermin.Datum);
                datum.SelectedDate = DateTime.Parse(izabraniTermin.Datum);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string vp = vremePocetka.Text;
            string vk = vremeKraja.Text;

            String dat = null;
            DateTime? selectedDate = datum.SelectedDate;
            if (selectedDate.HasValue)
            {
                dat = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }

                TipTermina tp;
            if (tip.Text.Equals("Pregled"))
            {
                tp = TipTermina.Pregled;
            }
            else
            {
                tp = TipTermina.Operacija;
            }

            Lekar l = new Lekar(5, "Filip", "Filipovic");

            Sala s = SaleMenadzer.NadjiSaluPoId((int)prostorije.SelectedItem);
            
            String p = pacijenti.Text;
            string[] podaci = p.Split(' ');
            Pacijent pacijent = PacijentiMenadzer.PronadjiPoId(Int32.Parse(podaci[2]));

            Termin t = new Termin(termin.IdTermin, dat, vp, vk, tp, l, s, pacijent);
            TerminMenadzer.IzmeniTerminSekretar(termin, t);
            // zameni i zauzeca ako treba
            SaleMenadzer.sacuvajIzmjene();  // zbog zauzeca novog termina
            
            this.Close();
        }

        // odustani
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {            
            this.Close();
        }

        // pravljenje guest naloga
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DodajPacijentaGuest dodavanje = new DodajPacijentaGuest();          // proslediti this         !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            dodavanje.Show();
        }

        private void Button_LostFocus(object sender, RoutedEventArgs e)
        {
            pacijenti.Items.Clear();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                pacijenti.Items.Add(p.ImePacijenta + " " + p.PrezimePacijenta + " " + p.Jmbg);
            }
            int ukupno = PacijentiMenadzer.pacijenti.Count;
            pacijenti.SelectedIndex = ukupno - 1;
        }

        // dodati novi dijalog DdoajPacijentaGuest za izmenu termina         !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public void AzurirajComboBox()
        {
            pacijenti.Items.Clear();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                pacijenti.Items.Add(p.ImePacijenta + " " + p.PrezimePacijenta + " " + p.Jmbg);
            }
            int ukupno = PacijentiMenadzer.pacijenti.Count;
            pacijenti.SelectedIndex = ukupno - 1;
        }
    }
}
