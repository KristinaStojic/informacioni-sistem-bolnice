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
using Projekat.Pomoc;
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikaziTerminSekretar.xaml
    /// </summary>
    public partial class PrikaziTerminSekretar : Window
    {
        private bool flag = false;
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
            //List<Termin> terminiLista = TerminiSekretarServis.NadjiSveTermine();
            foreach (Termin t in TerminMenadzer.termini)
            {
                TerminiSekretar.Add(t);
            }

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(TerminiSekretar);
            view.Filter = UserFilter;
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(datumFilter.Text))
            {
                return true;
            }
            else
            {   // filtriranje po datumu, pacijentu, lekaru i prostoriji
                return ((item as Termin).Datum.IndexOf(datumFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Termin).imePrezimePacijenta.IndexOf(datumFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Termin).Pacijent.Jmbg.ToString().IndexOf(datumFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Termin).imePrezimeLekara.IndexOf(datumFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Termin).Prostorija.Id.ToString().IndexOf(datumFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            TerminiSekretarServis.sacuvajIzmene();
            SaleServis.sacuvajIzmjene();
            this.Close();
            Sekretar s = new Sekretar();
            s.Show();
        }

        private void Zakazi_Click(object sender, RoutedEventArgs e)
        {
            ZakaziTerminSekretar zakazivanje = new ZakaziTerminSekretar();
            zakazivanje.Show();
        }

        private void Pomeri_Click(object sender, RoutedEventArgs e)
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

        private void Otkazi_Click(object sender, RoutedEventArgs e)
        {
            flag = true;
            Termin zaBrisanje = (Termin)terminiSekretarTabela.SelectedItem;
            canvas2.Visibility = Visibility.Hidden;

            if (zaBrisanje != null)
            {
                OtkaziTerminSekretar otkazivanje = new OtkaziTerminSekretar(zaBrisanje);
                otkazivanje.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali termin koji zelite da otkazete!");
            }

            flag = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TerminiSekretarServis.sacuvajIzmene();
            SaleServis.sacuvajIzmjene();
        }

        private void Hitan_slucaj_Click(object sender, RoutedEventArgs e)
        {
            HitanSlucaj h = new HitanSlucaj();
            h.Show();
        }


        private void Napusti_uvid_Click(object sender, RoutedEventArgs e)
        {
            canvas2.Visibility = Visibility.Hidden;
        }

        private void TerminiSekretarTabela_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                prostorija.Text = t.Prostorija.Id.ToString();
                tip.Text = t.tipTermina.ToString();
                imePac.Text = t.Pacijent.ImePacijenta;
                prezimePac.Text = t.Pacijent.PrezimePacijenta;
                jmbgPac.Text = t.Pacijent.Jmbg.ToString();
                imeLek.Text = t.Lekar.ImeLek;
                prezimeLek.Text = t.Lekar.PrezimeLek;
                specijalizacijaLek.Text = t.Lekar.specijalizacija.ToString();
            }
        }

        private void DatumFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(terminiSekretarTabela.ItemsSource).Refresh();
        }

        private void Zdravstveni_karton_Click(object sender, RoutedEventArgs e)
        {
            Termin t = (Termin)terminiSekretarTabela.SelectedItem;

            if (t != null)
            {
                if (t.Pacijent != null)
                {
                    if (t.Pacijent.StatusNaloga.Equals(statusNaloga.Guest))
                    {
                        MessageBox.Show("Guest nalozi nemaju zdravstveni karton.");
                    }
                    else
                    {
                        UvidZdravstveniKarton karton = new UvidZdravstveniKarton(t.Pacijent);
                        karton.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Niste selektovali pacijenta ciji karton zelite da vidite!");
                }
            }
            else
            {
                MessageBox.Show("Niste selektovali pacijenta ciji karton zelite da vidite!");
            }
        }

        private void Pacijenti_Click(object sender, RoutedEventArgs e)
        {
            TerminiSekretarServis.sacuvajIzmene();
            SaleServis.sacuvajIzmjene();

            this.Close();
            PrikaziPacijenta p = new PrikaziPacijenta();
            p.Show();
        }
        private void Lekari_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziLekare prikaz = new PrikaziLekare();
            prikaz.Show();
        }

        private void Oglasna_tabla_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OglasnaTabla o = new OglasnaTabla();
            o.Show();
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            PrikaziTerminSekretarPomoc pomoc = new PrikaziTerminSekretarPomoc();
            pomoc.Show();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {     
            if (e.Key == Key.Z && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Zakazi_Click(sender, e);
            } 
            else if (e.Key == Key.Z && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Zakazi_Click(sender, e);
            } 
            else if (e.Key == Key.I && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Pomeri_Click(sender, e);
            }
            else if (e.Key == Key.I && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Pomeri_Click(sender, e);
            } 
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Otkazi_Click(sender, e);
            }
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Otkazi_Click(sender, e);
            }
            else if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Hitan_slucaj_Click(sender, e);
            }
            else if (e.Key == Key.H && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Hitan_slucaj_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Napusti_uvid_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Napusti_uvid_Click(sender, e);
            }
            else if (e.Key == Key.U && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Zdravstveni_karton_Click(sender, e);
            }
            else if (e.Key == Key.U && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Zdravstveni_karton_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Nazad_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Nazad_Click(sender, e);
            }
            // tabela termina
            else if (e.Key == Key.T && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                //terminiSekretarTabela_SelectionChanged(sender, SelectionChangedEventArgs e);
            }
            else if (e.Key == Key.T && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                //terminiSekretarTabela_SelectionChanged(sender, SelectionChangedEventArgs e);
            }
        }

    }
}
