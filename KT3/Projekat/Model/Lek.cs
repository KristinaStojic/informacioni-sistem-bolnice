using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class Lek
    {
        public int idLeka { get; set; }
        public string nazivLeka { get; set; }
        public string sifraLeka { get; set; }

        public List<int> zamenskiLekovi { get; set; }
        public List<Sastojak> sastojci { get; set; }

        public Lek(int id, string naziv, string sifra)
        {
            this.idLeka = id;
            this.nazivLeka = naziv;
            this.sifraLeka = sifra;
            this.zamenskiLekovi = new List<int>();
            this.sastojci = new List<Sastojak>();
        }
        public Lek(int id, string naziv, string sifra, List<int> zamenskiLekovi, List<Sastojak> sastojci)
        {
            this.idLeka = id;
            this.nazivLeka = naziv;
            this.sifraLeka = sifra;
            this.zamenskiLekovi = zamenskiLekovi;
            this.sastojci = sastojci;
        }

        public Lek() { }
    }
}
