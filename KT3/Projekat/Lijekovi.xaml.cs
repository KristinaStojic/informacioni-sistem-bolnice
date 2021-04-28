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
                BrisanjeLijeka brisanjeLijeka = new BrisanjeLijeka(izabraniLijek);
                brisanjeLijeka.Show();
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

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Lek izabraniLijek = (Lek)dataGridLijekovi.SelectedItem;
            if (izabraniLijek != null)
            {
                Sastojci sastojci = new Sastojci(izabraniLijek);
                sastojci.Show();
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
                if (e.Key == Key.N || e.Key == Key.Z)
                {
                    Button_Click_2(sender, e);
                }else if(e.Key == Key.D)
                {
                    Button_Click(sender, e);
                }
                else if (e.Key == Key.I)
                {
                    Button_Click_1(sender, e);
                }
                else if (e.Key == Key.O)
                {
                    Button_Click_3(sender, e);
                }
                else if (e.Key == Key.T)
                {
                    MenuItem_Click(sender, e);
                }else if(e.Key == Key.P)
                {
                    this.Pretraga.Focus();
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            PrikaziSalu ps = new PrikaziSalu();
            this.Hide();
            ps.Show();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Lekovi.Clear();
            foreach (Lek lijek in LekoviMenadzer.lijekovi)
            {
                if (lijek.nazivLeka.StartsWith(this.Pretraga.Text))
                {
                    Lekovi.Add(lijek);
                }
            }
        }
    }
}
