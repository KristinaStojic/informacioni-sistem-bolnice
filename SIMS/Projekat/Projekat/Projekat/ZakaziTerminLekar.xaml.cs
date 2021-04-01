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

                int idPac = int.Parse(IDpacijenta.Text);
                List<Pacijent> pacijenti = PacijentiMenadzer.PronadjiSve();
                List<Sala> sale = SaleMenadzer.NadjiSveSale();
                Pacijent p = PacijentiMenadzer.PronadjiPoId(idPac);
                int idSale = int.Parse(prostorije.Text);
                Sala sala = SaleMenadzer.NadjiSaluPoId(idSale);   //kada uradimo serijalizaciju
                                                                  //Sala sala = new Sala(idSale);

                Termin s = new Termin(brojTermina, formatted, vp, vk, tp, l, sala, p);
                TerminMenadzer.ZakaziTermin(s);
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
