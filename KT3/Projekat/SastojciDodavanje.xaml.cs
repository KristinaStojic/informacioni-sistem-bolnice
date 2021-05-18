using Projekat.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for SastojciDodavanje.xaml
    /// </summary>
    public partial class SastojciDodavanje : Window
    {
        /*Lek uneseniLijek;
        private int colNum = 0;

        public static ObservableCollection<Sastojak> SastojciLijeka {get; set;}
        */
        public SastojciDodavanje()
        {
            InitializeComponent();
            //inicijalizujElemente(uneseniLijek);
            //dodajSastojke();
        }

        /*private void inicijalizujElemente(Lek uneseniLijek)
        {
            this.DataContext = this;
            this.uneseniLijek = uneseniLijek;
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
        }*/

        /*private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }*/

        /*private void DodajSastojak_Click(object sender, RoutedEventArgs e)
        {
            SastojciDodaj dodajSastojak = new SastojciDodaj(uneseniLijek);
            dodajSastojak.Show();
        }*/

        /*private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.N)
                {
                    Odustani_Click(sender, e);
                }
            }
        }*/
    }
}
