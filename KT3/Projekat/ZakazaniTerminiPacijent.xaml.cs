using Model;
using Projekat.Model;
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
    public partial class ZakazaniTerminiPacijent : Page
    {
        private int colNum = 0;
        public static ObservableCollection<Termin> Termini { get; set; }
        public static int idPacijent;
        public static Pacijent prijavljeniPacijent;

        public ZakazaniTerminiPacijent(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = idPrijavljenogPacijenta;
            Termini = new ObservableCollection<Termin>();
            foreach (Termin t in TerminMenadzer.termini)
            {
                if (t.Pacijent.IdPacijenta == idPacijent)
                {
                    Termini.Add(t);
                }
            }
            prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Termini);
            view.Filter = UserFilter;
            PrikaziTermin.AktivnaTema(this.zaglavlje , this.svetlaTema);
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // izmeni
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // brisanje
            Termin terminZaBrisanje = (Termin)dataGridTermini.SelectedItem;
            if (terminZaBrisanje == null)
            {
                MessageBox.Show("Selektujte termin koji zelite da otkazete", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            Page otkazivanjeTermina = new OtkaziTermin(terminZaBrisanje);
            this.NavigationService.Navigate(otkazivanjeTermina);
        }

        private void odjava_Click(object sender, RoutedEventArgs e)
        {
            Page odjava = new PrijavaPacijent();
            this.NavigationService.Navigate(odjava);
        }

        public void karton_Click(object sender, RoutedEventArgs e)
        {
            Page karton = new ZdravstveniKartonPacijent(idPacijent);
            this.NavigationService.Navigate(karton);
        }

        public void zakazi_Click(object sender, RoutedEventArgs e)
        {
            Page zakaziTermin = new ZakaziTermin(idPacijent);
            this.NavigationService.Navigate(zakaziTermin);
        }

        public void uvid_Click(object sender, RoutedEventArgs e)
        {
            Page uvid = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvid);
        }

        private void pocetna_Click(object sender, RoutedEventArgs e)
        {
            Page pocetna = new PrikaziTermin(idPacijent);
            this.NavigationService.Navigate(pocetna);
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dataGridTermini.ItemsSource).Refresh();
        }

        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            Page prikaziAnkete = new PrikaziAnkete(idPacijent);
            this.NavigationService.Navigate(prikaziAnkete);
        }
        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
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
            }
        }
    }
}
