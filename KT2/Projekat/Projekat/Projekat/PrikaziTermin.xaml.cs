using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using Model;


namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikaziTermin.xaml
    /// </summary>
    public partial class PrikaziTermin : Window
    {
        private int colNum = 0;
        public static ObservableCollection<Termin> Termini
        {
            get;
            set;
        }
        public PrikaziTermin()
        {
            InitializeComponent();
            this.DataContext = this;
            Termini = new ObservableCollection<Termin>();
            foreach (Termin t in TerminMenadzer.NadjiSveTermine())
            {
                Termini.Add(t);
            }
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 8)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ZakaziTermin zt = new ZakaziTermin();
            zt.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IzmeniTermin it = new IzmeniTermin();
            it.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OtkaziTermin ot = new OtkaziTermin();
            ot.Show();
            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
