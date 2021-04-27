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
            DodajZamenskeLekove(lek);

        }

        private void DodajZamenskeLekove(Lek lek)
        {
            TabelaZamenskihLekova = new ObservableCollection<Lek>();
            foreach (Lek l in LekoviMenadzer.lijekovi)
            {
                if (lek.idLeka == l.idLeka)
                {
                    if (l.zamenskiLekovi != null)
                    {
                        foreach (int zamjenskiLijek in l.zamenskiLekovi)
                        {
                            foreach (Lek zamjenski in LekoviMenadzer.lijekovi)
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

        }

        private void Button_Izmeni(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Obrisi(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Nazad(object sender, RoutedEventArgs e)
        {

        }
    }
}
