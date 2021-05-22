using Model;
using Projekat.Model;
using Projekat.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

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
                //SkladisteViewModel.zamjeniOpremuUSkladistu(oprema);
                SkladisteViewModel.azurirajPrikaz();
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
                //SkladisteViewModel.dodajOpremuUSkladiste(oprema);
                SkladisteViewModel.azurirajPrikaz();
            }
            else
            {
                azurirajPrikazStaticke();
            }
        }

        private static void azurirajPrikazStaticke()
        {
            SaleViewModel.azuriraj = true;
            /*if (PrikazStaticke.otvoren)
            {
                PrikazStaticke.azurirajPrikaz();
            }*/
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
                //SkladisteViewModel.ukloniOpremuIzSkladista(sala, oprema);
                SkladisteViewModel.azurirajPrikaz();
            }
            else
            {
                //SkladisteViewModel.zamjeniOpremuUSkladistu(oprema);
                SkladisteViewModel.azurirajPrikaz();
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
            foreach (Sala sala in SaleMenadzer.NadjiSveSale())
            {
                foreach (Oprema oprema in sala.Oprema)
                {
                    if (opremaZaSlanje.IdOpreme == oprema.IdOpreme)
                    {
                        zakazi.oprema = oprema;
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
                    //PreraspodjelaDinamicke.smanjiKolicinuOpreme(kolicina, oprema, sala);
                }
            }
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

        public static void prebaciOpremu(Sala izabranaSala, int kolicina, Oprema izabranaOprema, Sala salaDodavanje)
        {
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (sala.Id == izabranaSala.Id)
                {
                    ukloniDinamickuOpremuIzSale(sala, kolicina, izabranaOprema);
                }
                if (sala.Id == salaDodavanje.Id)
                {
                    dodajDinamickuOpremuUSalu(sala, kolicina, izabranaOprema);

                }
            }
        }

        private static void dodajDinamickuOpremuUSalu(Sala sala, int kolicina, Oprema izabranaOprema)
        {
            if (prebaciDinamicku(sala, kolicina, izabranaOprema) == 0)
            {
                Oprema oprema = new Oprema(izabranaOprema.NazivOpreme, kolicina, false);
                oprema.IdOpreme = izabranaOprema.IdOpreme;
                //PrikazDinamicke.OpremaDinamicka.Add(oprema);
                sala.Oprema.Add(oprema);
            }
        }

        private static int prebaciDinamicku(Sala sala, int kolicina, Oprema izabranaOprema)
        {
            int x = 0;
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    oprema.Kolicina += kolicina;
                    x += 1;
                    //PreraspodjelaDinamicke.azurirajPrikaz(oprema);
                }
            }
            return x;
        }

        private static void ukloniDinamickuOpremuIzSale(Sala sala, int kolicina, Oprema izabranaOprema)
        {
            foreach (Oprema oprema in sala.Oprema.ToArray())
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    ukloniOpremu(oprema, kolicina, sala);
                }
            }
        }

        private static void ukloniOpremu(Oprema oprema, int kolicina, Sala sala)
        {
            oprema.Kolicina -= kolicina;
            if (oprema.Kolicina == 0)
            {
                sala.Oprema.Remove(oprema);
            }
        }

        public static void dodajStatickuOpremu(Sala izabranaSala, int kolicina, DateTime datumIVrijeme, Sala salaDodavanje, Oprema izabranaOprema)
        {
            if (datumIVrijeme.Date.ToString().Equals(DateTime.Now.Date.ToString()))
            {
                provjeriPremjestaj(datumIVrijeme, izabranaSala, kolicina, salaDodavanje, izabranaOprema);
            }
            else
            {
                zakaziPremjestaj(izabranaSala, kolicina, datumIVrijeme, izabranaOprema, salaDodavanje);
            }
            sacuvajIzmjene();
            SaleServis.sacuvajIzmjene();
        }

        private static void provjeriPremjestaj(DateTime datumIVrijeme, Sala izSale, int kolicina, Sala salaDodavanje, Oprema izabranaOprema)
        {
            if (datumIVrijeme.TimeOfDay <= DateTime.Now.TimeOfDay)
            {
                premjestiOpremu(salaDodavanje, kolicina, izSale, izabranaOprema);
            }
            else
            {
                zakaziPremjestaj(izSale, kolicina, datumIVrijeme, izabranaOprema, salaDodavanje);
            }
        }

        private static void ukloniOpremuIzSaleStaticka(Sala sala, int kolicina, Oprema izabranaOprema, Sala salaDodavanje)
        {
            foreach (Oprema oprema in sala.Oprema.ToArray())
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    oprema.Kolicina -= kolicina;
                    if (sala.Namjena.Equals("Skladiste"))
                    {
                        sala.Oprema.Remove(oprema);
                        //PreraspodjelaStaticke.prebaciOpremuIzSkladista(oprema);
                        SaleViewModel.azuriraj = true;
                    }
                    else
                    {
                        //PreraspodjelaStaticke.azurirajPrikazStaticke(oprema, sala);
                        SaleViewModel.azuriraj = true;
                    }
                }
            }
        }

        private static void ukloniOpremu(Sala sala, Oprema oprema)
        {
            if (sala.Namjena.Equals("Skladiste"))
            {
                if (SkladisteViewModel.otvoren)
                {
                    // PreraspodjelaStaticke.prebaciUSkladiste(oprema);
                    SaleViewModel.azuriraj = true;
                }
            }
            else
            {
                //PreraspodjelaStaticke.azurirajPrikaz();
                SaleViewModel.azuriraj = true;
            }
        }

        private static void premjestiOpremu(Sala salaDodavanje, int kolicina, Sala izabranaSala, Oprema izabranaOprema)
        {
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (sala.Id == izabranaSala.Id)
                {
                    ukloniOpremuIzSaleStaticka(sala, kolicina, izabranaOprema, salaDodavanje);
                }
                if (sala.Id == salaDodavanje.Id)
                {
                    dodajOpremuUSaluStaticka(sala, kolicina, izabranaOprema, salaDodavanje);

                }
            }
        }

        private static int prebaciOpremu(Sala sala, int kolicina, Oprema izabranaOprema)
        {
            int x = 0;
            foreach (Oprema oprema in sala.Oprema)
            {
                if (oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    oprema.Kolicina += kolicina;
                    x += 1;
                    ukloniOpremu(sala, oprema);
                }
            }
            return x;
        }

        private static void dodajOpremu(Sala sala, int kolicina, Sala salaDodavanje, Oprema izabranaOprema)
        {
            Oprema oprema = new Oprema(izabranaOprema.NazivOpreme, kolicina, true);
            oprema.IdOpreme = izabranaOprema.IdOpreme;
            sala.Oprema.Add(oprema);
            if (salaDodavanje.Namjena.Equals("Skladiste"))
            {
                //PreraspodjelaStaticke.dodajStaticku(oprema);
                SaleViewModel.azuriraj = true;
            }
            else
            {
                // PreraspodjelaStaticke.azurirajPrikaz();
                SaleViewModel.azuriraj = true;
            }
        }

        private static void zakaziPremjestaj(Sala izabranaSala, int kolicina, DateTime datumIVrijeme, Oprema izabranaOprema, Sala salaDodavanje)
        {
            Premjestaj zakazi = new Premjestaj();
            zakazi.kolicina = kolicina;
            definisiSale(izabranaSala, zakazi, salaDodavanje);
            definisiOpremu(zakazi, izabranaOprema);
            dodajOpremuIzSala(zakazi, izabranaOprema);
            dodajPremjestajStaticke(zakazi, datumIVrijeme);
        }

        private static void definisiSale(Sala izabranaSala, Premjestaj zakazi, Sala salaDodavanje)
        {
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.Id == izabranaSala.Id)
                {
                    zakazi.izSale = s;
                }
                if (s.Id == salaDodavanje.Id)
                {
                    zakazi.uSalu = s;
                }
            }
        }

        private static void dodajOpremuUSaluStaticka(Sala sala, int kolicina, Oprema izabranaOprema, Sala salaDodavanje)
        {
            if (prebaciOpremu(sala, kolicina, izabranaOprema) == 0)
            {
                dodajOpremu(sala, kolicina, salaDodavanje, izabranaOprema);
            }
        }

        private static void dodajPremjestajStaticke(Premjestaj zakazi, DateTime datumIVrijeme)
        {
            zakazi.datumIVrijeme = datumIVrijeme;
            PremjestajServis.dodajPremjestaj(zakazi);
        }

        private static void dodajOpremuIzSala(Premjestaj zakazi, Oprema izabranaOprema)
        {
            if (zakazi.oprema == null)
            {
                foreach (Sala sala in SaleMenadzer.sale)
                {
                    dodajOpremu(sala, zakazi, izabranaOprema);
                }
            }
        }

        private static void dodajOpremu(Sala sala, Premjestaj zakazi, Oprema izabranaOprema)
        {
            foreach (Oprema oprema in sala.Oprema)
            {
                if (izabranaOprema.IdOpreme == oprema.IdOpreme)
                {
                    zakazi.oprema = oprema;
                }
            }
        }

        private static void definisiOpremu(Premjestaj zakazi, Oprema izabranaOprema)
        {
            foreach (Oprema o in OpremaMenadzer.oprema)
            {
                if (izabranaOprema.IdOpreme == o.IdOpreme)
                {
                    zakazi.oprema = o;
                }
            }
        }

        public static void prebaciDinamickuOpremu(Sala salaUKojuSaljem, int kolicina, Sala salaIzKojeSaljem, Oprema opremaZaSlanje)
        {
            foreach (Sala sala in SaleMenadzer.sale)
            {
                posaljiIzSale(sala, kolicina, salaIzKojeSaljem, opremaZaSlanje);
                dodajUSalu(sala, salaUKojuSaljem, kolicina, opremaZaSlanje);
            }
        }

        private static void dodajUSalu(Sala sala, Sala salaUKojuSaljem, int kolicina, Oprema opremaZaSlanje)
        {
            if (sala.Id == salaUKojuSaljem.Id)
            {
                dodajOpremuUSaluDinamicka(sala, kolicina, opremaZaSlanje);
            }
        }

        private static void posaljiIzSale(Sala sala, int kolicina, Sala salaIzKojeSaljem, Oprema opremaZaSlanje)
        {
            if (sala.Id == salaIzKojeSaljem.Id)
            {
                foreach (Oprema oprema in sala.Oprema.ToArray())
                {
                    prebaciOpremuIzSaleDinamicka(oprema, kolicina, sala, opremaZaSlanje);
                }
            }
        }

        private static void prebaciOpremuIzSaleDinamicka(Oprema oprema, int kolicina, Sala sala, Oprema opremaZaSlanje)
        {
            if (oprema.IdOpreme == opremaZaSlanje.IdOpreme)
            {
                if (oprema.Kolicina - kolicina == 0)
                {
                    sala.Oprema.Remove(oprema);
                   // SlanjeDinamicke.ukloniOpremu(sala, oprema);
                }
                else
                { 
                    oprema.Kolicina -= kolicina;
                    //SlanjeDinamicke.prebaciOpremu(oprema, kolicina);
                }

            }
        }

        private static void dodajOpremuUSaluDinamicka(Sala sala, int kolicina, Oprema opremaZaSlanje)
        {
            if (!postojiOprema(sala, kolicina, opremaZaSlanje))
            {
                Oprema oprema = new Oprema(opremaZaSlanje.NazivOpreme, kolicina, false);
                oprema.IdOpreme = opremaZaSlanje.IdOpreme;
                sala.Oprema.Add(oprema);
            }
        }

        private static bool postojiOprema(Sala sala, int kolicina, Oprema opremaZaSlanje)
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

        public static void prebaciStatickuOpremu(DateTime datumIVrijeme, int kolicina, Sala salaUKojuSaljem, Sala salaIzKojeSaljem, Oprema opremaZaSlanje)
        {
            if (datumIVrijeme.Date.ToString().Equals(DateTime.Now.Date.ToString()))
            {
                provjeriPremjestajStaticka(datumIVrijeme, salaUKojuSaljem, kolicina, salaIzKojeSaljem, opremaZaSlanje);
            }
            else
            {
                zakaziPremjestajStaticke(kolicina, salaUKojuSaljem, datumIVrijeme, opremaZaSlanje, salaIzKojeSaljem);
            }
        }
        private static void provjeriPremjestajStaticka(DateTime datumIVrijeme, Sala salaUKojuSaljem, int kolicina, Sala salaIzKojeSaljem, Oprema opremaZaSlanje)
        {
            if (datumIVrijeme.TimeOfDay <= DateTime.Now.TimeOfDay)
            {
                premjestiStatickuOpremu(salaUKojuSaljem, kolicina, salaIzKojeSaljem, opremaZaSlanje);
            }
            else
            {
                zakaziPremjestajStaticke(kolicina, salaUKojuSaljem, datumIVrijeme, opremaZaSlanje, salaIzKojeSaljem);
            }
        }

        private static void zakaziPremjestajStaticke(int kolicina, Sala salaUKojuSaljem, DateTime datumIVrijeme, Oprema opremaZaSlanje, Sala salaIzKojeSaljem)
        {
            Premjestaj zakazi = new Premjestaj();
            zakazi.kolicina = kolicina;
            definisiSaleStaticka(zakazi, salaUKojuSaljem, salaIzKojeSaljem);
            definisiOpremuIzSkladista(zakazi, opremaZaSlanje);
            definisiOpremuIzSala(zakazi, opremaZaSlanje);
            zakazi.datumIVrijeme = datumIVrijeme;
            dodajPremjestaj(zakazi);
        }
        private static void premjestiStatickuOpremu(Sala salaUKojuSaljem, int kolicina, Sala salaIzKojeSaljem, Oprema opremaZaSlanje)
        {
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (sala.Id == salaIzKojeSaljem.Id)
                {
                    prebaciOpremuIzSale(sala, kolicina, opremaZaSlanje);
                }
                if (sala.Id == salaUKojuSaljem.Id)
                {
                    dodajOpremuUSaluStaticka(sala, kolicina, opremaZaSlanje);
                }
            }
        }

        private static void dodajOpremuUSaluStaticka(Sala sala, int kolicina, Oprema opremaZaSlanje)
        {
            if (!postojiStatickaOprema(sala, kolicina, opremaZaSlanje))
            {
                Oprema oprema = new Oprema(opremaZaSlanje.NazivOpreme, kolicina, true);
                oprema.IdOpreme = opremaZaSlanje.IdOpreme;
                sala.Oprema.Add(oprema);
            }
        }

        private static void definisiSaleStaticka(Premjestaj zakazi, Sala salaUKojuSaljem, Sala salaIzKojeSaljem)
        {
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.Id == salaIzKojeSaljem.Id)
                {
                    zakazi.izSale = s;
                }
                if (s.Id == salaUKojuSaljem.Id)
                {
                    zakazi.uSalu = s;
                }
            }
        }

        private static bool postojiStatickaOprema(Sala s, int kolicina, Oprema opremaZaSlanje)
        {
            foreach (Oprema o in s.Oprema)
            {
                if (o.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    o.Kolicina += kolicina;
                    return true;
                }
            }
            return false;
        }

        private static void prebaciOpremuIzSale(Sala sala, int kolicina, Oprema opremaZaSlanje)
        {
            foreach (Oprema oprema in sala.Oprema.ToArray())
            {
                prebaciStatickuOpremu(oprema, sala, kolicina, opremaZaSlanje);
            }
        }

        private static void prebaciStatickuOpremu(Oprema oprema, Sala sala, int kolicina, Oprema opremaZaSlanje)
        {
            if (oprema.IdOpreme == opremaZaSlanje.IdOpreme)
            {
                if (oprema.Kolicina - kolicina == 0)
                {
                    sala.Oprema.Remove(oprema);
                    //SlanjeStaticke.ukloniOpremu(sala, oprema);
                    SaleViewModel.azuriraj = true;
                }
                else
                {
                    oprema.Kolicina -= kolicina;
                    //SlanjeStaticke.smanjiKolicinuOpreme(oprema, kolicina);
                    SaleViewModel.azuriraj = true;
                }
            }
        }

        private static void definisiOpremuIzSkladista(Premjestaj zakazi, Oprema opremaZaSlanje)
        {
            foreach (Oprema oprema in OpremaMenadzer.oprema)
            {
                if (opremaZaSlanje.IdOpreme == oprema.IdOpreme)
                {
                    zakazi.oprema = oprema;
                }
            }
        }

        private static void nadjiOpremuSale(Sala sala, Premjestaj zakazi, Oprema opremaZaSlanje)
        {
            foreach (Oprema oprema in sala.Oprema)
            {
                if (opremaZaSlanje.IdOpreme == oprema.IdOpreme)
                {
                    zakazi.oprema = oprema;
                }
            }
        }

        private static void definisiOpremuIzSala(Premjestaj zakazi, Oprema opremaZaSlanje)
        {
            if (zakazi.oprema == null)
            {
                foreach (Sala sala in SaleMenadzer.sale)
                {
                    nadjiOpremuSale(sala, zakazi, opremaZaSlanje);
                }
            }
        }

    }
}
