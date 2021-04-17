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
    /// Interaction logic for DodajRecept.xaml
    /// </summary>
    public partial class DodajRecept : Window
    {
        Pacijent pacijent;
        Termin termin;
        public DodajRecept(Pacijent izabraniPacijent, Termin izabraniTermin)
        {
            InitializeComponent();
            this.pacijent = izabraniPacijent;
            this.termin = izabraniTermin;
            this.nadjiLek.ItemsSource = MainWindow.lekovi;
            this.pacijentIme.Text = izabraniPacijent.ImePacijenta + " " + izabraniPacijent.PrezimePacijenta;
            this.jmbg.Text = izabraniPacijent.Jmbg.ToString();
            this.lekar.Text = izabraniTermin.Lekar.ImeLek + " " + izabraniTermin.Lekar.PrezimeLek;
            datum.SelectedDate = DateTime.Parse(izabraniTermin.Datum);


            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(nadjiLek.ItemsSource);
            view.Filter = UserFilter;
           
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nadjiLek.SelectedItems.Count > 0)
            {
                Lek item = (Lek)nadjiLek.SelectedItems[0];
                nazivSifra.Text = item.nazivLeka;
            }
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(pretraga.Text))
                return true;
            else
                return ((item as Lek).nazivLeka.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void pretraga_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(nadjiLek.ItemsSource).Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int brojRecepta = ZdravstveniKartonMenadzer.GenerisanjeIdRecepta(pacijent.IdPacijenta);
                String nazivLeka = nazivSifra.Text;
                
                int kolicinaNaDan = int.Parse(kolicina.Text);
                int kolikoDana = int.Parse(dani.Text);
                String pocetakKoriscenja = sati.Text + ":" + min.Text;

                LekarskiRecept recept = new LekarskiRecept(pacijent, brojRecepta, nazivLeka, kolikoDana, kolicinaNaDan, pocetakKoriscenja, termin.Datum);
                ZdravstveniKartonMenadzer.DodajRecept(recept);

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
