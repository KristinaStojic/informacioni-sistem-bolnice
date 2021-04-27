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
using Model;
using Projekat.Model;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for OglasnaTabla.xaml
    /// </summary>
    public partial class OglasnaTabla : Window
    {
        bool flag = false;

        public static ObservableCollection<Obavestenja> oglasnaTabla { get; set; }

        public OglasnaTabla()
        {
            InitializeComponent();

            foreach (Obavestenja obavestenje in ObavestenjaMenadzer.obavestenja)
            { 
                if (obavestenje.Notifikacija == false)
                {
                    listView.Items.Add(obavestenje);
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziPacijenta p = new PrikaziPacijenta();
            p.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziTerminSekretar p = new PrikaziTerminSekretar();
            p.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Sekretar s = new Sekretar();
            s.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ObavestenjaMenadzer.sacuvajIzmene();
        }

        private void obavestenja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flag == false)
            {
                canvas2.Visibility = Visibility.Visible;
            }

            Obavestenja selektovanoObavestenje = (Obavestenja)listView.SelectedItem;

            if (selektovanoObavestenje != null)
            {
                naslov.Text = selektovanoObavestenje.TipObavestenja;
                datum.Text = selektovanoObavestenje.Datum;
                sadrzaj.Text = selektovanoObavestenje.SadrzajObavestenja;

                if (selektovanoObavestenje.Oznaka.Equals("svi"))
                {
                    namena.Text = "sve zaposlene";
                }
                else if (selektovanoObavestenje.Oznaka.Equals("lekari"))
                {
                    namena.Text = "lekare";
                }
                else if (selektovanoObavestenje.Oznaka.Equals("upravnici"))
                {
                    namena.Text = "upravnike";
                }
                else if (selektovanoObavestenje.Oznaka.Equals("pacijenti"))
                {
                    namena.Text = "sve pacijente";
                }
                else if (selektovanoObavestenje.Oznaka.Equals("specificni pacijenti"))
                {
                    namena.Text = "";
                    foreach (int id in selektovanoObavestenje.ListaIdPacijenata)
                    {
                        Pacijent pacijent = PacijentiMenadzer.PronadjiPoId(id);
                        namena.Text += pacijent.ImePacijenta + " " + pacijent.PrezimePacijenta +" \n";
                    }
                }
            }
        }

        // dodaj
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DodajObavestenje dodavanje = new DodajObavestenje();
            dodavanje.Show();
        }

        // izmeni
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        // obrisi
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        // X na detaljnom uvid u selektovano obavestenje
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            canvas2.Visibility = Visibility.Hidden;
        }
    }
}
