using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Projekat.Model
{
    class AnketaMenadzer
    {
        public static List<Anketa> ankete = new List<Anketa>();
        public static int oznakaAnketeZaKliniku = 0;
        public static void sacuvajIzmene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Anketa>));
            TextWriter fileStream = new StreamWriter("anketa.xml");
            serializer.Serialize(fileStream, ankete);
            fileStream.Close();
        }

        public static List<Anketa> NadjiSveAnkete()
        {
            if (File.ReadAllText("anketa.xml").Trim().Equals(""))
            {
                return ankete;
            }
            else
            {
                FileStream fileStream = File.OpenRead("anketa.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Anketa>));
                ankete = (List<Anketa>)serializer.Deserialize(fileStream);
                fileStream.Close();
                return ankete;
            }
        }

        public static int GenerisanjeIdAnkete()
        {
            bool pomocna = false;
            int idAnkete = 1;
            for (idAnkete = 1; idAnkete <= ankete.Count; idAnkete++)
            {
                foreach (Anketa anketa in ankete)
                {
                    if (anketa.IdAnkete.Equals(idAnkete))
                    {
                        pomocna = true;
                        break;
                    }
                }
                if (!pomocna)
                {
                    return idAnkete;
                }
                pomocna = false;
            }
            return idAnkete;
        }

        public static Anketa NadjiAnketuPoId(int IdAnkete)
        {
            foreach (Anketa anketa in ankete)
            {
                if (anketa.IdAnkete == IdAnkete)
                {
                    return anketa;
                }
            }
            return null;
        }

        public static void ObrisiAnketu(int idTerminaZaBrisanje)
        {
            foreach(Anketa anketa in ankete)
            {
                if(anketa.IdTermina == idTerminaZaBrisanje)
                {
                    ankete.Remove(anketa);
                    return;
                }
            }
        }

        public static void DodajAnketuZaLekara(Termin termin, int idPacijent) 
        {
            string podaciLekara = termin.Lekar.ImeLek + " " + termin.Lekar.PrezimeLek;
            Anketa anketaZaLekara = new Anketa(VrstaAnkete.ZaLekare, "Anketa za lekara: " + podaciLekara, idPacijent, termin.IdTermin);
            ankete.Add(anketaZaLekara);
        }

        public static void DodajAnketuZaKliniku(int idPacijent)
        {
            Anketa anketa = new Anketa(VrstaAnkete.ZaKliniku, "Anketa o uslugama bolnice", idPacijent, oznakaAnketeZaKliniku);
            ankete.Add(anketa);
        }

        public static void IzmeniAnketuZaLekara(Termin stariTermin, Termin noviTermin)
        {
            if (stariTermin.Lekar.IdLekara == noviTermin.Lekar.IdLekara)
            {
                return;
            }
            IzmeniNazivAnkete(stariTermin, noviTermin);
        }

        private static void IzmeniNazivAnkete(Termin stariTermin, Termin noviTermin)
        {
            foreach (Anketa anketa in ankete)
            {
                if (anketa.IdTermina == stariTermin.IdTermin)
                {
                    string podaciLekara = noviTermin.Lekar.ImeLek + " " + noviTermin.Lekar.PrezimeLek;
                    anketa.NazivAnkete = "Anketa za lekara: " + podaciLekara;
                    return;
                }
            }
        }

        public static Anketa PronadjiAnketuZaKliniku(int idPacijent)
        {
            foreach(Anketa anketa in ankete)
            {
                if (anketa.IdPacijent == idPacijent && anketa.IdTermina == oznakaAnketeZaKliniku)
                {
                    return anketa;
                }
            }
            return null;
        }
    }
}
