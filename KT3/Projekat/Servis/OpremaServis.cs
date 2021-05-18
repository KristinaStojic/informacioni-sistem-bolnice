using Model;
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

        public static bool portojiOpremaZaPrebacivanje(Oprema oprema, List<Oprema> opremaZaPrebacivanje)
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

        private static bool postojiOpremaUSali(Sala sala, Oprema oprema)
        {
            foreach (Oprema opremaSale in sala.Oprema)
            {
                if (opremaSale.IdOpreme == oprema.IdOpreme)
                {
                    opremaSale.Kolicina += oprema.Kolicina;
                    return true;
                }
            }
            return false;
        }

        public static void dodajOpremuIzSaleZaDodavanje(Sala izabranaSala, Sala salaZaSpajanje)
        {
            foreach (Sala sala in SaleServis.Sale())
            {
                if (sala.Id == izabranaSala.Id)
                {
                    dodajOpremu(sala, salaZaSpajanje);
                }
            }
        }

        private static void dodajOpremu(Sala sala, Sala salaZaSpajanje)
        {
            foreach (Oprema oprema in salaZaSpajanje.Oprema)
            {
                if (!postojiOpremaUSali(sala, oprema))
                {
                    sala.Oprema.Add(oprema);
                }
            }
        }

        
    }
}
