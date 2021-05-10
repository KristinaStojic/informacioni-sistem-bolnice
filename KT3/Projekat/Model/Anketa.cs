using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public enum VrstaAnkete { ZaLekare, ZaKliniku }
    public class Anketa
    {
        public int IdAnkete { get; set; }
        public int IdPacijent { get; set; }
        public string Odgovori { get; set; }
        public VrstaAnkete VrstaAnkete { get; set; }
        public string NazivAnkete { get; set; }
        public int IdTermina { get; set; } 
        public bool PopunjenaAnketa { get; set; }

        public Anketa() { }

        public Anketa(VrstaAnkete vrsta, string nazivAnkete, int idPacijent, int idTermina)
        {
            this.IdAnkete = AnketaMenadzer.GenerisanjeIdAnkete();
            this.IdPacijent = idPacijent;
            this.Odgovori = "";
            this.VrstaAnkete = vrsta;
            this.NazivAnkete = nazivAnkete;
            this.IdTermina = idTermina;
            this.PopunjenaAnketa = false;
        }


    }
}
