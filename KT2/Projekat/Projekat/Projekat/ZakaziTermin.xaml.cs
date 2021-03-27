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
using static Model.Termin;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for ZakaziTermin.xaml
    /// </summary>
    public partial class ZakaziTermin : Window
    {
        public ZakaziTermin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrikaziTermin pt = new PrikaziTermin();
            pt.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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
            String vk = text3.Text;
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
            Lekar l = new Lekar(idLek, "Milan", "Milanovic");
            /*if (idLek > 0 && idLek < 50)
            {
                l.Ime = "Pera";
                l.Prezime = "Peric";
            } 
            else if (idLek >= 50 && idLek < 100)
            {
                l.Ime = "Marko";
                l.Prezime = "Markovic";
            }
            else if (idLek >= 100 && idLek <= 200)
            {
                l.Ime = "Milan";
                l.Prezime = "Milanovic";
            }*/


            int idPac = int.Parse(text5.Text);
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
    }
}
