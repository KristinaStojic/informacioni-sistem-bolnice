using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class Premjestaj
    {
        public Sala izSale { get; set; }
        public Sala uSalu { get; set; }
        public Oprema oprema{ get; set; }
        public int kolicina { get; set; }
        public int id { get; set; }
        public DateTime datumIVrijeme { get; set; }
        public bool salji { get; set; }

        public Premjestaj(Sala izSale, Sala uSalu, Oprema oprema, int kolicina, DateTime datumIVrijeme)
        {
            this.id = PremjestajMenadzer.GenerisanjeIdPremjestaja();
            this.izSale = izSale;
            this.uSalu = uSalu;
            this.oprema = oprema;
            this.kolicina = kolicina;
            this.datumIVrijeme = datumIVrijeme;
        }
        public Premjestaj() { }
    }
    

}

