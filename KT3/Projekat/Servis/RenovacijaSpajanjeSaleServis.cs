using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Projekat.Model;

namespace Projekat.Servis
{
    public class RenovacijaSpajanjeSaleServis : IRenovacijaServis
    {
        public void Renoviraj(Renovacija renovacija)
        {
            SaleServis.dodajOpremuIzSaleZaDodavanje(renovacija.IzabranaSala, renovacija.NovaSala);
            SaleServis.ObrisiSalu(renovacija.NovaSala);
            SaleServis.sacuvajIzmjene();
            SaleServis.zauzmiSalu(renovacija.Zauzece, renovacija.IzabranaSala);
            SaleServis.zauzmiSalu(renovacija.Zauzece, renovacija.NovaSala);
            SaleServis.sacuvajIzmjene();
        }
    }
}
