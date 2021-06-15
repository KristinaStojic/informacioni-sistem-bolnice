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

        public static void DodajMalicioznoPonasanje(int idPacijent)
        {
            MalicioznoPonasanje malicioznoPonasanje = new MalicioznoPonasanje(idPacijent);
            malicioznaPonasanja.Add(malicioznoPonasanje);
        }
    }
}
