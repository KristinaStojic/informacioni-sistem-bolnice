using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projekat.Model;
using Model;
using System.ComponentModel;

namespace Projekat.Model
{
    public enum VrstaObavestenja 
    { 
        Zakazan, Izmenjen, Otkazan 
    }

    public class Obavestenja : INotifyPropertyChanged
    {
        public Obavestenja() { }
        public Obavestenja(String datum, string TipOb, string SadrzajOb, VrstaObavestenja vrsta)
        {
            this.Datum = datum;
            this.TipObavestenja = TipOb;
            this.SadrzajObavestenja = SadrzajOb;
            this.Vrsta = vrsta;
        }

        public Obavestenja(String datum, string TipOb, string SadrzajOb)
        {
            this.Datum = datum;
            this.TipObavestenja = TipOb;
            this.SadrzajObavestenja = SadrzajOb;
        }
        public string TipObavestenja { get; set; } 
        public string SadrzajObavestenja { get; set; }      
        public string Datum { get; set; }
        public VrstaObavestenja Vrsta { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
