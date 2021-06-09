using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Projekat.Model
{
    public abstract class JSONSerialization<T>
    {
        public static List<T> lista = new List<T>();
        private static string lokacijaFajla = "../obavestenja1.json";

        public static void sacuvajIzmene()
        {
            var file = JsonConvert.SerializeObject(lista, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
            using (StreamWriter writer = new StreamWriter(lokacijaFajla))
            {
                writer.Write(file);
            }
        }

        public static List<T> NadjiSve()
        {
            String text = File.ReadAllText(lokacijaFajla);
            List<T> list = JsonConvert.DeserializeObject<List<T>>(text);
            return list;
        }

        public static void Dodaj(T element)
        {
            lista.Add(element);
            sacuvajIzmene();
        }

        public static void Obrisi(T element)
        {
            lista.Remove(element);
            sacuvajIzmene();
        }

        public abstract void Izmeni(T element, T element1);
    }
}
