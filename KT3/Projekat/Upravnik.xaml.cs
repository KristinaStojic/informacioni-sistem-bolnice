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
    /// Interaction logic for Upravnik.xaml
    /// </summary>
    public partial class Upravnik : Window
    {
        public static ObservableCollection<Obavestenja> obavjestenjaUpravnik
        {
            get;
            set;
        }
        public Upravnik()
        {
            InitializeComponent();
            this.DataContext = this;
            obavjestenjaUpravnik = new ObservableCollection<Obavestenja>();
            dodajObavjestenja();
        }
        private void dodajObavjestenja()
        {
            foreach (Obavestenja obavjestenje in ObavestenjaMenadzer.obavestenja)
            {
                obavjestenjaUpravnik.Add(obavjestenje);
            }
        }
        private void Prostorije_Click(object sender, RoutedEventArgs e)
        {
            PrikaziSalu w1 = new PrikaziSalu();
            this.Close();
            w1.ShowDialog();
        }

        private void Zahtjevi_Click(object sender, RoutedEventArgs e)
        {
            Zahtjevi w2 = new Zahtjevi();
            this.Close();
            w2.ShowDialog();
        }

        private void Odjava_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Obavestenja izabranoObavjestenje = (Obavestenja)this.dataGridObavjestenja.SelectedItem;
            if (izabranoObavjestenje != null)
            {
                PrikazObavjestenja prikazObavjestenja = new PrikazObavjestenja(izabranoObavjestenje);
                prikazObavjestenja.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati obavjestenje!");
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.O)
                {
                    Odjava_Click(sender, e);
                }
            }
        }
    }
}
