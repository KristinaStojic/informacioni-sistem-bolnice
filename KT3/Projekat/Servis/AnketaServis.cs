using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    public class AnketaServis
    {
        #region Ankete menadzer
        public static int oznakaAnketeZaKliniku = AnketaMenadzer.oznakaAnketeZaKliniku;
        public static int minBrojTerminaZaAnketuKlinika = 3;
        public static void sacuvajIzmene()
        {
            AnketaMenadzer.sacuvajIzmene();
        }

        public static List<Anketa> NadjiSveAnkete()
        {
            return AnketaMenadzer.NadjiSveAnkete();
        }

        public static List<Anketa> NadjiSveAnketePoId(int idPacijenta)
        {
            return AnketaMenadzer.NadjiSveAnketePoId(idPacijenta);
        }

        public static Anketa NadjiAnketuPoId(int IdAnkete)
        {
            return AnketaMenadzer.NadjiAnketuPoId(IdAnkete);
        }

        public static void ObrisiAnketu(int idTerminaZaBrisanje)
        {
            AnketaMenadzer.ObrisiAnketu(idTerminaZaBrisanje);
        }

        public static void DodajAnketuZaLekara(Termin termin, int idPacijent)
        {
            AnketaMenadzer.DodajAnketuZaLekara(termin, idPacijent);
        }

        public static void DodajAnketuZaKliniku(int idPacijent)
        {
            AnketaMenadzer.DodajAnketuZaKliniku(idPacijent);
        }
        public static void IzmeniAnketuZaLekara(Termin stariTermin, Termin noviTermin)
        {
            AnketaMenadzer.IzmeniAnketuZaLekara(stariTermin, noviTermin);
        }

        public static Anketa PronadjiAnketuZaKliniku(int idPacijent)
        {
            return AnketaMenadzer.PronadjiAnketuZaKliniku(idPacijent);
        }

        public static List<Anketa> SveAnketePacijenta(int idPacijent)
        {
            return AnketaMenadzer.SveAnketePacijenta(idPacijent);
        }

        #endregion

        #region ankete u PrikaziAnkete
        public static ObservableCollection<Anketa> PrikaziSveAnketeZaProsleTermine(ObservableCollection<Anketa> AnketePacijenta, int idPacijent)
        {
            foreach (Anketa anketa in AnketaServis.SveAnketePacijenta(idPacijent))
            {
                foreach (Termin termin in TerminServis.PronadjiTerminPoIdPacijenta(idPacijent))
                {
                    PrikaziAnketeZaProsleTermine(anketa, termin, AnketePacijenta);
                }
            }
            return AnketePacijenta;
        }
        private static void PrikaziAnketeZaProsleTermine(Anketa anketa, Termin termin, ObservableCollection<Anketa> AnketePacijenta)
        {
            DateTime datumTermina = DateTime.Parse(termin.Datum);
            TimeSpan vremeKrajaTermina = TimeSpan.Parse(termin.VremeKraja);
            if ((datumTermina == DateTime.Now.Date && vremeKrajaTermina <= DateTime.Now.TimeOfDay) || datumTermina < DateTime.Now.Date)
            {
                PrikaziAnketuZaLekara(anketa, termin.IdTermin, AnketePacijenta);
                PrikaziAnketuZaKliniku(AnketePacijenta, termin.Pacijent.IdPacijenta);
            }
        }

        private static void PrikaziAnketuZaLekara(Anketa anketa, int IdTermina, ObservableCollection<Anketa> AnketePacijenta)
        {
            if (anketa.IdTermina == IdTermina)
            {
                AnketePacijenta.Add(anketa);
            }
        }

        private static void PrikaziAnketuZaKliniku(ObservableCollection<Anketa> AnketePacijenta, int idPacijent)
        {
            if (AnketePacijenta.Count() == minBrojTerminaZaAnketuKlinika)  /* posle 3 termina - anketa o radu klinike */
            {
                Anketa anketa = AnketaServis.PronadjiAnketuZaKliniku(idPacijent);
                if (anketa == null) return;
                AnketePacijenta.Add(anketa);
            }
        }

        public static Lekar pronadjiLekaraZaAnketu(int idAnkete)
        {
            Anketa anketa = NadjiAnketuPoId(idAnkete);
            Termin termin = TerminServis.NadjiTerminPoId(anketa.IdTermina);
            return termin.Lekar;
        }
        public static string PrikaziNaslovAnkete(Lekar lekar)
        {
            return "(" + lekar.ImeLek + " " + lekar.PrezimeLek + ")";
        }

        #endregion

        #region ankete u ZakaziTermin
        public static void ProveriAnketuZaKliniku(int idPacijent)
        {
            int brojacTermina = 0;
            foreach (Termin termin in TerminServis.PronadjiTerminPoIdPacijenta(idPacijent))
            {
                brojacTermina++;
                if (brojacTermina == minBrojTerminaZaAnketuKlinika && !AnketaServis.SveAnketePacijenta(idPacijent).Exists(x => x.IdTermina == AnketaServis.oznakaAnketeZaKliniku))
                {
                    AnketaServis.DodajAnketuZaKliniku(idPacijent);
                    return;
                }
            }
        }

        #endregion



    }
}
