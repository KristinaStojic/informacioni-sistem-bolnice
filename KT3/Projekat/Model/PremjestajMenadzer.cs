using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Projekat.Model
{
    class PremjestajMenadzer
    {
        public static void dodajPremjestaj(Premjestaj p)
        {
            premjestaji.Add(p);
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
                FileStream filestream = File.OpenRead("premjestaj.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Premjestaj>));
                premjestaji = (List<Premjestaj>)serializer.Deserialize(filestream);
                filestream.Close();
                return premjestaji;
            }
        }

        public static void sacuvajIzmjene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Premjestaj>));
            TextWriter filestream = new StreamWriter("premjestaj.xml");
            serializer.Serialize(filestream, premjestaji);
            filestream.Close();
        }
        private static bool provjeri(Premjestaj premjestaj)
        {
            if (premjestaj.datumIVrijeme.Date.ToString().Equals(DateTime.Now.Date.ToString()))
            {
                return false; 
            }
            if (premjestaj.datumIVrijeme.TimeOfDay <= DateTime.Now.TimeOfDay)
            {
                return false;
            }
            return true;
        }
        public static void odradiZakazanePremjestaje()
        {
            foreach(Premjestaj premjestaj in premjestaji.ToList())
            {
                if (provjeri(premjestaj))
                {
                    continue;
                }
                Sala izabranaSala = premjestaj.izSale;
                Sala salaDodavanje = premjestaj.uSalu;
                int kolicina = premjestaj.kolicina;
                int x = 0;
                Oprema izabranaOprema = premjestaj.oprema;
                foreach (Sala sala in SaleMenadzer.sale)
                {
                    if (sala.Id == izabranaSala.Id)
                    {
                        foreach (Oprema o in sala.Oprema)
                        {
                            if (o.IdOpreme == izabranaOprema.IdOpreme)
                            {
                                o.Kolicina -= kolicina;
                                if (sala.Namjena.Equals("Skladiste"))
                                {
                                    if (o.Kolicina == 0)
                                    {
                                        if (Skladiste.OpremaStaticka != null)
                                        {
                                            sala.Oprema.Remove(o);
                                            Skladiste.OpremaStaticka.Remove(o);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (Skladiste.OpremaStaticka != null)
                                        {
                                            int idx = Skladiste.OpremaStaticka.IndexOf(o);
                                            Skladiste.OpremaStaticka.RemoveAt(idx);
                                            Skladiste.OpremaStaticka.Insert(idx, o);
                                        }
                                    }
                                }
                                else
                                {
                                    if (o.Kolicina == 0)
                                    {

                                        sala.Oprema.Remove(o);
                                        if (PrikazStaticke.otvoren)
                                        {
                                            PrikazStaticke.azurirajPrikaz();
                                        }
                                        break;
                                    }
                                    if (PrikazStaticke.otvoren)
                                    {
                                        PrikazStaticke.azurirajPrikaz();
                                    }
                                }

                            }
                        }
                    }
                    if (sala.Id == salaDodavanje.Id)
                    {
                        foreach (Oprema o in sala.Oprema)
                        {
                            if (o.IdOpreme == izabranaOprema.IdOpreme)
                            {
                                o.Kolicina += kolicina;
                                x += 1;
                                if (sala.Namjena.Equals("Skladiste"))
                                {
                                    if (Skladiste.OpremaStaticka != null)
                                    {
                                        int idx = Skladiste.OpremaStaticka.IndexOf(o);
                                        Skladiste.OpremaStaticka.RemoveAt(idx);
                                        Skladiste.OpremaStaticka.Insert(idx, o);
                                    }
                                }
                                else
                                {

                                    if (PrikazStaticke.otvoren)
                                    {
                                        PrikazStaticke.azurirajPrikaz();
                                    }
                                }
                            }


                        }
                        if (x == 0)
                        {
                            Oprema op = new Oprema(izabranaOprema.NazivOpreme, kolicina, true);
                            op.IdOpreme = izabranaOprema.IdOpreme;
                            sala.Oprema.Add(op);
                            if (salaDodavanje.Namjena.Equals("Skladiste"))
                            {
                                if (Skladiste.OpremaStaticka != null)
                                {
                                    Skladiste.OpremaStaticka.Add(op);
                                }
                            }
                            else
                            {
                                if (PrikazStaticke.otvoren)
                                {
                                    PrikazStaticke.azurirajPrikaz();
                                }
                            }

                        }
                        else
                        {
                            x = 0;
                        }

                    }
                }
                premjestaji.Remove(premjestaj);
                sacuvajIzmjene();
            
        }
        }
       
        public static int GenerisanjeIdPremjestaja()
        {
            bool pomocna = false;
            int id = 1;

            for (id = 1; id <= premjestaji.Count; id++)
            {
                foreach (Premjestaj s in premjestaji)
                {
                    if (s.id.Equals(id))
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
        public static List<Premjestaj> premjestaji = new List<Premjestaj>();
    }
}
