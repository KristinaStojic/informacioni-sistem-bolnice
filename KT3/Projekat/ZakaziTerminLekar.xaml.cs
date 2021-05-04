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
    /// Interaction logic for ZakaziTerminLekar.xaml
    /// </summary>
    public partial class ZakaziTerminLekar : Window
    {
        public static ObservableCollection<string> sviSlobodniTermini { get; set; }
        public static ObservableCollection<string> sviSlobodniTerminiKraj { get; set; }
        public static ObservableCollection<string> pomocnaSviSlobodniTermini { get; set; }
        public static ObservableCollection<string> pomocnaSviSlobodniTerminiKraj { get; set; }
        public ZakaziTerminLekar()
        {
            this.DataContext = this;
            InitializeComponent();
            popuniPacijente();
            popuniSale();
            popuniLekare();
            dodajSveSlobodneTermine();
         }
        private void dodajSveSlobodneTermine()
        {
            popuniListeZaPocetakiKraj();
            popuniPomocneListeZaPocetakiKraj();
        }

        private void popuniListeZaPocetakiKraj()
        {
            sviSlobodniTermini = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00"};
            sviSlobodniTerminiKraj = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00", "20:30"};
        }

        private void popuniPomocneListeZaPocetakiKraj()
        {
            pomocnaSviSlobodniTermini = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00"};
            pomocnaSviSlobodniTerminiKraj = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00"};
        }

        private void popuniPacijente()
        {
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                pacijenti.Items.Add(p.ImePacijenta + " " + p.PrezimePacijenta + " " + p.IdPacijenta);
            }
        }

        private void popuniSale()
        {
            foreach (Sala s in SaleMenadzer.sale)
            {
                prostorije.Items.Add(s.Id);
            }
        }

        private void popuniLekare()
        {
            foreach (Lekar lekar in MainWindow.lekari)
            {
                this.lekar.Items.Add(lekar.ImeLek + " " + lekar.PrezimeLek + " " + lekar.specijalizacija);
            }
            this.lekar.IsEnabled = false;
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
            if (tipPregleda.Text.Equals("Pregled"))
            {
                tp = TipTermina.Pregled;
            }
            else
            {
                tp = TipTermina.Operacija;
            }
            //Lekar lekar = new Lekar() { IdLekara = 1, ImeLek = "Petar", PrezimeLek = "Nebojsic", specijalizacija = Specijalizacija.Opsta_praksa };
            //Lekar lekar = new Lekar() { IdLekara = 2, ImeLek = "Milos", PrezimeLek = "Dragojevic", specijalizacija = Specijalizacija.Opsta_praksa };
            //Lekar lekar = new Lekar() { IdLekara = 3, ImeLek = "Petar", PrezimeLek = "Milosevic", specijalizacija = Specijalizacija.Specijalista };
            //Lekar lekar = new Lekar() { IdLekara = 4, ImeLek = "Dejan", PrezimeLek = "Milosevic", specijalizacija = Specijalizacija.Specijalista };
            //Lekar lekar = new Lekar() { IdLekara = 5, ImeLek = "Isidora", PrezimeLek = "Isidorovic", specijalizacija = Specijalizacija.Specijalista };
            Lekar noviLek = new Lekar();
            String lekPod = this.lekar.Text;
            string[] podaciLek = lekPod.Split(' ');
            foreach (Lekar lekar in MainWindow.lekari)
            {
                if (lekar.ImeLek.Equals(podaciLek[0]) && lekar.PrezimeLek.Equals(podaciLek[1]))
                {
                    noviLek = lekar;
                }
            }




            Sala sala = SaleMenadzer.NadjiSaluPoId((int)prostorije.SelectedItem);
            String p = pacijenti.Text;
            string[] podaci = p.Split(' ');
            Pacijent pacijent = PacijentiMenadzer.PronadjiPoId(Int32.Parse(podaci[2]));
            


            Termin t = new Termin(brojTermina, dat, vp, vk, tp, noviLek, sala, pacijent);

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

                if (sala.zauzetiTermini.Count != 0)        // ako postoje zauzeti termini
                {
                    foreach (ZauzeceSale zauzece in sala.zauzetiTermini)
                    {
                        string[] zauzetiTerminPocetak = zauzece.pocetakTermina.Split(':');
                        string zauzetiTerminPocetakSati = zauzetiTerminPocetak[0];
                        string zauzetiTerminPocetakMinuti = zauzetiTerminPocetak[1];

                        string[] zauzetiTerminKraj = zauzece.krajTermina.Split(':');
                        string zauzetiTerminKrajSati = zauzetiTerminKraj[0];
                        string zauzetiTerminKrajMinuti = zauzetiTerminKraj[1];

                        if ((t.Prostorija.Id.Equals(sala.Id) && dat.Equals(zauzece.datumPocetkaTermina) && Convert.ToInt32(pocetakSati) >= Convert.ToInt32(zauzetiTerminPocetakSati) && (Convert.ToInt32(pocetakSati) < Convert.ToInt32(zauzetiTerminKrajSati) || Convert.ToInt32(pocetakMinuti) < Convert.ToInt32(zauzetiTerminKrajMinuti))) ||
                            (t.Prostorija.Id.Equals(sala.Id) && dat.Equals(zauzece.datumPocetkaTermina) && (Convert.ToInt32(krajSati) > Convert.ToInt32(zauzetiTerminPocetakSati) || Convert.ToInt32(krajMinuti) > Convert.ToInt32(zauzetiTerminPocetakMinuti)) && Convert.ToInt32(krajSati) <= Convert.ToInt32(zauzetiTerminKrajSati) && Convert.ToInt32(pocetakSati) <= Convert.ToInt32(zauzetiTerminPocetakSati)) ||
                            (t.Prostorija.Id.Equals(sala.Id) && dat.Equals(zauzece.datumPocetkaTermina) && Convert.ToInt32(pocetakSati) <= Convert.ToInt32(zauzetiTerminPocetakSati) && Convert.ToInt32(krajSati) >= Convert.ToInt32(zauzetiTerminKrajSati)) && !Convert.ToInt32(krajSati).Equals(Convert.ToInt32(zauzetiTerminPocetakSati)) && !Convert.ToInt32(pocetakSati).Equals(Convert.ToInt32(zauzetiTerminKrajSati)))
                        {
                            MessageBox.Show("Vec postoji termin");
                            vpp.Text = "";
                            vkk.Text = "";
                            zauzeto = 1;
                            break;
                        }
                    }

                    if (zauzeto == 0)
                    {
                        TerminMenadzer.ZakaziTerminLekar(t);
                        ZauzeceSale z = new ZauzeceSale(vp, vk, dat, t.IdTermin);
                        sala.zauzetiTermini.Add(z);

                        // za svaki termin koji je zakazan u istoj prostoriji s, dodati to novo zauzece u zauzeca te prostorije
                        foreach (Termin t1 in TerminMenadzer.termini)
                        {
                            if (t1.Prostorija.Id == sala.Id)
                            {
                                t1.Prostorija = sala;
                            }
                        }

                        TerminMenadzer.sacuvajIzmene();
                        SaleMenadzer.sacuvajIzmjene();

                        this.Close();
                    }



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
                   /* else  //ako postoje zauzeti temrini
                    {
                    TerminMenadzer.ZakaziTerminLekar(t);
                        ZauzeceSale z = new ZauzeceSale(vp, vk, dat, t.IdTermin);
                        sala.zauzetiTermini.Add(z);
                        this.Close();
                    }*/
                }
                else    // ako ne postoje zauzeti termini
                {
                    TerminMenadzer.ZakaziTerminLekar(t);
                    ZauzeceSale z = new ZauzeceSale(vp, vk, dat, t.IdTermin);
                    sala.zauzetiTermini.Add(z);

                    TerminMenadzer.sacuvajIzmene();
                    SaleMenadzer.sacuvajIzmjene();

                    this.Close();
                }
            }
           
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //odustani
            this.Close();
        }

        private void tipPregleda_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tipPregleda.Text.Equals("Pregled"))
            {
                this.lekar.IsEnabled = true;
            }
            else if (tipPregleda.Text.Equals("Operacija"))
            {
                this.lekar.IsEnabled = false;
            }
           

            
        }

        private void dp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //datum
            IzbaciTermineKojiSuProsliZaDanasnjiDan();
            vpp.ItemsSource = sviSlobodniTermini;

            string prviSlobodan = sviSlobodniTermini[0];
            
            foreach(string vreme in pomocnaSviSlobodniTerminiKraj)
             {
                DateTime izabraniPocetak = DateTime.Parse(prviSlobodan);
                DateTime izaberiKraj = DateTime.Parse(vreme);
                
                if (izaberiKraj.TimeOfDay <= izabraniPocetak.TimeOfDay)
                {
                    sviSlobodniTerminiKraj.Remove(vreme);
                }
            }

           

            vkk.ItemsSource = sviSlobodniTerminiKraj;
        }

        private void IzbaciTermineKojiSuProsliZaDanasnjiDan()
        {
            if (dp.SelectedDate == DateTime.Now.Date)
            {
                foreach (string slot in pomocnaSviSlobodniTermini)
                {
                    DateTime vreme = DateTime.Parse(slot);
                    DateTime sada = DateTime.Now;
                    if (vreme.TimeOfDay <= sada.TimeOfDay)
                    {
                        sviSlobodniTermini.Remove(slot);
                    }

                }
            }
            else
            {
                popuniListeZaPocetakiKraj();
            }
        }

        private void vpp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //vreme pocetka
        }

        private void vkk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //vreme kraja
        }

        private void prostorije_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //prostorija
        }

        private void lekar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //lekar
        }
    }
}
