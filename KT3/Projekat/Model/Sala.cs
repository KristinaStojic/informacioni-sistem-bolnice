/***********************************************************************
 * Module:  Sala.cs
 * Author:  pc
 * Purpose: Definition of the Class Sala
 ***********************************************************************/

using Projekat;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Model
{
   

    public enum tipSale
    {
        OperacionaSala, SalaZaPregled
    }

    public class Sala: INotifyPropertyChanged
    {
        public Sala(int id, int brojSale, string namjena, tipSale tip)
        {
            this.Id = id;
            this.brojSale = brojSale;
            this.TipSale = tip;
            this.Namjena = namjena;
            //Sanja
            this.zauzetiTermini = new List<ZauzeceSale>();
        }

        public Sala(int brojSale, string namjena, tipSale tip)
        { 
            this.brojSale = brojSale;
            this.TipSale = tip;
            this.Namjena = namjena;
            //Sanja
            this.zauzetiTermini = new List<ZauzeceSale>();
        }

        public status Status { get; set; }
        public tipSale TipSale { get; set; }
        public int Id { get; set; }
        public int brojSale { get; set; }
        public string Namjena { get; set; }
        public List<Oprema> Oprema { get; set; }
        public List<ZauzeceSale> zauzetiTermini { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public Sala(int id)
        {
            this.Id = id;
            //Sanja
            this.zauzetiTermini = new List<ZauzeceSale>();
        }
        public Sala()
        { 
            //Sanja
            this.zauzetiTermini = new List<ZauzeceSale>();
        }
        public override string ToString()
        {
            string val = brojSale + " - " + Namjena;
            if (PreraspodjelaStaticke.aktivna && PreraspodjelaStaticke.izabranaOprema != null)
            {
                bool postoji = false;
                int kolicina;
                foreach (Oprema o in Oprema) {
                    if (o.IdOpreme == PreraspodjelaStaticke.izabranaOprema.IdOpreme) {
                        kolicina = o.Kolicina;
                        foreach (Premjestaj pm in PremjestajMenadzer.premjestaji)
                        {
                            if(pm.izSale.Id == this.Id)
                            {
                                kolicina -= pm.kolicina;
                                
                                postoji = true;
                            }
                        }

                        if (!postoji)
                        {
                            val = brojSale + " - " + Namjena + " (" + o.Kolicina + ")";
                        }
                        else
                        {
                            val = brojSale + " - " + Namjena + " (" + kolicina + ")";
                        }
                    }
                }
            }else if (PreraspodjelaDinamicke.aktivna && PreraspodjelaDinamicke.izabranaOprema != null)
            {
                foreach (Oprema o in Oprema)
                {
                    if (o.IdOpreme == PreraspodjelaDinamicke.izabranaOprema.IdOpreme)
                    {
                        val += " (" + o.Kolicina + ")";
                    }
                }
            }
            return val;
        }
    }
}