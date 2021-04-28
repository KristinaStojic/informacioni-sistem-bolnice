using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public enum VrstaAnkete { ZaLekare, ZaKliniku }
    public class Anketa
    {
        public int idAnkete;
        public int idPacijent;
        public string odgovori;
        public VrstaAnkete nazivAnkete;

        public Anketa() { }

        public Anketa(int idAnkete, VrstaAnkete nazivAnkete, int idPacijent, string pitanjaOdgovor)
        {
            this.idAnkete = idAnkete;
            this.nazivAnkete = nazivAnkete;
            this.idPacijent = idPacijent;
            this.odgovori = pitanjaOdgovor;
        }


    }
}
