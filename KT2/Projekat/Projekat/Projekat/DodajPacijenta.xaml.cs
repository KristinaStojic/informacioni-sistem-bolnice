using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Model;

namespace Projekat.Model
{
    /// <summary>
    /// Interaction logic for DodajPacijenta.xaml
    /// </summary>
  
    public partial class DodajPacijenta : Window
    {
        public DodajPacijenta()
        {
            InitializeComponent();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            statusNaloga status;
            if (combo.Text.Equals("STALAN"))
            {
                status = statusNaloga.Stalni;
            }
            else
            {
                status = statusNaloga.Guest;
            }

            if (brojTelefona.Text.Equals("") || adresa.Text.Equals("") || email.Text.Equals(""))
            {
                int idP1 = PacijentiMenadzer.GenerisanjeIdPacijenta();
                Pacijent p1 = new Pacijent(idP1, ime.Text, prezime.Text, Convert.ToInt32(jmbg.Text), status);
                PacijentiMenadzer.DodajNalog(p1);
            }
            else
            {
                int idP = PacijentiMenadzer.GenerisanjeIdPacijenta();
                Pacijent p = new Pacijent(idP, ime.Text, prezime.Text, Convert.ToInt32(jmbg.Text), Convert.ToInt64(brojTelefona.Text), email.Text, adresa.Text, status);
                PacijentiMenadzer.DodajNalog(p);
                // kreiraj zdravstveni karton za datog pacijenta
                // napraviti klasu zdravstveni karton
            }

            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void combo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (combo.Text.Equals("GUEST")) 
            {
                brojTelefona.IsEnabled = false;
                email.IsEnabled = false;
                adresa.IsEnabled = false;
            }
            else if (combo.Text.Equals("STALAN"))
            {
                brojTelefona.IsEnabled = true;
                email.IsEnabled = true;
                adresa.IsEnabled = true;
            }
        }
    }
}

