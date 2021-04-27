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
using System.Windows.Shapes;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikazSastojakaLekar.xaml
    /// </summary>
    public partial class PrikazSastojakaLekar : Window
    {
        Lek lek;
        public static ObservableCollection<Sastojak> TabelaSastojaka
        {
            get;
            set;
        }

        public PrikazSastojakaLekar(Lek izabranilek)
        {
            InitializeComponent();
            this.DataContext = this;
            this.lek = izabranilek;
            dodajSastojkeLeka(lek);
        }

        private void dodajSastojkeLeka(Lek lek)
        {
            TabelaSastojaka = new ObservableCollection<Sastojak>();
            foreach (Sastojak sastojak in lek.sastojci)
            {
                TabelaSastojaka.Add(sastojak);
            }
            Console.WriteLine(TabelaSastojaka.Count);
        }

        private void Button_Nazad(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Izmeni(object sender, RoutedEventArgs e)
        {
            Sastojak izabraniSastojak = (Sastojak)dataGridSastojci.SelectedItem;

            if (izabraniSastojak != null)
            {

                IzmeniSastojakLekar izmeniSastojak = new IzmeniSastojakLekar(lek,izabraniSastojak);
                izmeniSastojak.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali nijedan sastojak!");
            }

            
        }

        private void Button_Dodaj(object sender, RoutedEventArgs e)
        {
            DodajSastojakLekar sastojak = new DodajSastojakLekar(lek);
            sastojak.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
