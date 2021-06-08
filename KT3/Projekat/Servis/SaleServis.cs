using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Projekat.Servis
{
    public class SaleServis
    {
        public static void DodajSalu(Sala sala)
        {
            SaleMenadzer.Dodaj(sala, "sale.xml");
        }

        public static void ObrisiSalu(Sala sala)
        {
            SaleMenadzer.Obrisi(sala, "sale.xml");
            obrisiTermineUSali(sala);
        }

        private static void obrisiTermineUSali(Sala sala)
        {
            foreach (Termin t in TerminMenadzer.termini.ToArray())
            {
                if (t.Prostorija.Id == sala.Id)
                {
                    TerminMenadzer.termini.Remove(t);
                    TerminServisLekar.sacuvajIzmene();
                }
            }
        }

        public static void IzmjeniSalu(Sala izSale, Sala uSalu)
        {
            SaleMenadzer sm = new SaleMenadzer();
            sm.Izmjeni(izSale, uSalu);
        }

        public static List<Sala> NadjiSveSale()
        {
            return SaleMenadzer.NadjiSve("sale.xml");
        }

        public static List<Sala> Sale()
        {
            return SaleMenadzer.lista;
        }

        public static Sala NadjiSaluPoId(int id)
        {
            foreach (Sala sala in SaleMenadzer.lista)
            {
                if (sala.Id == id)
                {
                    return sala;
                }
            }
            return null;
        }

        public static Krevet NadjiKrevetPoId(int id, Sala soba)
        {
            foreach (Krevet krevet in soba.Kreveti) //OpremaMenadzer.kreveti
            {
                if (krevet.IdKreveta == id)
                {
                    return krevet;
                }
            }
            return null;
        }

        public static void sacuvajIzmjene()
        {
            SaleMenadzer.sacuvajIzmjene("sale.xml");
        }

        public static int GenerisanjeIdSale()
        {
            int id;
            for (id = 1; id <= SaleMenadzer.lista.Count; id++)
            {
                if (!postojiIdSale(id))
                {
                    return id;
                }
            }
            return id;
        }

        private static bool postojiIdSale(int id)
        {
            foreach (Sala sala in SaleMenadzer.lista)
            {
                if (sala.Id.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static ZauzeceSale NadjiZauzece(int idProstorije, int idTermin, string datum, string pocetak, string kraj)
        {
            Sala sala = NadjiSaluPoId(idProstorije);
            foreach (ZauzeceSale zauzece in sala.zauzetiTermini)
            {
                if (idTermin == zauzece.idTermina && datum.Equals(zauzece.datumPocetkaTermina) && pocetak.Equals(zauzece.pocetakTermina) && kraj.Equals(zauzece.krajTermina))
                {
                    return zauzece;
                }
            }
            return null;
        }

        public static void ObrisiZauzeceSale(int IdSale, int IdTermina)
        {
            Sala sala = NadjiSaluPoId(IdSale);
            foreach (ZauzeceSale zauzeceSale in sala.zauzetiTermini)
            {
                if (zauzeceSale.idTermina == IdTermina)
                {
                    sala.zauzetiTermini.Remove(zauzeceSale);
                    return;
                }
            }
        }

        public static List<Sala> PronadjiSaleZaPregled()
        {
            List<Sala> slobodneSaleZaPregled = new List<Sala>();
            foreach (Sala sala in SaleMenadzer.lista)
            {
                if (sala.TipSale.Equals(tipSale.SalaZaPregled) && !sala.Namjena.Equals("Skladiste"))
                {
                    slobodneSaleZaPregled.Add(sala);
                }
            }
            return slobodneSaleZaPregled;
        }

        public static List<Sala> PronadjiSaleZaOperaciju()
        {
            List<Sala> slobodneSaleZaOperaciju = new List<Sala>();
            foreach (Sala sala in SaleMenadzer.lista)
            {
                if (sala.TipSale.Equals(tipSale.OperacionaSala) && !sala.Namjena.Equals("Skladiste"))
                {
                    slobodneSaleZaOperaciju.Add(sala);
                }
            }
            return slobodneSaleZaOperaciju;
        }

        public static ObservableCollection<string> InicijalizujSveSlotove()
        {
            return new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30", "09:00", "09:30",  "10:00", "10:30","11:00", "11:30",
                                                        "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
                                                        "15:00", "15:30", "16:00", "16:30","17:00", "17:30",
                                                        "18:00", "18:30", "19:00", "19:30", "20:00"};
        }

        public static void DodajZauzeceSale(Termin termin, Sala prvaSlobodnaSala)
        {
            ZauzeceSale zs = new ZauzeceSale(termin.VremePocetka, termin.VremeKraja, termin.Datum, termin.IdTermin);
            prvaSlobodnaSala.zauzetiTermini.Add(zs);
        }

        public static bool salaZakazanaZaRenoviranje(Sala izabranaSala)
        {
            foreach (ZauzeceSale zauzeceSale in izabranaSala.zauzetiTermini)
            {
                if (zauzeceSale.idTermina == 0 && datumProsao(zauzeceSale.datumKrajaTermina) && vrijemeProslo(zauzeceSale.krajTermina))
                {
                    izabranaSala.zauzetiTermini.Remove(zauzeceSale);
                    return false;
                }
                else if (zauzeceSale.idTermina == 0 && (!datumProsao(zauzeceSale.datumKrajaTermina) || !vrijemeProslo(zauzeceSale.krajTermina)))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool datumProsao(string datum)
        {
            datum = datum.Replace('/', '-');
            DateTime datumKraja = DateTime.Parse(datum);
            return datumKraja <= DateTime.Now.Date;
        }

        private static bool vrijemeProslo(string vrijeme)
        {
            int vrijemeKraja = int.Parse(vrijeme.Split(':')[0]);
            int sadasnjeVrijeme = int.Parse(DateTime.Now.TimeOfDay.ToString().Split(':')[0]);
            return vrijemeKraja <= sadasnjeVrijeme;
        }

        public static void ukloniOpremuIzSale(Oprema izabranaOprema, int kolicina)
        {

            foreach (Oprema oprema in OpremaServis.Oprema().ToArray())
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    oprema.Kolicina -= kolicina;
                    if (oprema.Kolicina == 0)
                    {
                        OpremaServis.Oprema().Remove(oprema);
                    }
                }
            }
        }

        public static void prebaciOpremuIzStareSale(Sala izabranaSala, List<Oprema> opremaZaPrebacivanje)
        {
            foreach (Oprema oprema in izabranaSala.Oprema.ToArray())
            {
                prebaciOpremu(oprema, opremaZaPrebacivanje, izabranaSala);
            }
        }

        private static void prebaciOpremu(Oprema oprema, List<Oprema> opremaZaPrebacivanje, Sala izabranaSala)
        {
            foreach (Oprema opremaPrebacivanje in opremaZaPrebacivanje)
            {
                if (opremaPrebacivanje.IdOpreme == oprema.IdOpreme)
                {
                    ukloniOpremuIzSale(oprema, opremaPrebacivanje, izabranaSala);
                }
            }
        }

        private static void ukloniOpremuIzSale(Oprema oprema, Oprema opremaPrebacivanje, Sala izabranaSala)
        {
            oprema.Kolicina -= opremaPrebacivanje.Kolicina;
            if (oprema.Kolicina == 0)
            {
                izabranaSala.Oprema.Remove(oprema);
            }
        }


        public static void zauzmiSalu(ZauzeceSale zauzeceSale, Sala izabranaSala)
        {
            foreach (Sala sala in SaleMenadzer.lista)
            {
                if (sala.Id == izabranaSala.Id)
                {
                    sala.zauzetiTermini.Add(zauzeceSale);
                }
            }
        }
        public static void dodajOpremuIzSaleZaDodavanje(Sala izabranaSala, Sala salaZaSpajanje)
        {
            foreach (Sala sala in SaleServis.Sale())
            {
                if (sala.Id == izabranaSala.Id)
                {
                    dodajOpremu(sala, salaZaSpajanje);
                }
            }
        }
        private static void dodajOpremu(Sala sala, Sala salaZaSpajanje)
        {
            foreach (Oprema oprema in salaZaSpajanje.Oprema)
            {
                if (!postojiOpremaUSali(sala, oprema))
                {
                    sala.Oprema.Add(oprema);
                }
            }
        }

        private static bool postojiOpremaUSali(Sala sala, Oprema oprema)
        {
            foreach (Oprema opremaSale in sala.Oprema)
            {
                if (opremaSale.IdOpreme == oprema.IdOpreme)
                {
                    opremaSale.Kolicina += oprema.Kolicina;
                    return true;
                }
            }
            return false;
        }
    }
}
