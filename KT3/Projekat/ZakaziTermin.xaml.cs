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
            dp.BlackoutDates.AddDatesInPast();
            CalendarDateRange cdr = new CalendarDateRange();
            cdr.Start = DateTime.Now.AddDays(3);
            cdr.End = DateTime.Now.AddDays(2000);
            dp.BlackoutDates.Add(cdr);
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
                int brojTermina = TerminMenadzer.GenerisanjeIdTermina();
                String formatted = null;
                DateTime? selectedDate = dp.SelectedDate;
                Console.WriteLine(selectedDate);
                if (selectedDate.HasValue)
                {
                    formatted = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                }
                String vp = text2.Text;
                //String vk = text3.Text;
                //TODO
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
                    }
                    else
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

                Termin s = new Termin(brojTermina, formatted, vp, vk, tp, l);
                foreach (Pacijent p in PacijentiMenadzer.pacijenti)
                {
                    if (p.IdPacijenta == 1)
                    {
                        s.Pacijent = p;
                    }
                }
                foreach (Sala sala in SaleMenadzer.NadjiSveSale())
                {
                    try
                    {
                        if (sala.Status.Equals(status.Slobodna))
                        {
                            s.Prostorija = sala;  // kad naidje na prvu slobodnu

                            // TODO: ispraviti , NIJE DOBRO!!!
                            for(int i = 0; i < PrikaziSalu.Sale.Count(); i++) 
                            { 
                                if (PrikaziSalu.Sale[i].Id == sala.Id)
                                {
                                    PrikaziSalu.Sale[i].Status = status.Zauzeta;
                                }
                            }
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ne postoji nijedna slobodna sala", "Zauzete sale");
                    }
                }
                TerminMenadzer.ZakaziTermin(s);
                this.Close();

            } catch(System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void zdravstevniKarton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
