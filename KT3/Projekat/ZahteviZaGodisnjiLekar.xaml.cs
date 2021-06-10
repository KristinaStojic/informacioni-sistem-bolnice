using Model;
using Projekat.Model;
using Projekat.Pomoc;
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
    /// Interaction logic for ZahteviZaGodisnjiLekar.xaml
    /// </summary>
    public partial class ZahteviZaGodisnjiLekar : Window
    {
        int IdLekara;
        public static ObservableCollection<ZahtevZaGodisnji> TabelaZahteva
        {
            get;
            set;
        }
        public ZahteviZaGodisnjiLekar(int id)
        {
            InitializeComponent();
            this.DataContext = this;
            this.IdLekara = id;
            dodajZahteveUTabelu();

        }
        private void dodajZahteveUTabelu()
        {
            TabelaZahteva = new ObservableCollection<ZahtevZaGodisnji>();
            foreach (Lekar lekar in LekariMenadzer.lekari)
            {
                if(lekar.IdLekara == IdLekara)
                {
                    foreach(ZahtevZaGodisnji zahtev in LekariMenadzer.zahtevi)
                    {
                        if(zahtev.lekar.IdLekara == IdLekara)
                        {
                            TabelaZahteva.Add(zahtev);
                        }
                    }
                }
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            ZahteviZaGodisnjiPomoc pomoc = new ZahteviZaGodisnjiPomoc();
            pomoc.Show();

        }

        private void Button_Zahtev(object sender, RoutedEventArgs e)
        {
            DodajZahtevZaGodisnji zahtev = new DodajZahtevZaGodisnji(IdLekara);
            zahtev.Show();
        }

        private void Button_Nazad(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Zahtev(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Nazad(sender, e);
            }
            else if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Hyperlink_Click(sender, e);
            }
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
