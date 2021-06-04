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
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for HitanSlucaj.xaml
    /// </summary>
    public partial class HitanSlucaj : Window
    {
        public const int oznakaZaRenovaciju = 0;
        public const int polaSata = 1;
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
        int slotoviMinuti;

        ObservableCollection<Termin> TerminiZauzeto = new ObservableCollection<Termin>();
        ObservableCollection<string> VremenskiSlotovi = new ObservableCollection<string>();

        string GeneratorSati = "06";
        string GenratorMinuta = "30";
        Sala novaSala;
        Lekar noviLekar;
        Termin hitanTermin;

        string noviMinuti;
        string noviSati;

        public HitanSlucaj()
        {
            InitializeComponent();

            List<Pacijent> pacijentiLista = PacijentiServis.PronadjiSve();
            listaPacijenata.ItemsSource = pacijentiLista;
            CollectionView prikazPacijenata = (CollectionView)CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource);
            prikazPacijenata.Filter = PretragaPacijenta;

            oblastLekara.ItemsSource = Enum.GetValues(typeof(Specijalizacija)).Cast<Specijalizacija>();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            DodeliLekaraZaHitanTermin();
            TerminiSekretarServis.ZakaziHitanTermin(hitanTermin, datum);
            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Guest_Pacijent(object sender, RoutedEventArgs e)
        {
            HitanSlucajDodajGuest dodavanjeGuestPacijenta = new HitanSlucajDodajGuest(this);
            dodavanjeGuestPacijenta.Show();
        }

        private void DodeliLekaraZaHitanTermin()
        {
            if (hitanTermin.Lekar == null)
            {
                hitanTermin.Lekar = noviLekar;
            }
        }

        private void OdrediLekara()
        {
            OdrediOblastLekara();
            foreach (Lekar selektovaniLekar in slobodniLekari)
            {
                if (SlobodanLekar(selektovaniLekar.IdLekara) != null)
                {
                    Lekar = selektovaniLekar;
                    break;
                }
            }
        }

        private void OdrediSalu()
        {
            PronadjiAdekvatneSale();
            foreach (Sala selektovanaSala in slobodneSale)
            {
                if (SlobodnaSala(selektovanaSala.Id) != null)
                {
                    Sala = selektovanaSala;
                    break;
                }
            }
        }

        private Termin KreirajHitanTermin()
        {
            return new Termin(TerminiSekretarServis.GenerisanjeIdTermina(), datum, VremePocetka(), VremeKraja(), Tip, Lekar, Sala, Pacijent);
        }

        private void Pretrazi_Click(object sender, RoutedEventArgs e)
        {
            PronadjiTerminUNarednihPolaSata();
            OdrediLekara();
            OdrediSalu();
            hitanTermin = KreirajHitanTermin();

            if (Pacijent == null || Sala == null || Lekar == null)
            {
                PrikaziZauzeteTermine();
            }
            else
            {
                potvrdiDugme.IsEnabled = true;
            }
        }

        private void PomeriZakazaniTermin()
        {
            foreach (Termin stariTermin in zauzetiTermini.SelectedItems)
            {
                hitanTermin.Prostorija = stariTermin.Prostorija;
                noviLekar = stariTermin.Lekar;

                Termin pomereniTermin = PronadjiSledeceSlobodnoZauzece(stariTermin);
                TerminiSekretarServis.OtkaziTerminSekretar(stariTermin);
                TerminiSekretarServis.ZakaziHitanTermin(pomereniTermin, pomereniTermin.Datum);
                potvrdiDugme.IsEnabled = true;               
            }
        }

        private void Pomeri_Click(object sender, RoutedEventArgs e)
        {
            pretraziDugme.IsEnabled = false;
            PomeriZakazaniTermin();
        }

        private void PrikaziZauzeteTermine()
        {
            pomeriDugme.IsEnabled = true;
            zauzetiTermini.IsEnabled = true;

            List<Termin> terminiZaPomeranje = FiltrirajPrikazaneTermine();
            zauzetiTermini.ItemsSource = terminiZaPomeranje;

            if (terminiZaPomeranje.Count == 0  && Lekar == null)
            {
                MessageBox.Show("Ne postoje lekari izabrane specijalizacije!");
                pomeriDugme.IsEnabled = false;
                pretraziDugme.IsEnabled = false;
            }
        }

        private string Sati(string Vreme)
        {
            return Vreme.Split(':')[0];
        }

        private string Minuti(string Vreme)
        {
            return Vreme.Split(':')[1];
        }

        private List<Termin> FiltrirajPrikazaneTermine()
        {
            List<Termin> filtriraniTermini = new List<Termin>();

            foreach (Termin t in TerminiZauzeto)
            {
                if ((Convert.ToInt32(VremePocetkaSatiKonvertovano) <= Convert.ToInt32(Sati(t.VremePocetka)) &&
                 Convert.ToInt32(Sati(t.VremePocetka)) <= Convert.ToInt32(VremeKrajaSatiKonvertovano)) ||
                (Convert.ToInt32(Sati(t.VremeKraja)) > Convert.ToInt32(VremePocetkaSatiKonvertovano) &&
                 Convert.ToInt32(Sati(t.VremePocetka)) <= Convert.ToInt32(VremePocetkaSatiKonvertovano)))
                {
                    filtriraniTermini.Add(t);
                }
            }

            return filtriraniTermini;
        }

        private int[] TrajanjeUSatimaIMinutima(Termin termin)
        {
            int[] Trajanje = new int[2];

            int TrajanjeSati = Convert.ToInt32(Sati(termin.VremeKraja)) - Convert.ToInt32(Sati(termin.VremePocetka));
            int TrajanjeMinuti = Convert.ToInt32(Minuti(termin.VremeKraja)) - Convert.ToInt32(Minuti(termin.VremePocetka));
            if (TrajanjeMinuti == -30)
            {
                TrajanjeMinuti = 30;
                TrajanjeSati -= 1;
            }

            Trajanje[0] = TrajanjeSati;
            Trajanje[1] = TrajanjeMinuti;
            
            return Trajanje;
        }

        private Termin PronadjiSledeceSlobodnoZauzece(Termin terminKojiPomeramo)
        {
            ObservableCollection<string> SlobodnoVremePocetka = new ObservableCollection<string>();

            int[] Trajanje = TrajanjeUSatimaIMinutima(terminKojiPomeramo);
            int TrajanjeSati = Trajanje[0];
            int TrajanjeMinuti = Trajanje[1]; 
            Termin pronadjeniTermin = PronadjiSlobodanTermin(terminKojiPomeramo, SlobodnoVremePocetka, TrajanjeSati, TrajanjeMinuti);

            return pronadjeniTermin;
        }

        private Termin PronadjiSlobodanTermin(Termin termin, ObservableCollection<string> SlobodnoVremePocetka, int TrajanjeSati, int TrajanjeMinuti)
        {
            VremenskiSlotovi = InicijalizujVremenskeSlotove();
            foreach (string vremeskiSlot in VremenskiSlotovi)
            {
                GenerisiVremePocetkaIKraja(TrajanjeSati, TrajanjeMinuti);

                foreach (Sala sala in slobodneSale)
                {
                    SlobodnoVremePocetka = InicijalizujListuTermina();
                    IzbaciZauzeteTermineSale(sala, SlobodnoVremePocetka);

                    if (SlobodnaSalaNaOsnovuVremena(SlobodnoVremePocetka))
                    {
                        novaSala = sala ;
                        Termin noviTermin = new Termin(termin.IdTermin, KonvertovaniDatum(), VremePocetka(), VremeKraja(), termin.tipTermina, termin.Lekar, novaSala, termin.Pacijent);
                        return noviTermin;
                    }
                }
            }            
            return null;
        }

        private string KonvertovaniDatum() 
        {
            return sledeciDan.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }

        private void IzbaciZauzeteTermineSale(Sala s, ObservableCollection<string> SlobodnoVremePocetka)
        {
            foreach (ZauzeceSale zauzece in s.zauzetiTermini)
            {
                RenovacijaSale(zauzece, SlobodnoVremePocetka, KonvertovaniDatum());   
                IzbaciZauzeteTerminaZaDatum(zauzece, SlobodnoVremePocetka);
            }
        }

        private void IzbaciZauzeteTerminaZaDatum(ZauzeceSale zauzece, ObservableCollection<string> SlobodnoVremePocetka)
        {
            if (zauzece.datumPocetkaTermina.Equals(KonvertovaniDatum()))
            {
                if (zauzece.idTermina == oznakaZaRenovaciju)
                {
                    return;
                }

                Termin pomocna = TerminiSekretarServis.NadjiTerminPoId(zauzece.idTermina);
                if (pomocna == null)
                {
                    return;
                }
                
                int IzbaceniSlotovi = OdrediBrojSlotovaZaIzbacivanje(zauzece.pocetakTermina, zauzece.krajTermina);
                int indeksPocetkaTermina = SlobodnoVremePocetka.IndexOf(zauzece.pocetakTermina);
                if (indeksPocetkaTermina == -1)
                {
                    return;
                }
                
                SlobodnoVremePocetka = AzurirajListuSlobodnihTermina(IzbaceniSlotovi, indeksPocetkaTermina, SlobodnoVremePocetka);
            }
        }

        private ObservableCollection<string> AzurirajListuSlobodnihTermina(int izbaceniSlotovi, int indeksPocetkaTermina, ObservableCollection<string> SlobodnoVremePocetka)
        {
            for (int i = 0; i < izbaceniSlotovi; i++)
            {
                SlobodnoVremePocetka.RemoveAt(indeksPocetkaTermina);
            }
            return SlobodnoVremePocetka;
        }

        private void OdrediVremeZaPretposlednjiSlot()
        {
            if (Convert.ToInt32(VremeKrajaMinutiKonvertovano) - 30 == 0)
            {
                noviMinuti = "00";
                noviSati = VremePocetkaSatiKonvertovano;
            }
            else if (Convert.ToInt32(VremeKrajaMinutiKonvertovano) - 30 == -30)
            {
                noviMinuti = "30";
                int Sati = Convert.ToInt32(VremeKrajaSatiKonvertovano) - 1;
                noviSati = JednocifreniSati(Sati);
            }
        }

        private string VremePocetka()
        {
            return VremePocetkaSatiKonvertovano + ":" + VremePocetkaMinutiKonvertovano;
        }

        private string VremeKraja()
        {
            return VremeKrajaSatiKonvertovano + ":" + VremeKrajaMinutiKonvertovano; 
        }

        private bool SlobodnaSalaNaOsnovuVremena(ObservableCollection<string> SlobodnoVremePocetka)
        {
            if (SlobodnoVremePocetka.Contains(VremePocetka()) && trajanjePomerenogTermina > polaSata)
            {
                int indeksPocetkaTermina = SlobodnoVremePocetka.IndexOf(VremePocetka());
                int pretposlednjiIndeks = indeksPocetkaTermina + trajanjePomerenogTermina - 1;

                string pretposlednjiSlot = SlobodnoVremePocetka[pretposlednjiIndeks];
                OdrediVremeZaPretposlednjiSlot();

                if (Sati(pretposlednjiSlot).Equals(noviSati) && Minuti(pretposlednjiSlot).Equals(noviMinuti))
                {
                    return true;
                }
            }
            else if (SlobodnoVremePocetka.Contains(VremePocetka()) && trajanjePomerenogTermina == polaSata)
            {
                return true;
            }

            return false;
        }

        private void PredjiNaSledeciDan()
        {
            if (GeneratorSati.Equals("20"))
            {
                sledeciDan = sledeciDan.AddDays(1);
                GeneratorSati = "06";
                GenratorMinuta = "30";
            }
        }

        private void GenerisiVremePocetkaIKraja(int TrajanjeSati, int TrajanjeMinuti)
        {
            PredjiNaSledeciDan();
            trajanjePomerenogTermina = OdrediTrajanjeTermina(TrajanjeSati.ToString() + ":" + JednocifreniMinuti(TrajanjeMinuti));

            VremePocetkaSati = Convert.ToInt32(GeneratorSati) + 0;
            VremePocetkaMinuti = Convert.ToInt32(GenratorMinuta) + 30;
            KonvertujVremePocetka();

            VremeKrajaSati = TrajanjeSati + Convert.ToInt32(VremePocetkaSatiKonvertovano);
            VremeKrajaMinuti = TrajanjeMinuti + Convert.ToInt32(VremePocetkaMinutiKonvertovano);
            KonvertujVremeKraja();

            GeneratorSati = VremePocetkaSatiKonvertovano;
            GenratorMinuta = VremePocetkaMinutiKonvertovano;
        }

        private void PronadjiAdekvatneSale()
        {
            if (tipTermina.Text.Equals("Pregled"))
            {
                Tip = TipTermina.Pregled;
                slobodneSale = SaleServis.PronadjiSaleZaPregled();
            }
            else if (tipTermina.Text.Equals("Operacija"))
            {
                Tip = TipTermina.Operacija;
                slobodneSale = SaleServis.PronadjiSaleZaOperaciju();
            }
        }

        private void OdrediOblastLekara()
        {
            Specijalizacija oblastSpecijalizacije = (Specijalizacija)oblastLekara.SelectedItem;
            slobodniLekari = LekariServis.PronadjiLekarePoSpecijalizaciji(oblastSpecijalizacije);
        }

        private int OdrediBrojSlotovaZaIzbacivanje(string pocetakTermina, string krajTermina)
        {
            int trajanjeSati = Convert.ToInt32(Sati(krajTermina)) - Convert.ToInt32(Sati(pocetakTermina));
            int trajanjeMinuti = Convert.ToInt32(Minuti(krajTermina)) - Convert.ToInt32(Minuti(pocetakTermina));
            int slotoviMinuti = trajanjeMinuti / 30;

            return trajanjeSati * 2 + slotoviMinuti;
        }

        private void IzbaciZauzeteTermine(ZauzeceSale z, Termin pomocna, ObservableCollection<string> SlobodnoVremePocetka)
        { 
            int IzbaceniSlotovi = OdrediBrojSlotovaZaIzbacivanje(z.pocetakTermina, z.krajTermina);
            int indeksPocetkaTermina = SlobodnoVremePocetka.IndexOf(z.pocetakTermina);

            if (indeksPocetkaTermina == -1)
            {
                return;
            }

            SlobodnoVremePocetka = AzurirajListuSlobodnihTermina(IzbaceniSlotovi, indeksPocetkaTermina, SlobodnoVremePocetka);
            TerminiZauzeto = AzurirajListuZauzetihTermina(pomocna);
        }

        private ObservableCollection<Termin> AzurirajListuZauzetihTermina(Termin pomocna)
        {
            if (!TerminiZauzeto.Contains(pomocna))
            {
                TerminiZauzeto.Add(pomocna);
            }
            return TerminiZauzeto;
        }

        private ObservableCollection<string> InicijalizujListuTermina()
        {
            ObservableCollection<string> SlobodnoVremePocetka = new ObservableCollection<string>()
                                                             { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00" };
            return SlobodnoVremePocetka;
        }

        private ObservableCollection<string> InicijalizujVremenskeSlotove()
        {
            VremenskiSlotovi = new ObservableCollection<string>()
                                                             { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30","17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00" };
            return VremenskiSlotovi;
        }

        private Object PronadjiOdgovarajuciObjekat(ObservableCollection<string> SlobodnoVremePocetka, int idLekara, int idSale)
        {
            if (SlobodnoVremePocetka.Contains(VremePocetka()))
            {
                int indeksPocetkaTermina = SlobodnoVremePocetka.IndexOf(VremePocetka());
                int noviIndex = indeksPocetkaTermina + trajanjeHitnogTermina;
                int poslednji = SlobodnoVremePocetka.IndexOf(VremeKraja()); 
                if (poslednji == -1)
                {
                    return null;
                }

                if (Sati(SlobodnoVremePocetka[noviIndex]).Equals(Sati(SlobodnoVremePocetka[poslednji])) && 
                    Minuti(SlobodnoVremePocetka[noviIndex]).Equals(Minuti(SlobodnoVremePocetka[poslednji])) )
                {
                    if (idLekara != 0)
                    {
                        return LekariServis.NadjiPoId(idLekara);
                    }
                    else if (idSale != 0)
                    { 
                        return SaleServis.NadjiSaluPoId(idSale);
                    }
                }
                return null;
            }

            return null;
        }

        private bool LekarNijeNaGodisnjemOdmoru(int idLekara)
        {
            List<Lekar> lekari = LekariServis.NadjiSveLekare();
            foreach (Lekar lekar in lekari)
            { 
                if (lekar.IdLekara == idLekara)
                {
                    foreach (RadniDan dan in lekar.RadniDani)
                    {
                        DateTime parsiraniDatum = DateTime.Parse(dan.Datum);
                        if (DateTime.Parse(datum) == parsiraniDatum)
                        {
                            if (dan.NaGodisnjemOdmoru == false)
                            {
                                return true;
                            }
                        }
                    }

                }
            }
            return false;
        }

        private Lekar SlobodanLekar(int idLekara)
        {
            if (!LekarNijeNaGodisnjemOdmoru(idLekara))
            {
                return null;
            }

            ObservableCollection<string> SlobodnoVremePocetka = InicijalizujListuTermina();
            List<Sala> sale = SaleServis.NadjiSveSale();
            foreach (Sala sala in sale)
            {
                foreach (ZauzeceSale zauzece in sala.zauzetiTermini)
                {
                    if (zauzece.datumPocetkaTermina.Equals(datum))
                    {
                        if (zauzece.idTermina == oznakaZaRenovaciju)
                        {
                            continue;                            
                        }

                        Termin pomocna = TerminiSekretarServis.NadjiTerminPoId(zauzece.idTermina);
                        if (pomocna == null)
                        {
                            return null;
                        }

                        if (pomocna.Lekar.IdLekara == idLekara)
                        {
                            IzbaciZauzeteTermine(zauzece, pomocna, SlobodnoVremePocetka);
                        }
                    }
                }
            }

            return (Lekar)PronadjiOdgovarajuciObjekat(SlobodnoVremePocetka, idLekara, 0);
        }

        private Sala SlobodnaSala(int idSale)
        {
            ObservableCollection<string> SlobodnoVremePocetka = InicijalizujListuTermina();

            Sala sala = SaleServis.NadjiSaluPoId(idSale);
            foreach (ZauzeceSale zauzece in sala.zauzetiTermini)
            {
                RenovacijaSale(zauzece, SlobodnoVremePocetka, datum);

                if (zauzece.datumPocetkaTermina.Equals(datum))
                {
                    if (zauzece.idTermina == oznakaZaRenovaciju)
                    {
                        continue;
                    }

                    Termin pomocna = TerminiSekretarServis.NadjiTerminPoId(zauzece.idTermina);
                    if (pomocna == null)
                    {
                        return null;
                    }

                    IzbaciZauzeteTermine(zauzece, pomocna, SlobodnoVremePocetka);
                }
            }

            return (Sala)PronadjiOdgovarajuciObjekat(SlobodnoVremePocetka, 0, idSale);
        }

        private void IzbaciTermineTokomRenovacije(int izbaceniSlotovi, int indeksPocetkaTermina, int indeksPocetkaTerminaPomocna ,ObservableCollection<string> SlobodnoVremePocetka)
        {
            if (indeksPocetkaTermina != -1)
            {
                for (int i = 0; i < izbaceniSlotovi; i++)
                {
                    if (SlobodnoVremePocetka.Count > 0)
                    {
                        SlobodnoVremePocetka.RemoveAt(indeksPocetkaTerminaPomocna);
                    }
                }
            }
        }

        private void RenovacijaSale(ZauzeceSale zauzece, ObservableCollection<string> SlobodnoVremePocetka, string trenutniDatum)
        {
            if (zauzece.idTermina == oznakaZaRenovaciju) 
            {
                DateTime datumPocetka = DateTime.Parse(zauzece.datumPocetkaTermina);
                DateTime datumKraja = DateTime.Parse(zauzece.datumKrajaTermina);
                DateTime datumZakazivanja = DateTime.Parse(trenutniDatum);

                if (zauzece.datumPocetkaTermina.Equals(trenutniDatum) && zauzece.datumKrajaTermina.Equals(trenutniDatum))  
                {
                    int izbaceniSlotovi = OdrediBrojSlotovaZaIzbacivanje(zauzece.pocetakTermina, zauzece.krajTermina);   
                    int indeksPocetkaTermina = SlobodnoVremePocetka.IndexOf(zauzece.pocetakTermina);
                    IzbaciTermineTokomRenovacije(izbaceniSlotovi, indeksPocetkaTermina, indeksPocetkaTermina + 1, SlobodnoVremePocetka);
                }
                else if (zauzece.datumPocetkaTermina.Equals(trenutniDatum) && !zauzece.datumKrajaTermina.Equals(trenutniDatum))
                {   
                    int izbaceniSlotovi = OdrediBrojSlotovaZaIzbacivanje(zauzece.pocetakTermina, "20:00");      
                    int indeksPocetkaTermina = SlobodnoVremePocetka.IndexOf(zauzece.pocetakTermina);
                    IzbaciTermineTokomRenovacije(izbaceniSlotovi + 1, indeksPocetkaTermina, indeksPocetkaTermina, SlobodnoVremePocetka);
                }
                else if (!zauzece.datumPocetkaTermina.Equals(trenutniDatum) && zauzece.datumKrajaTermina.Equals(trenutniDatum))  
                {
                    int izbaceniSlotovi = OdrediBrojSlotovaZaIzbacivanje("07:00", zauzece.krajTermina);  
                    IzbaciTermineTokomRenovacije(izbaceniSlotovi, 0, 0, SlobodnoVremePocetka);
                }
                else if (datumZakazivanja < datumKraja && datumZakazivanja > datumPocetka) 
                {
                    SlobodnoVremePocetka.Clear();
                }
            }
        }

        private string JednocifreniSati(int Sati)
        {
            string SatiKonvertovano;
            if (Sati >= 0 && Sati <= 9)
            {
                SatiKonvertovano = "0" + Sati.ToString();
            }
            else
            {
                SatiKonvertovano = Sati.ToString();
            }

            return SatiKonvertovano;
        }

        private string JednocifreniMinuti(int Minuti)
        {
            string MinutiKonvertovano;
            if (Minuti == 0)
            {
                MinutiKonvertovano = Minuti.ToString() + "0";
            }
            else
            {
                MinutiKonvertovano = Minuti.ToString();
            }

            return MinutiKonvertovano;
        }

        private void KonvertujVremePocetka()
        {
            if (VremePocetkaMinuti >= 60)
            {
                VremePocetkaSati += 1;
                VremePocetkaMinuti -= 60;
            }
          
            VremePocetkaMinutiKonvertovano = JednocifreniMinuti(VremePocetkaMinuti);
            VremePocetkaSatiKonvertovano = JednocifreniSati(VremePocetkaSati);
        }

        private void KonvertujVremeKraja()
        {
            if (VremeKrajaMinuti >= 60)
            {
                VremeKrajaSati += 1;
                VremeKrajaMinuti -= 60;
            }
            else if (VremeKrajaMinuti == -30)
            {
                VremeKrajaSati -= 1;
                VremeKrajaMinuti = 30;
            }

            VremeKrajaMinutiKonvertovano = JednocifreniMinuti(VremeKrajaMinuti);
            VremeKrajaSatiKonvertovano = JednocifreniSati(VremeKrajaSati);
        }

        private void OdrediVremePocetka(string sati, string minuti)
        {
            if (minuti.Equals("00"))
            {
                VremePocetkaSati = Convert.ToInt32(sati);
                VremePocetkaMinuti = Convert.ToInt32(minuti);
            }
            else if (Convert.ToInt32(minuti) >= 30)
            {
                VremePocetkaSati = Convert.ToInt32(sati) + 1;
                VremePocetkaMinuti = 0;
            }
            else 
            {
                VremePocetkaSati = Convert.ToInt32(sati);
                VremePocetkaMinuti = 30;
            }
            KonvertujVremePocetka();
        }

        private void OdrediVremeKraja(string trajanjeTermina)
        {
            VremeKrajaSati = Convert.ToInt32(Sati(trajanjeTermina)) + VremePocetkaSati;
            VremeKrajaMinuti = Convert.ToInt32(Minuti(trajanjeTermina)) + VremePocetkaMinuti;
            KonvertujVremeKraja();
        }

        private string KonvertujTrenutnoVreme()
        {
            return DateTime.Now.ToString("HH:mm", System.Globalization.CultureInfo.InvariantCulture);
        }

        private void PronadjiTerminUNarednihPolaSata()
        {
            string trajanjeTermina = trajanje.Text;
            string vreme = KonvertujTrenutnoVreme();
            OdrediVremePocetka(Sati(vreme), Minuti(vreme));
            OdrediVremeKraja(trajanjeTermina);
            trajanjeHitnogTermina = OdrediTrajanjeTermina(trajanjeTermina);
        }

        private int OdrediTrajanjeTermina(string trajanjeTermina)
        {
            if (Minuti(trajanjeTermina).Equals("00"))
            {
                slotoviMinuti = 0;
            }
            else if (Minuti(trajanjeTermina).Equals("30"))
            {
                slotoviMinuti = 1;
            }

            return Convert.ToInt32(Sati(trajanjeTermina)) * 2 + slotoviMinuti;
        }

        private bool PretragaPacijenta(object item)
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

        public void AzurirajListuPacijenata()
        {
            List<Pacijent> pacijentiLista = PacijentiServis.PronadjiSve();
            foreach (Pacijent pacijent in pacijentiLista)
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.G && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Guest_Pacijent(sender, e);
            }
            else if (e.Key == Key.G && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Guest_Pacijent(sender, e);
            }
        }

        private void ZauzetiTermini_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
    }
}