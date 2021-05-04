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
    /// Interaction logic for OdbijeniLijekovi.xaml
    /// </summary>
    public partial class OdbijeniLijekovi : Window
    {
        private int colNum = 0;
        public static ObservableCollection<Lek> OdbijeniLekovi
        {
            get;
            set;
        }
        public OdbijeniLijekovi()
        {
            InitializeComponent();
            inicijalizujLijekove();
        }

        private void inicijalizujLijekove()
        {
            this.DataContext = this;
            OdbijeniLekovi = new ObservableCollection<Lek>();
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (!zahtjev.odobrenZahtev && zahtjev.obradjenZahtev)
                {
                    OdbijeniLekovi.Add(zahtjev.lek);
                }
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
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Lek izabraniLijek = (Lek)this.dataGridOdbijeniLijekovi.SelectedItem;
            if (izabraniLijek != null)
            {
                Obrazlozenje obrazlozenje = new Obrazlozenje(izabraniLijek);
                obrazlozenje.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.N)
                {
                    Button_Click(sender, e);

                }
            }
        }
    }
}
