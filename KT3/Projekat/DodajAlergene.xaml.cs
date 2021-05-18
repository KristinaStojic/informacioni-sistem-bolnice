using Model;
using Projekat.Model;
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
using System.Windows.Shapes;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for DodajAlergene.xaml
    /// </summary>
    public partial class DodajAlergene : Window
    {

        Pacijent pacijent;
        Termin termin;

        public DodajAlergene(Pacijent izabraniPacijent, Termin izabraniTermin)
        {
            InitializeComponent();
            this.pacijent = izabraniPacijent;
            this.termin = izabraniTermin;

            PopuniPodatkePacijenta();
        }

        private void PopuniPodatkePacijenta()
        {
            this.nadjiAlergen.ItemsSource = LekoviMenadzer.NadjiSveSastojke();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(nadjiAlergen.ItemsSource);
            view.Filter = UserFilter;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nadjiAlergen.SelectedItems.Count > 0)
            {
                Sastojak item = (Sastojak)nadjiAlergen.SelectedItems[0];
                naziv.Text = item.naziv;
                sifra.Text = item.kolicina.ToString();
            }
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(pretraga.Text))
                return true;
            else
                return ((item as Sastojak).naziv.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void pretraga_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(nadjiAlergen.ItemsSource).Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idAlergena = ZdravstveniKartonMenadzer.GenerisanjeIdAlergena(pacijent.IdPacijenta);
                String nazivLeka = naziv.Text;
                String sifraLeka = sifra.Text;
                String Nuspojava = nuspojava.Text;
                String vremeNuspojave = vreme.Text;


                Alergeni alergen = new Alergeni(idAlergena, pacijent.IdPacijenta, nazivLeka, sifraLeka, Nuspojava, vremeNuspojave);
                ZdravstveniKartonMenadzer.DodajAlergen(alergen);

                TerminMenadzer.sacuvajIzmene();
                PacijentiServis.SacuvajIzmenePacijenta();
                SaleServis.sacuvajIzmjene();

                this.Close();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
