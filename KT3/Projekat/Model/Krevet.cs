using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class Krevet
    {
        public int IdKreveta { get; set; }
        public int IdSobe { get; set; }
        public bool Zauzet { get; set; }
        public string PocetakZauzeca { get; set; }
        public string KrajZauzeca { get; set; }
        public Krevet() { }

        public Krevet(int idKreveta)
        {
            this.IdKreveta = idKreveta;
        }

        public Krevet(int idKreveta, int idSobe, bool zauzet)
        {
            this.IdKreveta = idKreveta;
            this.IdSobe = IdSobe;
            this.Zauzet = zauzet;
        }
    }
}
