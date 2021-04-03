﻿using Model;
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
    /// Interaction logic for PrebaciDinamicku.xaml
    /// </summary>
    public partial class PrebaciDinamicku : Window
    {
        public Oprema izabranaOprema;
        public PrebaciDinamicku(Oprema oprema)
        {
            InitializeComponent();
            this.izabranaOprema = oprema;
            if (izabranaOprema != null)
            {
                this.oprema.Text = izabranaOprema.NazivOpreme;
            }
            DataContext = new ViewModel();
        }
        public class ViewModel
        {
            public ObservableCollection<Sala> sale { get; set; }
            public ViewModel()
            {
                sale = new ObservableCollection<Sala>();
                foreach (Sala s in SaleMenadzer.sale)
                {
                    sale.Add(s);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
