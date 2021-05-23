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
        #region Obavestenja menadzer
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
#endregion

        #region Obavestenja Sekretar
        public static void DodajObavestenjeSekretar(Obavestenja novoObavestenje)
        {
            ObavestenjaMenadzer.DodajObavestenje(novoObavestenje);
        }

        public static void IzmeniObavestenjeSekretar(Obavestenja obavestenje, Obavestenja novoObavestenje)
        {
            ObavestenjaMenadzer.IzmeniObavestenje(obavestenje, novoObavestenje);
        }
        #endregion

        #region Obavestenja Pacijent
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

        public static void ObrisiObavestenjePacijent(Obavestenja selektovanoObavestenje)
        {
            ObavestenjaMenadzer.ObrisiObavestenjePacijent(selektovanoObavestenje);
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
        #region Obavestenja nit
        public static void ProveriSvaObavestenja(int idPacijent, ObservableCollection<Obavestenja> ObavestenjaPacijent)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                // TODO: CLEAN CODE i ispraviti
                foreach (Obavestenja obavestenje in ObavestenjaServis.PronadjiObavestenjaPoIdPacijenta(idPacijent))
                {
                    DateTime datumObavestenja = DateTime.Parse(obavestenje.Datum);
                    string trenutnoVreme = DateTime.Now.ToString("MM/dd/yyyy HH:mm"); // HH:mm
                    string vremeZaTerapiju = datumObavestenja.ToString("MM/dd/yyyy HH:mm");
                    if (vremeZaTerapiju.Equals(trenutnoVreme))
                    {
                        bool postojeNovaObavestenja = ProveriObjavljenaObavestenja(obavestenje, ObavestenjaPacijent);
                        if (!postojeNovaObavestenja)
                        {
                            Obavestenja novoObavestenje = PronadjiSledeceObavestenje(datumObavestenja.ToString("MM/dd/yyyy HH:mm"), idPacijent, ObavestenjaPacijent);
                            if (novoObavestenje == null)
                            {
                                return;
                            }
                            ObavestenjaPacijent.Add(novoObavestenje);
                            string sadrzajObavestenja = novoObavestenje.SadrzajObavestenja;
                            //return true;
                        }
                    }
                }
                //return false;
           });
        }

        public static void ObrisiSelektovanoObavestenje(Obavestenja obavestenje, ObservableCollection<Obavestenja> ObavestenjaPacijent)
        {
            if (obavestenje != null && obavestenje.TipObavestenja.Equals("Terapija"))
            {
                ObavestenjaServis.ObrisiObavestenjePacijent(obavestenje);
                ObavestenjaPacijent.Remove(obavestenje);
            }
        }

        // obavestenja servis
        private static Obavestenja PronadjiSledeceObavestenje(string datum, int idPacijent, ObservableCollection<Obavestenja> ObavestenjaPacijent)
        {
            foreach (Obavestenja o in ObavestenjaServis.NadjiSvaObavestenja())
            {
                if (o.ListaIdPacijenata.Contains(idPacijent))
                {
                    if (o.Datum.Equals(datum) && !ObavestenjaPacijent.Any(x => x.IdObavestenja == o.IdObavestenja))
                    {
                        return o;
                    }
                }
            }
            return null;
        }
        // obavestenja servis
        private static bool ProveriObjavljenaObavestenja(Obavestenja obavestenje, ObservableCollection<Obavestenja> ObavestenjaPacijent)
        {
            foreach (Obavestenja o in ObavestenjaPacijent)
            {
                if (o.IdObavestenja == obavestenje.IdObavestenja)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion


        #endregion
    }
}