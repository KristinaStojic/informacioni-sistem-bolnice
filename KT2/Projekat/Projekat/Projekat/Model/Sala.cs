/***********************************************************************
 * Module:  Sala.cs
 * Author:  pc
 * Purpose: Definition of the Class Sala
 ***********************************************************************/

using System;
using System.ComponentModel;

namespace Model
{
    public enum status
    {
        Zauzeta, Slobodna, Renoviranje
    }

    public enum tipSale
    {
        OperacionaSala, SalaZaPregled
    }

    public class Sala: INotifyPropertyChanged
    {
        public Sala(int id, string namjena, tipSale tip)
        {
            this.Id = id;
            this.Status = status.Slobodna;
            this.TipSale = tip;
            this.Namjena = namjena;
        }
    
        public status Status { get; set; }
        public tipSale TipSale { get; set; }
        public int Id { get; set; }
        public string Namjena { get; set; }

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
        }
        public Sala() { }
    }
}