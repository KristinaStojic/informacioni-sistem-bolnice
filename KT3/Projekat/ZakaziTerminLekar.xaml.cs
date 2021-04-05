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

            try
            {
                int brojTermina = TerminMenadzer.GenerisanjeIdTermina();
                String formatted = null;
                DateTime? selectedDate = dp.SelectedDate;
                Console.WriteLine(selectedDate);
                if (selectedDate.HasValue)
                {
                    formatted = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                }

                String vp = vpp.Text;
                String vk = vkk.Text;
                TipTermina tp;
                if (tipPregleda.Text.Equals("Pregled"))
                {
                    tp = TipTermina.Pregled;
                }
                else
                {
                    tp = TipTermina.Operacija;
                }
                //int idLek = int.Parse(text4.Text);
                

                Lekar l = new Lekar(5, "Filip", "Filipovic");
                List<Sala> sale = SaleMenadzer.NadjiSveSale();


                int idSale = int.Parse(prostorije.Text);


                Termin s = new Termin(brojTermina, formatted, vp, vk, tp, l);
                /*int idPac = int.Parse(IDpacijenta.Text);
                foreach (Pacijent p in PacijentiMenadzer.pacijenti)
                {
                    if (p.IdPacijenta == idPac)
                    {
                        s.Pacijent = p;
                    }
                }*/
                String p = pacijenti.Text;
                string[] podaci = p.Split(' ');
                Pacijent pacijent = PacijentiMenadzer.PronadjiPoId(Int32.Parse(podaci[2]));

                foreach (Sala sala in SaleMenadzer.sale)
                {
                    if(sala.Id == idSale)
                    {
                        s.Prostorija = sala;

                    }
                }
                                      
                /*if (sala.Status == status.Zauzeta)
                {
                    MessageBox.Show("Izabrana sala je zauzeta u tom terminu", "Promenite salu", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (pacijent == null)
                {

                    MessageBox.Show("Uneli ste nepostojećeg pacijenta!", "Proverite podatke", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (sala == null)
                {

                    MessageBox.Show("Uneli ste nepostojeću prostoriju!", "Proverite podatke", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                */

              
                TerminMenadzer.ZakaziTerminLekar(s);
                this.Close();

            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //odustani
            this.Close();
        }
    }
}
