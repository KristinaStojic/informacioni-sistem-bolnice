using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static void DodajObavestenje(Obavestenja novoObavestenje)
        {
            obavestenja.Insert(0, novoObavestenje);
            if (OglasnaTabla.oglasnaTabla == null)
            {
                OglasnaTabla.oglasnaTabla = new ObservableCollection<Obavestenja>();
            }
            OglasnaTabla.oglasnaTabla.Insert(0, novoObavestenje);  

            ObavestenjaMenadzer.sacuvajIzmene();
        }

        public static void IzmeniObavestenje(Obavestenja staroObavestenje, Obavestenja novoObavestenje)
        {
            foreach (Obavestenja obavestenje in obavestenja)
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
        }

        public static void ObrisiObavestenje(Obavestenja obavestenje)
        {
            for (int i = 0; i < obavestenja.Count; i++)
            {
                if (obavestenja[i].IdObavestenja == obavestenje.IdObavestenja)
                {
                    obavestenja.RemoveAt(i);
                    OglasnaTabla.oglasnaTabla.Remove(obavestenje);
                }
            }
        }

        public static Obavestenja PronadjiPoId(int id)
        {
            foreach (Obavestenja obavestenje in obavestenja)
            {
                if (obavestenje.IdObavestenja == id)
                {
                    return obavestenje;
                }
            }
            return null;
        }

        public static int GenerisanjeIdObavestenja()
        {
            bool pomocna = false;
            int id = 1;

            for (id = 1; id <= obavestenja.Count; id++)
            {
                foreach (Obavestenja obavestenje in obavestenja)
                {
                    if (obavestenje.IdObavestenja == id)
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

        public static void ObrisiObavestenjePacijent(Obavestenja selektovanoObavestenje)
        {
            foreach(Obavestenja o in obavestenja)
            {
                if(o.IdObavestenja == selektovanoObavestenje.IdObavestenja)
                {
                    obavestenja.Remove(o);
                    return;
                }
            }
        }
    }
}
