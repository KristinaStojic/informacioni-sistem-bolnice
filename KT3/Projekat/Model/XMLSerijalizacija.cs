using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Projekat.Model
{
    public abstract class XMLSerijalizacija<T> : ISerijalizacija<T>
    {
        public void SacuvajIzmene(string fajl, List<T> lista)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            TextWriter fileStream = new StreamWriter(fajl);
            serializer.Serialize(fileStream, lista);
            fileStream.Close();
        }

        public List<T> NadjiSve(string fajl)
        {
            if (File.ReadAllText(fajl).Trim().Equals(""))
            {
                return new List<T>();
            }
            else
            {
                FileStream fileStream = File.OpenRead(fajl);
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                List<T> lista = (List<T>)serializer.Deserialize(fileStream);
                fileStream.Close();
                return lista;
            }
        }

        public abstract void Dodaj(T element, string fajl);
        public abstract void Obrisi(T element, string fajl);
        public abstract void Izmeni(T element, T element1, string lokacijaFajla);
    }
}
