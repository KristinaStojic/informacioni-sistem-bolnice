using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Projekat.ViewModel
{
    public class UpravnikViewModel : BindableBase
    {
        #region UpravnikViewModel
        private ObservableCollection<Obavestenja> obavestenja;
        private Obavestenja izabranoObavjestenje;
        private string obavjestenje;
        public static Window UpravnikProzor { get; set; }
        public string Obavjestenje { get { return obavjestenje; } set { obavjestenje = value; OnPropertyChanged("Obavjestenje"); } }
        public Obavestenja IzabranoObavjestenje { get { return izabranoObavjestenje; } set { izabranoObavjestenje = value; OnPropertyChanged("IzabranoObavjestenj"); } }
        public ObservableCollection<Obavestenja> Obavestenja { get { return obavestenja; } set { obavestenja = value; OnPropertyChanged("Obavestenja"); } }
        public UpravnikViewModel()
        {
            OdjavaKomanda = new MyICommand(ZatvoriAplikaciju);
            ProstorijeProzor = new MyICommand(OtvoriProstorije);
            ZahtjeviProzor = new MyICommand(OtvoriZahtjeve);
            KomunikacijProzor = new MyICommand(OtvoriKomunikaciju);
            PrikazObavjestenja = new MyICommand(ObavjestenjeDetaljnije);
            ZatvoriObavjestenje = new MyICommand(ZatvoriObavjestenja);
            dodajObavjestenja();
        }
        public Window ObavjestenjeProzor { get; set; }
        private void ZatvoriObavjestenja()
        {
            ObavjestenjeProzor.Close();
        }
        private void ObavjestenjeDetaljnije()
        {
            if(izabranoObavjestenje != null)
            {
                ObavjestenjeProzor = new PrikazObavjestenja();
                ObavjestenjeProzor.Show();
                Obavjestenje = izabranoObavjestenje.SadrzajObavestenja;
                ObavjestenjeProzor.DataContext = this;
            }
            else
            {
                MessageBox.Show("Morate izabrati obavjestenje!");
            }
        }

        private void dodajObavjestenja()
        {
            Obavestenja = new ObservableCollection<Obavestenja>();
            foreach (Obavestenja obavjestenja in ObavestenjaMenadzer.obavestenja)
            {
                if (obavjestenja.Oznaka.Equals("svi") || obavjestenja.Oznaka.Equals("upravnici"))
                {
                    Obavestenja.Add(obavjestenja);
                }
            }
        }
        public MyICommand OdjavaKomanda { get; set; }
        public MyICommand PrikazObavjestenja { get; set; }
        public MyICommand ProstorijeProzor { get; set; }
        public MyICommand ZahtjeviProzor { get; set; }
        public MyICommand KomunikacijProzor { get; set; }
        public MyICommand ZatvoriObavjestenje { get; set; }

        private void OtvoriKomunikaciju()
        {
            KomunikacijaViewModel.KomunikacijaProzor = new Komunikacija();
            KomunikacijaViewModel.KomunikacijaProzor.Show();
            KomunikacijaViewModel.KomunikacijaProzor.DataContext = new KomunikacijaViewModel();
            UpravnikProzor.Close();
        }
        private void OtvoriZahtjeve()
        {
            ZahtjeviViewModel.ZahtjeviProzor = new Zahtjevi();
            ZahtjeviViewModel.ZahtjeviProzor.Show();
            ZahtjeviViewModel.ZahtjeviProzor.DataContext = new ZahtjeviViewModel();
            UpravnikProzor.Close();
        }
        private void OtvoriProstorije()
        {
            SaleViewModel.SaleProzor = new PrikaziSalu();
            SaleViewModel.SaleProzor.Show();
            SaleViewModel.SaleProzor.DataContext = new SaleViewModel();
            UpravnikProzor.Close();
        }
        private void ZatvoriAplikaciju()
        {
            System.Windows.Application.Current.Shutdown();
        }
        #endregion
    }
}
