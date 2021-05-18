using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ZdravstveniKarton
    {
        public int IdPacijenta { get; set; }
        public List<LekarskiRecept> LekarskiRecepti { get; set; } 
        public List<Anamneza> Anamneze { get; set; } 
        
        public List<Alergeni> Alergeni { get; set; }
        public List<Uput> Uputi { get; set; }

        public int brojLaboratorijskihUputa { get; set; }
        public int brojSpecijalistickihUputa { get; set; }
        public int brojBolnickihUputa { get; set; }

        public ZdravstveniKarton() {}
        public ZdravstveniKarton(int id)
        {
            this.IdPacijenta = id;
            this.brojBolnickihUputa = 0;
            this.brojLaboratorijskihUputa = 0;
            this.brojSpecijalistickihUputa = 0;
        }
    }
}
