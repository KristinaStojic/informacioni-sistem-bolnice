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
    /// Interaction logic for ZakaziTerminLekar.xaml
    /// </summary>
    public partial class ZakaziTerminLekar : Window
    {
        public ZakaziTerminLekar()
        {
            InitializeComponent();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                pacijenti.Items.Add(p.ImePacijenta + " " + p.PrezimePacijenta + " " + p.Jmbg);
            }

            foreach (Sala s in SaleMenadzer.sale)
            {
                prostorije.Items.Add(s.Id);
            }
        }



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //potvrdi

            int brojTermina = TerminMenadzer.GenerisanjeIdTermina();

            string vp = vpp.Text;
            string vk = vkk.Text;

            /* if (Int32.Parse(vp) >= Int32.Parse(vk))
             {
                 MessageBox.Show("Neispravno vreme pocetka i kraja");
             }*/

            String dat = null;
            DateTime selectedDate = (DateTime)dp.SelectedDate;
            dat = selectedDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            TipTermina tp;
            if (prostorije.Text.Equals("Pregled"))
            {
                tp = TipTermina.Pregled;
            }
            else
            {
                tp = TipTermina.Operacija;
            }

            /*   Lekar l;
               if (lekari.SelectedItem.Equals("Filip Filipovic"))
               {
                   l = new Lekar(5, "Filip", "Filipovic");
               }
               else if (lekari.SelectedItem.Equals("Milica Milic"))
               {
                   l = new Lekar(2, "Milica", "Milic");
               }
               else if (lekari.SelectedItem.Equals("Nevena Nevenic"))
               {
                   l = new Lekar(3, "Nevena", "Nevenic");
               }
            */
            Lekar l = new Lekar(5, "Filip", "Filipovic");

            Sala s = SaleMenadzer.NadjiSaluPoId((int)prostorije.SelectedItem);
            String p = pacijenti.Text;

            string[] podaci = p.Split(' ');
            Pacijent pacijent = PacijentiMenadzer.PronadjiPoId(Int32.Parse(podaci[2]));

            // promeni da bude metoda u PacijentMenadzer kad se merge uradi
            /*foreach (Pacijent pac in PacijentiMenadzer.pacijenti)
            {
                if (podaci[2].Equals(pac.Jmbg))
                {
                    pacijent = pac;
                }
            }*/

            if (TerminMenadzer.SlobodanTermin(dat, vp, vk, s) == false)
            {
                MessageBox.Show("Vec postoji zakazan termin u to vreme u toj prostoriji");
            }
            else
            {
                Termin t = new Termin(brojTermina, dat, vp, vk, tp, l, s, pacijent);
                TerminMenadzer.ZakaziTerminLekar(t);
            }
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //odustani
            this.Close();
        }
    }
}
