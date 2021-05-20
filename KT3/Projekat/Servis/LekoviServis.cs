using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    public class LekoviServis
    {
        public static void DodajLijek(Lek lijek)
        {
            LekoviMenadzer.DodajLijek(lijek);
        }
        public static void dodajZamjenskeLijekove(Lek izabraniLijek, List<Lek> zamjenskiLijekovi)
        {
            LekoviMenadzer.dodajZamjenskeLijekove(izabraniLijek, zamjenskiLijekovi);
        }
        public static void obrisiLijek(Lek lijek)
        {
            LekoviMenadzer.obrisiLijek(lijek);
        }
        public static void izmjeniLijek(Lek izabraniLijek, Lek izmjenjeniLijek)
        {
            LekoviMenadzer.izmjeniLijek(izabraniLijek, izmjenjeniLijek);
        }
        public static void IzmjeniOdbijeniLijek(Lek izabraniLijek, Lek uLijek)
        {
            LekoviMenadzer.IzmjeniOdbijeniLijek(izabraniLijek, uLijek);
        }
        public static void obrisiSastojakLijeka(Lek izabraniLijek, Sastojak sastojak)
        {
            LekoviMenadzer.obrisiSastojakLijeka(izabraniLijek, sastojak);
        }
        public static void izmjeniSastojakLijeka(Lek izabraniLijek, Sastojak stariSastojak, Sastojak noviSastojak)
        {
            LekoviMenadzer.izmjeniSastojakLijeka(izabraniLijek, stariSastojak, noviSastojak);
        }
        public static void izmjeniSastojakOdbijenogLijeka(Lek izabraniLijek, Sastojak izabraniSastojak, Sastojak uSastojak)
        {
            LekoviMenadzer.izmjeniSastojakOdbijenogLijeka(izabraniLijek, izabraniSastojak, uSastojak);
        }
        public static void obrisiZamjenski(Lek izabraniLijek, Lek zamjenskiLijek)
        {
            LekoviMenadzer.obrisiZamjenski(izabraniLijek, zamjenskiLijek);
        }
        public static List<Lek> NadjiSveLijekove()
        {
            return LekoviMenadzer.NadjiSveLijekove();
        }

        public static List<Lek> Lijekovi()
        {
            return LekoviMenadzer.lijekovi;
        }

        public static void sacuvajIzmjene()
        {
            LekoviMenadzer.sacuvajIzmjene();
        }
        public static int GenerisanjeIdLijeka()
        {
            return LekoviMenadzer.GenerisanjeIdLijeka();
        }
        public static void dodajSastojak(Sastojak sastojak, Lek izabraniLijek)
        {
            LekoviMenadzer.dodajSastojak(sastojak, izabraniLijek);
        }
        public static void dodajZahtjev(Lek lijek)
        {
            ZahtevZaLekove zahtjev = new ZahtevZaLekove(LekoviMenadzer.GenerisanjeIdZahtjeva(), lijek, DateTime.Now.Date.ToString("d"), false);
            LekoviMenadzer.zahteviZaLekove.Add(zahtjev);
            LekoviMenadzer.sacuvajIzmeneZahteva();
            LekoviMenadzer.sacuvajIzmeneZahteva();
        }
    }
}
