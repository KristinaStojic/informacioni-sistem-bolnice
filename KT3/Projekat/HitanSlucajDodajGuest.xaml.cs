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
    /// Interaction logic for HitanSlucajDodajGuest.xaml
    /// </summary>
    public partial class HitanSlucajDodajGuest : Window
    {
        HitanSlucaj hitanSlucaj;
        public HitanSlucajDodajGuest(HitanSlucaj hitanSlucaj)
        {
            InitializeComponent();
            this.hitanSlucaj = hitanSlucaj;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            pol pol = PacijentiServis.OdreditiPolPacijenta(polPacijenta.Text);
            Pacijent guestPacijent = new Pacijent(PacijentiServis.GenerisanjeIdPacijenta(), ime.Text, prezime.Text, long.Parse(jmbg.Text), pol, statusNaloga.Guest);
            PacijentiMenadzer.pacijenti.Add(guestPacijent);
            PacijentiServis.SacuvajIzmenePacijenta();

            hitanSlucaj.pacijenti.Text = guestPacijent.ImePacijenta + " " + guestPacijent.PrezimePacijenta;
            hitanSlucaj.AzurirajListuPacijenata();
            hitanSlucaj.Pacijent = guestPacijent;

            this.Close();
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Jmbg_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!PacijentiServis.JedinstvenJmbg(long.Parse(jmbg.Text)))
            {
                MessageBox.Show("JMBG vec postoji");
                jmbg.Text = "";
            }
        }
    }
}
