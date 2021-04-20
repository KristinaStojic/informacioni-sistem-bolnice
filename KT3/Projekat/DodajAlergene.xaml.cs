using Model;
using Projekat.Model;
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
            this.nadjiAlergen.ItemsSource = MainWindow.alergeni;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(nadjiAlergen.ItemsSource);
            view.Filter = UserFilter;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nadjiAlergen.SelectedItems.Count > 0)
            {
                Alergeni item = (Alergeni)nadjiAlergen.SelectedItems[0];
                naziv.Text = item.NazivLeka;
                sifra.Text = item.SifraLeka;
            }
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(pretraga.Text))
                return true;
            else
                return ((item as Alergeni).NazivLeka.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0);
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
                PacijentiMenadzer.SacuvajIzmenePacijenta();
                SaleMenadzer.sacuvajIzmjene();

                this.Close();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
