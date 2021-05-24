using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Model;
using Projekat.Model;
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for DodajPacijenta.xaml
    /// </summary>

    public partial class DodajPacijenta : Window, INotifyPropertyChanged
    {
        bracnoStanje brStanje;
        public DodajPacijenta()
        {
            InitializeComponent();
            this.DataContext = this;
            jmbgStaratelja.IsEnabled = false;
        }

        public string validacijaJmbg;
        public string validacijaJmbgStaratelja;
        public string validacijaBrojTelefona;

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

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            statusNaloga status;
            pol pol;
            bool maloletnoLice;
            long staratelj;

            if (combo.Text.Equals("STALAN"))
            {
                status = statusNaloga.Stalni;
            }
            else
            {
                status = statusNaloga.Guest;
            }

            if (combo2.Text.Equals("M"))
            {
                pol = pol.M;
            }
            else
            {
                pol = pol.Z;
            }

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

            if ((bool)maloletnik.IsChecked)
            {
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

            if (status.Equals(statusNaloga.Guest))
            {
                Pacijent guestPacijent = new Pacijent(PacijentiServis.GenerisanjeIdPacijenta(), ime.Text, prezime.Text, long.Parse(jmbg.Text), pol, status);
                PacijentiServis.DodajNalog(guestPacijent);
            }
            else
            {
                Pacijent pacijent = new Pacijent(PacijentiServis.GenerisanjeIdPacijenta(), ime.Text, prezime.Text, long.Parse(jmbg.Text), pol, long.Parse(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje, maloletnoLice, staratelj);
                ZdravstveniKarton karton = new ZdravstveniKarton(pacijent.IdPacijenta);
                pacijent.Karton = karton;
                List<LekarskiRecept> lr = new List<LekarskiRecept>();
                pacijent.Karton.LekarskiRecepti = lr;
                List<Anamneza> an = new List<Anamneza>();
                pacijent.Karton.Anamneze = an;
                List<Alergeni> ale = new List<Alergeni>();
                pacijent.Karton.Alergeni = ale;
                List<Uput> uput = new List<Uput>();
                pacijent.Karton.Uputi = uput;
                PacijentiServis.DodajNalog(pacijent);
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

        private void jmbg_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(jmbg.Text.Equals("")))
            {
                if ((!PacijentiServis.JedinstvenJmbg(long.Parse(jmbg.Text))))
                {
                    MessageBox.Show("JMBG vec postoji");
                    jmbg.Text = "";
                }
            }
        }

        private void maloletnik_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((bool)maloletnik.IsChecked)
            {
                jmbgStaratelja.IsEnabled = true;
                jmbgStaratelja.Focusable = true; 
            }
            else 
            {
                jmbgStaratelja.IsEnabled = false;
            }
        }

        private void jmbg_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
           /* if (this.jmbg != null)
            {
                if (IsNumeric(this.jmbg.Text))
                {
                    int.Parse(this.jmbg.Text);
                }
            }*/
        }

        private void Button_LostFocus(object sender, RoutedEventArgs e)
        {
            /*if (ime.Text.Equals("") || prezime.Text.Equals("") || jmbg.Text.Equals(""))
            {
                potvrdi.IsEnabled.Equals(false);
            }
            else
            {
                potvrdi.IsEnabled = true;

            }*/
        }
    }
}

