using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Model;
using Projekat.Model;

namespace Projekat.Servis
{
    public class LekariServis
    {
        public const int BROJ_NEDELJA_ZA_TRI_MESECA = 12;
        LekariMenadzer menadzer = new LekariMenadzer();
        ZahteviZaGodisnjiMenadzer zahteviZaGodisnjiMenadzer = new ZahteviZaGodisnjiMenadzer();

        #region Lekari
        public void DodajLekara(Lekar noviLekar)
        {
            menadzer.Dodaj(noviLekar, "lekari.xml");
        }

        public void IzmeniLekara(Lekar stariLekar, Lekar noviLekar)
        {
            menadzer.Izmeni(stariLekar, noviLekar, "lekari.xml");
        }

        public void ObrisiLekara(Lekar lekar)
        {
            menadzer.Obrisi(lekar, "lekari.xml");
        }

        public List<Lekar> NadjiSveLekare()
        {
            return menadzer.NadjiSve("lekari.xml");
        }

        public Lekar NadjiPoId(int id)
        {
            foreach (Lekar lekar in NadjiSveLekare())
            {
                if (lekar.IdLekara == id)
                {
                    return lekar;
                }
            }
            return null;
        }

        public int GenerisanjeIdLekara()
        {
            bool pomocna = false;
            int id = 1;

            for (id = 1; id <= NadjiSveLekare().Count; id++)
            {
                foreach (Lekar lekar in NadjiSveLekare())
                {
                    if (lekar.IdLekara.Equals(id))
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

        public int GenerisanjeIdZahtevaZaOdmor(int idLekara)
        {
            bool pomocna = false;
            int id = 1;

            for (id = 1; id <= NadjiSveZahteve().Count; id++)
            {
                foreach (ZahtevZaGodisnji zahtev in NadjiSveZahteve())
                {
                    if (zahtev.idZahteva.Equals(id))
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

        public bool JedinstvenJmbg(long jmbg)
        {
            foreach (Lekar lekar in NadjiSveLekare())
            {
                if (lekar.Jmbg == jmbg)
                {
                    return false;
                }
            }
            return true;
        }

        public List<Lekar> PronadjiLekarePoSpecijalizaciji(Specijalizacija tipSpecijalizacije)
        {
            List<Lekar> specijalizovaniLekari = new List<Lekar>();
            foreach (Lekar lekar in NadjiSveLekare())
            {
                if (lekar.specijalizacija.Equals(tipSpecijalizacije))
                {
                    specijalizovaniLekari.Add(lekar);
                }
            }
            return specijalizovaniLekari;
        }

        public void DodajZahtev(ZahtevZaGodisnji zahtev)
        {
            zahteviZaGodisnjiMenadzer.DodajZahtev(zahtev);
        }

        public List<ZahtevZaGodisnji> NadjiSveZahteve()
        {
            return zahteviZaGodisnjiMenadzer.NadjiSveZahteve();
        }

        public ZahtevZaGodisnji NadjiZahtevPoId(int id)
        {
            foreach (ZahtevZaGodisnji zahtev in NadjiSveZahteve())
            {
                if (zahtev.idZahteva == id)
                {
                    return zahtev;
                }
            }
            return null;
        }

        public void sacuvajIzmjeneZahteva()
        {
            zahteviZaGodisnjiMenadzer.sacuvajIzmjeneZahteva();
        }
        #endregion

        #region Radno vreme
        public string KonvertujDatum(DateTime datum)
        {
            return datum.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }

        public List<RadniDan> NapraviListuRadnogVremena(string pocetak, string kraj, Calendar kalendar, Lekar lekar)
        {
            List<RadniDan> radniDani = new List<RadniDan>();
            string vremePocetka = pocetak;
            string vremeKraja = kraj;

            for (int i = 0; i < BROJ_NEDELJA_ZA_TRI_MESECA; i++)
            {
                foreach (DateTime datum in kalendar.SelectedDates)
                {
                    DateTime noviDatum = datum.AddDays(7 * i);
                    RadniDan noviDan = new RadniDan(lekar.IdLekara, this.KonvertujDatum(noviDatum), vremePocetka, vremeKraja);
                    radniDani.Add(noviDan);
                }
            }

            return radniDani;
        }

        #endregion

        #region Godisnji odmor
        public void OdobriZahtevZaGodisnji(ZahtevZaGodisnji izabraniZahtev, int indeks)
        {
            List<ZahtevZaGodisnji> zahtevi = NadjiSveZahteve();
            for (int i = 0; i < zahtevi.Count; i++)
            {
                if (zahtevi[i].idZahteva == izabraniZahtev.idZahteva && izabraniZahtev.odobren == StatusZahteva.NA_CEKANJU)
                {
                    Lekar lekar = NadjiPoId(izabraniZahtev.lekar.IdLekara);
                    if (zahtevi[i].brojDanaOdmora - lekar.ZahtevaniDaniGodisnjegOdmora < 0)
                    {
                        MessageBox.Show("Lekar nema vise slobodnih dana!");
                        return;
                    }
                    else
                    {
                        OdobravanjeZahteva(izabraniZahtev, zahtevi[i], indeks);
                        OduzmiSlobodneDaneLekaru(zahtevi[i]);
                        sacuvajIzmjeneZahteva();
                        return;
                    }
                }
                else if(izabraniZahtev.odobren == StatusZahteva.ODBIJEN || izabraniZahtev.odobren == StatusZahteva.ODOBREN)
                {
                    MessageBox.Show("Zahtev je vec obradjen!");
                    return;
                }
            }
        }

        private void OdobravanjeZahteva(ZahtevZaGodisnji izabraniZahtev, ZahtevZaGodisnji zahtevIzListe, int indeks)
        {
            izabraniZahtev.odobren = StatusZahteva.ODOBREN;
            zahtevIzListe.odobren = StatusZahteva.ODOBREN;
            OdobravanjeGodisnjegOdmora.TabelaZahteva.RemoveAt(indeks);
            OdobravanjeGodisnjegOdmora.TabelaZahteva.Insert(indeks, izabraniZahtev);
        }

        public void OduzmiSlobodneDaneLekaru(ZahtevZaGodisnji zahtev)
        {
            List<Lekar> lekari = NadjiSveLekare();
            foreach (Lekar lekar in lekari)
            {
                if (lekar.IdLekara == zahtev.lekar.IdLekara)
                {
                    OznaciLekarimaGodisnjiOdmor(lekar, zahtev);
                    lekar.SlobodniDaniGodisnjegOdmora -= zahtev.brojDanaOdmora;
                    //SacuvajIzmeneLekara();
                }
            }
        }

        public void OznaciLekarimaGodisnjiOdmor(Lekar lekar, ZahtevZaGodisnji zahtev)
        {
            foreach (RadniDan dan in lekar.RadniDani)
            {
                if ((DateTime.Parse(zahtev.pocetakOdmora) <= DateTime.Parse(dan.Datum)) &&
                     (DateTime.Parse(zahtev.krajOdmora) >= DateTime.Parse(dan.Datum)))
                {
                    dan.NaGodisnjemOdmoru = true;
                    //SacuvajIzmeneLekara();
                }
            }
        }

        public void OdbijZahtevZaGodisnji(ZahtevZaGodisnji izabraniZahtev, int indeks)
        {
            List<ZahtevZaGodisnji> zahtevi = NadjiSveZahteve();
            for (int i = 0; i < zahtevi.Count; i++)
            {
                if (zahtevi[i].idZahteva == izabraniZahtev.idZahteva && izabraniZahtev.odobren == StatusZahteva.NA_CEKANJU)
                {
                    izabraniZahtev.odobren = StatusZahteva.ODBIJEN;
                    zahtevi[i].odobren = StatusZahteva.ODBIJEN;
                    OdobravanjeGodisnjegOdmora.TabelaZahteva.RemoveAt(indeks);
                    OdobravanjeGodisnjegOdmora.TabelaZahteva.Insert(indeks, izabraniZahtev);

                    sacuvajIzmjeneZahteva();
                    return;
                }
                else if(izabraniZahtev.odobren == StatusZahteva.ODBIJEN || izabraniZahtev.odobren == StatusZahteva.ODOBREN)
                {
                    MessageBox.Show("Zahtev je vec obradjen!");
                    return;
                }
            }
        }

        public void DodajZahteveUTabelu()
        {
            OdobravanjeGodisnjegOdmora.TabelaZahteva = new ObservableCollection<ZahtevZaGodisnji>();

            List<ZahtevZaGodisnji> zahtevi = NadjiSveZahteve();
            foreach (ZahtevZaGodisnji zahtev in zahtevi)
            {    
                OdobravanjeGodisnjegOdmora.TabelaZahteva.Add(zahtev);   
            }
        }

        #endregion
    }
}
