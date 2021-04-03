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
    /// Interaction logic for PrikazStaticke.xaml
    /// </summary>
    public partial class PrikazStaticke : Window
    {
        Sala izabranaSala;
        
        private int colNum = 0;
        
        public static ObservableCollection<Oprema> OpremaStaticka
        {
            get;
            set;
        }

        public PrikazStaticke(Sala izabranaSala)
        {
            InitializeComponent();
            this.izabranaSala = izabranaSala;
            this.DataContext = this;
            if(izabranaSala != null)
            {
                if (izabranaSala.TipSale == tipSale.SalaZaPregled)
                {
                    this.tekst.Text = "Sala za pregled (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
                else
                {
                    this.tekst.Text = "Opreaciona sala (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
            }
            OpremaStaticka = new ObservableCollection<Oprema>();
            foreach (Oprema o in izabranaSala.Oprema)
            {
                if (o.Staticka)
                {
                    OpremaStaticka.Add(o);
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
    }
}
