using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public enum VrstaAnkete { ZaLekare, ZaKliniku }
    public class Anketa
    {
        public int idAnkete { get; set; }
        public int idPacijent { get; set; }
        public string odgovori { get; set; }
        public VrstaAnkete vrstaAnkete { get; set; }
        public string nazivAnkete { get; set; }
        public int idTermina { get; set; } 
        public bool popunjenaAnketa { get; set; }

        public Anketa() { }

        public Anketa(int idAnkete, VrstaAnkete vrsta, string nazivAnkete, int idPacijent, int idTermina)
        {
            this.idAnkete = idAnkete;
            this.idPacijent = idPacijent;
            this.odgovori = "";
            this.vrstaAnkete = vrsta;
            this.nazivAnkete = nazivAnkete;
            this.idTermina = idTermina;
            this.popunjenaAnketa = false;
        }


    }
}
