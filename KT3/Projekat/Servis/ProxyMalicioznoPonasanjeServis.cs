using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    class ProxyMalicioznoPonasanjeServis : IMalicioznoPonasanje
    {
        public static int maksBrojMalicioznogPonasanjaPoDanu = 3;
        public bool DetektujMalicioznoPonasanje(int idPacijenta)
        {
            int brojacMalicioznogPonasanja = 0;
            MalicioznoPonasanjeServis malicioznoPonasanjeServis = new MalicioznoPonasanjeServis();
            List<MalicioznoPonasanje> malicioznaPonasanja = malicioznoPonasanjeServis.NadjiSvaMalicioznaPonasanja();
            foreach (MalicioznoPonasanje ponasanje in malicioznaPonasanja)
            {
                if (ponasanje.IdPacijenta == idPacijenta && ponasanje.DatumModifikacije == DateTime.Now.Date)
                {
                    brojacMalicioznogPonasanja++;
                    if (brojacMalicioznogPonasanja == maksBrojMalicioznogPonasanjaPoDanu)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public void DodajMalicioznoPonasanje(int idPacijent)
        {
            MalicioznoPonasanjeServis servis = new MalicioznoPonasanjeServis();
            servis.DodajMalicioznoPonasanje(idPacijent);
        }

        public List<MalicioznoPonasanje> NadjiSvaMalicioznaPonasanja()
        {
            MalicioznoPonasanjeServis malicioznoPonasanjeServis = new MalicioznoPonasanjeServis();
            return malicioznoPonasanjeServis.NadjiSvaMalicioznaPonasanja();
        }

        public void sacuvajIzmene()
        {
            MalicioznoPonasanjeServis malicioznoPonasanjeServis = new MalicioznoPonasanjeServis();
            malicioznoPonasanjeServis.sacuvajIzmene();
        }
    }
}
