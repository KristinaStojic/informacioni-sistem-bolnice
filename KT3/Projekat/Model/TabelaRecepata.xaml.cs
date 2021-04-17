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
        Termin termin;
        public Pacijent pacijent;

        public static ObservableCollection<LekarskiRecept> PrikazRecepata
        {
            get;
            set;
        }
        public TabelaRecepata(Pacijent izabraniPacijent, Termin Iztermin)
        {
            InitializeComponent();
            this.DataContext = this;
            this.termin = Iztermin;
            this.pacijent = izabraniPacijent;
            
            PrikazRecepata = new ObservableCollection<LekarskiRecept>();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                if(p.IdPacijenta == pacijent.IdPacijenta)
                {
                    foreach(LekarskiRecept lr in p.Karton.LekarskiRecepti)
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
            //Recept rec = new Recept(pacijent);
            DodajRecept rec = new DodajRecept(pacijent,termin);
            rec.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dataGridTermini_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
