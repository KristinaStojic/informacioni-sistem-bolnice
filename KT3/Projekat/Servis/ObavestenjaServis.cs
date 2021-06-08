using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Projekat.Servis
{
    public class ObavestenjaServis
    {
        #region Obavestenja Menadzer
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

        public static string OdrediOznakuObavestenja(string namena)
        {
            string oznaka = "";
            if (namena.Equals("sve"))
            {
                oznaka = "svi";
            }
            else if (namena.Equals("sve lekare"))
            {
                oznaka = "lekari";
            }
            else if (namena.Equals("sve upravnike"))
            {
                oznaka = "upravnici";
            }
            else if (namena.Equals("sve pacijente"))
            {
                oznaka = "pacijenti";
            }
            else
            {
                oznaka = "specificni pacijenti";
            }

            return oznaka;
        }

        public static List<int> DodajSelektovanePacijente(string oznaka, ListView listaPacijenata)
        {
            List<int> selektovaniPacijentiId = new List<int>();
            if (oznaka.Equals("specificni pacijenti"))
            {
                foreach (Pacijent p in listaPacijenata.SelectedItems)
                {
                    selektovaniPacijentiId.Add(p.IdPacijenta);
                }
            }

            return selektovaniPacijentiId;
        }

        public static List<Obavestenja> SvaObavestenja()
        {
            return ObavestenjaMenadzer.SvaObavestenja();
        }

        public static string PopuniNamenuObavestenja(Obavestenja selektovanoObavestenje) 
        {
            string namena = "";
            if (selektovanoObavestenje.Oznaka.Equals("svi"))
            {
                namena = "sve";
            }
            else if (selektovanoObavestenje.Oznaka.Equals("lekari"))
            {
                namena = "lekare";
            }
            else if (selektovanoObavestenje.Oznaka.Equals("upravnici"))
            {
                namena = "upravnike";
            }
            else if (selektovanoObavestenje.Oznaka.Equals("pacijenti"))
            {
                namena = "sve pacijente";
            }   
            else if (selektovanoObavestenje.Oznaka.Equals("specificni pacijenti"))
            {
                namena = "";
                foreach (int id in selektovanoObavestenje.ListaIdPacijenata)
                {
                    Pacijent pacijent = PacijentiServis.PronadjiPoId(id);
                    namena += pacijent.ImePacijenta + " " + pacijent.PrezimePacijenta + " \n";
                }
            }

            return namena;
        }

        public static int OdrediIndeksIzabranogObavestenja(Obavestenja selektovanoObavestenje)
        {
            int namena = 0;
            if (selektovanoObavestenje.Oznaka.Equals("svi"))
            {
                namena = 0;
            }
            else if (selektovanoObavestenje.Oznaka.Equals("lekari"))
            {
                namena = 1;
            }
            else if (selektovanoObavestenje.Oznaka.Equals("upravnici"))
            {
                namena = 2;
            }
            else if (selektovanoObavestenje.Oznaka.Equals("pacijenti"))
            {
                namena = 3;
            }
            else if (selektovanoObavestenje.Oznaka.Equals("specificni pacijenti"))
            {
                namena = 4;
            }

            return namena;
        }

        #endregion

        #region Obavestenja Pacijent
        public static List<Obavestenja> PronadjiObavestenjaPoIdPacijenta(int idPacijent)
        {
            return ObavestenjaMenadzer.PronadjiObavestenjaPoIdPacijenta(idPacijent);
        }

        public static List<Obavestenja> PronadjiSvaObavestenja()
        {
            List<Obavestenja> obavestenja = ObavestenjaMenadzer.obavestenja;
            return obavestenja;
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
                
                if (obavestenje.ListaIdPacijenata.Contains(idPacijent) || obavestenje.Oznaka.Equals("pacijenti") || obavestenje.Oznaka.Equals("svi"))
                {
                    if (!(obavestenje.TipObavestenja.Equals("Terapija") || obavestenje.TipObavestenja.Equals("Podsetnik")))
                    {
                        ObavestenjaPacijent.Add(obavestenje);
                    }
                }
                if (obavestenje.ListaIdPacijenata.Contains(idPacijent))
                {
                    if (obavestenje.TipObavestenja.Equals("Terapija") || obavestenje.TipObavestenja.Equals("Podsetnik"))
                    {
                        DodajStaraObavestenjaZaTerapijePodsetnike(obavestenje, ObavestenjaPacijent);
                    }
                }
            }
            return ObavestenjaPacijent;
        }
        private static void DodajStaraObavestenjaZaTerapijePodsetnike(Obavestenja obavestenje, ObservableCollection<Obavestenja> ObavestenjaPacijent)
        {
            DateTime datumObavestenja = DateTime.Parse(obavestenje.Datum);
            if (datumObavestenja.Date > DateTime.Now.Date) return;
            if (datumObavestenja.Date == DateTime.Now.Date)
            {
                if (datumObavestenja.TimeOfDay <= DateTime.Now.TimeOfDay && !ObavestenjaPacijent.Contains(obavestenje))
                {
                    ObavestenjaPacijent.Add(obavestenje);
                }
            }
            if (datumObavestenja.Date < DateTime.Now.Date)
            {
                if (!ObavestenjaPacijent.Contains(obavestenje))
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
                foreach (Obavestenja obavestenje in ObavestenjaServis.PronadjiObavestenjaPoIdPacijenta(idPacijent))
                {
                    DateTime datumObavestenja = DateTime.Parse(obavestenje.Datum);
                    string trenutnoVreme = FormatirajDatum(DateTime.Now); 
                    string vremeZaTerapiju = FormatirajDatum(datumObavestenja);
                    if (vremeZaTerapiju.Equals(trenutnoVreme))
                    {
                        if (!ProveriObjavljenaObavestenja(obavestenje, ObavestenjaPacijent))
                        {
                            DodajNovoObavestenje(idPacijent, ObavestenjaPacijent, datumObavestenja);
                        }
                    }
                }
            });
        }

        private static string FormatirajDatum(DateTime datum)
        {
            return datum.ToString("MM/dd/yyyy HH:mm");
        }

        private static void DodajNovoObavestenje(int idPacijent, ObservableCollection<Obavestenja> ObavestenjaPacijent, DateTime datumObavestenja)
        {
            Obavestenja novoObavestenje = PronadjiSledeceObavestenje(datumObavestenja.ToString("MM/dd/yyyy HH:mm"), idPacijent, ObavestenjaPacijent);
            if (novoObavestenje == null)
            {
                return;
            }
            ObavestenjaPacijent.Add(novoObavestenje);
            string sadrzajObavestenja = novoObavestenje.SadrzajObavestenja;
            MessageBox.Show(novoObavestenje.TipObavestenja + ": " + sadrzajObavestenja, "Novo obaveštenje");
        }

        public static void ObrisiSelektovanoObavestenje(Obavestenja obavestenje, ObservableCollection<Obavestenja> ObavestenjaPacijent, MenuItem jezik)
        {
            //if (obavestenje != null && obavestenje.TipObavestenja.Equals("Terapija"))
            if(obavestenje != null)
            {
                ObavestenjaServis.ObrisiObavestenjePacijent(obavestenje);
                ObavestenjaPacijent.Remove(obavestenje);
                if(jezik.Header.Equals("_en-US"))
                {
                    MessageBox.Show("Selektovano obavestenje je obrisano.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                MessageBox.Show("The selected notification has been deleted.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

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