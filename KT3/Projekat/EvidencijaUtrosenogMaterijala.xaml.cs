using Model;
using Projekat.Servis;
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
    /// Interaction logic for EvidencijaUtrosenogMaterijala.xaml
    /// </summary>
    public partial class EvidencijaUtrosenogMaterijala : Window
    {
        int idLekara;
        public EvidencijaUtrosenogMaterijala(int id)
        {
            InitializeComponent();
            idLekara = id;
            this.datum.IsEnabled = false;
            this.prostorija.SelectedIndex = 0;

            this.materijali.ItemsSource = null;
            this.kol.ItemsSource = null;
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource);
            datumFormat.Visibility = Visibility.Hidden;

            foreach (Lekar l in LekariServis.NadjiSveLekare())
            {
                if(l.IdLekara == idLekara)
                {
                    this.ime.Text = l.ImeLek;
                    this.prezime.Text = l.PrezimeLek;
                    this.datum.SelectedDate = DateTime.Now.Date;
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_2(sender, e);
            }
            else if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_2(sender, e);
            }
        }

        private void naziv_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            string ime = this.naziv.Text;
            this.materijali.Items.Add(ime);
        }

        private void kolicina_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            long result;
            if (long.TryParse(kolicina.Text, out result))
            {
                string koliko = this.kolicina.Text;
                this.kol.Items.Add(koliko);

            }
           
        }

        private void kolicina_TextChanged(object sender, TextChangedEventArgs e)
        {
            long result;
            if (long.TryParse(kolicina.Text, out result))
            {
                datumFormat.Visibility = Visibility.Hidden;

            }
            else
            {
                datumFormat.Visibility = Visibility.Visible;
            }
        }
    }
}
