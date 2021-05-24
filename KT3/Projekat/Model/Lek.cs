using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class Lek : INotifyPropertyChanged
    {
        public int idLeka { get; set; }
        public string nazivLeka { get; set; }
        public string sifraLeka { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public List<int> zamenskiLekovi { get; set; }
        public List<Sastojak> sastojci { get; set; }

        public Lek(int id, string naziv, string sifra)
        {
            this.idLeka = id;
            this.nazivLeka = naziv;
            this.sifraLeka = sifra;
            this.zamenskiLekovi = new List<int>();
            this.sastojci = new List<Sastojak>();
        }
        public Lek(int id, string naziv, string sifra, List<int> zamenskiLekovi, List<Sastojak> sastojci)
        {
            this.idLeka = id;
            this.nazivLeka = naziv;
            this.sifraLeka = sifra;
            this.zamenskiLekovi = zamenskiLekovi;
            this.sastojci = sastojci;
        }

        public Lek() {
            this.sastojci = new List<Sastojak>();
        }
    }
}
