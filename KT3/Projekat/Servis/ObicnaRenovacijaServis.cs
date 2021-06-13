using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projekat.Model;

namespace Projekat.Servis
{
    public class ObicnaRenovacijaServis : IRenovacijaServis
    {
        public void Renoviraj(Renovacija renovacija)
        {
            SaleServis.zauzmiSalu(renovacija.Zauzece, renovacija.IzabranaSala);
            SaleServis.sacuvajIzmjene();
        }
    }
}
