using Model;
using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Interfejsi
{
    public class Pregled : ITipTermina
    {
       
        public Sala pronadjiPrvuSlobodnuProstoriju(string selektovaniDatum, string selektovaniSlot)
        {
            int satiVremeSelektovanogSlota = TerminServis.ParsirajSateVremenskogSlota(selektovaniSlot);
            Sala prvaSlobodnaSala = null;
            foreach (Sala sala in PronadjiSaleZaPregled())
            {
                bool postojiZauzece = TerminServis.ProveriVremeZauzecaZaTermine(selektovaniDatum, selektovaniSlot, sala) || TerminServis.ProveriVremeSvihZauzecaZaRenoviranje(selektovaniDatum, satiVremeSelektovanogSlota, sala);
                if (!postojiZauzece)
                {
                    prvaSlobodnaSala = sala;
                    return prvaSlobodnaSala;
                }
            }


            return null;
        }

        private List<Sala> PronadjiSaleZaPregled()
        {
            List<Sala> slobodneSaleZaPregled = new List<Sala>();
            foreach (Sala sala in SaleMenadzer.lista)
            {
                if (sala.TipSale.Equals(tipSale.SalaZaPregled) && !sala.Namjena.Equals("Skladiste"))
                {
                    slobodneSaleZaPregled.Add(sala);
                }
            }
            return slobodneSaleZaPregled;
        }


        



    }
}
