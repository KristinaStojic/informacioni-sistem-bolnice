/***********************************************************************
 * Module:  TerminMenadzer.cs
 * Author:  Kristina
 * Purpose: Definition of the Class TerminMenadzer
 ***********************************************************************/

using Projekat;
using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml.Serialization;


namespace Model
{
    public class TerminMenadzer
    {
        public static int index;
        public static void ZakaziTermin(Termin termin)
        {
            termini.Add(termin);
            PrikaziTermin.Termini.Add(termin);
            // TODO:
            //SaleMenadzer.sacuvajIzmjene();
            //PrikazTerminaLekar.Termini.Add(termin);
            //PrikaziTerminSekretar.TerminiSekretar.Add(termin);
        }

        public static void ZakaziTerminSekretar(Termin termin)
        {
            termini.Add(termin);
            PrikaziTerminSekretar.TerminiSekretar.Add(termin);

            // notifikacije 
            int idObavestenja = ObavestenjaServis.GenerisanjeIdObavestenja();
            List<int> ListaIdPacijenata = new List<int>();
            ListaIdPacijenata.Add(termin.Pacijent.IdPacijenta);
            Obavestenja o = new Obavestenja(idObavestenja, termin.Datum, termin.tipTermina.ToString(), "Zakazan termin u prostoriji" + " " + termin.Prostorija.brojSale + ", " + termin.VremePocetka + "- " + termin.VremeKraja, ListaIdPacijenata, termin.Lekar.IdLekara, true, "");
            ObavestenjaMenadzer.obavestenja.Add(o);
            ObavestenjaServis.sacuvajIzmene();
        }

        // hitan slucaj
        public static void ZakaziHitanTermin(Termin hitanTermin, string datum)
        {
            ZakaziTerminSekretar(hitanTermin);

            // TODO: ovaj deo direktno u ZakaziTerminSekretar
            ZauzeceSale novoZauzece = new ZauzeceSale(hitanTermin.VremePocetka, hitanTermin.VremeKraja, datum, hitanTermin.IdTermin);
            Sala sala = SaleServis.NadjiSaluPoId(hitanTermin.Prostorija.Id);
            sala.zauzetiTermini.Add(novoZauzece);
            SaleServis.sacuvajIzmjene();

            DodajZauzeceUSveSale(sala);
        }

        public static void DodajZauzeceUSveSale(Sala sala)
        {
            foreach (Termin termin in termini)
            {
                if (termin.Prostorija.Id == sala.Id)
                {
                    termin.Prostorija = sala;
                }
            }
            sacuvajIzmene();
            SaleServis.sacuvajIzmjene();
        }

        public static void ZakaziTerminLekar(Termin termin)
        {
            termini.Add(termin);
            PrikazTerminaLekar.Termini.Add(termin);
        }

