using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace Projekat.Servis
{
    public class PacijentiServis
    {
        public static void DodajNalog(Pacijent noviNalog)
        {
            PacijentiMenadzer.DodajNalog(noviNalog);
        }

        public static void IzmeniNalog(Pacijent stariNalog, Pacijent noviNalog)
        {
            PacijentiMenadzer.IzmeniNalog(stariNalog, noviNalog);
        }

        // Sanja
        public static void IzmeniNalogPacijent(Pacijent stari, Pacijent nalog)
        {
            PacijentiMenadzer.IzmeniNalogPacijent(stari, nalog);
        }

        public static void ObrisiNalog(Pacijent nalog)
        {
            PacijentiMenadzer.ObrisiNalog(nalog);
        }

        public static List<Pacijent> PronadjiSve()
        {
            return PacijentiMenadzer.PronadjiSve();
        }

        public static Pacijent PronadjiPoId(int id)
        {
            return PacijentiMenadzer.PronadjiPoId(id);
        }

        public static int GenerisanjeIdPacijenta()
        {
            return PacijentiMenadzer.GenerisanjeIdPacijenta();
        }

        public static void SacuvajIzmenePacijenta()
        {
            PacijentiMenadzer.SacuvajIzmenePacijenta();
        }

        public static bool JedinstvenJmbg(int jmbg)
        {
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                if (p.Jmbg == jmbg)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
