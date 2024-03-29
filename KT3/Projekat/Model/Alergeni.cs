﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class Alergeni
    {
        public int IdAlergena { get; set; }
        public int IdPacijenta { get; set; }
        public string NazivSastojka { get; set; }
        public string NuspojavaNaNastojak { get; set; }
        public string VremeReakcije { get; set; }

        public Alergeni() { }

        public Alergeni(int idAlergena, int idPacijenta, string nazivLeka, string nuspojava, string vremeReakcije)
        {
            this.IdAlergena = idAlergena;
            this.IdPacijenta = idPacijenta;
            this.NazivSastojka = nazivLeka;
            
            this.NuspojavaNaNastojak = nuspojava;
            this.VremeReakcije = vremeReakcije;
        }  
        
        public Alergeni(int idAlergena, string nazivLeka)
        {
            this.IdAlergena = idAlergena;  
            this.NazivSastojka = nazivLeka;
            
        }


    }
}
