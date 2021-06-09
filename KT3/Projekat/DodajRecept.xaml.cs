
using Model;
using Projekat.Model;
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
    /// Interaction logic for DodajRecept.xaml
    /// </summary>
    public partial class DodajRecept : Window
    {
        Pacijent pacijent;
        Termin termin;
        public bool flagDani = false;
        public bool flagKolicina = false;
        public bool flagSati = false;
        public bool flagMinuti = false;
        public bool flagLek = false;
        public DodajRecept(Pacijent izabraniPacijent, Termin izabraniTermin)
        {
            InitializeComponent();
            this.pacijent = izabraniPacijent;
            this.termin = izabraniTermin;
            potvrdi.IsEnabled = false;
            PopuniPodatkePacijenta(izabraniPacijent);
            validacijaDani.Visibility = Visibility.Hidden;
            validacijaKolicina.Visibility = Visibility.Hidden;
            validacijaMinuti.Visibility = Visibility.Hidden;
            validacijaSati.Visibility = Visibility.Hidden;

        }

        private void PopuniPodatkePacijenta(Pacijent izabraniPacijent)
        {
            
            this.nadjiLek.ItemsSource = ZdravstveniKartonServis.NadjiPacijentuDozvoljeneLekove(pacijent.IdPacijenta);
            this.pacijentIme.Text = izabraniPacijent.ImePacijenta + " " + izabraniPacijent.PrezimePacijenta;
            this.jmbg.Text = izabraniPacijent.Jmbg.ToString();
            this.lekar.Text = termin.Lekar.ImeLek + " " + termin.Lekar.PrezimeLek;
            datum.SelectedDate = DateTime.Parse(termin.Datum);


            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(nadjiLek.ItemsSource);
            view.Filter = UserFilter;
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nadjiLek.SelectedItems.Count > 0)
            {
                Lek item = (Lek)nadjiLek.SelectedItems[0];
                nazivSifra.Text = item.nazivLeka;
            }
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(pretraga.Text))
                return true;
            else
                return ((item as Lek).nazivLeka.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void pretraga_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(nadjiLek.ItemsSource).Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int brojRecepta = ZdravstveniKartonServis.GenerisanjeIdRecepta(pacijent.IdPacijenta);
                String nazivLeka = nazivSifra.Text;
                
                int kolicinaNaDan = int.Parse(kolicina.Text);
                int kolikoDana = int.Parse(dani.Text);
                String pocetakKoriscenja = sati.Text + ":" + min.Text;

                String datumPregleda = null;
                DateTime? selectedDate = datum.SelectedDate;
                if (selectedDate.HasValue)
                {
                    datumPregleda = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                }

                /* Sanja */
                List<DateTime> uzimanjeTerapije = GenerisiUzimanjeTerapije(datumPregleda, kolikoDana, kolicinaNaDan);
                /*-----------------------------*/

                LekarskiRecept recept = new LekarskiRecept(pacijent, brojRecepta, nazivLeka, datumPregleda, kolikoDana, kolicinaNaDan, pocetakKoriscenja, uzimanjeTerapije);
                recept.IdLekara = termin.Lekar.IdLekara;
                ZdravstveniKartonServis.DodajRecept(recept);

                PosaljiObavestenjeOTerapiji(recept);
                SacuvajIzmene();

                this.Close();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SacuvajIzmene()
        {
            TerminServis.sacuvajIzmene();
            PacijentiServis.SacuvajIzmenePacijenta();
            SaleServis.sacuvajIzmjene();
            ObavestenjaServis.sacuvajIzmene();
        }

        private List<DateTime> GenerisiUzimanjeTerapije(string datumPregleda,int kolicinaNaDan, int kolikoDana)
        {
            List<DateTime> uzimanjeTerapije = new List<DateTime>();
            int x = 24 / kolicinaNaDan;
            int ukupno = kolicinaNaDan * kolikoDana;

            int godina = int.Parse(datumPregleda.Substring(6));
            int mesec = int.Parse(datumPregleda.Substring(0, 2));
            int dan = int.Parse(datumPregleda.Substring(3, 2));
            int sat = int.Parse(sati.Text);
            int mm = int.Parse(min.Text);
            DateTime datumVreme = new DateTime(godina, mesec, dan, sat, mm, 00);

            for (int i = 0; i <= ukupno; i++)
            {
                DateTime dt = datumVreme.AddHours(i * x);
                Console.WriteLine(dt.ToString()); ;
                uzimanjeTerapije.Add(dt);
            }
            return uzimanjeTerapije;
        }

        private void PosaljiObavestenjeOTerapiji(LekarskiRecept recept)
        {
            foreach (DateTime dt in recept.UzimanjeTerapije)
            {
                List<int> lista = new List<int>();
                lista.Add(pacijent.IdPacijenta);
                int idObavestenja = ObavestenjaServis.GenerisanjeIdObavestenja();
                Obavestenja ob = new Obavestenja(idObavestenja, dt.ToString("MM/dd/yyyy HH:mm"), "Terapija", "Uzmite terapiju: " + recept.NazivLeka, lista, true);  // dodat flag da je notifikacija
                ObavestenjaMenadzer.NadjiSve().Add(ob);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl)) //Sacuvaj
            {
                Button_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) //Nazad
            {
                Button_Click_1(sender, e);
            }
        }

        private void dani_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flagDani = false;
                potvrdi.IsEnabled = false;

            }
            else
            {
                long result;
                if (long.TryParse(dani.Text, out result))
                {
                    if (dani.Text.Length >= 0 & dani.Text.Length <= 10)
                    {
                        validacijaDani.Visibility = Visibility.Hidden;
                        flagDani = true;
                        if (flagDani == true && flagKolicina == true && flagMinuti == true && flagSati == true && flagLek == true)
                            
                        {
                            potvrdi.IsEnabled = true;
                        }
                    }
                    else
                    {
                        flagDani = false;
                        validacijaDani.Visibility = Visibility.Visible;
                        potvrdi.IsEnabled = false;
                    }
                }
                else
                {
                    flagDani = false;
                    validacijaDani.Visibility = Visibility.Visible;
                    potvrdi.IsEnabled = false;
                }
            }
        }

        private void kolicina_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flagKolicina = false;
                potvrdi.IsEnabled = false;

            }
            else
            {
                long result;
                if (long.TryParse(kolicina.Text, out result))
                {
                    int kol = Int32.Parse(kolicina.Text);
                    if (kol >= 0 & kol <= 10)
                    {
                        validacijaKolicina.Visibility = Visibility.Hidden;
                        flagKolicina = true;
                        if (flagDani == true && flagKolicina == true && flagMinuti == true && flagSati == true && flagLek == true)

                        {
                            potvrdi.IsEnabled = true;
                        }
                    }
                    else
                    {
                        flagKolicina = false;
                        validacijaKolicina.Visibility = Visibility.Visible;
                        potvrdi.IsEnabled = false;
                    }
                }
                else
                {
                    flagKolicina = false;
                    validacijaKolicina.Visibility = Visibility.Visible;
                    potvrdi.IsEnabled = false;
                }
            }
        }

        
        private void sati_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flagKolicina = false;
                potvrdi.IsEnabled = false;

            }
            else
            {
                long result;
                if (long.TryParse(sati.Text, out result))
                {
                    int sat = Int32.Parse(sati.Text);
                    if (sat >= 0 & sat <= 24)
                    {
                        validacijaSati.Visibility = Visibility.Hidden;
                        flagSati = true;
                        if (flagDani == true && flagKolicina == true && flagMinuti == true && flagSati == true && flagLek == true)

                        {
                            potvrdi.IsEnabled = true;
                        }
                    }
                    else
                    {
                        flagSati = false;
                        validacijaSati.Visibility = Visibility.Visible;
                        potvrdi.IsEnabled = false;
                    }
                }
                else
                {
                    flagSati = false;
                    validacijaSati.Visibility = Visibility.Visible;
                    potvrdi.IsEnabled = false;
                }
            }
        }

        private void min_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flagMinuti = false;
                potvrdi.IsEnabled = false;

            }
            else
            {
                long result;
                if (long.TryParse(min.Text, out result))
                {
                    int minut = Int32.Parse(min.Text);
                    if (minut >= 0 & minut <= 60)
                    {
                        validacijaMinuti.Visibility = Visibility.Hidden;
                        flagMinuti = true;
                        if (flagDani == true && flagKolicina == true && flagMinuti == true && flagSati == true && flagLek == true)
                        {
                            potvrdi.IsEnabled = true;
                        }
                    }
                    else
                    {
                        flagMinuti = false;
                        validacijaMinuti.Visibility = Visibility.Visible;
                        potvrdi.IsEnabled = false;
                    }
                }
                else
                {
                    flagMinuti = false;
                    validacijaMinuti.Visibility = Visibility.Visible;
                    potvrdi.IsEnabled = false;
                }
            }
        }

        private void nazivSifra_TextChanged(object sender, TextChangedEventArgs e)
        {
            flagLek = true;
            if (flagDani == true && flagKolicina == true && flagMinuti == true && flagSati == true && flagLek == true)
            {
                potvrdi.IsEnabled = true;
            }
        }
    }
}
