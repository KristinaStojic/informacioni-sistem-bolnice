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
            statusNaloga status = PacijentiServis.OdrediStatusNaloga(combo.Text);
            pol pol = PacijentiServis.OdreditiPolPacijenta(polPacijenta.Text);
            bracnoStanje brStanje = PacijentiServis.OdreditiBracnoStanje(bracnoStanjePacijenta.SelectedIndex, polPacijenta.Text);
            bool maloletnoLice = PacijentiServis.MaloletnoLice((bool)maloletnik.IsChecked);
            long staratelj = PacijentiServis.OdrediJmbgStaratelja(jmbgStaratelja.Text);

            if (status.Equals(statusNaloga.Guest))
            {
                Pacijent guestPacijent = new Pacijent(PacijentiServis.GenerisanjeIdPacijenta(), ime.Text, prezime.Text, long.Parse(jmbg.Text), pol, status);
                PacijentiServis.DodajNalog(guestPacijent);
            }
            else
            {
                Pacijent pacijent = new Pacijent(PacijentiServis.GenerisanjeIdPacijenta(), ime.Text, prezime.Text, long.Parse(jmbg.Text), pol, long.Parse(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje, maloletnoLice, staratelj);
                PacijentiServis.DodajNalog(pacijent);
            }    

            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Combo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (combo.Text.Equals("GUEST")) 
            {
                brojTelefona.IsEnabled = false;
                email.IsEnabled = false;
                adresa.IsEnabled = false;
                zanimanje.IsEnabled = false;
                bracnoStanjePacijenta.IsEnabled = false;
                maloletnik.IsEnabled = false;
                jmbgStaratelja.IsEnabled = false;
            }
            else if (combo.Text.Equals("STALAN"))
            {
                brojTelefona.IsEnabled = true;
                email.IsEnabled = true;
                adresa.IsEnabled = true;
                zanimanje.IsEnabled = true;
                bracnoStanjePacijenta.IsEnabled = true;
                
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

        private void Jmbg_LostFocus(object sender, RoutedEventArgs e)
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

        private void Maloletnik_LostFocus(object sender, RoutedEventArgs e)
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

    }
}

