using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Newtonsoft.Json;

namespace Projekat.Model
{
    public abstract class JSONSerialization<T> : ISerialization<T>
    {
        public void SacuvajIzmene(string lokacijaFajla, List<T> lista)
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

        public List<T> NadjiSve(string lokacijaFajla)
        {
            String text = File.ReadAllText(lokacijaFajla);
            List<T> list = JsonConvert.DeserializeObject<List<T>>(text);
            return list;
        }

        public abstract void Dodaj(T element, string lokacijaFajla);
        public abstract void Obrisi(T element, string lokacijaFajla);
        public abstract void Izmeni(T element, T element1, string lokacijaFajla);
    }
}
