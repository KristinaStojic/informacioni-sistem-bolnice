using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    public class SaleServis
    {
        public static void DodajSalu(Sala sala)
        {
            SaleMenadzer.DodajSalu(sala);
        }

        public static void ObrisiSalu(Sala sala)
        {
            SaleMenadzer.ObrisiSalu(sala);
        }

        public static void IzmjeniSalu(Sala izSale, Sala uSalu)
        {
            SaleMenadzer.IzmjeniSalu(izSale, uSalu);
        }

        public static List<Sala> NadjiSveSale()
        {
            return SaleMenadzer.NadjiSveSale();
        }

        public static List<Sala> Sale()
        {
            return SaleMenadzer.sale;
        }

        public static Sala NadjiSaluPoId(int id)
        {
            return SaleMenadzer.NadjiSaluPoId(id);
        }

        public static Krevet NadjiKrevetPoId(int id, Sala soba)
        {
            return SaleMenadzer.NadjiKrevetPoId(id, soba);
        }

        public static void sacuvajIzmjene()
        {
            SaleMenadzer.sacuvajIzmjene();
        }

        public static int GenerisanjeIdSale()
        {
            return SaleMenadzer.GenerisanjeIdSale();
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
                    Console.WriteLine("Obrisano zauzuce sale: " + sala.Id);
                    return;
                }
            }
        }

        public static List<Sala> PronadjiSaleZaPregled()
        {
            List<Sala> slobodneSaleZaPregled = new List<Sala>();
            foreach (Sala sala in SaleMenadzer.sale)
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
            foreach (Sala sala in SaleMenadzer.sale)
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
    }
}
