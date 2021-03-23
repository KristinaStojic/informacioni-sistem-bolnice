/***********************************************************************
 * Module:  Pregledi.cs
 * Author:  Kristina
 * Purpose: Definition of the Class Pregledi
 ***********************************************************************/

using System;

namespace Model
{
   public class Pregledi
   {
      public int IdPregleda { get; set; }
      public DateTime VremePocetka { get; set; }
      public DateTime VremeKraja { get; set; }

      public Lekar lekar { get; set; }
      public Pacijent pacijent { get; set; }

    }
}