using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class Sastojak : INotifyPropertyChanged
    {
        public string naziv { get; set; }
        public double kolicina { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public Sastojak(string naziv, double kolicina) {
            this.naziv = naziv;
            this.kolicina = kolicina;
        }

        public Sastojak() { }

    }
}
