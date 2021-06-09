using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace Projekat.Model
{
    public abstract class XMLSerialization<T>
    {
        public static List<T> lista = new List<T>();

        public static void sacuvajIzmene(string fajl)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            TextWriter fileStream = new StreamWriter(fajl);
            serializer.Serialize(fileStream, lista);
            fileStream.Close();
        }
        public static List<T> NadjiSve(string fajl)
        {
            if (File.ReadAllText(fajl).Trim().Equals(""))
            {
                return lista;
            }
            else
            {
                FileStream fileStream = File.OpenRead(fajl);
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                lista = (List<T>)serializer.Deserialize(fileStream);
                fileStream.Close();
                return lista;
            }
        }

        public static void Dodaj(T element, String fajl)
        {
            lista.Add(element);
            sacuvajIzmene(fajl);
        }

        public static void Obrisi(T element, String fajl)
        {
            lista.Remove(element);
            sacuvajIzmene(fajl);
        }

        public abstract void Izmeni(T element, T element1);
    }
}
