using Projekat.Model;
using System.Collections.Generic;

namespace Projekat.Servis
{
    public class UpravnikServis
    {
        public static List<UpravnikModel> NadjiSveUpravnike()
        {
            return UpravnikMenadzer.NadjiSve("upravnici.xml");
        }

        public static void SacuvajIzmjene()
        {
            UpravnikMenadzer.sacuvajIzmjene("upravnici.xml");
        }


        public static void DodajUpravnika(UpravnikModel upravnik)
        {
            UpravnikMenadzer.Dodaj(upravnik, "upravnici.xml");
        }


    }
}
