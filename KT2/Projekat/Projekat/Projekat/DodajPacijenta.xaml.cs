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
        int id = 0; // idPacijenta 

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

            Pacijent p = new Pacijent(++id, ime.Text, prezime.Text, Convert.ToInt32(jmbg.Text), Convert.ToInt64(brojTelefona.Text), email.Text, adresa.Text, status);
            PacijentiFileManager.DodajNalog(p);

            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}

