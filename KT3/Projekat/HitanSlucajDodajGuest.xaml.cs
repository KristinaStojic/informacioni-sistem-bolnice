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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pol pol;

            if (polPacijenta.Text.Equals("M"))
            {
                pol = pol.M;
            }
            else
            {
                pol = pol.Z;
            }

            Pacijent guestPacijent = new Pacijent(PacijentiServis.GenerisanjeIdPacijenta(), ime.Text, prezime.Text, Convert.ToInt32(jmbg.Text), pol, statusNaloga.Guest);
            PacijentiMenadzer.pacijenti.Add(guestPacijent);
            PacijentiServis.SacuvajIzmenePacijenta();

            hitanSlucaj.pacijenti.Text = guestPacijent.ImePacijenta + " " + guestPacijent.PrezimePacijenta;
            hitanSlucaj.AzurirajListuPacijenata();
            hitanSlucaj.Pacijent = guestPacijent;

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void jmbg_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!PacijentiServis.JedinstvenJmbg(Convert.ToInt32(jmbg.Text)))
            {
                MessageBox.Show("JMBG vec postoji");
                jmbg.Text = "";
            }
        }
    }
}
