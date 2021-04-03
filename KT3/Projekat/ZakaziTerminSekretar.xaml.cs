using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ZakaziTerminSekretar.xaml
    /// </summary>
    public partial class ZakaziTerminSekretar : Window
    {
        public ZakaziTerminSekretar()
        {
            InitializeComponent();
            foreach (Sala s in SaleMenadzer.NadjiSveSale()) // IZMENIIIIIIIIIIIIIIIIIIIIII
            {
                prostorije.Items.Add(s.Id);
            }

            foreach (Pacijent p in PrikaziPacijenta.PacijentiTabela)
            {
                pacijenti.Items.Add(p.ImePacijenta + " " +  p.PrezimePacijenta + " " + p.Jmbg);
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

            Sala s =  SaleMenadzer.NadjiSaluPoId((int)prostorije.SelectedItem);
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
            
            if (TerminMenadzer.SlobodanTermin(dat, vp,vk,s) == false)
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
