using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projekat.Model;

namespace Projekat.Servis
{
    public class RenovacijaPodelaSaleServis : IRenovacijaServis
    {
        public void Renoviraj(Renovacija renovacija)
        {
            SaleServis.DodajSalu(renovacija.NovaSala);
            SaleServis.sacuvajIzmjene();
            SaleServis.prebaciOpremuIzStareSale(renovacija.IzabranaSala, renovacija.NovaSala.Oprema);
            SaleServis.zauzmiSalu(renovacija.Zauzece, renovacija.IzabranaSala);
            SaleServis.zauzmiSalu(renovacija.Zauzece, renovacija.NovaSala);
            SaleServis.sacuvajIzmjene();
        }
    }
}
