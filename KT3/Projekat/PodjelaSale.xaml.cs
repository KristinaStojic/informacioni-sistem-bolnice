using Model;
using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PodjelaSale.xaml
    /// </summary>
    public partial class PodjelaSale : Window
    {
        public PodjelaSale(Sala staraSala, Sala novaSala)
        {
            InitializeComponent();
        }
    }
}
