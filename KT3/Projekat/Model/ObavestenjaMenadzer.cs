using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Projekat.Model
{
    public static class ObavestenjaMenadzer
    {
        public static List<Obavestenja> obavestenja = new List<Obavestenja>();

        public static void sacuvajIzmene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Obavestenja>));
            TextWriter fileStream = new StreamWriter("obavestenja.xml");
            serializer.Serialize(fileStream, obavestenja);
            fileStream.Close();
        }
        public static List<Obavestenja> NadjiSvaObavestenja()
        {
            if (File.ReadAllText("obavestenja.xml").Trim().Equals(""))
            {
                return obavestenja;
            }
            else
            {
                FileStream fileStream = File.OpenRead("obavestenja.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Obavestenja>));
                obavestenja = (List<Obavestenja>)serializer.Deserialize(fileStream);
                fileStream.Close();
                return obavestenja;
            }
        }

        public static void DodajObavestenje(Obavestenja novoObavestenje)
        {
            obavestenja.Add(novoObavestenje);
            if (OglasnaTabla.oglasnaTabla == null)
            {
                OglasnaTabla.oglasnaTabla = new ObservableCollection<Obavestenja>();
            }
            OglasnaTabla.oglasnaTabla.Add(novoObavestenje);

            ObavestenjaMenadzer.sacuvajIzmene();
        }

     /*   public static void IzmeniObavestenje(Obavestenja staroObavestenje, Obavestenja novoObavestenje)
        {
            foreach (Obavestenja obavestenje in obavestenja)
            {
                if (obavestenje.datum == nalog1.IdPacijenta)
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
                    p.Maloletnik = nalog.Maloletnik;
                    p.JmbgStaratelja = nalog.JmbgStaratelja;
                    
                    int idx = PrikaziPacijenta.PacijentiTabela.IndexOf(nalog1);
                    PrikaziPacijenta.PacijentiTabela.RemoveAt(idx);
                    PrikaziPacijenta.PacijentiTabela.Insert(idx, p);
                }
            }
            
        }

        public static void ObrisiObavestenje(Obavestenja obavestenje)
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
                    }
                }
            }
        }*/

        public static List<Obavestenja> PronadjiSve()
        {
            if (File.ReadAllText("obavestenja.xml").Trim().Equals(""))
            {
                return obavestenja;
            }
            else
            {
                FileStream filestream = File.OpenRead("obavestenja.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Obavestenja>));
                obavestenja = (List<Obavestenja>)serializer.Deserialize(filestream);
                filestream.Close();
                return obavestenja;
            }
        }

        public static Obavestenja PronadjiPoId(int id)
        {
            foreach (Obavestenja obavestenje in obavestenja)
            {
                if (obavestenje.IdObavestenja == id)
                {
                    return obavestenje;
                }
            }
            return null;
        }

        public static int GenerisanjeIdObavestenja()
        {
            bool pomocna = false;
            int id = 1;

            for (id = 1; id <= obavestenja.Count; id++)
            {
                foreach (Obavestenja obavestenje in obavestenja)
                {
                    if (obavestenje.IdObavestenja == id)
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

    }
}
