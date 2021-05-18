using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    public class ObavestenjaServis
    {
        #region Obavestenja Pacijent
        public static void sacuvajIzmene()
        {
            ObavestenjaMenadzer.sacuvajIzmene();
        }
        public static List<Obavestenja> NadjiSvaObavestenja()
        {
            return ObavestenjaMenadzer.NadjiSvaObavestenja();
        }
        public static void ObrisiObavestenje(Obavestenja obavestenje)
        {
            ObavestenjaMenadzer.ObrisiObavestenje(obavestenje);
        }

        public static int GenerisanjeIdObavestenja()
        {
            return ObavestenjaMenadzer.GenerisanjeIdObavestenja();
        }
        public static void ObrisiObavestenjePacijent(Obavestenja selektovanoObavestenje)
        {
            ObavestenjaMenadzer.ObrisiObavestenjePacijent(selektovanoObavestenje);
        }

        public static List<Obavestenja> PronadjiObavestenjaPoIdPacijenta(int idPacijent)
        {
            return ObavestenjaMenadzer.PronadjiObavestenjaPoIdPacijenta(idPacijent);
        }

        public static void DodajPodsetnikePacijenta(ObservableCollection<Obavestenja> obavestenjaPodsetnici, int idPacijent)
        {
            foreach (Obavestenja obavestenje in PronadjiObavestenjaPoIdPacijenta(idPacijent))
            {
                if (obavestenje.TipObavestenja.Equals("Podsetnik"))
                {
                    obavestenjaPodsetnici.Add(obavestenje);
                }
            }
        }

        public static ObservableCollection<Obavestenja> DodajObavestenja(int idPacijent)
        {
            ObservableCollection<Obavestenja> ObavestenjaPacijent = new ObservableCollection<Obavestenja>();
            foreach (Obavestenja obavestenje in ObavestenjaMenadzer.obavestenja)
            {
                if (obavestenje.ListaIdPacijenata.Contains(idPacijent))
                {
                    if (obavestenje.TipObavestenja.Equals("Terapija") || obavestenje.TipObavestenja.Equals("Podsetnik"))
                    {
                        DodajStaraObavestenjaZaTerapijePodsetnike(obavestenje, ObavestenjaPacijent);
                    }
                }
                else // if (!o.TipObavestenja.Equals("Terapija") ||  !o.TipObavestenja.e)
                {
                    if (obavestenje.ListaIdPacijenata.Contains(idPacijent) || obavestenje.Oznaka.Equals("pacijenti") || obavestenje.Oznaka.Equals("svi"))
                    {
                        ObavestenjaPacijent.Add(obavestenje);
                    }
                }
            }
            return ObavestenjaPacijent;
        }

        private static void DodajStaraObavestenjaZaTerapijePodsetnike(Obavestenja obavestenje, ObservableCollection<Obavestenja> ObavestenjaPacijent)
        {
            DateTime dt = DateTime.Parse(obavestenje.Datum);
            if (dt.Date <= DateTime.Now.Date)
            {
                if (dt.TimeOfDay <= DateTime.Now.TimeOfDay)
                {
                    ObavestenjaPacijent.Add(obavestenje);
                }
            }
        }

        #endregion
    }

}
