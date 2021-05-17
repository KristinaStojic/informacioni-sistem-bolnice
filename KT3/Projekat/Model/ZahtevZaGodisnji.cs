using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public enum StatusZahteva { NA_CEKANJU, ODOBREN, ODBIJEN }
    public class ZahtevZaGodisnji
    {
        public int idZahteva { get; set; }
        public Lekar lekar { get; set; }
        public string pocetakOdmora {get;set;}
        public string krajOdmora {get;set;}

        public int brojDanaOdmora { get; set;}

        public string napomena { get; set;}

        public StatusZahteva odobren { get; set; }

        public ZahtevZaGodisnji() { }

        public ZahtevZaGodisnji(int id,Lekar lekar, string pocetak,string kraj, int dani, string napomena)
        {
            this.idZahteva = id;
            this.lekar = lekar;
            this.pocetakOdmora = pocetak;
            this.krajOdmora = kraj;
            this.brojDanaOdmora = dani;
            this.napomena = napomena;
            this.odobren = StatusZahteva.NA_CEKANJU;
        }

        public override string ToString()
        {
            if (odobren.Equals(StatusZahteva.ODOBREN))
                return "DA";
            else if (odobren.Equals(StatusZahteva.ODBIJEN))
                return "NE";
            else
                return "PROCESIRANJE_ZAHTEVA";
        }
    }
}
