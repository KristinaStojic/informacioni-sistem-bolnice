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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for DodajPacijentaGuest.xaml
    /// </summary>
    public partial class DodajPacijentaGuest : Window
    {
        public ZakaziTerminSekretar z;

        public DodajPacijentaGuest()
        {
            InitializeComponent();
        }       

        public DodajPacijentaGuest(ZakaziTerminSekretar terminSekretar)
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

            int idP1 = PacijentiMenadzer.GenerisanjeIdPacijenta();
            Pacijent p1 = new Pacijent(idP1, ime.Text, prezime.Text, Convert.ToInt32(jmbg.Text), pol, statusNaloga.Guest);
            PacijentiMenadzer.pacijenti.Add(p1);
            z.AzurirajComboBox();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