        public static int GenerisanjeIdTermina()
        {
            bool pomocna = false;
            int id = 1;

            for (id = 1; id <= termini.Count; id++)
            {
                foreach (Termin p in termini)
                {
                    if (p.IdTermin.Equals(id))
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

        public static void IzmeniTermin(Termin stariTermin, Termin noviTermin)
        {
            foreach (Termin termin in termini)
            {
                if (termin.IdTermin == stariTermin.IdTermin)
                {
                    AnketaServis.IzmeniAnketuZaLekara(stariTermin, noviTermin);
                    termin.IdTermin = noviTermin.IdTermin;
                    termin.VremePocetka = noviTermin.VremePocetka;
                    termin.VremeKraja = noviTermin.VremeKraja;
                    termin.Lekar = noviTermin.Lekar;
                    termin.Pacijent = noviTermin.Pacijent;
                    termin.tipTermina = noviTermin.tipTermina;
                    termin.Datum = noviTermin.Datum;
                    termin.Prostorija = noviTermin.Prostorija;
                    termin.Pomeren = noviTermin.Pomeren;
                    termin.HitnaOperacija = noviTermin.HitnaOperacija;
                    int idx = PrikaziTermin.Termini.IndexOf(stariTermin);
                    PrikaziTermin.Termini.RemoveAt(idx);
                    PrikaziTermin.Termini.Insert(idx, noviTermin);
                    foreach (Sala sala in SaleMenadzer.sale)
                    {
                        foreach (ZauzeceSale zauzece in sala.zauzetiTermini)
                        {
                            if (zauzece.idTermina.Equals(stariTermin.IdTermin))
                            {
                                // MessageBox.Show(stariTermin.IdTermin.ToString());
                                sala.zauzetiTermini.Remove(zauzece);
                                return;
                            }
                        }
                    }

                }
            }
        }

        public static void IzmeniTerminLekar(Termin termin, Termin termin1)
        {
            foreach (Termin t in termini)
            {
                if (t.IdTermin == termin.IdTermin)
                {
                    t.IdTermin = termin1.IdTermin;
                    t.VremePocetka = termin1.VremePocetka;
                    t.VremeKraja = termin1.VremeKraja;
                    t.Lekar = termin1.Lekar;  // ili preko id-ja?
                    t.Pacijent = termin1.Pacijent;
                    t.tipTermina = termin1.tipTermina;
                    t.Datum = termin1.Datum;
                    t.Prostorija = termin1.Prostorija;
                    //Console.WriteLine(termin1.Pacijent.ImePacijenta + "  "  + termin1.Pacijent.PrezimePacijenta);
                }

            }

            //  **** napraviti metodu --> izmeniTerminLekar(...)
            int idx = PrikazTerminaLekar.Termini.IndexOf(termin);
            PrikazTerminaLekar.Termini.RemoveAt(idx);
            PrikazTerminaLekar.Termini.Insert(idx, termin1);
        }

        public static void IzmeniTerminSekretar(Termin stariTermin, Termin noviTermin)
        {
            foreach (Termin t in termini)
            {
                if (t.IdTermin == stariTermin.IdTermin)
                {
                    t.IdTermin = noviTermin.IdTermin;
                    t.VremePocetka = noviTermin.VremePocetka;
                    t.VremeKraja = noviTermin.VremeKraja;
                    t.Lekar = noviTermin.Lekar;
                    t.Pacijent = noviTermin.Pacijent;
                    t.tipTermina = noviTermin.tipTermina;
                    t.Datum = noviTermin.Datum;
                    t.Prostorija = noviTermin.Prostorija;

                    index = PrikaziTerminSekretar.TerminiSekretar.IndexOf(stariTermin);
                    PrikaziTerminSekretar.TerminiSekretar.RemoveAt(index);
                }
            }

            // brisanje termina
            for (int i = 0; i < termini.Count; i++)
            {
                if (stariTermin.IdTermin == termini[i].IdTermin)
                {
                    // brisanje termina iz zauzetih termina u sali
                    foreach (Sala s in SaleMenadzer.sale)
                    {
                        if (s.Id == stariTermin.Prostorija.Id)
                        {
                            s.zauzetiTermini.Remove(SaleServis.NadjiZauzece(s.Id, stariTermin.IdTermin, stariTermin.Datum, stariTermin.VremePocetka, stariTermin.VremeKraja));
                            SaleServis.sacuvajIzmjene();
                        }
                    }
                    termini.RemoveAt(i);
                }
            }

            // brisanje otkazanih termina iz zauzeca sala unutar drugih termina
            foreach (Termin t in TerminMenadzer.termini)
            {
                if (t.Prostorija.Id == stariTermin.Prostorija.Id)
                {
                    t.Prostorija = SaleServis.NadjiSaluPoId(stariTermin.Prostorija.Id);
                    SaleServis.sacuvajIzmjene();
                }
            }

            // dodavanje novog
            termini.Add(noviTermin);
            PrikaziTerminSekretar.TerminiSekretar.Insert(index, noviTermin);

            // za svaki termin koji je zakazan u istoj prostoriji, dodati to novo zauzece u zauzeca te prostorije (dodavanje novog izmenjenog termina)
            foreach (Termin t1 in TerminMenadzer.termini)
            {
                if (t1.Prostorija.Id == noviTermin.Prostorija.Id)
                {
                    t1.Prostorija = noviTermin.Prostorija;
                }
            }

            // notifikacija 
            int idObavestenja = ObavestenjaServis.GenerisanjeIdObavestenja();
            List<int> ListaIdPacijenata = new List<int>();
            ListaIdPacijenata.Add(stariTermin.Pacijent.IdPacijenta);
            Obavestenja o = new Obavestenja(idObavestenja, stariTermin.Datum, stariTermin.tipTermina.ToString(), "Izmenjen termin u prostoriji" + " " + stariTermin.Prostorija.brojSale + ", " + stariTermin.VremePocetka + "- " + stariTermin.VremeKraja, ListaIdPacijenata, stariTermin.Lekar.IdLekara, true, "");
            ObavestenjaMenadzer.obavestenja.Add(o);
            ObavestenjaServis.sacuvajIzmene();
        }

        public static void OtkaziTermin(Termin termin)
        {
            for (int i = 0; i < termini.Count; i++)
            {
                if (termin.IdTermin == termini[i].IdTermin)
                {
                    foreach (Sala s in SaleMenadzer.sale)
                    {
                        if (s.Id == termin.Prostorija.Id)
                        {
                            s.zauzetiTermini.Remove(SaleServis.NadjiZauzece(s.Id, termin.IdTermin, termin.Datum, termin.VremePocetka, termin.VremeKraja));
                            SaleServis.sacuvajIzmjene();
                        }
                    }
                    AnketaServis.ObrisiAnketu(termin.IdTermin);
                    termini.RemoveAt(i);
                }
            }
            PrikaziTermin.Termini.Remove(termin);
        }


        public static void OtkaziTerminSekretar(Termin termin)
        {
            int id = termin.Prostorija.Id;
            for (int i = 0; i < termini.Count; i++)
            {
                if (termin.IdTermin == termini[i].IdTermin)
                {
                    // brisanje termina iz zauzetih termina u sali
                    foreach (Sala s in SaleMenadzer.sale)
                    {
                        if (s.Id == termin.Prostorija.Id)
                        {
                            s.zauzetiTermini.Remove(SaleServis.NadjiZauzece(s.Id, termin.IdTermin, termin.Datum, termin.VremePocetka, termin.VremeKraja));
                            SaleServis.sacuvajIzmjene();
                        }
                    }
                    termini.RemoveAt(i);
                }
            }

            PrikaziTerminSekretar.TerminiSekretar.Remove(termin);

            // brisanje otkazanih termina iz zauzeca sala unutar drugih termina
            foreach (Termin t in TerminMenadzer.termini)
            {
                if (t.Prostorija.Id == id)
                {
                    t.Prostorija = SaleServis.NadjiSaluPoId(id);
                }       

            }
            TerminMenadzer.sacuvajIzmene();

            // notifikacija
            int idObavestenja = ObavestenjaServis.GenerisanjeIdObavestenja();
            List<int> ListaIdPacijenata = new List<int>();
            ListaIdPacijenata.Add(termin.Pacijent.IdPacijenta);
            Obavestenja o = new Obavestenja(idObavestenja, termin.Datum, termin.tipTermina.ToString(), "Otkazan termin" + ", " + termin.VremePocetka + "- " + termin.VremeKraja, ListaIdPacijenata, termin.Lekar.IdLekara, true, "");
            ObavestenjaMenadzer.obavestenja.Add(o);
            ObavestenjaServis.sacuvajIzmene();
        }

        public static void OtkaziTerminLekar(Termin termin)
        {
            //termini.Remove(termin);
            for (int i = 0; i < termini.Count; i++)
            {
                if (termin.IdTermin == termini[i].IdTermin)
                {

                    foreach (Sala s in SaleMenadzer.sale)
                    {
                        if (s.Id == termin.Prostorija.Id)
                        {
                            s.zauzetiTermini.Remove(SaleServis.NadjiZauzece(s.Id, termin.IdTermin, termin.Datum, termin.VremePocetka, termin.VremeKraja));
                            SaleServis.sacuvajIzmjene();
                        }
                    }

                    termini.RemoveAt(i);
                }
            }
            PrikazTerminaLekar.Termini.Remove(termin);
        }

        public static List<Termin> NadjiSveTermine()
        {
            if (File.ReadAllText("termini.xml").Trim().Equals(""))
            {
                return termini;
            }
            else
            {
                FileStream fileStream = File.OpenRead("termini.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Termin>));
                termini = (List<Termin>)serializer.Deserialize(fileStream);
                fileStream.Close();
                return termini;
            }
        }

        public static Termin NadjiTerminPoId(int idTermin)
        {
            foreach (Termin t in termini)
            {
                if (t.IdTermin == idTermin)
                {
                    return t;
                }
            }
            return null;
        }


        public static void sacuvajIzmene()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Termin>));
            TextWriter fileStream = new StreamWriter("termini.xml");
            serializer.Serialize(fileStream, termini);
            fileStream.Close();
        }

        public static Boolean SlobodanTermin(String datum, String VremePocetka, String VremeKraja, Sala sala)
        {
            // foreach (Termin t in TerminMenadzer.NadjiSveTermine())
            //{
            // postoji zakazan termin u tom opsegu
            // if (/*t.Datum.Equals(datum) &&*/ t.Prostorija.Id == sala.Id /*&& Int32.Parse(VremePocetka) >= Int32.Parse(t.VremePocetka) && Int32.Parse(VremeKraja) <= Int32.Parse(t.VremeKraja)*/)
            /* {
                 return false;
             } */
            //}
            return true;
        }

        // Sanja
        public static List<Termin> PronadjiTerminPoIdPacijenta(int idPacijenta)
        {
            List<Termin> terminiPacijenta = new List<Termin>();
            foreach (Termin termin in termini)
            {
                if (termin.Pacijent.IdPacijenta == idPacijenta)
                {
                    terminiPacijenta.Add(termin);
                }
            }
            return terminiPacijenta;
        }

        public static List<Termin> PronadjiSveTerminePacijentaZaSelektovaniDatum(int idPacijent, string selektovaniDatum)
        {
            List<Termin> terminiZaSelektovaniDatum = new List<Termin>();
            List<Termin> terminiPacijenta = PronadjiTerminPoIdPacijenta(idPacijent);
            foreach (Termin termin in terminiPacijenta)
            {
                if (termin.Datum.Equals(selektovaniDatum))
                {
                    terminiZaSelektovaniDatum.Add(termin);
                }
            }
            return terminiZaSelektovaniDatum;
        }
        public static ObservableCollection<Termin> DodajTerminePacijenta(int idPacijent)
        {
            ObservableCollection<Termin> Termini = new ObservableCollection<Termin>();
            foreach (Termin t in TerminMenadzer.termini)
            {
                if (t.Pacijent.IdPacijenta == idPacijent)
                {
                    Termini.Add(t);
                }
            }
            return Termini;
        }

        public static List<Termin> termini = new List<Termin>();
    }
}