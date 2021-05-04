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
using Model;
using Projekat.Model;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for HitanSlucaj.xaml
    /// </summary>
    public partial class HitanSlucaj : Window
    {
        public List<Pacijent> AzuriranaLista = new List<Pacijent>();
        public Pacijent Pacijent;
        public TipTermina Tip;
        Sala Sala;
        Lekar Lekar;
        public string datum = DateTime.Now.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        
        int VremePocetkaSati;
        int VremePocetkaMinuti;
        int VremeKrajaSati;
        int VremeKrajaMinuti; 

        string VremePocetkaMinutiKonvertovano;
        string VremeKrajaMinutiKonvertovano;
        string VremePocetkaSatiKonvertovano;
        string VremeKrajaSatiKonvertovano;

        public List<Sala> slobodneSale = new List<Sala>();
        public List<Lekar> slobodniLekari = new List<Lekar>();
        int slotoviMinuti;

        int trajanjeHitnogTermina;
        int izbaceniSlotoviMinuti;

        ObservableCollection<Termin> TerminiZauzeto = new ObservableCollection<Termin>();


        public HitanSlucaj()
        {
            InitializeComponent();

            this.listaPacijenata.ItemsSource = PacijentiMenadzer.pacijenti;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource);
            view.Filter = UserFilter;
        }

        private bool UserFilter(object item)
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

        // dodavanje novog termina kao hitan slucaj
        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            Termin t = new Termin(TerminMenadzer.GenerisanjeIdTermina(), datum, VremePocetkaSatiKonvertovano + ":" + VremePocetkaMinutiKonvertovano, VremeKrajaSatiKonvertovano + ":" + VremeKrajaMinutiKonvertovano, Tip, Lekar, Sala, Pacijent);
            TerminMenadzer.ZakaziTerminSekretar(t);
            ZauzeceSale z = new ZauzeceSale(VremePocetkaSatiKonvertovano + ":" + VremePocetkaMinutiKonvertovano, VremeKrajaSatiKonvertovano + ":" + VremeKrajaMinutiKonvertovano, datum, t.IdTermin);
            Sala.zauzetiTermini.Add(z);

            TerminMenadzer.sacuvajIzmene();
            SaleMenadzer.sacuvajIzmjene();

            // za svaki termin koji je zakazan u istoj prostoriji s, dodati to novo zauzece u zauzeca te prostorije
            foreach (Termin t1 in TerminMenadzer.termini)
            {
                if (t1.Prostorija.Id == Sala.Id)
                {
                    t1.Prostorija = Sala;
                }
            }
            TerminMenadzer.sacuvajIzmene();

            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // guest
        private void Guest_Pacijent(object sender, RoutedEventArgs e)
        {
            HitanSlucajDodajGuest dodavanje = new HitanSlucajDodajGuest(this);
            dodavanje.Show();
        }

        // pretrazi 
        private void Pretrazi_Click(object sender, RoutedEventArgs e)
        {
            ZakaziUNarednihPolaSata();
            PronadjiAdekvatneLekare();
            PronadjiAdekvatneSale();

            foreach (Lekar l in slobodniLekari)
            {
                if (SlobodanLekar(l.IdLekara) != null)
                {
                    Lekar = l;
                    break;
                }
            }

            foreach (Sala s in slobodneSale)
            {
                if (SlobodnaSala(s.Id) != null)
                {
                    Sala = s;
                    break;
                }
            }

            if (Pacijent == null)
            {
                MessageBox.Show("Unesite pacijenta!");
                PrikaziZauzeteTermine();
            }
            else if (Sala == null)
            {
                MessageBox.Show("Unesite salu!"); 
                PrikaziZauzeteTermine();
            }
            else if (Lekar == null)
            {
                MessageBox.Show("Unesite lekara!");
                PrikaziZauzeteTermine();
            }
            else
            {
                potvrdi.IsEnabled = true;
            }
        }

        private void Pomeri_Click(object sender, RoutedEventArgs e)
        {
            //zauzetiTermini.SelectedItems

            pretrazi.IsEnabled = false;
            zauzetiTermini.IsEnabled = false;
            potvrdi.IsEnabled = true;
        }

        private void PrikaziZauzeteTermine() 
        {
            pomeri.IsEnabled = true;
            zauzetiTermini.IsEnabled = true;
            
            zauzetiTermini.ItemsSource = TerminiZauzeto;
            
        }

        private void PomeriTermin(Termin termin) {
            // isti pacijent, lekar, trajanje (i isto vreme pocetka i kraja), datum  - druga prostorija -  nema smisla
            // danasnji termin se pomera najranije za sutra, ne moze za isti dan
            
            // isti pacijent, lekar, prostorija - drugo vreme pocetka i kraja datum    ili
            // isti pacijent, lekar - drugo vreme pocetka i kraja, datum, prostorija


        }

        private void PronadjiAdekvatneSale()
        {
            if (tip.Text.Equals("Pregled"))
            {
                Tip = TipTermina.Pregled;

                foreach (Sala sal in SaleMenadzer.sale)
                {
                    if (sal.TipSale.Equals(tipSale.SalaZaPregled))
                    {
                        slobodneSale.Add(sal);
                    }
                }
            }
            else if(tip.Text.Equals("Operacija"))
            {
                Tip = TipTermina.Operacija;

                foreach (Sala sal in SaleMenadzer.sale)
                {
                    if (sal.TipSale.Equals(tipSale.OperacionaSala))
                    {
                        slobodneSale.Add(sal);
                    }
                }
            }
        }

        private void PronadjiAdekvatneLekare()
        {
            if (oblast.Text.Equals("Opsta praksa"))
            {
                foreach (Lekar l in MainWindow.lekari)
                {
                    if (l.specijalizacija.Equals(Specijalizacija.Opsta_praksa))
                    {
                        slobodniLekari.Add(l);
                    }
                }
            }
            else if (oblast.Text.Equals("Ginekologija"))
            {
                foreach (Lekar l in MainWindow.lekari)
                {
                    if (l.specijalizacija.Equals(Specijalizacija.Ginekologija))
                    {
                        slobodniLekari.Add(l);
                    }
                }
            }
            else if (oblast.Text.Equals("Hirurgija"))
            {
                foreach (Lekar l in MainWindow.lekari)
                {
                    if (l.specijalizacija.Equals(Specijalizacija.Hirurgija))
                    {
                        slobodniLekari.Add(l);
                    }
                }
            }
            else if (oblast.Text.Equals("Ortopedija"))
            {
                foreach (Lekar l in MainWindow.lekari)
                {
                    if (l.specijalizacija.Equals(Specijalizacija.Ortopedija))
                    {
                        slobodniLekari.Add(l);
                    }
                }
            }
        }

        private Lekar SlobodanLekar(int idLekara)
        {
            ObservableCollection<string> SlobodnoVremePocetka = new ObservableCollection<string>()
                                                             { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00" };
                                                            
            string PocetakSati;
            string PocetakMinuti;
            string KrajSati;
            string KrajMinuti;
            int TrajanjeSati;
            int TrajanjeMinuti;
            int IzbaceniSlotovi;

            foreach (Sala s in SaleMenadzer.sale)
            {
                foreach (ZauzeceSale z in s.zauzetiTermini)
                {
                    if (z.datumPocetkaTermina.Equals(datum)) // za selektovani datum gledamo zauzetost selektovanog lekara
                    {
                        if (TerminMenadzer.NadjiTerminPoId(z.idTermina) != null)
                        {
                            Termin pomocna = TerminMenadzer.NadjiTerminPoId(z.idTermina);

                            if (pomocna.Lekar.IdLekara == idLekara)
                            {
                                PocetakSati = z.pocetakTermina.Split(':')[0];
                                PocetakMinuti = z.pocetakTermina.Split(':')[1];

                                KrajSati = z.krajTermina.Split(':')[0];
                                KrajMinuti = z.krajTermina.Split(':')[1];

                                TrajanjeSati = Convert.ToInt32(KrajSati) - Convert.ToInt32(PocetakSati);  // 0 1 2 ... koliko slotova izbacujemo
                                TrajanjeMinuti = Convert.ToInt32(KrajMinuti) - Convert.ToInt32(PocetakMinuti);  // 0 30 -30

                                if (TrajanjeMinuti == 0)
                                {
                                    izbaceniSlotoviMinuti = 0;
                                }
                                else if (TrajanjeMinuti == 30)
                                {
                                    izbaceniSlotoviMinuti = 1;
                                }
                                else if (TrajanjeMinuti == -30)
                                {
                                    izbaceniSlotoviMinuti = -1;
                                }

                                IzbaceniSlotovi = TrajanjeSati * 2 + izbaceniSlotoviMinuti;
                                int index = SlobodnoVremePocetka.IndexOf(z.pocetakTermina);

                                if (index != -1)
                                {
                                    for (int i = 0; i < IzbaceniSlotovi; i++)
                                    {
                                       // Zauzeto.Add(SlobodnoVremePocetka[index]);
                                        SlobodnoVremePocetka.RemoveAt(index);
                                    }

                                    if (!TerminiZauzeto.Contains(pomocna))
                                    {
                                        TerminiZauzeto.Add(pomocna);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // za zeljenog lekara imamo listu slobodnih termina za izabrani dan
            if (SlobodnoVremePocetka.Contains(VremePocetkaSatiKonvertovano + ":" + VremePocetkaMinutiKonvertovano))
            {
                int index = SlobodnoVremePocetka.IndexOf(VremePocetkaSatiKonvertovano + ":" + VremePocetkaMinutiKonvertovano);
                string prvi = SlobodnoVremePocetka[index];
               
                int noviIndex = index + trajanjeHitnogTermina;
                int index1 = SlobodnoVremePocetka.IndexOf(VremeKrajaSatiKonvertovano + ":" + VremeKrajaMinutiKonvertovano);

                if (index1 == -1)
                {
                    return null;
                }

                string drugi = SlobodnoVremePocetka[noviIndex];
                string treci = SlobodnoVremePocetka[index1];

                string[] drugiSplitovan = drugi.Split(':');
                string[] treciSplitovan = treci.Split(':'); 

                if(drugiSplitovan[0].Equals(treciSplitovan[0]) && drugiSplitovan[1].Equals(treciSplitovan[1]))
        //      if (SlobodnoVremePocetka.IndexOf(noviIndex.ToString()).Equals(SlobodnoVremePocetka.IndexOf(VremeKrajaSatiKonvertovano + ":" + VremeKrajaMinutiKonvertovano)))  // da li izmedju pocetka i kraja termina postoje neki zazueti slotovi(termini)
                {
                   //Console.WriteLine("USLo 1");
                   return MainWindow.PronadjiPoId(idLekara);
                }
                else
                {
                    //Console.WriteLine("USLo 2");
                    return null;
                }
            }
            else
            {
                //Console.WriteLine("USLo 3");
                return null;
            }
        }

        private Sala SlobodnaSala(int idSale)
        {
            ObservableCollection<string> SlobodnoVremePocetka = new ObservableCollection<string>()
                                                             { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00" };

            string PocetakSati;
            string PocetakMinuti;
            string KrajSati;
            string KrajMinuti;
            int TrajanjeSati;
            int TrajanjeMinuti;
            int IzbaceniSlotovi;

            Sala s = SaleMenadzer.NadjiSaluPoId(idSale);

            foreach (ZauzeceSale z in s.zauzetiTermini)
            {
                if (z.datumPocetkaTermina.Equals(datum)) // za selektovani datum gledamo zauzetost selektovanog lekara
                {
                    if (TerminMenadzer.NadjiTerminPoId(z.idTermina) != null)
                    {
                        Termin pomocna = TerminMenadzer.NadjiTerminPoId(z.idTermina);

                        PocetakSati = z.pocetakTermina.Split(':')[0];
                        PocetakMinuti = z.pocetakTermina.Split(':')[1];

                        KrajSati = z.krajTermina.Split(':')[0];
                        KrajMinuti = z.krajTermina.Split(':')[1];

                        TrajanjeSati = Convert.ToInt32(KrajSati) - Convert.ToInt32(PocetakSati);  // 0 1 2 ... koliko slotova izbacujemo
                        TrajanjeMinuti = Convert.ToInt32(KrajMinuti) - Convert.ToInt32(PocetakMinuti);  // 0 30 -30

                        if (TrajanjeMinuti == 0)
                        {
                            izbaceniSlotoviMinuti = 0;
                        }
                        else if (TrajanjeMinuti == 30)
                        {
                            izbaceniSlotoviMinuti = 1;
                        }
                        else if (TrajanjeMinuti == -30)
                        {
                            izbaceniSlotoviMinuti = -1;
                        }

                        IzbaceniSlotovi = TrajanjeSati * 2 + izbaceniSlotoviMinuti;
                        int index = SlobodnoVremePocetka.IndexOf(z.pocetakTermina);

                        if (index != -1)
                        {
                            for (int i = 0; i < IzbaceniSlotovi; i++)
                            {
                                //Zauzeto.Add(SlobodnoVremePocetka[index]);
                                SlobodnoVremePocetka.RemoveAt(index);
                            }

                            if (!TerminiZauzeto.Contains(pomocna))
                            {
                                TerminiZauzeto.Add(pomocna);
                            }
                        }

                    }
                }
            }

            // za zeljenu prostoriju imamo listu slobodnih termina za izabrani dan
            if (SlobodnoVremePocetka.Contains(VremePocetkaSatiKonvertovano + ":" + VremePocetkaMinutiKonvertovano))
            {
                int index = SlobodnoVremePocetka.IndexOf(VremePocetkaSatiKonvertovano + ":" + VremePocetkaMinutiKonvertovano);

                int noviIndex = index + trajanjeHitnogTermina;
                int index1 = SlobodnoVremePocetka.IndexOf(VremeKrajaSatiKonvertovano + ":" + VremeKrajaMinutiKonvertovano);

                if (index1 == -1)
                {
                    return null;
                }

                string drugi = SlobodnoVremePocetka[noviIndex];
                string treci = SlobodnoVremePocetka[index1];

                string[] drugiSplitovan = drugi.Split(':');
                string[] treciSplitovan = treci.Split(':');

                if (drugiSplitovan[0].Equals(treciSplitovan[0]) && drugiSplitovan[1].Equals(treciSplitovan[1]))
                //      if (SlobodnoVremePocetka.IndexOf(noviIndex.ToString()).Equals(SlobodnoVremePocetka.IndexOf(VremeKrajaSatiKonvertovano + ":" + VremeKrajaMinutiKonvertovano)))  // da li izmedju pocetka i kraja termina postoje neki zazueti slotovi(termini)
                {
                    Console.WriteLine("USLo 1");
                    return SaleMenadzer.NadjiSaluPoId(idSale);
                }
                else
                {
                    Console.WriteLine("USLo 2");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("USLo 3");
                return null;
            }
        }

        // ukoliko je vreme pocetka jednocifren broj, dodaj 0 ispred
        private void JednocifrenBrojSati()
        {
            if (VremePocetkaSati >= 0 && VremePocetkaSati <= 9)
            {
                VremePocetkaSatiKonvertovano = "0" + VremePocetkaSati.ToString();
            }
            else
            {
                VremePocetkaSatiKonvertovano = VremePocetkaSati.ToString();
            }
        }

        private void KonvertujVremePocetka()
        {
            // ukoliko je vreme pocetka u minutima 0, dodaj 0
            if (VremePocetkaMinuti == 0)
            {
                VremePocetkaMinutiKonvertovano = VremePocetkaMinuti.ToString() + "0";
            }
            else
            {
                VremePocetkaMinutiKonvertovano = VremePocetkaMinuti.ToString();
            }
        }

        private void KonvertujVremeKraja() 
        { 
            if (VremeKrajaMinuti >= 60)
            {
                VremeKrajaSati += 1;
                VremeKrajaMinuti = VremeKrajaMinuti - 60;
            }

            // ukoliko je vreme kraja u minutima 0, dodaj 0
            if (VremeKrajaMinuti == 0)
            {
                VremeKrajaMinutiKonvertovano = VremeKrajaMinuti.ToString() + "0";
            }
            else
            {
                VremeKrajaMinutiKonvertovano = VremeKrajaMinuti.ToString();
            }

            // ukoliko je vreme kraja jednocifren broj, dodaj 0 ispred
            if (VremeKrajaSati >= 0 && VremeKrajaSati <= 9)
            {
                VremeKrajaSatiKonvertovano = "0" + VremeKrajaSati.ToString();
            }
            else
            {
                VremeKrajaSatiKonvertovano = VremeKrajaSati.ToString();
            }
        }


        // odredjuje vreme pocetka i kraja termina ukoliko u prvih pola sata zakazujemo
        private void ZakaziUNarednihPolaSata() 
        {
            string trajanjeTermina = trajanje.Text;
            string[] nizTrajanja = trajanjeTermina.Split(':');
            string trajanjeSati = nizTrajanja[0];
            string trajanjeMinute = nizTrajanja[1];

            string datumVreme = DateTime.Now.ToString("MM/dd/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            string[] nizDatumVreme = datumVreme.Split(' ');
            //string datum = nizDatumVreme[0];
            string vreme = nizDatumVreme[1];

            string[] SatiMinuti = vreme.Split(':');
            string sati = SatiMinuti[0];
            string minuti = SatiMinuti[1];
            
            if (!minuti.Equals("00"))
            {
                if (Convert.ToInt32(minuti) >= 30)
                {
                    VremePocetkaSati = Convert.ToInt32(sati) + 1;
                    VremePocetkaMinuti = 0;
                }
                else
                {
                    VremePocetkaSati = Convert.ToInt32(sati);
                    VremePocetkaMinuti = 30;
                }
            }
            else
            {
                VremePocetkaSati = Convert.ToInt32(sati);
                VremePocetkaMinuti = Convert.ToInt32(minuti);
            }

            VremeKrajaSati = Convert.ToInt32(trajanjeSati) + VremePocetkaSati;
            VremeKrajaMinuti = Convert.ToInt32(trajanjeMinute) + VremePocetkaMinuti;
      
            JednocifrenBrojSati();
            KonvertujVremePocetka();
            KonvertujVremeKraja();

            // koliko slotova zauzima termin
            if (trajanjeMinute.Equals("00"))
            {
                slotoviMinuti = 0;
            }
            else if (trajanjeMinute.Equals("30"))
            {
                slotoviMinuti = 1;
            }

            trajanjeHitnogTermina = Convert.ToInt32(trajanjeSati) * 2 + slotoviMinuti;
        }

        private void Pretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource).Refresh();
        }

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

        private void ListaPacijenata_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (listaPacijenata.SelectedItem != null)
            {
                Pacijent = (Pacijent)listaPacijenata.SelectedItem;
                pacijenti.Text = Pacijent.ImePacijenta + " " + Pacijent.PrezimePacijenta;
            }
        }
    }
}
