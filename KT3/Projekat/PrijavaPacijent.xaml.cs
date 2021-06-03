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
            int IdPacijent = -1;
            string korisnicko = korisnickoIme.Text;
            string lozinka = lozinkaPassword.Password;
            try
            {
                if(korisnicko.Equals("konstantin") && lozinka.Equals("konstantin"))
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
                        return;
                    }
                    else
                    {
                        MessageBox.Show("You did not enter a valid username and / or password");
                        return;
                    }
                }
                Page pocetna = new PrikaziTermin(IdPacijent);
                this.NavigationService.Navigate(pocetna);
            }
            catch
            {
                MessageBox.Show("Niste uneli ispravno korisnicko ime i/ili lozinku");
            }
        }

        // TODO: izmeniti
        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            /*var app = (App)Application.Current;
            MenuItem mi = (MenuItem)sender;
            if (mi.Header.Equals("Svetla"))
            {
                mi.Header = "Tamna";
                app.ChangeTheme(new Uri("Teme/Svetla.xaml", UriKind.Relative));
            }
            else
            {
                mi.Header = "Svetla";
                app.ChangeTheme(new Uri("Teme/Tamna.xaml", UriKind.Relative));
            }*/
            PacijentWebStranice.PromeniTemu(SvetlaTema, tamnaTema);


        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            /*var app = (App)Application.Current;
            // TODO: proveriti
            string eng = "en-US";
            string srb = "sr-LATN";
            MenuItem mi = (MenuItem)sender;
            if (mi.Header.Equals("en-US"))
            {
                mi.Header = "sr-LATN";
                app.ChangeLanguage(eng);
            }
            else
            {
                mi.Header = "en-US";
                app.ChangeLanguage(srb);
            }*/
            PacijentWebStranice.Jezik_Click(Jezik);

        }


    }
}
