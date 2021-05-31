namespace Projekat.Model
{
    public class ZauzeceSale
    {
        public string pocetakTermina { get; set; }
        public string krajTermina { get; set; }
        public string datumPocetkaTermina { get; set; }
        public string datumKrajaTermina { get; set; }
        public int idTermina { get; set; }
      
        public ZauzeceSale(string pocetakTermina, string krajTermina, string datumPocetka, string datumKraja)
        {
            this.pocetakTermina = pocetakTermina;
            this.krajTermina = krajTermina;
            this.datumPocetkaTermina = datumPocetka;
            this.datumKrajaTermina = datumKraja;
        }

        public ZauzeceSale(string pocetakTermina, string krajTermina, string datumPocetka, string datumKraja, int id)
        {
            this.pocetakTermina = pocetakTermina;
            this.krajTermina = krajTermina;
            this.datumPocetkaTermina = datumPocetka;
            this.datumKrajaTermina = datumKraja;
            this.idTermina = id;
        }
        public ZauzeceSale(string pocetakTermina, string krajTermina, string datumPocetka, int id)
        {
            this.pocetakTermina = pocetakTermina;
            this.krajTermina = krajTermina;
            this.datumPocetkaTermina = datumPocetka;
            this.datumKrajaTermina = datumPocetka;
            this.idTermina = id;
        }

        public ZauzeceSale() { }
        
    }
}
