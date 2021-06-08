using Model;
using Projekat.Model;
using Projekat.ViewModel;
using System;

namespace Projekat.Servis
{
    public class PremjestajOpremeServis
    {
        public static void premjestajStatickeOpreme(Sala izabranaSala, int kolicina, DateTime datumIVrijeme, Sala salaDodavanje, Oprema izabranaOprema)
        {
            if (datumIVrijeme.Date.ToString().Equals(DateTime.Now.Date.ToString()) && datumIVrijeme.TimeOfDay <= DateTime.Now.TimeOfDay)
            {
                OpremaServis.premjestiStatickuOpremu(salaDodavanje, kolicina, izabranaSala, izabranaOprema);
            }
            else
            {
                zakaziPremjestaj(izabranaSala, kolicina, datumIVrijeme, izabranaOprema, salaDodavanje);
            }
            PremjestajServis.sacuvajIzmjene();
            SaleServis.sacuvajIzmjene();
        }

        public static void premjestajDinamickeOpreme(Sala izabranaSala, int kolicina, Oprema izabranaOprema, Sala salaDodavanje)
        {
            foreach (Sala sala in SaleServis.Sale())
            {
                if (sala.Id == izabranaSala.Id)
                {
                    IzmjenaKolicineStatickeServis.SmanjiKolicinuOpreme(sala, kolicina, izabranaOprema);
                }
                if (sala.Id == salaDodavanje.Id)
                {
                    OpremaServis.dodajDinamickuOpremuUSalu(sala, kolicina, izabranaOprema);

                }
            }
        }

        private static void zakaziPremjestaj(Sala izabranaSala, int kolicina, DateTime datumIVrijeme, Oprema izabranaOprema, Sala salaDodavanje)
        {
            Premjestaj zakazi = new Premjestaj();
            zakazi.kolicina = kolicina;
            definisiSalePremjestaja(izabranaSala, zakazi, salaDodavanje);
            definisiOpremuPremjestaja(zakazi, izabranaOprema);
            definisiOpremuIzSkladistaPremjestaj(zakazi, izabranaOprema);
            zakazi.datumIVrijeme = datumIVrijeme;
            sacuvajPremjestaj(zakazi);
        }

        public static void sacuvajPremjestaj(Premjestaj premjestaj)
        {
            PremjestajMenadzer.Dodaj(premjestaj, "premjestaj.xml");
        }

        private static void definisiOpremuPremjestaja(Premjestaj zakazi, Oprema izabranaOprema)
        {
            foreach (Oprema oprema in OpremaServis.Oprema())
            {
                if (izabranaOprema.IdOpreme == oprema.IdOpreme)
                {
                    zakazi.oprema = oprema;
                }
            }
        }

        private static void definisiOpremuIzSkladistaPremjestaj(Premjestaj zakazi, Oprema izabranaOprema)
        {
            if (zakazi.oprema == null)
            {
                foreach (Sala sala in SaleServis.Sale())
                {
                    foreach (Oprema oprema in sala.Oprema)
                    {
                        if (izabranaOprema.IdOpreme == oprema.IdOpreme)
                        {
                            zakazi.oprema = oprema;
                        }
                    }
                }
            }
        }

        private static void definisiSalePremjestaja(Sala izabranaSala, Premjestaj zakazi, Sala salaDodavanje)
        {
            foreach (Sala sala in SaleServis.Sale())
            {
                if (sala.Id == izabranaSala.Id)
                {
                    zakazi.izSale = sala;
                }
                if (sala.Id == salaDodavanje.Id)
                {
                    zakazi.uSalu = sala;
                }
            }
        }
    }
}
