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
        public int Jmbg { get; set; }
        public long BrojTelefona { get; set; }
        public string Email { get; set; }
        public string AdresaStanovanja { get; set; }
        public Specijalizacija specijalizacija { get; set; }
        public int SlobodniDaniGodisnjegOdmora { get; set; }
        public int ZahtevaniiDaniGodisnjegOdmora { get; set; } // prilikom slanja zahteva od strane lekara 
        public List<int> zahteviZaOdmor { get; set; }

        // radno vreme?

        public Lekar(int IdLekara, string Ime, string Prezime, int Jmbg, long BrojTelefona, string Email, string AdresaStanovanja, Specijalizacija Specijalizacija)
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
            this.ZahtevaniiDaniGodisnjegOdmora = 0;
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