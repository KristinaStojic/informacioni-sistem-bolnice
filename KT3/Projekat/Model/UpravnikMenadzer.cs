using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Projekat.Model
{
    public class UpravnikMenadzer
    {

        public static List<UpravnikModel> NadjiSveUpravnike()
        {
            if (File.ReadAllText("upravnici.xml").Trim().Equals(""))
            {
                return Upravnici;
            }
            else
            {
                UcitajUpravnikeIzFajla();
                return Upravnici;
            }
        }
        public static void UcitajUpravnikeIzFajla()
        {
            FileStream filestream = File.OpenRead("upravnici.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(List<UpravnikModel>));
            Upravnici = (List<UpravnikModel>)serializer.Deserialize(filestream);
            filestream.Close();
        }

        public static void SacuvajIzmjene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<UpravnikModel>));
            TextWriter filestream = new StreamWriter("upravnici.xml");
            serializer.Serialize(filestream, Upravnici);
            filestream.Close();
        }

        public static void DodajUpravnika(UpravnikModel upravnik)
        {
            Upravnici.Add(upravnik);
            SacuvajIzmjene();
        }

        public static List<UpravnikModel> Upravnici = new List<UpravnikModel>();
    }
}
