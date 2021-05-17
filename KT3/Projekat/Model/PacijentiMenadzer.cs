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
    public static class PacijentiMenadzer
    {
        public static List<Pacijent> pacijenti = new List<Pacijent>();

        public static void DodajNalog(Pacijent noviNalog)
        {
            pacijenti.Add(noviNalog);
            PrikaziPacijenta.PacijentiTabela.Add(noviNalog);
        }

        public static void IzmeniNalog(Pacijent stariNalog, Pacijent noviNalog)
        {
            foreach (Pacijent p in pacijenti)
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
                    
                    int idx = PrikaziPacijenta.PacijentiTabela.IndexOf(stariNalog);
                    PrikaziPacijenta.PacijentiTabela.RemoveAt(idx);
                    PrikaziPacijenta.PacijentiTabela.Insert(idx, p);
                }
            }
            
        }

        // Sanja 
        public static void IzmeniNalogPacijent(Pacijent stari, Pacijent nalog)
        {
            foreach (Pacijent p in pacijenti)
            {
                if (p.IdPacijenta == stari.IdPacijenta)
                {
                    p.ImePacijenta = nalog.ImePacijenta;
                    p.PrezimePacijenta = nalog.PrezimePacijenta;
                    p.Jmbg = nalog.Jmbg;
                    p.Pol = nalog.Pol;
                    p.StatusNaloga = nalog.StatusNaloga;
                    p.BrojTelefona = nalog.BrojTelefona;
                    p.Email = nalog.Email;
                    p.AdresaStanovanja = nalog.AdresaStanovanja;
                    p.BracnoStanje = nalog.BracnoStanje;
                    p.Zanimanje = nalog.Zanimanje;
                    p.IzabraniLekar = nalog.IzabraniLekar;
                    //p.Maloletnik = nalog.Maloletnik;
                    //p.JmbgStaratelja = nalog.JmbgStaratelja;

                    for (int i = 0; i < PrikaziTermin.Termini.Count; i++)
                    {
                        if (PrikaziTermin.Termini[i].Pacijent.IdPacijenta.Equals(stari.IdPacijenta))
                        {
                            MessageBox.Show(i.ToString());
                            //int i = PrikaziTermin.Termini.IndexOf(stari)
                            // PrikaziTermin.Termini.RemoveAt(i);
                            PrikaziTermin.Termini[i].Pacijent = p;   //*
                           // PrikaziTermin.Termini.Insert(i, PrikaziTermin.Termini[i]);
                        }
                    }
                }
            }
        }

        public static void ObrisiNalog(Pacijent nalog)
        {
            // obrisi iz obavestenja
            foreach (Obavestenja o in ObavestenjaMenadzer.obavestenja.ToList())
            {
                if (o.ListaIdPacijenata.Contains(nalog.IdPacijenta) && o.ListaIdPacijenata.Count == 1)
                {
                    ObavestenjaServis.ObrisiObavestenje(o);
                    ObavestenjaServis.sacuvajIzmene();
                }
                else if (o.ListaIdPacijenata.Contains(nalog.IdPacijenta) && o.ListaIdPacijenata.Count > 1)
                {
                    o.ListaIdPacijenata.Remove(nalog.IdPacijenta);
                    ObavestenjaServis.sacuvajIzmene();
                }
            }

            for (int i = 0; i < pacijenti.Count; i++)
            {
                if (pacijenti[i].IdPacijenta == nalog.IdPacijenta)
                {
                    pacijenti.RemoveAt(i);
                    PrikaziPacijenta.PacijentiTabela.Remove(nalog);
                    
                    for (int j = 0; j < TerminMenadzer.termini.Count; j++)
                    {
                        if (TerminMenadzer.termini[j].Pacijent.IdPacijenta == nalog.IdPacijenta)
                        {   
                            foreach (Sala s in SaleMenadzer.sale)
                            {
                                if (s.Id == TerminMenadzer.termini[j].Prostorija.Id)
                                {
                                    s.zauzetiTermini.Remove(SaleServis.NadjiZauzece(s.Id, TerminMenadzer.termini[j].IdTermin, TerminMenadzer.termini[j].Datum, TerminMenadzer.termini[j].VremePocetka, TerminMenadzer.termini[j].VremeKraja));
                                    //SaleMenadzer.sacuvajIzmjene();
                                }
                            }

                            TerminMenadzer.termini.RemoveAt(j);
                            j--;
                        }
                    }
                }
            }
        }

        public static List<Pacijent> PronadjiSve()
        {
            if (File.ReadAllText("pacijenti.xml").Trim().Equals(""))
            {
                return pacijenti;
            }
            else
            {
                FileStream filestream = File.OpenRead("pacijenti.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Pacijent>));
                pacijenti = (List<Pacijent>)serializer.Deserialize(filestream);
                filestream.Close();
                return pacijenti;
            }
        }

        public static Pacijent PronadjiPoId(int id)
        {
            foreach (Pacijent p in pacijenti)
            {
                if (p.IdPacijenta == id)
                {
                    return p;
                }
            }
            return null;
        }

        public static int GenerisanjeIdPacijenta()
        {
            bool pomocna = false;
            int id = 1;

            for (id = 1; id <= pacijenti.Count; id++)
            {
                foreach (Pacijent p in pacijenti)
                {
                    if (p.IdPacijenta.Equals(id))
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

        public static void SacuvajIzmenePacijenta()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Pacijent>));
            TextWriter filestream = new StreamWriter("pacijenti.xml");
            serializer.Serialize(filestream, pacijenti);
            filestream.Close();
        }

    }
}