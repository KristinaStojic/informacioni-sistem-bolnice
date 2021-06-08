using Projekat.Model;
using Projekat.Servis;
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
    /// Interaction logic for PrikazZamenskihLekovaLekar.xaml
    /// </summary>
    public partial class PrikazZamenskihLekovaLekar : Window
    {
        Lek lek;
        public static ObservableCollection<Lek> TabelaZamenskihLekova
        {
            get;
            set;
        }

        public PrikazZamenskihLekovaLekar(Lek izabraniLek)
        {
            InitializeComponent();
            this.DataContext = this;
            this.lek = izabraniLek;
            this.tekst.Text = "Zamenski lekovi za lek: " + izabraniLek.nazivLeka;
            DodajZamenskeLekove(lek);

        }

        private void DodajZamenskeLekove(Lek lek)
        {
            TabelaZamenskihLekova = new ObservableCollection<Lek>();
            foreach (Lek l in LekoviServis.Lijekovi())
            {
                if (lek.idLeka == l.idLeka)
                {
                    if (l.zamenskiLekovi != null)
                    {
                        foreach (int zamjenskiLijek in l.zamenskiLekovi)
                        {
                            foreach (Lek zamjenski in LekoviServis.Lijekovi())
                            {
                                if (zamjenski.idLeka == zamjenskiLijek)
                                {
                                    TabelaZamenskihLekova.Add(zamjenski);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Button_Dodaj(object sender, RoutedEventArgs e)
        {
            DodajZamenskiLekLekar zamenskiLekovi = new DodajZamenskiLekLekar(lek);
            zamenskiLekovi.Show();
        }

        private void Button_Izmeni(object sender, RoutedEventArgs e)
        {
            Lek izabraniLek = (Lek)dataGridZamenskiLekovi.SelectedItem;

            if (izabraniLek != null)
            {

                IzmeniLekLekar izmeniLek = new IzmeniLekLekar(izabraniLek);
                izmeniLek.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali nijedan lek!");
            }
        }

        private void Button_Obrisi(object sender, RoutedEventArgs e)
        {
            Lek izabraniZamenskiLek = (Lek)dataGridZamenskiLekovi.SelectedItem;
            if (izabraniZamenskiLek != null)
            {
                ObrisiZamenskiLekLekar obrisiSLek = new ObrisiZamenskiLekLekar(lek,izabraniZamenskiLek);
                obrisiSLek.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati lek koji zelite da obrisete!");
            }
        }

        private void Button_Nazad(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                Button_Obrisi(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Nazad(sender, e);
            }
        }
    }
}
