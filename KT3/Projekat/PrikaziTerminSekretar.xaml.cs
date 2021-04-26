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
    /// Interaction logic for PrikaziTerminSekretar.xaml
    /// </summary>
    public partial class PrikaziTerminSekretar : Window
    {
        bool flag = false;
        public static ObservableCollection<Termin> TerminiSekretar
        {
            get;
            set;
        }

        public PrikaziTerminSekretar()
        {
            InitializeComponent();
            this.DataContext = this;
            TerminiSekretar = new ObservableCollection<Termin>();
            foreach (Termin t in TerminMenadzer.termini)
            {
                TerminiSekretar.Add(t);
            }
        }

        // nazad
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TerminMenadzer.sacuvajIzmene();
            SaleMenadzer.sacuvajIzmjene();
            this.Close();
        }

        // zakazivanje
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ZakaziTerminSekretar zakazivanje = new ZakaziTerminSekretar();
            zakazivanje.Show();
        }

        // izmena
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Termin izabraniTermin = (Termin)terminiSekretarTabela.SelectedItem;
            if (izabraniTermin != null)
            {
                IzmeniTerminSekretar it = new IzmeniTerminSekretar(izabraniTermin);
                it.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali termin koji zelite da izmenite!");
            }
        }

        // brisanje
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            flag = true;
            Termin zaBrisanje = (Termin)terminiSekretarTabela.SelectedItem;
            canvas2.Visibility = Visibility.Hidden;

            if (zaBrisanje != null)
            {
                int idProstorije = zaBrisanje.Prostorija.Id;
                TerminMenadzer.OtkaziTerminSekretar(zaBrisanje);

                foreach (Termin t in TerminMenadzer.termini)
                {
                    foreach (Sala s in SaleMenadzer.sale)
                    {
                        if (s.Id == idProstorije)
                        {
                            t.Prostorija = s;
                        }
                    }
                }

                TerminMenadzer.sacuvajIzmene();
            }
            else
            {
                MessageBox.Show("Niste selektovali termin koji zelite da otkazete!");
            }
            flag = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TerminMenadzer.sacuvajIzmene();
            SaleMenadzer.sacuvajIzmjene();
        }

        // button nalozi pacijenata
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            TerminMenadzer.sacuvajIzmene();
            SaleMenadzer.sacuvajIzmjene();

            this.Close();
            PrikaziPacijenta p = new PrikaziPacijenta();
            p.Show();
        }

        // X na prikazu termina
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            canvas2.Visibility = Visibility.Hidden;
        }

        private void terminiSekretarTabela_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flag == false)
            {
                canvas2.Visibility = Visibility.Visible;
            }

            Termin t = (Termin)terminiSekretarTabela.SelectedItem;

            if (t != null)
            {
                datum.Text = t.Datum;
                pocetak.Text = t.VremePocetka;
                kraj.Text = t.VremeKraja;
                prostorija.Text = t.Prostorija.brojSale.ToString();
                tip.Text = t.tipTermina.ToString();
                imePac.Text = t.Pacijent.ImePacijenta;
                prezimePac.Text = t.Pacijent.PrezimePacijenta;
                jmbgPac.Text = t.Pacijent.Jmbg.ToString();
                imeLek.Text = t.Lekar.ImeLek;
                prezimeLek.Text = t.Lekar.PrezimeLek;
            }
        }

        // oglasna tabla
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            OglasnaTabla o = new OglasnaTabla();
            o.Show();
        }

        // hitna intervencija
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            HitanSlucaj h = new HitanSlucaj();
            h.Show();
        }
    }
}
