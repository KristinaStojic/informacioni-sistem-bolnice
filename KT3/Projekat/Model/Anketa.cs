using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Projekat.Model
{
    public class Anketa : INotifyPropertyChanged
    {
        public int IdAnkete { get; set; }
        public string NazivAnkete { get; set; } // ocenjivanje usluga bolnice, ocenjivanje rada lekara, ocenjivanje sajta
        public int IdPacijenta { get; set; }
        public int RedniBrojPitanja { get; set; }
        public List<string> PitanjaAnkete { get; set; }
        public int OdgovorNaPitanje { get; set; }
        public Dictionary<string, int> PitanjeOdgovor { get; set; } // ?
        public bool PopunjenaAnketa { get; set; } = false;

        public Anketa() { }

        public Anketa(int idAnkete, string naziv, int rbrPitanja, List<string> pitanja) { // set-ovati idPacijenta i odgovor i popounjenaAnketa na true
            this.IdAnkete = idAnkete;
            this.RedniBrojPitanja = rbrPitanja;
            this.PitanjaAnkete = pitanja;
            this.NazivAnkete = naziv;
        }

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
