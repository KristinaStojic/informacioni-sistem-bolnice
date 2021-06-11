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
        ObavestenjaMenadzer menadzer = new ObavestenjaMenadzer();
        public void sacuvajIzmene()
        {
            //menadzer.SacuvajIzmene();
        }

        public List<Obavestenja> NadjiSvaObavestenja()
        {
            return menadzer.NadjiSve("../Projekat.Model.Obavestenja.json");
        }

        public void ObrisiObavestenje(Obavestenja obavestenje)
        {
            menadzer.Obrisi(obavestenje, "../Projekat.Model.Obavestenja.json");
        }

        public int GenerisanjeIdObavestenja()
        {
            bool pomocna = false;
            int id = 1;

            for (id = 1; id <= NadjiSvaObavestenja().Count; id++)
            {
                foreach (Obavestenja obavestenje in NadjiSvaObavestenja())
                {
                    if (obavestenje.IdObavestenja == id)
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

        #endregion

        #region Obavestenja Sekretar
        public void DodajObavestenjeSekretar(Obavestenja novoObavestenje)
        {
            menadzer.Dodaj(novoObavestenje, "../Projekat.Model.Obavestenja.json");
        }

        public void IzmeniObavestenjeSekretar(Obavestenja obavestenje, Obavestenja novoObavestenje)
        {
            menadzer.Izmeni(obavestenje, novoObavestenje, "../Projekat.Model.Obavestenja.json");
        }

        public string OdrediOznakuObavestenja(string namena)
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

        public List<int> DodajSelektovanePacijente(string oznaka, ListView listaPacijenata)
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

        public string PopuniNamenuObavestenja(Obavestenja selektovanoObavestenje) 
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

        public int OdrediIndeksIzabranogObavestenja(Obavestenja selektovanoObavestenje)
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
        public List<Obavestenja> PronadjiObavestenjaPoIdPacijenta(int idPacijent)
        {
            List<Obavestenja> retObavestenja = new List<Obavestenja>();
            foreach (Obavestenja obavestenje in NadjiSvaObavestenja())
            {
                foreach (int idPacijenta in obavestenje.ListaIdPacijenata)
                {
                    if (idPacijenta == idPacijent)
                    {
                        retObavestenja.Add(obavestenje);
                    }
                }
            }
            return retObavestenja;
        }

        public List<Obavestenja> PronadjiSvaObavestenja()
        {
            List<Obavestenja> obavestenja = menadzer.NadjiSve("../Projekat.Model.Obavestenja.json");
            return obavestenja;
        }

        public void DodajPodsetnikePacijenta(ObservableCollection<Obavestenja> obavestenjaPodsetnici, int idPacijent)
        {
            foreach (Obavestenja obavestenje in PronadjiObavestenjaPoIdPacijenta(idPacijent))
            {
                if (obavestenje.TipObavestenja.Equals("Podsetnik"))
                {
                    obavestenjaPodsetnici.Add(obavestenje);
                }
            }
        }

        public void ObrisiObavestenjePacijent(Obavestenja selektovanoObavestenje)
        {
            foreach (Obavestenja o in NadjiSvaObavestenja())
            {
                if (o.IdObavestenja == selektovanoObavestenje.IdObavestenja)
                {
                    NadjiSvaObavestenja().Remove(o);
                    return;
                }
            }
        }

        public ObservableCollection<Obavestenja> DodajObavestenja(int idPacijent)
        {
            ObservableCollection<Obavestenja> ObavestenjaPacijent = new ObservableCollection<Obavestenja>();
            foreach (Obavestenja obavestenje in NadjiSvaObavestenja())
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

        private void DodajStaraObavestenjaZaTerapijePodsetnike(Obavestenja obavestenje, ObservableCollection<Obavestenja> ObavestenjaPacijent)
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
        public void ProveriSvaObavestenja(int idPacijent, ObservableCollection<Obavestenja> ObavestenjaPacijent)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                foreach (Obavestenja obavestenje in this.PronadjiObavestenjaPoIdPacijenta(idPacijent))
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

        private string FormatirajDatum(DateTime datum)
        {
            return datum.ToString("MM/dd/yyyy HH:mm");
        }

        private void DodajNovoObavestenje(int idPacijent, ObservableCollection<Obavestenja> ObavestenjaPacijent, DateTime datumObavestenja)
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

        public void ObrisiSelektovanoObavestenje(Obavestenja obavestenje, ObservableCollection<Obavestenja> ObavestenjaPacijent, MenuItem jezik)
        {
            //if (obavestenje != null && obavestenje.TipObavestenja.Equals("Terapija"))
            if(obavestenje != null)
            {
                this.ObrisiObavestenjePacijent(obavestenje);
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

        private Obavestenja PronadjiSledeceObavestenje(string datum, int idPacijent, ObservableCollection<Obavestenja> ObavestenjaPacijent)
        {
            foreach (Obavestenja o in this.NadjiSvaObavestenja())
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
        private bool ProveriObjavljenaObavestenja(Obavestenja obavestenje, ObservableCollection<Obavestenja> ObavestenjaPacijent)
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