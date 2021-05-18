using Model;
using Projekat.Servis;
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
        public DateTime PocetakZauzeca { get; set; }
        public DateTime KrajZauzeca { get; set; }
        public Krevet() { }

        public Krevet(int idKreveta)
        {
            this.IdKreveta = idKreveta;
        }

        public Krevet(int idSobe, bool zauzet)
        {
            this.IdKreveta = OpremaMenadzer.GenerisanjeIdKreveta(idSobe);
            this.IdSobe = idSobe;
            this.Zauzet = zauzet;
        }
    }
}
