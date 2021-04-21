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

        public ZdravstveniKarton() {}
        public ZdravstveniKarton(int id)
        {
            this.IdPacijenta = id;
        }
    }
}
