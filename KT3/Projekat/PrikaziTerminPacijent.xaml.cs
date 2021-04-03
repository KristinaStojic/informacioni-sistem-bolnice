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

namespace Projekat
{
    public partial class PrikaziTerminPacijent : Window
    {
        private int colNum = 0;
        public static ObservableCollection<Termin> Termini
        {
            get;
            set;
        }

        public PrikaziTerminPacijent()
        {
            InitializeComponent();
            this.DataContext = this;
            Termini = new ObservableCollection<Termin>();
            foreach (Termin t in TerminMenadzer.NadjiSveTermine())
            {
                Termini.Add(t);
            }
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 12) // *
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ZakaziTerminPacijent zt = new ZakaziTerminPacijent();
            zt.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // izmeni
            Termin izabraniTermin = (Termin)dataGridTerminiPacijenta.SelectedItem;
            if (izabraniTermin != null)
            {
                IzmeniTerminPacijent it = new IzmeniTerminPacijent(izabraniTermin);
                it.Show();
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // brisanje
            Termin zaBrisanje = (Termin)dataGridTerminiPacijenta.SelectedItem;
            if (zaBrisanje != null)
            {
                TerminMenadzer.OtkaziTermin(zaBrisanje);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // nazad
            TerminMenadzer.sacuvajIzmene();
            this.Hide();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TerminMenadzer.sacuvajIzmene();
        }
    }
}
