using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    public class MalicioznoPonasanjeServis
    {
        public static void sacuvajIzmene()
        {
            MalicioznoPonasanjeMenadzer.sacuvajIzmene();
        }

        public static List<MalicioznoPonasanje> NadjiSvaMalicioznaPonasanja()
        {
            return MalicioznoPonasanjeMenadzer.NadjiSvaMalicioznaPonasanja();
        }

        public static bool DetektujMalicioznoPonasanje(int idPacijenta)
        {
            return MalicioznoPonasanjeMenadzer.DetektujMalicioznoPonasanje(idPacijenta);
        }

        public static void DodajMalicioznoPonasanje(int idPacijent)
        {
            MalicioznoPonasanjeMenadzer.DodajMalicioznoPonasanje(idPacijent);
        }
    }
}
