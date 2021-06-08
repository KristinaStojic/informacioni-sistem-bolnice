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
            this.tekst.Text = "Sastojci leka: " + izabranilek.nazivLeka;
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
            Sastojak izabraniSastojak = (Sastojak)dataGridSastojci.SelectedItem;
            if (izabraniSastojak != null)
            {
                ObrisiSastojakLekar obrisiSastojak = new ObrisiSastojakLekar(izabraniSastojak,lek);
                obrisiSastojak.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati sastojak!");
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Dodaj(sender, e);
            }
            else if (e.Key == Key.I && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Izmeni(sender, e);
            }
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_1(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Nazad(sender, e);
            }
        }
    }
}
