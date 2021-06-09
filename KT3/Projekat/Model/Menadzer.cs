using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Projekat.Model
{
    public abstract class Menadzer<T>
    {
        public static List<T> lista; 

        public static void sacuvajIzmjene(string fajl)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            TextWriter filestream = new StreamWriter(fajl);
            serializer.Serialize(filestream, lista);
            filestream.Close();
        }

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

        private static void ucitajIzFajla(String fajl)
        {
            FileStream filestream = File.OpenRead(fajl);
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            lista = (List<T>)serializer.Deserialize(filestream);
            filestream.Close();
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

        public abstract void Izmjeni(T element, T element1);
    }
}
