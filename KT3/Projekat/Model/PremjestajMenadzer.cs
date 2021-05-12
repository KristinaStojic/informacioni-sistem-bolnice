using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Projekat.Model
{
    class PremjestajMenadzer
    {
        public static void dodajPremjestaj(Premjestaj premjestaj)
        {
            premjestaji.Add(premjestaj);
            sacuvajIzmjene();
        }

        public static List<Premjestaj> NadjiSvePremjestaje()
        {
            if (File.ReadAllText("premjestaj.xml").Trim().Equals(""))
            {
                return premjestaji;
            }
            else
            {
                ucitajPremjestajeIzFajla();
                return premjestaji;
            }
        }

        private static void ucitajPremjestajeIzFajla()
        {
            FileStream filestream = File.OpenRead("premjestaj.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(List<Premjestaj>));
            premjestaji = (List<Premjestaj>)serializer.Deserialize(filestream);
            filestream.Close();
        }

        public static void sacuvajIzmjene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Premjestaj>));
            TextWriter filestream = new StreamWriter("premjestaj.xml");
            serializer.Serialize(filestream, premjestaji);
            filestream.Close();
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
            foreach(Premjestaj premjestaj in premjestaji.ToList())
            {
                if (!odradiPremjestaj(premjestaj))
                {
                    continue;
                }

                izvrsiPremjestaj(premjestaj);
                premjestaji.Remove(premjestaj);
                sacuvajIzmjene();
            
            }
        }

        private static void izvrsiPremjestaj(Premjestaj premjestaj)
        {
            foreach (Sala sala in SaleMenadzer.sale)
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

        private static void dodajOpremuUSkladiste(Oprema oprema)
        {
            if (Skladiste.OpremaStaticka != null)
            {
                Skladiste.OpremaStaticka.Add(oprema);
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
       
        public static int GenerisanjeIdPremjestaja()
        {
            int id;

            for (id = 1; id <= premjestaji.Count; id++)
            { 
                if (!postojiIdPremjestaja(id))
                {
                    return id;
                }
            }

            return id;
        }

        private static bool postojiIdPremjestaja(int id)
        {
            foreach (Premjestaj premjestaj in premjestaji)
            {
                if (premjestaj.id.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Premjestaj> premjestaji = new List<Premjestaj>();
    }
}
