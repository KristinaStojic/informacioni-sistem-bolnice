/***********************************************************************
 * Module:  TerminMenadzer.cs
 * Author:  Kristina
 * Purpose: Definition of the Class TerminMenadzer
 ***********************************************************************/

using Projekat;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;


namespace Model
{
    public class TerminMenadzer
   {
      public static void ZakaziTermin(Termin termin)
      {
            termini.Add(termin);
            PrikaziTermin.Termini.Add(termin);
            //PrikazTerminaLekar.Termini.Add(termin);
            //PrikaziTerminSekretar.TerminiSekretar.Add(termin);
        }

        public static void ZakaziTerminSekretar(Termin termin)
        {
            termini.Add(termin);
            PrikaziTerminSekretar.TerminiSekretar.Add(termin);
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

        public static void IzmeniTerminLekar(Termin termin, Termin termin1)
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

      public static void IzmeniTerminSekretar(Termin termin, Termin termin1)
        {
            foreach (Termin t in termini)
            {
                if (t.IdTermin == termin.IdTermin)
                {
                    t.IdTermin = termin1.IdTermin;
                    t.VremePocetka = termin1.VremePocetka;
                    t.VremeKraja = termin1.VremeKraja;
                    t.Lekar = termin1.Lekar; 
                    t.Pacijent = termin1.Pacijent;
                    t.tipTermina = termin1.tipTermina;
                    t.Datum = termin1.Datum;
                    t.Prostorija = termin1.Prostorija;
                }

            }            
            int idx = PrikaziTerminSekretar.TerminiSekretar.IndexOf(termin);
            PrikaziTerminSekretar.TerminiSekretar.RemoveAt(idx);
            PrikaziTerminSekretar.TerminiSekretar.Insert(idx, termin1);
        }

        public static void OtkaziTermin(Termin termin)
      {
            //termini.Remove(termin);
            //PrikazTerminaLekar.Termini.Remove(termin);
            //PrikaziTerminSekretar.TerminiSekretar.Remove(termin);
            //PrikazTerminaLekar.Termini.Remove(termin);
            for (int i = 0; i < termini.Count; i++)
            {
                if (termin.IdTermin == termini[i].IdTermin)
                {
                    termini.RemoveAt(i);
                    termin.Prostorija.Status = status.Slobodna;
                }
            }
            PrikaziTermin.Termini.Remove(termin);
       }

        
        public static void OtkaziTerminSekretar(Termin termin)
        {
            for (int i = 0; i < termini.Count; i++)
            {
                if (termin.IdTermin == termini[i].IdTermin)
                {
                    // brisanje termina iz zauzetih termina u prostorijama
                    foreach (Sala s in SaleMenadzer.sale)
                    {
                        if (s.Id == termin.Prostorija.Id)
                        {
                            s.zauzetiTermini.Remove(SaleMenadzer.NadjiZauzece(s.Id, termin.IdTermin, termin.Datum, termin.VremePocetka, termin.VremeKraja));
                            //SaleMenadzer.sacuvajIzmjene();
                        }
                    }

                    termini.RemoveAt(i);
                    termin.Prostorija.Status = status.Slobodna;
                    Console.WriteLine("obrisan i termin");
                }
            }          
          
            PrikaziTerminSekretar.TerminiSekretar.Remove(termin);
       }

        // samo izmeni u svojoj klasi iz OtkaziTermin ---> OtkaziTerminLekar
        public static void OtkaziTerminLekar(Termin termin)
        {
            //termini.Remove(termin);
            for (int i = 0; i < termini.Count; i++)
            {
                if (termin.IdTermin == termini[i].IdTermin)
                {

                    termini.RemoveAt(i);
                    termin.Prostorija.Status = status.Slobodna;
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

        public static Boolean SlobodanTermin(String datum, String VremePocetka, String VremeKraja, Sala sala) 
        {
           // foreach (Termin t in TerminMenadzer.NadjiSveTermine())
            //{
                // postoji zakazan termin u tom opsegu
               // if (/*t.Datum.Equals(datum) &&*/ t.Prostorija.Id == sala.Id /*&& Int32.Parse(VremePocetka) >= Int32.Parse(t.VremePocetka) && Int32.Parse(VremeKraja) <= Int32.Parse(t.VremeKraja)*/)
               /* {
                    return false;
                } */
            //}
            return true;
        }

      //public int AdresaFajla;  // ?
      public static List<Termin> termini = new List<Termin>();
    }
}