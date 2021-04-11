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
    /// Interaction logic for PrikazAnamneza.xaml
    /// </summary>
    public partial class PrikazAnamneza : Window
    {
        Pacijent pacijent;
        public int colNum = 0;
         
        
       
        public static ObservableCollection<Anamneza> TabelaAnamneza
        {
            get;
            set;
        }

        public PrikazAnamneza(Pacijent izabraniPacijent)
        {
            InitializeComponent();
            this.DataContext = this;
            this.pacijent = izabraniPacijent;
            //Console.WriteLine(pacijent.ImePacijenta + " " + pacijent.PrezimePacijenta); //dobro nadje pacijenta
            Console.WriteLine("PACIJENT IMA ANAMNEZA NA ULAZU U PRIKAZ: " + pacijent.Karton.Anamneze.Count);
            TabelaAnamneza = new ObservableCollection<Anamneza>();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
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
            DodajAnamnezu da = new DodajAnamnezu(pacijent);
            Console.WriteLine("PACIJENT IMA ANAMNEZA NA ULAZU U DODAVANJE ANAMNEZE : " + pacijent.Karton.Anamneze.Count);
            da.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            Anamneza izabranaAnamneza = (Anamneza)dataGridTermini.SelectedItem;
            Console.WriteLine("IZABRANA ANAMNEZA: " + izabranaAnamneza.Terapija);
            Console.WriteLine("PACIJENT IMA ANAMNEZA NA ULAZU U DETALJE: " + pacijent.Karton.Anamneze.Count);
           
            if (izabranaAnamneza != null)
            {
                
                DetaljiAnamneze da = new DetaljiAnamneze(izabranaAnamneza);
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
