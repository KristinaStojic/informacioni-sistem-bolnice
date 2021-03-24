/***********************************************************************
 * Module:  TerminMenadzer.cs
 * Author:  Kristina
 * Purpose: Definition of the Class TerminMenadzer
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Projekat;


namespace Model
{
   // [Serializable]
    public class TerminMenadzer
   {
      public static void ZakaziTermin(Termin termin)
      {
            termini.Add(termin);
            PrikaziTermin.Termini.Add(termin);
            
      }
      
      public static void IzmeniTermin(Termin termin, Termin termin1)
      {
            foreach(Termin t in termini)
            {
                if (t.IdTermin == termin.IdTermin)
                {
                    t.VremePocetka = termin.VremePocetka;
                    t.VremeKraja = termin.VremeKraja;
                    t.Lekar = termin.Lekar;  // ili preko id-ja?
                    t.Pacijent = termin.Pacijent;
                    t.tipTermina = termin.tipTermina;
                    t.Datum = termin.Datum;
                    t.Prostorija = termin.Prostorija;
                }
                int idx = PrikaziTermin.Termini.IndexOf(termin);
                PrikaziTermin.Termini.RemoveAt(idx);
                PrikaziTermin.Termini.Insert(idx, termin1);

            }
      }
      
      public static void OtkaziTermin(Termin termin)
      {
            termini.Remove(termin);
            //PrikaziTermin.Termini.Remove(termin);
            int idx = PrikaziTermin.Termini.IndexOf(termin);
            PrikaziTermin.Termini.RemoveAt(idx);
            
        }
      
      public static List<Termin> NadjiSveTermine()
      {
        
         return termini;
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
   
      public int AdresaFajla;
      public static List<Termin> termini = new List<Termin>();
    }
}