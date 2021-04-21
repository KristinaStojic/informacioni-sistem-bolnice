using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Projekat.Model
{
    public class AnketaMenadzer
    {
        public static List<Anketa> ankete = new List<Anketa>();

        public static void DodajAnketu(Anketa novaAnketa)
        {
            ankete.Add(novaAnketa);
        }

        public static void sacuvajIzmene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Oprema>));
            TextWriter filestream = new StreamWriter("ankete.xml");
            serializer.Serialize(filestream, ankete);
            filestream.Close();
        }

        public static List<Anketa> NadjiSveAnkete()
        {
            if (File.ReadAllText("ankete.xml").Trim().Equals(""))
            {
                return ankete;
            }
            else
            {
                FileStream filestream = File.OpenRead("ankete.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Anketa>));
                ankete = (List<Anketa>)serializer.Deserialize(filestream);
                filestream.Close();
                return ankete;
            }
        }

        public static int GenerisanjeIdAnkete()
        {
            bool pomocna = false;
            int id = 1;

            for (id = 1; id <= ankete.Count; id++)
            {
                foreach (Anketa anketa in ankete)
                {
                    if (anketa.IdAnkete.Equals(id))
                    {
                        pomocna = true;
                        break;
                    }
                }

                if (!pomocna)
                {
                    return id;
                }
                pomocna = false;
            }

            return id;
        }
    }
}
