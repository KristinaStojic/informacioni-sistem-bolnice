using Model;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Projekat.Model
{
    class OpremaMenadzer
    {

        public static void izmjeniOpremu(Oprema izOpreme, Oprema uOpremu)
        {
            izmjeniPrikazOpreme(izOpreme, uOpremu);
            azurirajOpremuUSkladistu();
            sacuvajIzmjene();
        }

        private static void izmjeniPrikazOpreme(Oprema izOpreme, Oprema uOpremu)
        {
            foreach (Oprema oprema in OpremaMenadzer.oprema)
            {
                if (oprema.IdOpreme == izOpreme.IdOpreme)
                {
                    oprema.NazivOpreme = uOpremu.NazivOpreme;
                    oprema.Kolicina = uOpremu.Kolicina;
                    zamjeniOpremu(uOpremu, izOpreme, oprema);
                }
            }
        }

        private static void zamjeniOpremu(Oprema uOpremu, Oprema izOpreme, Oprema oprema)
        {
            if (uOpremu.Staticka)
            {
                zamjeniStatickuOpremuUSkladistu(izOpreme, oprema);
            }
            else
            {
                zamjeniDinamickuOpremuUSkladistu(izOpreme, oprema);
            }
        }

        private static void zamjeniDinamickuOpremuUSkladistu(Oprema izOpreme, Oprema oprema)
        {
            int idx = Skladiste.OpremaDinamicka.IndexOf(izOpreme);
            Skladiste.OpremaDinamicka.RemoveAt(idx);
            Skladiste.OpremaDinamicka.Insert(idx, oprema);
        }

        private static void zamjeniStatickuOpremuUSkladistu(Oprema izOpreme, Oprema oprema)
        {
            int idx = Skladiste.OpremaStaticka.IndexOf(izOpreme);
            Skladiste.OpremaStaticka.RemoveAt(idx);
            Skladiste.OpremaStaticka.Insert(idx, oprema);
        }

        private static void azurirajOpremuUSkladistu()
        {
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (sala.Namjena.Equals("Skladiste"))
                {
                    sala.Oprema = OpremaMenadzer.oprema;
                }
            }
        }

        public static List<Oprema> NadjiSvuOpremu()
        {
            if (File.ReadAllText("oprema.xml").Trim().Equals(""))
            {
                return oprema;
            }
            else
            {
                ucitajOpremuIzFajla();
                return oprema;
            }
        }

        private static void ucitajOpremuIzFajla()
        {
            FileStream filestream = File.OpenRead("oprema.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(List<Oprema>));
            oprema = (List<Oprema>)serializer.Deserialize(filestream);
            filestream.Close();
            azurirajOpremuUSkladistu();
        }

        public static void DodajOpremu(Oprema oprema)
        {
            OpremaMenadzer.oprema.Add(oprema);
            azurirajOpremuUSkladistu();
            dodajOpremuUPrikaz(oprema);
            sacuvajIzmjene();
        }

        private static void dodajOpremuUPrikaz(Oprema oprema)
        {
            if (oprema.Staticka)
            {
                Skladiste.OpremaStaticka.Add(oprema);
            }
            else
            {
                Skladiste.OpremaDinamicka.Add(oprema);
            }
        }

        public static void ObrisiOpremu(Oprema oprema)
        {
            ukloniOpremu(oprema);
            azurirajOpremuUSkladistu();
            ukloniOpremuIzPrikaza(oprema);
            sacuvajIzmjene();
        }

        private static void ukloniOpremu(Oprema oprema)
        {
            for (int i = 0; i < OpremaMenadzer.oprema.Count; i++)
            {
                if (oprema.IdOpreme == OpremaMenadzer.oprema[i].IdOpreme)
                {
                    OpremaMenadzer.oprema.RemoveAt(i);
                }
            }
        }

        private static void ukloniOpremuIzPrikaza(Oprema oprema)
        {
            if (oprema.Staticka)
            {
                Skladiste.OpremaStaticka.Remove(oprema);
            }
            else
            {
                Skladiste.OpremaDinamicka.Remove(oprema);
            }
        }


        public static void sacuvajIzmjene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Oprema>));
            TextWriter filestream = new StreamWriter("oprema.xml");
            serializer.Serialize(filestream, oprema);
            filestream.Close();
        }

        public static int GenerisanjeIdOpreme()
        {
            int id;
            for (id = 1; id <= oprema.Count; id++)
            {
                if (!postojiIdOpreme(id))
                {
                    return id;
                }
            }
            return id;
        }

        private static bool postojiIdOpreme(int id)
        {
            foreach (Oprema o in oprema)
            {
                if (o.IdOpreme.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Oprema> oprema = new List<Oprema>();
    }
}
