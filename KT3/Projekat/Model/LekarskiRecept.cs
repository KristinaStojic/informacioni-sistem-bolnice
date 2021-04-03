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

        public LekarskiRecept(Pacijent p, int id, string naziv, string datum, int brojkor, int kol, String pocetak)
        {
                 
              this.ImePacijenta = p.ImePacijenta;
               this.PrezimePacijenta = p.PrezimePacijenta;
               this.idPacijenta = p.IdPacijenta;
            //this.IzabraniLekar = p.IzabraniLekar;*/
            this.IdRecepta = id;
            this.NazivLeka = naziv;
            this.DatumPropisivanjaLeka = datum;
            this.BrojDanaKoriscenja = brojkor;
            this.DnevnaKolicina = kol;
            this.PocetakKoriscenja = pocetak;
        }
        public int IdRecepta { get; set; }
        public string ImePacijenta { get; set; }
        public string PrezimePacijenta { get; set; }
        public int idPacijenta { get; set; }
        public int IdLekara { get; set; } //??????????
        public string NazivLeka { get; set; }
        public string DatumPropisivanjaLeka { get; set; }
        public int BrojDanaKoriscenja { get; set; }
        public int DnevnaKolicina { get; set; }
        public String PocetakKoriscenja { get; set; }
    }
}
