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
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for DodajPacijentaGuest.xaml
    /// </summary>
    public partial class DodajPacijentaGuest : Window
    {
        public ZakaziTerminSekretar zakaziTermin;

        public DodajPacijentaGuest(ZakaziTerminSekretar terminSekretar)
        {
            InitializeComponent();
            this.zakaziTermin = terminSekretar;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            pol pol = PacijentiServis.OdreditiPolPacijenta(polPacijenta.Text);
            Pacijent guestPacijent = new Pacijent(PacijentiServis.GenerisanjeIdPacijenta(), ime.Text, prezime.Text, long.Parse(jmbg.Text), pol, statusNaloga.Guest);
            PacijentiMenadzer.pacijenti.Add(guestPacijent);
            PacijentiServis.SacuvajIzmenePacijenta();

            zakaziTermin.pacijenti.Text = guestPacijent.ImePacijenta + " " + guestPacijent.PrezimePacijenta;
            zakaziTermin.AzurirajListuPacijenata();
            zakaziTermin.Pacijent = guestPacijent;

            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void jmbg_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!PacijentiServis.JedinstvenJmbg(long.Parse(jmbg.Text)))
            {
                MessageBox.Show("JMBG vec postoji");
                jmbg.Text = "";
            }
        }
    }
}
