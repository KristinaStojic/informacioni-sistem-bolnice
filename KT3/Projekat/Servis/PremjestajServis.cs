using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    public class PremjestajServis
    {
        public static void dodajPremjestaj(Premjestaj premjestaj)
        {
            PremjestajMenadzer.dodajPremjestaj(premjestaj);
        }

        public static List<Premjestaj> NadjiSvePremjestaje()
        {
            return PremjestajMenadzer.NadjiSvePremjestaje();
        }

        public static List<Premjestaj> Premjestaji()
        {
            return PremjestajMenadzer.premjestaji;
        }

        public static void sacuvajIzmjene()
        {
            PremjestajMenadzer.sacuvajIzmjene(); 
        }

        private static bool odradiPremjestaj(Premjestaj premjestaj)
        {
            if (!premjestaj.datumIVrijeme.Date.ToString().Equals(DateTime.Now.Date.ToString()))
            {
                return false;
            }
            if (premjestaj.datumIVrijeme.TimeOfDay > DateTime.Now.TimeOfDay)
            {
                return false;
            }
            return true;
        }

        public static void odradiZakazanePremjestaje()
        {
            foreach (Premjestaj premjestaj in PremjestajMenadzer.premjestaji.ToList())
            {
                if (!odradiPremjestaj(premjestaj))
                {
                    continue;
                }

                izvrsiPremjestaj(premjestaj);
                PremjestajMenadzer.premjestaji.Remove(premjestaj);
                sacuvajIzmjene();

            }
        }

        private static void izvrsiPremjestaj(Premjestaj premjestaj)
        {
            foreach (Sala sala in SaleServis.Sale())
            {
                if (sala.Id == premjestaj.izSale.Id)
                {
                    prebaciOpremuIzSale(sala, premjestaj.oprema, premjestaj.kolicina);
                }
                if (sala.Id == premjestaj.uSalu.Id)
                {
                    dodajOpremuUSalu(sala, premjestaj.oprema, premjestaj.kolicina, premjestaj.uSalu);
                }
            }
        }

        private static void dodajOpremuUSalu(Sala sala, Oprema izabranaOprema, int kolicina, Sala salaDodavanje)
        {
            if (!postojiOprema(sala, izabranaOprema, kolicina))
            {
                dodajNovuOpremu(izabranaOprema, sala, kolicina, salaDodavanje);

            }
        }

        private static bool postojiOprema(Sala sala, Oprema izabranaOprema, int kolicina)
        {
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    prebaciOpremu(oprema, kolicina, sala);
                    return true;
                }
            }
            return false;
        }

        private static void prebaciOpremu(Oprema oprema, int kolicina, Sala sala)
        {
            oprema.Kolicina += kolicina;
            if (sala.Namjena.Equals("Skladiste"))
            {
                zamjeniOpremuUSkladistu(oprema);
            }
            else
            {
                azurirajPrikazStaticke();
            }
        }

        private static void dodajNovuOpremu(Oprema izabranaOprema, Sala sala, int kolicina, Sala salaDodavanje)
        {
            Oprema oprema = new Oprema(izabranaOprema.NazivOpreme, kolicina, true);
            oprema.IdOpreme = izabranaOprema.IdOpreme;
            sala.Oprema.Add(oprema);
            if (salaDodavanje.Namjena.Equals("Skladiste"))
            {
                dodajOpremuUSkladiste(oprema);
            }
            else
            {
                azurirajPrikazStaticke();
            }
        }

        private static void azurirajPrikazStaticke()
        {

            if (PrikazStaticke.otvoren)
            {
                PrikazStaticke.azurirajPrikaz();
            }
        }

        private static void prebaciOpremuIzSale(Sala sala, Oprema izabranaOprema, int kolicina)
        {
            foreach (Oprema oprema in sala.Oprema.ToArray())
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    smanjiKolicinuOpreme(oprema, kolicina, sala);
                }
            }
        }

        private static void smanjiKolicinuOpreme(Oprema oprema, int kolicina, Sala sala)
        {
            oprema.Kolicina -= kolicina;
            if (sala.Namjena.Equals("Skladiste"))
            {
                izvrsiPremjestajUSkladistu(sala, oprema);
            }
            else
            {
                ukloniOpremuIzSale(oprema, sala);
            }
        }

        private static void ukloniOpremuIzSale(Oprema oprema, Sala sala)
        {
            if (oprema.Kolicina == 0)
            {
                sala.Oprema.Remove(oprema);
                azurirajPrikazStaticke();
            }
            azurirajPrikazStaticke();
        }

        private static void izvrsiPremjestajUSkladistu(Sala sala, Oprema oprema)
        {
            if (oprema.Kolicina == 0)
            {
                ukloniOpremuIzSkladista(sala, oprema);
            }
            else
            {
                zamjeniOpremuUSkladistu(oprema);
            }
        }

        private static void ukloniOpremuIzSkladista(Sala sala, Oprema oprema)
        {
            if (Skladiste.OpremaStaticka != null)
            {
                sala.Oprema.Remove(oprema);
                Skladiste.OpremaStaticka.Remove(oprema);
            }
        }

        private static void zamjeniOpremuUSkladistu(Oprema oprema)
        {
            if (Skladiste.OpremaStaticka != null)
            {
                int idx = Skladiste.OpremaStaticka.IndexOf(oprema);
                Skladiste.OpremaStaticka.RemoveAt(idx);
                Skladiste.OpremaStaticka.Insert(idx, oprema);
            }
        }

        private static void dodajOpremuUSkladiste(Oprema oprema)
        {
            if (Skladiste.OpremaStaticka != null)
            {
                Skladiste.OpremaStaticka.Add(oprema);
            }
        }

        public static int GenerisanjeIdPremjestaja()
        {
            return PremjestajMenadzer.GenerisanjeIdPremjestaja();
        }

        public static void izvrsiPremjestanje(Sala salaUKojuSaljem, int kolicina, Oprema opremaZaSlanje)
        {
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (sala.Namjena.Equals("Skladiste"))
                {
                    ukloniOpremuIzSale(sala, kolicina, opremaZaSlanje);
                }
                if (sala.Id == salaUKojuSaljem.Id)
                {
                    dodajOpremuUSalu(sala, kolicina, opremaZaSlanje);
                }
            }
        }

        private static void dodajOpremuUSalu(Sala sala, int kolicina, Oprema opremaZaSlanje)
        {
            bool postojiOprema = false;
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    oprema.Kolicina += kolicina;
                    postojiOprema = true;
                }
            }
            dodajNovuOpremu(postojiOprema, kolicina, sala, opremaZaSlanje);
        }

        private static void dodajNovuOpremu(bool postojiOprema, int kolicina, Sala sala, Oprema opremaZaSlanje)
        {
            if (!postojiOprema)
            {
                Oprema oprema = new Oprema(opremaZaSlanje.NazivOpreme, kolicina, false);
                oprema.IdOpreme = opremaZaSlanje.IdOpreme;
                sala.Oprema.Add(oprema);
            }
        }

        public static void ukloniOpremuIzSale(Sala sala, int kolicina, Oprema opremaZaSlanje)
        {
            foreach (Oprema oprema in sala.Oprema.ToArray())
            {
                if (oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    prebaciDinamickuOpremu(oprema, kolicina, sala);
                }
            }
        }

        private static void prebaciDinamickuOpremu(Oprema oprema, int kolicina, Sala sala)
        {
            if (oprema.Kolicina - kolicina == 0)
            {
                sala.Oprema.Remove(oprema);
            }
            else
            {
                oprema.Kolicina -= kolicina;
            }
        }

        public static void odradiPremjestaj(DateTime datumIVrijeme, Sala salaUKojuSaljem, int kolicina, Oprema opremaZaSlanje)
        {
            if (datumIVrijeme.Date.ToString().Equals(DateTime.Now.Date.ToString()))
            {
                odradiPremjestajSada(datumIVrijeme, salaUKojuSaljem, kolicina, opremaZaSlanje);
            }
            else
            {
                zakaziPremjestaj(salaUKojuSaljem, datumIVrijeme, kolicina, opremaZaSlanje);
            }
        }

        private static void zakaziPremjestaj(Sala salaUKojuSaljem, DateTime datumIVrijeme, int kolicina, Oprema opremaZaSlanje)
        {
            Premjestaj zakazi = new Premjestaj();
            zakazi.kolicina = kolicina;
            definisiElemente(zakazi, salaUKojuSaljem, opremaZaSlanje);
            if (zakazi.oprema == null)
            {
                nadjiOpremuUSalama(zakazi, opremaZaSlanje);
            }
            dodajPremjestaj(zakazi, datumIVrijeme);
        }

        private static void definisiElemente(Premjestaj zakazi, Sala salaUKojuSaljem, Oprema opremaZaSlanje)
        {
            foreach (Sala sala in SaleServis.Sale())
            {
                definisiSale(zakazi, sala, salaUKojuSaljem);
            }
            foreach (Oprema oprema in OpremaServis.Oprema())
            {
                definisiOpremu(zakazi, oprema, opremaZaSlanje);
            }
        }

        private static void dodajPremjestaj(Premjestaj zakazi, DateTime datumIVrijeme)
        {
            zakazi.datumIVrijeme = datumIVrijeme;
            PremjestajServis.dodajPremjestaj(zakazi);
        }

        private void zavrsiPremjestaj()
        {
            SaleServis.sacuvajIzmjene();
            PremjestajServis.sacuvajIzmjene();
        }

        private static void odradiPremjestajSada(DateTime datumIVrijeme, Sala salaUKojuSaljem, int kolicina, Oprema opremaZaSlanje)
        {
            if (datumIVrijeme.TimeOfDay <= DateTime.Now.TimeOfDay)
            {
                odradiPremjestaj(salaUKojuSaljem, kolicina, opremaZaSlanje);
            }
            else
            {
                zakaziPremjestaj(salaUKojuSaljem, datumIVrijeme, kolicina, opremaZaSlanje);
            }
        }

        private static void nadjiOpremuUSalama(Premjestaj zakazi, Oprema opremaZaSlanje)
        {
            foreach (Sala s in SaleMenadzer.NadjiSveSale())
            {
                foreach (Oprema o in s.Oprema)
                {
                    if (opremaZaSlanje.IdOpreme == o.IdOpreme)
                    {
                        zakazi.oprema = o;
                    }
                }
            }
        }

        private static void definisiOpremu(Premjestaj zakazi, Oprema oprema, Oprema opremaZaSlanje)
        {
            if (opremaZaSlanje.IdOpreme == oprema.IdOpreme)
            {
                zakazi.oprema = oprema;
            }
        }

        private static void definisiSale(Premjestaj zakazi, Sala s, Sala salaUKojuSaljem)
        {
            if (s.Namjena.Equals("Skladiste"))
            {
                zakazi.izSale = s;
            }
            if (s.Id == salaUKojuSaljem.Id)
            {
                zakazi.uSalu = s;
            }
        }

        private static void odradiPremjestaj(Sala salaUKojuSaljem, int kolicina, Oprema opremaZaSlanje)
        {
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (sala.Namjena.Equals("Skladiste"))
                {
                    ukloniStatickuOpremuIzSale(sala, kolicina, opremaZaSlanje);
                }
                if (sala.Id == salaUKojuSaljem.Id)
                {
                    dodajStatickuOpremuUSalu(sala, kolicina, opremaZaSlanje);
                }
            }
        }

        private static void ukloniStatickuOpremuIzSale(Sala sala, int kolicina, Oprema opremaZaSlanje)
        {
            foreach (Oprema oprema in sala.Oprema.ToArray())
            {
                if (oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    smanjiKolicinuOpreme(kolicina, oprema, sala);
                }
            }
        }

        private static void smanjiKolicinuOpreme(int kolicina, Oprema oprema, Sala sala)
        {
            if (oprema.Kolicina - kolicina == 0)
            {
                ukloniOpremu(sala, oprema);
            }
            else
            {
                smanjiKolicinu(oprema, kolicina);
            }
        }

        private static void smanjiKolicinu(Oprema oprema, int kolicina)
        {
            oprema.Kolicina -= kolicina;
            int idx = Skladiste.OpremaStaticka.IndexOf(oprema);
            Skladiste.OpremaStaticka.RemoveAt(idx);
            Skladiste.OpremaStaticka.Insert(idx, oprema);
        }

        private static void ukloniOpremu(Sala sala, Oprema oprema)
        {
            sala.Oprema.Remove(oprema);
            Skladiste.OpremaStaticka.Remove(oprema);
        }

        private static void dodajStatickuOpremuUSalu(Sala s, int kolicina, Oprema opremaZaSlanje)
        {
            napraviNovuOpremu(dodajOpremu(s, kolicina, opremaZaSlanje), kolicina, s, opremaZaSlanje);
        }

        private static bool dodajOpremu(Sala sala, int kolicina, Oprema opremaZaSlanje)
        {
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    oprema.Kolicina += kolicina;
                    return true;
                }
            }
            return false;
        }

        private static void napraviNovuOpremu(bool postojiOprema, int kolicina, Sala sala, Oprema opremaZaSlanje)
        {
            if (!postojiOprema)
            {
                Oprema oprema = new Oprema(opremaZaSlanje.NazivOpreme, kolicina, true);
                oprema.IdOpreme = opremaZaSlanje.IdOpreme;
                sala.Oprema.Add(oprema);
            }
        }

    }
}
