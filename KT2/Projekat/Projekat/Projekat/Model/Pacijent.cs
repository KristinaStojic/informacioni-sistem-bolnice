/***********************************************************************
 * Module:  NalogPacijenta.cs
 * Author:  Teodora
 * Purpose: Definition of the Class NalogPacijenta
 ***********************************************************************/

using System;
using System.ComponentModel;

namespace Model
{
    public enum statusNaloga
    {
        Stalni, Guest
    }
    public class Pacijent : INotifyPropertyChanged
    {
        public Pacijent(int IdPacijenta, string ImePacijenta, string PrezimePacijenta, int Jmbg, long BrojTelefona,
            string Email, string AdresaStanovanja, statusNaloga Status)
        {
            this.IdPacijenta = IdPacijenta;
            this.ImePacijenta = ImePacijenta;
            this.PrezimePacijenta = PrezimePacijenta;
            this.Jmbg = Jmbg;
            this.BrojTelefona = BrojTelefona;
            this.Email = Email;
            this.AdresaStanovanja = AdresaStanovanja;
            this.StatusNaloga = Status;
        }

        public int IdPacijenta { get; set; }
        public string ImePacijenta { get; set; }
        public string PrezimePacijenta { get; set; }
        public int Jmbg { get; set; }
        public statusNaloga StatusNaloga { get; set; }
        public long BrojTelefona { get; set; }
        public string Email { get; set; }
        public string AdresaStanovanja { get; set; }
        public Lekar izabraniLekar { get; set; }

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