using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    public interface IMalicioznoPonasanje
    {
        void sacuvajIzmene();
        List<MalicioznoPonasanje> NadjiSvaMalicioznaPonasanja();
        void DodajMalicioznoPonasanje(int idPacijent);

        //bool DetektujMalicioznoPonasanje(int idPacijenta);

    }
}
