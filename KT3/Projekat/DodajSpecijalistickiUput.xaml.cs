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
    /// Interaction logic for DodajSpecijalistickiUput.xaml
    /// </summary>
    public partial class DodajSpecijalistickiUput : Window
    {
        Pacijent pacijent;
        Termin termin;
        public DodajSpecijalistickiUput(Pacijent izabraniPacijent, Termin izabraniTermin)
        {
            InitializeComponent();

            this.pacijent = izabraniPacijent;
            this.termin = izabraniTermin;

            popuniPodatke();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(nadjiDoktora.ItemsSource);
            view.Filter = UserFilter;
        }
        private void popuniPodatke()
        {
            this.nadjiDoktora.ItemsSource = MainWindow.lekari;
            this.ime.Text = pacijent.ImePacijenta;
            this.prezime.Text = pacijent.PrezimePacijenta;
            this.jmbg.Text = pacijent.Jmbg.ToString();
            this.lekar.Text = termin.Lekar.ImeLek + " " + termin.Lekar.PrezimeLek;
            datum.SelectedDate = DateTime.Parse(termin.Datum);
            specijalistickiTab.IsSelected = true;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nadjiDoktora.SelectedItems.Count > 0)
            {
                Lekar item = (Lekar)nadjiDoktora.SelectedItems[0];
                specijalista.Text = item.ImeLek + " " + item.PrezimeLek;
            }
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(pretraga.Text))
                return true;
            else
                return ((item as Lekar).PrezimeLek.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void pretraga_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(nadjiDoktora.ItemsSource).Refresh();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //id uputa
                int idUputa = ZdravstveniKartonMenadzer.GenerisanjeIdUputa(pacijent.IdPacijenta);
                String detaljiOPregledu = napomena.Text;
                //specijalista
                int idSpecijaliste = nadjiIDSpecijaliste();
                //datum
                string datum = nadjiDatum();
                //tip pregleda
                tipUputa tip = nadjiTipUputa();

                Uput noviUput= new Uput(idUputa, pacijent.IdPacijenta, termin.Lekar.IdLekara, idSpecijaliste, detaljiOPregledu, datum, tip);
                ZdravstveniKartonMenadzer.DodajUput(noviUput);


                TerminMenadzer.sacuvajIzmene();
                PacijentiMenadzer.SacuvajIzmenePacijenta();

                this.Close();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int nadjiIDSpecijaliste()
        {
            String[] imeprz = specijalista.Text.Split(' ');
            String imeSpecijaliste = imeprz[0];
            String prezimeSpecijaliste = imeprz[1];
            int idSpecijaliste = 40;
            foreach (Lekar lekar in MainWindow.lekari)
            {
                if (lekar.ImeLek.Equals(imeSpecijaliste) && lekar.PrezimeLek.Equals(prezimeSpecijaliste))
                {
                    idSpecijaliste = lekar.IdLekara;
                }
            }
            return idSpecijaliste;
        }

        private string nadjiDatum()
        {
            String formatirano = null;
            DateTime? selectedDate = datum.SelectedDate;
            if (selectedDate.HasValue)
            {
                formatirano = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            }
            return formatirano;
        }
        private tipUputa nadjiTipUputa()
        {
            tipUputa tip = tipUputa.Laboratorija;
            string tab = (string)(uputi.SelectedItem as TabItem).Header;
            if (tab.Equals("Specijalistički pregled"))
            {
                tip = tipUputa.SpecijallistickiPregled;
            }
            else if (tab.Equals("Laboratorija"))
            {
                tip = tipUputa.Laboratorija;
            }
            else if (tab.Equals("Stacionarno lečenje"))
            {
                tip = tipUputa.StacionarnoLecenje;
            }

            return tip;

        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
