using Projekat.Model;
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
    /// Interaction logic for PrikazObavjestenja.xaml
    /// </summary>
    public partial class PrikazObavjestenja : Window
    {
        public Obavestenja izabranoObavjestenje;
        public PrikazObavjestenja(Obavestenja izabranoObavjestenje)
        {
            InitializeComponent();
            this.izabranoObavjestenje = izabranoObavjestenje;
            postaviTekst();
        }

        private void postaviTekst()
        {
            this.sadrzajObavjestenja.Text = izabranoObavjestenje.SadrzajObavestenja;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
