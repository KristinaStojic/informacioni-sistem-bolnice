using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    /// Interaction logic for ZakaziTerminLekar2.xaml
    /// </summary>
    public partial class ZakaziTerminLekar2 : Window
    {
        public Pacijent Pacijent;
        public Lekar Lekar;
        public Sala Sala;
        public static ObservableCollection<string> sviSlobodniTermini { get; set; }
        public static ObservableCollection<string> sviSlobodniTerminiKraj { get; set; }
        public static ObservableCollection<string> pomocnaSviSlobodniTermini { get; set; }
        public static ObservableCollection<string> pomocnaSviSlobodniTerminiKraj { get; set; }
        public ZakaziTerminLekar2()
        {
            InitializeComponent();

            datum.BlackoutDates.AddDatesInPast();
            popuniTabelePodacima();
            dodajSveSlobodneTermine();
            popuniSale();

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

            vpp.ItemsSource = sviSlobodniTermini;
            vkk.ItemsSource = sviSlobodniTerminiKraj;
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

        private void popuniTabelePodacima()
        {
            this.listaPacijenata.ItemsSource = PacijentiMenadzer.pacijenti;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource);
            view.Filter = UserFilterPacijenti;

            this.listaLekara.ItemsSource = MainWindow.lekari;
            CollectionView viewLekari = (CollectionView)CollectionViewSource.GetDefaultView(listaLekara.ItemsSource);
            viewLekari.Filter = UserFilterLekari;
        }
        private void popuniSale()
        {
            foreach (Sala s in SaleMenadzer.sale)
            {
                prostorije.Items.Add(s.Id);
            }
        }

        private bool UserFilterPacijenti(object item)
        {
            if (String.IsNullOrEmpty(pretraga.Text))
            {
                return true;
            }
            else
            {
                return ((item as Pacijent).ImePacijenta.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Pacijent).PrezimePacijenta.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                       || ((item as Pacijent).Jmbg.ToString().IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private bool UserFilterLekari(object item)
        {
            if (String.IsNullOrEmpty(pretraga.Text))
            {
                return true;
            }
            else
            {
                return ((item as Lekar).ImeLek.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Lekar).PrezimeLek.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                       || ((item as Lekar).specijalizacija.ToString().IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void Pretraga_Pacijenata(object sender, RoutedEventArgs e)
        {
            listaPacijenata.Visibility = Visibility.Visible;
            listaLekara.Visibility = Visibility.Hidden;
        }

        private void Pretraga_Lekara(object sender, RoutedEventArgs e)
        {
            listaLekara.Visibility = Visibility.Visible;
            listaPacijenata.Visibility = Visibility.Hidden;
        }
        private void pretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(listaLekara.ItemsSource).Refresh();
        }


        private void listaLekara_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (listaLekara.SelectedItem != null)
            {
                Lekar = (Lekar)listaLekara.SelectedItem;
                lekar.Text = Lekar.ImeLek + " " + Lekar.PrezimeLek;
            }
        }

        private void listaPacijenata_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (listaPacijenata.SelectedItem != null)
            {
                Pacijent = (Pacijent)listaPacijenata.SelectedItem;
                pacijent.Text = Pacijent.ImePacijenta + " " + Pacijent.PrezimePacijenta;
            }
        }

        private void listaPacijenata_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaPacijenata.SelectedItem != null)
            {
                Pacijent = (Pacijent)listaPacijenata.SelectedItem;
                pacijent.Text = Pacijent.ImePacijenta + " " + Pacijent.PrezimePacijenta + " " + Pacijent.IdPacijenta; //izmijeni ovo ID
            }
        }

        private void listaLekara_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaLekara.SelectedItem != null)
            {
                Lekar = (Lekar)listaLekara.SelectedItem;
                lekar.Text = Lekar.ImeLek + " " + Lekar.PrezimeLek;
            }
        }

        private void vpp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void vkk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void prostorije_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void dat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void tipPregleda_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {          
            prostorije.Items.Clear();

            if (tipPregleda.SelectedIndex == 0) // pregled
            {
                this.dugmeLekari.IsEnabled = false;
                this.lekar.Text = "Petar Nebojsic";
                this.listaLekara.IsEnabled = false;
                dodajSaleZaPregled();
            }
            else if (tipPregleda.SelectedIndex == 1) // operacija
            {
                this.dugmeLekari.IsEnabled = true;
                this.lekar.Text = "";
                this.listaLekara.IsEnabled = true;
                dodajSaleZaOperacije();
            }

            prostorije.SelectedIndex = 0;
        }

        private void dodajSaleZaPregled()
        {
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.TipSale.Equals(tipSale.SalaZaPregled))
                {
                    if (!prostorije.Items.Contains(s.Id))
                    {
                        prostorije.Items.Add(s.Id);
                    }
                }
            }
        }

        private void dodajSaleZaOperacije()
        {
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.TipSale.Equals(tipSale.OperacionaSala))
                {
                    if (!prostorije.Items.Contains(s.Id))
                    {
                        prostorije.Items.Add(s.Id);
                    }
                }
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //sacuvaj
            int brojTermina = TerminMenadzer.GenerisanjeIdTermina();

            string vp = vpp.Text;
            string vk = vkk.Text;
            String dat = null;
            DateTime selectedDate = (DateTime)datum.SelectedDate;
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
            String p = this.pacijent.Text;
            string[] podaci = p.Split(' ');
            Pacijent pacijent = PacijentiMenadzer.PronadjiPoId(Int32.Parse(podaci[2]));



            Termin t = new Termin(brojTermina, dat, vp, vk, tp, noviLek, sala, pacijent);

            String[] pocetak = vp.Split(':');
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
            else if (Convert.ToInt32(pocetakSati) == Convert.ToInt32(krajSati))
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

        

       
    }
}
