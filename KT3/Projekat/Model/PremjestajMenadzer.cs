using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Projekat.Model
{
    class PremjestajMenadzer
    {
        public static void dodajPremjestaj(Premjestaj premjestaj)
        {
            premjestaji.Add(premjestaj);
            sacuvajIzmjene();
        }

        public static List<Premjestaj> NadjiSvePremjestaje()
        {
            if (File.ReadAllText("premjestaj.xml").Trim().Equals(""))
            {
                return premjestaji;
            }
            else
            {
                ucitajPremjestajeIzFajla();
                return premjestaji;
            }
        }

        private static void ucitajPremjestajeIzFajla()
        {
            FileStream filestream = File.OpenRead("premjestaj.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(List<Premjestaj>));
            premjestaji = (List<Premjestaj>)serializer.Deserialize(filestream);
            filestream.Close();
        }

        public static void sacuvajIzmjene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Premjestaj>));
            TextWriter filestream = new StreamWriter("premjestaj.xml");
            serializer.Serialize(filestream, premjestaji);
            filestream.Close();
        }
       
        public static int GenerisanjeIdPremjestaja()
        {
            int id;

            for (id = 1; id <= premjestaji.Count; id++)
            { 
                if (!postojiIdPremjestaja(id))
                {
                    return id;
                }
            }

            return id;
        }

        private static bool postojiIdPremjestaja(int id)
        {
            foreach (Premjestaj premjestaj in premjestaji)
            {
                if (premjestaj.id.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Premjestaj> premjestaji = new List<Premjestaj>();
    }
}
