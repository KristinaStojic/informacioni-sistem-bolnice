using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace Projekat.Model
{
    public class Renovacija
    {
        public Sala IzabranaSala { get; set; }
        public Sala NovaSala { get; set; }
        public ZauzeceSale Zauzece { get; set; }

        public Renovacija(Sala izabranaSala, ZauzeceSale zauzece)
        {
            IzabranaSala = izabranaSala;
            Zauzece = zauzece;
        }

        public Renovacija(Sala izabranaSala, Sala novaSala, ZauzeceSale zauzece)
        {
            IzabranaSala = izabranaSala;
            NovaSala = novaSala;
            Zauzece = zauzece;
        }
    }
}
