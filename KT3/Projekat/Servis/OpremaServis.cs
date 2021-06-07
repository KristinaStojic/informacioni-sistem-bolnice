using Model;
using Projekat.Model;
using System.Collections.Generic;

namespace Projekat.Servis
{
    public class OpremaServis
    {

        public static void izmjeniOpremu(Oprema izOpreme, Oprema uOpremu)
        {
            OpremaMenadzer om = new OpremaMenadzer();
            om.Izmjeni(izOpreme, uOpremu); 
        }

        public static List<Oprema> NadjiSvuOpremu()
        {
            return OpremaMenadzer.NadjiSve("oprema.xml");
        }

        public static List<Oprema> Oprema()
        {
            foreach(Oprema oprema in OpremaMenadzer.lista.ToArray())
            {
                if(oprema.Kolicina == 0)
                {
                    OpremaMenadzer.lista.Remove(oprema);
                }
            }
            return OpremaMenadzer.lista;
        }

        public static void DodajOpremu(Oprema oprema)
        {
            OpremaMenadzer.Dodaj(oprema, "oprema.xml");
            SkladisteServis.azurirajOpremuUSkladistu();
            OpremaMenadzer.sacuvajIzmjene("oprema.xml");
        }

        public static void sacuvajIzmjene()
        {
            OpremaMenadzer.sacuvajIzmjene("oprema.xml");
        }

        public static Oprema NapraviNovuOpremu(int kolicina, Oprema opremaZaSlanje)
        {
            Oprema oprema = new Oprema(opremaZaSlanje.NazivOpreme, kolicina, true);
            oprema.IdOpreme = opremaZaSlanje.IdOpreme;
            return oprema;
        }

    }
}
