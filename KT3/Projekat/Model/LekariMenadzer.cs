using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Projekat;
using Projekat.Model;

namespace Model
{
    class LekariMenadzer
    {
        public static List<Lekar> lekari = new List<Lekar>();

        public static void DodajLekara(Lekar noviLekar)
        {
            lekari.Add(noviLekar);
            PrikaziLekare.Lekari.Add(noviLekar);
            SacuvajIzmeneLekara();
        }

        public static void IzmeniLekara(Lekar stariLekar, Lekar noviLekar)
        {
            foreach (Lekar l in lekari)
            {
                if (l.IdLekara == stariLekar.IdLekara)
                {
                    l.ImeLek = noviLekar.ImeLek;
                    l.PrezimeLek = noviLekar.PrezimeLek;
                    l.Jmbg = noviLekar.Jmbg;
                    l.BrojTelefona = noviLekar.BrojTelefona;
                    l.Email = noviLekar.Email;
                    l.AdresaStanovanja = noviLekar.AdresaStanovanja;
                    l.specijalizacija = noviLekar.specijalizacija;

                    int idx = PrikaziLekare.Lekari.IndexOf(stariLekar);
                    PrikaziLekare.Lekari.RemoveAt(idx);
                    PrikaziLekare.Lekari.Insert(idx, l);
                    SacuvajIzmeneLekara();
                }
            }
        }

        public static void ObrisiLekara(Lekar lekar)
        {
            // TODO: brisanje termina sa datim lekarom
            // TODO: brisanje lekara iz izabranih lekara pacijenata (ili je to automatski?)

            for (int i = 0; i < lekari.Count; i++)
            {
                if (lekari[i].IdLekara == lekar.IdLekara)
                {   
                    lekari.RemoveAt(i);
                    PrikaziLekare.Lekari.Remove(lekar);
                    SacuvajIzmeneLekara();

                    /*   for (int j = 0; j < TerminMenadzer.termini.Count; j++)
                       {
                           if (TerminMenadzer.termini[j].Pacijent.IdPacijenta == nalog.IdPacijenta)
                           {
                               foreach (Sala s in SaleMenadzer.sale)
                               {
                                   if (s.Id == TerminMenadzer.termini[j].Prostorija.Id)
                                   {
                                       s.zauzetiTermini.Remove(SaleMenadzer.NadjiZauzece(s.Id, TerminMenadzer.termini[j].IdTermin, TerminMenadzer.termini[j].Datum, TerminMenadzer.termini[j].VremePocetka, TerminMenadzer.termini[j].VremeKraja));
                                       //SaleMenadzer.sacuvajIzmjene();
                                   }
                               }

                               TerminMenadzer.termini.RemoveAt(j);
                               j--;
                           }
                       }*/
                }
            }            
        }

        public static List<Lekar> NadjiSveLekare()
        {
            if (File.ReadAllText("lekari.xml").Trim().Equals(""))
            {
                return lekari;
            }
            else
            {
                FileStream filestream = File.OpenRead("lekari.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Lekar>));
                lekari = (List<Lekar>)serializer.Deserialize(filestream);
                filestream.Close();
                return lekari;
            }
        }

        public static Lekar NadjiPoId(int id)
        {
            foreach (Lekar lekar in lekari)
            {
                if (lekar.IdLekara == id)
                {
                    return lekar;
                }
            }
            return null;
        }

        public static int GenerisanjeIdLekara()
        {
            bool pomocna = false;
            int id = 1;

            for (id = 1; id <= lekari.Count; id++)
            {
                foreach (Lekar lekar in lekari)
                {
                    if (lekar.IdLekara.Equals(id))
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

            return id;
        }
        
        public static int GenerisanjeIdZahtevaZaOdmor(int idLekara)
        {
            bool pomocna = false;
            int id = 1;
            foreach (Lekar lekar in lekari)
            {
                if (lekar.IdLekara == idLekara)
                {
                    for (id = 1; id <= lekar.zahteviZaOdmor.Count; id++)
                    {
                        foreach (int z in lekar.zahteviZaOdmor)
                        {
                            if (z == id)
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

        public static void SacuvajIzmeneLekara()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Lekar>));
            TextWriter filestream = new StreamWriter("lekari.xml");
            serializer.Serialize(filestream, lekari);
            filestream.Close();
        }

        // TODO: problem - moguce je da neki pacijent i neki lekar imaju isti jmbg a nisu ista osoba
        public static bool JedinstvenJmbg(int jmbg)
        {
            foreach (Lekar lekar in lekari)
            {
                if (lekar.Jmbg == jmbg)
                {
                    return false;
                }
            }
            return true;
        }


        public static void DodajZahtev(ZahtevZaGodisnji zahtev)
        {
            foreach (Lekar lekar in lekari)
            {
                if (lekar.IdLekara == zahtev.lekar.IdLekara)
                {
                    lekar.zahteviZaOdmor.Add(zahtev.idZahteva);
                    zahtevi.Add(zahtev);
                    ZahteviZaGodisnjiLekar.TabelaZahteva.Add(zahtev);
                    sacuvajIzmjene();
                }
            }
        }


        public static List<ZahtevZaGodisnji> NadjiSveZahteve()
        {

            if (File.ReadAllText("zahteviZaOdmor.xml").Trim().Equals(""))
            {
                return zahtevi;
            }
            else
            {
                FileStream filestream = File.OpenRead("zahteviZaOdmor.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<ZahtevZaGodisnji>));
                zahtevi = (List<ZahtevZaGodisnji>)serializer.Deserialize(filestream);
                filestream.Close();
                return zahtevi;
            }
        }
        public static void sacuvajIzmjene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ZahtevZaGodisnji>));
            TextWriter filestream = new StreamWriter("zahteviZaOdmor.xml");
            serializer.Serialize(filestream, zahtevi);
            filestream.Close();
        }

        public static List<ZahtevZaGodisnji> zahtevi = new List<ZahtevZaGodisnji>();
    }
}
