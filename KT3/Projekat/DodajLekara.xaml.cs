using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for DodajLekara.xaml
    /// </summary>
    public partial class DodajLekara : Window//, INotifyPropertyChanged
    {
        public DodajLekara()
        {
            InitializeComponent();
        //    forma.DataContext = this;
        //    oblastLekara.ItemsSource = Enum.GetValues(typeof(Specijalizacija)).Cast<Specijalizacija>();
        }
        /*
        public string validacijaJmbg;
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

        private void jmbg_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(jmbg.Text.Equals("")))
            {
                if ((!LekariServis.JedinstvenJmbg(long.Parse(jmbg.Text))))
                {
                    MessageBox.Show("JMBG vec postoji");
                    jmbg.Text = "";
                }
            }
        }

        private void jmbg_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            Specijalizacija specijalizacija = (Specijalizacija)oblastLekara.SelectedItem;
            Lekar lekar = new Lekar(LekariServis.GenerisanjeIdLekara(), ime.Text, prezime.Text, long.Parse(jmbg.Text), long.Parse(brojTelefona.Text), email.Text, adresa.Text, specijalizacija);
            List<int> zahtevi = new List<int>();
            lekar.ZahteviZaOdmor = zahtevi;
            LekariServis.DodajLekara(lekar);
            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       */
    }
}
