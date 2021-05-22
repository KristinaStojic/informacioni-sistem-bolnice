/***********************************************************************
 * Module:  NalogPacijenta.cs
 * Author:  Teodora
 * Purpose: Definition of the Class NalogPacijenta
 ***********************************************************************/

using System;
using System.ComponentModel;
using Projekat.Model;

namespace Model
{
    public enum bracnoStanje
    { 
        Neozenjen, Neudata, Ozenjen, Udata, Udovac, Udovica, Razveden, Razvedena, Neodredjeno
    }
    public enum statusNaloga
    {
        Stalni, Guest
    }
    public enum pol
    {
        M, Z
    }
    public class Pacijent : INotifyPropertyChanged
    {
        public Pacijent(int IdPacijenta, string ImePacijenta, string PrezimePacijenta, long Jmbg, pol pol, long BrojTelefona, string Email, string AdresaStanovanja, statusNaloga Status, String zanimanje, bracnoStanje stanje)
        {
            this.IdPacijenta = IdPacijenta;
            this.ImePacijenta = ImePacijenta;
            this.PrezimePacijenta = PrezimePacijenta;
            this.Pol = pol;
            this.Jmbg = Jmbg;
            this.BrojTelefona = BrojTelefona;
            this.Email = Email;
            this.AdresaStanovanja = AdresaStanovanja;
            this.StatusNaloga = Status;
            this.Zanimanje = zanimanje;
            this.BracnoStanje = stanje;
        }

        public Pacijent(int IdPacijenta, string ImePacijenta, string PrezimePacijenta, long Jmbg, pol pol, long BrojTelefona, string Email, string AdresaStanovanja, statusNaloga Status, String zanimanje, bracnoStanje stanje, bool Maloletnik, long JmbgStaratelja)
        {
            this.IdPacijenta = IdPacijenta;
            this.ImePacijenta = ImePacijenta;
            this.PrezimePacijenta = PrezimePacijenta;
            this.Pol = pol;
            this.Jmbg = Jmbg;
            this.BrojTelefona = BrojTelefona;
            this.Email = Email;
            this.AdresaStanovanja = AdresaStanovanja;
            this.StatusNaloga = Status;
            this.Zanimanje = zanimanje;
            this.BracnoStanje = stanje;
            this.Maloletnik = Maloletnik;
            this.JmbgStaratelja = JmbgStaratelja;
        }

        public Pacijent() { }

        public Pacijent(int IdPacijenta, string ImePacijenta, string PrezimePacijenta, long Jmbg, pol pol, statusNaloga Status)
        {
            this.IdPacijenta = IdPacijenta;
            this.ImePacijenta = ImePacijenta;
            this.PrezimePacijenta = PrezimePacijenta;
            this.Jmbg = Jmbg;
            this.Pol = pol;
            this.StatusNaloga = Status;
        }

        public int IdPacijenta { get; set; }
        public string ImePacijenta { get; set; }
        public string PrezimePacijenta { get; set; }
        public pol Pol { get; set; }
        public long Jmbg { get; set; }
        public statusNaloga StatusNaloga { get; set; }
        public long BrojTelefona { get; set; }
        public string Email { get; set; }
        public string AdresaStanovanja { get; set; }
        public string Zanimanje { get; set; }
        public bracnoStanje BracnoStanje { get; set; }
        public Lekar IzabraniLekar { get; set; }
        public ZdravstveniKarton Karton { get; set; }
        public bool Maloletnik { get; set; }
        public long JmbgStaratelja { get; set; } 

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