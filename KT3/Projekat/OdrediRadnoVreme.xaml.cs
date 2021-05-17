using System;
using System.Collections.Generic;
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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for OdrediRadnoVreme.xaml
    /// </summary>
    public partial class OdrediRadnoVreme : Window
    {
        public const int BROJ_NEDELJA_ZA_TRI_MESECA = 12;
        public Lekar lekar;
        public OdrediRadnoVreme(Lekar selektovaniLekar)
        {
            InitializeComponent();
            this.lekar = selektovaniLekar;

            kalendar.DisplayDateStart = DateTime.Now;
            kalendar.DisplayDateEnd = DateTime.Now.AddDays(7);
        }

        private string KonvertujDatum(DateTime datum)
        {
            return datum.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            List<RadniDan> radniDani = NapraviListuRadnogVremena();

            foreach (Lekar l in LekariMenadzer.lekari)
            {
                if (l.IdLekara == lekar.IdLekara)
                {
                    l.RadniDani = radniDani;
                    LekariMenadzer.SacuvajIzmeneLekara();
                }
            }

            this.Close();
        }

        private List<RadniDan> NapraviListuRadnogVremena()
        {
            List<RadniDan> radniDani = new List<RadniDan>();
            string vremePocetka = pocetak.Text;
            string vremeKraja = kraj.Text;

            for (int i = 0; i < BROJ_NEDELJA_ZA_TRI_MESECA; i++)
            {
                foreach (DateTime datum in kalendar.SelectedDates)
                {
                    DateTime noviDatum = datum.AddDays(7 * i);
                    RadniDan noviDan = new RadniDan(lekar.IdLekara, KonvertujDatum(noviDatum), vremePocetka, vremeKraja);
                    radniDani.Add(noviDan);
                }
            }

            return radniDani;
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
