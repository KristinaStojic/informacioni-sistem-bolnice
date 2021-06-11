using LiveCharts;
using Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Projekat.Model
{
    class ZdravstveniKartonMenadzer
    {
        public static List<ZdravstveniKarton> kartoni = new List<ZdravstveniKarton>();
        public static List<LekarskiRecept> recepti = new List<LekarskiRecept>();
        PacijentiServis servis = new PacijentiServis();
        PacijentiMenadzer menadzer = new PacijentiMenadzer();

        public static List<ZdravstveniKarton> nadjiKartone()
        {
            return kartoni;
        }

        public int GenerisanjeIdRecepta(int idPac)
        {
            bool pomocna = false;
            int id = 1;
            foreach(Pacijent pac in servis.pacijenti())
            {
                if(pac.IdPacijenta == idPac)
                {
                    for (id = 1; id <= pac.Karton.LekarskiRecepti.Count; id++)
                    {
                        foreach (LekarskiRecept p in pac.Karton.LekarskiRecepti)
                        {
                            if (p.IdRecepta.Equals(id))
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
                }
            }
           
            return id;
        
        }
        
        public int GenerisanjeIdAnamneze(int idPac)
        {
            bool pomocna = false;
            int id = 1;
            foreach(Pacijent pac in servis.pacijenti())
            {
                if(pac.IdPacijenta == idPac)
                {
                    for (id = 1; id <= pac.Karton.Anamneze.Count; id++)
                    {
                        foreach (Anamneza p in pac.Karton.Anamneze)
                        {
                            if (p.IdAnamneze.Equals(id))
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
                }
            }
            

            return id;
        }
        
        public int GenerisanjeIdAlergena(int idPac)
        {
            bool pomocna = false;
            int id = 1;
            foreach(Pacijent pac in servis.pacijenti())
            {
                if(pac.IdPacijenta == idPac)
                {
                    for (id = 1; id <= pac.Karton.Alergeni.Count; id++)
                    {
                        foreach (Alergeni p in pac.Karton.Alergeni)
                        {
                            if (p.IdAlergena.Equals(id))
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
                }
            }
            

            return id;
        }

        public void DodajRecept(LekarskiRecept recept)
        {
            List<Pacijent> lista = servis.pacijenti();
            foreach (Pacijent pacijent in lista)
            {
                if(pacijent.IdPacijenta == recept.idPacijenta)
                {                    
                    pacijent.Karton.LekarskiRecepti.Add(recept);
                    ZdravstveniKartonLekar.PrikazRecepata.Add(recept);
                }
            }
            menadzer.SacuvajIzmene("pacijenti.xml", lista);
        }

        public void DodajAnamnezu(Anamneza anamneza)  
        {
            List<Pacijent> lista = servis.pacijenti();
            foreach (Pacijent pacijent in lista)
            {
                if(pacijent.IdPacijenta == anamneza.IdPacijenta)
                {                    
                    pacijent.Karton.Anamneze.Add(anamneza);
                    ZdravstveniKartonLekar.TabelaAnamneza.Add(anamneza);
                    
                }
            }
            menadzer.SacuvajIzmene("pacijenti.xml", lista);
        }

        public void IzmeniAnamnezu(Anamneza stara, Anamneza nova)
        {
            List<Pacijent> lista = servis.pacijenti();
            foreach (Pacijent pacijent in lista)
            {
                if(pacijent.IdPacijenta == stara.IdPacijenta)
                {
                    foreach(Anamneza a in pacijent.Karton.Anamneze)
                    {
                        if(a.IdAnamneze == stara.IdAnamneze)
                        {
                            a.OpisBolesti = nova.OpisBolesti;
                            a.Terapija = nova.Terapija;
                            a.Datum = nova.Datum;
                        }
                    }
                }
            }

            int idx = ZdravstveniKartonLekar.TabelaAnamneza.IndexOf(stara);
            ZdravstveniKartonLekar.TabelaAnamneza.RemoveAt(idx);
            ZdravstveniKartonLekar.TabelaAnamneza.Insert(idx, nova);
            menadzer.SacuvajIzmene("pacijenti.xml", lista);
        }

        public void IzmeniAnamnezuPacijent(Anamneza stara, Anamneza nova)
        {
            List<Pacijent> lista = servis.pacijenti();
            foreach (Pacijent pacijent in lista)
            {
                if (pacijent.IdPacijenta == stara.IdPacijenta)
                {
                    foreach (Anamneza a in pacijent.Karton.Anamneze)
                    {
                        if (a.IdAnamneze == stara.IdAnamneze)
                        {
                            a.OpisBolesti = nova.OpisBolesti;
                            a.Terapija = nova.Terapija;
                            a.Datum = nova.Datum;
                            a.Beleska = nova.Beleska;
                        }
                    }
                }
            }
            menadzer.SacuvajIzmene("pacijenti.xml", lista);
        }


        public void IzmeniUput(Uput stara, Uput nova)
        {
            List<Pacijent> lista = servis.pacijenti();
            foreach (Pacijent pacijent in lista)
            {
                if(pacijent.IdPacijenta == stara.idPacijenta)
                {
                    foreach(Uput a in pacijent.Karton.Uputi)
                    {
                        if(a.IdUputa == stara.IdUputa)
                        {
                            a.datumKrajaLecenja = nova.datumKrajaLecenja;
                        }
                    }
                }
            }

            int idx = ZdravstveniKartonLekar.TabelaUputa.IndexOf(stara);
            ZdravstveniKartonLekar.TabelaUputa.RemoveAt(idx);
            ZdravstveniKartonLekar.TabelaUputa.Insert(idx, nova);
            menadzer.SacuvajIzmene("pacijenti.xml", lista);
        }

        public void DodajAlergen(Alergeni alergen)  
        {
            List<Pacijent> lista = servis.pacijenti();
            foreach (Pacijent pacijent in lista)
            {
                if (pacijent.IdPacijenta == alergen.IdPacijenta)
                {
                    pacijent.Karton.Alergeni.Add(alergen);
                    ZdravstveniKartonLekar.TabelaAlergena.Add(alergen);
                }
            }
            menadzer.SacuvajIzmene("pacijenti.xml", lista);
        }

        public void IzmeniAlergen(Alergeni stariAlergen, Alergeni noviAlergen)
        {
            List<Pacijent> lista = servis.pacijenti();
            foreach (Pacijent pacijent in lista)
            {
                if (pacijent.IdPacijenta == stariAlergen.IdPacijenta)
                {
                    foreach (Alergeni a in pacijent.Karton.Alergeni)
                    {
                        if (a.IdAlergena == stariAlergen.IdAlergena)
                        {
                            a.NuspojavaNaNastojak = noviAlergen.NuspojavaNaNastojak;
                            a.VremeReakcije = noviAlergen.VremeReakcije;
                        }
                    }
                }
            }

            int idx = ZdravstveniKartonLekar.TabelaAlergena.IndexOf(stariAlergen);
            ZdravstveniKartonLekar.TabelaAlergena.RemoveAt(idx);
            ZdravstveniKartonLekar.TabelaAlergena.Insert(idx, noviAlergen);
            menadzer.SacuvajIzmene("pacijenti.xml", lista);
        }

        public int GenerisanjeIdUputa(int idPacijenta)
        {
            int idUputa = 1;

            List<Pacijent> lista = servis.pacijenti();
            foreach (Pacijent pacijent in lista)
            {
                if (pacijent.IdPacijenta == idPacijenta)
                {
                    for (idUputa = 1; idUputa <= pacijent.Karton.Uputi.Count; idUputa++)
                    {  
                        if (!PostojiIdUputa(pacijent, idUputa))
                        {
                            return idUputa;
                        }       
                    }
                }
            }

            return idUputa;
        }

        private static bool PostojiIdUputa(Pacijent pacijent, int idUputa)
        {
            bool postojiUput = false;
            foreach (Uput uput in pacijent.Karton.Uputi)
            {
                if (uput.IdUputa.Equals(idUputa))
                {
                    postojiUput = true;
                    break;
                }
            }

            return postojiUput;
        }

        public void DodajUput(Uput uput)
        {
            List<Pacijent> lista = servis.pacijenti();
            foreach (Pacijent pacijent in lista)
            {
                if (pacijent.IdPacijenta == uput.idPacijenta)
                {
                    pacijent.Karton.Uputi.Add(uput);
                    if (uput.TipUputa.Equals(tipUputa.Laboratorija))
                    {
                        pacijent.Karton.brojLaboratorijskihUputa++;   
                        
                    } 
                    else if (uput.TipUputa.Equals(tipUputa.SpecijalistickiPregled))
                    {
                        pacijent.Karton.brojSpecijalistickihUputa++;
                    } 
                    else if (uput.TipUputa.Equals(tipUputa.StacionarnoLecenje))
                    {
                        pacijent.Karton.brojBolnickihUputa++;
                    }
                    ZdravstveniKartonLekar.TabelaUputa.Add(uput);
                    //ZdravstveniKartonLekar.PostaviDeloveGrafika(pacijent);
                    /*ZdravstveniKartonLekar.UkupnoLaboratorija = new ChartValues<int>() { pacijent.Karton.brojLaboratorijskihUputa };
                    ZdravstveniKartonLekar.UkupnoSpecijalisticki = new ChartValues<int>() { pacijent.Karton.brojSpecijalistickihUputa };
                    ZdravstveniKartonLekar.UkupnoStacionarno = new ChartValues<int>() { pacijent.Karton.brojBolnickihUputa };*/
                    

                }
            }
            menadzer.SacuvajIzmene("pacijenti.xml", lista);
        }

        public List<Uput> PronadjiSveSpecijalistickeUputePacijenta(int idPacijenta)
        {
            List<Uput> specijalistickiUputiPacijenta = new List<Uput>();
            Pacijent pacijent = servis.PronadjiPoId(idPacijenta);
            foreach(Uput uput in pacijent.Karton.Uputi)
            {
                // TODO: moze i za lab. upute i za stacionarno lecenje
                if (uput.TipUputa.Equals(tipUputa.SpecijalistickiPregled))
                {
                    specijalistickiUputiPacijenta.Add(uput);
                }
            }
            return specijalistickiUputiPacijenta;
        }




    }
}
