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
        public static int GenerisanjeIdRecepta(int IdPacijenta)
        {
            return ZdravstveniKartonMenadzer.GenerisanjeIdRecepta(IdPacijenta);
        }
        
        public static int GenerisanjeIdAnamneze(int IdPacijenta)
        {
            return ZdravstveniKartonMenadzer.GenerisanjeIdAnamneze(IdPacijenta);
        }
        
        public static int GenerisanjeIdAlergena(int IdPacijenta)
        {
            return ZdravstveniKartonMenadzer.GenerisanjeIdAlergena(IdPacijenta);
        }

        public static int GenerisanjeIdUputa(int idPacijenta)
        {
            return ZdravstveniKartonMenadzer.GenerisanjeIdUputa(idPacijenta);
        }

        public static void DodajRecept(LekarskiRecept recept)
        {
            ZdravstveniKartonMenadzer.DodajRecept(recept);
        }

        public static void DodajAnamnezu(Anamneza anamneza)
        {
            ZdravstveniKartonMenadzer.DodajAnamnezu(anamneza);
        }

        public static void IzmeniAnamnezu(Anamneza stara, Anamneza nova)
        {
            ZdravstveniKartonMenadzer.IzmeniAnamnezu(stara, nova);
        }

        public static void DodajAlergen(Alergeni alergen)
        {
            ZdravstveniKartonMenadzer.DodajAlergen(alergen);
        }

        public static void IzmeniAlergen(Alergeni stariAlergen, Alergeni noviAlergen)
        {
            ZdravstveniKartonMenadzer.IzmeniAlergen(stariAlergen, noviAlergen);
        }

        public static void DodajUput(Uput uput)
        {
            ZdravstveniKartonMenadzer.DodajUput(uput);
        }

        public static List<Lek> NadjiPacijentuDozvoljeneLekove(int idSelektovanogPacijenta)
        {
            List<Lek> dozvoljeniLekovi = new List<Lek>();


            dozvoljeniLekovi = nadjiDozvoljeneLekove();

            foreach (Pacijent pacijent in PacijentiServis.pacijenti())
            {
                if (idSelektovanogPacijenta == pacijent.IdPacijenta)
                {                 
                    izbaciZabranjeneLekove(dozvoljeniLekovi, pacijent);
                }
            }

            return dozvoljeniLekovi;

        }

        private static List<Lek> nadjiDozvoljeneLekove()
        {
            List<Lek> dozvoljeniLekovi = new List<Lek>();

            foreach (Lek lek in LekoviServis.Lijekovi())
            {
               dozvoljeniLekovi.Add(lek);
            }

            return dozvoljeniLekovi;
        }

        private static void izbaciZabranjeneLekove(List<Lek> dozvoljeniLekovi, Pacijent pacijent)
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

        public static List<Uput> PronadjiSveSpecijalistickeUputePacijenta(int idPacijenta)
        {
            return ZdravstveniKartonMenadzer.PronadjiSveSpecijalistickeUputePacijenta(idPacijenta);
        }

        public static List<ZdravstveniKarton> kartoni()
        {
            return ZdravstveniKartonMenadzer.nadjiKartone();
        }


        public static ObservableCollection<Uput> DodajUputePacijenta(Pacijent prijavljeniPacijent)
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
