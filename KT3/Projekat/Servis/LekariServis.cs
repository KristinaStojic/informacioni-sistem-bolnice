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

        #region Lekari
        public static void DodajLekara(Lekar noviLekar)
        {
            LekariMenadzer.DodajLekara(noviLekar);
        }

        public static void IzmeniLekara(Lekar stariLekar, Lekar noviLekar)
        {
            LekariMenadzer.IzmeniLekara(stariLekar, noviLekar);
        }

        public void ObrisiLekara(Lekar lekar)
        {
            menadzer.ObrisiLekara(lekar);
        }

        public static List<Lekar> NadjiSveLekare()
        {
            return LekariMenadzer.NadjiSveLekare();
        }

        public static Lekar NadjiPoId(int id)
        {
            return LekariMenadzer.NadjiPoId(id);
        }

        public static int GenerisanjeIdLekara()
        {
            return LekariMenadzer.GenerisanjeIdLekara();
        }

        public static int GenerisanjeIdZahtevaZaOdmor(int idLekara)
        {
            return LekariMenadzer.GenerisanjeIdZahtevaZaOdmor(idLekara);
        }

        public static void SacuvajIzmeneLekara()
        {
            LekariMenadzer.SacuvajIzmeneLekara();
        }

        public static bool JedinstvenJmbg(long jmbg)
        {
            foreach (Lekar lekar in LekariMenadzer.lekari)
            {
                if (lekar.Jmbg == jmbg)
                {
                    return false;
                }
            }
            return true;
        }

        public static List<Lekar> PronadjiLekarePoSpecijalizaciji(Specijalizacija tipSpecijalizacije)
        {
            return LekariMenadzer.PronadjiLekarePoSpecijalizaciji(tipSpecijalizacije);
        }

        public static void DodajZahtev(ZahtevZaGodisnji zahtev)
        {
            LekariMenadzer.DodajZahtev(zahtev);
        }

        public static List<ZahtevZaGodisnji> NadjiSveZahteve()
        {
            return LekariMenadzer.NadjiSveZahteve();
        }

        public static ZahtevZaGodisnji NadjiZahtevPoId(int id)
        {
            return LekariMenadzer.NadjiZahtevPoId(id);
        }

        public static void sacuvajIzmjeneZahteva()
        {
            LekariMenadzer.sacuvajIzmjeneZahteva();
        }
        #endregion

        #region Radno vreme
        public static string KonvertujDatum(DateTime datum)
        {
            return datum.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static List<RadniDan> NapraviListuRadnogVremena(string pocetak, string kraj, Calendar kalendar, Lekar lekar)
        {
            List<RadniDan> radniDani = new List<RadniDan>();
            string vremePocetka = pocetak;
            string vremeKraja = kraj;

            for (int i = 0; i < BROJ_NEDELJA_ZA_TRI_MESECA; i++)
            {
                foreach (DateTime datum in kalendar.SelectedDates)
                {
                    DateTime noviDatum = datum.AddDays(7 * i);
                    RadniDan noviDan = new RadniDan(lekar.IdLekara, LekariServis.KonvertujDatum(noviDatum), vremePocetka, vremeKraja);
                    radniDani.Add(noviDan);
                }
            }

            return radniDani;
        }

        #endregion

        #region Godisnji odmor

        public static void OdobriZahtevZaGodisnji(ZahtevZaGodisnji izabraniZahtev, int indeks)
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

        private static void OdobravanjeZahteva(ZahtevZaGodisnji izabraniZahtev, ZahtevZaGodisnji zahtevIzListe, int indeks)
        {
            izabraniZahtev.odobren = StatusZahteva.ODOBREN;
            zahtevIzListe.odobren = StatusZahteva.ODOBREN;
            OdobravanjeGodisnjegOdmora.TabelaZahteva.RemoveAt(indeks);
            OdobravanjeGodisnjegOdmora.TabelaZahteva.Insert(indeks, izabraniZahtev);
        }

        public static void OduzmiSlobodneDaneLekaru(ZahtevZaGodisnji zahtev)
        {
            List<Lekar> lekari = NadjiSveLekare();
            foreach (Lekar lekar in lekari)
            {
                if (lekar.IdLekara == zahtev.lekar.IdLekara)
                {
                    OznaciLekarimaGodisnjiOdmor(lekar, zahtev);
                    lekar.SlobodniDaniGodisnjegOdmora -= zahtev.brojDanaOdmora;
                    SacuvajIzmeneLekara();
                }
            }
        }

        public static void OznaciLekarimaGodisnjiOdmor(Lekar lekar, ZahtevZaGodisnji zahtev)
        {
            foreach (RadniDan dan in lekar.RadniDani)
            {
                if ((DateTime.Parse(zahtev.pocetakOdmora) <= DateTime.Parse(dan.Datum)) &&
                     (DateTime.Parse(zahtev.krajOdmora) >= DateTime.Parse(dan.Datum)))
                {
                    dan.NaGodisnjemOdmoru = true;
                    SacuvajIzmeneLekara();
                }
            }
        }

        public static void OdbijZahtevZaGodisnji(ZahtevZaGodisnji izabraniZahtev, int indeks)
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

        public static void DodajZahteveUTabelu()
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
