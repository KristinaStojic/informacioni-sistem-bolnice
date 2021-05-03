using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class MalicioznoPonasanje
    {
        public int IdMalicioznogPonasanja { get; set; }
        public int IdPacijenta {get; set;}
        public DateTime DatumModifikacije { get; set; }

        public MalicioznoPonasanje() { }

        public MalicioznoPonasanje(int idPacijenta)
        {
            this.IdMalicioznogPonasanja = MalicioznoPonasanjeMenadzer.GenerisanjeIdMalicioznogPonasanja();
            this.IdPacijenta = idPacijenta;
            this.DatumModifikacije = DateTime.Now.Date;
        }

    }
}
