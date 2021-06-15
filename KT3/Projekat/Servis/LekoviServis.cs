using Model;
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

        public static List<ZahtevZaLekove> ZahtjeviZaLijekove()
        {
            return LekoviMenadzer.zahteviZaLekove;
        }
        public static void SacuvajIzmeneZahteva()
        {
            LekoviMenadzer.sacuvajIzmeneZahteva();
        }
        public static void dodajZamjenskeLijekove(Lek izabraniLijek, Lek zamjenskiLijekovi)
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
            ZahtevZaLekove zahtjev = new ZahtevZaLekove(LekoviServis.GenerisanjeIdZahtjeva(), lijek, DateTime.Now.Date.ToString("d"), false);
            LekoviMenadzer.zahteviZaLekove.Add(zahtjev);
            LekoviMenadzer.sacuvajIzmeneZahteva();
            LekoviMenadzer.sacuvajIzmeneZahteva();
        }

        public static void IzmeniLekoveLekar(Lek izabraniLek, Lek izmenjeniLek)
        {
            LekoviMenadzer.IzmeniLekoveLekar(izabraniLek, izmenjeniLek);
        }

        public static void obrisiSastojakLekaLekar(Lek izabraniLek, Sastojak sastojak)
        {
            LekoviMenadzer.obrisiSastojakLekaLekar(izabraniLek, sastojak);
        }

        public static void izmeniSastojakLekaLekar(Lek izabraniLek, Sastojak stariSastojak, Sastojak noviSastojak)
        {
            LekoviMenadzer.izmeniSastojakLekaLekar(izabraniLek, stariSastojak, noviSastojak);
        }

        public static void obrisiZamenskiLekLekar(Lek izabraniLek, Lek zamenskiLek)
        {
            LekoviMenadzer.obrisiZamenskiLekLekar(izabraniLek, zamenskiLek);
        }

        public static List<Sastojak> nadjiSastojke(ZahtevZaLekove izabraniZahtev)
        {
            return LekoviMenadzer.nadjiSastojke(izabraniZahtev);
        }

        public static void izmeniZahtev(ZahtevZaLekove izabraniZahtev)
        {
            LekoviMenadzer.izmeniZahtev(izabraniZahtev);
        }

        public static void odbijaZahtev(ZahtevZaLekove izabraniZahtev, String razlogOdbijanja)
        {
            LekoviMenadzer.odbijaZahtev(izabraniZahtev, razlogOdbijanja);
        }

        public static List<Sastojak> NadjiSveSastojke()
        {
            return LekoviMenadzer.NadjiSveSastojke();
        }

        public static int GenerisanjeIdZahtjeva()
        {
            return LekoviMenadzer.GenerisanjeIdZahtjeva();
        }

        public static List<ZahtevZaLekove> NadjiSveZahteve()
        {
            return LekoviMenadzer.NadjiSveZahteve();
        }

        public static void sacuvajIzmeneZahteva()
        {
            LekoviMenadzer.sacuvajIzmeneZahteva();
        }
        public static void ukloniZahtjev(ZahtevZaLekove izabraniZahtjev)
        {
            LekoviMenadzer.zahteviZaLekove.Remove(izabraniZahtjev);
        }

        public static int GenerisanjeIdKreveta(int idSobe)
        {
            bool pomocna = false;
            int id = 1;
            foreach (Sala sala in SaleMenadzer.lista)
            {
                if (sala.Id == idSobe && sala.Kreveti != null)
                {
                    for (id = 1; id <= sala.Kreveti.Count; id++)
                    {
                        foreach (Krevet k in sala.Kreveti)
                        {
                            if (k.IdKreveta == id)
                            {
                                pomocna = true;
                                break;
                            }
                        }

                        if (!pomocna)
                        {
                            return id;
                        }
                        pomocna = false;
                    }
                }
            }
            return id;
        }

    }
}
