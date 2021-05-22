using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class RadniDan
    {
        public int IdLekara { get; set; }
        public string Datum { get; set; }
        public string VremePocetka { get; set; }
        public string VremeKraja { get; set; }
        public bool NaGodisnjemOdmoru { get; set; }

        public RadniDan() { }

        public RadniDan(int Id, string Datum, string VremePocetka, string VremeKraja) 
        {
            this.IdLekara = Id;
            this.Datum = Datum;
            this.VremePocetka = VremePocetka;
            this.VremeKraja = VremeKraja;
            this.NaGodisnjemOdmoru = false;
        }
    }
}
