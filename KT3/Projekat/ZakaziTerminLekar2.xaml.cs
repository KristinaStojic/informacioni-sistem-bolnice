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
        public Termin t;
        public string dat;
        public string PocetnoVreme;
        int izbaceniSlotoviMinuti;

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
            sviSlobodniTerminiKraj = new ObservableCollection<string>() { "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00", "20:30"};

            //vpp.ItemsSource = sviSlobodniTermini;
            //vkk.ItemsSource = sviSlobodniTerminiKraj;
        }

        private void popuniPomocneListeZaPocetakiKraj()
        {
            pomocnaSviSlobodniTermini = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00"};
            pomocnaSviSlobodniTerminiKraj = new ObservableCollection<string>() { "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00", "20:30"};
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
            PocetnoVreme = vpp.Text;
            //vkk.ItemsSource = sviSlobodniTerminiKraj;


        }

        private void vkk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void prostorije_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void dat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IzbaciDanasnjeProsleTermine();
            DateTime? selectedDate = datum.SelectedDate;
            if (selectedDate.HasValue)
            {
                dat = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }

            vpp.ItemsSource = sviSlobodniTermini;
            vkk.ItemsSource = sviSlobodniTerminiKraj;

            IzbaciZauzeteTermine();

        }

        private void IzbaciZauzeteTermine()
        {
            string pocSati;
            string pocMinuti;
            string krajSati;
            string krajMinuti;
            int trajanjeSati;
            int trajanjeMinuti;
            int izbaceniSlotovi;

            if (prostorije.SelectedItem != null)
            {
                Sala = SaleMenadzer.NadjiSaluPoId((int)prostorije.SelectedItem);
            }

            if (Sala != null)
            {
                foreach (ZauzeceSale z in Sala.zauzetiTermini)
                {
                    if (z.idTermina == 0) // ako sala ima zakazano renoviranje
                    {
                        DateTime datumPocetka = DateTime.Parse(z.datumPocetkaTermina);
                        DateTime datumKraja = DateTime.Parse(z.datumKrajaTermina);
                        DateTime datumZakazivanja = DateTime.Parse(dat);

                        if (z.datumPocetkaTermina.Equals(dat) && z.datumKrajaTermina.Equals(dat)) // isti dan i pocinje i zavrsava se 
                        {
                            pocSati = z.pocetakTermina.Split(':')[0];
                            pocMinuti = z.pocetakTermina.Split(':')[1];

                            krajSati = z.krajTermina.Split(':')[0];
                            krajMinuti = z.krajTermina.Split(':')[1];

                            trajanjeSati = Convert.ToInt32(krajSati) - Convert.ToInt32(pocSati);  // 0 1 2 ... koliko slotova izbacujemo
                            trajanjeMinuti = Convert.ToInt32(krajMinuti) - Convert.ToInt32(pocMinuti);  // 0 30 -30

                            if (trajanjeMinuti == 0)
                            {
                                izbaceniSlotoviMinuti = 0;
                            }
                            else if (trajanjeMinuti == 30)
                            {
                                izbaceniSlotoviMinuti = 1;
                            }
                            else if (trajanjeMinuti == -30)
                            {
                                izbaceniSlotoviMinuti = -1;
                            }

                            izbaceniSlotovi = trajanjeSati * 2 + izbaceniSlotoviMinuti;

                            int index = sviSlobodniTermini.IndexOf(z.pocetakTermina);

                            if (index != -1)
                            {
                                for (int i = 0; i < izbaceniSlotovi; i++)
                                {
                                    sviSlobodniTermini.RemoveAt(index);
                                    sviSlobodniTermini.RemoveAt(index);
                                }
                            }
                        }
                        else if (z.datumPocetkaTermina.Equals(dat) && !z.datumKrajaTermina.Equals(dat)) 
                        {
                            pocSati = z.pocetakTermina.Split(':')[0];
                            pocMinuti = z.pocetakTermina.Split(':')[1];

                            trajanjeSati = Convert.ToInt32("20") - Convert.ToInt32(pocSati);  // 0 1 2 ... koliko slotova izbacujemo
                            trajanjeMinuti = Convert.ToInt32("00") - Convert.ToInt32(pocMinuti);  // 0 30 -30

                            if (trajanjeMinuti == 0)
                            {
                                izbaceniSlotoviMinuti = 0;
                            }
                            else if (trajanjeMinuti == 30)
                            {
                                izbaceniSlotoviMinuti = 1;
                            }
                            else if (trajanjeMinuti == -30)
                            {
                                izbaceniSlotoviMinuti = -1;
                            }

                            izbaceniSlotovi = trajanjeSati * 2 + izbaceniSlotoviMinuti;

                            int index = sviSlobodniTermini.IndexOf(z.pocetakTermina);
                            if (index != -1)
                            {
                                for (int i = 0; i <= izbaceniSlotovi; i++)
                                {
                                    sviSlobodniTermini.RemoveAt(index);

                                    if (i != 0) // ne brisemo vreme pocetka renovacije iz ove liste jer termin moze da traje do vremena pocetka renovacije
                                    {
                                        if (sviSlobodniTerminiKraj.Count > 0)
                                        {
                                            sviSlobodniTerminiKraj.RemoveAt(index + 1);
                                        }
                                    }
                                }
                            }
                        }
                        else if (!z.datumPocetkaTermina.Equals(dat) && z.datumKrajaTermina.Equals(dat))
                        {
                            krajSati = z.krajTermina.Split(':')[0];
                            krajMinuti = z.krajTermina.Split(':')[1];

                            // od pocetka dana do kraja renovacije sve izbacujemo
                            trajanjeSati = Convert.ToInt32(krajSati) - Convert.ToInt32("07");  // 0 1 2 ... koliko slotova izbacujemo
                            trajanjeMinuti = Convert.ToInt32(krajMinuti) - Convert.ToInt32("00");  // 0 30 -30

                            if (trajanjeMinuti == 0)
                            {
                                izbaceniSlotoviMinuti = 0;
                            }
                            else if (trajanjeMinuti == 30)
                            {
                                izbaceniSlotoviMinuti = 1;
                            }
                            else if (trajanjeMinuti == -30)
                            {
                                izbaceniSlotoviMinuti = -1;
                            }

                            izbaceniSlotovi = trajanjeSati * 2 + izbaceniSlotoviMinuti;

                            for (int i = 0; i <= izbaceniSlotovi; i++)
                            {
                                if (i != izbaceniSlotovi)
                                {
                                    if (sviSlobodniTermini.Count > 0)  //     ------------------------------------------
                                    {
                                        sviSlobodniTermini.RemoveAt(0);
                                    }
                                }
                                if (sviSlobodniTerminiKraj.Count > 0)
                                {
                                    sviSlobodniTerminiKraj.RemoveAt(0);
                                }
                            }
                        }
                        else if (datumZakazivanja < datumKraja && datumZakazivanja > datumPocetka) // ako je trenutni datum izmedju vremena pocetka i vremena kraja renovacije
                        {
                            Console.WriteLine("USLO ovde");

                            sviSlobodniTermini.Clear();
                            sviSlobodniTerminiKraj.Clear();

                            MessageBox.Show("Prostorija se renovira u to vreme!");
                        }
                    }
                    // slobodni termini u selektovanoj sali
                    if (TerminMenadzer.NadjiTerminPoId(z.idTermina) != null)  // za zauzece nadjemo koji je to termin
                    {
                        Termin t = TerminMenadzer.NadjiTerminPoId(z.idTermina);

                        if (z.datumPocetkaTermina.Equals(dat))
                        {
                            pocSati = z.pocetakTermina.Split(':')[0];
                            pocMinuti = z.pocetakTermina.Split(':')[1];

                            krajSati = z.krajTermina.Split(':')[0];
                            krajMinuti = z.krajTermina.Split(':')[1];

                            trajanjeSati = Convert.ToInt32(krajSati) - Convert.ToInt32(pocSati);  // 0 1 2 ... koliko slotova izbacujemo
                            trajanjeMinuti = Convert.ToInt32(krajMinuti) - Convert.ToInt32(pocMinuti);  // 0 30 -30

                            if (trajanjeMinuti == 0)
                            {
                                izbaceniSlotoviMinuti = 0;
                            }
                            else if (trajanjeMinuti == 30)
                            {
                                izbaceniSlotoviMinuti = 1;
                            }
                            else if (trajanjeMinuti == -30)
                            {
                                izbaceniSlotoviMinuti = -1;
                            }

                            izbaceniSlotovi = trajanjeSati * 2 + izbaceniSlotoviMinuti;

                            int index = sviSlobodniTermini.IndexOf(z.pocetakTermina);

                            if (index != -1)
                            {
                                for (int i = 0; i < izbaceniSlotovi; i++)
                                {
                                    sviSlobodniTermini.RemoveAt(index);
                                    sviSlobodniTerminiKraj.RemoveAt(index);

                                }
                            }
                        }
                    }
                }
                // slobodni termini lekara
                foreach (Sala s in SaleMenadzer.sale)
                {
                    foreach (ZauzeceSale z in s.zauzetiTermini)
                    {
                        if (z.datumPocetkaTermina.Equals(dat)) // za selektovani datum gledamo zauzetost selektovanog lekara
                        {
                            if (TerminMenadzer.NadjiTerminPoId(z.idTermina) != null)
                            {
                                Termin pomocna = TerminMenadzer.NadjiTerminPoId(z.idTermina);

                                if (pomocna.Lekar.IdLekara == Lekar.IdLekara)
                                {
                                    pocSati = z.pocetakTermina.Split(':')[0];
                                    pocMinuti = z.pocetakTermina.Split(':')[1];

                                    krajSati = z.krajTermina.Split(':')[0];
                                    krajMinuti = z.krajTermina.Split(':')[1];

                                    trajanjeSati = Convert.ToInt32(krajSati) - Convert.ToInt32(pocSati);  // 0 1 2 ... koliko slotova izbacujemo
                                    trajanjeMinuti = Convert.ToInt32(krajMinuti) - Convert.ToInt32(pocMinuti);  // 0 30 -30

                                    if (trajanjeMinuti == 0)
                                    {
                                        izbaceniSlotoviMinuti = 0;
                                    }
                                    else if (trajanjeMinuti == 30)
                                    {
                                        izbaceniSlotoviMinuti = 1;
                                    }
                                    else if (trajanjeMinuti == -30)
                                    {
                                        izbaceniSlotoviMinuti = -1;
                                    }

                                    izbaceniSlotovi = trajanjeSati * 2 + izbaceniSlotoviMinuti;

                                    int index = sviSlobodniTermini.IndexOf(z.pocetakTermina);

                                    if (index != -1)
                                    {
                                        for (int i = 0; i < izbaceniSlotovi; i++)
                                        {
                                            sviSlobodniTermini.RemoveAt(index);
                                            sviSlobodniTerminiKraj.RemoveAt(index);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                // slobodni termini pacijenta - pacijent ne moze biti na istim mestima u isto vreme
                foreach (Sala s in SaleMenadzer.sale)
                {
                    foreach (ZauzeceSale z in s.zauzetiTermini)
                    {
                        if (z.datumPocetkaTermina.Equals(dat)) // za selektovani datum gledamo zauzetost selektovanog lekara
                        {
                            if (TerminMenadzer.NadjiTerminPoId(z.idTermina) != null)
                            {
                                Termin pomocna = TerminMenadzer.NadjiTerminPoId(z.idTermina);

                                if (pomocna.Pacijent.IdPacijenta == Pacijent.IdPacijenta)
                                {
                                    pocSati = z.pocetakTermina.Split(':')[0];
                                    pocMinuti = z.pocetakTermina.Split(':')[1];

                                    krajSati = z.krajTermina.Split(':')[0];
                                    krajMinuti = z.krajTermina.Split(':')[1];

                                    trajanjeSati = Convert.ToInt32(krajSati) - Convert.ToInt32(pocSati);  // 0 1 2 ... koliko slotova izbacujemo
                                    trajanjeMinuti = Convert.ToInt32(krajMinuti) - Convert.ToInt32(pocMinuti);  // 0 30 -30

                                    if (trajanjeMinuti == 0)
                                    {
                                        izbaceniSlotoviMinuti = 0;
                                    }
                                    else if (trajanjeMinuti == 30)
                                    {
                                        izbaceniSlotoviMinuti = 1;
                                    }
                                    else if (trajanjeMinuti == -30)
                                    {
                                        izbaceniSlotoviMinuti = -1;
                                    }

                                    izbaceniSlotovi = trajanjeSati * 2 + izbaceniSlotoviMinuti;

                                    int index = sviSlobodniTermini.IndexOf(z.pocetakTermina);

                                    if (index != -1)
                                    {
                                        for (int i = 0; i < izbaceniSlotovi; i++)
                                        {
                                            sviSlobodniTermini.RemoveAt(index);
                                            sviSlobodniTerminiKraj.RemoveAt(index);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //  vremePocetka.ItemsSource = SlobodnoVremePocetka;
                //  vremeKraja.ItemsSource = SlobodnoVremeKraja;
            }
            else
            {
                MessageBox.Show("Unesite prostoriju.");
            }
        }



        private void IzbaciDanasnjeProsleTermine()
        {
            if (datum.SelectedDate == DateTime.Now.Date)
            {
                foreach (string slot in sviSlobodniTermini.ToList())
                {
                    DateTime vreme = DateTime.Parse(slot);
                    DateTime sada = DateTime.Now;
                    if (vreme.TimeOfDay <= sada.TimeOfDay)
                    {
                        sviSlobodniTermini.Remove(slot);
                    }
                }
            }
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

        private void vkk_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            nadjiDozvoljeneTermineKraja();
        }

        private void vpp_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PocetnoVreme = vpp.Text;
            //nadjiDozvoljeneTermineKraja();
        }

        private void nadjiDozvoljeneTermineKraja()
        {
            string[] pocetak = PocetnoVreme.Split(':');
            string pocetakSati = pocetak[0];
            string pocetakMinuti = pocetak[1];
            /*Izbaci neodgovarajuce termine za kraj*/
            foreach (string s in pomocnaSviSlobodniTerminiKraj.ToList())
            {
                string[] kraj = s.Split(':');
                string krajTerminaSati = kraj[0];
                string krajTerminaMinuti = kraj[1];
               
                if (Convert.ToInt32(pocetakSati) >= Convert.ToInt32(krajTerminaSati) && Convert.ToInt32(pocetakMinuti) == 30)
                {
                    sviSlobodniTerminiKraj.Remove(s);
                }
                else if (Convert.ToInt32(pocetakSati) > Convert.ToInt32(krajTerminaSati))
                {
                    sviSlobodniTerminiKraj.Remove(s);
                }
            }

           // izbaciKrajTermina();

            sviSlobodniTerminiKraj.Remove(vpp.Text);
            vkk.ItemsSource = sviSlobodniTerminiKraj;
            
            /*foreach(string s in pomocnaSviSlobodniTerminiKraj)
            {
                string[] kraj = s.Split(':');
                string krajTerminaSati = kraj[0];
                string krajTerminaMinuti = kraj[1];

                if (!sviSlobodniTerminiKraj.Contains(s) && ((Convert.ToInt32(pocetakSati) <= Convert.ToInt32(krajTerminaSati)))) 
                {
                    sviSlobodniTerminiKraj.Add(s);
                    sviSlobodniTerminiKraj.Remove(vpp.Text);

                }
            }
           */
            izbaciKrajTermina();
        }

        private void izbaciKrajTermina()
        {
            string[] pocetak = PocetnoVreme.Split(':');
            string pocetakSati = pocetak[0];
            string pocetakMinuti = pocetak[1];

            foreach(ZauzeceSale zauzece in Sala.zauzetiTermini)
            {
                string[] pocetakZauzeca = zauzece.pocetakTermina.Split(':');
                string pocetakSatiZauzeca = pocetakZauzeca[0];
                string pocetakMinutiZauzeca = pocetakZauzeca[1];
                //nadje prvi sledeci termin koji je zakazan
                Console.WriteLine("Prvi sledeci zauzeti termin: " + pocetakSatiZauzeca + ":" + pocetakMinutiZauzeca);
                Console.WriteLine("Izabrani pocetak: " + pocetakSati + ":" + pocetakMinuti);

                if (Convert.ToInt32(pocetakSatiZauzeca) > Convert.ToInt32(pocetakSati) && dat == zauzece.datumPocetkaTermina) //dodati i ako je isto
                {
                    Console.WriteLine("USLOOOOOOOOOOOOOOOOOOOOOOOOOO");
                    foreach(string s in pomocnaSviSlobodniTerminiKraj.ToList())
                    {
                        string[] pocetakKraja = s.Split(':');
                        string pocetakSatiKraja = pocetakKraja[0];
                        string pocetakMinutiKraja = pocetakKraja[1];

                        if(Convert.ToInt32(pocetakSatiKraja) > Convert.ToInt32(pocetakSatiZauzeca) && sviSlobodniTerminiKraj.Contains(s)) //&& sviSlobodniTerminiKraj.Contains(s)
                        {
                            sviSlobodniTerminiKraj.Remove(s);
                        }
                    }
                    //break;
                }
            }
           
        }
        private void vpp_LostFocus(object sender, RoutedEventArgs e)
        {
            //nadjiDozvoljeneTermineKraja();

        }
    }
}
