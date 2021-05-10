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
    /// Interaction logic for DodajZamjenskiLijek.xaml
    /// </summary>
    public partial class DodajZamjenskiLijek : Window
    {
        Lek izabraniLijek;
        private int colNum = 0;
        public static ObservableCollection<Lek> ZamjenskiLekovi
        {
            get;
            set;
        }
        public DodajZamjenskiLijek(Lek izabraniLijek)
        {
            InitializeComponent();
            this.izabraniLijek = izabraniLijek;
            this.DataContext = this;
            postaviTekst();
            dodajLijekove();
        }
        private void postaviTekst()
        {
            this.tekst.Text = izabraniLijek.nazivLeka;
        }
        private void dodajLijekove()
        {
            ZamjenskiLekovi = new ObservableCollection<Lek>();
            foreach (Lek lijek in LekoviMenadzer.lijekovi)
            {
                if (lijek.idLeka != izabraniLijek.idLeka && !postojiZamjenski(lijek))
                {
                    ZamjenskiLekovi.Add(lijek);
                }
            }
        }

        private bool postojiZamjenski(Lek lijek)
        {
            foreach(int zamjenski in izabraniLijek.zamenskiLekovi)
            {
                if(zamjenski == lijek.idLeka)
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Lek> izabraniLijekovi = nadjiIzabraneLijekove();
            if(izabraniLijekovi != null)
            {
                LekoviMenadzer.dodajZamjenskeLijekove(izabraniLijek, izabraniLijekovi);
            }
            else
            {
                MessageBox.Show("Morate izabrati lijekove!");
            }
            this.Close();
        }

        private List<Lek> nadjiIzabraneLijekove()
        {
            List<Lek> izabraniLijekovi = new List<Lek>();
            for(int i = 0; i < this.dataGridLijekovi.SelectedItems.Count; i++)
            {
                izabraniLijekovi.Add((Lek)dataGridLijekovi.SelectedItems[i]);
            }
            
            return izabraniLijekovi;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.N)
                {
                    Button_Click_1(sender, e);
                }
            }
        }
    }
}
