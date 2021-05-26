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
    /// Interaction logic for IzmeniTerminSekretar.xaml
    /// </summary>
    public partial class IzmeniTerminSekretar : Window
    {
        public Termin termin; 
        public List<Pacijent> AzuriranaLista = new List<Pacijent>();
        public Pacijent Pacijent;
        public Lekar Lekar;
        public Sala Sala;
        public string dat;
        public string PocetnoVreme;
        int izbaceniSlotoviMinuti;
        public static ObservableCollection<string> SlobodnoVremePocetka { get; set; }
        public static ObservableCollection<string> SlobodnoVremeKraja { get; set; }

        public IzmeniTerminSekretar(Termin izabraniTermin)
        {
            InitializeComponent();
            this.termin = izabraniTermin;

            this.listaPacijenata.ItemsSource = PacijentiMenadzer.pacijenti;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource);
            view.Filter = UserFilterPacijenti;

            this.listaLekara.ItemsSource = LekariMenadzer.lekari;
            CollectionView viewLekari = (CollectionView)CollectionViewSource.GetDefaultView(listaLekara.ItemsSource);
            viewLekari.Filter = UserFilterLekari;

            SlobodnoVremePocetka = new ObservableCollection<string>() {"07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00"};

            SlobodnoVremeKraja = new ObservableCollection<string>() {"07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00"};
            vremePocetka.ItemsSource = SlobodnoVremePocetka;
            vremeKraja.ItemsSource = SlobodnoVremeKraja;

            if (izabraniTermin != null)
            {
                termin.IdTermin = izabraniTermin.IdTermin;

                // vreme
                vremePocetka.SelectedItem = izabraniTermin.VremePocetka;
                PocetnoVreme = vremePocetka.Text;       
                vremeKraja.SelectedItem = izabraniTermin.VremeKraja;

                // lekar
                lekari.Text = izabraniTermin.Lekar.ImeLek + " " + izabraniTermin.Lekar.PrezimeLek;
                Lekar = izabraniTermin.Lekar;

                for (int i = 0; i < LekariMenadzer.lekari.Count; i++)
                {
                    if (LekariMenadzer.lekari[i].IdLekara == izabraniTermin.Lekar.IdLekara)
                    {
                        listaLekara.SelectedIndex = i;
                    }
                }

                // pacijent
                pacijenti.Text = izabraniTermin.Pacijent.ImePacijenta + " " + izabraniTermin.Pacijent.PrezimePacijenta;
                Pacijent = izabraniTermin.Pacijent;

                for (int j = 0; j < PacijentiMenadzer.pacijenti.Count; j++)
                {
                    if (PacijentiMenadzer.pacijenti[j].IdPacijenta == izabraniTermin.Pacijent.IdPacijenta)
                    {
                        listaPacijenata.SelectedIndex = j;
                    }
                }

                // tip termina
                TipTermina tp;
                if (izabraniTermin.tipTermina.Equals(TipTermina.Pregled))
                {
                    tip.SelectedIndex = 0;
                    
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

                    prostorije.SelectedItem = Sala;
                }
                else if (izabraniTermin.tipTermina.Equals(TipTermina.Operacija))
                {
                    tip.SelectedIndex = 1;

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

                    prostorije.SelectedItem = Sala;
                }
                tp = izabraniTermin.tipTermina;

                // prostorija
                Sala = izabraniTermin.Prostorija;
                prostorije.Text = izabraniTermin.Prostorija.Id.ToString();  


                // datum
                datum.SelectedDate = DateTime.Parse(izabraniTermin.Datum);
            }

           // datum.BlackoutDates.AddDatesInPast();   // ne mogu se menjati termini koji su prosli, za njih ovo javlja error - TODO: handle exception
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

        // azuriranje liste pacijenata prilikom dodavanja guest pacijenta
        public void AzurirajListuPacijenata()
        {
            foreach (Pacijent pacijent in PacijentiMenadzer.pacijenti)
            {
                AzuriranaLista.Add(pacijent);
            }

            listaPacijenata.ItemsSource = AzuriranaLista;
            int duzina = AzuriranaLista.Count;
            listaPacijenata.SelectedIndex = duzina - 1;
        }

        private void Pretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(listaLekara.ItemsSource).Refresh();
        }

        private void ListaPacijenata_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (listaPacijenata.SelectedItem != null)
            {
                Pacijent = (Pacijent)listaPacijenata.SelectedItem;
                pacijenti.Text = Pacijent.ImePacijenta + " " + Pacijent.PrezimePacijenta;
            }
        }

        private void ListaLekara_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (listaLekara.SelectedItem != null)
            {
                Lekar = (Lekar)listaLekara.SelectedItem;
                lekari.Text = Lekar.ImeLek + " " + Lekar.PrezimeLek;
            }
        }

        private void IzbaciDanasnjeProsleTermine()
        {
            if (datum.SelectedDate == DateTime.Now.Date)
            {
                foreach (string slot in SlobodnoVremePocetka.ToList())
                { 
                    DateTime vreme = DateTime.Parse(slot);
                    DateTime sada = DateTime.Now;
                    if (vreme.TimeOfDay <= sada.TimeOfDay)
                    {
                        SlobodnoVremePocetka.Remove(slot);
                    }
                }
            }
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
                Sala = SaleServis.NadjiSaluPoId((int)prostorije.SelectedItem);
            }

            if (Sala != null)
            {
                foreach (ZauzeceSale z in Sala.zauzetiTermini)
                {
                    if (TerminiSekretarServis.NadjiTerminPoId(z.idTermina) != null)
                    {
                        Termin t = TerminiSekretarServis.NadjiTerminPoId(z.idTermina);

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

                            int index = SlobodnoVremePocetka.IndexOf(z.pocetakTermina);

                            if (index != -1)
                            {
                                for (int i = 0; i < izbaceniSlotovi; i++)
                                {
                                    SlobodnoVremePocetka.RemoveAt(index);
                                    SlobodnoVremeKraja.RemoveAt(index);
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
                            if (TerminiSekretarServis.NadjiTerminPoId(z.idTermina) != null)
                            {
                                Termin pomocna = TerminiSekretarServis.NadjiTerminPoId(z.idTermina);

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

                                    int index = SlobodnoVremePocetka.IndexOf(z.pocetakTermina);

                                    if (index != -1)
                                    {
                                        for (int i = 0; i < izbaceniSlotovi; i++)
                                        {
                                            SlobodnoVremePocetka.RemoveAt(index);
                                            SlobodnoVremeKraja.RemoveAt(index);
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
                            if (TerminiSekretarServis.NadjiTerminPoId(z.idTermina) != null)
                            {
                                Termin pomocna = TerminiSekretarServis.NadjiTerminPoId(z.idTermina);

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

                                    int index = SlobodnoVremePocetka.IndexOf(z.pocetakTermina);

                                    if (index != -1)
                                    {
                                        for (int i = 0; i < izbaceniSlotovi; i++)
                                        {
                                            SlobodnoVremePocetka.RemoveAt(index);
                                            SlobodnoVremeKraja.RemoveAt(index);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("Unesite prostoriju.");
            }
        }

        private void Datum_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = datum.SelectedDate;
            if (selectedDate.HasValue)
            {
                dat = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }

            //vremePocetka.ItemsSource = SlobodnoVremePocetka;
            //vremeKraja.ItemsSource = SlobodnoVremeKraja;

            //IzbaciDanasnjeProsleTermine();            
            //IzbaciZauzeteTermine();  
        }

        private void Tip_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)  // tip_SelectionChanged
        {
            prostorije.Items.Clear();

            if (tip.SelectedIndex == 0) // pregled
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
            else if (tip.SelectedIndex == 1) // operacija
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

            prostorije.SelectedIndex = 0;
        }

        private void VremeKraja_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            string[] pocetak = PocetnoVreme.Split(':');
            string pocetakSati = pocetak[0];
            string pocetakMinuti = pocetak[1];
            vremeKraja.ItemsSource = SlobodnoVremeKraja;

            foreach (string s in SlobodnoVremeKraja.ToList())
            {
                string[] kraj = s.Split(':');
                string krajSati = kraj[0];
                //string krajMinuti = kraj[1];

                if (Convert.ToInt32(pocetakSati) >= Convert.ToInt32(krajSati) && Convert.ToInt32(pocetakMinuti) == 30)
                {
                    SlobodnoVremeKraja.Remove(s);
                }
                else if (Convert.ToInt32(pocetakSati) > Convert.ToInt32(krajSati))
                {
                    SlobodnoVremeKraja.Remove(s);
                }
            }

            SlobodnoVremeKraja.Remove(vremePocetka.Text);
            vremeKraja.ItemsSource = SlobodnoVremeKraja;
        }

        private void VremePocetka_LostFocus(object sender, RoutedEventArgs e)
        {
            PocetnoVreme = vremePocetka.Text;                
            vremeKraja.ItemsSource = SlobodnoVremeKraja;
        }

        private void Guest_nalog_Click(object sender, RoutedEventArgs e)
        {
            DodajPacijentaGuestIzmeni dodavanje = new DodajPacijentaGuestIzmeni(this);
            dodavanje.Show();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
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

            // datum
            DateTime? selectedDate = datum.SelectedDate;
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

            foreach (Termin t in TerminMenadzer.termini)
            {
                if (termin.IdTermin == t.IdTermin)
                {
                    foreach (Sala sala in SaleMenadzer.sale)
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
                foreach (Termin t in TerminMenadzer.termini)
                {
                    if (t.Prostorija.Id == termin.Prostorija.Id)
                    {
                        t.Prostorija = SaleServis.NadjiSaluPoId(termin.Prostorija.Id);
                    }
                }
            }

            TerminiSekretarServis.IzmeniTerminSekretar(termin, izmenjeniTermin);
            ZauzeceSale z = new ZauzeceSale(vp, vk, dat, termin.IdTermin);
            Sala.zauzetiTermini.Add(z);

            TerminiSekretarServis.sacuvajIzmene();
            SaleServis.sacuvajIzmjene();

            this.Close();
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

    }
}
