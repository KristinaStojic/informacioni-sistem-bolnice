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