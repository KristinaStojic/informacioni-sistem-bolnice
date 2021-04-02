using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class Oprema : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public string NazivOpreme { get; set; }
        public int Kolicina { get; set; }
        public bool Staticka { get; set; }
        public int IdOpreme { get; set; }

        public Oprema(string naziv, int kolicina, bool staticka)
        {
            this.IdOpreme = OpremaMenadzer.GenerisanjeIdOpreme();
            this.NazivOpreme = naziv;
            this.Kolicina = kolicina;
            this.Staticka = staticka;
        }

    }
}
