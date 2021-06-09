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
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Notify();
    }

    public class ObjavaObavestenja : ISubject
    {
        private List<IObserver> observers;
        private Obavestenja novoObavestenje;
        public Obavestenja NovoObavestenje
        {
            get { return novoObavestenje; }
            set
            {
                novoObavestenje = value;
                Notify();
            }
        }

        public ObjavaObavestenja()
        {
            observers = new List<IObserver>();
        }

        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Notify()
        {
            foreach (IObserver o in observers)
            {
                o.Update(this);
            }
        }
    }

    public interface IObserver
    {
        void Update(ISubject subject);
    }

    public class Lekar : IObserver, INotifyPropertyChanged
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

        public List<Obavestenja> obavestenja;

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
            this.korisnickoIme = Prezime;
            this.lozinka = Jmbg.ToString();
            this.obavestenja = new List<Obavestenja>();
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
            this.obavestenja = new List<Obavestenja>();
        }

        public Lekar(int id)
        {
            this.IdLekara = id;
        }
        public Lekar() { }

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

        public void Update(ISubject subject)
        {
            Console.WriteLine("dodato obavestenje");
        }
    }
}