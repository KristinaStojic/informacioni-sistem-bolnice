using Projekat.Model;
using System.Collections.Generic;

namespace Projekat.Servis
{
    public class OpremaServis
    {

        public static void izmjeniOpremu(Oprema izOpreme, Oprema uOpremu)
        {
            OpremaMenadzer.izmjeniOpremu(izOpreme, uOpremu);
        }

        public static List<Oprema> NadjiSvuOpremu()
        {
            return OpremaMenadzer.NadjiSvuOpremu();
        }

        public static List<Oprema> Oprema()
        {
            return OpremaMenadzer.oprema;
        }

        public static void DodajOpremu(Oprema oprema)
        {
            OpremaMenadzer.DodajOpremu(oprema);
        }

        public static void sacuvajIzmjene()
        {
            OpremaMenadzer.sacuvajIzmjene();
        }

        public static int GenerisanjeIdOpreme()
        {
            return OpremaMenadzer.GenerisanjeIdOpreme();
        }

        /*public static int GenerisanjeIdKreveta()
        {
            return OpremaMenadzer.GenerisanjeIdKreveta();
        }*/

        public static bool postojiOprema(Oprema oprema, List<Oprema> opremaZaPrebacivanje)
        {
            foreach (Oprema opremaPrebacivanje in opremaZaPrebacivanje)
            {
                if (oprema.IdOpreme == opremaPrebacivanje.IdOpreme)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
