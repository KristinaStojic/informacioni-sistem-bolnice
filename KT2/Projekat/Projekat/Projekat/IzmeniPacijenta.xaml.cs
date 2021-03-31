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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmeniPacijenta.xaml
    /// </summary>
    public partial class IzmeniPacijenta : Window
    {
        public Pacijent pacijent;
        public IzmeniPacijenta(Pacijent izabraniNalog)
        {
            InitializeComponent();
            this.pacijent = izabraniNalog;
            if (izabraniNalog != null) 
            {
                ime.Text = izabraniNalog.ImePacijenta;
                prezime.Text = izabraniNalog.PrezimePacijenta;
                jmbg.Text = izabraniNalog.Jmbg.ToString();
               
                if (izabraniNalog.StatusNaloga.Equals(statusNaloga.Stalni))
                {
                    combo.SelectedIndex = 0;
                    combo.IsEnabled = false;

                    brojTelefona.IsEnabled = true;
                    email.IsEnabled = true;
                    adresa.IsEnabled = true;
                }
                else if (izabraniNalog.StatusNaloga.Equals(statusNaloga.Guest)) 
                {
                    combo.SelectedIndex = 1;

                    brojTelefona.IsEnabled = false;
                    email.IsEnabled = false;
                    adresa.IsEnabled = false;
                }

                brojTelefona.Text = izabraniNalog.BrojTelefona.ToString();
                email.Text = izabraniNalog.Email;
                adresa.Text = izabraniNalog.AdresaStanovanja;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            statusNaloga status;
            if (combo.SelectedIndex == 0)
            {
                status = statusNaloga.Stalni;
                Pacijent noviPacijent = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, int.Parse(jmbg.Text), long.Parse(brojTelefona.Text), email.Text, adresa.Text, status);
                PacijentiMenadzer.IzmeniNalog(pacijent, noviPacijent);
            }
            else 
            {
                status = statusNaloga.Guest;
                Pacijent noviPacijent1 = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, int.Parse(jmbg.Text), status);
                PacijentiMenadzer.IzmeniNalog(pacijent, noviPacijent1);
            }

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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
