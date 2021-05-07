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
        DateTime sledeciDan = DateTime.Now.AddDays(1);

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

        int trajanjeHitnogTermina;
        int trajanjePomerenogTermina;
        int izbaceniSlotoviMinuti;
        int slotoviMinutiT;

        ObservableCollection<Termin> TerminiZauzeto = new ObservableCollection<Termin>();
        //ObservableCollection<Termin> PotencijaloPomereniTermini = new ObservableCollection<Termin>();

        string PomocnaVremePocetkaSati = "06";
        string PomocnaVremePocetkaMinuta = "30";
        Sala novaSala;
        Lekar noviLekar;

        Termin hitanTermin;

        string noviMinuti;  // koristeno kao pomocna polja
        string noviSati;
        string pomocna;


        public HitanSlucaj()
        {
            InitializeComponent();

            this.listaPacijenata.ItemsSource = PacijentiMenadzer.pacijenti;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource);
            view.Filter = UserFilter;
        }

        // dodavanje novog termina kao hitan slucaj
        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (hitanTermin.Lekar == null)
            {
                hitanTermin.Lekar = noviLekar;
            }

            TerminMenadzer.ZakaziTerminSekretar(hitanTermin);
            ZauzeceSale z = new ZauzeceSale(hitanTermin.VremePocetka, hitanTermin.VremeKraja, datum, hitanTermin.IdTermin);

            Sala s = SaleMenadzer.NadjiSaluPoId(hitanTermin.Prostorija.Id);
            s.zauzetiTermini.Add(z);

            TerminMenadzer.sacuvajIzmene();
            SaleMenadzer.sacuvajIzmjene();

            // za svaki termin koji je zakazan u istoj prostoriji s, dodati to novo zauzece u zauzeca te prostorije
            foreach (Termin t1 in TerminMenadzer.termini)
            {
                if (t1.Prostorija.Id == s.Id)
                {
                    t1.Prostorija = s;
                }
            }
            TerminMenadzer.sacuvajIzmene();

            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // dodavanje guest pacijenta
        private void Guest_Pacijent(object sender, RoutedEventArgs e)
        {
            HitanSlucajDodajGuest dodavanje = new HitanSlucajDodajGuest(this);
            dodavanje.Show();
        }

        private void NadjiLekara()
        {
            PronadjiAdekvatneLekare();
            foreach (Lekar l in slobodniLekari)
            {
                if (SlobodanLekar(l.IdLekara) != null)
                {
                    Lekar = l;
                    break;
                }
            }
        }

        private void NadjiSalu()
        {
            PronadjiAdekvatneSale();
            foreach (Sala s in slobodneSale)
            {
                if (SlobodnaSala(s.Id) != null)
                {
                    Sala = s;
                    break;
                }
            }
        }

        // pretrazi 
        private void Pretrazi_Click(object sender, RoutedEventArgs e)
        {
            ZakaziUNarednihPolaSata();
            NadjiLekara();
            NadjiSalu();

            hitanTermin = new Termin(TerminMenadzer.GenerisanjeIdTermina(), datum, VremePocetkaSatiKonvertovano + ":" + VremePocetkaMinutiKonvertovano, VremeKrajaSatiKonvertovano + ":" + VremeKrajaMinutiKonvertovano, Tip, Lekar, Sala, Pacijent) ;

            if (Pacijent == null || Sala == null || Lekar == null)
            {
                if (hitanTermin.Lekar == null)
                {
                    Console.WriteLine("lekar je null");
                }
                if (hitanTermin.Prostorija == null)
                {
                    Console.WriteLine("sala je null");
                }
                PrikaziZauzeteTermine();
            }
            else
            {
                potvrdi.IsEnabled = true;
            }
        }

        private void Pomeri_Click(object sender, RoutedEventArgs e)
        {
            pretrazi.IsEnabled = false;
            //zauzetiTermini.IsEnabled = false;

            foreach (Termin stariTermin in zauzetiTermini.SelectedItems)
            {
                if (stariTermin == null)
                {
                    Console.WriteLine("stari termin je null");
                }

                hitanTermin.Prostorija = stariTermin.Prostorija;
                
                if (hitanTermin.Lekar == null)
                {
                    noviLekar = stariTermin.Lekar;
                }

                Termin pomereniTermin = PronadjiSledeceSlobodnoZauzece(stariTermin);  // ne nadje pomereniTermin !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                if (pomereniTermin != null)
                {
                    potvrdi.IsEnabled = true;

                    TerminMenadzer.OtkaziTerminSekretar(stariTermin);
                    TerminMenadzer.ZakaziTerminSekretar(pomereniTermin);

                    ZauzeceSale z = new ZauzeceSale(pomereniTermin.VremePocetka, pomereniTermin.VremeKraja, pomereniTermin.Datum, pomereniTermin.IdTermin);
                    Sala s = SaleMenadzer.NadjiSaluPoId(pomereniTermin.Prostorija.Id);
                    s.zauzetiTermini.Add(z);

                    // za svaki termin koji je zakazan u istoj prostoriji s, dodati to novo zauzece u zauzeca te prostorije
                    foreach (Termin t1 in TerminMenadzer.termini)
                    {
                        if (t1.Prostorija.Id == s.Id)
                        {
                            t1.Prostorija = s;
                        }
                    }

                    TerminMenadzer.sacuvajIzmene();
                    SaleMenadzer.sacuvajIzmjene();
                }
                else
                {
                    Console.WriteLine("pomereni termin je null");
                }
            }
        }

        private void PrikaziZauzeteTermine() 
        {
            pomeri.IsEnabled = true;
            zauzetiTermini.IsEnabled = true;

            List<Termin> terminiZaPomeranje = new List<Termin>();

            // filtriranje koje zauzete termine prikazujemo
            foreach (Termin t in TerminiZauzeto)
            {
                if ( (Convert.ToInt32(VremePocetkaSatiKonvertovano) <= Convert.ToInt32(t.VremePocetka.Split(':')[0]) &&
                      Convert.ToInt32(t.VremePocetka.Split(':')[0]) <= Convert.ToInt32(VremeKrajaSatiKonvertovano))   ||
                     (Convert.ToInt32(t.VremeKraja.Split(':')[0]) > Convert.ToInt32(VremePocetkaSatiKonvertovano) && 
                      Convert.ToInt32(t.VremePocetka.Split(':')[0]) <= Convert.ToInt32(VremePocetkaSatiKonvertovano)) )
                {
                    terminiZaPomeranje.Add(t);
                }
            }

            zauzetiTermini.ItemsSource = terminiZaPomeranje;
        }

        private Termin PronadjiSledeceSlobodnoZauzece(Termin termin)
        {
            int TrajanjeSati = Convert.ToInt32(termin.VremeKraja.Split(':')[0]) - Convert.ToInt32(termin.VremePocetka.Split(':')[0]);  
            int TrajanjeMinuti = Convert.ToInt32(termin.VremeKraja.Split(':')[1]) - Convert.ToInt32(termin.VremePocetka.Split(':')[1]);

            // u slucaju 1:-30
            if (TrajanjeMinuti == -30)
            {
                TrajanjeMinuti = 30;
                TrajanjeSati -= 1; 
            }

            ObservableCollection<string> SlobodnoVremePocetka;
            Termin t;

            // TODO: lista vremena pocetaka i kraja
            for (int i = 0; i < 27; i++)
            {
                OdrediVremePocetkaIKraja(TrajanjeSati, TrajanjeMinuti);

                // TODO: posebna fja
                foreach (Sala s in slobodneSale)
                {
                    SlobodnoVremePocetka = new ObservableCollection<string>()
                                                             { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00" };

                    IzbaciZauzeteTermineSale(s, SlobodnoVremePocetka);

                    if (SlobodnaSalaNaOsnovuVremena(SlobodnoVremePocetka)) // ako je slobodna, nasli smo salu, ako ne idi na sledecu salu
                    { 
                        novaSala = s;
                        t = new Termin(termin.IdTermin, sledeciDan.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture), VremePocetkaSatiKonvertovano + ":" + VremePocetkaMinutiKonvertovano, VremeKrajaSatiKonvertovano + ":" + VremeKrajaMinutiKonvertovano, termin.tipTermina, termin.Lekar, novaSala, termin.Pacijent);
                        return t;
                    }
                }
            }

            return null;
        }

        private void IzbaciZauzeteTermineSale(Sala s, ObservableCollection<string> SlobodnoVremePocetka)
        {            
            foreach (ZauzeceSale z in s.zauzetiTermini)
            {
                RenovacijaSale(z, SlobodnoVremePocetka, sledeciDan.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture));

                if (z.datumPocetkaTermina.Equals(sledeciDan.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture))) // za selektovani datum gledamo zauzetost sale
                {
                    if (TerminMenadzer.NadjiTerminPoId(z.idTermina) != null)
                    {
                        int TrajanjeSati1 = Convert.ToInt32(z.krajTermina.Split(':')[0]) - Convert.ToInt32(z.pocetakTermina.Split(':')[0]);
                        int TrajanjeMinuti1 = Convert.ToInt32(z.krajTermina.Split(':')[1]) - Convert.ToInt32(z.pocetakTermina.Split(':')[1]);

                        //  TODO - trajanje:30
                        if (TrajanjeMinuti1 == 0)
                        {
                            izbaceniSlotoviMinuti = 0;
                        }
                        else if (TrajanjeMinuti1 == 30)
                        {
                            izbaceniSlotoviMinuti = 1;
                        }
                        else if (TrajanjeMinuti1 == -30)
                        {
                            izbaceniSlotoviMinuti = -1;
                        }

                        int IzbaceniSlotovi = TrajanjeSati1 * 2 + izbaceniSlotoviMinuti;
                        int index = SlobodnoVremePocetka.IndexOf(z.pocetakTermina);

                        if (index != -1)
                        {
                            for (int i = 0; i < IzbaceniSlotovi; i++)
                            {
                                SlobodnoVremePocetka.RemoveAt(index);
                            }
                        }
                    }
                }
            }
        }

        // imamo sredjenu listu slobodnih vremena, na osnovu toga biramo salu
        private bool SlobodnaSalaNaOsnovuVremena(ObservableCollection<string> SlobodnoVremePocetka) 
        {            
            if (SlobodnoVremePocetka.Contains(VremePocetkaSatiKonvertovano + ":" + VremePocetkaMinutiKonvertovano))
            {
                int prviIndex = SlobodnoVremePocetka.IndexOf(VremePocetkaSatiKonvertovano + ":" + VremePocetkaMinutiKonvertovano);
         
                if (trajanjePomerenogTermina > 1) // opcija kad termin traje vise od pola sata
                {
                    int pretposlednjiIndex = prviIndex + trajanjePomerenogTermina - 1;

                    /*
                    if (SlobodnoVremePocetka.Count <= pretposlednjiIndex)
                    {
                        Console.WriteLine("prvi ulaz u return false");
                        return false;
                    }*/

                    if (SlobodnoVremePocetka[pretposlednjiIndex] == null)
                    {
                        return false;
                    }
                       
                    string pretposlednjiSlot = SlobodnoVremePocetka[pretposlednjiIndex];
                    
                    if (Convert.ToInt32(VremeKrajaMinutiKonvertovano) - 30 == 0)
                    {
                        noviMinuti = "00";
                        noviSati = VremePocetkaSatiKonvertovano; // sati ostaju isti ->  30 - 30 = 0
                    }
                    else if (Convert.ToInt32(VremeKrajaMinutiKonvertovano) - 30 == -30)
                    {
                        noviMinuti = "30";
                        int pomocna = Convert.ToInt32(VremeKrajaSatiKonvertovano) - 1; // sat ide 1 unazad

                        // TODOD: metoda - jednocifren broj sati
                        if (pomocna >= 0 && pomocna <= 9)
                        {
                            noviSati = "0" + pomocna.ToString();
                        }
                        else
                        {
                            noviSati = pomocna.ToString();
                        }
                    }

                    if (pretposlednjiSlot.Split(':')[0].Equals(noviSati) && pretposlednjiSlot.Split(':')[1].Equals(noviMinuti))
                    {
                        return true;
                    }
                }
                else // ako termin traje pola sata
                {
                    return true;
                }

                return false;
            }
            
            return false;
        }

        // svaki put trazi termin, za 1 slot kasnije
        private void OdrediVremePocetkaIKraja(int TrajanjeSati, int TrajanjeMinuti) // 1 0 
        {
            if (PomocnaVremePocetkaSati.Equals("20")/* || VremeKrajaSatiKonvertovano.Equals("20")*/)
            {
                sledeciDan = sledeciDan.AddDays(1);
                PomocnaVremePocetkaSati = "06";
                PomocnaVremePocetkaMinuta = "30";
            }

            if (TrajanjeMinuti == 0)
            {
                pomocna = "00";
            }
            else
            {
                pomocna = TrajanjeMinuti.ToString();
            } 

            trajanjePomerenogTermina = OdrediTrajanjeTermina(TrajanjeSati.ToString() + ":" + pomocna);
          
            VremePocetkaSati = Convert.ToInt32(PomocnaVremePocetkaSati) + 0;
            VremePocetkaMinuti = Convert.ToInt32(PomocnaVremePocetkaMinuta) + 30;
            KonvertujVremePocetka();

            PomocnaVremePocetkaSati = VremePocetkaSatiKonvertovano;
            PomocnaVremePocetkaMinuta = VremePocetkaMinutiKonvertovano;

            VremeKrajaSati = TrajanjeSati + Convert.ToInt32(VremePocetkaSatiKonvertovano);
            VremeKrajaMinuti = TrajanjeMinuti + Convert.ToInt32(VremePocetkaMinutiKonvertovano);
            KonvertujVremeKraja();

        }

        private void PronadjiAdekvatneSale()
        {
            if (tip.Text.Equals("Pregled"))
            {
                Tip = TipTermina.Pregled;

                foreach (Sala sal in SaleMenadzer.sale)
                {
                    if (sal.TipSale.Equals(tipSale.SalaZaPregled) && !sal.Namjena.Equals("Skladiste"))
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
                    if (sal.TipSale.Equals(tipSale.OperacionaSala) && !sal.Namjena.Equals("Skladiste"))
                    {
                        slobodneSale.Add(sal);
                    }
                }
            }
        }

        private void PronadjiAdekvatneLekare()
        {
            if (oblast.Text.Equals("Opsta_praksa"))
            {
                foreach (Lekar l in MainWindow.lekari)
                {
                    if (l.specijalizacija.Equals(Specijalizacija.Opsta_praksa))
                    {
                        slobodniLekari.Add(l);
                    }
                }
            }
            else if (oblast.Text.Equals("Akuserstvo"))
            {
                foreach (Lekar l in MainWindow.lekari)
                {
                    if (l.specijalizacija.Equals(Specijalizacija.Akuserstvo))
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
                                int TrajanjeSati = Convert.ToInt32(z.krajTermina.Split(':')[0]) - Convert.ToInt32(z.pocetakTermina.Split(':')[0]);  // 0 1 2 ... koliko slotova izbacujemo
                                int TrajanjeMinuti = Convert.ToInt32(z.krajTermina.Split(':')[1]) - Convert.ToInt32(z.pocetakTermina.Split(':')[1]);  // 0 30 -30

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

                                int IzbaceniSlotovi = TrajanjeSati * 2 + izbaceniSlotoviMinuti;
                                int index = SlobodnoVremePocetka.IndexOf(z.pocetakTermina);

                                if (index != -1)
                                {
                                    for (int i = 0; i < IzbaceniSlotovi; i++)
                                    {
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
                int prvi = SlobodnoVremePocetka.IndexOf(VremePocetkaSatiKonvertovano + ":" + VremePocetkaMinutiKonvertovano);               
                int noviIndex = prvi + trajanjeHitnogTermina;
                int poslednji = SlobodnoVremePocetka.IndexOf(VremeKrajaSatiKonvertovano + ":" + VremeKrajaMinutiKonvertovano);

                if (poslednji == -1)
                {
                    return null;
                }

                if(SlobodnoVremePocetka[noviIndex].Split(':')[0].Equals(SlobodnoVremePocetka[poslednji].Split(':')[0]) && SlobodnoVremePocetka[noviIndex].Split(':')[1].Equals(SlobodnoVremePocetka[poslednji].Split(':')[1]))
                {
                   return MainWindow.PronadjiPoId(idLekara);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private void RenovacijaSale(ZauzeceSale z, ObservableCollection<string> SlobodnoVremePocetka, string datum)
        {
            if (z.idTermina == 0) // ako sala ima zakazano renoviranje - TODO: oznakaZaRenovaciju == 0 
            {
                DateTime datumPocetka = DateTime.Parse(z.datumPocetkaTermina);
                DateTime datumKraja = DateTime.Parse(z.datumKrajaTermina);
                DateTime datumZakazivanja = DateTime.Parse(datum);

                if (z.datumPocetkaTermina.Equals(datum) && z.datumKrajaTermina.Equals(datum)) // isti dan i pocinje i zavrsava se
                {
                    int trajanjeSati = Convert.ToInt32(z.krajTermina.Split(':')[0]) - Convert.ToInt32(z.pocetakTermina.Split(':')[0]);
                    int trajanjeMinuti = Convert.ToInt32(z.krajTermina.Split(':')[1]) - Convert.ToInt32(z.pocetakTermina.Split(':')[1]);

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

                    int izbaceniSlotovi = trajanjeSati * 2 + izbaceniSlotoviMinuti;
                    int index = SlobodnoVremePocetka.IndexOf(z.pocetakTermina);

                    if (index != -1)
                    {
                        for (int i = 0; i < izbaceniSlotovi; i++)
                        {
                            SlobodnoVremePocetka.RemoveAt(index + 1);
                        }
                    }
                }
                else if (z.datumPocetkaTermina.Equals(datum) && !z.datumKrajaTermina.Equals(datum))
                {
                    int trajanjeSati = Convert.ToInt32("20") - Convert.ToInt32(z.pocetakTermina.Split(':')[0]);
                    int trajanjeMinuti = Convert.ToInt32("00") - Convert.ToInt32(z.pocetakTermina.Split(':')[1]);

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

                    int izbaceniSlotovi = trajanjeSati * 2 + izbaceniSlotoviMinuti;
                    int index = SlobodnoVremePocetka.IndexOf(z.pocetakTermina);

                    if (index != -1)
                    {
                        for (int i = 0; i <= izbaceniSlotovi; i++)
                        {
                            SlobodnoVremePocetka.RemoveAt(index);
                        }
                    }
                }
                else if (!z.datumPocetkaTermina.Equals(datum) && z.datumKrajaTermina.Equals(datum))
                {
                    // od pocetka dana do kraja renovacije sve izbacujemo
                    int trajanjeSati = Convert.ToInt32(z.krajTermina.Split(':')[0]) - Convert.ToInt32("07");
                    int trajanjeMinuti = Convert.ToInt32(z.krajTermina.Split(':')[1]) - Convert.ToInt32("00");

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

                    int izbaceniSlotovi = trajanjeSati * 2 + izbaceniSlotoviMinuti;

                    for (int i = 0; i <= izbaceniSlotovi; i++)
                    {
                        if (i != izbaceniSlotovi)
                        {
                            if (SlobodnoVremePocetka.Count > 0)
                            {
                                SlobodnoVremePocetka.RemoveAt(0);
                            }
                        }
                    }
                }
                else if (datumZakazivanja < datumKraja && datumZakazivanja > datumPocetka) // ako je trenutni datum izmedju vremena pocetka i vremena kraja renovacije
                {
                    SlobodnoVremePocetka.Clear();
                    MessageBox.Show("Prostorija se renovira u to vreme!");
                }
            }
        }

        private Sala SlobodnaSala(int idSale)
        {
            ObservableCollection<string> SlobodnoVremePocetka = new ObservableCollection<string>()
                                                             { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00" };

            Sala s = SaleMenadzer.NadjiSaluPoId(idSale);

            foreach (ZauzeceSale z in s.zauzetiTermini)
            {
                RenovacijaSale(z, SlobodnoVremePocetka, datum);

                if (z.datumPocetkaTermina.Equals(datum)) // za selektovani datum gledamo zauzetost selektovanog lekara
                {
                    if (TerminMenadzer.NadjiTerminPoId(z.idTermina) != null)
                    {
                        Termin pomocna = TerminMenadzer.NadjiTerminPoId(z.idTermina);

                        int TrajanjeSati = Convert.ToInt32(z.krajTermina.Split(':')[0]) - Convert.ToInt32(z.pocetakTermina.Split(':')[0]);  // 0 1 2 ... koliko slotova izbacujemo
                        int TrajanjeMinuti = Convert.ToInt32(z.krajTermina.Split(':')[1]) - Convert.ToInt32(z.pocetakTermina.Split(':')[1]);  // 0 30 -30

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

                        int IzbaceniSlotovi = TrajanjeSati * 2 + izbaceniSlotoviMinuti;
                        int index = SlobodnoVremePocetka.IndexOf(z.pocetakTermina);

                        if (index != -1)
                        {
                            for (int i = 0; i < IzbaceniSlotovi; i++)
                            {
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
                {
                    return SaleMenadzer.NadjiSaluPoId(idSale);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        // ukoliko je vreme pocetka jednocifren broj, dodaj 0 ispred
        private void JednocifrenoVremePocetkaSati()
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

        private void JednocifrenoVremeKrajaSati()
        {
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

        private void KonvertujVremePocetka()
        {
            if (VremePocetkaMinuti >= 60)
            {
                VremePocetkaSati += 1;
                VremePocetkaMinuti -= 60;
            }

            // ukoliko je vreme pocetka u minutima 0, dodaj 0
            if (VremePocetkaMinuti == 0)
            {
                VremePocetkaMinutiKonvertovano = VremePocetkaMinuti.ToString() + "0";
            }
            else
            {
                VremePocetkaMinutiKonvertovano = VremePocetkaMinuti.ToString();
            }

            JednocifrenoVremePocetkaSati();
        }

        private void KonvertujVremeKraja()
        {
            if (VremeKrajaMinuti >= 60)
            {
                VremeKrajaSati += 1;
                VremeKrajaMinuti -= 60;
            }

            if (VremeKrajaMinuti == -30)
            {
                VremeKrajaSati -= 1;
                VremeKrajaMinuti = 30;
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

            JednocifrenoVremeKrajaSati();
        }

        private void OdrediVremePocetka(string sati, string minuti)
        {
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
        }

        private void OdrediVremeKraja(string trajanjeTermina) 
        {
            VremeKrajaSati = Convert.ToInt32(trajanjeTermina.Split(':')[0]) + VremePocetkaSati;
            VremeKrajaMinuti = Convert.ToInt32(trajanjeTermina.Split(':')[1]) + VremePocetkaMinuti;
        }

        // odredjuje vreme pocetka i kraja termina ukoliko u prvih pola sata zakazujemo
        private void ZakaziUNarednihPolaSata() 
        {
            string trajanjeTermina = trajanje.Text;

            string datumVreme = DateTime.Now.ToString("MM/dd/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            string[] nizDatumVreme = datumVreme.Split(' ');
            string vreme = nizDatumVreme[1];

            string[] SatiMinuti = vreme.Split(':');
            string sati = SatiMinuti[0];
            string minuti = SatiMinuti[1];

            OdrediVremePocetka(sati, minuti);
            OdrediVremeKraja(trajanjeTermina);

            KonvertujVremePocetka();
            KonvertujVremeKraja();

            trajanjeHitnogTermina = OdrediTrajanjeTermina(trajanjeTermina);
            //TrajanjeHitnogTermina(trajanjeTermina);
        }

        // koliko slotova zauzima termin
        private void TrajanjeHitnogTermina(string trajanjeTermina) 
        {
            int slotoviMinuti;

            if (trajanjeTermina.Split(':')[1].Equals("00"))
            {
                slotoviMinuti = 0;
                trajanjeHitnogTermina = Convert.ToInt32(trajanjeTermina.Split(':')[0]) * 2 + slotoviMinuti;
            }
            else if (trajanjeTermina.Split(':')[1].Equals("30"))
            {
                slotoviMinuti = 1;
                trajanjeHitnogTermina = Convert.ToInt32(trajanjeTermina.Split(':')[0]) * 2 + slotoviMinuti;
            }
        }

        private void TrajanjePomerenogTermina(string trajanjeTermina)
        {
            int slotoviMinuti;

            if (trajanjeTermina.Split(':')[1].Equals("00"))
            {
                slotoviMinuti = 0;
                trajanjePomerenogTermina = Convert.ToInt32(trajanjeTermina.Split(':')[0]) * 2 + slotoviMinuti;
                Console.WriteLine("ooooooooooooooooooooooo");
            }
            else if (trajanjeTermina.Split(':')[1].Equals("30"))
            {
                Console.WriteLine("mmmmmmmmmmmmmmmmm");
                slotoviMinuti = 1;
                trajanjePomerenogTermina = Convert.ToInt32(trajanjeTermina.Split(':')[0]) * 2 + slotoviMinuti;
            }
        } 

        private int OdrediTrajanjeTermina(string trajanjeTermina)
        {
            //int slotoviMinutiT;

            if (trajanjeTermina.Split(':')[1].Equals("00"))
            {
                Console.WriteLine("mmmmmmmmmmmmmmmmm");
                slotoviMinutiT = 0;
            }
            else if (trajanjeTermina.Split(':')[1].Equals("0"))
            {
                Console.WriteLine("Ovdeee udjeeeeeeeeeeeeeee");
                slotoviMinutiT = 0;
            }
            else if (trajanjeTermina.Split(':')[1].Equals("30"))
            {
                slotoviMinutiT = 1;
            }

            int trajanje = Convert.ToInt32(trajanjeTermina.Split(':')[0]) * 2 + slotoviMinutiT;

            return trajanje;
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

        private void Pretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource).Refresh();
        }

        // azuriranje nakon dodavanja guest pacijenta
        public void AzurirajListuPacijenata()
        {
            foreach (Pacijent pacijent in PacijentiMenadzer.pacijenti)
            {
                AzuriranaLista.Add(pacijent);
            }

            listaPacijenata.ItemsSource = AzuriranaLista;
            listaPacijenata.SelectedIndex = AzuriranaLista.Count - 1;
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
