using Model;
using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Projekat.ViewModel
{
    class LekarLekoviViewModel : BindableBase
    {
        private ObservableCollection<ZahtevZaLekove> tabelaZahteva;
        public ObservableCollection<ZahtevZaLekove> TabelaZahteva { get { return tabelaZahteva; } set { tabelaZahteva = value; OnPropertyChanged("TabelaZahteva"); } }

        private ObservableCollection<Lek> tabelaLekova;
        public ObservableCollection<Lek> TabelaLekova { get { return tabelaLekova; } set { tabelaLekova = value; OnPropertyChanged("TabelaLekova"); } }


        private ZahtevZaLekove izabraniZahtev;
        public ZahtevZaLekove IzabraniZahtev { get { return izabraniZahtev; } set { izabraniZahtev = value; OnPropertyChanged("IzabraniZahtev"); } }
        
        private Lek izabraniLek;
        public Lek IzabraniLek { get { return izabraniLek; } set { izabraniLek = value; OnPropertyChanged("IzabraniLek"); } }


        public MyICommand ObradiZahtevKomanda { get; set; }
        public MyICommand ObrisiZahtevKomanda { get; set; }
        public MyICommand NazadIzZahtevaKomanda { get; set; }
        public static Window OtvoriObraduZahteva { get; set; }
        public static Window OtvoriBrisanjeZahteva { get; set; }

        public LekarLekoviViewModel()
        {
            DodajZahteve();
            DodajLekove();
            ObradiZahtevKomanda = new MyICommand(ObradiZahtev);
            ObrisiZahtevKomanda = new MyICommand(ObrisiZahtev);
            NazadIzZahtevaKomanda = new MyICommand(NazadIzZahteva);
        }

        private void ObradiZahtev()
        {
            if (izabraniZahtev == null)
            {
                MessageBox.Show("Niste selektovali zahtev koji zelite da obradite!");
            }
            else if (izabraniZahtev.obradjenZahtev == true)
            {
                MessageBox.Show("Izabrani zahtev je vec obradjen!");
            }
            else if (izabraniZahtev != null && izabraniZahtev.obradjenZahtev == false)
            {
                OtvoriObraduZahteva = new ObradiZahtevZaLek(IzabraniZahtev);
                OtvoriObraduZahteva.Show();
                OtvoriObraduZahteva.DataContext = this;
            }


        }

        private void ObrisiZahtev()
        {
            if (izabraniZahtev == null)
            {
                MessageBox.Show("Niste selektovali zahtev koji zelite da obradite!");
            }
            else if (izabraniZahtev != null)
            {
                OtvoriBrisanjeZahteva = new ObrisiZahtevLekar(IzabraniZahtev);
                OtvoriBrisanjeZahteva.Show();
                OtvoriBrisanjeZahteva.DataContext = this;
            }
        }

        private void NazadIzZahteva()
        {

        }

        private void DodajZahteve()
        {
            TabelaZahteva = new ObservableCollection<ZahtevZaLekove>();
            foreach (ZahtevZaLekove zahtev in LekoviMenadzer.zahteviZaLekove)
            {
                TabelaZahteva.Add(zahtev);
            }
        }

        private void DodajLekove()
        {
            TabelaLekova = new ObservableCollection<Lek>();
            foreach (Lek lek in LekoviServis.Lijekovi())
            {
                TabelaLekova.Add(lek);
            }
        }
    }
}
