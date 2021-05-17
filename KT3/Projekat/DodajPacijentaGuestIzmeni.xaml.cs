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
    /// Interaction logic for DodajPacijentaGuestIzmeni.xaml
    /// </summary>
    public partial class DodajPacijentaGuestIzmeni : Window
    {
        public IzmeniTerminSekretar z;

        public DodajPacijentaGuestIzmeni(IzmeniTerminSekretar terminSekretar)
        {
            InitializeComponent();
            this.z = terminSekretar;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pol pol;

            if (combo2.Text.Equals("M"))
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

            z.pacijenti.Text = guestPacijent.ImePacijenta + " " + guestPacijent.PrezimePacijenta;
            z.AzurirajListuPacijenata();
            z.Pacijent = guestPacijent;

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
