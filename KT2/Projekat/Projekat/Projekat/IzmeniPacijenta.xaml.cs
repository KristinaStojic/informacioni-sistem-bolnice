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
                this.ime.Text = izabraniNalog.ImePacijenta;
                this.prezime.Text = izabraniNalog.PrezimePacijenta;
                this.jmbg.Text = izabraniNalog.Jmbg.ToString();
               
                if (izabraniNalog.StatusNaloga.Equals(statusNaloga.Stalni))
                {
                    this.combo.SelectedIndex = 0;
                }
                else if (izabraniNalog.StatusNaloga.Equals(statusNaloga.Guest)) 
                {
                    this.combo.SelectedIndex = 1;
                }

                this.brojTelefona.Text = izabraniNalog.BrojTelefona.ToString();
                this.email.Text = izabraniNalog.Email;
                this.adresa.Text = izabraniNalog.AdresaStanovanja;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            statusNaloga status;
            if (this.combo.SelectedIndex == 0)
            {
                status = statusNaloga.Stalni;
            }
            else 
            {
                status = statusNaloga.Guest;
            }

            Pacijent noviPacijent = new Pacijent(pacijent.IdPacijenta, this.ime.Text, this.prezime.Text, int.Parse(this.jmbg.Text), long.Parse(this.brojTelefona.Text), this.email.Text, this.adresa.Text, status);
            PacijentiMenadzer.IzmeniNalog(pacijent, noviPacijent);
            this.Close();
       
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
