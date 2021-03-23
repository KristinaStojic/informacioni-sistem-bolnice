/***********************************************************************
 * Module:  Operacije.cs
 * Author:  Kristina
 * Purpose: Definition of the Class Operacije
 ***********************************************************************/

using System;

namespace Model
{
   public class Operacije
   {
      public int IdOperacije { get; set; }
      public DateTime VremePocetka { get; set; }
      public DateTime VremeKraja { get; set; }

      public Sala sala { get; set; }
      public Lekar[] lekar { get; set; }
      public Pacijent pacijent { get; set; }

    }
}