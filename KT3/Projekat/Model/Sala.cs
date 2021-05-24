/***********************************************************************
 * Module:  Sala.cs
 * Author:  pc
 * Purpose: Definition of the Class Sala
 ***********************************************************************/

using Projekat;
using Projekat.Model;
using Projekat.Servis;
using Projekat.ViewModel;
using System;
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
            string stringStaticka = brojSale + " - " + Namjena;
            if (SaleViewModel.izabranaStat != null)
            {
                foreach (Oprema oprema in Oprema)
                {
                    if (oprema.IdOpreme == SaleViewModel.izabranaStat.IdOpreme)
                    {
                        stringStaticka = napraviStringStaticke(smanjiKolicinuPreostaleOpreme(oprema));
                    }
                }
            }
            return stringStaticka;
        }

        private int smanjiKolicinuPreostaleOpreme(Oprema oprema)
        {
            int kolicinaPreostaleOpreme = oprema.Kolicina;
            foreach (Premjestaj premjestaj in PremjestajServis.Premjestaji())
            {
                if (premjestaj.izSale.Id == this.Id && premjestaj.oprema.IdOpreme == SaleViewModel.izabranaStat.IdOpreme)
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
            string stringDinamicka = brojSale + " - " + Namjena;
            foreach (Oprema oprema in Oprema)
            {
                if (oprema.IdOpreme == SaleViewModel.opremaZaDodavanje.IdOpreme)
                {
                    stringDinamicka +=  " (" + oprema.Kolicina + ")";
                }
            }
            return stringDinamicka;
        }

        private bool statickaAktivna()
        {
            return SaleViewModel.aktivnaStaticka;
        }

        private bool dinamickaAktivna()
        {
            return SaleViewModel.aktivnaDinamicka && SaleViewModel.opremaZaDodavanje != null;
        }
    }
}