using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Projekat.Model
{
    class MalicioznoPonasanjeMenadzer
    {
        public static List<MalicioznoPonasanje> malicioznaPonasanja = new List<MalicioznoPonasanje>();
        private static int maksBrojMalicioznogPonasanjaPoDanu = 3;

        public static void sacuvajIzmene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<MalicioznoPonasanje>));
            TextWriter fileStream = new StreamWriter("detektor.xml");
            serializer.Serialize(fileStream, malicioznaPonasanja);
            fileStream.Close();
        }

        public static List<MalicioznoPonasanje> NadjiSvaMalicioznaPonasanja()
        {
            if (File.ReadAllText("detektor.xml").Trim().Equals(""))
            {
                return malicioznaPonasanja;
            }
            else
            {
                FileStream fileStream = File.OpenRead("detektor.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<MalicioznoPonasanje>));
                malicioznaPonasanja = (List<MalicioznoPonasanje>)serializer.Deserialize(fileStream);
                fileStream.Close();
                return malicioznaPonasanja;
            }
        }

        public static int GenerisanjeIdMalicioznogPonasanja()
        {
            bool pomocna = false;
            int idMalicioznog = 1;
            for (idMalicioznog = 1; idMalicioznog <= malicioznaPonasanja.Count; idMalicioznog++)
            {
                foreach (MalicioznoPonasanje malicioznoPonasanje in malicioznaPonasanja)
                {
                    if (malicioznoPonasanje.IdMalicioznogPonasanja.Equals(idMalicioznog))
                    {
                        pomocna = true;
                        break;
                    }
                }
                if (!pomocna)
                {
                    return idMalicioznog;
                }
                pomocna = false;
            }
            return idMalicioznog;
        }
      
        public static void ObrisiMalicioznoPonasanje(int idMalicioznog)
        {
            foreach (MalicioznoPonasanje mp in malicioznaPonasanja)
            {
                if (mp.IdMalicioznogPonasanja == idMalicioznog)
                {
                    malicioznaPonasanja.Remove(mp);
                    return;
                }
            }
        }

        public static bool DetektujMalicioznoPonasanje(int idPacijenta)
        {
            int brojacMalicioznogPonasanja = 0;
            foreach(MalicioznoPonasanje ponasanje in malicioznaPonasanja)
            {
                if(ponasanje.IdPacijenta == idPacijenta && ponasanje.DatumModifikacije == DateTime.Now.Date)
                {
                    brojacMalicioznogPonasanja++;
                    if (brojacMalicioznogPonasanja == maksBrojMalicioznogPonasanjaPoDanu)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void DodajMalicioznoPonasanje(int idPacijent)
        {
            MalicioznoPonasanje malicioznoPonasanje = new MalicioznoPonasanje(idPacijent);
            malicioznaPonasanja.Add(malicioznoPonasanje);
        }
    }
}
