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
    /// Interaction logic for IzmeniTermin.xaml
    /// </summary>
    public partial class IzmeniTermin : Window
    {
        public Termin termin;
        public IzmeniTermin(Termin izabraniTermin)
        {
            InitializeComponent();
            this.termin = izabraniTermin;
            if (izabraniTermin != null) 
            {
                this.text1.Text = izabraniTermin.IdTermin.ToString();
                this.text2.Text = izabraniTermin.VremePocetka;
                this.text3.Text = izabraniTermin.VremeKraja;
                this.text4.Text = izabraniTermin.Lekar.IdLekara.ToString();
                //this.text5.Text = izabraniTermin.Pacijent.IdPacijenta.ToString();
                TipTermina tp;
                if (this.combo.Equals("Operacija"))
                {
                    tp = TipTermina.Operacija;
                }
                else
                {
                    tp = TipTermina.Pregled;
                }
                tp = izabraniTermin.tipTermina;
                // 1. nacin
                // var date = Convert.ToDateTime(this.datum.Text).ToString("yyyy/MM/dd");
                // date = izabraniTermin.Datum;
                //this.datum.Text = izabraniTermin.Datum;
                String dat = null;
                
                datum.SelectedDate = DateTime.Parse(izabraniTermin.Datum);
                // 2. nacin
               /* DateTime? selectedDate = this.datum.SelectedDate;
                Console.WriteLine(selectedDate);
                if (selectedDate.HasValue)
                {
                    izabraniTermin.Datum = selectedDate.Value.ToString("dd/MM/yyyy");

                }*/
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //dugme odustani
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //dugme sacuvaj
            int brojTermina = int.Parse(text1.Text);
            String formatted = null;
            DateTime? selectedDate = datum.SelectedDate;
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
            Lekar l = new Lekar(idLek);

            //int idPac = int.Parse(text5.Text);
            //Pacijent p = new Pacijent(idPac);

            Termin t = new Termin(brojTermina, formatted, vp, vk, tp, l);
            TerminMenadzer.IzmeniTermin(termin, t); 
            this.Close();

        }
    }
}
