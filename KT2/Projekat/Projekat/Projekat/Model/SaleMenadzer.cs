/***********************************************************************
 * Module:  SaleMenadzer.cs
 * Author:  pc
 * Purpose: Definition of the Class SaleMenadzer
 ***********************************************************************/

using Projekat;
using System;
using System.Collections.Generic;

namespace Model
{
   public class SaleMenadzer
   {
      public static void DodajSalu(Sala sala)
      {
            sale.Add(sala);
            PrikaziSalu.Sale.Add(sala);
        }

        public static void ObrisiSalu(Sala sala)
      {
            sale.Remove(sala);
            PrikaziSalu.Sale.Remove(sala);
      }
      
      public void IzmjeniSalu(int id, Sala sala)
      {
            foreach (Sala s in sale)
            {
                if (s.Id == id)
                {
                    s.Status = sala.Status;
                }
            }
        }
      
      public static List<Sala> NadjiSveSale()
      {
         return sale;
      }
      
      public Sala NadjiSaluPoId(int id)
      {
         foreach(Sala s in sale)
            {
                if(s.Id == id)
                {
                    return s;
                }
            }
            return null;
      }
   
      private string AdresaFajla;
      public static List<Sala> sale = new List<Sala>();
   }
}