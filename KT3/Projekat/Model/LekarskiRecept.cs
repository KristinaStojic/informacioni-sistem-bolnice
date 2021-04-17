using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Projekat.Model
{
    public class LekarskiRecept
    {

        public LekarskiRecept(Pacijent p, int id, string naziv, int brojkor, int kol, String pocetak, String datum)
        {     
            this.idPacijenta = p.IdPacijenta;
            //this.IzabraniLekar = p.IzabraniLekar;*/
            this.IdRecepta = id;
            this.NazivLeka = naziv;
            this.BrojDanaKoriscenja = brojkor;
            this.DnevnaKolicina = kol;
            this.PocetakKoriscenja = pocetak;
            this.DatumPropisivanjaLeka = datum;
        }

        public LekarskiRecept() { } 
        
        public LekarskiRecept(int idPac) {
            this.idPacijenta = idPac;
        }
        public int IdRecepta { get; set; }
        public int idPacijenta { get; set; }
        public int IdLekara { get; set; } //??????????
        public string NazivLeka { get; set; }
        public string DatumPropisivanjaLeka { get; set; }
        public int BrojDanaKoriscenja { get; set; }
        public int DnevnaKolicina { get; set; }
        public String PocetakKoriscenja { get; set; }
    }
}
