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

namespace Projekat.Model
{
    /// <summary>
    /// Interaction logic for TabelaRecepata.xaml
    /// </summary>
    public partial class TabelaRecepata : Window
    {
        public int colNum = 0;
        Pacijent pacijent;
        public static ObservableCollection<LekarskiRecept> PrikazRecepata
        {
            get;
            set;
        }
        public TabelaRecepata(Pacijent izabraniPacijent)
        {
            InitializeComponent();
            this.DataContext = this; 
            this.pacijent = izabraniPacijent;
            PrikazRecepata = new ObservableCollection<LekarskiRecept>();
            foreach (ZdravstveniKarton k in ZdravstveniKartonMenadzer.NadjiSveKartone())
            {
                if(k.idPacijenta == pacijent.IdPacijenta)
                {
                    foreach(LekarskiRecept lr in k.LekarskiRecepti)
                    {
                        PrikazRecepata.Add(lr);
                    }
                    
                }
               
            }
            
        }


        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 6) 
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Recept rec = new Recept(pacijent);
            rec.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
