using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    public class AnketaServis : IAnketeServis
    {
        #region Ankete menadzer
        public static int oznakaAnketeZaKliniku = AnketaMenadzer.oznakaAnketeZaKliniku;
        public static int minBrojTerminaZaAnketuKlinika = 3;

        public void sacuvajIzmene()
        {
            AnketaMenadzer.sacuvajIzmene();
        }

        public List<Anketa> Ankete()
        {
            return AnketaMenadzer.ankete;
        }

        public List<Anketa> NadjiSveAnkete()
        {
            return AnketaMenadzer.NadjiSveAnkete();
        }

        public Anketa NadjiAnketuPoId(int IdAnkete)
        {
            return AnketaMenadzer.NadjiAnketuPoId(IdAnkete);
        }

        public void ObrisiAnketu(int idTerminaZaBrisanje)
        {
            AnketaMenadzer.ObrisiAnketu(idTerminaZaBrisanje);
        }
      
        public List<Anketa> SveAnketePacijenta(int idPacijent)
        {
            return AnketaMenadzer.SveAnketePacijenta(idPacijent);
        }
        #endregion

        #region ankete u PrikaziAnkete
        public ObservableCollection<Anketa> PrikaziSveAnketeZaProsleTermine(ObservableCollection<Anketa> AnketePacijenta, int idPacijent)
        {
            foreach (Anketa anketa in AnketaMenadzer.SveAnketePacijenta(idPacijent))  
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
                AnketeZaLekaraServis anketeZaLekaraServis = new AnketeZaLekaraServis();
                AnketeZaKlinikuServis anketeZaKlinikuServis = new AnketeZaKlinikuServis();

                anketeZaLekaraServis.PrikaziAnketuZaLekara(anketa, termin.IdTermin, AnketePacijenta);
                anketeZaKlinikuServis.PrikaziAnketuZaKliniku(AnketePacijenta, termin.Pacijent.IdPacijenta);
            }
        }
        #endregion
    }
}
