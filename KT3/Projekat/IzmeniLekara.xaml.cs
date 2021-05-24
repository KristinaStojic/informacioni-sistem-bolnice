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
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmeniLekara.xaml
    /// </summary>
    public partial class IzmeniLekara : Window //, INotifyPropertyChanged
    {
       /* public Lekar lekar;
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
       */
        public IzmeniLekara(/*Lekar izbraniLekar*/)
        {
            InitializeComponent();
           /* this.lekar = izbraniLekar;
            this.DataContext = this;
            oblastLekara.ItemsSource = Enum.GetValues(typeof(Specijalizacija)).Cast<Specijalizacija>();

            if (lekar != null)
            {
                ime.Text = izbraniLekar.ImeLek;
                prezime.Text = izbraniLekar.PrezimeLek;
                ValidacijaJmbg = izbraniLekar.Jmbg.ToString();
                ValidacijaBrojTelefona = izbraniLekar.BrojTelefona.ToString();
                email.Text = izbraniLekar.Email;
                adresa.Text = izbraniLekar.AdresaStanovanja;
                oblastLekara.SelectedItem = izbraniLekar.specijalizacija;   
            }*/
        }

      /*  private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            Specijalizacija specijalizacija = (Specijalizacija)oblastLekara.SelectedItem;
            Lekar izmenjeniLekar = new Lekar(lekar.IdLekara, ime.Text, prezime.Text, long.Parse(jmbg.Text), long.Parse(brojTelefona.Text), email.Text, adresa.Text, specijalizacija);
            LekariServis.IzmeniLekara(lekar, izmenjeniLekar);
            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void jmbg_LostFocus(object sender, RoutedEventArgs e)
        {
            // TODO: jedinstven jmbg (isto fali i kod izmene pacijenta)
        }

        private void jmbg_TextChanged(object sender, TextChangedEventArgs e)
        {
        }*/
    }
}
