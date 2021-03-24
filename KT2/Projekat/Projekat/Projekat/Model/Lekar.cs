/***********************************************************************
 * Module:  Lekar.cs
 * Author:  Kristina
 * Purpose: Definition of the Class Lekar
 ***********************************************************************/

using System;

namespace Model
{
   public class Lekar
   {
      public int IdLekara { get; set; }
      public string Ime { get; set; }
      public string Prezime { get; set; }

    public Lekar(int id, string ime, string prz)
        {
            this.IdLekara = id;
            this.Ime = ime;
            this.Prezime = prz;
        }

    public Lekar(int id)
        {
            this.IdLekara = id;
        }

    }
}