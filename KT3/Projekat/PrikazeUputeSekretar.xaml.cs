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
using Model;
using Projekat.Model;
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikazeUputeSekretar.xaml
    /// </summary>
    public partial class PrikazeUputeSekretar : Window
    {
        public Uput uput;
        public Pacijent pacijent;
        public ZakaziTerminSekretar zakazivanje;
        public static ObservableCollection<Uput> TabelaUputa
        {
            get;
            set;
        }

        public PrikazeUputeSekretar(Pacijent selektovaniPacijent, ZakaziTerminSekretar zakazivanjeTermina)
        {
            InitializeComponent();
            this.pacijent = selektovaniPacijent;
            this.zakazivanje = zakazivanjeTermina;
            this.DataContext = this;

            TabelaUputa = new ObservableCollection<Uput>();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                if (p.IdPacijenta == pacijent.IdPacijenta)
                {
                    foreach (Uput uput in p.Karton.Uputi)
                    {
                        TabelaUputa.Add(uput);
                    }
                }
            }
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Izaberi_uput_Click(object sender, RoutedEventArgs e)
        {
            uput = (Uput)dataGridUputi.SelectedItem;
            this.Close();

            // popunjavanje polja na formi za zakazivanje
            zakazivanje.uputZaPregled = uput;
            Lekar l = LekariServis.NadjiPoId(uput.IdLekaraKodKogSeUpucuje);
            zakazivanje.Lekar = l;
            zakazivanje.lekari.Text = l.ImeLek + " " + l.PrezimeLek;
            zakazivanje.tip.SelectedIndex = 1;
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.TipSale.Equals(tipSale.SalaZaPregled) && !s.Namjena.Equals("Skladiste"))
                {
                    if (!zakazivanje.prostorije.Items.Contains(s.Id))
                    {
                        zakazivanje.prostorije.Items.Add(s.Id);
                    }
                }
            }
        }
    }
}
