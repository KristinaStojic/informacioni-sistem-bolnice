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
    /// Interaction logic for IzmeniTerminLekara.xaml
    /// </summary>
    public partial class IzmeniTerminLekara : Window
    {
        public IzmeniTerminLekara()
        {
            InitializeComponent();
        }
        public Termin termin;
        public IzmeniTerminLekara(Termin izabraniTermin)
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

            this.termin = izabraniTermin;
            if (izabraniTermin != null)
            {

                this.vpp.Text = izabraniTermin.VremePocetka;
                this.vkk.Text = izabraniTermin.VremeKraja;


                foreach(Pacijent pacij in PacijentiMenadzer.pacijenti)
                {
                    if(pacij.ImePacijenta.Equals(izabraniTermin.Pacijent.ImePacijenta) && pacij.PrezimePacijenta.Equals(izabraniTermin.Pacijent.PrezimePacijenta) && pacij.Jmbg == izabraniTermin.Pacijent.Jmbg && pacij.IdPacijenta == izabraniTermin.Pacijent.IdPacijenta)
                    {
     
                        if(PacijentiMenadzer.pacijenti.IndexOf(pacij) == PacijentiMenadzer.pacijenti.Count - 1)
                        {
                            this.pacijenti.SelectedIndex = izabraniTermin.Pacijent.IdPacijenta - 1;
                        }
                        else
                        {
                            this.pacijenti.SelectedIndex = izabraniTermin.Pacijent.IdPacijenta;
                        }
                       
                        Console.WriteLine("T: " + izabraniTermin.Pacijent.ImePacijenta + " " + izabraniTermin.Pacijent.PrezimePacijenta + " " + izabraniTermin.Pacijent.Jmbg + " " +izabraniTermin.Pacijent.IdPacijenta);
                        Console.WriteLine("P: " + pacij.ImePacijenta + " " + pacij.PrezimePacijenta + " " + pacij.Jmbg + " " +pacij.IdPacijenta);
                        break;
                    }
                }



               /* foreach (Pacijent pacij in PacijentiMenadzer.pacijenti)
                {
                    //if (pacij.ImePacijenta.Equals(izabraniTermin.Pacijent.ImePacijenta) && pacij.PrezimePacijenta.Equals(izabraniTermin.Pacijent.PrezimePacijenta) && pacij.Jmbg == izabraniTermin.Pacijent.Jmbg && pacij.IdPacijenta == izabraniTermin.Pacijent.IdPacijenta)
                    //{
                        for(int i = 0; i < pacijenti.Items.Count; i++)
                        {
                        if (pacijenti.Items.Equals(pacij.ImePacijenta + " " + pacij.PrezimePacijenta + " " + pacij.Jmbg))
                        {
                            this.pacijenti.SelectedIndex = izabraniTermin.Pacijent.IdPacijenta;
                        }
                    }
                        
                        Console.WriteLine("T: " + izabraniTermin.Pacijent.ImePacijenta + " " + izabraniTermin.Pacijent.PrezimePacijenta + " " + izabraniTermin.Pacijent.Jmbg + " " + izabraniTermin.Pacijent.IdPacijenta);
                        Console.WriteLine("P: " + pacij.ImePacijenta + " " + pacij.PrezimePacijenta + " " + pacij.Jmbg + " " + pacij.IdPacijenta);
                        break;
                   // }
                }
               */


                // this.pacijenti.SelectedItem = izabraniTermin.Pacijent;
                TipTermina tp;
                if (izabraniTermin.tipTermina.Equals(TipTermina.Operacija))
                {
                    this.tipPregleda.SelectedIndex = 1;
                }
                else if (izabraniTermin.tipTermina.Equals(TipTermina.Pregled))
                {
                    this.tipPregleda.SelectedIndex = 0;
                }


                tp = izabraniTermin.tipTermina;
                dp.SelectedDate = DateTime.Parse(izabraniTermin.Datum);
                this.prostorije.SelectedIndex = izabraniTermin.Prostorija.Id;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //potvrdi
            string vp = vpp.Text;
            string vk = vkk.Text;
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
            Lekar l = new Lekar(5, "Filip", "Filipovic");
            Sala s = SaleMenadzer.NadjiSaluPoId((int)prostorije.SelectedItem);
            String p = pacijenti.Text;
            string[] podaci = p.Split(' ');
            Pacijent pacijent = PacijentiMenadzer.PronadjiPoId(Int32.Parse(podaci[2]));

            
             Termin t = new Termin(termin.IdTermin, dat, vp, vk, tp, l, s, pacijent);
             TerminMenadzer.IzmeniTerminLekar(termin,t);
            
            this.Close();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //odustani
            this.Close();
        }
    }
}

