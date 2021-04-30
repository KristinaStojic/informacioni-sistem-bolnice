using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Projekat.Model
{
    class AnketaMenadzer
    {
        public static List<Anketa> ankete = new List<Anketa>();

        public static void sacuvajIzmene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Anketa>));
            TextWriter fileStream = new StreamWriter("anketa.xml");
            serializer.Serialize(fileStream, ankete);
            fileStream.Close();
        }

        public static List<Anketa> NadjiSveAnkete()
        {
            if (File.ReadAllText("anketa.xml").Trim().Equals(""))
            {
                return ankete;
            }
            else
            {
                FileStream fileStream = File.OpenRead("anketa.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Anketa>));
                ankete = (List<Anketa>)serializer.Deserialize(fileStream);
                fileStream.Close();
                return ankete;
            }
        }

        public static int GenerisanjeIdAnkete()
        {
            bool pomocna = false;
            int idAnkete = 1;
            for (idAnkete = 1; idAnkete <= ankete.Count; idAnkete++)
            {
                foreach (Anketa anketa in ankete)
                {
                    if (anketa.idAnkete.Equals(idAnkete))
                    {
                        pomocna = true;
                        break;
                    }
                }
                if (!pomocna)
                {
                    return idAnkete;
                }
                pomocna = false;
            }
            return idAnkete;
        }

        public static Anketa NadjiAnketuPoId(int IdAnkete)
        {
            foreach (Anketa anketa in ankete)
            {
                if (anketa.idAnkete == IdAnkete)
                {
                    return anketa;
                }
            }
            return null;
        }

    }
}
