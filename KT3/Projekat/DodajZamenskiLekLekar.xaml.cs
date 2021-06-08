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
    /// Interaction logic for DodajZamenskiLekLekar.xaml
    /// </summary>
    public partial class DodajZamenskiLekLekar : Window
    {
        Lek lek;
        private int colNum = 0;
        public static ObservableCollection<Lek> TabelaZamenskihLekova
        {
            get;
            set;
        }
        public DodajZamenskiLekLekar(Lek izabraniLek)
        {
            InitializeComponent();
            this.lek = izabraniLek;
            this.DataContext = this;
            postaviTekst();
            dodajLijekove();
        }
        private void postaviTekst()
        {
            this.tekst.Text = "Izaberite zamenske lekove za lek: " + lek.nazivLeka;
        }
        /* private void dodajLijekove()
         {
             TabelaZamenskihLekova = new ObservableCollection<Lek>();
             foreach (Lek lijek in LekoviMenadzer.lijekovi)
             {
                 if (lijek.idLeka != lek.idLeka && !postojiZamenski(lijek))
                 {
                     TabelaZamenskihLekova.Add(lijek);
                 }
             }
         }

         private bool postojiZamenski(Lek lijek)
         {
             foreach (int zamenski in lek.zamenskiLekovi)
             {
                 if (zamenski == lijek.idLeka)
                 {
                     return true;
                 }
             }
             return false;
         }*/

        private void dodajLijekove()
        {
            TabelaZamenskihLekova = new ObservableCollection<Lek>();
            foreach (Lek lijek in LekoviMenadzer.lijekovi)
            {
                if (lijek.idLeka != lek.idLeka && !postojiZamjenski(lijek))
                {
                    TabelaZamenskihLekova.Add(lijek);
                }
            }
        }

        private bool postojiZamjenski(Lek lijek)
        {
            foreach (int zamjenski in lek.zamenskiLekovi)
            {
                if (zamjenski == lijek.idLeka)
                {
                    return true;
                }
            }
            return false;
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void Button_Potvrdi(object sender, RoutedEventArgs e)
        {
            List<Lek> izabraniLekovi = nadjiIzabraneLekove();
            if (izabraniLekovi != null)
            {
                LekoviMenadzer.dodajZamenskeLekoveLekar(lek, izabraniLekovi);
            }
            else
            {
                MessageBox.Show("Morate izabrati lekove!");
            }
            this.Close();
        }

        private List<Lek> nadjiIzabraneLekove()
        {
            List<Lek> izabraniLekovi = new List<Lek>();
            for (int i = 0; i < this.dataGridLekovi.SelectedItems.Count; i++)
            {
                izabraniLekovi.Add((Lek)dataGridLekovi.SelectedItems[i]);
            }

            return izabraniLekovi;
        }

        private void Button_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void StackPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Potvrdi(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Odustani(sender, e);
            }
        }
    }
}
