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
using Projekat.Model;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for OglasnaTabla.xaml
    /// </summary>
    public partial class OglasnaTabla : Window
    {
        public List<Obavestenja> list;

        public OglasnaTabla()
        {
            InitializeComponent();


            list = new List<Obavestenja>();
            foreach (Obavestenja o in ObavestenjaMenadzer.obavestenja)
            {
                obavestenjaLista.Items.Add(o);
                list.Add(o);
            }
            this.listaOb.ItemsSource = list;


        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziPacijenta p = new PrikaziPacijenta();
            p.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziTerminSekretar p = new PrikaziTerminSekretar();
            p.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void obavestenja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // dodaj
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        // izmeni
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        // obrisi
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
    }
}
