using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class ZahtevZaLekove
    {
        public int idZahteva { get; set; }
        public int idLeka { get; set; }

        public String nazivLeka { get; set; }

        public String sifraLeka { get; set; }

        public String datumSlanjaZahteva { get; set; }

        public Boolean obradjenZahtev { get; set; }
        public Boolean odbijenZahtev { get; set; }

        public String obrazlozenjeOdbijanja { get; set; }

        public ZahtevZaLekove() { }

        public ZahtevZaLekove(int idZahteva, String nazivLeka,String sifraLeka, String datumSlanja, Boolean obradjen)
        {
            this.idZahteva = idZahteva;
            this.nazivLeka = nazivLeka;
            this.sifraLeka = sifraLeka;
            this.datumSlanjaZahteva = datumSlanja;
            this.obradjenZahtev = obradjen;
        }


        
    }
}
