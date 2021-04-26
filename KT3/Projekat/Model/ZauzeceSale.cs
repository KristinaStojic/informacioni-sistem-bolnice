using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class ZauzeceSale
    {
        public string pocetakTermina { get; set; }
        public string krajTermina { get; set; }
        public string datumPocetka { get; set; }
        public string datumKraja { get; set; }
        public int idTermina { get; set; }
       // public int idSale { get; set; }
        public ZauzeceSale(string pocetakTermina, string krajTermina, string datumPocetka, string datumKraja)
        {
            this.pocetakTermina = pocetakTermina;
            this.krajTermina = krajTermina;
            this.datumPocetka = datumPocetka;
            this.datumKraja = datumKraja;
        }

        public ZauzeceSale(string pocetakTermina, string krajTermina, string datumPocetka, string datumKraja, int id)
        {
            this.pocetakTermina = pocetakTermina;
            this.krajTermina = krajTermina;
            this.datumPocetka = datumPocetka;
            this.datumKraja = datumKraja;
            this.idTermina = id;
        }
        public ZauzeceSale(string pocetakTermina, string krajTermina, string datumPocetka, int id)
        {
            this.pocetakTermina = pocetakTermina;
            this.krajTermina = krajTermina;
            this.datumPocetka = datumPocetka;
            this.datumKraja = datumPocetka;
            this.idTermina = id;
        }

        public ZauzeceSale() { }
        
    }
}
