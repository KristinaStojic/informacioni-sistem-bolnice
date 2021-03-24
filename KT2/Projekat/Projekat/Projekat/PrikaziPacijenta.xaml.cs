using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Model;
using Projekat.Model;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikaziPacijenta.xaml
    /// </summary>
    public partial class PrikaziPacijenta : Window
    {
        public static ObservableCollection<Pacijent> PacijentiTabela
        {
            get;
            set;
        }
        public PrikaziPacijenta()
        {
            InitializeComponent();
            this.DataContext = this;
            PacijentiTabela = new ObservableCollection<Pacijent>();
            foreach (Pacijent p in PacijentiFileManager.PronadjiSve())
            {
                PacijentiTabela.Add(p);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DodajPacijenta dodavanje = new DodajPacijenta();
            dodavanje.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Pacijent zaIzmenu = (Pacijent)TabelaPacijenata.SelectedItem;
          //  IzmeniPacijenta izmena = new IzmeniPacijenta(zaIzmenu);
          //  izmena.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Pacijent zaBrisanje = (Pacijent)TabelaPacijenata.SelectedItem;
            PacijentiFileManager.ObrisiNalog(zaBrisanje);
        }
    }
}
