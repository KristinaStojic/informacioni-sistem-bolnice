using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ZdravstveniKarton
    {
        public ZdravstveniKarton(Pacijent p) 
        {
            this.IdKartona = p.IdPacijenta;
         /*   this.ImePacijenta = p.ImePacijenta;
            this.PrezimePacijenta = p.PrezimePacijenta;
            this.Pol = p.Pol;
            this.Jmbg = p.Jmbg;
            this.StatusNaloga = p.StatusNaloga;
            this.BrojTelefona = p.BrojTelefona;
            this.Email = p.Email;
            this.AdresaStanovanja = p.AdresaStanovanja;
            this.Zanimanje = p.Zanimanje;
            this.BracnoStanje = p.BracnoStanje;
            //this.IzabraniLekar = p.IzabraniLekar;*/
        }
        public int IdKartona { get; set; }
        public string ImePacijenta { get; set; }
        public string PrezimePacijenta { get; set; }
        public pol Pol { get; set; }
        public int Jmbg { get; set; }
        public statusNaloga StatusNaloga { get; set; }
        public long BrojTelefona { get; set; }
        public string Email { get; set; }
        public string AdresaStanovanja { get; set; }
        public string Zanimanje { get; set; }
        public bracnoStanje BracnoStanje { get; set; }
        //public Lekar IzabraniLekar { get; set; }  // ili id?
        public int idLekara { get; set; }
        public String Izvestaj { get; set;  }
    }
}
