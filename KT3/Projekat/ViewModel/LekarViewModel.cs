using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Model;
using Projekat.Servis;

namespace Projekat.ViewModel
{
    public class LekarViewModel: BindableBase
    {
        #region Lekari
        public static Window LekariProzor { get; set; }
        public Window DodajLekaraProzor { get; set; }
        public Window IzmeniLekaraProzor { get; set; }
        public Window BrisanjeLekaraProzor { get; set; }

        private ObservableCollection<Lekar> lekari;
        public ObservableCollection<Lekar> Lekari 
        {
            get { return lekari; } 
            set { lekari = value; OnPropertyChanged("Lekari"); } 
        }
        public MyICommand ZatvoriLekara { get; set; }
        public MyICommand JmbgLostFocus { get; set; }
        public MyICommand OtvoriPrikazSaStrane { get; set; }

        public LekarViewModel()
        {
            DodajLekaraKomanda = new MyICommand(OtvoriDodavanjeLekara);
            OdustaniOdDodavanjaLekara = new MyICommand(ZatvoriDodavanjeLekara);
            PotvrdiDodavanjeLekara = new MyICommand(DodajNovogLekara, ValidnaPoljaZaDodavanjeLekara);
            ZatvoriLekara = new MyICommand(Zatvori);
            JmbgLostFocus = new MyICommand(JedinstvenJmbg);
            
            IzmeniLekaraKomanda = new MyICommand(OtvoriIzmenuLekara);
            OdustaniOdIzmeneLekara = new MyICommand(ZatvoriIzmenuLekara);
            PotvrdiIzmenuLekara = new MyICommand(IzmeniIzabranogLekara, ValidnaPoljaZaIzmenuLekara);

            ObrisiLekaraKomanda = new MyICommand(ObrisiLekara);
            PotvrdiBrisanjeLekara = new MyICommand(ObrisiIzabranogLekara);
            OdustaniOdBrisanjaLekara = new MyICommand(ZatvoriBrisanjeLekara);

            DodajLekare();
        }

        private void DodajLekare()
        {
            Lekari = new ObservableCollection<Lekar>();
            List<Lekar> lekariLista = LekariServis.NadjiSveLekare();
            foreach (Lekar lekar in lekariLista)
            {
                Lekari.Add(lekar);
            }
        }

        private void Zatvori()
        {
            LekariServis.SacuvajIzmeneLekara();
        }
      
        #endregion

        #region DodajLekaraViewModel
        public MyICommand DodajLekaraKomanda { get; set; }
        public MyICommand OdustaniOdDodavanjaLekara { get; set; }
        public MyICommand PotvrdiDodavanjeLekara { get; set; }

        private string imeLekaraDodavanje;
        private string prezimeLekaraDodavanje;
        private string jmbgLekaraDodavanje;
        private string brojTelefonaDodavanje;
        private string emailDodavanje;
        private string adresaDodavanje;
        private Specijalizacija specijalizacijaDodavanje;

        public string ImeLekaraDodavanje { get { return imeLekaraDodavanje; } set { imeLekaraDodavanje = value; OnPropertyChanged("ImeLekaraDodavanje"); PotvrdiDodavanjeLekara.RaiseCanExecuteChanged(); } }
        public string PrezimeLekaraDodavanje { get { return prezimeLekaraDodavanje; } set { prezimeLekaraDodavanje = value; OnPropertyChanged("PrezimeLekaraDodavanje"); PotvrdiDodavanjeLekara.RaiseCanExecuteChanged(); } }
        public string JmbgLekaraDodavanje { get { return jmbgLekaraDodavanje; } set { jmbgLekaraDodavanje = value; OnPropertyChanged("JmbgLekaraDodavanje"); PotvrdiDodavanjeLekara.RaiseCanExecuteChanged(); } }
        public string BrojTelefonaDodavanje { get { return brojTelefonaDodavanje; } set { brojTelefonaDodavanje = value; OnPropertyChanged("BrojTelefonaDodavanje"); PotvrdiDodavanjeLekara.RaiseCanExecuteChanged(); } }
        public string EmailDodavanje { get { return emailDodavanje; } set { emailDodavanje = value; OnPropertyChanged("EmailDodavanje"); PotvrdiDodavanjeLekara.RaiseCanExecuteChanged(); } }
        public string AdresaDodavanje { get { return adresaDodavanje; } set { adresaDodavanje = value; OnPropertyChanged("AdresaDodavanje"); PotvrdiDodavanjeLekara.RaiseCanExecuteChanged(); } }
        public Specijalizacija SpecijalizacijaDodavanje { get { return specijalizacijaDodavanje; } set { specijalizacijaDodavanje = value; OnPropertyChanged("SpecijalizacijaDodavanje"); PotvrdiDodavanjeLekara.RaiseCanExecuteChanged(); } }

