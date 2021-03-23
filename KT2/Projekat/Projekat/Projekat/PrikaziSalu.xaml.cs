using Model;
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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikaziSalu.xaml
    /// </summary>
    public partial class PrikaziSalu : Window
    {
        private int colNum = 0;
        public static ObservableCollection<Sala> Sale
        {
            get;
            set;
        }
        public PrikaziSalu()
        {
            InitializeComponent();
            this.DataContext = this;
            Sale = new ObservableCollection<Sala>();
            foreach (Sala s in SaleMenadzer.NadjiSveSale())
            {
                Sale.Add(s);
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
            DodajSalu ds = new DodajSalu();
            ds.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var izabranaSala = dataGridSale.SelectedItem;
            if (izabranaSala != null)
            {
                
                SaleMenadzer.ObrisiSalu((Sala)izabranaSala);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Sala izabranaSala = (Sala)dataGridSale.SelectedItem;
            if (izabranaSala != null)
            {
                IzmjeniSalu iss = new IzmjeniSalu(izabranaSala);
                iss.Show();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mw = new MainWindow();
            mw.Show();
        }
    }
}
