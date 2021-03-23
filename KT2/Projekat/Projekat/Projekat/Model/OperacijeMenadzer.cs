/***********************************************************************
 * Module:  OperacijeMenadzer.cs
 * Author:  Kristina
 * Purpose: Definition of the Class OperacijeMenadzer
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Model
{
    [Serializable]
    public class OperacijeMenadzer
   {
      public void ZakaziOperaciju(Operacije operacija)
      {
         // TODO: implement
      }
      
      public void IzmeniOperaciju(Operacije operacija)
      {
         // TODO: implement
      }
      
      public void OtkaziOperaciju(Operacije operacija)
      {
         // TODO: implement
      }
      
      public List<Operacije> NadjiSveOperacije()
      {
         // TODO: implement
         return null;
      }
      
      public Operacije NadjiOperacijuPoId(int idOperacije)
      {
         // TODO: implement
         return null;
      }
   
      public int AdresaFajla;

    }
}