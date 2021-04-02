/***********************************************************************
 * Module:  TerminMenadzer.cs
 * Author:  Kristina
 * Purpose: Definition of the Class TerminMenadzer
 ***********************************************************************/

using Projekat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;


namespace Model
{
    public class TerminMenadzer
   {
      public static void ZakaziTermin(Termin termin)
      {
            termini.Add(termin);
            PrikaziTermin.Termini.Add(termin);
        }

        // isto ovu metodu
        public static void ZakaziTerminLekar(Termin termin)
        {
            termini.Add(termin);
            PrikazTerminaLekar.Termini.Add(termin);
        }

        public static int GenerisanjeIdTermina()
        {
            bool pomocna = false;
            int id = 1;

            for (id = 1; id <= termini.Count; id++)
            {
                foreach (Termin p in termini)
                {
                    if (p.IdTermin.Equals(id))
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


        public static void IzmeniTermin(Termin termin, Termin termin1)
        {
            foreach(Termin t in termini)
            {
                if (t.IdTermin == termin.IdTermin)
                {
                    t.IdTermin = termin1.IdTermin;
                    t.VremePocetka = termin1.VremePocetka;
                    t.VremeKraja = termin1.VremeKraja;
                    t.Lekar = termin1.Lekar;  // ili preko id-ja?
                    t.Pacijent = termin1.Pacijent;
                    t.tipTermina = termin1.tipTermina;
                    t.Datum = termin1.Datum;
                    t.Prostorija = termin1.Prostorija;
                    //Console.WriteLine(termin1.Pacijent.ImePacijenta + "  "  + termin1.Pacijent.PrezimePacijenta);
                }
                
            }
            int idx = PrikaziTermin.Termini.IndexOf(termin);
            PrikaziTermin.Termini.RemoveAt(idx);
            PrikaziTermin.Termini.Insert(idx, termin1);
            
            //  **** napraviti metodu --> izmeniTerminLekar(...)
            /*int idx = PrikazTerminaLekar.Termini.IndexOf(termin);
            PrikazTerminaLekar.Termini.RemoveAt(idx);
            PrikazTerminaLekar.Termini.Insert(idx, termin1);*/
        }

        public static void IzmeniTermin(Termin termin, Termin termin1)
        {
            foreach (Termin t in termini)
            {
                if (t.IdTermin == termin.IdTermin)
                {
                    t.IdTermin = termin1.IdTermin;
                    t.VremePocetka = termin1.VremePocetka;
                    t.VremeKraja = termin1.VremeKraja;
                    t.Lekar = termin1.Lekar;  // ili preko id-ja?
                    t.Pacijent = termin1.Pacijent;
                    t.tipTermina = termin1.tipTermina;
                    t.Datum = termin1.Datum;
                    t.Prostorija = termin1.Prostorija;
                    //Console.WriteLine(termin1.Pacijent.ImePacijenta + "  "  + termin1.Pacijent.PrezimePacijenta);
                }

            }

            //  **** napraviti metodu --> izmeniTerminLekar(...)
            int idx = PrikazTerminaLekar.Termini.IndexOf(termin);
            PrikazTerminaLekar.Termini.RemoveAt(idx);
            PrikazTerminaLekar.Termini.Insert(idx, termin1);
        }

        public static void OtkaziTermin(Termin termin)
      {
           // termini.Remove(termin);
            //PrikazTerminaLekar.Termini.Remove(termin);
            for (int i = 0; i < termini.Count; i++)
            {
                if (termin.IdTermin == termini[i].IdTermin)
                {
                    termini.RemoveAt(i);
                }
            }
            PrikaziTermin.Termini.Remove(termin);
       }

        // samo izmeni u svojoj klasi iz OtkaziTermin ---> OtkaziTerminLekar
        public static void OtkaziTerminLekar(Termin termin)
        {
            termini.Remove(termin);
            for (int i = 0; i < termini.Count; i++)
            {
                if (termin.IdTermin == termini[i].IdTermin)
                {
                    termini.RemoveAt(i);
                }
            }
            PrikazTerminaLekar.Termini.Remove(termin);

        }

        public static List<Termin> NadjiSveTermine()
      {
            if (File.ReadAllText("termini.xml").Trim().Equals(""))
            {
                return termini;
            }
            else
            {
                FileStream fileStream = File.OpenRead("termini.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Termin>));
                termini = (List<Termin>)serializer.Deserialize(fileStream);
                fileStream.Close();
                return termini;
            }
        }
      
      public Termin NadjiTerminPoId(int idTermin)
      {
            foreach (Termin t in termini)
            {
                if (t.IdTermin == idTermin)
                {
                    return t;
                }
            }
            return null;
        }


        public static void sacuvajIzmene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Termin>));
            TextWriter fileStream = new StreamWriter("termini.xml");
            serializer.Serialize(fileStream, termini);
            fileStream.Close();
        }
   
      //public int AdresaFajla;  // ?
      public static List<Termin> termini = new List<Termin>();
    }
}