/***********************************************************************
 * Module:  Termini.cs
 * Author:  Kristina
 * Purpose: Definition of the Class Termini
 ***********************************************************************/

using System;
using System.ComponentModel;

namespace Model
{
    public enum TipTermina
    {
        Operacija, Pregled
    }

    public class Termin: INotifyPropertyChanged
    {

        public int IdTermin { get; set; }
        public String VremePocetka { get; set; }
        public String VremeKraja { get; set; }
        public Lekar Lekar { get; set; }
        public Pacijent Pacijent { get; set; }

        public String imePrezimePacijenta 
        {
            get
            {
                return Pacijent.ImePacijenta + " " + Pacijent.PrezimePacijenta; 
            } 
        }

        public String imePrezimeLekara
        {
            get
            {
                if (Lekar != null)
                {
                    return Lekar.ImeLek + " " + Lekar.PrezimeLek;
                }
                else
                {
                    return null;
                }
            }
        }

        public String Datum { get; set; }

        public Sala Prostorija { get; set; }

        public TipTermina tipTermina { get; set; }

        public bool Pomeren { get; set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public Termin(int broj, String dt, String vp, String vk, TipTermina tp, Lekar l, Sala s, Pacijent p)
        {
            this.IdTermin = broj;
            this.Datum = dt;
            this.VremePocetka = vp;
            this.VremeKraja = vk;
            this.tipTermina = tp;
            this.Lekar = l;
            this.Prostorija = s;
            this.Pacijent = p;
            //this.Pomeren = false;
        }

        // Sanja
        public Termin() { }

        public Termin(int broj, String dt, String vp, String vk, TipTermina tp, Lekar l)
        {
            this.IdTermin = broj;
            this.Datum = dt;
            this.VremePocetka = vp;
            this.VremeKraja = vk;
            this.tipTermina = tp;
            this.Lekar = l;
            //this.Prostorija = s;
            //this.Pacijent = p;
            //this.Pomeren = false;
        }
        public Termin(int broj, String dt, String vp, String vk, TipTermina tp)
        {
            this.IdTermin = broj;
            this.Datum = dt;
            this.VremePocetka = vp;
            this.VremeKraja = vk;
            this.tipTermina = tp;
            //this.Lekar = l;
            //this.Prostorija = s;
            //this.Pacijent = p;
            //this.Pomeren = false;
        }
    }
}