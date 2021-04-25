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

            //pacijent
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                pacijenti.Items.Add(p.ImePacijenta + " " + p.PrezimePacijenta + " " + p.IdPacijenta);
            }

            //sala
            foreach (Sala s in SaleMenadzer.sale)
            {
                prostorije.Items.Add(s.Id);
            }

            this.termin = izabraniTermin;
            if (izabraniTermin != null)
            {
                //vreme
                this.vpp.Text = izabraniTermin.VremePocetka;
                this.vkk.Text = izabraniTermin.VremeKraja;


                //pacijent
                pacijenti.Text = izabraniTermin.Pacijent.ImePacijenta + " " + izabraniTermin.Pacijent.PrezimePacijenta + " " + izabraniTermin.Pacijent.IdPacijenta;

                //tip termina
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

                //datum
                dp.SelectedDate = DateTime.Parse(izabraniTermin.Datum);

                //prostorija
                this.prostorije.SelectedIndex = izabraniTermin.Prostorija.Id;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //potvrdi

            //vreme
            string vp = vpp.Text;
            string vk = vkk.Text;

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


            //datum
            String dat = null;
            DateTime selectedDate = (DateTime)dp.SelectedDate;
            dat = selectedDate.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);


            //tip termina
            TipTermina tp;
            if (tipPregleda.Text.Equals("Pregled"))
            {
                tp = TipTermina.Pregled;
            }
            else
            {
                tp = TipTermina.Operacija;
            }

            //lekar
            Lekar l = new Lekar() { IdLekara = 1, ImeLek = "Petar", PrezimeLek = "Nebojsic", specijalizacija = Specijalizacija.Opsta_praksa };
            // Lekar l = new Lekar() { IdLekara = 2, ImeLek = "Milos", PrezimeLek = "Dragojevic", specijalizacija = Specijalizacija.Opsta_praksa };
            //Lekar l = new Lekar() { IdLekara = 3, ImeLek = "Petar", PrezimeLek = "Milosevic", specijalizacija = Specijalizacija.Specijalista };
            //Lekar l = new Lekar() { IdLekara = 4, ImeLek = "Dejan", PrezimeLek = "Milosevic", specijalizacija = Specijalizacija.Specijalista };
            //Lekar l = new Lekar() { IdLekara = 5, ImeLek = "Isidora", PrezimeLek = "Isidorovic", specijalizacija = Specijalizacija.Specijalista };

            //prostorija
            Sala s = SaleMenadzer.NadjiSaluPoId((int)prostorije.SelectedItem);

            //pacijent
            String p = pacijenti.Text;
            string[] podaci = p.Split(' ');
            Pacijent pacijent = PacijentiMenadzer.PronadjiPoId(Int32.Parse(podaci[2]));

            //izmjenjeni termin
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

                        if (termin.IdTermin != t.IdTermin)
                        {

                            if ((t.Prostorija.Id.Equals(s.Id) && dat.Equals(zauzece.datumTermina) && Convert.ToInt32(pocetakSati) >= Convert.ToInt32(zauzetiTerminPocetakSati) && (Convert.ToInt32(pocetakSati) < Convert.ToInt32(zauzetiTerminKrajSati) || Convert.ToInt32(pocetakMinuti) < Convert.ToInt32(zauzetiTerminKrajMinuti))) ||
                            (t.Prostorija.Id.Equals(s.Id) && dat.Equals(zauzece.datumTermina) && (Convert.ToInt32(krajSati) > Convert.ToInt32(zauzetiTerminPocetakSati) || Convert.ToInt32(krajMinuti) > Convert.ToInt32(zauzetiTerminPocetakMinuti)) && Convert.ToInt32(krajSati) <= Convert.ToInt32(zauzetiTerminKrajSati) && Convert.ToInt32(pocetakSati) <= Convert.ToInt32(zauzetiTerminPocetakSati)) ||
                            (t.Prostorija.Id.Equals(s.Id) && dat.Equals(zauzece.datumTermina) && Convert.ToInt32(pocetakSati) <= Convert.ToInt32(zauzetiTerminPocetakSati) && Convert.ToInt32(krajSati) >= Convert.ToInt32(zauzetiTerminKrajSati)) && !Convert.ToInt32(krajSati).Equals(Convert.ToInt32(zauzetiTerminPocetakSati)) && !Convert.ToInt32(pocetakSati).Equals(Convert.ToInt32(zauzetiTerminKrajSati)))
                            {
                                MessageBox.Show("Vec postoji termin");
                                vpp.Text = "";
                                vkk.Text = "";
                                zauzeto = 1;
                                break;
                            }
                        }
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

                        TerminMenadzer.IzmeniTerminLekar(termin, t);
                        ZauzeceSale z = new ZauzeceSale(vp, vk, dat, t.IdTermin);  // ubaci novo izmenjeno zauzece sale 
                        s.zauzetiTermini.Add(z);

                        foreach (Termin t1 in TerminMenadzer.termini)
                        {
                            if (t1.Prostorija.Id == s.Id)
                            {
                                t1.Prostorija = s;
                            }
                        }

                        TerminMenadzer.sacuvajIzmene();
                        SaleMenadzer.sacuvajIzmjene();
                        this.Close();

                    }
                }
                else    // ako ne postoje zauzeti termini
                {
                    TerminMenadzer.IzmeniTerminLekar(termin, t);
                    ZauzeceSale z = new ZauzeceSale(vp, vk, dat, t.IdTermin);
                    s.zauzetiTermini.Add(z);
                    TerminMenadzer.sacuvajIzmene
();
                    SaleMenadzer.sacuvajIzmjene(
);
                    this.Close();
                }
            }
            
            this.Close();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //odustani
            this.Close();
        }
    }
}

