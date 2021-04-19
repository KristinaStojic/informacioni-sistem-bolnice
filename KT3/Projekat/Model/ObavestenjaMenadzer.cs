using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Projekat.Model
{
    public static class ObavestenjaMenadzer
    {
        public static List<Obavestenja> obavestenja = new List<Obavestenja>();

        public static void sacuvajIzmene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Obavestenja>));
            TextWriter fileStream = new StreamWriter("obavestenja.xml");
            serializer.Serialize(fileStream, obavestenja);
            fileStream.Close();
        }
        public static List<Obavestenja> NadjiSvaObavestenja()
        {
            if (File.ReadAllText("obavestenja.xml").Trim().Equals(""))
            {
                return obavestenja;
            }
            else
            {
                FileStream fileStream = File.OpenRead("obavestenja.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Obavestenja>));
                obavestenja = (List<Obavestenja>)serializer.Deserialize(fileStream);
                fileStream.Close();
                return obavestenja;
            }
        }
    }
}
