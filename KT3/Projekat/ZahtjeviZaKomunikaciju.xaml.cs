﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for ZahtjeviZaKomunikaciju.xaml
    /// </summary>
    public partial class ZahtjeviZaKomunikaciju : Window
    {
        private int colNum = 0;
        public static ObservableCollection<ZahtjevZaKomunikaciju> Zahtjevi{get; set;}

        public ZahtjeviZaKomunikaciju()
        {
            InitializeComponent();
            inicijalizujZahtjeve();
        }

        private void inicijalizujZahtjeve()
        {
            this.DataContext = this;
            Zahtjevi = new ObservableCollection<ZahtjevZaKomunikaciju>();
            Zahtjevi.Add(new ZahtjevZaKomunikaciju("ZDRAVO", "Beograd", "Transfer materijala"));
            Zahtjevi.Add(new ZahtjevZaKomunikaciju("ZDRAVO", "Beograd", "Transfer osoblja"));
            Zahtjevi.Add(new ZahtjevZaKomunikaciju("ZDRAVO", "Sarajevo", "Transfer materijala"));
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
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
    }

    public class ZahtjevZaKomunikaciju
    {
        public string nazivUstanove { get; set; }
        public string sjedisteUstanove { get; set; }
        public string tipZahtjeva { get; set; }

        public ZahtjevZaKomunikaciju(string nazivUstanove, string sjedisteUstanove, string tipZahtjeva)
        {
            this.nazivUstanove = nazivUstanove;
            this.sjedisteUstanove = sjedisteUstanove;
            this.tipZahtjeva = tipZahtjeva;
        }
    }
}

