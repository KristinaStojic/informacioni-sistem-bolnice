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
    /// Interaction logic for Sastojci.xaml
    /// </summary>
    public partial class Sastojci : Window
    {
        public Lek izabraniLijek;
        private int colNum = 0;
        public static ObservableCollection<Sastojak> SastojciLijeka
        {
            get;
            set;
        }
        public Sastojci(Lek izabraniLijek)
        {
            InitializeComponent();
            this.izabraniLijek = izabraniLijek;
            this.DataContext = this;
            postaviTekst();
            dodajSastojke();
        }
        private void dodajSastojke()
        {
            SastojciLijeka = new ObservableCollection<Sastojak>();
            foreach(Lek lijek in LekoviMenadzer.lijekovi)
            {
                if(lijek.idLeka == izabraniLijek.idLeka)
                {
                    foreach(Sastojak sastojak in lijek.sastojci)
                    {
                        SastojciLijeka.Add(sastojak);
                    }
                }
            }
        }
        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }
        private void postaviTekst()
        {
            this.tekst.Text = "Sastojci za lijek: " + izabraniLijek.nazivLeka;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DodajSastojak dodajSastojak = new DodajSastojak(izabraniLijek);
            dodajSastojak.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Sastojak izabraniSastojak = (Sastojak)dataGridSastojci.SelectedItem;
            if(izabraniSastojak != null)
            {
                IzmjeniSastojak izmjeniSastojak = new IzmjeniSastojak(izabraniSastojak, izabraniLijek);
                izmjeniSastojak.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati sastojak!");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Sastojak izabraniSastojak = (Sastojak)dataGridSastojci.SelectedItem;
            if (izabraniSastojak != null)
            {
                LekoviMenadzer.obrisiSastojakLijeka(izabraniLijek, izabraniSastojak);
            }
            else
            {
                MessageBox.Show("Morate izabrati sastojak!");
            }
        }
    }
}
