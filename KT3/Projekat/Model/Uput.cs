using Projekat.Interfejsi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.Model
{

    public enum tipUputa
    {
        Laboratorija, SpecijalistickiPregled, StacionarnoLecenje

    }

    public class Uput
    {
        //public object TipUputa { get; set; }
        public object NadjiVrstuUputaInterfejs()
        {
            if(this.TipUputa == tipUputa.Laboratorija)
            {
                return new Laboratorija();
            }
            else if(this.TipUputa == tipUputa.SpecijalistickiPregled)
            {
                return new SpecijalistickiPregled();
            }
            else
            {
                return new BolnickoLecenje();
            }

           
        }


        public int IdUputa { get; set; }
        public int idPacijenta { get; set; }
        public int IdLekaraKojiIzdajeUput { get; set; }
        public int IdLekaraKodKogSeUpucuje { get; set; }
        public string datumIzdavanja { get; set; }
        public string opisPregleda { get; set; }
        public int brojSobe { get; set; }
        public int brojKreveta { get; set; }

        public tipUputa TipUputa { get; set; }

        public string datumPocetkaLecenja { get; set; }
        public string datumKrajaLecenja { get; set; }


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
        
        public Uput(int idUputa, int idPacijenta, int idLekaraKojiUpucuje, int IdLekaraKodKogSeUpucuje, string opis, string datum)
        {
            this.IdUputa = idUputa;
            this.IdLekaraKojiIzdajeUput = idLekaraKojiUpucuje;
            this.idPacijenta = idPacijenta;
            this.IdLekaraKodKogSeUpucuje = IdLekaraKodKogSeUpucuje;
            this.opisPregleda = opis;
            this.datumIzdavanja = datum;
           
        }
        
        public Uput(int idUputa, int idPacijenta, int idLekaraKojiUpucuje, tipUputa tip)
        {
            this.IdUputa = idUputa;
            this.IdLekaraKojiIzdajeUput = idLekaraKojiUpucuje;
            this.idPacijenta = idPacijenta;  
            this.TipUputa = tip;
        }

        public Uput(int idUputa, int idPacijenta, int idLekaraKojiUpucuje, tipUputa tip, String opis)
        {
            this.IdUputa = idUputa;
            this.IdLekaraKojiIzdajeUput = idLekaraKojiUpucuje;
            this.idPacijenta = idPacijenta;  
            this.TipUputa = tip;
            this.opisPregleda = opis;
        }

        public Uput(int idUputa, int idPacijenta, int idLekaraKojiUpucuje, int brojSobe, int brojKreveta, string krajLecenja, string datumPocetkaLecenja,string datumIzdavanja, string napomena, tipUputa tip)
        {
            this.IdUputa = idUputa;
            this.IdLekaraKojiIzdajeUput = idLekaraKojiUpucuje;
            this.idPacijenta = idPacijenta;
            this.opisPregleda = napomena;
            this.datumKrajaLecenja = krajLecenja;
            this.brojSobe = brojSobe;
            this.brojKreveta = brojKreveta;
            this.datumPocetkaLecenja = datumPocetkaLecenja;
            this.datumIzdavanja = datumIzdavanja;
            this.TipUputa = tip;


        }

        public override string ToString()
        {
            if (TipUputa.Equals(tipUputa.SpecijalistickiPregled))
                return "Specijalistički pregled";
            else if (TipUputa.Equals("Laboratorija"))
                return "Laboratorija";
            else //if (TipUputa.Equals(tipUputa.StacionarnoLecenje))
                 return "Bolničko lečenje";
        }

    }

}

