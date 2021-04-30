using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{

    public enum tipUputa
    {
        Laboratorija, SpecijallistickiPregled, StacionarnoLecenje

    }

    public class Uput
    {
        public int IdUputa { get; set; }
        public int idPacijenta { get; set; }
        public int IdLekaraKojiIzdajeUput { get; set; }
        public int IdLekaraKodKogSeUpucuje { get; set; }
        public string datumIzdavanja { get; set; }
        public string opisPregleda { get; set; }
        public int brojDanaZadrzavanja { get; set; }
        public tipUputa TipUputa { get; set; }

        public Uput() { }

        public Uput(int idUputa, int idPacijenta, int idLekaraKojiUpucuje, int IdLekaraKodKogSeUpucuje, string opis, string datum, tipUputa tip)
        {
            this.IdUputa = idUputa;
            this.IdLekaraKojiIzdajeUput = idLekaraKojiUpucuje;
            this.idPacijenta = idPacijenta;
            this.IdLekaraKodKogSeUpucuje = IdLekaraKodKogSeUpucuje;
            this.opisPregleda = opis;
            this.datumIzdavanja = datum;
            this.TipUputa = tip;
        }

        public override string ToString()
        {
            if (TipUputa.Equals(tipUputa.SpecijallistickiPregled))
                 return "Specijalistički pregled";
            else if (TipUputa.Equals(tipUputa.Laboratorija))
                 return "Laboratorija";
            else //if (TipUputa.Equals(tipUputa.StacionarnoLecenje))
                 return "Stacionarno lečenje";
        }

    }

}

