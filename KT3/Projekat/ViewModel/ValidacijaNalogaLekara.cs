using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Model;
using Projekat.Servis;

namespace Projekat.ViewModel
{
    public class ValidacijaNalogaLekara : ValidationBase
    {
        private string imeLek;
        private string prezimeLek;
        private string jmbg;
        private string brojTelefona;
        private string email;
        private string adresaStanovanja;
        private Specijalizacija specijalizacija;
        public string stariJmbg { get; set; } = "";
        public string ImeLek { get { return imeLek; } set { imeLek = value; OnPropertyChanged("ImeLek"); } }
        public string PrezimeLek { get { return prezimeLek; } set { prezimeLek = value; OnPropertyChanged("PrezimeLek");  } }
        public string Jmbg { get { return jmbg; } set { jmbg = value; OnPropertyChanged("Jmbg");  } }
        public string BrojTelefona { get { return brojTelefona; } set { brojTelefona = value; OnPropertyChanged("BrojTelefona"); } }
        public string Email { get { return email; } set { email = value; OnPropertyChanged("Email"); } }
        public string AdresaStanovanja { get { return adresaStanovanja; } set { adresaStanovanja = value; OnPropertyChanged("AdresaStanovanja");  } }
        public Specijalizacija Specijalizacija { get { return specijalizacija; } set { specijalizacija = value; OnPropertyChanged("Specijalizacija"); } }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this.imeLek))
            {
                this.ValidationErrors["ImeLek"] = "Unesite ime";
            }

            if (string.IsNullOrWhiteSpace(this.prezimeLek))
            {
                this.ValidationErrors["PrezimeLek"] = "Unesite prezime";
            }

            if (string.IsNullOrWhiteSpace(this.jmbg) || !this.jmbg.All(Char.IsDigit) || this.jmbg.Length < 9 || this.jmbg.Length > 13)
            {
                
                this.ValidationErrors["Jmbg"] = "Potrebno je uneti 9-13 cifara!";
            }

            if (string.IsNullOrWhiteSpace(this.brojTelefona) || this.brojTelefona.Length < 6 || this.brojTelefona.Length > 10)
            {
                this.ValidationErrors["BrojTelefona"] = "Potrebno je uneti 6-10 cifara!";
            }

            if (string.IsNullOrWhiteSpace(this.email))
            {
                this.ValidationErrors["Email"] = "Unesite email";
            }

            if (string.IsNullOrWhiteSpace(this.adresaStanovanja))
            {
                this.ValidationErrors["AdresaStanovanja"] = "Unesite adresu stanovanja";
            }

            // jedinstvenost jmbg-a
            if (!string.IsNullOrWhiteSpace(this.jmbg) && this.jmbg.All(Char.IsDigit))
            {
                if (!stariJmbg.Equals(this.jmbg))
                {
                    if ((!LekariServis.JedinstvenJmbg(long.Parse(jmbg))))
                    {
                        this.ValidationErrors["Jmbg"] = "Jmbg vec postoji!";
                    }
                }
            }
        }
    }
}
