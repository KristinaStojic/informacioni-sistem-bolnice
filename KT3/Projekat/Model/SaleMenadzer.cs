/***********************************************************************
 * Module:  SaleMenadzer.cs
 * Author:  pc
 * Purpose: Definition of the Class SaleMenadzer
 ***********************************************************************/

using Projekat.Model;

namespace Model
{
    public class SaleMenadzer : Menadzer<Sala>
    {
        private SaleMenadzer() { }

        private static SaleMenadzer instanca = null;
        public static SaleMenadzer Instanca
        {
            get
            {
                if (instanca == null)
                {
                    instanca = new SaleMenadzer();
                }
                return instanca;
            }
        }
        public override void Izmjeni(Sala izSale, Sala uSalu)
        {
            foreach (Sala sala in lista)
            {
                if (sala.Id == izSale.Id)
                {
                    sala.brojSale = uSalu.brojSale;
                    sala.Namjena = uSalu.Namjena;
                    sala.TipSale = uSalu.TipSale;
                }
            }
            sacuvajIzmjene("sale.xml");
        }

    }
}