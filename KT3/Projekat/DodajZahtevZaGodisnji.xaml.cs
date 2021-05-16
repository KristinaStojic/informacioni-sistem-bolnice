﻿using Model;
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
    /// Interaction logic for DodajZahtevZaGodisnji.xaml
    /// </summary>
    public partial class DodajZahtevZaGodisnji : Window
    {
        int idLekara;
        public DodajZahtevZaGodisnji(int id)
        {
            InitializeComponent();
            this.idLekara = id;
            popuniPodatke();
        }

        private void popuniPodatke()
        {
            foreach(Lekar lekar in LekariMenadzer.lekari)
            {
                if(lekar.IdLekara == idLekara)
                {
                    this.ime.Text = lekar.ImeLek;
                    this.prezime.Text = lekar.PrezimeLek; 
                }
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
