using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    public class MalicioznoPonasanjeServis : IMalicioznoPonasanje
    {
        public void sacuvajIzmene()
        {
            MalicioznoPonasanjeMenadzer.sacuvajIzmene();
        }

        public List<MalicioznoPonasanje> NadjiSvaMalicioznaPonasanja()
        {
            return MalicioznoPonasanjeMenadzer.NadjiSvaMalicioznaPonasanja();
        }

        public void DodajMalicioznoPonasanje(int idPacijent)
        {
            MalicioznoPonasanjeMenadzer.DodajMalicioznoPonasanje(idPacijent);
        }
    }
}
