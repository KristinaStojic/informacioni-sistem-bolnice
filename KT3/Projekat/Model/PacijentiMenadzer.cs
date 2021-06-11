/***********************************************************************
 * Module:  NaloziFileManager.cs
 * Author:  Teodora
 * Purpose: Definition of the Class NaloziFileManager
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;
using Projekat;
using Projekat.Model;
using Projekat.Servis;

namespace Model
{
    public class PacijentiMenadzer : XMLSerialization<Pacijent>
    {
        ObavestenjaServis servis = new ObavestenjaServis();

        public override void Dodaj(Pacijent element, string fajl)
        {
            List<Pacijent> lista = NadjiSve(fajl);
            lista.Add(element);
            PrikaziPacijenta.PacijentiTabela.Add(element);
            SacuvajIzmene(fajl, lista);
        }

        public override void Izmeni(Pacijent stariNalog, Pacijent noviNalog, string fajl)
        {
            foreach (Pacijent p in NadjiSve(fajl))
            {
                if (p.IdPacijenta == stariNalog.IdPacijenta)
                {
                    p.ImePacijenta = noviNalog.ImePacijenta;
                    p.PrezimePacijenta = noviNalog.PrezimePacijenta;
                    p.Jmbg = noviNalog.Jmbg;
                    p.Pol = noviNalog.Pol;
                    p.StatusNaloga = noviNalog.StatusNaloga;
                    p.BrojTelefona = noviNalog.BrojTelefona;
                    p.Email = noviNalog.Email;
                    p.AdresaStanovanja = noviNalog.AdresaStanovanja;
                    p.BracnoStanje = noviNalog.BracnoStanje;
                    p.Zanimanje = noviNalog.Zanimanje;
                    p.Maloletnik = noviNalog.Maloletnik;
                    p.JmbgStaratelja = noviNalog.JmbgStaratelja;

                    if (noviNalog.Karton != null) // ako menjamo nalog iz guest u stalan
                    {
                        p.Karton = noviNalog.Karton;
                    }

                    int idx = PrikaziPacijenta.PacijentiTabela.IndexOf(stariNalog);
                    PrikaziPacijenta.PacijentiTabela.RemoveAt(idx);
                    PrikaziPacijenta.PacijentiTabela.Insert(idx, p);
                }
            }
            SacuvajIzmene(fajl, PrikaziPacijenta.PacijentiTabela.ToList());
        }

        public override void Obrisi(Pacijent element, string fajl)
        {
            ObrisiObavestenjaPacijenta(element);
            List<Pacijent> lista = NadjiSve(fajl);

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].IdPacijenta == element.IdPacijenta)
                {
                    lista.RemoveAt(i);
                    PrikaziPacijenta.PacijentiTabela.Remove(element);
                    ObrisiTerminePacijenta(element);
                }
            }
            SacuvajIzmene(fajl, lista);
        }

        public void ObrisiObavestenjaPacijenta(Pacijent nalog)
        {
            foreach (Obavestenja obavestenje in servis.NadjiSvaObavestenja().ToList())
            {
                if (obavestenje.ListaIdPacijenata.Contains(nalog.IdPacijenta) && obavestenje.ListaIdPacijenata.Count == 1)
                {
                    servis.ObrisiObavestenje(obavestenje);
                }
                else if (obavestenje.ListaIdPacijenata.Contains(nalog.IdPacijenta) && obavestenje.ListaIdPacijenata.Count > 1)
                {
                    obavestenje.ListaIdPacijenata.Remove(nalog.IdPacijenta);
                }
            }
        }

        private void ObrisiTerminePacijenta(Pacijent nalog)
        {
            for (int j = 0; j < TerminMenadzer.termini.Count; j++)
            {
                if (TerminMenadzer.termini[j].Pacijent.IdPacijenta == nalog.IdPacijenta)
                {
                    ObrisiZauzecaSala(j);
                    TerminMenadzer.termini.RemoveAt(j);
                    j--;
                }
            }
        }

        private void ObrisiZauzecaSala(int j)
        {
            foreach (Sala s in SaleMenadzer.lista)
            {
                if (s.Id == TerminMenadzer.termini[j].Prostorija.Id)
                {
                    s.zauzetiTermini.Remove(SaleServis.NadjiZauzece(s.Id, TerminMenadzer.termini[j].IdTermin, TerminMenadzer.termini[j].Datum, TerminMenadzer.termini[j].VremePocetka, TerminMenadzer.termini[j].VremeKraja));
                }
            }
        }

    }
}