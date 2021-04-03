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
    /// Interaction logic for Skladiste.xaml
    /// </summary>
    public partial class Skladiste : Window
    {
        private int colNum = 0;
        public static ObservableCollection<Oprema> OpremaStaticka
        {
            get;
            set;
        }
        public static ObservableCollection<Oprema> OpremaDinamicka
        {
            get;
            set;
        }
        public Skladiste()
        {
            InitializeComponent();
            this.DataContext = this;
            OpremaStaticka = new ObservableCollection<Oprema>();
            OpremaDinamicka = new ObservableCollection<Oprema>();
            foreach (Oprema o in OpremaMenadzer.NadjiStatickuOpremu())
            {
                OpremaStaticka.Add(o);
            }
            foreach (Oprema o in OpremaMenadzer.NadjiDinamickuOpremu())
            {
                OpremaDinamicka.Add(o);
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
            OpremaMenadzer.sacuvajIzmjene();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (T1.IsSelected)
            {
                DodajOpremu w1 = new DodajOpremu(true);
                w1.Show();
            }
            else
            {
                DodajOpremu w1 = new DodajOpremu(false);
                w1.Show();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (T1.IsSelected)
            {
                var izabranaOprema = dataGridT1.SelectedItem;
                if(izabranaOprema != null)
                {
                    OpremaMenadzer.ObrisiOpremu((Oprema)izabranaOprema);
                }
            }
            else
            {
                var izabranaOprema = dataGridT2.SelectedItem;
                if (izabranaOprema != null)
                {
                    OpremaMenadzer.ObrisiOpremu((Oprema)izabranaOprema);
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (T1.IsSelected)
            {
                Oprema izabranaOprema = (Oprema)dataGridT1.SelectedItem;
                if (izabranaOprema != null)
                {
                    IzmjeniOpremu iop = new IzmjeniOpremu(izabranaOprema);
                    iop.Show();
                }
            }
            else
            {
                Oprema izabranaOprema = (Oprema)dataGridT2.SelectedItem;
                if (izabranaOprema != null)
                {
                    IzmjeniOpremu iop = new IzmjeniOpremu(izabranaOprema);
                    iop.Show();
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpremaMenadzer.sacuvajIzmjene();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (T1.IsSelected)
            {
                Oprema izabranaOprema = (Oprema)dataGridT1.SelectedItem;
                PrebaciStaticku ps = new PrebaciStaticku(izabranaOprema);
                ps.Show();
            }
            else
            {
                PrebaciDinamicku pd = new PrebaciDinamicku();
                pd.Show();
            }
        }
    }
}
