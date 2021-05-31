using Model;
using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for SlanjeStaticke.xaml
    /// </summary>
    public partial class SlanjeStaticke : Window
    {
        public SlanjeStaticke(Sala izabranaSala, Oprema opremaZaSlanje)
        {
            InitializeComponent();            
        }

    }
}
