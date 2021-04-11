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
        public string datumTermina { get; set; }
        public int idTermina { get; set; }
        public int idSale { get; set; }
        public ZauzeceSale(string pocetakTermina, string krajTermina, string datumTermina)
        {
            this.pocetakTermina = pocetakTermina;
            this.krajTermina = krajTermina;
            this.datumTermina = datumTermina;
        }
        // Sanja
        public ZauzeceSale(string pocetakTermina, string krajTermina, string datumTermina, int idSale, int idTermina)
        {
            this.pocetakTermina = pocetakTermina;
            this.krajTermina = krajTermina;
            this.datumTermina = datumTermina;
            this.idSale = idSale;
            this.idTermina = idTermina;
        }
        // Sanja
        public ZauzeceSale(string pocetakTermina, string krajTermina, string datumTermina, int idSale)
        {
            this.pocetakTermina = pocetakTermina;
            this.krajTermina = krajTermina;
            this.datumTermina = datumTermina;
            this.idSale = idSale;
        }

        public ZauzeceSale() { }
        
    }
}
