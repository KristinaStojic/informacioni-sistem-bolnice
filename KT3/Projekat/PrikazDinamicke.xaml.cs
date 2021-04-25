using Model;
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
    /// Interaction logic for PrikazDinamicke.xaml
    /// </summary>
    public partial class PrikazDinamicke : Window
    {
        Sala izabranaSala;
        private int colNum = 0;
        public static ObservableCollection<Oprema> OpremaDinamicka
        {
            get;
            set;
        }
        public PrikazDinamicke(Sala izabranaSala)
        {
            InitializeComponent();
            this.izabranaSala = izabranaSala;
            this.DataContext = this;
            OpremaDinamicka = new ObservableCollection<Oprema>();
            if (izabranaSala != null)
            {
                if (izabranaSala.TipSale == tipSale.SalaZaPregled)
                {
                    this.tekst.Text = "Sala za pregled (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
                else
                {
                    this.tekst.Text = "Operaciona sala (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
            }
            foreach(Oprema o in izabranaSala.Oprema)
            {
                if (!o.Staticka)
                {
                    OpremaDinamicka.Add(o);
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
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PreraspodjelaDinamicke.aktivna = true;
            PreraspodjelaDinamicke pd = new PreraspodjelaDinamicke(izabranaSala);
            pd.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Oprema opremaZaSlanje = (Oprema)dataGrid.SelectedItem;
            if (opremaZaSlanje != null) {
                SlanjeDinamicke sd = new SlanjeDinamicke(izabranaSala, opremaZaSlanje);
                SlanjeDinamicke.aktivan = true;
                sd.ShowDialog();
            }
            else
            {
                MessageBox.Show("Morate izabrati opremu");
            }
        }

        private void Pretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            OpremaDinamicka.Clear();
            foreach(Oprema oprema in izabranaSala.Oprema)
            {
                if (oprema.NazivOpreme.StartsWith(this.Pretraga.Text) && !oprema.Staticka)
                {
                    OpremaDinamicka.Add(oprema);
                }
            }
        }
    }
}
