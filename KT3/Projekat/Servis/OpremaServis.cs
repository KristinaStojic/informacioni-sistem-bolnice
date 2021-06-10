using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;

namespace Projekat.Servis
{
    public class OpremaServis
    {

        public static void izmjeniOpremu(Oprema izOpreme, Oprema uOpremu)
        { 
            OpremaMenadzer.Instanca.Izmjeni(izOpreme, uOpremu); 
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

        private static bool postojiOprema(Sala sala, Oprema izabranaOprema)
        {
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    return true;
                }
            }
            return false;
        }

        public static void premjestiStatickuOpremu(Sala uSalu, int kolicina, Sala izSale, Oprema izabranaOprema)
        {
            foreach (Sala sala in SaleServis.Sale())
            {
                if (sala.Id == izSale.Id)
                {
                    IzmjenaKolicineOpremeServis.SmanjiKolicinuOpreme(sala, kolicina, izabranaOprema);
                }
                if (sala.Id == uSalu.Id)
                {
                    dodajStatickuOpremuUSalu(sala, kolicina, izabranaOprema, uSalu);

                }
            }
        }

        private static void dodajStatickuOpremuUSalu(Sala sala, int kolicina, Oprema izabranaOprema, Sala salaDodavanje)
        {
            if (!postojiOprema(sala, izabranaOprema))
            {
                sala.Oprema.Add(NapraviNovuOpremu(kolicina, izabranaOprema));
            }
            else
            {
                IzmjenaKolicineOpremeServis.PovecajKolicinuOpreme(sala, kolicina, izabranaOprema);
            }
        }

        

        public static void dodajDinamickuOpremuUSalu(Sala sala, int kolicina, Oprema izabranaOprema)
        {
            if (!postojiOprema(sala, izabranaOprema))
            {
                IzmjenaKolicineOpremeServis.PovecajKolicinuOpreme(sala, kolicina, izabranaOprema);
            }
            else
            {
                sala.Oprema.Add(NapraviNovuOpremu(kolicina, izabranaOprema));
            }
        }

    }
}
