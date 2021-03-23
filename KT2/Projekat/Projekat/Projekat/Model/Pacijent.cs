/***********************************************************************
 * Module:  NalogPacijenta.cs
 * Author:  Teodora
 * Purpose: Definition of the Class NalogPacijenta
 ***********************************************************************/

using System;

namespace Model
{
   public class Pacijent
   {
      public string ImePacijenta { get; set; }
      public string PrezimePacijenta { get; set; }
      public int Jmbg { get; set; }
      public int IdKartona { get; set; }
      public Lekar lekar { get; set; }

    }
}