using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Model;
using Projekat.Model;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikaziTermin.xaml
    /// </summary>
    public partial class PrikaziTermin : Window
    {
        private int colNum = 0;
        public static ObservableCollection<Termin> Termini
        {
            get;
            set;
        }
        public PrikaziTermin()
        {
            InitializeComponent();
            this.DataContext = this;
            Termini = new ObservableCollection<Termin>();
<<<<<<< Updated upstream
=======
            Obavestenja = new ObservableCollection<Obavestenja>();
            // Teodora
            foreach (Obavestenja o in ObavestenjaMenadzer.obavestenja)
            {
                Obavestenja.Add(o);
            }

            pacijentProzor = true;
            Thread thread = new Thread(izvrsiNit);
            thread.Start();
>>>>>>> Stashed changes
            foreach (Termin t in TerminMenadzer.termini)
            {
                /*if (t.Pacijent.IdPacijenta == 1)
                {
                    Termini.Add(t);
                }*/
                Termini.Add(t);
            }
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 12) // **
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // zakazi
            ZakaziTermin zt = new ZakaziTermin();
            zt.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // izmeni
            Termin izabraniTermin = (Termin)dataGridTermini.SelectedItem;
            if (izabraniTermin != null)
            {
                IzmeniTermin it = new IzmeniTermin(izabraniTermin);
                //TerminMenadzer.sacuvajIzmene();
                it.Show();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // brisanje
            Termin zaBrisanje = (Termin)dataGridTermini.SelectedItem;
            if (zaBrisanje != null)
            {
                TerminMenadzer.OtkaziTermin(zaBrisanje);
                //TerminMenadzer.sacuvajIzmene();
            }
            Sala s = SaleMenadzer.NadjiSaluPoId(zaBrisanje.Prostorija.Id);
            foreach(ZauzeceSale zs in s.zauzetiTermini)
            {
                //MessageBox.Show(zs.idTermina.ToString());
                if (zs.idTermina.Equals(zaBrisanje.IdTermin))
                {
                    s.zauzetiTermini.Remove(zs);
                    break;
                }
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // nazad
            TerminMenadzer.sacuvajIzmene();
            SaleMenadzer.sacuvajIzmjene();
            PacijentiMenadzer.SacuvajIzmenePacijenta();
            this.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TerminMenadzer.sacuvajIzmene();
            SaleMenadzer.sacuvajIzmjene();
            PacijentiMenadzer.SacuvajIzmenePacijenta();
        }

        private void zdravstveniKarton_Click(object sender, RoutedEventArgs e)
        {
            Termin izabraniTermin = (Termin)dataGridTermini.SelectedItem;
            if (izabraniTermin != null)
            {
                ZdravstveniKartonPacijent it = new ZdravstveniKartonPacijent(izabraniTermin.Pacijent);
                //TerminMenadzer.sacuvajIzmene();
                it.Show();
            }
        }

        private void dataGridTermini_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // obavestenja
        private void obavestenja_Click(object sender, RoutedEventArgs e)
        {
            ObavestenjaPacijent o = new ObavestenjaPacijent();
            o.Show();
        }
    }
}
