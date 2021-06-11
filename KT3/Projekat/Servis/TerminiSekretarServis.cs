using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Model;

namespace Projekat.Servis
{
    public class TerminiSekretarServis
    {
        #region Termini Sekretar Menadzer
        TerminMenadzer m = new TerminMenadzer();
       
        public  void ZakaziTerminSekretar(Termin termin)
        {
            m.ZakaziTerminSekretar(termin);
        }

        public  void IzmeniTerminSekretar(Termin stariTermin, Termin noviTermin)
        {
            m.IzmeniTerminSekretar(stariTermin, noviTermin);
        }

        public  void OtkaziTerminSekretar(Termin termin)
        {
            m.OtkaziTerminSekretar(termin);
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

        public void ZakaziHitanTermin(Termin hitanTermin, string datum)
        {
            m.ZakaziHitanTermin(hitanTermin, datum);
        }

        #endregion
    }
}
