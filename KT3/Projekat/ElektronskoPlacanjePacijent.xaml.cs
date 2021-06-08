using Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class ElektronskoPlacanjePacijent : Page
    {
        private static int idPacijent;
        private static bool prvo = false;
        private static bool drugo = false;
        private static bool trece = false;
        private static bool cetvrto = false;
        private static bool peto = false;
        private static bool sesto = false;
        private static bool sedmo = false;
        private static bool osmo = false;
        private static bool deveto = false;
        private static bool deseto = false;
        public ElektronskoPlacanjePacijent(int idPrijavaljenogPacijenta, TipTermina tip)
        {
            InitializeComponent();
            idPacijent = idPrijavaljenogPacijenta;
            OdrediCenuPregleda(tip);

            Pacijent prijavljeniPacijent = PacijentiServis.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
            this.potvrdi.IsEnabled = false;
            MessageBox.Show(drzava.SelectedItem.ToString());
        }

        private void OdrediCenuPregleda(TipTermina tip)
        {
            if (tip.Equals(TipTermina.Pregled))
            {
                if (Jezik.Header.Equals("_sr-LATN"))
                {
                    cena.Text = "10 €";
                }
                else
                {
                    cena.Text = "1170 DIN";
                }
            }
            else
            {
                if (Jezik.Header.Equals("_sr-LATN"))
                {
                    cena.Text = "50 €";
                }
                else
                {
                    cena.Text = "6000 DIN";
                }
            }
        }

        private void odjava_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.odjava_Click(this);
        }

        public void karton_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.karton_Click(this, idPacijent);
        }

        public void zakazi_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.zakazi_Click(this, idPacijent);
        }

        public void uvid_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.uvid_Click(this, idPacijent);
        }

        private void pocetna_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.pocetna_Click(this, idPacijent);
        }

        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.anketa_Click(this, idPacijent);
        }

        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Korisnik_Click(this, idPacijent);
        }

        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.PromeniTemu(SvetlaTema, tamnaTema);

        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
          
            PacijentWebStranice.Jezik_Click(Jezik);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ElektronskoPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            Page uvid = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvid);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!LicniPodaciPacijenta.ProveraCifara(brojKartice.Text))
            {
                valBrojKartice.Visibility = Visibility.Visible;
                return;
            }
            valBrojKartice.Visibility = Visibility.Hidden;
            prvo = true;
            ProveriSvaPolja();
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            drugo = true;
            ProveriSvaPolja();
        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {
            trece = true;
            ProveriSvaPolja();
        }

        public void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show(drzava.SelectedValue.ToString());
            if(drzava.SelectedValue.ToString().Length == 36) // drzava ili country
            {
                cetvrto = false;
                return;
            }
            cetvrto = true;
            ProveriSvaPolja();
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            peto = true;
            ProveriSvaPolja();
        }

        private void ComboBox_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            sesto = true;
           ProveriSvaPolja();
        }

        private void TextBox_TextChanged_3(object sender, TextChangedEventArgs e)
        {
            if (!LicniPodaciPacijenta.ProveraCifara(cvv.Text))
            {
                valCVV.Visibility = Visibility.Visible;
                return;
            }
            valCVV.Visibility = Visibility.Hidden;
            sedmo = true;
            ProveriSvaPolja();
        }

        private void TextBox_TextChanged_4(object sender, TextChangedEventArgs e)
        {
            if (!LicniPodaciPacijenta.ProveraCifara(postanskiBroj.Text))
            {
                valPostanskiBroj.Visibility = Visibility.Visible;
                return;
            }
            valPostanskiBroj.Visibility = Visibility.Hidden;
            osmo = true;
            ProveriSvaPolja();
        }

        private void ProveriSvaPolja()
        {
            if((Visa.IsChecked == false && American.IsChecked == false) || (Visa.IsChecked == true && American.IsChecked == true) )
            {
                return;
            } 
            if (prvo == true && drugo == true && trece == true && cetvrto == true && peto == true && sesto == true && sedmo == true && osmo == true && (American.IsChecked == true || Visa.IsChecked == true))
            {
                potvrdi.IsEnabled = true;
            }
            //potvrdi.IsEnabled = false;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            deveto = true;
            ProveriSvaPolja();
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            deseto = true;
            if(American.IsChecked == true || Visa.IsChecked == true)
            {
                MessageBox.Show("cekirano");
            }
            ProveriSvaPolja();
        }
    }
}
