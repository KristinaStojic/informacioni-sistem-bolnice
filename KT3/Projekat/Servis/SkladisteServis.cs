using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    public class SkladisteServis
    {
        public static void azurirajOpremuUSkladistu()
        {
            foreach (Sala sala in SaleServis.Sale())
            {
                if (sala.Namjena.Equals("Skladiste"))
                {
                    sala.Oprema = OpremaServis.Oprema();
                }
            }
        }
        public static void EvidentirajUtrosenuOpremu(Oprema oprema)
        {
            foreach (Oprema opremaSkladista in OpremaServis.Oprema())
            {
                if (oprema.NazivOpreme.Equals(opremaSkladista.NazivOpreme))
                {
                    opremaSkladista.Kolicina -= oprema.Kolicina;
                    if (opremaSkladista.Kolicina == 0)
                    {
                        OpremaServis.Oprema().Remove(oprema);
                    }
                }
            }
        }
        public static int GenerisanjeIdOpreme()
        {
            int id;
            for (id = 1; id <= OpremaMenadzer.NadjiSve("oprema.xml").Count; id++)
            {
                if (!postojiIdOpreme(id))
                {
                    return id;
                }
            }
            return id;
        }

        private static bool postojiIdOpreme(int id)
        {
            foreach (Oprema o in OpremaMenadzer.NadjiSve("oprema.xml"))
            {
                if (o.IdOpreme.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }
    }
    
}
