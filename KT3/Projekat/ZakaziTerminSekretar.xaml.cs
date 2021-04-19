using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for ZakaziTerminSekretar.xaml
    /// </summary>
    public partial class ZakaziTerminSekretar : Window
    {
        public ZakaziTerminSekretar()
        {
            InitializeComponent();
            foreach (Sala s in SaleMenadzer.sale)
            {
                prostorije.Items.Add(s.Id);
            }

            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                pacijenti.Items.Add(p.ImePacijenta + " " +  p.PrezimePacijenta + " " + p.IdPacijenta);
            }

            foreach (Lekar l in MainWindow.lekari)
            {
                lekari.Items.Add(l.ImeLek + " " + l.PrezimeLek + " " + l.IdLekara);
            }

            datum.BlackoutDates.AddDatesInPast();

            this.search.ItemsSource = MainWindow.lekari;
            //this.search.SelectedItem = izabraniTermin.Lekar.IdLekara; // TODO: ispraviti

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(search.ItemsSource);
            view.Filter = UserFilter;

        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as Lekar).PrezimeLek.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            int brojTermina = TerminMenadzer.GenerisanjeIdTermina();

            // vreme pocetka i kraja
            string vp = vremePocetka.Text;
            string vk = vremeKraja.Text;

            string[] pocetak = vp.Split(':');
            string pocetakSati = pocetak[0];
            string pocetakMinuti = pocetak[1];

            string[] kraj = vk.Split(':');
            string krajSati = kraj[0];
            string krajMinuti = kraj[1];

            int pogresnoVreme = 0;

            if (Convert.ToInt32(pocetakSati) > Convert.ToInt32(krajSati))
            {
                MessageBox.Show("Neispravno vreme pocetka i kraja");
                pogresnoVreme = 1;
            }
            else if (Convert.ToInt32(pocetakSati) == Convert.ToInt32(krajSati))
            {
                if (Convert.ToInt32(pocetakMinuti) >= Convert.ToInt32(krajMinuti))
                {
                    MessageBox.Show("Neispravno vreme pocetka i kraja");
                    pogresnoVreme = 1;
                }
            }

            // datum
            string dat = null;
            DateTime? selectedDate = datum.SelectedDate;
            Console.WriteLine(selectedDate);
            if (selectedDate.HasValue)
            {
                dat = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }

            // tip termina
            TipTermina tp;
            if (tip.Text.Equals("Pregled"))
            {
                 tp = TipTermina.Pregled;
            }
            else
            {
                 tp = TipTermina.Operacija;
            }

            // lekar
            string lek = lekari.Text;
            string[] podaciLekar = lek.Split(' ');
            Lekar l = MainWindow.PronadjiPoId(Int32.Parse(podaciLekar[2]));

            // pacijent
            string p = pacijenti.Text;
            string[] podaciPacijent = p.Split(' ');
            Pacijent pacijent = PacijentiMenadzer.PronadjiPoId(Int32.Parse(podaciPacijent[2]));

            // prostorija
            Sala s = SaleMenadzer.NadjiSaluPoId((int)prostorije.SelectedItem);  
            Termin t = new Termin(brojTermina, dat, vp, vk, tp, l, s, pacijent);

            int zauzeto = 0;
            if (pogresnoVreme == 0)
            {
                if (s.zauzetiTermini.Count != 0)        // ako postoje zauzeti termini
                {
                    foreach (ZauzeceSale zauzece in s.zauzetiTermini)
                    {
                        string[] zauzetiTerminPocetak = zauzece.pocetakTermina.Split(':');
                        string zauzetiTerminPocetakSati = zauzetiTerminPocetak[0];
                        string zauzetiTerminPocetakMinuti = zauzetiTerminPocetak[1];

                        string[] zauzetiTerminKraj = zauzece.krajTermina.Split(':');
                        string zauzetiTerminKrajSati = zauzetiTerminKraj[0];
                        string zauzetiTerminKrajMinuti = zauzetiTerminKraj[1];

                        if ( (t.Prostorija.Id.Equals(s.Id) && dat.Equals(zauzece.datumTermina) && Convert.ToInt32(pocetakSati) >= Convert.ToInt32(zauzetiTerminPocetakSati) && (Convert.ToInt32(pocetakSati) < Convert.ToInt32(zauzetiTerminKrajSati) || Convert.ToInt32(pocetakMinuti) < Convert.ToInt32(zauzetiTerminKrajMinuti))) || 
                            (t.Prostorija.Id.Equals(s.Id) && dat.Equals(zauzece.datumTermina) && (Convert.ToInt32(krajSati) > Convert.ToInt32(zauzetiTerminPocetakSati) || Convert.ToInt32(krajMinuti) > Convert.ToInt32(zauzetiTerminPocetakMinuti)) && Convert.ToInt32(krajSati) <= Convert.ToInt32(zauzetiTerminKrajSati) && Convert.ToInt32(pocetakSati) <= Convert.ToInt32(zauzetiTerminPocetakSati)) ||
                            (t.Prostorija.Id.Equals(s.Id) && dat.Equals(zauzece.datumTermina) && Convert.ToInt32(pocetakSati) <= Convert.ToInt32(zauzetiTerminPocetakSati) && Convert.ToInt32(krajSati) >= Convert.ToInt32(zauzetiTerminKrajSati)) && !Convert.ToInt32(krajSati).Equals(Convert.ToInt32(zauzetiTerminPocetakSati)) && !Convert.ToInt32(pocetakSati).Equals(Convert.ToInt32(zauzetiTerminKrajSati) ))
                        {
                            MessageBox.Show("Vec postoji termin");
                            vremePocetka.Text = "";
                            vremeKraja.Text = "";
                            zauzeto = 1;
                            break;
                        }
                    }

                    if (zauzeto == 0)
                    {
                        TerminMenadzer.ZakaziTerminSekretar(t);
                        ZauzeceSale z = new ZauzeceSale(vp, vk, dat, t.IdTermin);
                        s.zauzetiTermini.Add(z);

                        // za svaki termin koji je zakazan u istoj prostoriji s, dodati to novo zauzece u zauzeca te prostorije
                        foreach (Termin t1 in TerminMenadzer.termini)
                        {
                            if (t1.Prostorija.Id == s.Id)
                            {
                                t1.Prostorija = s;
                            }         
                        }

                        TerminMenadzer.sacuvajIzmene();
                        SaleMenadzer.sacuvajIzmjene();

                        this.Close();
                    }
                }
                else    // ako ne postoje zauzeti termini
                {
                    TerminMenadzer.ZakaziTerminSekretar(t);
                    ZauzeceSale z = new ZauzeceSale(vp, vk, dat, t.IdTermin);
                    s.zauzetiTermini.Add(z);

                    TerminMenadzer.sacuvajIzmene();
                    SaleMenadzer.sacuvajIzmjene();

                    this.Close();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // kreiranje guest naloga
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DodajPacijentaGuest dodavanje = new DodajPacijentaGuest(this);  // prosledjujemo u DodajPacijentaGuest konstruktor klase ZakaziTerminSekretar
            dodavanje.Show();
        }

        public void AzurirajComboBox()
        {
            pacijenti.Items.Clear();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                pacijenti.Items.Add(p.ImePacijenta + " " + p.PrezimePacijenta + " " + p.IdPacijenta);
            }
            int ukupno = PacijentiMenadzer.pacijenti.Count;
            pacijenti.SelectedIndex = ukupno - 1;
        }

        private void search_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (search.SelectedItems.Count > 0)
            {
                Lekar item = (Lekar)search.SelectedItems[0];
               // imePrz.Text = item.ToString();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(search.ItemsSource).Refresh();

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

        }
    }
}
