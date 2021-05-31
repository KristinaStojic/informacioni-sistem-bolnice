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
using System.Windows.Shapes;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for DetaljiAlergena.xaml
    /// </summary>
    public partial class DetaljiAlergena : Window
    {
        Pacijent pacijent;
        Alergeni stariAlergen;
        Termin termin;
        public DetaljiAlergena(Alergeni izabraniAlergen, Termin termin)
        {
            InitializeComponent();
            this.stariAlergen = izabraniAlergen;
            this.termin = termin;

            this.nadjiAlergen.ItemsSource = LekoviServis.NadjiSveSastojke();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(nadjiAlergen.ItemsSource);
            view.Filter = UserFilter;

            PopuniPodatkeOAlergenu(izabraniAlergen);
        }
        private void PopuniPodatkeOAlergenu(Alergeni izabraniAlergen)
        {
            foreach (Pacijent pac in PacijentiServis.pacijenti())
            {
                if (pac.IdPacijenta == izabraniAlergen.IdPacijenta)
                {
                    this.naziv.Text = izabraniAlergen.NazivSastojka;
                    this.nuspojava.Text = izabraniAlergen.NuspojavaNaNastojak;
                    this.vreme.Text = izabraniAlergen.VremeReakcije;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //sacuvaj
            string nazivLeka = naziv.Text;
            string nuspojavaNaLek = nuspojava.Text;
            string vremeReakcije = vreme.Text;

            Alergeni noviAlergen = new Alergeni(stariAlergen.IdAlergena, stariAlergen.IdPacijenta, nazivLeka, nuspojavaNaLek, vremeReakcije);
            ZdravstveniKartonServis.IzmeniAlergen(stariAlergen, noviAlergen);

            TerminServisLekar.sacuvajIzmene();
            PacijentiServis.SacuvajIzmenePacijenta();
            SaleServis.sacuvajIzmjene();


            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //odustani
            this.Close();
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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nadjiAlergen.SelectedItems.Count > 0)
            {
                Sastojak item = (Sastojak)nadjiAlergen.SelectedItems[0];
                naziv.Text = item.naziv;
            }
        }
    }
}
