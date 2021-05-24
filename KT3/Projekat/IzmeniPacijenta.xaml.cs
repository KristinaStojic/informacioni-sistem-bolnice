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

        public IzmeniPacijenta(Pacijent izabraniNalog)
        {
            InitializeComponent();
            this.pacijent = izabraniNalog;
            this.DataContext = this;

            if (izabraniNalog != null) 
            {
                PopuniPoljaForme(izabraniNalog);
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

        private void PopuniPoljaForme(Pacijent izabraniNalog)
        {
            ime.Text = izabraniNalog.ImePacijenta;
            prezime.Text = izabraniNalog.PrezimePacijenta;
            ValidacijaJmbg = izabraniNalog.Jmbg.ToString();
            ValidacijaBrojTelefona = izabraniNalog.BrojTelefona.ToString();
            email.Text = izabraniNalog.Email;
            adresa.Text = izabraniNalog.AdresaStanovanja;
            zanimanje.Text = izabraniNalog.Zanimanje;
            ValidacijaJmbgStaratelja = izabraniNalog.JmbgStaratelja.ToString();
            polPacijenta.SelectedIndex = PacijentiServis.UcitajIndeksPola(izabraniNalog);
            bracnoStanjePacijenta.SelectedIndex = PacijentiServis.UcitajIndeksBracnogStanja(izabraniNalog);

            OnemoguciPoljaZaGuestNalog(izabraniNalog);
            OmoguciPoljaZaMaloletnika(izabraniNalog);
        }

        public void OmoguciPoljaZaMaloletnika(Pacijent izabraniNalog)
        {
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

        public void OnemoguciPoljaZaGuestNalog(Pacijent izabraniNalog)
        {
            if (izabraniNalog.StatusNaloga.Equals(statusNaloga.Stalni))
            {
                statusPacijenta.SelectedIndex = 0;
                statusPacijenta.IsEnabled = false;
                brojTelefona.IsEnabled = true;
                email.IsEnabled = true;
                adresa.IsEnabled = true;
                bracnoStanjePacijenta.IsEnabled = true;
                zanimanje.IsEnabled = true;
            }
            else if (izabraniNalog.StatusNaloga.Equals(statusNaloga.Guest))
            {
                statusPacijenta.SelectedIndex = 1;
                statusPacijenta.IsEnabled = true;
                brojTelefona.IsEnabled = false;
                email.IsEnabled = false;
                adresa.IsEnabled = false;
                bracnoStanjePacijenta.IsEnabled = false;
                zanimanje.IsEnabled = false;
            }
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            statusNaloga status = PacijentiServis.OdrediStatusNaloga(statusPacijenta.Text);
            pol pol = PacijentiServis.OdreditiPolPacijenta(polPacijenta.Text);
            bracnoStanje brStanje = PacijentiServis.OdreditiBracnoStanje(bracnoStanjePacijenta.SelectedIndex, polPacijenta.Text);
            bool maloletnoLice = PacijentiServis.MaloletnoLice((bool)maloletnik.IsChecked);
            long staratelj = PacijentiServis.OdrediJmbgStaratelja(jmbgStaratelja.Text);

            if (statusPacijenta.SelectedIndex == 0) // stalan nalog
            {
                Pacijent noviPacijent = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, long.Parse(jmbg.Text), pol, long.Parse(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje, maloletnoLice, staratelj);
                PacijentiServis.IzmeniNalog(pacijent, noviPacijent);
            }
            else
            {
                Pacijent guestPacijent = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, long.Parse(jmbg.Text), pol, status);
                PacijentiServis.IzmeniNalog(pacijent, guestPacijent);    
            }

            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Combo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (statusPacijenta.Text.Equals("GUEST"))
            {
                brojTelefona.IsEnabled = false;
                email.IsEnabled = false;
                adresa.IsEnabled = false; 
                zanimanje.IsEnabled = false;
                bracnoStanjePacijenta.IsEnabled = false;
                maloletnik.IsEnabled = false;
                jmbgStaratelja.IsEnabled = false;
            }
            else if (statusPacijenta.Text.Equals("STALAN"))
            {
                brojTelefona.IsEnabled = true;
                email.IsEnabled = true;
                adresa.IsEnabled = true;
                zanimanje.IsEnabled = true;
                bracnoStanjePacijenta.IsEnabled = true;
                maloletnik.IsEnabled = true;
            }
        }

        private void Maloletnik_LostFocus(object sender, RoutedEventArgs e)
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
