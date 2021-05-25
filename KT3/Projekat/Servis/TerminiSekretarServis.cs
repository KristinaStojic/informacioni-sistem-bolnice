using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace Projekat.Servis
{
    public class TerminiSekretarServis
    {
        #region Termini Sekretar Menadzer
       
        public static void ZakaziTerminSekretar(Termin termin)
        {
            TerminMenadzer.ZakaziTerminSekretar(termin);
        }

        public static void IzmeniTerminSekretar(Termin stariTermin, Termin noviTermin)
        {
            TerminMenadzer.IzmeniTerminSekretar(stariTermin, noviTermin);
        }

        public static void OtkaziTerminSekretar(Termin termin)
        {
            TerminMenadzer.OtkaziTerminSekretar(termin);
        }

        public static int GenerisanjeIdTermina()
        {
            return TerminMenadzer.GenerisanjeIdTermina();
        }

        public static List<Termin> NadjiSveTermine()
        {
            return TerminMenadzer.NadjiSveTermine();
        }

        public static Termin NadjiTerminPoId(int idTermin)
        {
            return TerminMenadzer.NadjiTerminPoId(idTermin);
        }

        public static void sacuvajIzmene()
        {
            TerminMenadzer.sacuvajIzmene();
        }

        #endregion


        #region Zakazi Hitan Termin

        public static void ZakaziHitanTermin(Termin hitanTermin, string datum)
        {
            TerminMenadzer.ZakaziHitanTermin(hitanTermin, datum);
        }

        public static void DodajZauzeceUSveSale(Sala sala)
        {
            TerminMenadzer.DodajZauzeceUSveSale(sala);
        }

        #endregion
    }
}
