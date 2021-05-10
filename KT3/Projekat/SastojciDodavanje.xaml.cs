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
    /// Interaction logic for SastojciDodavanje.xaml
    /// </summary>
    public partial class SastojciDodavanje : Window
    {
        Lek uneseniLijek;
        private int colNum = 0;
        public static ObservableCollection<Sastojak> SastojciLijeka
        {
            get;
            set;
        }
        public SastojciDodavanje(Lek uneseniLijek)
        {
            InitializeComponent();
            this.DataContext = this;
            this.uneseniLijek = uneseniLijek;
            dodajSastojke();
        }
        private void dodajSastojke()
        {
            SastojciLijeka = new ObservableCollection<Sastojak>();

            foreach (Sastojak sastojak in uneseniLijek.sastojci)
            {
                SastojciLijeka.Add(sastojak);
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
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SastojciDodaj dodajSastojak = new SastojciDodaj(uneseniLijek);
            dodajSastojak.Show();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.N)
                {
                    Button_Click(sender, e);
                }
            }
        }
    }
}
