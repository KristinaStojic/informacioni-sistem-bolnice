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
    /// Interaction logic for Lijekovi.xaml
    /// </summary>
    public partial class Lijekovi : Window
    {

        private int colNum = 0;
        public static ObservableCollection<Lek> Lekovi
        {
            get;
            set;
        }

        public Lijekovi()
        {
            InitializeComponent();
            this.DataContext = this;
            dodajLijekove();
        }

        private void dodajLijekove()
        {
            Lekovi = new ObservableCollection<Lek>();
            foreach(Lek lijek in LekoviMenadzer.lijekovi)
            {
                Lekovi.Add(lijek);
            }
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DodajLijek dodajLijek = new DodajLijek();
            dodajLijek.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Lek izabraniLijek = (Lek)dataGridLijekovi.SelectedItem;
            if(izabraniLijek != null)
            {
                IzmjeniLijek izmjeniLijek = new IzmjeniLijek(izabraniLijek);
                izmjeniLijek.ShowDialog();
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Zahtjevi zahtjevi = new Zahtjevi();
            this.Close();
            zahtjevi.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Lek izabraniLijek = (Lek)dataGridLijekovi.SelectedItem;
            if(izabraniLijek != null)
            {
                LekoviMenadzer.obrisiLijek(izabraniLijek);
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Lek izabraniLijek = (Lek)dataGridLijekovi.SelectedItem;
            if (izabraniLijek != null)
            {
                ZamjenskiLijekovi zamjenskiLijekovi = new ZamjenskiLijekovi(izabraniLijek);
                zamjenskiLijekovi.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }
    }
}
