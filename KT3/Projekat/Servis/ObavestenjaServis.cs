using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projekat.Model;

namespace Projekat.Servis
{
    public class ObavestenjaServis
    {
        public static void sacuvajIzmene()
        {
            ObavestenjaMenadzer.sacuvajIzmene();
        }

        public static List<Obavestenja> NadjiSvaObavestenja()
        {
            return ObavestenjaMenadzer.NadjiSvaObavestenja();
        }

        public static void DodajObavestenje(Obavestenja novoObavestenje)
        {
            ObavestenjaMenadzer.DodajObavestenje(novoObavestenje);
        }

        public static void IzmeniObavestenje(Obavestenja staroObavestenje, Obavestenja novoObavestenje)
        {
            ObavestenjaMenadzer.IzmeniObavestenje(staroObavestenje, novoObavestenje);
        }

        public static void ObrisiObavestenje(Obavestenja obavestenje)
        {
            ObavestenjaMenadzer.ObrisiObavestenje(obavestenje);
        }

        public static Obavestenja PronadjiPoId(int id)
        {
            return ObavestenjaMenadzer.PronadjiPoId(id);
        }

        public static int GenerisanjeIdObavestenja()
        {
            return ObavestenjaMenadzer.GenerisanjeIdObavestenja();
        }

        public static void ObrisiObavestenjePacijent(Obavestenja selektovanoObavestenje)
        {
            ObavestenjaMenadzer.ObrisiObavestenjePacijent(selektovanoObavestenje);
        }

        public static List<Obavestenja> PronadjiObavestenjaPoIdPacijenta(int idPacijent)
        {
            return ObavestenjaMenadzer.PronadjiObavestenjaPoIdPacijenta(idPacijent);
        }

    }
}
