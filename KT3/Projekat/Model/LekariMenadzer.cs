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
    public class LekariMenadzer: XMLSerialization<Lekar>
    {
        PacijentiMenadzer menadzer = new PacijentiMenadzer();
        ZahteviZaGodisnjiMenadzer godisnjiMenadzer = new ZahteviZaGodisnjiMenadzer();
        public override void Dodaj(Lekar noviLekar, string fajl)
        {
            List<Lekar> lista = NadjiSve(fajl);
            noviLekar.lozinka = noviLekar.Jmbg.ToString();
            noviLekar.korisnickoIme = noviLekar.PrezimeLek;
            lista.Add(noviLekar);
            PrikaziLekare.Lekari.Add(noviLekar);
            SacuvajIzmene(fajl, lista);
        }

        public override void Izmeni(Lekar stariLekar, Lekar noviLekar, string lokacijaFajla)
        {
            foreach (Lekar l in NadjiSve(lokacijaFajla))
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

                    int idx = PrikaziLekare.Lekari.IndexOf(stariLekar);
                    PrikaziLekare.Lekari.RemoveAt(idx);
                    PrikaziLekare.Lekari.Insert(idx, l);
                    SacuvajIzmene(lokacijaFajla, PrikaziLekare.Lekari.ToList());
                }
            }
        }

        public override void Obrisi(Lekar lekar, string fajl)
        { 
            ObrisiUpute(lekar);
            ObrisiRecepte(lekar);
            ObrisiZahteveZaGodisnji(lekar);
            ObrisiIzabranogLekara(lekar);
            ObrisiTermineZaLekara(lekar);

            List<Lekar> lista = NadjiSve(fajl);

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].IdLekara == lekar.IdLekara)
                {
                    lista.RemoveAt(i);
                    PrikaziLekare.Lekari.Remove(lekar);
                }
            }
            SacuvajIzmene(fajl, lista);

        }

        public void ObrisiTermineZaLekara(Lekar lekar)
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

        public void ObrisiZahteveZaGodisnji(Lekar lekar)
        {
            for (int i = 0; i < godisnjiMenadzer.NadjiSveZahteve().Count; i++)
            {
                if (godisnjiMenadzer.NadjiSveZahteve()[i].lekar.IdLekara == lekar.IdLekara)
                {
                    godisnjiMenadzer.NadjiSveZahteve().RemoveAt(i);
                    i--;
                    godisnjiMenadzer.sacuvajIzmjeneZahteva();
                }
            }
        }

        public void ObrisiRecepte(Lekar lekar)
        {
            List<Pacijent> lista = menadzer.NadjiSve("pacijenti.xml");
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].Karton == null)
                {
                    return;
                }
                else 
                { 
                    for (int j = 0; j < lista[i].Karton.LekarskiRecepti.Count; j++)
                    {
                        if (lista[i].Karton.LekarskiRecepti[j].IdLekara == lekar.IdLekara)
                        {
                            lista[i].Karton.LekarskiRecepti.RemoveAt(j);
                            j--;
                            menadzer.SacuvajIzmene("pacijenti.xml", lista);
                        }
                    }
                }
            }
        }

        public void ObrisiUpute(Lekar lekar)
        {
            List<Pacijent> lista = menadzer.NadjiSve("pacijenti.xml");
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].Karton == null)
                {
                    return;
                }
                else
                { 
                    for (int j = 0; j < lista[i].Karton.Uputi.Count; j++)
                    {
                        if (lista[i].Karton.Uputi[j].IdLekaraKodKogSeUpucuje == lekar.IdLekara
                            || lista[i].Karton.Uputi[j].IdLekaraKojiIzdajeUput == lekar.IdLekara)
                        {
                            lista[i].Karton.Uputi.RemoveAt(j);
                            j--;
                            menadzer.SacuvajIzmene("pacijenti.xml", lista);
                        }
                    }
                }
            }
        }

        public void ObrisiIzabranogLekara(Lekar lekar)
        {
            List<Pacijent> lista = menadzer.NadjiSve("pacijenti.xml");
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].IzabraniLekar != null)
                {
                    if (lista[i].IzabraniLekar.IdLekara == lekar.IdLekara)
                    {
                        lista[i].IzabraniLekar = new Lekar();
                        menadzer.SacuvajIzmene("pacijenti.xml", lista);
                    }
                }
            }
        }

    }
}
