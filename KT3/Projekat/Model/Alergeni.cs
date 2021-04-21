using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class Alergeni
    {
        public int IdAlergena { get; set; }
        public int IdPacijenta { get; set; }
        public string NazivLeka { get; set; }
        public string SifraLeka { get; set; }
        public string NuspojavaNaLek { get; set; }
        public string VremeReakcije { get; set; }

        public Alergeni() { }

        public Alergeni(int idAlergena, int idPacijenta, string nazivLeka, string sifraLeka, string nuspojava, string vremeReakcije)
        {
            this.IdAlergena = idAlergena;
            this.IdPacijenta = idPacijenta;
            this.NazivLeka = nazivLeka;
            this.SifraLeka = sifraLeka;
            this.NuspojavaNaLek = nuspojava;
            this.VremeReakcije = vremeReakcije;
        }  
        
        public Alergeni(int idAlergena, string nazivLeka, string sifraLeka)
        {
            this.IdAlergena = idAlergena;  
            this.NazivLeka = nazivLeka;
            this.SifraLeka = sifraLeka;
        }
    }
}
