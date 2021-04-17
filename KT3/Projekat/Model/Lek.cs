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



        public Lek(int id, string naziv, string sifra)
        {
            this.idLeka = id;
            this.nazivLeka = naziv;
            this.sifraLeka = sifra;
        }

        public Lek() { }
    }
}
