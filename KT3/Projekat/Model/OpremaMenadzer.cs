﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Projekat.Model
{
    class OpremaMenadzer
    {


        public static List<Oprema> NadjiStatickuOpremu()
        {
            List<Oprema> staticka = new List<Oprema>();
            List<Oprema> sve = NadjiSvuOpremu();
            foreach(Oprema o in sve)
            {
                if (o.Staticka)
                {
                    staticka.Add(o);
                }
            }
            return staticka;
        }

        public static List<Oprema> NadjiDinamickuOpremu()
        {
            List<Oprema> dinamicka = new List<Oprema>();
            List<Oprema> sve = NadjiSvuOpremu();
            foreach (Oprema o in sve)
            {
                if (!o.Staticka)
                {
                    dinamicka.Add(o);
                }
            }
            
            return dinamicka;
        }

        public static List<Oprema> NadjiSvuOpremu()
        {
            /*if (File.ReadAllText("oprema.xml").Trim().Equals(""))
            {
                return oprema;
            }
            else
            {
                FileStream filestream = File.OpenRead("oprema.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Oprema>));
                oprema = (List<Oprema>)serializer.Deserialize(filestream);
                filestream.Close();
                return oprema;
            }*/
            
            return oprema;
        }

        public static void DodajOpremu(Oprema o)
        {
            oprema.Add(o);
            if (o.Staticka)
            {
                Skladiste.OpremaStaticka.Add(o);
            }
            else
            {
                Skladiste.OpremaDinamicka.Add(o);
            }
        }

        public static void ObrisiOpremu(Oprema o)
        {
            oprema.Remove(o);
            if (o.Staticka)
            {
                Skladiste.OpremaStaticka.Remove(o);
            }
            else
            {
                Skladiste.OpremaDinamicka.Remove(o);
            }
        }

        public static Oprema NadjiOpremuPoId(int id)
        {
            foreach (Oprema o in oprema)
            {
                if (o.IdOpreme == id)
                {
                    return o;
                }
            }
            return null;
        }

        public static void sacuvajIzmjene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Oprema>));
            TextWriter filestream = new StreamWriter("oprema.xml");
            serializer.Serialize(filestream, oprema);
            filestream.Close();
        }
        public static int GenerisanjeIdOpreme()
        {
            bool pomocna = false;
            int id = 1;

            for (id = 1; id <= oprema.Count; id++)
            {
                foreach (Oprema o in oprema)
                {
                    if (o.IdOpreme.Equals(id))
                    {
                        pomocna = true;
                        break;
                    }
                }

                if (!pomocna)
                {
                    return id;
                }
                pomocna = false;
            }

            return id;
        }

        public static List<Oprema> oprema = new List<Oprema>();
    }
}