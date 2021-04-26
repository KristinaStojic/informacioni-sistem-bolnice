using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projekat.Model;
using Model;
using System.ComponentModel;

namespace Projekat.Model
{
    public class Obavestenja : INotifyPropertyChanged
    {
        public Obavestenja() { }

        public Obavestenja(String datum, string TipOb, string SadrzajOb, int IdPacijenta, int IdLekara, bool UpravnikObavestenja)
        {
            this.Datum = datum;
            this.TipObavestenja = TipOb;
            this.SadrzajObavestenja = SadrzajOb;
            this.IdPacijenta = IdPacijenta;
            this.IdLekara = IdLekara;
            this.UpravnikObavestenja = UpravnikObavestenja;
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

        // dodato idPacijenta i idLekara
        public int IdPacijenta { get; set; }
        public int IdLekara { get; set; }
        public bool UpravnikObavestenja { get; set; }

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