        private void OtvoriDodavanjeLekara()
        {
            DodajLekaraProzor = new DodajLekara();
            DodajLekaraProzor.Show();
            imeLekaraDodavanje = "";
            prezimeLekaraDodavanje = "";
            jmbgLekaraDodavanje = "";
            brojTelefonaDodavanje = "";
            emailDodavanje = "";
            adresaDodavanje = "";
            DodajLekaraProzor.DataContext = this;
        }

        private void DodajNovogLekara()
        {
            Lekar lekar = new Lekar(LekariServis.GenerisanjeIdLekara(), imeLekaraDodavanje, prezimeLekaraDodavanje, long.Parse(jmbgLekaraDodavanje), long.Parse(brojTelefonaDodavanje), emailDodavanje, adresaDodavanje, specijalizacijaDodavanje);
            LekariServis.DodajLekara(lekar);
            Lekari.Add(lekar);
            DodajLekaraProzor.Close();
        }

        private void JedinstvenJmbg()
        {
            if (!(jmbgLekaraDodavanje.Equals("")))
            {
                if ((!LekariServis.JedinstvenJmbg(long.Parse(jmbgLekaraDodavanje))))
                {
                    MessageBox.Show("JMBG vec postoji");
                    JmbgLekaraDodavanje = "";
                }
            }
        }

        private bool ValidnaPoljaZaDodavanjeLekara()
        {
            if (imeLekaraDodavanje != null && prezimeLekaraDodavanje != null && jmbgLekaraDodavanje != null && emailDodavanje != null && brojTelefonaDodavanje != null && adresaDodavanje != null && specijalizacijaDodavanje != null)
            {
                //Console.WriteLine(jmbgLekaraDodavanje.Length);
                if (imeLekaraDodavanje.Trim().Equals("") || prezimeLekaraDodavanje.Trim().Equals("") || !ManjiOd6(jmbgLekaraDodavanje) || !jeBroj(jmbgLekaraDodavanje) || !jeBroj(brojTelefonaDodavanje) || emailDodavanje.Trim().Equals("") || adresaDodavanje.Trim().Equals("") )
                {
                    return false;
                }
                else if (!imeLekaraDodavanje.Trim().Equals("") && !prezimeLekaraDodavanje.Trim().Equals("") && ManjiOd6(jmbgLekaraDodavanje) && jeBroj(jmbgLekaraDodavanje) && jeBroj(brojTelefonaDodavanje) && !emailDodavanje.Trim().Equals("") && !adresaDodavanje.Trim().Equals(""))
                {
                    return true;
                }
            }

            return false;
        }

        public bool jeBroj(string tekst)
        {
            long test;
            return long.TryParse(tekst, out test);
        }

        public bool ManjiOd6(string tekst)
        {
            if (tekst.Length < 6)
            {
                return false;
            }
            return true;
        }

        private void ZatvoriDodavanjeLekara()
        {
            DodajLekaraProzor.Close();
        }

        #endregion

        #region IzmeniLekaraViewModel

        public MyICommand IzmeniLekaraKomanda { get; set; }
        public MyICommand OdustaniOdIzmeneLekara { get; set; }
        public MyICommand PotvrdiIzmenuLekara { get; set; }

        private Lekar izabraniLekar;
        public Lekar IzabraniLekar
        {
            get { return izabraniLekar; }
            set { izabraniLekar = value; OnPropertyChanged("IzabraniLekar"); }
        }

        private string imeLekaraIzmena;
        private string prezimeLekaraIzmena;
        private string jmbgLekaraIzmena;
        private string brojTelefonaIzmena;
        private string emailIzmena;
        private string adresaIzmena;
        private Specijalizacija specijalizacijaIzmena;

