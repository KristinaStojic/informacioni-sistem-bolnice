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
      
      public static void IzmeniTermin(Termin termin)
      {
         // TODO: implement
      }
      
      public static void OtkaziTermin(Termin termin)
      {
         // TODO: implement
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