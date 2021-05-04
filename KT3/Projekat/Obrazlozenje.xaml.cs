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
    /// Interaction logic for Obrazlozenje.xaml
    /// </summary>
    public partial class Obrazlozenje : Window
    {
        Lek izabraniLijek;
        public Obrazlozenje(Lek izabraniLijek)
        {
            InitializeComponent();
            this.izabraniLijek = izabraniLijek;
            inicijalizujElemente();
        }

        private void inicijalizujElemente()
        {
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (zahtjev.obradjenZahtev && !zahtjev.odobrenZahtev && zahtjev.lek.idLeka == izabraniLijek.idLeka)
                {
                    this.obrazlozenje.Text = zahtjev.obrazlozenjeOdbijanja;
                }
            }

        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.N)
                {
                    Button_Click(sender, e);

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
