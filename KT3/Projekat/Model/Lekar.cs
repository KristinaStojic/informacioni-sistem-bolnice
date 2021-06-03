/***********************************************************************
 * Module:  Lekar.cs
 * Author:  Kristina
 * Purpose: Definition of the Class Lekar
 ***********************************************************************/

using Projekat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Model
{
    public class Lekar : INotifyPropertyChanged
    {
        public const int MAX_DANA_GODISNJEG_ODMORA = 25;
        public int IdLekara { get; set; }
        public string ImeLek { get; set; }
        public string PrezimeLek { get; set; }
        public long Jmbg { get; set; }
        public long BrojTelefona { get; set; }
        public string Email { get; set; }
        public string AdresaStanovanja { get; set; }
        public Specijalizacija specijalizacija { get; set; }
        public int SlobodniDaniGodisnjegOdmora { get; set; }
        public int ZahtevaniDaniGodisnjegOdmora { get; set; } 
        public List<int> ZahteviZaOdmor { get; set; }
        public List<RadniDan> RadniDani { get; set; }
        public int BrojPregleda { get; set; }
        public int BrojOperacija { get; set; }
        
        public string korisnickoIme { get; set; }
        public string lozinka { get; set; }

        public Lekar(int IdLekara, string Ime, string Prezime, long Jmbg, long BrojTelefona, string Email, string AdresaStanovanja, Specijalizacija Specijalizacija)
        {
            this.IdLekara = IdLekara;
            this.ImeLek = Ime;
            this.PrezimeLek = Prezime;
            this.Jmbg = Jmbg;
            this.BrojTelefona = BrojTelefona;
            this.Email = Email;
            this.AdresaStanovanja = AdresaStanovanja;
            this.specijalizacija = Specijalizacija;
            this.SlobodniDaniGodisnjegOdmora = MAX_DANA_GODISNJEG_ODMORA;
            this.ZahtevaniDaniGodisnjegOdmora = 0;
            this.BrojOperacija = 0;
            this.BrojPregleda = 0;
            this.ZahteviZaOdmor = new List<int>();
            this.RadniDani = new List<RadniDan>();
        }
         public Lekar(int IdLekara, string Ime, string Prezime, long Jmbg, long BrojTelefona, string Email, string AdresaStanovanja, Specijalizacija Specijalizacija, String korIme, String sifra)
        {
            this.IdLekara = IdLekara;
            this.ImeLek = Ime;
            this.PrezimeLek = Prezime;
            this.Jmbg = Jmbg;
            this.BrojTelefona = BrojTelefona;
            this.Email = Email;
            this.AdresaStanovanja = AdresaStanovanja;
            this.specijalizacija = Specijalizacija;
            this.SlobodniDaniGodisnjegOdmora = MAX_DANA_GODISNJEG_ODMORA;
            this.ZahtevaniDaniGodisnjegOdmora = 0;
            this.BrojOperacija = 0;
            this.BrojPregleda = 0;
            this.ZahteviZaOdmor = new List<int>();
            this.RadniDani = new List<RadniDan>();
            this.korisnickoIme = korIme;
            this.lozinka = sifra;
        }

        public Lekar(int id)
        {
            this.IdLekara = id;
        }
        public Lekar() { }

        // Sanja
        public Lekar(int id, string ime, string prz, Specijalizacija specijalizacija)
        {
            this.ImeLek = ime;
            this.PrezimeLek = prz;
            this.specijalizacija = specijalizacija;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public override string ToString()
        {
            return this.ImeLek + " " + this.PrezimeLek;
        }

    }
}