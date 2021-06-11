using Model;
using Projekat.Model;
using Projekat.Servis;
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
    /// Interaction logic for PrikazAnamneza.xaml
    /// </summary>
    public partial class PrikazAnamneza : Window
    {
        Pacijent pacijent;
        public int colNum = 0;
        public Termin termin;
        
        public static ObservableCollection<Anamneza> TabelaAnamneza
        {
            get;
            set;
        }

        PacijentiServis servis = new PacijentiServis();

        public PrikazAnamneza(Pacijent izabraniPacijent, Termin termin)
        {
            InitializeComponent();
            TabelaAnamneza = new ObservableCollection<Anamneza>();
            this.DataContext = this;
            this.pacijent = izabraniPacijent;
            this.termin = termin;
            foreach (Pacijent p in servis.pacijenti())
            {
                if (p.IdPacijenta == pacijent.IdPacijenta)
                {
                    foreach (Anamneza an in p.Karton.Anamneze)
                    {
                        TabelaAnamneza.Add(an);
                    }

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
            DodajAnamnezu da = new DodajAnamnezu(pacijent,termin);   
            da.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            Anamneza izabranaAnamneza = (Anamneza)dataGridTermini.SelectedItem;
            
            if (izabranaAnamneza != null)
            {
                
                DetaljiAnamneze da = new DetaljiAnamneze(izabranaAnamneza,termin);
                da.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali nijednu anamnezu!");
            }
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
