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
using Projekat.Model;

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
            foreach (Sala s in SaleMenadzer.sale)
            {
                prostorije.Items.Add(s.Id);
            }

            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                pacijenti.Items.Add(p.ImePacijenta + " " +  p.PrezimePacijenta + " " + p.Jmbg);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            int brojTermina = TerminMenadzer.GenerisanjeIdTermina();
            
            string vp = vremePocetka.Text;
            string vk = vremeKraja.Text;


            /*if (Convert.ToInt32(vp) >= Convert.ToInt32(vk))
            {
                MessageBox.Show("Neispravno vreme pocetka i kraja");
            }*/

            String formatted = null;
            DateTime? selectedDate = datum.SelectedDate;
            Console.WriteLine(selectedDate);
            if (selectedDate.HasValue)
            {
                formatted = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
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

            String p = pacijenti.Text;
            string[] podaci = p.Split(' ');
            Pacijent pacijent = PacijentiMenadzer.PronadjiPoId(Int32.Parse(podaci[2]));

            Sala s = SaleMenadzer.NadjiSaluPoId((int)prostorije.SelectedItem);  
            Termin t = new Termin(brojTermina, formatted, vp, vk, tp, l, s, pacijent);

            int flag = 0;
            if (s.zauzetiTermini.Count != 0)        // ako postoje zauzeti termini
            {
                foreach (ZauzeceSale zauzece in s.zauzetiTermini)
                {
                    if (t.Prostorija.Id.Equals(s.Id) && formatted.Equals(zauzece.datumTermina) && vp.Equals(zauzece.pocetakTermina) && vk.Equals(zauzece.krajTermina))
                    {
                        MessageBox.Show("Vec postoji termin");
                        this.Close();
                        flag = 1;                    
                    }
                }

                if (flag == 0)
                {
                    TerminMenadzer.ZakaziTerminSekretar(t);
                    ZauzeceSale z = new ZauzeceSale(vp, vk, formatted, t.IdTermin);
                    s.zauzetiTermini.Add(z);
                    //SaleMenadzer.sacuvajIzmjene();
                }

            }
            else    // ako ne postoje zauzeti termini
            {
                TerminMenadzer.ZakaziTerminSekretar(t);
                ZauzeceSale z = new ZauzeceSale(vp, vk, formatted, t.IdTermin);
                s.zauzetiTermini.Add(z);
               // SaleMenadzer.sacuvajIzmjene();        
            }

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // kreiranje guest naloga
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DodajPacijentaGuest dodavanje = new DodajPacijentaGuest(this);  // prosledujumeo u DodajPacijentaGuest konstrukotr klase ZakaziTermniSekretar
            dodavanje.Show();
        }

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
