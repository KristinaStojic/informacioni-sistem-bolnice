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
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmeniTerminLekara.xaml
    /// </summary>
    public partial class IzmeniTerminLekara : Window
    {
        public Termin termin; 

        public List<Pacijent> AzuriranaLista = new List<Pacijent>();
        public Pacijent Pacijent;
        public Lekar Lekar;
        public Sala Sala;
        public string dat;
        public string PocetnoVreme;
        int izbaceniSlotoviMinuti;
        public static ObservableCollection<string> sviSlobodniTermini { get; set; }
        public static ObservableCollection<string> sviSlobodniTerminiKraj { get; set; }
        public static ObservableCollection<string> pomocnaSviSlobodniTermini { get; set; }
        public static ObservableCollection<string> pomocnaSviSlobodniTerminiKraj { get; set; }


        string pocSati;
        string pocMinuti;
        string krajSati;
        string krajMinuti;
        int trajanjeSati;
        int trajanjeMinuti;
        int izbaceniSlotovi;


        DateTime datumPocetkaRenoviranja;
        DateTime datumKrajaRenoviranja;
        DateTime datumZakazivanjaTermina;

        public IzmeniTerminLekara()
        {
            InitializeComponent();
        }
        public IzmeniTerminLekara(Termin izabraniTermin)
        {
            InitializeComponent();
            this.termin = izabraniTermin;
            this.DataContext = this;

            this.listaPacijenata.ItemsSource = PacijentiServis.pacijenti();
            List<Pacijent> pacijenti = PacijentiServis.PronadjiSve();
          

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource);
            view.Filter = UserFilterPacijenti;

            this.listaLekara.ItemsSource = LekariMenadzer.lekari;
            CollectionView viewLekari = (CollectionView)CollectionViewSource.GetDefaultView(listaLekara.ItemsSource);
            viewLekari.Filter = UserFilterLekari;




            dodajSveSlobodneTermine();

            vpp.ItemsSource = sviSlobodniTermini;
            vkk.ItemsSource = sviSlobodniTerminiKraj;

            if (izabraniTermin != null)
            {
                termin.IdTermin = izabraniTermin.IdTermin;

                //vpp.SelectedItem = termin.VremePocetka;
                PocetnoVreme = termin.VremePocetka;
                vkk.SelectedItem = termin.VremeKraja;

                vpp.Text = izabraniTermin.VremePocetka;

                // lekar
                lekar.Text = izabraniTermin.Lekar.ImeLek + " " + izabraniTermin.Lekar.PrezimeLek;
                Lekar = izabraniTermin.Lekar;
                for (int i = 0; i < LekariMenadzer.lekari.Count; i++)
                {
                    if (LekariMenadzer.lekari[i].IdLekara == izabraniTermin.Lekar.IdLekara)
                    {
                        listaLekara.SelectedIndex = i;
                    }
                }

                // pacijent
                pacijent.Text = izabraniTermin.Pacijent.ImePacijenta + " " + izabraniTermin.Pacijent.PrezimePacijenta;
                Pacijent = izabraniTermin.Pacijent;

                for (int j = 0; j < PacijentiServis.pacijenti().Count; j++)
                {
                    if (PacijentiServis.pacijenti()[j].IdPacijenta == izabraniTermin.Pacijent.IdPacijenta)
                    {
                        listaPacijenata.SelectedIndex = j;
                    }
                }

                // tip termina
                TipTermina tp;
                if (izabraniTermin.tipTermina.Equals(TipTermina.Pregled))
                {
                    tipPregleda.SelectedIndex = 0;

                    foreach (Sala s in SaleServis.Sale())
                    {
                        if (s.TipSale.Equals(tipSale.SalaZaPregled))
                        {
                            if (!prostorije.Items.Contains(s.Id))
                            {
                                prostorije.Items.Add(s.Id);
                            }
                        }
                    }

                    prostorije.SelectedItem = Sala;
                }
                else if (izabraniTermin.tipTermina.Equals(TipTermina.Operacija))
                {
                    tipPregleda.SelectedIndex = 1;

                    foreach (Sala s in SaleServis.Sale())
                    {
                        if (s.TipSale.Equals(tipSale.OperacionaSala))
                        {
                            if (!prostorije.Items.Contains(s.Id))
                            {
                                prostorije.Items.Add(s.Id);
                            }
                        }
                    }

                    prostorije.SelectedItem = Sala;
                }
                tp = izabraniTermin.tipTermina;

                // prostorija
                Sala = izabraniTermin.Prostorija;
                prostorije.Text = izabraniTermin.Prostorija.Id.ToString();


                // datum
                datum.SelectedDate = DateTime.Parse(izabraniTermin.Datum);
            }

            //datum.BlackoutDates.AddDatesInPast();   // ne mogu se menjati termini koji su prosli, za njih ovo javlja error - TODO: handle exception

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
            //PocetnoVreme = vpp.Text;
            //vkk.ItemsSource = sviSlobodniTerminiKraj;


        }

        private void vkk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void prostorije_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            IzbaciZauzeteTermineSale();

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

        private void NadjiIzabranuSalu()
        {
            if (prostorije.SelectedItem != null)
            {
                Sala = SaleServis.NadjiSaluPoId((int)prostorije.SelectedItem);
            }
        }

        private bool RenoviranjeSaleTrajeJedanDan(ZauzeceSale zauzece)
        {
            return zauzece.datumPocetkaTermina.Equals(dat) && zauzece.datumKrajaTermina.Equals(dat);
        }


        private bool RenoviranjeSalePocinjeIstiDanKaoTermin(ZauzeceSale zauzece)
        {
            return zauzece.datumPocetkaTermina.Equals(dat) && !zauzece.datumKrajaTermina.Equals(dat);
        }

        private bool RenoviranjeSaleSeZavrsavaIstiDanKaoTermin(ZauzeceSale zauzece)
        {
            return !zauzece.datumPocetkaTermina.Equals(dat) && zauzece.datumKrajaTermina.Equals(dat);
        }

        private bool ProstorijaSeRenovira()
        {
            return datumZakazivanjaTermina < datumKrajaRenoviranja && datumZakazivanjaTermina > datumPocetkaRenoviranja;
        }

        private void IzracunajBrojSlotovaZaIzbacivanje()
        {

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

        }
        private void NadjiDatumeRenoviranjaSale(ZauzeceSale zauzece)
        {
            datumPocetkaRenoviranja = DateTime.Parse(zauzece.datumPocetkaTermina);
            datumKrajaRenoviranja = DateTime.Parse(zauzece.datumKrajaTermina);
            datumZakazivanjaTermina = DateTime.Parse(dat);
        }
        private void BrisiIzabraneSlotove(int indexTermina)
        {
            for (int i = 0; i < izbaceniSlotovi; i++)
            {
                sviSlobodniTermini.RemoveAt(indexTermina);
                sviSlobodniTermini.RemoveAt(indexTermina);
            }
        }
        private void IzbaciTermineRenoviranjeJedanDan(ZauzeceSale z)
        {
            pocSati = z.pocetakTermina.Split(':')[0];
            pocMinuti = z.pocetakTermina.Split(':')[1];

            krajSati = z.krajTermina.Split(':')[0];
            krajMinuti = z.krajTermina.Split(':')[1];

            trajanjeSati = Convert.ToInt32(krajSati) - Convert.ToInt32(pocSati);  // 0 1 2 ... koliko slotova izbacujemo
            trajanjeMinuti = Convert.ToInt32(krajMinuti) - Convert.ToInt32(pocMinuti);  // 0 30 -30

            IzracunajBrojSlotovaZaIzbacivanje();

            int indexTermina = sviSlobodniTermini.IndexOf(z.pocetakTermina);

            if (indexTermina != -1)
            {
                BrisiIzabraneSlotove(indexTermina);
            }
        }
        private void BrisiIzabraneSlotoveRenoviranjePocinjeIstiDanKaoTermin(int indexTermina)
        {
            for (int i = 0; i <= izbaceniSlotovi; i++)
            {
                sviSlobodniTermini.RemoveAt(indexTermina);

                if (i != 0) // ne brisemo vreme pocetka renovacije iz ove liste jer termin moze da traje do vremena pocetka renovacije
                {
                    if (sviSlobodniTerminiKraj.Count > 0)
                    {
                        sviSlobodniTerminiKraj.RemoveAt(indexTermina + 1);
                    }
                }
            }
        }

        private void IzbaciTermineRenoviranjePocinjeIstiDanKaoTermin(ZauzeceSale zauzece)
        {
            pocSati = zauzece.pocetakTermina.Split(':')[0];
            pocMinuti = zauzece.pocetakTermina.Split(':')[1];

            trajanjeSati = Convert.ToInt32("20") - Convert.ToInt32(pocSati);  // 0 1 2 ... koliko slotova izbacujemo
            trajanjeMinuti = Convert.ToInt32("00") - Convert.ToInt32(pocMinuti);  // 0 30 -30

            IzracunajBrojSlotovaZaIzbacivanje();

            int indexTermina = sviSlobodniTermini.IndexOf(zauzece.pocetakTermina);
            if (indexTermina != -1)
            {
                BrisiIzabraneSlotoveRenoviranjePocinjeIstiDanKaoTermin(indexTermina);
            }
        }

        private void BrisiIzabraneSlotoveRenoviranjeSeZavrsavaIstiDanKaoTermin()
        {
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
        private void IzbaciTermineRenoviranjeSeZavrsavaIstiDanKaoTermin(ZauzeceSale zauzece)
        {
            krajSati = zauzece.krajTermina.Split(':')[0];
            krajMinuti = zauzece.krajTermina.Split(':')[1];

            // od pocetka dana do kraja renovacije sve izbacujemo
            trajanjeSati = Convert.ToInt32(krajSati) - Convert.ToInt32("07");  // 0 1 2 ... koliko slotova izbacujemo
            trajanjeMinuti = Convert.ToInt32(krajMinuti) - Convert.ToInt32("00");  // 0 30 -30

            IzracunajBrojSlotovaZaIzbacivanje();

            BrisiIzabraneSlotoveRenoviranjeSeZavrsavaIstiDanKaoTermin();
        }

        private void AzurirajDozvoljenaVremenaTermina()
        {
            sviSlobodniTermini.Clear();
            sviSlobodniTerminiKraj.Clear();
            MessageBox.Show("Prostorija se renovira u to vreme!");
        }
        private void IzbaciZauzeteTermineRenoviranjemSale()
        {
            foreach (ZauzeceSale z in Sala.zauzetiTermini)
            {
                if (z.idTermina == 0) // ako sala ima zakazano renoviranje
                {
                    NadjiDatumeRenoviranjaSale(z);

                    if (RenoviranjeSaleTrajeJedanDan(z)) // isti dan i pocinje i zavrsava se 
                    {
                        IzbaciTermineRenoviranjeJedanDan(z);
                    }
                    else if (RenoviranjeSalePocinjeIstiDanKaoTermin(z)) //pocinje isti dan kao termin a zavrsava se neki drugi
                    {
                        IzbaciTermineRenoviranjePocinjeIstiDanKaoTermin(z);
                    }
                    else if (RenoviranjeSaleSeZavrsavaIstiDanKaoTermin(z))
                    {
                        IzbaciTermineRenoviranjeSeZavrsavaIstiDanKaoTermin(z);

                    }
                    else if (ProstorijaSeRenovira()) // ako je trenutni datum izmedju vremena pocetka i vremena kraja renovacije
                    {
                        AzurirajDozvoljenaVremenaTermina();
                    }
                }
                // slobodni termini u selektovanoj sali
                IzbaciZauzeteTermineSale();

            }
        }

        private void IzbaciZauzeteTermine()
        {
            NadjiIzabranuSalu(); //pronadje salu koja je selektovana u combobox-u

            if (Sala != null) //ako je sala izabrana
            {
                IzbaciZauzeteTermineRenoviranjemSale();
                // slobodni termini lekara
                IzbaciZauzeteTermineLekara();
                // slobodni termini pacijenta - pacijent ne moze biti na istim mestima u isto vreme
                IzbaciZauzeteTerminePacijenata();

            }
            else
            {
                MessageBox.Show("Unesite prostoriju.");
            }
        }

        private void IzbaciZauzeteTermineSale()
        {
            if (Sala != null)
            {
                foreach (ZauzeceSale z in Sala.zauzetiTermini)
                {
                    if (TerminServisLekar.NadjiTerminPoId(z.idTermina) != null)  // za zauzece nadjemo koji je to termin
                    {
                        Termin t = TerminServisLekar.NadjiTerminPoId(z.idTermina);

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
            }


        }

        private void IzbaciZauzeteTermineLekara()
        {
            foreach (Sala s in SaleServis.Sale())
            {
                foreach (ZauzeceSale z in s.zauzetiTermini)
                {
                    if (z.datumPocetkaTermina.Equals(dat)) // za selektovani datum gledamo zauzetost selektovanog lekara
                    {
                        if (TerminServisLekar.NadjiTerminPoId(z.idTermina) != null)
                        {
                            Termin pomocna = TerminServisLekar.NadjiTerminPoId(z.idTermina);

                            if (pomocna.Lekar.IdLekara == Lekar.IdLekara)
                            {
                                pocSati = z.pocetakTermina.Split(':')[0];
                                pocMinuti = z.pocetakTermina.Split(':')[1];

                                krajSati = z.krajTermina.Split(':')[0];
                                krajMinuti = z.krajTermina.Split(':')[1];

                                trajanjeSati = Convert.ToInt32(krajSati) - Convert.ToInt32(pocSati);  // 0 1 2 ... koliko slotova izbacujemo
                                trajanjeMinuti = Convert.ToInt32(krajMinuti) - Convert.ToInt32(pocMinuti);  // 0 30 -30

                                IzracunajBrojSlotovaZaIzbacivanje();

                                int indexTermina = sviSlobodniTermini.IndexOf(z.pocetakTermina);

                                if (indexTermina != -1)
                                {
                                    BrisiIzabraneSlotove(indexTermina);
                                }
                            }
                        }
                    }
                }
            }
        }
        private void IzbaciZauzeteTerminePacijenata()
        {
            foreach (Sala s in SaleServis.Sale())
            {
                foreach (ZauzeceSale z in s.zauzetiTermini)
                {
                    if (z.datumPocetkaTermina.Equals(dat)) // za selektovani datum gledamo zauzetost selektovanog lekara
                    {
                        if (TerminServisLekar.NadjiTerminPoId(z.idTermina) != null)
                        {
                            Termin pomocna = TerminServisLekar.NadjiTerminPoId(z.idTermina);

                            if (pomocna.Pacijent.IdPacijenta == Pacijent.IdPacijenta)
                            {
                                pocSati = z.pocetakTermina.Split(':')[0];
                                pocMinuti = z.pocetakTermina.Split(':')[1];

                                krajSati = z.krajTermina.Split(':')[0];
                                krajMinuti = z.krajTermina.Split(':')[1];

                                trajanjeSati = Convert.ToInt32(krajSati) - Convert.ToInt32(pocSati);  // 0 1 2 ... koliko slotova izbacujemo
                                trajanjeMinuti = Convert.ToInt32(krajMinuti) - Convert.ToInt32(pocMinuti);  // 0 30 -30

                                IzracunajBrojSlotovaZaIzbacivanje();

                                int indexTermina = sviSlobodniTermini.IndexOf(z.pocetakTermina);

                                if (indexTermina != -1)
                                {
                                    BrisiIzabraneSlotove(indexTermina);

                                }
                            }
                        }
                    }
                }
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
                //Lekar = new Lekar() { IdLekara = 1, ImeLek = "Petar", PrezimeLek = "Nebojsic", specijalizacija = Specijalizacija.Opsta_praksa };
                //Lekar = new Lekar() { IdLekara = 2, ImeLek = "Milos", PrezimeLek = "Dragojevic", specijalizacija = Specijalizacija.Opsta_praksa };
                //Lekar = new Lekar() { IdLekara = 3, ImeLek = "Petar", PrezimeLek = "Milosevic", specijalizacija = Specijalizacija.Specijalista };
                //Lekar = new Lekar() { IdLekara = 4, ImeLek = "Dejan", PrezimeLek = "Milosevic", specijalizacija = Specijalizacija.Specijalista };
                //Lekar = new Lekar() { IdLekara = 5, ImeLek = "Isidora", PrezimeLek = "Isidorovic", specijalizacija = Specijalizacija.Specijalista };
                //this.lekar.Text = Lekar.ImeLek + " " + Lekar.PrezimeLek;
                foreach (Lekar lekar in LekariMenadzer.lekari)
                {
                    if (lekar.IdLekara == termin.Lekar.IdLekara)
                    {
                        this.lekar.Text = lekar.ImeLek + " " + lekar.PrezimeLek;
                        this.listaLekara.IsEnabled = false;
                        this.Lekar = lekar;
                    }
                }
                this.listaLekara.IsEnabled = false;
                this.hitno.IsEnabled = false;
                dodajSaleZaPregled();
            }
            else if (tipPregleda.SelectedIndex == 1) // operacija
            {
                this.dugmeLekari.IsEnabled = true;
                //this.lekar.Text = "";
                this.listaLekara.IsEnabled = true;
                this.hitno.IsEnabled = true;
                dodajSaleZaOperacije();
            }

            prostorije.SelectedIndex = 0;
        }

        private void dodajSaleZaPregled()
        {
            foreach (Sala s in SaleServis.Sale())
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
            foreach (Sala s in SaleServis.Sale())
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

            //DORADITI-----------------------------------------------------------------------------------------------------------------------------------------------------------

            foreach (string s in pomocnaSviSlobodniTerminiKraj)
            {

                foreach (ZauzeceSale zauzece in Sala.zauzetiTermini)
                {
                    string[] pocetakZauzeca = zauzece.pocetakTermina.Split(':');
                    string pocetakSatiZauzeca = pocetakZauzeca[0];
                    string pocetakMinutiZauzeca = pocetakZauzeca[1];

                    string[] krajZauzeca = zauzece.krajTermina.Split(':');
                    string krajSatiZauzeca = krajZauzeca[0];
                    string krajMinutiZauzeca = krajZauzeca[1];


                    string[] kraj = s.Split(':');
                    string krajTerminaSati = kraj[0];
                    string krajTerminaMinuti = kraj[1];

                    if (Convert.ToInt32(pocetakSatiZauzeca) > Convert.ToInt32(pocetakSati) && dat == zauzece.datumPocetkaTermina) //dodati i ako je isto
                    {
                        if (!sviSlobodniTerminiKraj.Contains(s) && ((Convert.ToInt32(pocetakSati) <= Convert.ToInt32(krajSatiZauzeca))))
                        {
                            sviSlobodniTerminiKraj.Add(s);
                            sviSlobodniTerminiKraj.Remove(vpp.Text);

                        }
                    }
                    /*else if (Convert.ToInt32(pocetakSatiZauzeca) == Convert.ToInt32(pocetakSati) && dat == zauzece.datumPocetkaTermina && Convert.ToInt32(pocetakMinutiZauzeca) > Convert.ToInt32(pocetakMinuti)) //dodati i ako je isto
                    {
                        if (!sviSlobodniTerminiKraj.Contains(s) && ((Convert.ToInt32(pocetakSati) <= Convert.ToInt32(krajSatiZauzeca))))
                        {
                            sviSlobodniTerminiKraj.Add(s);
                            sviSlobodniTerminiKraj.Remove(vpp.Text);

                        }
                    }*/
                    else if (!sviSlobodniTerminiKraj.Contains(s) && ((Convert.ToInt32(pocetakSati) < Convert.ToInt32(krajTerminaSati))))
                    {
                        sviSlobodniTerminiKraj.Add(s);
                        sviSlobodniTerminiKraj.Remove(vpp.Text);

                    }
                    else if (!sviSlobodniTerminiKraj.Contains(s) && ((Convert.ToInt32(pocetakSati) == Convert.ToInt32(krajTerminaSati)) && ((Convert.ToInt32(pocetakMinuti) < Convert.ToInt32(krajTerminaMinuti)))))
                    {
                        sviSlobodniTerminiKraj.Add(s);
                        sviSlobodniTerminiKraj.Remove(vpp.Text);

                    }
                }

            }
            sviSlobodniTerminiKraj.Add(pocetakSati + ":" + "30");
            foreach (string s in pomocnaSviSlobodniTerminiKraj.ToList())
            {
                string[] kraj = s.Split(':');
                string krajTerminaSati = kraj[0];
                string krajTerminaMinuti = kraj[1];
                if (sviSlobodniTerminiKraj.Contains(s) && Convert.ToInt32(pocetakSati) >= Convert.ToInt32(krajTerminaSati)) //provjeri kad je isti sat a razliciti minuti
                {
                    sviSlobodniTerminiKraj.Remove(s);
                }
            }


            izbaciKrajTermina();
        }

        private void izbaciKrajTermina()
        {
            string[] pocetak = PocetnoVreme.Split(':');
            string pocetakSati = pocetak[0];
            string pocetakMinuti = pocetak[1];

            foreach (ZauzeceSale zauzece in Sala.zauzetiTermini)
            {
                string[] pocetakZauzeca = zauzece.pocetakTermina.Split(':');
                string pocetakSatiZauzeca = pocetakZauzeca[0];
                string pocetakMinutiZauzeca = pocetakZauzeca[1];

                //sprecava preklapanje termina (da kraj jednog termina bude posle/u toku drugog)
                if (Convert.ToInt32(pocetakSatiZauzeca) > Convert.ToInt32(pocetakSati) && dat == zauzece.datumPocetkaTermina) //dodati i ako je isto
                {
                    foreach (string s in pomocnaSviSlobodniTerminiKraj.ToList())
                    {
                        string[] pocetakKraja = s.Split(':');
                        string pocetakSatiKraja = pocetakKraja[0];
                        string pocetakMinutiKraja = pocetakKraja[1];

                        if (Convert.ToInt32(pocetakSatiKraja) > Convert.ToInt32(pocetakSatiZauzeca) && sviSlobodniTerminiKraj.Contains(s))
                        {
                            sviSlobodniTerminiKraj.Remove(s);
                        }
                        else if (Convert.ToInt32(pocetakSatiKraja) == Convert.ToInt32(pocetakSatiZauzeca) && Convert.ToInt32(pocetakMinutiKraja) > Convert.ToInt32(pocetakMinutiZauzeca) && sviSlobodniTerminiKraj.Contains(s))
                        {
                            sviSlobodniTerminiKraj.Remove(s);
                        }
                    }

                }
                //ako je termin npr. u 17:30 a izaberemo vreme pocetka u 17:00 -> izbacuje sve termine posle 17:30
                else if (Convert.ToInt32(pocetakSatiZauzeca) == Convert.ToInt32(pocetakSati) && dat == zauzece.datumPocetkaTermina && Convert.ToInt32(pocetakMinutiZauzeca) > Convert.ToInt32(pocetakMinuti)) //dodati i ako je isto
                {
                    foreach (string s in pomocnaSviSlobodniTerminiKraj.ToList())
                    {
                        string[] pocetakKraja = s.Split(':');
                        string pocetakSatiKraja = pocetakKraja[0];
                        string pocetakMinutiKraja = pocetakKraja[1];

                        if (Convert.ToInt32(pocetakSatiKraja) > Convert.ToInt32(pocetakSatiZauzeca) && sviSlobodniTerminiKraj.Contains(s))
                        {
                            sviSlobodniTerminiKraj.Remove(s);
                        }
                        else if (Convert.ToInt32(pocetakSatiKraja) == Convert.ToInt32(pocetakSatiZauzeca) && Convert.ToInt32(pocetakMinutiKraja) < Convert.ToInt32(pocetakMinutiZauzeca) && sviSlobodniTerminiKraj.Contains(s))
                        {
                            sviSlobodniTerminiKraj.Remove(s);
                        }
                    }

                }
            }

        }
        private void vpp_LostFocus(object sender, RoutedEventArgs e)
        {
            //nadjiDozvoljeneTermineKraja();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //potvrdi
            // vreme pocetka i kraja
            string vp = vpp.Text;
            string vk = vkk.Text;

            string[] pocetak = vp.Split(':');
            string pocetakSati = pocetak[0];
            string pocetakMinuti = pocetak[1];

            string[] kraj = vk.Split(':');
            string krajSati = kraj[0];
            string krajMinuti = kraj[1];

            // datum
            DateTime? selectedDate = datum.SelectedDate;
            if (selectedDate.HasValue)
            {
                dat = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }

            // tip termina
            TipTermina tp;
            if (tipPregleda.Text.Equals("Pregled"))
            {
                tp = TipTermina.Pregled;
            }
            else
            {
                tp = TipTermina.Operacija;
            }

            Lekar = (Lekar)listaLekara.SelectedItem;
            Pacijent = (Pacijent)listaPacijenata.SelectedItem;
            Sala = SaleServis.NadjiSaluPoId((int)prostorije.SelectedItem);

            Lekar l = LekariServis.NadjiPoId(Lekar.IdLekara);
            Pacijent pacijent = PacijentiServis.PronadjiPoId(Pacijent.IdPacijenta);
            Termin izmenjeniTermin = new Termin(termin.IdTermin, dat, vp, vk, tp, l, Sala, pacijent);

            //
            // termin koji menjamo
            string[] vpt = termin.VremePocetka.Split(':');
            string[] vkt = termin.VremeKraja.Split(':');

            foreach (Termin t in TerminServisLekar.termini())
            {
                if (termin.IdTermin == t.IdTermin)
                {
                    foreach (Sala sala in SaleServis.Sale())
                    {
                        if (sala.Id == termin.Prostorija.Id)
                        {
                            foreach (ZauzeceSale zauzece1 in sala.zauzetiTermini.ToList())                  // dodato ToList() izmena 
                            {
                                string[] zauzecePocetak1 = zauzece1.pocetakTermina.Split(':');
                                string[] zauzeceKraj1 = zauzece1.krajTermina.Split(':');

                                // izbaci iz zauzetih termina to zauzece sale koje menjamo
                                if (zauzece1.datumPocetkaTermina.Equals(termin.Datum) && zauzece1.idTermina == termin.IdTermin && zauzecePocetak1[0].Equals(vpt[0]) && zauzecePocetak1[1].Equals(vpt[1]) && zauzeceKraj1[0].Equals(vkt[0]) && zauzeceKraj1[1].Equals(vkt[1]))
                                {
                                    sala.zauzetiTermini.Remove(zauzece1);
                                    SaleServis.sacuvajIzmjene();
                                }
                            }
                        }
                    }
                }
            }

            // ako je prostorija izmenjena
            if (termin.Prostorija.Id != izmenjeniTermin.Prostorija.Id)
            {
                foreach (Termin t in TerminServisLekar.termini())
                {
                    if (t.Prostorija.Id == termin.Prostorija.Id)
                    {
                        t.Prostorija = SaleServis.NadjiSaluPoId(termin.Prostorija.Id);
                    }
                }
            }

            TerminServisLekar.IzmeniTerminLekar(termin, izmenjeniTermin);
            ZauzeceSale z = new ZauzeceSale(vp, vk, dat, termin.IdTermin);
            Sala.zauzetiTermini.Add(z);

            TerminServisLekar.sacuvajIzmene();
            SaleServis.sacuvajIzmjene();
            this.Close();
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //odustani
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_2(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_3(sender, e);
            }
        }
    }
}

