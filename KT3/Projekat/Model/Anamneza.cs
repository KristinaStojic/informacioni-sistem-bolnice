using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class Anamneza
    {
        public int IdAnamneze { get; set; }
        public int IdPacijenta { get; set; }
        public int IdLekara { get; set; }

        public string OpisBolesti { get; set; }

        public string Terapija { get; set; }
        public string Datum { get; set; }
        /*TO DO: DODAJ KONSTRUKTOR SA LEKAROM*/

        public Anamneza(int id,int p,string dat,string bolest, string terapija)
        {
            this.IdAnamneze = id;
            this.IdPacijenta = p;
            this.Datum = dat;
            this.OpisBolesti = bolest;
            this.Terapija = terapija;
        }

        public Anamneza () { }
        
        public Anamneza (string ter, string bol)
        {
            this.Terapija = ter;
            this.OpisBolesti = bol;
        }
    }
}
