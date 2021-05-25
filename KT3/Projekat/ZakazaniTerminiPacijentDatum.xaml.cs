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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat
{
    public partial class ZakazaniTerminiPacijentDatum : Page
    {
        private int colNum = 0;
        public static ObservableCollection<Termin> Termini { get; set; }
        public static int idPacijent;
        public static Pacijent prijavljeniPacijent;

        public ZakazaniTerminiPacijentDatum(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            Termini = TerminServis.PronadjiTerminPoIdPacijenta(idPacijent);
            prijavljeniPacijent = PacijentiServis.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Termini);
            view.Filter = UserFilter;
            PacijentPagesServis.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
        }


        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as Termin).Datum.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void dataGridTermini_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 8) // **
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        /* Pomeri termin */
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Termin terminZaPomeranje = (Termin)dataGridTermini.SelectedItem;
            if (terminZaPomeranje == null)
            {
                MessageBox.Show("Selektujte termin koji zelite da izmenite", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (terminZaPomeranje.Pomeren == true)
            {
                MessageBox.Show("Nemoguce je pomeriti ovaj termin", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            Page izmeniTermin = new IzmeniTermin(terminZaPomeranje);
            this.NavigationService.Navigate(izmeniTermin);
        }


        /* Otkazi termin */
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Termin terminZaBrisanje = (Termin)dataGridTermini.SelectedItem;
            if (terminZaBrisanje == null)
            {
                MessageBox.Show("Selektujte termin koji zelite da otkazete", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (MalicioznoPonasanjeServis.DetektujMalicioznoPonasanje(idPacijent))
            {
                MessageBox.Show("Nije Vam omoguceno otkazivanje termina jer ste prekoracili dnevni limit modifikacije termina.", "Upozorenje", MessageBoxButton.OK);
                return;
            }
            Page otkazivanjeTermina = new OtkaziTermin(terminZaBrisanje);
            this.NavigationService.Navigate(otkazivanjeTermina);
        }

        private void odjava_Click(object sender, RoutedEventArgs e)
        {
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

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dataGridTermini.ItemsSource).Refresh();
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

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            PacijentPagesServis.Jezik_Click(Jezik);
        }

    }
}