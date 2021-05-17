/***********************************************************************
 * Module:  SaleMenadzer.cs
 * Author:  pc
 * Purpose: Definition of the Class SaleMenadzer
 ***********************************************************************/

using Projekat;
using Projekat.Model;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Model
{
    public class SaleMenadzer
    {
        public static void DodajSalu(Sala sala)
        {
            sale.Add(sala);
            sacuvajIzmjene();
        }

        public static void ObrisiSalu(Sala sala)
        {
            sale.Remove(sala);
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

        public static void IzmjeniSalu(Sala izSale, Sala uSalu)
        {
            foreach (Sala sala in sale)
            {
                if (sala.Id == izSale.Id)
                {
                    sala.brojSale = uSalu.brojSale;
                    sala.Namjena = uSalu.Namjena;
                    sala.TipSale = uSalu.TipSale;
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
        
        public static Krevet NadjiKrevetPoId(int id)
        {
            foreach (Krevet krevet in MainWindow.kreveti) //OpremaMenadzer.kreveti
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

        public static List<Sala> sale = new List<Sala>();
   }
}