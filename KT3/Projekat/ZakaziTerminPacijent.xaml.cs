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
using static Model.Termin;  // ?

namespace Projekat
{
    public partial class ZakaziTerminPacijent : Window
    {
        public ZakaziTerminPacijent()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // PrikaziTermin pt = new PrikaziTermin();
            //pt.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                int brojTermina = int.Parse(text1.Text);
                String formatted = null;
                DateTime? selectedDate = dp.SelectedDate;
                Console.WriteLine(selectedDate);
                if (selectedDate.HasValue)
                {
                    formatted = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                }

                String vp = text2.Text;
                // TODO 
                String vk;
                String hh = vp.Substring(0, 2);
                String min = vp.Substring(3);
                if (min == "30")
                {
                    int vkInt = int.Parse(hh);
                    vkInt++;
                    if (vkInt <= 9)
                    {
                        vk = "0" + vkInt.ToString() + ":00";
                    } else
                    {
                        vk = vkInt.ToString() + ":00";
                    }
                }
                else
                {
                    vk = hh + ":30";
                }

                TipTermina tp;
                if (combo.Text.Equals("Pregled"))
                {
                    tp = TipTermina.Pregled;
                }
                else
                {
                    tp = TipTermina.Operacija;
                }
                int idLek = int.Parse(text4.Text);
                Lekar l = new Lekar(idLek, "Filip", "Filipovic");

                Termin t = new Termin(brojTermina, formatted, vp, vk, tp/*, l, sala, p*/);
                int idPac = int.Parse(text5.Text);
                //List<Pacijent> pacijenti = PacijentiMenadzer.PronadjiSve();
                //Pacijent p = PacijentiMenadzer.PronadjiPoId(idPac);
                foreach (Pacijent pac in PacijentiMenadzer.PronadjiSve())
                {
                    if (pac.Jmbg == idPac)
                    {
                        t.Pacijent = pac;
                    }
                }

                //int idSale = int.Parse(prostorije.Text);
                // Sala sala = SaleMenadzer.NadjiSaluPoId(idSale);   //kada uradimo serijalizaciju
                foreach (Sala sala in SaleMenadzer.NadjiSveSale())
                {
                   /* if (sala.Id == idSale)
                    {
                        s.Prostorija = sala; // seter
                    }*/
                   if (sala.Status == status.Slobodna)
                    {
                        t.Prostorija = sala;
                    }
                }


                t.Lekar = l; // za sad ostaje kopija Lekara
                TerminMenadzer.ZakaziTermin(t);
                //PrikaziTermin.Termini.Add(s);
                this.Close();

            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
