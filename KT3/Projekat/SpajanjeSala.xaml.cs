using Model;
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
    /// Interaction logic for SpajanjeSala.xaml
    /// </summary>
    public partial class SpajanjeSala : Window
    {
        private int colNum = 0;
        Sala izabranaSala;

        public static ObservableCollection<Sala> Sale{get; set;}

        public SpajanjeSala(Sala izabranaSala)
        {
            InitializeComponent();
            inicijalizujElemente(izabranaSala);
            dodajSale();
        }

        private void inicijalizujElemente(Sala izabranaSala)
        {
            this.izabranaSala = izabranaSala;
            this.DataContext = this;

        }

        private void dodajSale()
        {
            Sale = new ObservableCollection<Sala>();
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (!sala.Namjena.Equals("Skladiste") && sala.Id != izabranaSala.Id && sala.TipSale.Equals(izabranaSala.TipSale))
                {
                    Sale.Add(sala);
                }
            }
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            Sala izabranaSalaZaSpajanje = (Sala)dataGridSale.SelectedItem;
            if(izabranaSalaZaSpajanje != null)
            {
                Renoviranje.salaZaSpajanje = izabranaSalaZaSpajanje;
                Renoviranje.opremaZaPrebacivanje = null;
                this.Close();
            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
