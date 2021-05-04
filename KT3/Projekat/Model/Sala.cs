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
        OperacionaSala, SalaZaPregled, SalaZaOdmor
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

            this.Oprema = new List<Oprema>();
        }

        public Sala() { }
        
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
        
        public override string ToString()
        {
            string val = brojSale + " - " + Namjena;
            if (statickaAktivna())
            {
                val = stringZaStaticku();
            }else if (dinamickaAktivna())
            {
                val = stringZaDinamicku();
            }
            return val;
        }

        private string stringZaStaticku()
        {
            bool postojiPremjestajIzSale = false;
            int kolicinaPreostaleOpreme;
            string stringStaticka = "";
            foreach (Oprema o in Oprema)
            {
                if (o.IdOpreme == PreraspodjelaStaticke.izabranaOprema.IdOpreme)
                {
                    kolicinaPreostaleOpreme = o.Kolicina;
                    foreach (Premjestaj pm in PremjestajMenadzer.premjestaji)
                    {
                        if (pm.izSale.Id == this.Id)
                        {
                            kolicinaPreostaleOpreme -= pm.kolicina;

                            postojiPremjestajIzSale = true;
                        }
                    }

                    stringStaticka = napraviStringStaticke(postojiPremjestajIzSale, o.Kolicina, kolicinaPreostaleOpreme);
                }
            }
            return stringStaticka;
        }

        private string napraviStringStaticke(bool postojiPremjestajIzSale, int kolicinaOpreme, int kolicinaPreostaleOpreme)
        {
            if (!postojiPremjestajIzSale)
            {
                return brojSale + " - " + Namjena + " (" + kolicinaOpreme + ")";
            }
            else
            {
                return  brojSale + " - " + Namjena + " (" + kolicinaPreostaleOpreme + ")";
            }
        }

        private string stringZaDinamicku()
        {
            string stringDinamicka = "";
            foreach (Oprema o in Oprema)
            {
                if (o.IdOpreme == PreraspodjelaDinamicke.izabranaOprema.IdOpreme)
                {
                    stringDinamicka = brojSale + " - " + Namjena + " (" + o.Kolicina + ")";
                }
            }
            return stringDinamicka;
        }

        private bool statickaAktivna()
        {
            return PreraspodjelaStaticke.aktivna && PreraspodjelaStaticke.izabranaOprema != null;
        }

        private bool dinamickaAktivna()
        {
            return PreraspodjelaDinamicke.aktivna && PreraspodjelaDinamicke.izabranaOprema != null;
        }
    }
}