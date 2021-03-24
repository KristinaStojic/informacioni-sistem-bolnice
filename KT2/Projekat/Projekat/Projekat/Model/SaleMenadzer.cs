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
      
      public static void IzmjeniSalu(Sala sala1, Sala sala)
      {
            foreach (Sala s in sale)
            {
                if (s.Id == sala1.Id)
                {
                    s.Namjena = sala.Namjena;
                    s.TipSale = sala.TipSale;
                    s.Status = sala.Status;
                }
            }
            int idx = PrikaziSalu.Sale.IndexOf(sala1);
            PrikaziSalu.Sale.RemoveAt(idx);
            PrikaziSalu.Sale.Insert(idx, sala);
        }
      
      public static List<Sala> NadjiSveSale()
      {
         return sale;
      }
      
      public static Sala NadjiSaluPoId(int id)
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