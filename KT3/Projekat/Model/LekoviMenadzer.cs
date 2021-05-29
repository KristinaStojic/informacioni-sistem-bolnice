using Projekat.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Projekat.Model
{
    class LekoviMenadzer
    {
        public static void DodajLijek(Lek lijek)
        {
            lijekovi.Add(lijek);
            SpisakZahtevaZaLekove.TabelaLekova.Add(lijek);
            sacuvajIzmjene();
        }

        public static void dodajZamjenskeLijekove(Lek izabraniLijek, Lek zamjenskiLijekovi)
        {
            foreach(Lek lijek in lijekovi)
            {
                if(lijek.idLeka == izabraniLijek.idLeka)
                {
                        lijek.zamenskiLekovi.Add(zamjenskiLijekovi.idLeka);
                }
            }
            sacuvajIzmjene();
        } 
        public static void dodajZamenskeLekoveLekar(Lek izabraniLek, List<Lek> zamenskiLekovi)
        {
            foreach(Lek lek in lijekovi)
            {
                if(lek.idLeka == izabraniLek.idLeka)
                {
                    foreach(Lek zamenski in zamenskiLekovi)
                    {
                        lek.zamenskiLekovi.Add(zamenski.idLeka);
                        PrikazZamenskihLekovaLekar.TabelaZamenskihLekova.Add(zamenski);
                    }
                }
            }
            sacuvajIzmjene();
        }
        public static void dodajSastojak(Sastojak sastojak, Lek izabraniLijek)
        {
            foreach (Lek lijek in lijekovi)
            {
                if (lijek.idLeka == izabraniLijek.idLeka)
                {
                    lijek.sastojci.Add(sastojak);
                }
            }
            sacuvajIzmjene();
        }
        
        public static void dodajSastojakLekar(Sastojak sastojak, Lek izabraniLek)
        {
            foreach (Lek lek in lijekovi)
            {
                if (lek.idLeka == izabraniLek.idLeka)
                {
                    lek.sastojci.Add(sastojak);
                    PrikazSastojakaLekar.TabelaSastojaka.Add(sastojak);
                    //sastojci.Add(sastojak);

                }
            }
            sacuvajIzmjene();
        }
        public static void obrisiLijek(Lek lijek)
        {
            lijekovi.Remove(lijek);
            zahteviZaLekove.Remove(izabraniZahtjev(lijek));
            sacuvajIzmjene();
            sacuvajIzmeneZahteva();
        }

        private static ZahtevZaLekove izabraniZahtjev(Lek lijek)
        {
            foreach(ZahtevZaLekove zahtjev in zahteviZaLekove)
            {
                if (zahtjev.lek.sifraLeka.Equals(lijek.sifraLeka))
                {
                    return zahtjev;
                }
            }
            return null;
        }

        public static void izmjeniLijek(Lek izabraniLijek, Lek izmjenjeniLijek)
        {
            foreach(Lek lijek in lijekovi)
            {
                if(lijek.idLeka == izabraniLijek.idLeka)
                {
                    lijek.sifraLeka = izmjenjeniLijek.sifraLeka;
                    lijek.nazivLeka = izmjenjeniLijek.nazivLeka;
                    lijek.zamenskiLekovi = izmjenjeniLijek.zamenskiLekovi;
                    lijek.sastojci = izmjenjeniLijek.sastojci;
                }
            }
            sacuvajIzmjene();
        }      

        public static void IzmjeniOdbijeniLijek(Lek izabraniLijek, Lek uLijek)
        {
            foreach(ZahtevZaLekove zahtjev in zahteviZaLekove)
            {
                if(zahtjev.lek.sifraLeka.Equals(izabraniLijek.sifraLeka))
                {
                    zahtjev.lek.sifraLeka = uLijek.sifraLeka;
                    zahtjev.lek.nazivLeka = uLijek.nazivLeka;
                    zahtjev.sifraLeka = uLijek.sifraLeka;
                    zahtjev.nazivLeka = uLijek.nazivLeka;
                }
            }
        }
        
        public static void IzmeniLekoveLekar(Lek izabraniLek, Lek izmenjeniLek)
        {
            foreach(Lek lek in lijekovi)
            {
                if(lek.idLeka == izabraniLek.idLeka)
                {
                    lek.sifraLeka = izmenjeniLek.sifraLeka;
                    lek.nazivLeka = izmenjeniLek.nazivLeka;
                    lek.zamenskiLekovi = izmenjeniLek.zamenskiLekovi;
                    lek.sastojci = izmenjeniLek.sastojci;
                    int idx = SpisakZahtevaZaLekove.TabelaLekova.IndexOf(izabraniLek);
                    SpisakZahtevaZaLekove.TabelaLekova.RemoveAt(idx);
                    SpisakZahtevaZaLekove.TabelaLekova.Insert(idx, lek);
                    if (PrikazZamenskihLekovaLekar.TabelaZamenskihLekova != null)
                    {
                        int idx1 = PrikazZamenskihLekovaLekar.TabelaZamenskihLekova.IndexOf(izabraniLek);
                        PrikazZamenskihLekovaLekar.TabelaZamenskihLekova.RemoveAt(idx1);
                        PrikazZamenskihLekovaLekar.TabelaZamenskihLekova.Insert(idx1, lek);
                    }
                }
            }
            sacuvajIzmjene();
        }
        public static void obrisiSastojakLijeka(Lek izabraniLijek, Sastojak sastojak)
        {
            foreach (Lek lijek in lijekovi)
            {
                if (lijek.idLeka == izabraniLijek.idLeka)
                {
                    lijek.sastojci.Remove(sastojak);
                }
            }
            sacuvajIzmjene();
        }
        
        public static void obrisiSastojakLekaLekar(Lek izabraniLek, Sastojak sastojak)
        {
            foreach (Lek lek in lijekovi)
            {
                if (lek.idLeka == izabraniLek.idLeka)
                {
                    lek.sastojci.Remove(sastojak);
                    PrikazSastojakaLekar.TabelaSastojaka.Remove(sastojak);
                }
            }
            sacuvajIzmjene();
        }
        public static void izmjeniSastojakLijeka(Lek izabraniLijek, Sastojak stariSastojak, Sastojak noviSastojak)
        {
            foreach(Lek lijek in lijekovi)
            {
                if(lijek.idLeka == izabraniLijek.idLeka)
                {
                    foreach(Sastojak sastojak in lijek.sastojci)
                    {
                        if (sastojak.naziv.Equals(stariSastojak.naziv))
                        {
                            sastojak.naziv = noviSastojak.naziv;
                            sastojak.kolicina = noviSastojak.kolicina;
                            break;
                        }
                    }
                }
            }
            sacuvajIzmjene();
        }

        private static void izmjeniSastojakOdbijenog(Lek izabraniLijek, Sastojak izabraniSastojak, Sastojak uSastojak)
        {
            foreach (Sastojak sastojak in izabraniLijek.sastojci)
            {
                if (sastojak.naziv.Equals(izabraniSastojak.naziv))
                {
                    sastojak.naziv = uSastojak.naziv;
                    sastojak.kolicina = uSastojak.kolicina;
                }
            }
            sacuvajIzmeneZahteva();
        }

        public static void izmjeniSastojakOdbijenogLijeka(Lek izabraniLijek, Sastojak izabraniSastojak, Sastojak uSastojak)
        {
            foreach(ZahtevZaLekove zahtjev in zahteviZaLekove)
            {
                if (zahtjev.lek.sifraLeka.Equals(izabraniLijek.sifraLeka))
                {
                    izmjeniSastojakOdbijenog(zahtjev.lek, izabraniSastojak, uSastojak);
                }
            }
        }

        

        public static void izmeniSastojakLekaLekar(Lek izabraniLek, Sastojak stariSastojak, Sastojak noviSastojak)
        {
            foreach(Lek lek in lijekovi)
            {
                if(lek.idLeka == izabraniLek.idLeka)
                {
                    foreach(Sastojak sastojak in lek.sastojci)
                    {
                        if (sastojak.naziv.Equals(stariSastojak.naziv))
                        {
                            sastojak.naziv = noviSastojak.naziv;
                            sastojak.kolicina = noviSastojak.kolicina;
                            int idx = PrikazSastojakaLekar.TabelaSastojaka.IndexOf(stariSastojak);
                            PrikazSastojakaLekar.TabelaSastojaka.RemoveAt(idx);
                            PrikazSastojakaLekar.TabelaSastojaka.Insert(idx, sastojak);
                            break;
                        }
                    }
                }
            }
            sacuvajIzmjene();
        }

        public static void obrisiZamjenski(Lek izabraniLijek, Lek zamjenskiLijek)
        {
            foreach(Lek lijek in lijekovi)
            {
                if(lijek.idLeka == izabraniLijek.idLeka)
                {
                    lijek.zamenskiLekovi.Remove(zamjenskiLijek.idLeka);
                }
            }
            sacuvajIzmjene();
        }
        
        public static void obrisiZamenskiLekLekar(Lek izabraniLek, Lek zamenskiLek)
        {
            foreach(Lek lek in lijekovi)
            {
                if(lek.idLeka == izabraniLek.idLeka)
                {
                    lek.zamenskiLekovi.Remove(zamenskiLek.idLeka);
                    PrikazZamenskihLekovaLekar.TabelaZamenskihLekova.Remove(zamenskiLek);
                }
            }
            sacuvajIzmjene();
        }

        public static List<Lek> NadjiSveLijekove()
        {

            if (File.ReadAllText("lijekovi.xml").Trim().Equals(""))
            {
                return lijekovi;
            }
            else
            {
                FileStream filestream = File.OpenRead("lijekovi.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Lek>));
                lijekovi = (List<Lek>)serializer.Deserialize(filestream);
                filestream.Close();
                return lijekovi;
            }
        }
        public static void sacuvajIzmjene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Lek>));
            TextWriter filestream = new StreamWriter("lijekovi.xml");
            serializer.Serialize(filestream, lijekovi);
            filestream.Close();
        }



        public static List<Sastojak> nadjiSastojke(ZahtevZaLekove izabraniZahtev)
        {
            sastojci.Clear();
            foreach(ZahtevZaLekove zahtev in LekoviMenadzer.zahteviZaLekove)
            {
                if(zahtev.idZahteva == izabraniZahtev.idZahteva)
                {
                       foreach(Sastojak sastojak in zahtev.lek.sastojci)
                       {
                            sastojci.Add(sastojak);
                       }
                    
                }
            }
            Console.WriteLine(sastojci.Count);
            return sastojci;
        }
        


        public static List<ZahtevZaLekove> NadjiSveZahteve()
        {

            if (File.ReadAllText("zahtevi.xml").Trim().Equals(""))
            {
                return zahteviZaLekove;
            }
            else
            {
                FileStream filestream = File.OpenRead("zahtevi.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<ZahtevZaLekove>));
                zahteviZaLekove = (List<ZahtevZaLekove>)serializer.Deserialize(filestream);
                filestream.Close();
                return zahteviZaLekove;
            }
        }
        public static void sacuvajIzmeneZahteva()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ZahtevZaLekove>));
            TextWriter filestream = new StreamWriter("zahtevi.xml");
            serializer.Serialize(filestream, zahteviZaLekove);
            filestream.Close();
        }

        public static void izmeniZahtev(ZahtevZaLekove izabraniZahtev)
        {
            foreach(ZahtevZaLekove zahtev in zahteviZaLekove)
            {
                if(zahtev.idZahteva == izabraniZahtev.idZahteva)
                {
                    zahtev.obradjenZahtev = true;
                    zahtev.odobrenZahtev = true;

                    int id = SpisakZahtevaZaLekove.TabelaZahteva.IndexOf(izabraniZahtev);
                    SpisakZahtevaZaLekove.TabelaZahteva.RemoveAt(id);
                    SpisakZahtevaZaLekove.TabelaZahteva.Insert(id, zahtev);
                }
            }

            sacuvajIzmeneZahteva();
        }
        public static void odbijaZahtev(ZahtevZaLekove izabraniZahtev, String razlogOdbijanja)
        {
            foreach(ZahtevZaLekove zahtev in zahteviZaLekove)
            {
                if(zahtev.idZahteva == izabraniZahtev.idZahteva)
                {
                    zahtev.obradjenZahtev = true;
                    zahtev.odobrenZahtev = false;
                    zahtev.obrazlozenjeOdbijanja = razlogOdbijanja;

                    int id = SpisakZahtevaZaLekove.TabelaZahteva.IndexOf(izabraniZahtev);
                    SpisakZahtevaZaLekove.TabelaZahteva.RemoveAt(id);
                    SpisakZahtevaZaLekove.TabelaZahteva.Insert(id, zahtev);
                }
            }

            sacuvajIzmeneZahteva();
        }

        public static List<Sastojak> NadjiSveSastojke()
        {
            List<Sastojak> sviSastojci = new List<Sastojak>();
            foreach(Lek lek in lijekovi)
            {
                foreach(Sastojak sastojak in lek.sastojci)
                {
                    if (!PostojiSastojak(sastojak, sviSastojci))
                    {
                        sviSastojci.Add(sastojak);

                    }
                }
            }

            return sviSastojci;
        }

        public static bool PostojiSastojak(Sastojak noviSastojak, List<Sastojak> sastojci)
        {
            bool postoji = false;
            foreach (Sastojak sastojak in sastojci)
            {
                if (sastojak.naziv.Equals(noviSastojak.naziv))
                {
                    postoji = true;
                }
            }

            return postoji;
        }

        public static int GenerisanjeIdLijeka()
        {
            bool pomocna = false;
            int id = 1;

            for (id = 1; id <= LekoviMenadzer.lijekovi.Count + zahteviZaLekove.Count; id++)
            {
                foreach (Lek lijek in LekoviMenadzer.lijekovi)
                {
                    if (lijek.idLeka.Equals(id))
                    {
                        pomocna = true;
                        break;
                    }
                }
                foreach (ZahtevZaLekove zahtjev in NadjiSveZahteve())//da u zahtjeve ne ide isti id 
                {
                    if (zahtjev.lek.idLeka.Equals(id))
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

        public static int GenerisanjeIdZahtjeva()
        {
            bool pomocna = false;
            int id = 1;

            for (id = 1; id <= LekoviMenadzer.zahteviZaLekove.Count; id++)
            {
                foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
                {
                    if (zahtjev.idZahteva.Equals(id))
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

        public static List<Sastojak> sastojci = new List<Sastojak>();
        public static List<Lek> lijekovi = new List<Lek>();
        public static List<ZahtevZaLekove> zahteviZaLekove = new List<ZahtevZaLekove>();

    }
}
