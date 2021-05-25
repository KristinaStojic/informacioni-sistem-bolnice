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
            // TODO: moze ovde validacija
            try
            {
                int IdPacijent = Int32.Parse(this.korisnickoIme.Text);
                Page pocetna = new PrikaziTermin(IdPacijent);
                this.NavigationService.Navigate(pocetna);
            }
            catch
            {
                MessageBox.Show("Niste uneli ispravne kredencijale");
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
            PacijentPagesServis.PromeniTemu(SvetlaTema, tamnaTema);


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
            PacijentPagesServis.Jezik_Click(Jezik);

        }


    }
}
