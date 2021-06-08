using Projekat.Servis;
using System;
using System.ComponentModel;


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

        private static event EventHandler<PropertyChangedEventArgs> staticPC
                                                     = delegate { };
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged
        {
            add { staticPC += value; }
            remove { staticPC -= value; }
        }
        protected static void OnStaticPropertyChanged(string propertyName)
        {
            staticPC(null, new PropertyChangedEventArgs(propertyName));
        }

        public string NazivOpreme { get; set; }
        public int Kolicina { get; set; }
        public bool Staticka { get; set; }
        public int IdOpreme { get; set; }

        public Oprema(string naziv, int kolicina, bool staticka)
        {
            this.IdOpreme = SkladisteServis.GenerisanjeIdOpreme();
            this.NazivOpreme = naziv;
            this.Kolicina = kolicina;
            this.Staticka = staticka;

        }

        public Oprema() { }

        public override string ToString()
        {
            return NazivOpreme;
        }

    }
}
