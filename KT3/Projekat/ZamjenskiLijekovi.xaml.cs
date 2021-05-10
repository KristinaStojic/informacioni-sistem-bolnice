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
    /// Interaction logic for ZamjenskiLijekovi.xaml
    /// </summary>
    public partial class ZamjenskiLijekovi : Window
    {
        public Lek izabraniLijek;
        private int colNum = 0;
        public static ObservableCollection<Lek> ZamjenskiLekovi
        {
            get;
            set;
        }
        public ZamjenskiLijekovi(Lek izabraniLijek)
        {
            InitializeComponent();
            this.izabraniLijek = izabraniLijek;
            this.DataContext = this;
            postaviTekst();
            dodajLijekove();
        }
        private void postaviTekst()
        {
            this.tekst.Text = "Zamjenski lijekovi za lijek: " + izabraniLijek.nazivLeka;
        }
        private void dodajLijekove()
        {
            ZamjenskiLekovi = new ObservableCollection<Lek>();
            foreach(Lek lijek in LekoviMenadzer.lijekovi)
            {
                if(izabraniLijek.idLeka == lijek.idLeka)
                {
                    if (lijek.zamenskiLekovi != null)
                    {
                        dodajZamjenski(lijek);
                    }
                }
            }
        }

        private void dodajZamjenski(Lek lijek)
        {
            foreach (int zamjenskiLijek in lijek.zamenskiLekovi)
            {
                foreach (Lek zamjenski in LekoviMenadzer.lijekovi)
                {
                    if (zamjenski.idLeka == zamjenskiLijek)
                    {
                        ZamjenskiLekovi.Add(zamjenski);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DodajZamjenskiLijek dodajZamjenskiLijek = new DodajZamjenskiLijek(izabraniLijek);
            dodajZamjenskiLijek.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Lek izabraniLijek = (Lek)dataGridLijekovi.SelectedItem;
            if(izabraniLijek != null)
            {
                IzmjeniLijek izmjeniLijek = new IzmjeniLijek(izabraniLijek);
                izmjeniLijek.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati lijek!");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Lek zamjenskiLijek = (Lek)dataGridLijekovi.SelectedItem;
            if(zamjenskiLijek != null)
            {
                LekoviMenadzer.obrisiZamjenski(izabraniLijek, zamjenskiLijek);
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
                if (e.Key == Key.D)
                {
                    Button_Click(sender, e);
                }else if(e.Key == Key.D || e.Key == Key.N)
                {
                    MenuItem_Click(sender, e);
                }
                else if (e.Key == Key.I)
                {
                    Button_Click_1(sender, e);
                }
                else if (e.Key == Key.O)
                {
                    Button_Click_2(sender, e);
                }
                else if (e.Key == Key.P)
                {
                    this.Pretraga.Focus();
                }

            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ZamjenskiLekovi.Clear();

            if (izabraniLijek.zamenskiLekovi != null)
            {
                foreach (int zamjenskiLijek in izabraniLijek.zamenskiLekovi)
                {
                    foreach (Lek zamjenski in LekoviMenadzer.lijekovi)
                    {
                        if(zamjenski.idLeka == zamjenskiLijek && zamjenski.nazivLeka.StartsWith(this.Pretraga.Text))
                        {
                            ZamjenskiLekovi.Add(zamjenski);
                        }
                    }
                }
            }            
        }
    }
}
