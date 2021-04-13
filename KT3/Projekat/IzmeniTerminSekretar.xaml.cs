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
    /// Interaction logic for IzmeniTerminSekretar.xaml
    /// </summary>
    public partial class IzmeniTerminSekretar : Window
    {
        public Termin termin;

        public IzmeniTerminSekretar(Termin izabraniTermin)
        {
            InitializeComponent();
            this.termin = izabraniTermin;

            foreach (Sala s in SaleMenadzer.sale)
            {
                prostorije.Items.Add(s.Id);
            }

            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                pacijenti.Items.Add(p.ImePacijenta + " " + p.PrezimePacijenta + " " + p.Jmbg);
            }

            foreach (Lekar l in MainWindow.lekari)
            {
                lekari.Items.Add(l.ImeLek + " " + l.PrezimeLek + " " + l.IdLekara);
            }

            if (izabraniTermin != null)
            {
                termin.IdTermin = izabraniTermin.IdTermin;
             
                // vreme
                vremePocetka.Text = izabraniTermin.VremePocetka;
                vremeKraja.Text = izabraniTermin.VremeKraja;

                // lekar
                lekari.SelectedItem = izabraniTermin.Lekar.ImeLek + " " + izabraniTermin.Lekar.PrezimeLek + " " + izabraniTermin.Lekar.IdLekara;

                // pacijent
                pacijenti.Text = izabraniTermin.Pacijent.ImePacijenta + " " + izabraniTermin.Pacijent.PrezimePacijenta + " " + izabraniTermin.Pacijent.Jmbg;
                
                // prostorija
                prostorije.SelectedItem = izabraniTermin.Prostorija.Id.ToString();

                // tip termina
                TipTermina tp;
                if (izabraniTermin.tipTermina.Equals(TipTermina.Operacija))
                {
                    tip.SelectedIndex = 1;
                }
                else if (izabraniTermin.tipTermina.Equals(TipTermina.Pregled))
                {
                    tip.SelectedIndex = 0;
                }

                tp = izabraniTermin.tipTermina;

                // datum                                                        ------------------------------------------------------------------> NE RADI
                //datum.SelectedDate = DateTime.Parse(izabraniTermin.Datum);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // vreme pocetka i kraja
            string vp = vremePocetka.Text;
            string vk = vremeKraja.Text;

            string[] pocetak = vp.Split(':');
            string pocetakSati = pocetak[0];
            string pocetakMinuti = pocetak[1];

            string[] kraj = vk.Split(':');
            string krajSati = kraj[0];
            string krajMinuti = kraj[1];

            int pogresnoVreme = 0;

            if (Convert.ToInt32(pocetakSati) > Convert.ToInt32(krajSati))
            {
                MessageBox.Show("Neispravno vreme pocetka i kraja");
                pogresnoVreme = 1;
            }
            else if (Convert.ToInt32(pocetakSati) == Convert.ToInt32(krajSati))
            {
                if (Convert.ToInt32(pocetakMinuti) >= Convert.ToInt32(krajMinuti))
                {
                    MessageBox.Show("Neispravno vreme pocetka i kraja");
                    pogresnoVreme = 1;
                }
            }

            // datum
            string dat = null;
            DateTime? selectedDate = datum.SelectedDate;
            Console.WriteLine(selectedDate);
            if (selectedDate.HasValue)
            {
                dat = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }

            // tip termina
            TipTermina tp;
            if (tip.Text.Equals("Pregled"))
            {
                tp = TipTermina.Pregled;
            }
            else
            {
                tp = TipTermina.Operacija;
            }

            // lekar
            string lek = lekari.Text;
            string[] podaciLekar = lek.Split(' ');
            Lekar l = MainWindow.PronadjiPoId(Int32.Parse(podaciLekar[2]));

            // pacijent
            string p = pacijenti.Text;
            string[] podaciPacijent = p.Split(' ');
            Pacijent pacijent = PacijentiMenadzer.PronadjiPoId(Int32.Parse(podaciPacijent[2]));

            // prostorija
            Sala s = SaleMenadzer.NadjiSaluPoId((int)prostorije.SelectedItem);
            Termin t = new Termin(termin.IdTermin, dat, vp, vk, tp, l, s, pacijent);

            int zauzeto = 0;
            if (pogresnoVreme == 0)
            {
                if (s.zauzetiTermini.Count != 0)        // ako postoje zauzeti termini
                {
                    foreach (ZauzeceSale zauzece in s.zauzetiTermini)
                    {
                        string[] zauzetiTerminPocetak = zauzece.pocetakTermina.Split(':');
                        string zauzetiTerminPocetakSati = zauzetiTerminPocetak[0];
                        string zauzetiTerminPocetakMinuti = zauzetiTerminPocetak[1];

                        string[] zauzetiTerminKraj = zauzece.krajTermina.Split(':');
                        string zauzetiTerminKrajSati = zauzetiTerminKraj[0];
                        string zauzetiTerminKrajMinuti = zauzetiTerminKraj[1];

                        if (t.Prostorija.Id.Equals(s.Id) && dat.Equals(zauzece.datumTermina) && Convert.ToInt32(pocetakSati) >= Convert.ToInt32(zauzetiTerminPocetakSati) && Convert.ToInt32(pocetakSati) <= Convert.ToInt32(zauzetiTerminKrajSati))
                        {
                            MessageBox.Show("Vec postoji termin");
                            vremePocetka.Text = "";
                            vremeKraja.Text = "";
                            zauzeto = 1;
                        }
                        // obrisi taj stari termin stavi novi izmenjeni
                        //    TerminMenadzer.ZakaziTerminSekretar(t);
                        //    ZauzeceSale z = new ZauzeceSale(vp, vk, dat, t.IdTermin);
                        //    s.zauzetiTermini.Add(z);
                        //    this.Close();
                        //}
                    }

                    if (zauzeto == 0)
                    {    
                        if (termin.IdTermin == t.IdTermin)
                        {
                            foreach (Sala sala in SaleMenadzer.sale)
                            {
                                if (sala.Id == termin.Prostorija.Id)
                                {
                                    // izbaci iz zauzetih termina to zauzece sale koje menjamo
                                    sala.zauzetiTermini.Remove(SaleMenadzer.NadjiZauzece(sala.Id, termin.IdTermin, termin.Datum, termin.VremePocetka, termin.VremeKraja));
                                }
                            }
                        }

                        // za termine u istoj prostoriji gde smo izmenili termin, treba izbaciti to zauzece sale iz prostorije tog termina -------------------!!!!!!!!!!!!!!!!!!!!!!!!!!!!   NE RADI
                        // izmena prostorije - obrisati zauzece iz stare prostorije - isti problem !!!!!!!!!!!!!!!!!!!!!!!

                        // u svim terminima izbaci to zauzece iz zauzetih termina prostorija ????  preko t1.prostorija.zauzetiTermini.Remove(...);
                        foreach (Termin t1 in TerminMenadzer.termini)
                        {
                           // if (termin.Prostorija.Id == t1.Prostorija.Id)
                           // {
                                foreach (Sala s1 in SaleMenadzer.sale)
                                {
                                    if (s1.Id == termin.Prostorija.Id)
                                    {
                                        t1.Prostorija.zauzetiTermini.Remove(SaleMenadzer.NadjiZauzece(s1.Id, termin.IdTermin, termin.Datum, termin.VremePocetka, termin.VremeKraja));
                                        Console.WriteLine("USLOOOOOOOOOO JEEEE ");
                                    }
                                }
                          //  }
                        }

                        TerminMenadzer.IzmeniTerminSekretar(termin, t);
                        ZauzeceSale z = new ZauzeceSale(vp, vk, dat, t.IdTermin);  // ubaci novo izmenjeno zauzece sale 
                        s.zauzetiTermini.Add(z);
                        this.Close();
                    }
                }
                else    // ako ne postoje zauzeti termini
                {
                    TerminMenadzer.IzmeniTerminSekretar(termin, t);
                    ZauzeceSale z = new ZauzeceSale(vp, vk, dat, t.IdTermin);
                    s.zauzetiTermini.Add(z);
                    this.Close();
                }
            }
        }

        // odustani
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {            
            this.Close();
        }

        // pravljenje guest naloga
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DodajPacijentaGuestIzmeni dodavanje = new DodajPacijentaGuestIzmeni(this);
            dodavanje.Show();
        }

        private void Button_LostFocus(object sender, RoutedEventArgs e)
        {
            pacijenti.Items.Clear();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                pacijenti.Items.Add(p.ImePacijenta + " " + p.PrezimePacijenta + " " + p.Jmbg);
            }
            int ukupno = PacijentiMenadzer.pacijenti.Count;
            pacijenti.SelectedIndex = ukupno - 1;
        }

        public void AzurirajComboBox()
        {
            pacijenti.Items.Clear();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                pacijenti.Items.Add(p.ImePacijenta + " " + p.PrezimePacijenta + " " + p.Jmbg);
            }
            int ukupno = PacijentiMenadzer.pacijenti.Count;
            pacijenti.SelectedIndex = ukupno - 1;
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void search_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

        }
    }
}
