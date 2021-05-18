using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Projekat.Servis
{
    
    public class TerminServis
    {

        #region Zakazi termin
        private static int maksimalniJednocifren = 9;
        private static bool selektovanUput = false;
        private static Pacijent prijavljeniPacijent;
        private static int idPacijent;
        private static int oznakaZaRenoviranje = 0;
        private static ObservableCollection<string> SviSlobodniSlotovi { get; set; }
        private static List<string> SviZauzetiZaSelektovaniDatum { get; set; }
        private static ObservableCollection<string> PomocnaSviSlobodniSlotovi { get; set; }
        private static List<Sala> SaleZaPreglede;
        private static int ukupanBrojSalaZaPregled;
        private static Sala prvaSlobodnaSala;


        public static string IzracunajVremeKrajaPregleda(string vp)
        {
            string hh = vp.Substring(0, 2);
            string mm = vp.Substring(3);
            if (mm == "30")
            {
                int jednocifrenSat = int.Parse(hh);
                jednocifrenSat++;
                if (jednocifrenSat <= maksimalniJednocifren)
                {
                    return "0" + jednocifrenSat.ToString() + ":00";
                }
                else
                {
                    return jednocifrenSat.ToString() + ":00";
                }
            }
            else
            {
                return hh + ":30";
            }
        }

        public static int ParsirajSateVremenskogSlota(String vreme)
        {
            String sat = vreme.Split(':')[0];
            return Convert.ToInt32(sat);
        }

        public static int ParsirajMinuteVremenskogSlota(String vreme)
        {
            String minuti = vreme.Split(':')[1];
            return Convert.ToInt32(minuti);
        }

        public static string FormatirajSelektovaniDatum(DateTime selektovaniDatum)
        {
            return selektovaniDatum.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }
        public static List<Sala> combo_SelectionChanged(ComboBox combo, ComboBox comboUputi, Hyperlink preferenca, int idPrijavljenogPacijenta)
        { 
            idPacijent = idPrijavljenogPacijenta;
            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);

            SaleZaPreglede = SaleMenadzer.PronadjiSaleZaPregled();
            ukupanBrojSalaZaPregled = SaleZaPreglede.Count();
            if (combo.Text.Equals("Pregled"))
            {
                comboUputi.IsEnabled = true;
                preferenca.IsEnabled = false;
                selektovanUput = true;
            }
            else if (combo.Text.Equals("Specijalistički pregled"))
            {
                comboUputi.IsEnabled = false;
                preferenca.IsEnabled = true;
                selektovanUput = false;
            }
            return SaleZaPreglede;
        }

        public static ObservableCollection<string> datum_SelectedDatesChanged(Calendar datum)
        {
            string selektovaniDatum = FormatirajSelektovaniDatum(datum.SelectedDate.Value);
            SviSlobodniSlotovi = SaleMenadzer.InicijalizujSveSlotove();
            PomocnaSviSlobodniSlotovi = SaleMenadzer.InicijalizujSveSlotove();
            SviSlobodniSlotovi = UkloniProsleSlotoveZaDanasnjiDatum(SviSlobodniSlotovi, PomocnaSviSlobodniSlotovi, datum);
            SviSlobodniSlotovi = UkloniZauzecaPacijentaZaSelektovaniDatum(SviSlobodniSlotovi, selektovaniDatum, PomocnaSviSlobodniSlotovi);
            SviSlobodniSlotovi = UkolniSlotoveZauzeteUSvimSalama(SviSlobodniSlotovi, PomocnaSviSlobodniSlotovi, datum);
            return SviSlobodniSlotovi;
        }

        public static ObservableCollection<string> UkloniProsleSlotoveZaDanasnjiDatum(ObservableCollection<string> SviSlobodniSlotovi, ObservableCollection<string> PomocnaSviSlobodniSlotovi, Calendar datum)
        {
            if (datum.SelectedDate != DateTime.Now.Date)
                return SviSlobodniSlotovi;
            foreach (string slot in PomocnaSviSlobodniSlotovi)
            {
                DateTime vreme = DateTime.Parse(slot);
                DateTime sada = DateTime.Now;
                if (vreme.TimeOfDay <= sada.TimeOfDay)
                {
                    SviSlobodniSlotovi.Remove(slot);
                }
            }
            return SviSlobodniSlotovi;
        }

        public static ObservableCollection<string> UkloniZauzecaPacijentaZaSelektovaniDatum(ObservableCollection<string> SviSlobodniSlotovi, string selektovaniDatum, ObservableCollection<string> PomocnaSviSlobodniSlotovi)
        {
            Console.WriteLine(idPacijent);
            List<Termin> termini = TerminServis.PronadjiSveTerminePacijentaZaSelektovaniDatum(idPacijent, selektovaniDatum);
            foreach (Termin termin in termini)
            {
                foreach (string slot in PomocnaSviSlobodniSlotovi)
                {
                    if (termin.VremePocetka.Equals(slot))
                    {
                        SviSlobodniSlotovi.Remove(slot);
                    }
                }
            }
            return SviSlobodniSlotovi;
        }
        public static ObservableCollection<string> UkolniSlotoveZauzeteUSvimSalama(ObservableCollection<string> SviSlobodniSlotovi, ObservableCollection<string> PomocnaSviSlobodniSlotovi, Calendar datum)
        {
            List<string> SviZauzetiZaSelektovaniDatum = PronadjiSvaZauzecaZaSelektovaniDatum(SaleZaPreglede, datum);
            int brojacZauzetihSala;
            foreach (string slot in PomocnaSviSlobodniSlotovi)
            {
                brojacZauzetihSala = 0;
                foreach (string zauzeti in SviZauzetiZaSelektovaniDatum)
                {
                    if (slot.Equals(zauzeti))
                    {
                        brojacZauzetihSala++;
                        if (brojacZauzetihSala == ukupanBrojSalaZaPregled)
                        {
                            SviSlobodniSlotovi.Remove(slot);
                            break;
                        }
                    }
                }
            }
            return SviSlobodniSlotovi;
        }
        private static List<string> PronadjiSvaZauzecaZaSelektovaniDatum(List<Sala> SaleZaPreglede, Calendar datum)
        {
            SviZauzetiZaSelektovaniDatum = new List<string>();
            foreach (Sala sala in SaleZaPreglede)
            {
                foreach (ZauzeceSale zauzeceSale in sala.zauzetiTermini)
                {
                    DodajZauzeceZaSelektovaniDatum(SviZauzetiZaSelektovaniDatum, zauzeceSale, datum);
                }
            }
            return SviZauzetiZaSelektovaniDatum;
        }

        private static void DodajZauzeceZaSelektovaniDatum(List<string> SviZauzetiZaSelektovaniDatum, ZauzeceSale zauzeceSale, Calendar datum)
        {
            DateTime datumPocetkaZauzeca = DateTime.Parse(zauzeceSale.datumPocetkaTermina);
            DateTime datumKrajaZauzeca = DateTime.Parse(zauzeceSale.datumKrajaTermina);
            /* provera za termine i renoviranje(u periodu jednog dana - nekoliko sati) */
            if (datumPocetkaZauzeca.Equals(datum.SelectedDate) && datumKrajaZauzeca.Equals(datum.SelectedDate))
            {
                DodajZauzecaSaleZaTermine(SviZauzetiZaSelektovaniDatum, zauzeceSale);
            }
            /* ukoliko je selektovani datum u periodu renoviranja sale */
            else if (datumPocetkaZauzeca < datum.SelectedDate && datum.SelectedDate < datumKrajaZauzeca)
            {
                DodajZauzecaSaleZaVremeRenoviranja(SviZauzetiZaSelektovaniDatum);
            }
            /* provera da li se selektovani datum poklapa sa pocetkom renoviranja sale - slobodni termini pre renoviranja */
            else if (datumPocetkaZauzeca == datum.SelectedDate)
            {
                DodajZauzecaSaleZaPocetakRenoviranja(SviZauzetiZaSelektovaniDatum, zauzeceSale);
            }
            /* provera da li se selektovani datum poklapa sa krajem renoviranja sale - slobodni termini posle renoviranja */
            else if (datumKrajaZauzeca == datum.SelectedDate)
            {
                DodajZauzecaSaleZaKrajRenoviranja(SviZauzetiZaSelektovaniDatum, zauzeceSale);
            }
        }
        

        private static void DodajZauzecaSaleZaKrajRenoviranja(List<string> SviZauzetiZaSelektovaniDatum, ZauzeceSale zauzeceSale)
        {
            foreach (string slot in PomocnaSviSlobodniSlotovi)
            {
                int satiVreme = ParsirajSateVremenskogSlota(slot);
                int satiVremeKraja = ParsirajSateVremenskogSlota(zauzeceSale.krajTermina);
                if (satiVreme < satiVremeKraja)
                {
                    SviZauzetiZaSelektovaniDatum.Add(slot);
                }
            }
        }

        private static void DodajZauzecaSaleZaPocetakRenoviranja(List<string> SviZauzetiZaSelektovaniDatum, ZauzeceSale zauzeceSale)
        {
            foreach (string slot in PomocnaSviSlobodniSlotovi)
            {
                int satiVreme = ParsirajSateVremenskogSlota(slot);
                int satiVremePocetka = ParsirajSateVremenskogSlota(zauzeceSale.pocetakTermina);
                if (satiVreme >= satiVremePocetka)
                {
                    SviZauzetiZaSelektovaniDatum.Add(slot);
                }
            }
        }

        private static void DodajZauzecaSaleZaVremeRenoviranja(List<string> SviZauzetiZaSelektovaniDatum)
        {
            /* ukoliko je selektovani datum u periodu renoviranja sale - ceo dan sala je zauzeta */
            foreach (string slot in PomocnaSviSlobodniSlotovi)
            {
                SviZauzetiZaSelektovaniDatum.Add(slot);
            }
        }

        private static void DodajZauzecaSaleZaTermine(List<string> SviZauzetiZaSelektovaniDatum, ZauzeceSale zauzeceSale)
        {
            /* provera za termine i renoviranje(u periodu jednog dana - nekoliko sati) */
            foreach (string slot in PomocnaSviSlobodniSlotovi)
            {
                int satiVreme = ParsirajSateVremenskogSlota(slot);
                int minVreme = ParsirajMinuteVremenskogSlota(slot);
                int satiVremePocetka = ParsirajSateVremenskogSlota(zauzeceSale.pocetakTermina);
                int minVremePocetka = ParsirajMinuteVremenskogSlota(zauzeceSale.pocetakTermina);
                int satiVremeKraja = ParsirajSateVremenskogSlota(zauzeceSale.krajTermina);
                /* provera u slucaju da renoviranje traje jedan dan */
                if (zauzeceSale.idTermina == oznakaZaRenoviranje)
                {
                    if (satiVreme >= satiVremePocetka && satiVreme < satiVremeKraja)
                    {
                        SviZauzetiZaSelektovaniDatum.Add(slot);
                    }
                }
                /* provera da se selektovani datum poklapa sa nekim zakazanim terminom */
                else if (satiVreme == satiVremePocetka && minVreme == minVremePocetka)
                {
                    SviZauzetiZaSelektovaniDatum.Add(slot);
                }
            }
        }

        public static Sala Vpp_SelectionChanged(ComboBox vpp, Calendar datum)
        {
            string selektovaniDatum = FormatirajSelektovaniDatum(datum.SelectedDate.Value);
            string selektovaniSlot = vpp.SelectedValue.ToString();
            int satiVremeSelektovanogSlota = TerminServis.ParsirajSateVremenskogSlota(selektovaniSlot);
            /* Pronalazenje sale za koju je slobodan izabrani slot*/
            prvaSlobodnaSala = null;
            foreach (Sala sala in SaleZaPreglede)
            {
                bool postojiZauzece = ProveriVremeZauzecaZaTermine(selektovaniDatum, selektovaniSlot, sala) || ProveriVremeSvihZauzecaZaRenoviranje(selektovaniDatum, satiVremeSelektovanogSlota, sala);
                if (!postojiZauzece)
                {
                    prvaSlobodnaSala = sala;
                    return prvaSlobodnaSala;
                }
            }
            return null;
        }
        private static bool ProveriVremeZauzecaZaTermine(string selektovaniDatum, string selektovaniSlot, Sala sala)
        {
            foreach (ZauzeceSale zauzece in sala.zauzetiTermini)
            {
                if (prvaSlobodnaSala != null) break;
                if (zauzece.idTermina != oznakaZaRenoviranje && zauzece.datumPocetkaTermina.Equals(selektovaniDatum))
                {
                    if (zauzece.pocetakTermina.Equals(selektovaniSlot))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool ProveriVremeSvihZauzecaZaRenoviranje(string selektovaniDatum, int satiVreme, Sala sala)
        {
            foreach (ZauzeceSale zauzece in sala.zauzetiTermini)
            {
                if (zauzece.idTermina == oznakaZaRenoviranje)
                {
                    /* ukoliko renoviranje traje tokom jednog dana */
                    if (selektovaniDatum.Equals(zauzece.datumPocetkaTermina) && selektovaniDatum.Equals(zauzece.datumKrajaTermina))
                    {
                        int satiVremePocetka = ParsirajSateVremenskogSlota(zauzece.pocetakTermina);
                        int satiVremeKraja = ParsirajSateVremenskogSlota(zauzece.krajTermina);
                        if (satiVremePocetka <= satiVreme && satiVreme < satiVremeKraja)
                        {
                            return true;
                        }
                    }
                    /* ukoliko renoviranje traje vise dana, a termin se poklapa sa pocetkom zauzeca sale */
                    else if (selektovaniDatum.Equals(zauzece.datumPocetkaTermina))
                    {
                        int satiVremePocetka = ParsirajSateVremenskogSlota(zauzece.pocetakTermina);
                        if (satiVremePocetka <= satiVreme)
                        {
                            return true;
                        }
                    }
                    /* ukoliko renoviranje traje vise dana, a termin se poklapa sa krajem zauzeca sale */
                    else if (selektovaniDatum.Equals(zauzece.datumKrajaTermina))
                    {
                        int satiVremeKraja = ParsirajSateVremenskogSlota(zauzece.krajTermina);
                        if (satiVreme < satiVremeKraja)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region Termin Menadzer
        public static void ZakaziTermin(Termin termin)
        {
            TerminMenadzer.ZakaziTermin(termin);
        }
        public static int GenerisanjeIdTermina()
        {
            return TerminMenadzer.GenerisanjeIdTermina();
        }

        public static void IzmeniTermin(Termin stariTermin, Termin noviTermin)
        {
            TerminMenadzer.IzmeniTermin(stariTermin, noviTermin);
        }

        public static void OtkaziTermin(Termin termin)
        {
            TerminMenadzer.OtkaziTermin(termin);
        }

        public static List<Termin> NadjiSveTermine()
        {
            return TerminMenadzer.NadjiSveTermine();
        }

        public static Termin NadjiTerminPoId(int idTermin)
        {
            return TerminMenadzer.NadjiTerminPoId(idTermin);
        }

        public static void sacuvajIzmene()
        {
            TerminMenadzer.sacuvajIzmene();
        }

        public static List<Termin> PronadjiTerminPoIdPacijenta(int idPacijenta)
        {
            return TerminMenadzer.PronadjiTerminPoIdPacijenta(idPacijenta);
        }

        public static List<Termin> PronadjiSveTerminePacijentaZaSelektovaniDatum(int idPacijent, string selektovaniDatum)
        {
            return TerminMenadzer.PronadjiSveTerminePacijentaZaSelektovaniDatum(idPacijent, selektovaniDatum);
        }

        public static ObservableCollection<Termin> DodajTerminePacijenta(int idPacijent)
        {
            return TerminMenadzer.DodajTerminePacijenta(idPacijent);
        }

        #endregion
    }
}
