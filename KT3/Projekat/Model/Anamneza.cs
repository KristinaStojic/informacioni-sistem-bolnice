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
        public int IdTermina { get; set; }
        public string Beleska { get; set; }


        public string ImePrezimeLekara { get; set; }

        /*TO DO: DODAJ KONSTRUKTOR SA LEKAROM*/

        public Anamneza(int id,int p,string dat,string bolest, string terapija, int idLek, int idTermina)
        {
            this.IdAnamneze = id;
            this.IdPacijenta = p;
            this.Datum = dat;
            this.OpisBolesti = bolest;
            this.Terapija = terapija;
            this.IdLekara = idLek;
            this.ImePrezimeLekara = ImePrzLekaraPostavi(idLek);
            this.IdTermina = idTermina;
            this.Beleska = "";
        }

        public Anamneza () { }
        
        public Anamneza (string ter, string bol)
        {
            this.Terapija = ter;
            this.OpisBolesti = bol;
        }
        
        
        public Anamneza (string ter, string bol, string datum)
        {
            this.Terapija = ter;
            this.OpisBolesti = bol;
            this.Datum = datum;
        }


        public String ImePrzLekaraPostavi(int idLekara)
        {
            String imeprz = null;

            foreach(Lekar l in LekariMenadzer.lekari)
            {
                if (l.IdLekara == idLekara)
                {

                    imeprz = l.ImeLek + " " + l.PrezimeLek;
                }
            }

            return imeprz;

        }
    }
}
