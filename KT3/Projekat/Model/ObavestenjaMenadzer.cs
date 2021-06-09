using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Projekat.Model
{
    public class ObavestenjaMenadzer : JSONSerialization<Obavestenja>
    {
       // private ObavestenjaMenadzer() { }

        //public static List<Obavestenja> obavestenja = new List<Obavestenja>();
        //private static string fileLocation = "../obav.json";

        /* public static void sacuvajIzmene()
         {
             XmlSerializer serializer = new XmlSerializer(typeof(List<Obavestenja>));
             TextWriter fileStream = new StreamWriter("obavestenja.xml");
             serializer.Serialize(fileStream, obavestenja);
             fileStream.Close();
         }*/

        /* public static void sacuvajIzmene()
         {
             var file = JsonConvert.SerializeObject(ObavestenjaMenadzer.obavestenja, Formatting.Indented, new JsonSerializerSettings()
             {
                 ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                 PreserveReferencesHandling = PreserveReferencesHandling.Objects
             });
             using (StreamWriter writer = new StreamWriter(fileLocation))
             {
                 writer.Write(file);
             }
         }

        /

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

             String text = File.ReadAllText(fileLocation);
             List<Obavestenja> obavestenja = JsonConvert.DeserializeObject<List<Obavestenja>>(text);

             return obavestenja;
         }*/

        public static List<Obavestenja> SvaObavestenja()
        {
            return NadjiSve();
        }

        public override void Izmeni(Obavestenja staroObavestenje, Obavestenja novoObavestenje)
        {
            foreach (Obavestenja obavestenje in NadjiSve())
            {
                if (obavestenje.IdObavestenja == staroObavestenje.IdObavestenja)
                {
                    obavestenje.TipObavestenja = novoObavestenje.TipObavestenja;
                    obavestenje.Datum = novoObavestenje.Datum;
                    obavestenje.IdLekara = novoObavestenje.IdLekara;
                    obavestenje.ListaIdPacijenata = novoObavestenje.ListaIdPacijenata;
                    obavestenje.SadrzajObavestenja = novoObavestenje.SadrzajObavestenja;
                    obavestenje.Oznaka = novoObavestenje.Oznaka;
                    obavestenje.Notifikacija = novoObavestenje.Notifikacija;

                    int idx = OglasnaTabla.oglasnaTabla.IndexOf(staroObavestenje);
                    OglasnaTabla.oglasnaTabla.RemoveAt(idx);
                    OglasnaTabla.oglasnaTabla.Insert(0, obavestenje);
                }
            }

            sacuvajIzmene();
        }
    }
}
