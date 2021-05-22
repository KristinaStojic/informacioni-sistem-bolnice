using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Projekat.Model;

namespace Projekat.Servis
{
    public class LekariServis
    {
        #region Lekari
        public static void DodajLekara(Lekar noviLekar)
        {
            LekariMenadzer.DodajLekara(noviLekar);
        }

        public static void IzmeniLekara(Lekar stariLekar, Lekar noviLekar)
        {
            LekariMenadzer.IzmeniLekara(stariLekar, noviLekar);
        }

        public static void ObrisiLekara(Lekar lekar)
        {
            LekariMenadzer.ObrisiLekara(lekar);
        }

        public static List<Lekar> NadjiSveLekare()
        {
            return LekariMenadzer.NadjiSveLekare();
        }

        public static Lekar NadjiPoId(int id)
        {
            return LekariMenadzer.NadjiPoId(id);
        }

        public static int GenerisanjeIdLekara()
        {
            return LekariMenadzer.GenerisanjeIdLekara();
        }

        public static int GenerisanjeIdZahtevaZaOdmor(int idLekara)
        {
            return LekariMenadzer.GenerisanjeIdZahtevaZaOdmor(idLekara);
        }

        public static void SacuvajIzmeneLekara()
        {
            LekariMenadzer.SacuvajIzmeneLekara();
        }

        // TODO: problem - moguce je da neki pacijent i neki lekar imaju isti jmbg a nisu ista osoba
        public static bool JedinstvenJmbg(long jmbg)
        {
            foreach (Lekar lekar in LekariMenadzer.lekari)
            {
                if (lekar.Jmbg == jmbg)
                {
                    return false;
                }
            }
            return true;
        }

        public static List<Lekar> PronadjiLekarePoSpecijalizaciji(Specijalizacija tipSpecijalizacije)
        {
            return LekariMenadzer.PronadjiLekarePoSpecijalizaciji(tipSpecijalizacije);
        }

        public static void DodajZahtev(ZahtevZaGodisnji zahtev)
        {
            LekariMenadzer.DodajZahtev(zahtev);
        }

        public static List<ZahtevZaGodisnji> NadjiSveZahteve()
        {
            return LekariMenadzer.NadjiSveZahteve();
        }

        public static ZahtevZaGodisnji NadjiZahtevPoId(int id)
        {
            return LekariMenadzer.NadjiZahtevPoId(id);
        }

        public static void sacuvajIzmjeneZahteva()
        {
            LekariMenadzer.sacuvajIzmjeneZahteva();
        }
        #endregion
    }
}
