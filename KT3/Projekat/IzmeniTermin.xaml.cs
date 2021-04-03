using Model;
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
                this.rbr.Text = izabraniTermin.IdTermin.ToString();
                this.vpp.Text = izabraniTermin.VremePocetka;
               // this.vkk.Text = izabraniTermin.VremeKraja;
                this.idlekara.Text = izabraniTermin.Lekar.IdLekara.ToString();
               
                //this.idPacijenta.Text = izabraniTermin.Pacijent.Jmbg.ToString();
                TipTermina tp;
                if (izabraniTermin.tipTermina.Equals(TipTermina.Operacija))
                {
                    this.combo.SelectedIndex = 0;
                }
                else if (izabraniTermin.tipTermina.Equals(TipTermina.Pregled))
                {
                    this.combo.SelectedIndex = 1;
                }


                tp = izabraniTermin.tipTermina;
                datum.SelectedDate = DateTime.Parse(izabraniTermin.Datum);
                //this.prostorije.SelectedIndex = izabraniTermin.Prostorija.Id;  
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
            try
            {
                int brojTermina = int.Parse(rbr.Text);
                String formatted = null;
                DateTime? selectedDate = datum.SelectedDate;
                Console.WriteLine(selectedDate);
                if (selectedDate.HasValue)
                {
                    formatted = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                }
                String vp = vpp.Text;
                //String vk = vkk.Text;

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
                int idLek = int.Parse(idlekara.Text);
                Lekar l = new Lekar(idLek, "Filip", "Filipovic");

                Termin t = new Termin(brojTermina, formatted, vp, vk, tp, l);
                List<Pacijent> pacijenti = PacijentiMenadzer.PronadjiSve();
                // promeniti ovo na id, kasnije
                foreach (Pacijent p in PacijentiMenadzer.PronadjiSve())
                {
                    if (p.IdPacijenta == 1)
                    {
                        t.Pacijent = p;
                    }
                }

                List<Sala> sale = SaleMenadzer.NadjiSveSale();
                
                foreach (Sala sala in SaleMenadzer.NadjiSveSale())
                {
                    try
                    {
                        if (sala.Status == status.Slobodna)
                        {
                            t.Prostorija = sala;  // kad naidje na prvu slobodnu
                            sala.Status = status.Zauzeta;
                            break;
                        }
                    } catch (Exception)
                    {
                        MessageBox.Show("Ne postoji nijedna slobodna sala", "Zauzete sale");
                    }
                } 

                TerminMenadzer.IzmeniTermin(termin, t);
                this.Close();
            } catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
