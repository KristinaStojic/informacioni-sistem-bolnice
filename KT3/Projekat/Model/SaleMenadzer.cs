/***********************************************************************
 * Module:  SaleMenadzer.cs
 * Author:  pc
 * Purpose: Definition of the Class SaleMenadzer
 ***********************************************************************/

using Projekat;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace Model
{
    public class SaleMenadzer
    {
        public static void DodajSalu(Sala sala)
        {
            sale.Add(sala);
            PrikaziSalu.Sale.Add(sala);
            sacuvajIzmjene();
        }

        public static void ObrisiSalu(Sala sala)
        {
            sale.Remove(sala);
            PrikaziSalu.Sale.Remove(sala);
            obrisiTermineUSali(sala);
            sacuvajIzmjene();
        }

        private static void obrisiTermineUSali(Sala sala)
        {
            foreach (Termin t in TerminMenadzer.termini.ToArray())
            {
                if (t.Prostorija.Id == sala.Id)
                {
                    TerminMenadzer.termini.Remove(t);
                    TerminMenadzer.sacuvajIzmene();
                }
            }
        }

        public static void IzmjeniSalu(Sala izSale, Sala sala)
        {
            foreach (Sala s in sale)
            {
                if (s.Id == izSale.Id)
                {
                    s.brojSale = sala.brojSale;
                    s.Namjena = sala.Namjena;
                    s.TipSale = sala.TipSale;
                    int idx = PrikaziSalu.Sale.IndexOf(izSale);
                    PrikaziSalu.Sale.RemoveAt(idx);
                    PrikaziSalu.Sale.Insert(idx, s);
                }
            }
            sacuvajIzmjene();
        }

        public static List<Sala> NadjiSveSale()
        {
            if (File.ReadAllText("sale.xml").Trim().Equals(""))
            {
                return sale;
            }
            else {
                ucitajSaleIzFajla();
                return sale;
            }
        }

        private static void ucitajSaleIzFajla()
        {
            FileStream filestream = File.OpenRead("sale.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(List<Sala>));
            sale = (List<Sala>)serializer.Deserialize(filestream);
            filestream.Close();
        }

        public static Sala NadjiSaluPoId(int id)
        {
            foreach (Sala sala in sale)
            {
                if (sala.Id == id)
                {
                    return sala;
                }
            }
            return null;
        }

        public static void sacuvajIzmjene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Sala>));
            TextWriter filestream = new StreamWriter("sale.xml");
            serializer.Serialize(filestream, sale);
            filestream.Close();
        }

        public static int GenerisanjeIdSale()
        {
            int id;
            for (id = 1; id <= sale.Count; id++)
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
            foreach (Sala sala in sale)
            {
                if (sala.Id.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static ZauzeceSale NadjiZauzece(int idProstorije, int idTermin, string datum, string poc, string kraj)
        {
            Sala sala = NadjiSaluPoId(idProstorije);
            foreach (ZauzeceSale zauzece in sala.zauzetiTermini)
            {
                if (idTermin == zauzece.idTermina && datum.Equals(zauzece.datumPocetkaTermina) && poc.Equals(zauzece.pocetakTermina) && kraj.Equals(zauzece.krajTermina))
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
            foreach (Sala sala in sale)
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
            foreach (Sala sala in sale)
            {
                if (sala.TipSale.Equals(tipSale.OperacionaSala) && !sala.Namjena.Equals("Skladiste"))
                {
                    slobodneSaleZaOperaciju.Add(sala);
                }
            }
            return slobodneSaleZaOperaciju;
        }

        public static int UkupanBrojSalaZaPregled()
        {
            int ukupanBrojSalaZaPregled = 0;
            foreach (Sala sala in sale)
            {
                if (sala.TipSale.Equals(tipSale.SalaZaPregled) && !sala.Namjena.Equals("Skladiste"))
                {
                    ukupanBrojSalaZaPregled++;
                }
            }
            return ukupanBrojSalaZaPregled;
        }

        public static List<Sala> sale = new List<Sala>();
   }
}