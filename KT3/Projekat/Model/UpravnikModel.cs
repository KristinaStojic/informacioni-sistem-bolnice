using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class UpravnikModel
    {
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public UpravnikModel() { }
        public UpravnikModel(string KorisnickoIme, string Lozinka)
        {
            this.KorisnickoIme = KorisnickoIme;
            this.Lozinka = Lozinka;
        }
    }
}
