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
using Model;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikaziTerminSekretar.xaml
    /// </summary>
    public partial class PrikaziTerminSekretar : Window
    {
        public static ObservableCollection<Termin> TerminiSekretar
        {
            get;
            set;
        }

        public PrikaziTerminSekretar()
        {
            InitializeComponent();
            this.DataContext = this;
            TerminiSekretar = new ObservableCollection<Termin>();
            foreach (Termin t in TerminMenadzer.NadjiSveTermine())
            {
                TerminiSekretar.Add(t);
            }
        }

        // nazad
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TerminMenadzer.sacuvajIzmene();
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ZakaziTerminSekretar zakazivanje = new ZakaziTerminSekretar();
            zakazivanje.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Termin izabraniTermin = (Termin)terminiSekretarTabela.SelectedItem;
            if (izabraniTermin != null)
            {
                IzmeniTerminSekretar it = new IzmeniTerminSekretar(izabraniTermin);
                it.Show();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Termin zaBrisanje = (Termin)terminiSekretarTabela.SelectedItem;
            if (zaBrisanje != null)
            {
                TerminMenadzer.OtkaziTerminSekretar(zaBrisanje);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TerminMenadzer.sacuvajIzmene();
        }
    }
}
