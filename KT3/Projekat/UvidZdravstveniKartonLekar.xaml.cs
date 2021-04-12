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
using Projekat.Model;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for UvidZdravstveniKartonLekar.xaml
    /// </summary>
    public partial class UvidZdravstveniKartonLekar : Window
    {

        public Pacijent pacijent;
        public Termin termin;
        public UvidZdravstveniKartonLekar(Pacijent izabraniNalog, Termin termin)
        {
            InitializeComponent();
            this.pacijent = izabraniNalog;
            this.termin = termin;
            if (izabraniNalog != null)
            {
                ime.Text = izabraniNalog.ImePacijenta;
                prezime.Text = izabraniNalog.PrezimePacijenta;
                jmbg.Text = izabraniNalog.Jmbg.ToString();

                if (izabraniNalog.Pol.Equals(pol.M))
                {
                    combo2.SelectedIndex = 0;
                }
                else if (izabraniNalog.Pol.Equals(pol.Z))
                {
                    combo2.SelectedIndex = 1;
                }

                if (izabraniNalog.StatusNaloga.Equals(statusNaloga.Stalni))
                {
                    combo.SelectedIndex = 0;
                }
                else if (izabraniNalog.StatusNaloga.Equals(statusNaloga.Guest))
                {
                    combo.SelectedIndex = 1;
                }

                brojTelefona.Text = izabraniNalog.BrojTelefona.ToString();
                email.Text = izabraniNalog.Email;
                adresa.Text = izabraniNalog.AdresaStanovanja;
                zanimanje.Text = izabraniNalog.Zanimanje;

                if (izabraniNalog.BracnoStanje.Equals(bracnoStanje.Neozenjen) || izabraniNalog.BracnoStanje.Equals(bracnoStanje.Neudata))
                {
                    combo3.SelectedIndex = 0;
                }
                else if (izabraniNalog.BracnoStanje.Equals(bracnoStanje.Ozenjen) || izabraniNalog.BracnoStanje.Equals(bracnoStanje.Udata))
                {
                    combo3.SelectedIndex = 1;
                }
                else if (izabraniNalog.BracnoStanje.Equals(bracnoStanje.Udovac) || izabraniNalog.BracnoStanje.Equals(bracnoStanje.Udovica))
                {
                    combo3.SelectedIndex = 2;
                }
                else if (izabraniNalog.BracnoStanje.Equals(bracnoStanje.Razveden) || izabraniNalog.BracnoStanje.Equals(bracnoStanje.Razvedena))
                {
                    combo3.SelectedIndex = 3;
                }

                ime.IsEnabled = false;
                prezime.IsEnabled = false;
                jmbg.IsEnabled = false;
                combo.IsEnabled = false;
                combo2.IsEnabled = false;
                brojTelefona.IsEnabled = false;
                email.IsEnabled = false;
                adresa.IsEnabled = false;
                combo3.IsEnabled = false;
                zanimanje.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            this.Close();
            PrikazTerminaLekar pt = new PrikazTerminaLekar();
            pt.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TabelaRecepata tp = new TabelaRecepata(pacijent);
            tp.Show();
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           
            PrikazAnamneza pa = new PrikazAnamneza(pacijent,termin);
            pa.Show();
        }



    }
}

