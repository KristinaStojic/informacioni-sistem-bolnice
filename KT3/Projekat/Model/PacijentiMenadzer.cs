/***********************************************************************
 * Module:  NaloziFileManager.cs
 * Author:  Teodora
 * Purpose: Definition of the Class NaloziFileManager
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using Projekat;
using Projekat.Model;

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

        public static void IzmeniNalog(Pacijent nalog1, Pacijent nalog)
        {
            foreach (Pacijent p in pacijenti)
            {
                if (p.IdPacijenta == nalog1.IdPacijenta)
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
                    
                    int idx = PrikaziPacijenta.PacijentiTabela.IndexOf(nalog1);
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

                    for (int i = 0; i < PrikaziTermin.Termini.Count; i++)
                    {
                        if (PrikaziTermin.Termini[i].Pacijent.Equals(stari))
                        {
                            // TODO: ispraviti
                            MessageBox.Show(i.ToString());
                            PrikaziTermin.Termini.RemoveAt(i);
                            PrikaziTermin.Termini[i].Pacijent = p;   //*
                            PrikaziTermin.Termini.Insert(i, PrikaziTermin.Termini[i]);
                        }
                    }
                }
            }
        }

        public static void ObrisiNalog(Pacijent nalog)
        {  
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

        // napraviti da bude po id
        public static Pacijent PronadjiPoId(int jmbg)
        {
            foreach (Pacijent p in pacijenti)
            {
                if (p.Jmbg == jmbg)
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