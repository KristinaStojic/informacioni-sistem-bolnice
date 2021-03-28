/***********************************************************************
 * Module:  SaleMenadzer.cs
 * Author:  pc
 * Purpose: Definition of the Class SaleMenadzer
 ***********************************************************************/

using Projekat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

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
                    s.Id = sala.Id;
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
            if (File.ReadAllText("sale.xml").Trim().Equals(""))
            {
                return sale;
            }
            else {
                FileStream filestream = File.OpenRead("sale.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Sala>));
                sale = (List<Sala>)serializer.Deserialize(filestream);
                filestream.Close();
                return sale;
            }
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
   
        public static void sacuvajIzmjene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Sala>));
            TextWriter filestream = new StreamWriter("sale.xml");
            serializer.Serialize(filestream, sale);
            filestream.Close();
        }

      public static List<Sala> sale = new List<Sala>();
   }
}