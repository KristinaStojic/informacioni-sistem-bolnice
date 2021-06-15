using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    class AnketeZaLekaraServis 
    {
        public void PrikaziAnketuZaLekara(Anketa anketa, int IdTermina, ObservableCollection<Anketa> AnketePacijenta)
        {
            if (anketa.IdTermina == IdTermina)
            {
                AnketePacijenta.Add(anketa);
            }
        }

        public void DodajAnketuZaLekara(Termin termin, int idPacijent)
        {
            AnketaMenadzer.DodajAnketuZaLekara(termin, idPacijent);
        }

        public static Lekar pronadjiLekaraZaAnketu(int idAnkete)
        {
            AnketaServis anketaServis = new AnketaServis();
            Anketa anketa = anketaServis.NadjiAnketuPoId(idAnkete);
            Termin termin = TerminServis.NadjiTerminPoId(anketa.IdTermina);
            return termin.Lekar;
        }

        public string PrikaziNaslovAnkete(int idAnkete)
        {
            AnketaServis anketaServis = new AnketaServis();
            Anketa anketa = anketaServis.NadjiAnketuPoId(idAnkete);
            Termin terminAnkete = TerminServis.NadjiTerminPoId(anketa.IdTermina);
            Lekar lekar = terminAnkete.Lekar;
            return " (" + lekar.ImeLek + " " + lekar.PrezimeLek + ")";
        }

        public void IzmeniAnketuZaLekara(Termin stariTermin, Termin noviTermin)
        {
            AnketaMenadzer.IzmeniAnketuZaLekara(stariTermin, noviTermin);
        }
    }
}
