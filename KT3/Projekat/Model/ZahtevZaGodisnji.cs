using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class ZahtevZaGodisnji
    {
        public int idZahteva { get; set; }
        public Lekar lekar { get; set; }
        public string pocetakOdmora {get;set;}
        public string krajOdmora {get;set;}

        public int brojDanaOdmora { get; set;}

        public string napomena { get; set;}

        public bool odobren { get; set; }

        public ZahtevZaGodisnji() { }

        public ZahtevZaGodisnji(int id,Lekar lekar, string pocetak,string kraj, int dani, string napomena)
        {
            this.idZahteva = id;
            this.lekar = lekar;
            this.pocetakOdmora = pocetak;
            this.krajOdmora = kraj;
            this.brojDanaOdmora = dani;
            this.napomena = napomena;
        }

        public override string ToString()
        {
            if (odobren == true)
                return "DA";
            else
                return "NE";
        }
    }
}
