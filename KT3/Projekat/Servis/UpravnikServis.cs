using Projekat.Model;
using System.Collections.Generic;

namespace Projekat.Servis
{
    public class UpravnikServis
    {
        public static List<UpravnikModel> NadjiSveUpravnike()
        {
            return UpravnikMenadzer.NadjiSveUpravnike();
        }

        public static void SacuvajIzmjene()
        {
            UpravnikMenadzer.SacuvajIzmjene();
        }


        public static void DodajUpravnika(UpravnikModel upravnik)
        {
            UpravnikMenadzer.DodajUpravnika(upravnik);
        }


    }
}
