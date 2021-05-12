using System;
using System.Collections.Generic;
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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for DodajLekara.xaml
    /// </summary>
    public partial class DodajLekara : Window
    {
        public DodajLekara()
        {
            InitializeComponent();
            oblastLekara.ItemsSource = Enum.GetValues(typeof(Specijalizacija)).Cast<Specijalizacija>();
        }

        private void jmbg_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(jmbg.Text.Equals("")))
            {
                if ((!LekariMenadzer.JedinstvenJmbg(Convert.ToInt32(jmbg.Text))))
                {
                    MessageBox.Show("JMBG vec postoji");
                    jmbg.Text = "";
                }
            }
        }

        private void jmbg_TextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO: iskoristi prilikom validacije polja
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            Specijalizacija specijalizacija = (Specijalizacija)oblastLekara.SelectedItem;
            Lekar lekar = new Lekar(LekariMenadzer.GenerisanjeIdLekara(), ime.Text, prezime.Text, Int32.Parse(jmbg.Text), Int32.Parse(brojTelefona.Text), email.Text, adresa.Text, specijalizacija);
            LekariMenadzer.DodajLekara(lekar);
            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
