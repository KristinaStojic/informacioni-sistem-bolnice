/***********************************************************************
 * Module:  Sala.cs
 * Author:  pc
 * Purpose: Definition of the Class Sala
 ***********************************************************************/

using Projekat;
using Projekat.Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace Model
{


    public enum tipSale
    {
        OperacionaSala, SalaZaPregled, SalaZaLezanje
    }

    public class Sala : INotifyPropertyChanged
    {
        public Sala() { }
        public tipSale TipSale { get; set; }
        public int Id { get; set; }
        public int brojSale { get; set; }
        public string Namjena { get; set; }
        public List<Oprema> Oprema { get; set; }
        public List<ZauzeceSale> zauzetiTermini { get; set; }

        public List<Krevet> Kreveti { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        public Sala(int id, int brojSale, string namjena, tipSale tip)
        {
            this.Id = id;
            this.brojSale = brojSale;
            this.TipSale = tip;
            this.Namjena = namjena;
            this.zauzetiTermini = new List<ZauzeceSale>();
            this.Oprema = new List<Oprema>();
            this.Kreveti = new List<Krevet>();
        }

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public override string ToString()
        {
            string stringSale = brojSale + " - " + Namjena;
            if (statickaAktivna())
            {
                stringSale = stringZaStaticku();
            }
            else if (dinamickaAktivna())
            {
                stringSale = stringZaDinamicku();
            }
            return stringSale;
        }

        private string stringZaStaticku()
        {
            string stringStaticka = "";
            foreach (Oprema oprema in Oprema)
            {
                if (oprema.IdOpreme == PreraspodjelaStaticke.izabranaOprema.IdOpreme)
                {
                    stringStaticka = napraviStringStaticke(smanjiKolicinuPreostaleOpreme(oprema));
                }
            }
            return stringStaticka;
        }

        private int smanjiKolicinuPreostaleOpreme(Oprema oprema)
        {
            int kolicinaPreostaleOpreme = oprema.Kolicina;
            foreach (Premjestaj premjestaj in PremjestajMenadzer.premjestaji)
            {
                if (premjestaj.izSale.Id == this.Id && premjestaj.oprema.IdOpreme == PreraspodjelaStaticke.izabranaOprema.IdOpreme)
                {
                    kolicinaPreostaleOpreme -= premjestaj.kolicina;
                }
            }
            return kolicinaPreostaleOpreme;
        }

        private string napraviStringStaticke(int kolicinaPreostaleOpreme)
        {
            return brojSale + " - " + Namjena + " (" + kolicinaPreostaleOpreme + ")";    
        }

        private string stringZaDinamicku()
        {
            string stringDinamicka = "";
            foreach (Oprema oprema in Oprema)
            {
                if (oprema.IdOpreme == PreraspodjelaDinamicke.izabranaOprema.IdOpreme)
                {
                    stringDinamicka = brojSale + " - " + Namjena + " (" + oprema.Kolicina + ")";
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