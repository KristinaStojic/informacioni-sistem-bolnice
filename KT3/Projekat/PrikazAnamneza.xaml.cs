using System;
using System.Collections.Generic;
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
        int colNum = 0;
        public PrikazAnamneza()
        {
            InitializeComponent();
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DodajAnamnezu da = new DodajAnamnezu();
            da.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DetaljiAnamneze da = new DetaljiAnamneze();
            da.Show();
        }
    }
}
