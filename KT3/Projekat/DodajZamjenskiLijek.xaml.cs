using Projekat.Model;
using Projekat.Servis;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            inicijalizujElemente(izabraniLijek);
            postaviTekst();
            dodajLijekove();
        }
        
        private void inicijalizujElemente(Lek izabraniLijek)
        {
            this.izabraniLijek = izabraniLijek;
            this.DataContext = this;
        }
        
        private void postaviTekst()
        {
            this.tekst.Text = izabraniLijek.nazivLeka;
        }
        
        private void dodajLijekove()
        {
            ZamjenskiLekovi = new ObservableCollection<Lek>();
            foreach (Lek lijek in LekoviServis.Lijekovi())
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

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if(nadjiIzabraneLijekove().Count != 0)
            {
                LekoviServis.dodajZamjenskeLijekove(izabraniLijek, nadjiIzabraneLijekove());
                this.Close();
            }
            else
            {
                MessageBox.Show("Morate izabrati lijekove!");
            }
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

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.N)
                {
                    Odustani_Click(sender, e);
                }
            }
        }
    }
}
