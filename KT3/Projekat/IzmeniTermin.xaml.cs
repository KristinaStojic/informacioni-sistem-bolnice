using Model;
using Projekat.Model;
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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmeniTermin.xaml
    /// </summary>
    public partial class IzmeniTermin : Page
    {
        public Termin termin;
        private Lekar idLek;
        private string stariDatum;
        public static int idPacijent;
        //
        public List<Sala> slobodneSale;
        public static ObservableCollection<string> sviSlobodni { get; set; }
        public static ObservableCollection<string> sviSlobodni2 { get; set; }
        public Sala _sala;
        public List<string> vremeSala;
        public int brSala;
        public static Pacijent prijavljeniPacijent;

        public IzmeniTermin(Termin izabraniTermin)
        {
            InitializeComponent();
            this.DataContext = this;
            sviSlobodni2 = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30",
                                                               "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30",
                                                               "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30",
                                                               "17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00"};
            idPacijent = izabraniTermin.Pacijent.IdPacijenta;
            datum.BlackoutDates.AddDatesInPast();
            CalendarDateRange cdr = new CalendarDateRange();
            cdr.Start = DateTime.Now.AddDays(3);
            cdr.End = DateTime.Now.AddDays(2000);
            datum.BlackoutDates.Add(cdr);
            this.datum.Text = izabraniTermin.Datum.ToString();

            this.termin = izabraniTermin;
            if (izabraniTermin != null)
            {
                TipTermina tp;
                if (izabraniTermin.tipTermina.Equals(TipTermina.Operacija))
                {
                    this.combo.SelectedIndex = 0;
                }
                else if (izabraniTermin.tipTermina.Equals(TipTermina.Pregled))
                {
                    this.combo.SelectedIndex = 1;
                }
                tp = izabraniTermin.tipTermina;
                this.imePrz.Text = izabraniTermin.Lekar.ImeLek + " " + izabraniTermin.Lekar.PrezimeLek;
                prijavljeniPacijent = PacijentiMenadzer.PronadjiPoId(idPacijent);
                idLek = izabraniTermin.Lekar;
                stariDatum = izabraniTermin.Datum;
                int brojac = 0;
                this.dgSearch.ItemsSource = MainWindow.lekari;
                foreach (Lekar lekar in MainWindow.lekari)
                {
                    brojac++;
                    if (lekar.IdLekara.Equals(izabraniTermin.Lekar.IdLekara))
                    {
                        this.dgSearch.SelectedIndex = brojac;
                        break;
                    }
                }
                this.vpp.SelectedItem = izabraniTermin.VremePocetka;
                

                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dgSearch.ItemsSource);
                view.Filter = UserFilter;

              
            }
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as Lekar).PrezimeLek.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0) || ((item as Lekar).ImeLek.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Lekar).specijalizacija.ToString().IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgSearch.ItemsSource).Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // btn odustani
            Page uvidZakazaniTermini = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvidZakazaniTermini);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // sacuvaj --> pomeri termin
            if (termin.Pomeren == false)
            {
                //dugme sacuvaj
                try
                {
                    //int brojTermina = TerminMenadzer.GenerisanjeIdTermina();
                    String formatted = null;
                    DateTime? selectedDate = datum.SelectedDate;
                    if (selectedDate.HasValue)
                    {
                        formatted = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    }
                    else
                    {
                        formatted = stariDatum;
                    }

                    String vp = vpp.Text;
                    String vk = ZakaziTermin.IzracunajVremeKraja(vp);

                    TipTermina tp;
                    if (combo.Text.Equals("Pregled"))
                    {
                        tp = TipTermina.Pregled;
                    }
                    else
                    {
                        tp = TipTermina.Operacija;
                    }
                    Termin t = new Termin(termin.IdTermin, formatted, vp, vk, tp);
                    // TODO: promeniti ovo na id pacijenta koji je prijavljen
                    t.Pacijent = prijavljeniPacijent;
                    t.Pomeren = true;
                    ZauzeceSale zs = new ZauzeceSale(vp, vk, formatted, t.IdTermin);
                    _sala.zauzetiTermini.Add(zs);
                    t.Prostorija = _sala;
                    //SaleMenadzer.sacuvajIzmjene();

                    if (dgSearch.SelectedItems.Count > 0)
                    {
                        Lekar selLekar = (Lekar)dgSearch.SelectedItem;
                        t.Lekar = selLekar;
                    }
                    /*else
                    {
                        // TODO: optimizovati!
                        t.Lekar = idLek;
                    }*/
                    TerminMenadzer.IzmeniTermin(termin, t);
                    Page uvidZakazaniTermini = new ZakazaniTerminiPacijent(idPacijent);
                    this.NavigationService.Navigate(uvidZakazaniTermini);
                }
                catch (System.Exception)
                {
                    MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Nemoguce je pomeriti ovaj termin jer je vec jednom pomeren", "Upozorenje");
            }
        }

        private void lvWithSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void vpp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selektDatum = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string vreme = vpp.SelectedValue.ToString();

            foreach (Sala s in slobodneSale)
            {
                if (!s.zauzetiTermini.Exists(x => x.pocetakTermina.Equals(vreme))) // ako ne postoji
                {
                    _sala = SaleMenadzer.NadjiSaluPoId(s.Id);
                    break;
                }
            }
            if (_sala == null)
            {
                MessageBox.Show("Ne postoji slobodna sala za odabrani datum i vreme");
            }
            if (slobodneSale == null)
            {
                MessageBox.Show("Ne postoji slobodna sale");
            }

        }

        private void dgSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgSearch.SelectedItems.Count > 0)
            {
                Lekar item = (Lekar)dgSearch.SelectedItems[0];
                imePrz.Text = item.ToString();
            }
        }

        private void imePrz_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {


        }

        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /* tip termina*/
            slobodneSale = new List<Sala>();
            string tip = combo.SelectedValue.ToString().Split(' ')[1];
            brSala = 0;
            //this.datum.Text = "";
            //this.vpp.ItemsSource = null;

            if (tip.Equals("Operacija"))
            {
                foreach (Sala s in SaleMenadzer.sale)
                {
                    if (s.TipSale.Equals(tipSale.OperacionaSala))
                    {
                        slobodneSale.Add(s);
                        brSala++;
                        //  MessageBox.Show(s.Id.ToString());
                    }
                }
            }
            else
            {
                foreach (Sala s in SaleMenadzer.sale)
                {
                    if (s.TipSale.Equals(tipSale.SalaZaPregled))
                    {
                        slobodneSale.Add(s);
                        brSala++;
                        // MessageBox.Show(s.Id.ToString() + " " + brSala);
                    }
                }
            }
        }

        private void datum_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            sviSlobodni = new ObservableCollection<string>() { "07:00", "07:30", "08:00", "08:30",
                                                               "09:00", "09:30",  "10:00", "10:30",
                                                               "11:00", "11:30", "12:00", "12:30",
                                                               "13:00", "13:30", "14:00", "14:30",
                                                               "15:00", "15:30", "16:00", "16:30",
                                                               "17:00", "17:30", "18:00", "18:30",
                                                               "19:00", "19:30", "20:00"};
            //this.vpp.ItemsSource = null;
            string selectDatum = datum.SelectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            int i = 0;
            if (datum.SelectedDate == DateTime.Now.Date)
            {
                // MessageBox.Show("Odabrali ste danasnji dan");
                foreach (string slot in sviSlobodni2)
                {
                    DateTime dt = DateTime.Parse(slot);
                    DateTime sada = DateTime.Now;
                    if (dt.TimeOfDay <= sada.TimeOfDay)
                    {
                        //MessageBox.Show(dt.TimeOfDay.ToString() + " " + sada.TimeOfDay.ToString());
                        sviSlobodni.Remove(slot);
                    }

                }
            }

            if (slobodneSale == null)
            {
                MessageBox.Show("Prvo izberite tip termina", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                foreach (Sala s in slobodneSale)
                {
                    foreach (ZauzeceSale zs in s.zauzetiTermini)
                    {
                        if (zs.datumPocetkaTermina.Equals(selectDatum))
                        {
                            switch (zs.pocetakTermina)
                            {
                                case "07:00":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("07:00");
                                            // MessageBox.Show("Postoji zauzeti termin u sali " + s.brojSale + ": " + zs.datumTermina + " " + zs.pocetakTermina);
                                        }
                                        break;
                                    }
                                case "07:30":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("07:30");
                                        }
                                        break;
                                    }
                                case "08:00":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("08:00");
                                        }
                                        break;
                                    }
                                case "08:30":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("08:30");
                                        }
                                        break;
                                    }
                                case "09:00":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("09:00");
                                        }
                                        break;
                                    }
                                case "09:30":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("09:30");
                                        }
                                        break;
                                    }
                                case "10:00":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("10:00");
                                        }
                                        break;
                                    }
                                case "10:30":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("10:30");
                                        }
                                        break;
                                    }
                                case "11:00":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("11:00");
                                        }
                                        break;
                                    }
                                case "11:30":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("11:30");
                                        }
                                        break;
                                    }
                                case "12:00":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("12:00");
                                        }
                                        break;
                                    }
                                case "12:30":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("12:30");
                                        }
                                        break;
                                    }
                                case "13:00":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("13:00");
                                        }
                                        break;
                                    }
                                case "13:30":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("13:30");
                                        }
                                        break;
                                    }
                                case "14:00":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("14:00");
                                        }
                                        break;
                                    }
                                case "14:30":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("14:30");
                                        }
                                        break;
                                    }
                                case "15:00":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("15:00");
                                        }
                                        break;
                                    }
                                case "15:30":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("15:30");
                                        }
                                        break;
                                    }
                                case "16:00":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("16:00");
                                        }
                                        break;
                                    }
                                case "16:30":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("16:30");
                                        }
                                        break;
                                    }
                                case "17:00":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("17:00");
                                        }
                                        break;
                                    }
                                case "17:30":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("17:30");
                                        }
                                        break;
                                    }
                                case "18:00":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("18:00");
                                        }
                                        break;
                                    }
                                case "18:30":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("18:30");
                                        }
                                        break;
                                    }
                                case "19:00":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("19:00");
                                        }
                                        break;
                                    }
                                case "19:30":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("19:30");
                                        }
                                        break;
                                    }
                                case "20:00":
                                    {
                                        i++;
                                        if (i >= brSala)
                                        {
                                            sviSlobodni.Remove("20:00");
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Greska");
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            this.vpp.ItemsSource = sviSlobodni;
            if (sviSlobodni.Any() == false)
            {
                MessageBox.Show("Ne postoji nijedan slobodan temrin za izabrani datum", "Izaberite drugi datum");
            }
        }
        private void odjava_Click(object sender, RoutedEventArgs e)
        {
            Page odjava = new PrijavaPacijent();
            this.NavigationService.Navigate(odjava);
        }

        public void karton_Click(object sender, RoutedEventArgs e)
        {
            Page karton = new ZdravstveniKartonPacijent(idPacijent);
            this.NavigationService.Navigate(karton);
        }

        public void zakazi_Click(object sender, RoutedEventArgs e)
        {
            Page zakaziTermin = new ZakaziTermin(idPacijent);
            this.NavigationService.Navigate(zakaziTermin);
        }

        public void uvid_Click(object sender, RoutedEventArgs e)
        {
            Page uvid = new ZakazaniTerminiPacijent(idPacijent);
            this.NavigationService.Navigate(uvid);
        }

        private void pocetna_Click(object sender, RoutedEventArgs e)
        {
            Page pocetna = new PrikaziTermin(idPacijent);
            this.NavigationService.Navigate(pocetna);
        }
    }
}
