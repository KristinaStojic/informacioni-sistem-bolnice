﻿using System;
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
    /// Interaction logic for UvidUZahtjevOsoblje.xaml
    /// </summary>
    public partial class UvidUZahtjevOsoblje : Window
    {
        public ZahtjevZaKomunikaciju zahtjev;
        private int colNum = 0;
        public static ObservableCollection<Osoblje> Zahtjevi { get; set; }
        public UvidUZahtjevOsoblje(ZahtjevZaKomunikaciju zahtjev)
        {
            InitializeComponent();
            inicijalizujElemente(zahtjev);
            dodajZahtjeve();
        }

        private void dodajZahtjeve()
        {
            Zahtjevi = new ObservableCollection<Osoblje>();
            foreach(Osoblje osoblje in zahtjev.osoblje)
            {
                Zahtjevi.Add(osoblje);
            }
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void inicijalizujElemente(ZahtjevZaKomunikaciju zahtjev)
        {
            
            this.zahtjev = zahtjev;
            this.DataContext = this;
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.N)
                {
                    Odustani_Click(sender, e);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ZahtjeviZaKomunikaciju.Zahtjevi.Remove(zahtjev);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ZahtjeviZaKomunikaciju.Zahtjevi.Remove(zahtjev);
            this.Close();
        }
    }
}