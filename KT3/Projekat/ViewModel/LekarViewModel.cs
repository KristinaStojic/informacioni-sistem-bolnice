using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Model;
using Projekat.Servis;

namespace Projekat.ViewModel
{
    public class LekarViewModel: BindableBase
    {
        LekariServis servis = new LekariServis();
        #region Lekari
        public static Window LekariProzor { get; set; }
        public Window DodajLekaraProzor { get; set; }
        public Window IzmeniLekaraProzor { get; set; }
        public Window BrisanjeLekaraProzor { get; set; }

        private ICollectionView lekari;
        public ICollectionView Lekari 
        {
            get { return lekari; } 
            set { lekari = value; OnPropertyChanged("Lekari"); } 
        }

        public MyICommand ZatvoriLekara { get; set; }
        public MyICommand JmbgLostFocus { get; set; }
        
        private string pretragaTextBox;
        public string PretragaTextBox
        {
            get { return pretragaTextBox; }
            set 
            {
                pretragaTextBox = value;

                ICollectionView view = new CollectionViewSource { Source = LekariServis.NadjiSveLekare()}.View;
                view.Filter = null;
                view.Filter = delegate (object item)
                {
                    if (String.IsNullOrEmpty(pretragaTextBox))
                    {
                        return true;
                    }
                    else
                    {
                        return ((item as Lekar).ImeLek.IndexOf(pretragaTextBox, StringComparison.OrdinalIgnoreCase) >= 0)
                            || ((item as Lekar).PrezimeLek.IndexOf(pretragaTextBox, StringComparison.OrdinalIgnoreCase) >= 0)
                               || ((item as Lekar).Jmbg.ToString().IndexOf(pretragaTextBox, StringComparison.OrdinalIgnoreCase) >= 0)
                                  || ((item as Lekar).specijalizacija.ToString().IndexOf(pretragaTextBox, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                };

                Lekari = view;
                OnPropertyChanged("PretragaTextBox"); 
            }
        }

        public LekarViewModel()
        {
            DodajLekaraKomanda = new MyICommand(OtvoriDodavanjeLekara);
            OdustaniOdDodavanjaLekara = new MyICommand(ZatvoriDodavanjeLekara);
            PotvrdiDodavanjeLekara = new MyICommand(DodajNovogLekara);
            ZatvoriLekara = new MyICommand(Zatvori);
            
            IzmeniLekaraKomanda = new MyICommand(OtvoriIzmenuLekara);
            OdustaniOdIzmeneLekara = new MyICommand(ZatvoriIzmenuLekara);
            PotvrdiIzmenuLekara = new MyICommand(IzmeniIzabranogLekara);

            ObrisiLekaraKomanda = new MyICommand(ObrisiLekara);
            PotvrdiBrisanjeLekara = new MyICommand(ObrisiIzabranogLekara);
            OdustaniOdBrisanjaLekara = new MyICommand(ZatvoriBrisanjeLekara);

            DodajLekare();
        }

        private void DodajLekare()
        {
            Lekari = new CollectionViewSource { Source = LekariServis.NadjiSveLekare() }.View;
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

        private ValidacijaNalogaLekara validacija = new ValidacijaNalogaLekara();
        public ValidacijaNalogaLekara Validacija { get { return validacija; } set { validacija = value; OnPropertyChanged("Validacija"); } }

        private void OtvoriDodavanjeLekara()
        {
            Validacija = new ValidacijaNalogaLekara();
            DodajLekaraProzor = new DodajLekara();
            DodajLekaraProzor.Show();
            DodajLekaraProzor.DataContext = this;
        }

        private void DodajNovogLekara()
        {
            Validacija.Validate(); 
            if (Validacija.IsValid)
            {
                Lekar lekar = new Lekar(LekariServis.GenerisanjeIdLekara(), Validacija.ImeLek, Validacija.PrezimeLek, long.Parse(Validacija.Jmbg), long.Parse(Validacija.BrojTelefona), Validacija.Email, Validacija.AdresaStanovanja, Validacija.Specijalizacija);
                LekariServis.DodajLekara(lekar);
                Lekari = new CollectionViewSource { Source = LekariServis.NadjiSveLekare() }.View;
                DodajLekaraProzor.Close();
                Validacija = new ValidacijaNalogaLekara();
            }

        }

        private void ZatvoriDodavanjeLekara()
        {
            Validacija = new ValidacijaNalogaLekara();
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

        private ValidacijaNalogaLekara validacijaIzmena = new ValidacijaNalogaLekara();
        public ValidacijaNalogaLekara ValidacijaIzmena { get { return validacijaIzmena; } set { validacijaIzmena = value; OnPropertyChanged("ValidacijaIzmena"); } }

        private void OtvoriIzmenuLekara()
        {
            if (izabraniLekar != null)
            {
                IzmeniLekaraProzor = new IzmeniLekara();
                IzmeniLekaraProzor.Show();

                validacijaIzmena.ImeLek = izabraniLekar.ImeLek;
                validacijaIzmena.PrezimeLek = izabraniLekar.PrezimeLek;
                validacijaIzmena.Jmbg = izabraniLekar.Jmbg.ToString();
                validacijaIzmena.stariJmbg = izabraniLekar.Jmbg.ToString();
                validacijaIzmena.BrojTelefona = izabraniLekar.BrojTelefona.ToString();
                validacijaIzmena.Email = izabraniLekar.Email;
                validacijaIzmena.AdresaStanovanja = izabraniLekar.AdresaStanovanja;
                validacijaIzmena.Specijalizacija = izabraniLekar.specijalizacija;

                IzmeniLekaraProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati lekara!");
            }
        }

        private void IzmeniIzabranogLekara()
        {
            ValidacijaIzmena.Validate();
            if (ValidacijaIzmena.IsValid)
            {
                Lekar noviLekar = new Lekar(izabraniLekar.IdLekara, validacijaIzmena.ImeLek, validacijaIzmena.PrezimeLek, long.Parse(validacijaIzmena.Jmbg), long.Parse(validacijaIzmena.BrojTelefona), validacijaIzmena.Email, validacijaIzmena.AdresaStanovanja, validacijaIzmena.Specijalizacija);
                LekariServis.IzmeniLekara(izabraniLekar, noviLekar);

                Lekari = new CollectionViewSource { Source = LekariServis.NadjiSveLekare() }.View;
                IzmeniLekaraProzor.Close();
                ValidacijaIzmena = new ValidacijaNalogaLekara();
            }
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
            servis.ObrisiLekara(izabraniLekar);
            Lekari = new CollectionViewSource { Source = LekariServis.NadjiSveLekare() }.View;
            BrisanjeLekaraProzor.Close();
        }

        private void ZatvoriBrisanjeLekara()
        {
            BrisanjeLekaraProzor.Close();
        }

        #endregion
    }
}