        public string ImeLekaraIzmena { get { return imeLekaraIzmena; } set { imeLekaraIzmena = value; OnPropertyChanged("ImeLekaraIzmena"); PotvrdiIzmenuLekara.RaiseCanExecuteChanged(); } }
        public string PrezimeLekaraIzmena { get { return prezimeLekaraIzmena; } set { prezimeLekaraIzmena = value; OnPropertyChanged("PrezimeLekaraIzmena"); PotvrdiIzmenuLekara.RaiseCanExecuteChanged(); } }
        public string JmbgLekaraIzmena { get { return jmbgLekaraIzmena; } set { jmbgLekaraIzmena = value; OnPropertyChanged("JmbgLekaraIzmena"); PotvrdiIzmenuLekara.RaiseCanExecuteChanged(); } }
        public string BrojTelefonaIzmena { get { return brojTelefonaIzmena; } set { brojTelefonaIzmena = value; OnPropertyChanged("BrojTelefonaIzmena"); PotvrdiIzmenuLekara.RaiseCanExecuteChanged(); } }
        public string EmailIzmena { get { return emailIzmena; } set { emailIzmena = value; OnPropertyChanged("EmailIzmena"); PotvrdiIzmenuLekara.RaiseCanExecuteChanged(); } }
        public string AdresaIzmena { get { return adresaIzmena; } set { adresaIzmena = value; OnPropertyChanged("AdresaIzmena"); PotvrdiIzmenuLekara.RaiseCanExecuteChanged(); } }
        public Specijalizacija SpecijalizacijaIzmena { get { return specijalizacijaIzmena; } set { specijalizacijaIzmena = value; OnPropertyChanged("SpecijalizacijaIzmena"); PotvrdiIzmenuLekara.RaiseCanExecuteChanged(); } }

        private void OtvoriIzmenuLekara()
        {
            if (izabraniLekar != null)
            {
                IzmeniLekaraProzor = new IzmeniLekara();
                IzmeniLekaraProzor.Show();

                imeLekaraIzmena = izabraniLekar.ImeLek;
                prezimeLekaraIzmena = izabraniLekar.PrezimeLek;
                jmbgLekaraIzmena = izabraniLekar.Jmbg.ToString();
                brojTelefonaIzmena = izabraniLekar.BrojTelefona.ToString();
                emailIzmena = izabraniLekar.AdresaStanovanja;
                adresaIzmena = izabraniLekar.AdresaStanovanja;
                specijalizacijaIzmena = izabraniLekar.specijalizacija;

                IzmeniLekaraProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati lekara!");
            }
        }

        private void IzmeniIzabranogLekara()
        {
            Lekar noviLekar = new Lekar(izabraniLekar.IdLekara, imeLekaraIzmena, prezimeLekaraIzmena, long.Parse(jmbgLekaraIzmena), long.Parse(brojTelefonaIzmena), emailIzmena, adresaIzmena, specijalizacijaIzmena);
            LekariServis.IzmeniLekara(izabraniLekar, noviLekar);

            int idx = Lekari.IndexOf(izabraniLekar);
            Lekari.RemoveAt(idx);
            Lekari.Insert(idx, noviLekar);

            IzmeniLekaraProzor.Close();
        }

        private bool ValidnaPoljaZaIzmenuLekara()
        {
            if (imeLekaraIzmena != null && prezimeLekaraIzmena != null && jmbgLekaraIzmena != null && emailIzmena != null && brojTelefonaIzmena != null && adresaIzmena != null && specijalizacijaIzmena != null)
            {
                //Console.WriteLine(jmbgLekaraIzmena.Length);
                if (imeLekaraIzmena.Trim().Equals("") || prezimeLekaraIzmena.Trim().Equals("") || !ManjiOd6(jmbgLekaraIzmena) || !jeBroj(jmbgLekaraIzmena) || !jeBroj(brojTelefonaIzmena) ||  emailIzmena.Trim().Equals("") || adresaIzmena.Trim().Equals(""))
                {
                    return false;
                }
                else if (!imeLekaraIzmena.Trim().Equals("") && !prezimeLekaraIzmena.Trim().Equals("") && ManjiOd6(jmbgLekaraIzmena) && jeBroj(jmbgLekaraIzmena) && jeBroj(brojTelefonaIzmena) && !emailIzmena.Trim().Equals("") && !adresaIzmena.Trim().Equals(""))
                {
                    return true;
                }
            }

            return false;
        }

        private void ZatvoriIzmenuLekara()
        {
            IzmeniLekaraProzor.Close();
        }

        #endregion

        #region ObrisiLekaraViewModel
        
        public MyICommand ObrisiLekaraKomanda { get; set; }
        public MyICommand PotvrdiBrisanjeLekara { get; set; }
        public MyICommand OdustaniOdBrisanjaLekara { get; set; }

        private void ObrisiLekara()
        {
            if (izabraniLekar != null)
            {
                BrisanjeLekaraProzor = new ObrisiLekara();
                BrisanjeLekaraProzor.Show();
                BrisanjeLekaraProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati lekara!");
            }
        }

        private void ObrisiIzabranogLekara()
        {
            LekariServis.ObrisiLekara(izabraniLekar);
            Lekari.Remove(izabraniLekar);
            BrisanjeLekaraProzor.Close();
        }

        private void ZatvoriBrisanjeLekara()
        {
            BrisanjeLekaraProzor.Close();
        }

        #endregion

    }
}
