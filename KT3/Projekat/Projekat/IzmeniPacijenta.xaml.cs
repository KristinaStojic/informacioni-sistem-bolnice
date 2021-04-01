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
                    combo.IsEnabled = false;

                    brojTelefona.IsEnabled = true;
                    email.IsEnabled = true;
                    adresa.IsEnabled = true;
                    combo3.IsEnabled = true;
                    zanimanje.IsEnabled = true;

                }
                else if (izabraniNalog.StatusNaloga.Equals(statusNaloga.Guest)) 
                {
                    combo.SelectedIndex = 1;

                    brojTelefona.IsEnabled = false;
                    email.IsEnabled = false;
                    adresa.IsEnabled = false;
                    combo3.IsEnabled = false;
                    zanimanje.IsEnabled = false;
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
                else
                {
                    combo3.SelectedIndex = 4;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            statusNaloga status;
            pol pol;
            bracnoStanje brStanje;

            if (combo2.Text.Equals("M"))
            {
                pol = pol.M;
            }
            else
            {
                pol = pol.Z;
            }

            // za stalan nalog    
            if (combo.SelectedIndex == 0)
            {
                status = statusNaloga.Stalni;

                if (combo3.Text.Equals("Neozenjen/Neudata") && combo2.Text.Equals("Z"))
                {
                    brStanje = bracnoStanje.Neudata;
                    Pacijent noviPacijent = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, int.Parse(jmbg.Text), pol, long.Parse(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje);
                    PacijentiMenadzer.IzmeniNalog(pacijent, noviPacijent);
                }
                else if (combo3.Text.Equals("Ozenjen/Udata") && combo2.Text.Equals("Z"))
                {
                    brStanje = bracnoStanje.Udata;
                    Pacijent noviPacijent = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, int.Parse(jmbg.Text), pol, long.Parse(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje);
                    PacijentiMenadzer.IzmeniNalog(pacijent, noviPacijent);
                }
                else if (combo3.Text.Equals("Udovac/Udovica") && combo2.Text.Equals("Z"))
                {
                    brStanje = bracnoStanje.Udovica;
                    Pacijent noviPacijent = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, int.Parse(jmbg.Text), pol, long.Parse(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje);
                    PacijentiMenadzer.IzmeniNalog(pacijent, noviPacijent);
                }
                else if (combo3.Text.Equals("Razveden/Razvedena") && combo2.Text.Equals("Z"))
                {
                    brStanje = bracnoStanje.Razvedena;
                    Pacijent noviPacijent = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, int.Parse(jmbg.Text), pol, long.Parse(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje);
                    PacijentiMenadzer.IzmeniNalog(pacijent, noviPacijent);
                }

                if (combo3.Text.Equals("Neozenjen/Neudata") && combo2.Text.Equals("M"))
                {
                    brStanje = bracnoStanje.Neozenjen;
                    Pacijent noviPacijent = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, int.Parse(jmbg.Text), pol, long.Parse(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje);
                    PacijentiMenadzer.IzmeniNalog(pacijent, noviPacijent);
                }
                else if (combo3.Text.Equals("Ozenjen/Udata") && combo2.Text.Equals("M"))
                {
                    brStanje = bracnoStanje.Ozenjen;
                    Pacijent noviPacijent = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, int.Parse(jmbg.Text), pol, long.Parse(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje);
                    PacijentiMenadzer.IzmeniNalog(pacijent, noviPacijent);
                }
                else if (combo3.Text.Equals("Udovac/Udovica") && combo2.Text.Equals("M"))
                {
                    brStanje = bracnoStanje.Udovac;
                    Pacijent noviPacijent = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, int.Parse(jmbg.Text), pol, long.Parse(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje);
                    PacijentiMenadzer.IzmeniNalog(pacijent, noviPacijent);
                }
                else if (combo3.Text.Equals("Razveden/Razvedena") && combo2.Text.Equals("M"))
                {
                    brStanje = bracnoStanje.Razveden;
                    Pacijent noviPacijent = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, int.Parse(jmbg.Text), pol, long.Parse(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje);
                    PacijentiMenadzer.IzmeniNalog(pacijent, noviPacijent);
                }
                
            }
            else // za guest nalog
            {
                status = statusNaloga.Guest;      
                Pacijent noviPacijent1 = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, int.Parse(jmbg.Text), pol, status);
                noviPacijent1.BracnoStanje = bracnoStanje.Neodredjeno;
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
                zanimanje.IsEnabled = false;
                combo3.IsEnabled = false;
            }
            else if (combo.Text.Equals("STALAN"))
            {
                brojTelefona.IsEnabled = true;
                email.IsEnabled = true;
                adresa.IsEnabled = true;
                zanimanje.IsEnabled = true;
                combo3.IsEnabled = true;
            }
        }
    }
}
