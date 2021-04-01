using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Projekat.Model
{
    class LekarskiRecept
    {

        public LekarskiRecept(Pacijent p)
        {
                 
            /*  this.ImePacijenta = p.ImePacijenta;
               this.PrezimePacijenta = p.PrezimePacijenta;
               this.Jmbg = p.Jmbg;
               //this.IzabraniLekar = p.IzabraniLekar;*/
        }
        public int IdRecepta { get; set; }
        public string ImePacijenta { get; set; }
        public string PrezimePacijenta { get; set; }
        public int Jmbg { get; set; }
        public int IdLekara { get; set; } //??????????
        public string NazivLeka { get; set; }
        public string DatumPropisivanjaLeka { get; set; }
        public int BrojDanaKoriscenja { get; set; }
        public int DnevnaKolicina { get; set; }
        public string Napomena { get; set; }
    }
}
