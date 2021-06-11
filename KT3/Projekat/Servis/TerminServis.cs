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
        private static int maxBrojPreporucenihTermina = 3;
        PacijentiServis servis = new PacijentiServis();

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
        public List<Sala> combo_SelectionChanged(ComboBox combo, ComboBox comboUputi, Hyperlink preferenca, int idPrijavljenogPacijenta)
        { 
            idPacijent = idPrijavljenogPacijenta;
            prijavljeniPacijent = servis.PronadjiPoId(idPacijent);

            SaleZaPreglede = SaleServis.PronadjiSaleZaPregled();
            ukupanBrojSalaZaPregled = SaleZaPreglede.Count();
            if (comboUputi == null)
            {
                return SaleZaPreglede;
            }
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
            SviSlobodniSlotovi = SaleServis.InicijalizujSveSlotove();
            PomocnaSviSlobodniSlotovi = SaleServis.InicijalizujSveSlotove();
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
        #region Termin preference
        public static Termin InicijalizujTermin(Pacijent prijavljeniPacijent, Sala s, ZauzeceSale zs, string slot)
        {
            Termin preporuceniTermin = new Termin();
            preporuceniTermin.IdTermin = TerminServis.GenerisanjeIdTermina();
            preporuceniTermin.Datum = zs.datumPocetkaTermina;
            preporuceniTermin.VremePocetka = slot;
            preporuceniTermin.VremeKraja = TerminServis.IzracunajVremeKrajaPregleda(slot);
            preporuceniTermin.Prostorija = s;
            preporuceniTermin.tipTermina = TipTermina.Pregled;
            // TODO: ispraviti kada dobijemo raspored radnog vremena
            preporuceniTermin.Lekar = prijavljeniPacijent.IzabraniLekar;
            preporuceniTermin.Pacijent = prijavljeniPacijent;
            return preporuceniTermin;
        }

        public static Termin InicijalizujPreporuceniTermin(Pacijent prijavljeniPacijent, Sala s, DateTime noviDatum, string slot)
        {
            Termin preporuceniTermin = new Termin();
            preporuceniTermin.IdTermin = TerminServis.GenerisanjeIdTermina();
            preporuceniTermin.Datum = noviDatum.ToString("MM/dd/yyyy");
            preporuceniTermin.VremePocetka = slot;
            preporuceniTermin.VremeKraja = TerminServis.IzracunajVremeKrajaPregleda(slot);
            preporuceniTermin.Prostorija = s;
            preporuceniTermin.tipTermina = TipTermina.Pregled;
            preporuceniTermin.Lekar = prijavljeniPacijent.IzabraniLekar;
            preporuceniTermin.Pacijent = prijavljeniPacijent;
            return preporuceniTermin;
        }

        public static ObservableCollection<Termin> PronadjiPreporuceneTermine(Pacijent prijavljeniPacijent, int brojacPreporucenihTermina, bool jeTri)
        {
            ObservableCollection<Termin> TerminiPreferenca = new ObservableCollection<Termin>();
            ObservableCollection<string> SviSlobodniSlotoviPreferenca = SaleServis.InicijalizujSveSlotove();
            
            foreach (Sala sala in SaleServis.NadjiSveSale())
            {
                if (sala.TipSale.Equals(tipSale.SalaZaPregled))
                {
                    for (int i = 0; i < maxBrojPreporucenihTermina; i++)
                    {
                        DateTime noviDatum = DateTime.Now.Date.AddDays(i); // tri dana unapred
                        if (i == 0)
                        {
                            IzbaciProsleSlotoveZaDanasnjiDan(SviSlobodniSlotoviPreferenca);
                        }
                        foreach (ZauzeceSale zs in sala.zauzetiTermini)
                        {
                            DateTime zsDatum = DateTime.Parse(zs.datumPocetkaTermina);
                            foreach (string slot in SviSlobodniSlotoviPreferenca)
                            {
                                if (!sala.zauzetiTermini.Exists(x => x.datumPocetkaTermina.Equals(noviDatum)) && zs.idTermina != 0)
                                {
                                    Termin preporuceniTermin = TerminServis.InicijalizujPreporuceniTermin(prijavljeniPacijent, sala, noviDatum, slot);
                                    TerminiPreferenca.Add(preporuceniTermin);
                                    brojacPreporucenihTermina++;
                                    if (PovecajBrojacPreporucenihTermina(brojacPreporucenihTermina, jeTri))
                                    {
                                        return TerminiPreferenca;
                                    }
                                }
                                else
                                {
                                    if (!sala.zauzetiTermini.Exists(x => x.pocetakTermina.Equals(slot)) && zs.idTermina != 0)
                                    {
                                        Termin preporuceniTermin = TerminServis.InicijalizujTermin(prijavljeniPacijent, sala, zs, slot);
                                        TerminiPreferenca.Add(preporuceniTermin);
                                        brojacPreporucenihTermina++;
                                        if (PovecajBrojacPreporucenihTermina(brojacPreporucenihTermina, jeTri))
                                        {
                                            return TerminiPreferenca;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return TerminiPreferenca;
        }

        public static void IzbaciProsleSlotoveZaDanasnjiDan(ObservableCollection<string> SviSlobodniSlotoviP)
        {
            ObservableCollection<string> PomocnaSviSlobodniSlotovi = SaleServis.InicijalizujSveSlotove();
            foreach (string slot in PomocnaSviSlobodniSlotovi)
            {
                DateTime vreme = DateTime.Parse(slot);
                DateTime sada = DateTime.Now;
                if (vreme.TimeOfDay <= sada.TimeOfDay)
                {
                    SviSlobodniSlotoviP.Remove(slot);
                }
            }
        }

        private static bool PovecajBrojacPreporucenihTermina(int brojacPreporucenihTermina, bool jeMaksimum)
        {
            if (brojacPreporucenihTermina >= maxBrojPreporucenihTermina)
            {
                jeMaksimum = true;
                return true;
            }
            return false;
        }

        #endregion

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

        public static ObservableCollection<Termin> PronadjiTerminPoIdPacijenta(int idPacijenta)
        {
            // TODO: u observavble listu
            //return TerminMenadzer.PronadjiTerminPoIdPacijenta(idPacijenta);
            ObservableCollection<Termin> TerminiPacijenta = new ObservableCollection<Termin>();
            foreach(Termin termin in TerminMenadzer.PronadjiTerminPoIdPacijenta(idPacijenta))
            {
                TerminiPacijenta.Add(termin);   
            }
            return TerminiPacijenta;
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
