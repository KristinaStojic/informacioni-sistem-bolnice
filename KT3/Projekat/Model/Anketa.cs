using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class Anketa
    {
        public int idAnkete { get; set; }
        public int idPacijenta { get; set; }
        public int redniBrojPitanja { get; set; }
        public string tekstPitanja { get; set; }
        public int odgovorNaPitanje { get; set; }

        public Anketa() { }

        public Anketa(int idAnkete, int idPacijenta, int rbrPitanja, string pitanje, int odgovor ) {
            this.idAnkete = idAnkete;
            this.idPacijenta = idPacijenta;
            this.redniBrojPitanja = rbrPitanja;
            this.tekstPitanja = pitanje;
            this.odgovorNaPitanje = odgovor;
        }
    }
}
