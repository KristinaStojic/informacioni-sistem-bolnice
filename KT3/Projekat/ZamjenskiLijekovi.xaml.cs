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
                        foreach (int zamjenskiLijek in lijek.zamenskiLekovi)
                        {
                            foreach(Lek zamjenski in LekoviMenadzer.lijekovi)
                            {
                                if(zamjenski.idLeka == zamjenskiLijek)
                                {
                                    ZamjenskiLekovi.Add(zamjenski);
                                }
                            }
                        }
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
    }
}
