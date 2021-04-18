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
using Projekat.Model;

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
                pacijenti.Items.Add(p.ImePacijenta + " " + p.PrezimePacijenta + " " + p.IdPacijenta);
            }

            foreach (Sala s in SaleMenadzer.sale)
            {
                prostorije.Items.Add(s.Id);
            }
        }



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //potvrdi

            int brojTermina = TerminMenadzer.GenerisanjeIdTermina();

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

            Lekar l = new Lekar() { IdLekara = 1, ImeLek = "Petar", PrezimeLek = "Nebojsic", specijalizacija = Specijalizacija.Opsta_praksa };
           // Lekar l = new Lekar() { IdLekara = 2, ImeLek = "Milos", PrezimeLek = "Dragojevic", specijalizacija = Specijalizacija.Opsta_praksa };
            //Lekar l = new Lekar() { IdLekara = 3, ImeLek = "Petar", PrezimeLek = "Milosevic", specijalizacija = Specijalizacija.Specijalista };
            //Lekar l = new Lekar() { IdLekara = 4, ImeLek = "Dejan", PrezimeLek = "Milosevic", specijalizacija = Specijalizacija.Specijalista };
            //Lekar l = new Lekar() { IdLekara = 5, ImeLek = "Isidora", PrezimeLek = "Isidorovic", specijalizacija = Specijalizacija.Specijalista };
            

            Sala sala = SaleMenadzer.NadjiSaluPoId((int)prostorije.SelectedItem);
            String p = pacijenti.Text;
            string[] podaci = p.Split(' ');
            Pacijent pacijent = PacijentiMenadzer.PronadjiPoId(Int32.Parse(podaci[2]));
            Termin t = new Termin(brojTermina, dat, vp, vk, tp, l, sala, pacijent);

            String [] pocetak = vp.Split(':');
            String pocetakSati = pocetak[0];
            String pocetakMinuti = pocetak[1];

            String[] kraj = vk.Split(':');
            String krajSati = kraj[0];
            String krajMinuti = kraj[1];

            int pogresnoVreme = 0;

            if (Convert.ToInt32(pocetakSati) > Convert.ToInt32(krajSati))
            {
                MessageBox.Show("Neispravno vreme pocetka i kraja");
                pogresnoVreme = 1;
            }
            else if(Convert.ToInt32(pocetakSati) == Convert.ToInt32(krajSati))
            {
                if (Convert.ToInt32(pocetakMinuti) >= Convert.ToInt32(krajMinuti))
                {
                    MessageBox.Show("Neispravno vreme pocetka i kraja");
                    pogresnoVreme = 1;
                }
            }
            //provjera da li je termin zauzet
            //bool zauzeto = false;
            int zauzeto = 0;

            if (pogresnoVreme == 0)
            {  

                /*if (sala.zauzetiTermini.Count != 0)  //ako ne postoje zauzeti termini
                {
                    foreach (ZauzeceSale z in sala.zauzetiTermini.ToList())
                    {
                        if (dat.Equals(z.datumTermina) && vp.Equals(z.pocetakTermina) && vk.Equals(z.krajTermina))// t.Prostorija.Id.Equals(sala.Id)
                {
                    MessageBox.Show("Postoji termin!");
                            //this.Close();
                            //zauzeto = true;
                            zauzeto = 1;
                            // break;
                        }

                        if (zauzeto == 0)
                        {
                            TerminMenadzer.ZakaziTerminLekar(t);
                            ZauzeceSale za = new ZauzeceSale(vp, vk, dat, t.IdTermin);
                            sala.zauzetiTermini.Add(za);
                            this.Close();
                        }
                    }
                }
                else  //ako postoje zauzeti temrini
                {*/
                    TerminMenadzer.ZakaziTerminLekar(t);
                    ZauzeceSale z = new ZauzeceSale(vp, vk, dat, t.IdTermin);
                    sala.zauzetiTermini.Add(z);
                    this.Close();
               // }
            }
           
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //odustani
            this.Close();
        }
    }
}
