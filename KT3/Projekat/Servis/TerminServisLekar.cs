using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    public class TerminServisLekar
    {
        public static void ZakaziTerminLekar(Termin termin)
        {
            TerminMenadzer.ZakaziTerminLekar(termin);
        }

        public static void IzmeniTerminLekar(Termin termin, Termin termin1)
        {
            TerminMenadzer.IzmeniTerminLekar(termin, termin1);
        }

        public static void OtkaziTerminLekar(Termin termin)
        {
            TerminMenadzer.OtkaziTerminLekar(termin);
        }

        public static Termin NadjiTerminPoId(int idTermin)
        {
            return TerminMenadzer.NadjiTerminPoId(idTermin);
        }

        public static void sacuvajIzmene()
        {
            TerminMenadzer.sacuvajIzmene();
        }

        public static int GenerisanjeIdTermina()
        {
            return TerminMenadzer.GenerisanjeIdTermina();
        }

    }
}
