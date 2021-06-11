using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Projekat;
using Projekat.Model;
using Projekat.Servis;

namespace Model
{
    class LekariMenadzer
    {
        public static List<Lekar> lekari = new List<Lekar>();
        public static List<ZahtevZaGodisnji> zahtevi = new List<ZahtevZaGodisnji>();
        PacijentiServis servis = new PacijentiServis();
        PacijentiMenadzer menadzer = new PacijentiMenadzer();

        public static void DodajLekara(Lekar noviLekar)
        {
            noviLekar.lozinka = noviLekar.Jmbg.ToString();
            noviLekar.korisnickoIme = noviLekar.PrezimeLek;
            lekari.Add(noviLekar);
            //PrikaziLekare.Lekari.Add(noviLekar);
            SacuvajIzmeneLekara();
        }

        public static void IzmeniLekara(Lekar stariLekar, Lekar noviLekar)
        {
            foreach (Lekar l in lekari)
            {
                if (l.IdLekara == stariLekar.IdLekara)
                {
                    l.korisnickoIme = noviLekar.korisnickoIme;
                    l.lozinka = noviLekar.lozinka;
                    l.ImeLek = noviLekar.ImeLek;
                    l.PrezimeLek = noviLekar.PrezimeLek;
                    l.Jmbg = noviLekar.Jmbg;
                    l.BrojTelefona = noviLekar.BrojTelefona;
                    l.Email = noviLekar.Email;
                    l.AdresaStanovanja = noviLekar.AdresaStanovanja;
                    l.specijalizacija = noviLekar.specijalizacija;

                   // int idx = PrikaziLekare.Lekari.IndexOf(stariLekar);
                   // PrikaziLekare.Lekari.RemoveAt(idx);
                   // PrikaziLekare.Lekari.Insert(idx, l);
                    SacuvajIzmeneLekara();
                }
            }
        }

        public static void ObrisiTermineZaLekara(Lekar lekar)
        {
            for (int i = 0; i < TerminMenadzer.termini.Count; i++)
            {
                if (TerminMenadzer.termini[i].Lekar.IdLekara == lekar.IdLekara)
                {
                    TerminMenadzer.termini.RemoveAt(i);
                    i--;
                    TerminiSekretarServis.sacuvajIzmene();
                }
            }
        }

        public static void ObrisiZahteveZaGodisnji(Lekar lekar)
        {
            for (int i = 0; i < zahtevi.Count; i++)
            {
                if (zahtevi[i].lekar.IdLekara == lekar.IdLekara)
                {
                    zahtevi.RemoveAt(i);
                    i--;
                    sacuvajIzmjeneZahteva();
                }
            }
        }

        public void ObrisiRecepte(Lekar lekar)
        {
            List<Pacijent> sviPacijenti = servis.pacijenti();
            for (int i = 0; i < sviPacijenti.Count; i++)
            {
                if (sviPacijenti[i].Karton == null)
                {
                    return;
                }
                else 
                { 
                    for (int j = 0; j < sviPacijenti[i].Karton.LekarskiRecepti.Count; j++)
                    {
                        if (sviPacijenti[i].Karton.LekarskiRecepti[j].IdLekara == lekar.IdLekara)
                        {
                            sviPacijenti[i].Karton.LekarskiRecepti.RemoveAt(j);
                            j--;
                            menadzer.SacuvajIzmene("pacijenti.xml", sviPacijenti);
                        }
                    }
                }
            }
        }

        public void ObrisiUpute(Lekar lekar)
        {
            List<Pacijent> sviPacijenti = servis.pacijenti();
            for (int i = 0; i < sviPacijenti.Count; i++)
            {
                if (sviPacijenti[i].Karton == null)
                {
                    return;
                }
                else
                { 
                    for (int j = 0; j < sviPacijenti[i].Karton.Uputi.Count; j++)
                    {
                        if (sviPacijenti[i].Karton.Uputi[j].IdLekaraKodKogSeUpucuje == lekar.IdLekara
                            || sviPacijenti[i].Karton.Uputi[j].IdLekaraKojiIzdajeUput == lekar.IdLekara)
                        {
                            sviPacijenti[i].Karton.Uputi.RemoveAt(j);
                            j--;
                            menadzer.SacuvajIzmene("pacijenti.xml", sviPacijenti);
                        }
                    }
                }
            }
        }

        public  void ObrisiIzabranogLekara(Lekar lekar)
        {
            List<Pacijent> sviPacijenti = servis.pacijenti();
            for (int i = 0; i < sviPacijenti.Count; i++)
            {
                if (sviPacijenti[i].IzabraniLekar != null)
                {
                    if (sviPacijenti[i].IzabraniLekar.IdLekara == lekar.IdLekara)
                    {
                        sviPacijenti[i].IzabraniLekar = new Lekar();
                        menadzer.SacuvajIzmene("pacijenti.xml", sviPacijenti);
                    }
                }
            }
        }

        public void ObrisiLekara(Lekar lekar)
        {
            ObrisiUpute(lekar);
            ObrisiRecepte(lekar);
            ObrisiZahteveZaGodisnji(lekar);
            ObrisiIzabranogLekara(lekar);
            ObrisiTermineZaLekara(lekar);

            for (int i = 0; i < lekari.Count; i++)
            {
                if (lekari[i].IdLekara == lekar.IdLekara)
                {   
                    lekari.RemoveAt(i);
                   // PrikaziLekare.Lekari.Remove(lekar);
                    SacuvajIzmeneLekara();
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

            for (id = 1; id <= zahtevi.Count; id++)
            {
                foreach (ZahtevZaGodisnji zahtev in zahtevi)
                {
                    if (zahtev.idZahteva.Equals(id))
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

        public static void SacuvajIzmeneLekara()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Lekar>));
            TextWriter filestream = new StreamWriter("lekari.xml");
            serializer.Serialize(filestream, lekari);
            filestream.Close();
        }

        public static List<Lekar> PronadjiLekarePoSpecijalizaciji(Specijalizacija tipSpecijalizacije)
        {
            List<Lekar> specijalizovaniLekari = new List<Lekar>();
            foreach (Lekar lekar in lekari)
            {
                if (lekar.specijalizacija.Equals(tipSpecijalizacije))
                {
                    specijalizovaniLekari.Add(lekar);
                }
            }
            return specijalizovaniLekari;
        }

        public static void DodajZahtev(ZahtevZaGodisnji zahtev)
        {
            foreach (Lekar lekar in lekari)
            {
                if (lekar.IdLekara == zahtev.lekar.IdLekara)
                {
                    lekar.ZahteviZaOdmor.Add(zahtev.idZahteva);
                    zahtevi.Add(zahtev);
                    ZahteviZaGodisnjiLekar.TabelaZahteva.Add(zahtev);
                    sacuvajIzmjeneZahteva();
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

        public static ZahtevZaGodisnji NadjiZahtevPoId(int id)
        {
            foreach (ZahtevZaGodisnji zahtev in zahtevi)
            {
                if (zahtev.idZahteva == id)
                {
                    return zahtev;
                }
            }
            return null;
        }

        public static void sacuvajIzmjeneZahteva()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ZahtevZaGodisnji>));
            TextWriter filestream = new StreamWriter("zahteviZaOdmor.xml");
            serializer.Serialize(filestream, zahtevi);
            filestream.Close();
        }

    }
}
