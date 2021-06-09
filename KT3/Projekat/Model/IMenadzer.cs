using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public interface IMenadzer<T>
    {
        /*public static void sacuvajIzmene(string fajl)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            TextWriter filestream = new StreamWriter(fajl);
            serializer.Serialize(filestream, lista);
            filestream.Close();
        }

        private static void ucitajPodatke(String fajl)
        {
            FileStream filestream = File.OpenRead(fajl);
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            lista = (List<T>)serializer.Deserialize(filestream);
            filestream.Close();



            //
            var file = JsonConvert.SerializeObject(doctors, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
            using (StreamWriter writer = new StreamWriter(this.fileLocation))
            {
                writer.Write(file);
            }
        }*/


       /* public static List<T> lista;

        public static List<T> NadjiSve(String fajl)
        {
            if (File.ReadAllText(fajl).Trim().Equals(""))
            {
                return lista;
            }
            else
            {
                ucitajIzFajla(fajl);
                return lista;
            }
        }

        public static void Dodaj(T element, String fajl)
        {
            lista.Add(element);
            sacuvajIzmjene(fajl);
        }

        public static void Obrisi(T element, String fajl)
        {
            lista.Remove(element);
            sacuvajIzmjene(fajl);
        }

        public abstract void Izmeni(T element, T element1);
       */
    }
}
