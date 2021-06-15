using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    class AnketeZaKlinikuServis 
    {
        private static int minBrojTerminaZaAnketuKlinika = 3;
       
        public void PrikaziAnketuZaKliniku(ObservableCollection<Anketa> AnketePacijenta, int idPacijent)
        {
            if (AnketePacijenta.Count() == minBrojTerminaZaAnketuKlinika)  /* posle 3 termina - anketa o radu klinike */
            {
                Anketa anketa = PronadjiAnketuZaKliniku(idPacijent);
                if (anketa == null) return;
                AnketePacijenta.Add(anketa);
            }
        }

        public static Anketa PronadjiAnketuZaKliniku(int idPacijent)
        {
            return AnketaMenadzer.PronadjiAnketuZaKliniku(idPacijent);
        }

        public void ProveriAnketuZaKliniku(int idPacijent)
        {
            int brojacTermina = 0;
            AnketaServis anketaServis = new AnketaServis();
            foreach (Termin termin in TerminServis.PronadjiTerminPoIdPacijenta(idPacijent))
            {
                brojacTermina++;
                if (brojacTermina == minBrojTerminaZaAnketuKlinika && !anketaServis.SveAnketePacijenta(idPacijent).Exists(x => x.IdTermina == AnketaMenadzer.oznakaAnketeZaKliniku))
                {
                    AnketaMenadzer.DodajAnketuZaKliniku(idPacijent);
                    return;
                }
            }
        }

    }
}
