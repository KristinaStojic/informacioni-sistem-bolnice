using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmeniPacijenta.xaml
    /// </summary>
    public partial class IzmeniPacijenta : Window, INotifyPropertyChanged
    {
        public Pacijent pacijent;
        public bracnoStanje brStanje;

        public IzmeniPacijenta(Pacijent izabraniNalog)
        {
            InitializeComponent();
            this.pacijent = izabraniNalog;
            this.DataContext = this;
            if (izabraniNalog != null) 
            {
                PupuniPoljaForme(izabraniNalog);
            }
        }

        public string validacijaJmbg;
        public string validacijaBrojTelefona;
        public string validacijaJmbgStaratelja;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public string ValidacijaJmbg
        {
            get
            {
                return validacijaJmbg;
            }
            set
            {
                if (value != validacijaJmbg)
                {
                    validacijaJmbg = value;
                    OnPropertyChanged("ValidacijaJmbg");
                }
            }
        }

        public string ValidacijaBrojTelefona
        {
            get
            {
                return validacijaBrojTelefona;
            }
            set
            {
                if (value != validacijaBrojTelefona)
                {
                    validacijaBrojTelefona = value;
                    OnPropertyChanged("ValidacijaBrojTelefona");
                }
            }
        }

        public string ValidacijaJmbgStaratelja
        {
            get
            {
                return validacijaJmbgStaratelja;
            }
            set
            {
                if (value != validacijaJmbgStaratelja)
                {
                    validacijaJmbgStaratelja = value;
                    OnPropertyChanged("ValidacijaJmbgStaratelja");
                }
            }
        }

        private void PupuniPoljaForme(Pacijent izabraniNalog)
        {
            ime.Text = izabraniNalog.ImePacijenta;
            prezime.Text = izabraniNalog.PrezimePacijenta;
            ValidacijaJmbg = izabraniNalog.Jmbg.ToString();
            ValidacijaBrojTelefona = izabraniNalog.BrojTelefona.ToString();
            email.Text = izabraniNalog.Email;
            adresa.Text = izabraniNalog.AdresaStanovanja;
            zanimanje.Text = izabraniNalog.Zanimanje;
            ValidacijaJmbgStaratelja = izabraniNalog.JmbgStaratelja.ToString();

            if (izabraniNalog.Pol.Equals(pol.M))
            {
                combo2.SelectedIndex = 0;
            }
            else if (izabraniNalog.Pol.Equals(pol.Z))
            {
                combo2.SelectedIndex = 1;
            }

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

            if (izabraniNalog.Maloletnik == true)
            {
                maloletnik.IsChecked = true;
            }
            else
            {
                maloletnik.IsChecked = false;
                jmbgStaratelja.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            statusNaloga status;
            pol pol;
            bool maloletnoLice;
            long staratelj;

            if (combo2.Text.Equals("M"))
            {
                pol = pol.M;
            }
            else
            {
                pol = pol.Z;
            }

            if ((bool)maloletnik.IsChecked)
            {
                Console.WriteLine("cekirano");
                maloletnoLice = true;
            }
            else
            {
                maloletnoLice = false;
            }

            if (jmbgStaratelja.Text.Equals(""))
            {
                staratelj = 0;
            }
            else
            {
                staratelj = long.Parse(jmbgStaratelja.Text);
            }

            if (combo.SelectedIndex == 0) // za stalan nalog    
            {
                status = statusNaloga.Stalni;

                if (combo3.Text.Equals("Neozenjen/Neudata") && combo2.Text.Equals("Z"))
                {
                    brStanje = bracnoStanje.Neudata;
                }
                else if (combo3.Text.Equals("Ozenjen/Udata") && combo2.Text.Equals("Z"))
                {
                    brStanje = bracnoStanje.Udata;
                }
                else if (combo3.Text.Equals("Udovac/Udovica") && combo2.Text.Equals("Z"))
                {
                    brStanje = bracnoStanje.Udovica;
                }
                else if (combo3.Text.Equals("Razveden/Razvedena") && combo2.Text.Equals("Z"))
                {
                    brStanje = bracnoStanje.Razvedena;
                }
                else if (combo3.Text.Equals("Neozenjen/Neudata") && combo2.Text.Equals("M"))
                {
                    brStanje = bracnoStanje.Neozenjen;
                }
                else if (combo3.Text.Equals("Ozenjen/Udata") && combo2.Text.Equals("M"))
                {
                    brStanje = bracnoStanje.Ozenjen;
                }
                else if (combo3.Text.Equals("Udovac/Udovica") && combo2.Text.Equals("M"))
                {
                    brStanje = bracnoStanje.Udovac;
                }
                else if (combo3.Text.Equals("Razveden/Razvedena") && combo2.Text.Equals("M"))
                {
                    brStanje = bracnoStanje.Razveden;
                }

                Pacijent noviPacijent = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, long.Parse(jmbg.Text), pol, long.Parse(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje, maloletnoLice, staratelj);
                PacijentiServis.IzmeniNalog(pacijent, noviPacijent);
            }
            else
            {
                status = statusNaloga.Guest;      
                Pacijent guestPacijent = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, long.Parse(jmbg.Text), pol, status);
                guestPacijent.BracnoStanje = bracnoStanje.Neodredjeno;
                PacijentiServis.IzmeniNalog(pacijent, guestPacijent);    
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
                maloletnik.IsEnabled = false;
                jmbgStaratelja.IsEnabled = false;
            }
            else if (combo.Text.Equals("STALAN"))
            {
                brojTelefona.IsEnabled = true;
                email.IsEnabled = true;
                adresa.IsEnabled = true;
                zanimanje.IsEnabled = true;
                combo3.IsEnabled = true;
                maloletnik.IsEnabled = true;
                jmbgStaratelja.IsEnabled = true;
            }
        }

        private void maloletnik_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((bool)maloletnik.IsChecked)
            {
                jmbgStaratelja.IsEnabled = true;
            }
            else
            {
                jmbgStaratelja.IsEnabled = false;
            }
        }
    }
}
