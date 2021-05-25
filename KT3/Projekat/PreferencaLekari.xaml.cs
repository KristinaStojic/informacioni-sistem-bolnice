using Model;
using Projekat.Model;
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
    /// Interaction logic for PreferencaLekari.xaml
    /// </summary>
    public partial class PreferencaLekari : Page
    {
        private static int idPacijent;
        private static Pacijent prijavljeniPacijent;
        public PreferencaLekari(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            this.datagridLekari.ItemsSource = LekariMenadzer.PronadjiLekarePoSpecijalizaciji(Specijalizacija.Opsta_praksa);
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(datagridLekari.ItemsSource);
            view.Filter = UserFilter;
            idPacijent = idPrijavljenogPacijenta;
            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PacijentPagesServis.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
        }

        

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as Lekar).PrezimeLek.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0) || ((item as Lekar).ImeLek.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Lekar).specijalizacija.ToString().IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0); ;
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(datagridLekari.ItemsSource).Refresh();
        }

        private void datagridLekari_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zakaziLekar.IsEnabled = true;
            if (datagridLekari.SelectedItems.Count > 0)
            {
                Lekar item = (Lekar)datagridLekari.SelectedItem;
            }
        }

        private void zakaziLekar_Click(object sender, RoutedEventArgs e)
        {
            Lekar lekar = null;
            if (datagridLekari.SelectedItems.Count > 0)
            {
                lekar = (Lekar)datagridLekari.SelectedItems[0];
                ZakaziTermin.izabraniLekar = lekar;
                Page zt = new ZakaziTermin(idPacijent);
                this.NavigationService.Navigate(zt);
            }
            else
            {
                MessageBox.Show("Selektujte lekara");
            }
            
        }

        private void odjava_Click(object sender, RoutedEventArgs e)
        {
            /*Page odjava = new PrijavaPacijent();
            this.NavigationService.Navigate(odjava);*/
            PacijentPagesServis.odjava_Click(this);
        }

        public void karton_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.karton_Click(this, idPacijent);
        }

        public void zakazi_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.zakazi_Click(this, idPacijent);
        }
        public void uvid_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.uvid_Click(this, idPacijent);
        }

        private void pocetna_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.pocetna_Click(this, idPacijent);
        }

        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.anketa_Click(this, idPacijent);
        }
        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.PromeniTemu(SvetlaTema, tamnaTema);
        }

        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.Korisnik_Click(this, idPacijent);
        }

        private void nazad_Click(object sender, RoutedEventArgs e)
        {
            Page zakazivanje = new ZakaziTermin(idPacijent);
            this.NavigationService.Navigate(zakazivanje);
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
