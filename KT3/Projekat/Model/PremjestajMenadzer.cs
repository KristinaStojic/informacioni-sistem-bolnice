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
            PremjestajMenadzer.sacuvajIzmjene();
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

        public static void odradiZakazano()
        {
            foreach(Premjestaj p in premjestaji.ToList())
            {
                if (p.datumIVrijeme.Date.ToString().Equals(DateTime.Now.Date.ToString()))
                {
                    if (p.datumIVrijeme.TimeOfDay <= DateTime.Now.TimeOfDay)
                    {
                        if (p.salji)
                        {
                           
                            Sala izabranaSala = p.izSale;
                            Sala salaDodavanje = p.uSalu;
                            int kolicina = p.kolicina;
                            int x = 0;
                            Oprema izabranaOprema = p.oprema;
                            foreach (Sala s in SaleMenadzer.sale)
                            {
                                if (s.Id == izabranaSala.Id)
                                {
                                    foreach (Oprema o in s.Oprema)
                                    {
                                        if (o.IdOpreme == izabranaOprema.IdOpreme)
                                        {
                                            o.Kolicina -= kolicina;
                                            if (s.Namjena.Equals("Skladiste"))
                                            {
                                                if (o.Kolicina == 0)
                                                {
                                                    if (Skladiste.OpremaStaticka != null)
                                                    {
                                                        s.Oprema.Remove(o);
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
                                                   /* if (PrikazStaticke.OpremaStaticka != null)
                                                    {*/
                                                        s.Oprema.Remove(o);
                                                    if (PrikazStaticke.otvoren)
                                                    {
                                                        PrikazStaticke.azurirajPrikaz();
                                                    }
                                                        break;
                                                    //}
                                                }
                                                if (PrikazStaticke.otvoren)
                                                {
                                                    PrikazStaticke.azurirajPrikaz();
                                                }
                                            }

                                        }
                                    }
                                }
                                if (s.Id == salaDodavanje.Id)
                                {
                                    foreach (Oprema o in s.Oprema)
                                    {
                                        if (o.IdOpreme == izabranaOprema.IdOpreme)
                                        {
                                            o.Kolicina += kolicina;
                                            x += 1;
                                            if (s.Namjena.Equals("Skladiste"))
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
                                        s.Oprema.Add(op);
                                       //SaleMenadzer.sacuvajIzmjene();
                                        if (salaDodavanje.Namjena.Equals("Skladiste"))
                                        {
                                            if (Skladiste.OpremaStaticka != null)
                                            {
                                                Skladiste.OpremaStaticka.Add(op);
                                            }
                                        }
                                        else
                                        {
                                            /*if (PrikazStaticke.OpremaStaticka != null)
                                            {*/
                                            if (PrikazStaticke.otvoren)
                                            {
                                                PrikazStaticke.azurirajPrikaz();
                                            }
                                            //}
                                        }
                                        
                                    }else
                                    {
                                        x = 0;
                                    }

                                }
                            }
                            premjestaji.Remove(p);
                            sacuvajIzmjene();
                        }
                        else
                        {
                            Sala izabranaSala = p.izSale;
                            Sala salaDodavanje = p.uSalu;
                            int kolicina = p.kolicina;
                            int x = 0;
                            Oprema izabranaOprema = p.oprema;

                            foreach (Sala s in SaleMenadzer.sale)
                            {
                                if (s.Id == izabranaSala.Id)
                                {
                                    foreach (Oprema o in s.Oprema)
                                    {
                                        if (o.IdOpreme == izabranaOprema.IdOpreme)
                                        {
                                            o.Kolicina -= kolicina;
                                            if (s.Namjena.Equals("Skladiste"))
                                            {
                                                if (o.Kolicina == 0)
                                                {
                                                    if (Skladiste.OpremaStaticka != null)
                                                    {
                                                        s.Oprema.Remove(o);
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
                                                    /* if (PrikazStaticke.OpremaStaticka != null)
                                                     {*/
                                                    s.Oprema.Remove(o);
                                                    if (PrikazStaticke.otvoren)
                                                    {
                                                        PrikazStaticke.azurirajPrikaz();
                                                    }
                                                    break;
                                                    //}
                                                }
                                                if (PrikazStaticke.otvoren)
                                                {
                                                    PrikazStaticke.azurirajPrikaz();
                                                }
                                            }

                                        }
                                    }
                                }
                                if (s.Id == salaDodavanje.Id)
                                {
                                    foreach (Oprema o in s.Oprema)
                                    {
                                        if (o.IdOpreme == izabranaOprema.IdOpreme)
                                        {
                                            o.Kolicina += kolicina;
                                            x += 1;
                                            if (s.Namjena.Equals("Skladiste"))
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
                                        s.Oprema.Add(op);
                                        //SaleMenadzer.sacuvajIzmjene();
                                        if (salaDodavanje.Namjena.Equals("Skladiste"))
                                        {
                                            if (Skladiste.OpremaStaticka != null)
                                            {
                                                Skladiste.OpremaStaticka.Add(op);
                                            }
                                        }
                                        else
                                        {
                                            /*if (PrikazStaticke.OpremaStaticka != null)
                                            {*/
                                            if (PrikazStaticke.otvoren)
                                            {
                                                PrikazStaticke.azurirajPrikaz();
                                            }
                                            //}
                                        }

                                    }else
                                    {
                                        x = 0;
                                    }

                                }
                            }
                            premjestaji.Remove(p);
                            sacuvajIzmjene();
                        }
                    }
                }
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
