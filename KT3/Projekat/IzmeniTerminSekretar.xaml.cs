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
            if (izabraniTermin != null)
            {
                vremePocetka.Text = izabraniTermin.VremePocetka;
                vremeKraja.Text = izabraniTermin.VremeKraja;
                
                // namestiti lekare, pacijente, prostorije, datum !!!!!!!!!!!
                lekari.Text = izabraniTermin.Lekar.ToString();

                // pacijenti
                pacijenti.SelectedItem = izabraniTermin.Pacijent;
                //this.idPacijenta.Text = izabraniTermin.Pacijent.Jmbg.ToString();
                lekari.SelectedItem = izabraniTermin.Lekar;

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
                
                //prostorije.SelectedIndex = izabraniTermin.Prostorija;
                //datum.SelectedDate = DateTime.Parse(izabraniTermin.Datum);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int brojTermina = TerminMenadzer.GenerisanjeIdTermina();

            string vp = vremePocetka.Text;
            string vk = vremeKraja.Text;

            /* if (Int32.Parse(vp) >= Int32.Parse(vk))
             {
                 MessageBox.Show("Neispravno vreme pocetka i kraja");
             }*/

            String dat = null;
            DateTime selectedDate = (DateTime)datum.SelectedDate;
            dat = selectedDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

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

            // promeni da bude metoda u PacijentMenadzer kad se merge uradi
            foreach (Pacijent pac in PacijentiMenadzer.pacijenti)
            {
                if (podaci[2].Equals(pac.Jmbg))
                {
                    pacijent = pac;
                }
            }

            if (TerminMenadzer.SlobodanTermin(dat, vp, vk, s) == false)
            {
                MessageBox.Show("Vec postoji zakazan termin u to vreme u toj prostoriji");
            }
            else
            {
                Termin t = new Termin(brojTermina, dat, vp, vk, tp, l, s, pacijent);
                TerminMenadzer.ZakaziTerminSekretar(t);
            }
            this.Close();
        }

        // odustani
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        private void datum_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
