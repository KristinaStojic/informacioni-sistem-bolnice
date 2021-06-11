using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Projekat.Servis
{
    public class ZdravstveniKartonServis
    {
        ZdravstveniKartonMenadzer menadzer = new ZdravstveniKartonMenadzer();
        PacijentiServis servis = new PacijentiServis();
        public int GenerisanjeIdRecepta(int IdPacijenta)
        {
            return menadzer.GenerisanjeIdRecepta(IdPacijenta);
        }
        
        public int GenerisanjeIdAnamneze(int IdPacijenta)
        {
            return menadzer.GenerisanjeIdAnamneze(IdPacijenta);
        }
        
        public int GenerisanjeIdAlergena(int IdPacijenta)
        {
            return menadzer.GenerisanjeIdAlergena(IdPacijenta);
        }

        public int GenerisanjeIdUputa(int idPacijenta)
        {
            return menadzer.GenerisanjeIdUputa(idPacijenta);
        }

        public void DodajRecept(LekarskiRecept recept)
        {
            menadzer.DodajRecept(recept);
        }

        public void DodajAnamnezu(Anamneza anamneza)
        {
            menadzer.DodajAnamnezu(anamneza);
        }

        public void IzmeniAnamnezu(Anamneza stara, Anamneza nova)
        {
            menadzer.IzmeniAnamnezu(stara, nova);
        }

        public void DodajAlergen(Alergeni alergen)
        {
            menadzer.DodajAlergen(alergen);
        }

        public void IzmeniAlergen(Alergeni stariAlergen, Alergeni noviAlergen)
        {
            menadzer.IzmeniAlergen(stariAlergen, noviAlergen);
        }

        public void DodajUput(Uput uput)
        {
            menadzer.DodajUput(uput);
        }

        public List<Lek> NadjiPacijentuDozvoljeneLekove(int idSelektovanogPacijenta)
        {
            List<Lek> dozvoljeniLekovi = new List<Lek>();


            dozvoljeniLekovi = nadjiDozvoljeneLekove();

            foreach (Pacijent pacijent in servis.pacijenti())
            {
                if (idSelektovanogPacijenta == pacijent.IdPacijenta)
                {                 
                    izbaciZabranjeneLekove(dozvoljeniLekovi, pacijent);
                }
            }

            return dozvoljeniLekovi;

        }

        private List<Lek> nadjiDozvoljeneLekove()
        {
            List<Lek> dozvoljeniLekovi = new List<Lek>();

            foreach (Lek lek in LekoviServis.Lijekovi())
            {
               dozvoljeniLekovi.Add(lek);
            }

            return dozvoljeniLekovi;
        }

        private void izbaciZabranjeneLekove(List<Lek> dozvoljeniLekovi, Pacijent pacijent)
        {
            foreach (Lek lek in LekoviServis.Lijekovi().ToArray())
            {
                foreach (Alergeni alergen in pacijent.Karton.Alergeni.ToArray())
                {

                    foreach (Sastojak sastojak in lek.sastojci.ToArray())
                    {
                        if (sastojak.naziv.Equals(alergen.NazivSastojka))
                        {
                            dozvoljeniLekovi.Remove(lek);
                        }
                    }

                }


            }
        }

        public List<Uput> PronadjiSveSpecijalistickeUputePacijenta(int idPacijenta)
        {
            return menadzer.PronadjiSveSpecijalistickeUputePacijenta(idPacijenta);
        }

        public List<ZdravstveniKarton> kartoni()
        {
            return ZdravstveniKartonMenadzer.nadjiKartone();
        }


        public ObservableCollection<Uput> DodajUputePacijenta(Pacijent prijavljeniPacijent)
        {
            ObservableCollection<Uput> uputiPacijenta = new ObservableCollection<Uput>();
            if (prijavljeniPacijent.Karton.Uputi.Count != 0)
            {
                foreach (Uput uput in prijavljeniPacijent.Karton.Uputi)
                {
                    uputiPacijenta.Add(uput);

                }
            }
            return uputiPacijenta;
        }
    }
}
