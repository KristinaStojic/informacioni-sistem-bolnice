using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Projekat.Model
{
    public class LekarskiRecept : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public LekarskiRecept(Pacijent p, int id, string naziv, string datum, int brojkor, int kol, String pocetak, List<DateTime> uzimanjeTerapije)
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
            this.UzimanjeTerapije = uzimanjeTerapije;
            this.DatumPropisivanjaLeka = datum;

        }

        public LekarskiRecept() {}

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
        // Sanja
       // public bool Obavestenje { get; set;}
        public List<DateTime> UzimanjeTerapije { get; set; }
    }
}
