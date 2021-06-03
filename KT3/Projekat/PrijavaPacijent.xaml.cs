using Projekat.Servis;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrijavaPacijent.xaml
    /// </summary>
    public partial class PrijavaPacijent : Page
    {
        public PrijavaPacijent()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void prijava_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int IdPacijent = -1;
                string korisnicko = korisnickoIme.Text;
                string lozinka = lozinkaPassword.Password;
                /*if(!ValidacijaNedostajucihPodataka(korisnicko, lozinka))
                {
                    return;
                }*/
                IdPacijent = ValidacijaUnetihPodataka(IdPacijent, korisnicko, lozinka);
                if (IdPacijent == -1)
                {
                    return;
                }
                Page pocetna = new PrikaziTermin(IdPacijent);
                this.NavigationService.Navigate(pocetna);
            }
            catch
            {
                if (Jezik.Header.Equals("_en-US"))
                {
                    MessageBox.Show("Niste uneli ispravno korisnicko ime i/ili lozinku");
                    return;
                }
                else
                {
                    MessageBox.Show("You did not enter a valid username and / or password");
                    return;
                }
            }
        }

        private int ValidacijaUnetihPodataka(int IdPacijent, string korisnicko, string lozinka)
        {
            try
            {
                if (korisnicko.Equals("konstantin") && lozinka.Equals("konstantin"))
                {
                    IdPacijent = 1;
                }
                if (korisnicko.Equals("marko") && lozinka.Equals("marko"))
                {
                    IdPacijent = 3;
                }
                if (korisnicko.Equals("dimitrije") && lozinka.Equals("dimitrije"))
                {
                    IdPacijent = 2;
                }
                if (korisnicko.Equals("kristina") && lozinka.Equals("kristina"))
                {
                    IdPacijent = 4;
                }
                if (korisnicko.Equals("nevena") && lozinka.Equals("nevena"))
                {
                    IdPacijent = 6;
                }
                if (korisnicko.Equals("jovana") && lozinka.Equals("jovana"))
                {
                    IdPacijent = 5;
                }
                if (IdPacijent == -1)
                {
                    if (Jezik.Header.Equals("_en-US"))
                    {
                        MessageBox.Show("Niste uneli ispravno korisnicko ime i/ili lozinku");
                        return -1;
                    }
                    else
                    {
                        MessageBox.Show("You did not enter a valid username and / or password");
                        return -1;
                    }
                }
                return IdPacijent;
            }
            catch
            {
                MessageBox.Show("Niste uneli ispravno korisnicko ime i/ili lozinku");
                return -1;
            }
        }

        private bool ValidacijaNedostajucihPodataka(string korisnicko, string lozinka)
        {
            if (korisnicko == "" || lozinka == "")
            {
                if (Jezik.Header.Equals("_en-US"))
                {
                    MessageBox.Show("Niste uneli korisnicko ime i/ili lozinku");
                }
                else
                {
                    MessageBox.Show("You did not enter a username and / or password");
                }
                return false;
            }
            return true;
        }

        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.PromeniTemu(SvetlaTema, tamnaTema);
        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Jezik_Click(Jezik);
        }


    }
}
