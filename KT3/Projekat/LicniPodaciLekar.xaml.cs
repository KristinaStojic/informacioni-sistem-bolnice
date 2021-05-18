using Model;
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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for LicniPodaciLekar.xaml
    /// </summary>
    public partial class LicniPodaciLekar : Window
    {
        int IDLekara;
        public LicniPodaciLekar(int idLekara)
        {
            InitializeComponent();
            this.IDLekara = idLekara;
            PopuniPodatkeLekara();
        }

        private void PopuniPodatkeLekara()
        {
            foreach (Lekar lekar in LekariMenadzer.lekari)
            {
                if (lekar.IdLekara == IDLekara)
                {
                    this.ime.Text = lekar.ImeLek;
                    this.prezime.Text = lekar.PrezimeLek;
                    this.jmbg.Text = lekar.Jmbg.ToString();
                    this.telefon.Text = lekar.BrojTelefona.ToString();
                    this.email.Text = lekar.Email;
                    this.adresa.Text = lekar.AdresaStanovanja;
                    this.specijalizacija.Text = lekar.specijalizacija.ToString();
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) //Sacuvaj
            {
                Button_Click(sender, e);
            }
            else if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl)) //Nazad
            {
                Button_Click_1(sender, e);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
