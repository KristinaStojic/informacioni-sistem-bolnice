using Model;
using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for OdobravanjeGodisnjegOdmora.xaml
    /// </summary>
    public partial class OdobravanjeGodisnjegOdmora : Window
    {
        public static ObservableCollection<ZahtevZaGodisnji> TabelaZahteva
        {
            get;
            set;
        }

        public static ObservableCollection<ZahtevZaGodisnji> TabelaProcesiranihZahteva
        {
            get;
            set;
        }

        public OdobravanjeGodisnjegOdmora()
        {
            InitializeComponent();
            this.DataContext = this;
            DodajZahteveUTabelu();
        }

        private void DodajZahteveUTabelu()
        {
            TabelaZahteva = new ObservableCollection<ZahtevZaGodisnji>();
            TabelaProcesiranihZahteva = new ObservableCollection<ZahtevZaGodisnji>();

            foreach (ZahtevZaGodisnji zahtev in LekariMenadzer.zahtevi)
            {
                if (zahtev.odobren.Equals(StatusZahteva.NA_CEKANJU))
                {
                    TabelaZahteva.Add(zahtev);
                }
                else
                {
                    TabelaProcesiranihZahteva.Add(zahtev);
                }
            }
        }
        private void Odobri_Click(object sender, RoutedEventArgs e)
        {
            ZahtevZaGodisnji izabraniZahtev = (ZahtevZaGodisnji)TabelaLekara.SelectedItem;

            if (izabraniZahtev == null)
            {
                MessageBox.Show("Izaberite zahtev koji zelite da odobrite.");
            }
            else
            {
                OdobriZahtevZaGodisnji(izabraniZahtev);
            }
        }

        private void OdobriZahtevZaGodisnji(ZahtevZaGodisnji izabraniZahtev)
        {
            foreach (ZahtevZaGodisnji zahtev in LekariMenadzer.zahtevi)
            {
                if (zahtev.idZahteva == izabraniZahtev.idZahteva)
                {
                    zahtev.odobren = StatusZahteva.ODOBREN;
                    PremestiZahtevNakonProcesiranja(zahtev);
                    OduzmiSlobodneDaneLekaru(zahtev);
                    LekariServis.sacuvajIzmjeneZahteva();
                }
            }
        }

        private void OduzmiSlobodneDaneLekaru(ZahtevZaGodisnji zahtev) 
        {
            foreach (Lekar l in LekariMenadzer.lekari)
            {
                if (l.IdLekara == zahtev.lekar.IdLekara)
                {
                    l.SlobodniDaniGodisnjegOdmora -= zahtev.brojDanaOdmora;
                    LekariServis.SacuvajIzmeneLekara();
                }
            }
        }

        private void Odbij_Click(object sender, RoutedEventArgs e)
        {
            ZahtevZaGodisnji izabraniZahtev = (ZahtevZaGodisnji)TabelaLekara.SelectedItem;

            if (izabraniZahtev == null)
            {
                MessageBox.Show("Izaberite zahtev koji zelite da odbijete.");
            }
            else
            {
                OdbijZahtevZaGodisnji(izabraniZahtev);
            }
        }

        private void OdbijZahtevZaGodisnji(ZahtevZaGodisnji izabraniZahtev)
        {
            foreach (ZahtevZaGodisnji zahtev in LekariMenadzer.zahtevi)
            {
                if (zahtev.idZahteva == izabraniZahtev.idZahteva)
                {
                    zahtev.odobren = StatusZahteva.ODBIJEN;
                    PremestiZahtevNakonProcesiranja(zahtev);
                    LekariServis.sacuvajIzmjeneZahteva();
                }
            }
        }

        private void PremestiZahtevNakonProcesiranja(ZahtevZaGodisnji zahtev)
        {
            TabelaZahteva.Remove(zahtev);
            LekariServis.sacuvajIzmjeneZahteva();
            TabelaProcesiranihZahteva.Add(zahtev);
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
