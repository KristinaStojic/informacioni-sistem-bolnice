using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class Sastojak
    {
        public string naziv { get; set; }
        public double kolicina { get; set; }

        public Sastojak(string naziv, double kolicina) {
            this.naziv = naziv;
            this.kolicina = kolicina;
        }

        public Sastojak() { }

    }
}
