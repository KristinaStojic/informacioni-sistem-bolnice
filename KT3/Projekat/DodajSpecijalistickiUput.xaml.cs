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
    /// Interaction logic for DodajSpecijalistickiUput.xaml
    /// </summary>
    public partial class DodajSpecijalistickiUput : Window
    {
        Pacijent pacijent;
        Termin termin;
        public string formatiranDatum;
        public Sala Soba;
        public Krevet Krevet;
        public bool flagNapomenaSpec = false;
        public bool flagLekarSpec = false;
        public bool popunjeno = false;

        public bool flagNapomenaBolnicko = false;
        public bool flagPoc = false;
        public bool flagKraj = false;
        public bool popunjenoBolnicko = false;

        public bool popunjenoLab = false;
        public DodajSpecijalistickiUput(Pacijent izabraniPacijent, Termin izabraniTermin)
        {
            InitializeComponent();
            this.pacijent = izabraniPacijent;
            this.termin = izabraniTermin;
            this.potvrdiBolnicko.IsEnabled = false;
            this.potvrdiLab.IsEnabled = false;
            this.potvrdiSpec.IsEnabled = false;
            this.potvrdiLab.IsEnabled = false;
            PopuniPodatkePacijentaZaSpecijalistickiUput();
            PopuniPodatkePacijentaZaBolnickoLecenje();
            PopuniPodatkeLaboratorijskiUput();

        }

        private void PopuniPodatkeLaboratorijskiUput()
        {
            this.imeLab.Text = pacijent.ImePacijenta;
            this.prezimeLab.Text = pacijent.PrezimePacijenta;
            jmbgLab.Text = pacijent.Jmbg.ToString();
            lekarLab.Text = termin.imePrezimeLekara;
            datumLab.SelectedDate = DateTime.Parse(termin.Datum);
        }
        private void PopuniPodatkePacijentaZaSpecijalistickiUput()
        {
            this.listaLekara.ItemsSource = LekariMenadzer.lekari;
            this.ime.Text = pacijent.ImePacijenta;
            this.prezime.Text = pacijent.PrezimePacijenta;
            this.jmbg.Text = pacijent.Jmbg.ToString();
            this.lekar.Text = termin.Lekar.ImeLek + " " + termin.Lekar.PrezimeLek;
            datum.SelectedDate = DateTime.Parse(termin.Datum);
            specijalistickiTab.IsSelected = true;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listaLekara.ItemsSource);
            view.Filter = UserFilter;
        }

        private void PopuniPodatkePacijentaZaBolnickoLecenje()
        {
            this.imePacijenta.Text = pacijent.ImePacijenta;
            this.prezimePacijenta.Text = pacijent.PrezimePacijenta;
            this.jmbgPacijenta.Text = pacijent.Jmbg.ToString();
            this.Lekar.Text = termin.Lekar.ImeLek + " " + termin.Lekar.PrezimeLek;
            /*provjeriti da li moze ovo*/
            this.datumKraja.SelectedDate = DateTime.Today;
            this.datumPocetka.SelectedDate = DateTime.Today;


        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaLekara.SelectedItems.Count > 0)
            {
                Lekar item = (Lekar)listaLekara.SelectedItems[0];
                specijalista.Text = item.ImeLek + " " + item.PrezimeLek + " " + item.specijalizacija;
            }
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(pretraga.Text))
                return true;
            else
                return ((item as Lekar).PrezimeLek.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Lekar).ImeLek.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Lekar).specijalizacija.ToString().IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void pretraga_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listaLekara.ItemsSource).Refresh();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idUputa = ZdravstveniKartonServis.GenerisanjeIdUputa(pacijent.IdPacijenta);
                String detaljiOPregledu = napomena.Text;
                int idSpecijaliste = NadjiIDSpecijaliste();
                string datum = NadjiDatum();
                tipUputa tipUputa = NadjiTipUputa();

                Uput noviUput = new Uput(idUputa, pacijent.IdPacijenta, termin.Lekar.IdLekara, idSpecijaliste, detaljiOPregledu, datum, tipUputa);
                ZdravstveniKartonServis.DodajUput(noviUput);

                TerminServisLekar.sacuvajIzmene();
                PacijentiServis.SacuvajIzmenePacijenta();

                this.Close();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int NadjiIDSpecijaliste()
        {
            Lekar lekar = (Lekar)listaLekara.SelectedItem;
            return lekar.IdLekara;
        }

        private string NadjiDatum()
        {
            String formatiranDatum = null;
            DateTime? selectedDate = datum.SelectedDate;
            if (selectedDate.HasValue)
            {
                formatiranDatum = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            }
            return formatiranDatum;
        }
        private tipUputa NadjiTipUputa()
        {
            tipUputa tipUputa = tipUputa.Laboratorija;
            string selektovaniTab = (string)(uputi.SelectedItem as TabItem).Header;
            if (selektovaniTab.Equals("Specijalistički pregled"))
            {
                tipUputa = tipUputa.SpecijallistickiPregled;
            }
            else if (selektovaniTab.Equals("Laboratorija"))
            {
                tipUputa = tipUputa.Laboratorija;
            }
            else if (selektovaniTab.Equals("Bolničko lečenje"))
            {
                tipUputa = tipUputa.StacionarnoLecenje;
            }

            return tipUputa;
        }
        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl)) //Sacuvaj
            {
                Potvrdi_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) //Nazad
            {
                Odustani_Click(sender, e);
            }
        }

        private void brojSobe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (slobodneSobe.SelectedItem != null)
            {
                Soba = SaleServis.NadjiSaluPoId((int)slobodneSobe.SelectedItem);
            }
            NadjiSlobodneKrevete(Soba);
        }

        private void NadjiSlobodneKrevete(Sala Soba)
        {
            slobodniKreveti.Items.Clear();
            foreach (Krevet k in Soba.Kreveti)
            {
                if (!k.Zauzet)
                {
                    slobodniKreveti.Items.Add(k.IdKreveta);
                }
            }
        }

        private void datum_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = datumPocetka.SelectedDate;
            if (selectedDate.HasValue)
            {
                formatiranDatum = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }

            nadjiSlobodneSobe();

            postaviDugmeBolnicko();
        }

        private void postaviDugmeBolnicko()
        {
            if (this.datumPocetka.Text != null || this.datumKraja.Text != null || this.napomenaPregelda.Text != null)
            {
                izvrsiPostavljanjeBolnicko();
            }
            else
            {
                this.potvrdiBolnicko.IsEnabled = false;
            }
        }
        private void izvrsiPostavljanjeBolnicko()
        {
            if (this.datumPocetka.Text.Trim().Equals("") || this.datumKraja.Text.Trim().Equals("") || this.napomenaPregelda.Text.Trim().Equals(""))
            {
                this.potvrdiBolnicko.IsEnabled = false;
            }
            else if (!this.datumPocetka.Text.Trim().Equals("") && !this.datumPocetka.Text.Trim().Equals("") && !this.napomenaPregelda.Text.Trim().Equals(""))
            {
                this.potvrdiBolnicko.IsEnabled = true;
                popunjeno = true;
            }
        }

        private void nadjiSlobodneSobe()
        {
            slobodneSobe.Items.Clear();
            foreach (Sala sala in SaleServis.Sale())
            {
                if (sala.TipSale.Equals(tipSale.SalaZaLezanje))  
                {
                    /*DODATI PROVJERU DA NADJE SLOBODNE KREVETE*/
                    slobodneSobe.Items.Add(sala.Id);
                    
                }
            }

        }

        private void PotvrdiLecenje_Click(object sender, RoutedEventArgs e)
        {
            DateTime kraj = (DateTime)this.datumKraja.SelectedDate;
            DateTime pocetak = (DateTime)this.datumPocetka.SelectedDate;

            if (pocetak >= kraj || pocetak < DateTime.Now.Date)
            {
                MessageBox.Show("Niste uneli ispravne datume!");
            }
            else if (popunjeno == true)
            {
                int idUputa = ZdravstveniKartonServis.GenerisanjeIdUputa(pacijent.IdPacijenta);
                String detaljiOPregledu = napomenaPregelda.Text;
                string datumPocetka = NadjiDatumPocetkaLecenja();
                string datumKraja = NadjiDatumKrajaLecenja();
                tipUputa tipUputa = NadjiTipUputa();
                Soba = SaleServis.NadjiSaluPoId((int)slobodneSobe.SelectedItem);
                Krevet = SaleServis.NadjiKrevetPoId((int)slobodniKreveti.SelectedItem, Soba);
                Uput noviUput = new Uput(idUputa, pacijent.IdPacijenta, termin.Lekar.IdLekara,Soba.Id, Krevet.IdKreveta, datumKraja, datumPocetka, termin.Datum, detaljiOPregledu, tipUputa);
                zauzmiKrevet(Soba, Krevet);
                ZdravstveniKartonServis.DodajUput(noviUput);
                
                TerminServisLekar.sacuvajIzmene();
                PacijentiServis.SacuvajIzmenePacijenta();
                SaleServis.sacuvajIzmjene();
                this.Close();
            }
            /*else if (pocetak >= kraj || pocetak < DateTime.Now.Date)
            {
                MessageBox.Show("Niste uneli ispravne datume!");
            }*/
            else
            {
                MessageBox.Show("Popunite sva polja!");
            }
        }

        private void zauzmiKrevet(Sala soba,Krevet krevet)
        {
            foreach(Krevet k in soba.Kreveti)
            {
                if(k.IdKreveta == krevet.IdKreveta)
                {
                    k.Zauzet = true;
                    k.PocetakZauzeca = (DateTime)datumPocetka.SelectedDate;
                    k.KrajZauzeca = (DateTime) datumKraja.SelectedDate;
                }
            }
        }
        private string NadjiDatumPocetkaLecenja()
        {
            String formatiranDatum = null;
            DateTime? selectedDate = datumPocetka.SelectedDate;
            if (selectedDate.HasValue)
            {
                formatiranDatum = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            }
            return formatiranDatum;
        }
        private string NadjiDatumKrajaLecenja()
        {
            String formatiranDatum = null;
            DateTime? selectedDate = datumKraja.SelectedDate;
            if (selectedDate.HasValue)
            {
                formatiranDatum = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            }
            return formatiranDatum;
        }

        private void Potvrdi_Laboratorija_Click(object sender, RoutedEventArgs e)
        {
            if (popunjenoLab)
            {
                int idUputa = ZdravstveniKartonServis.GenerisanjeIdUputa(pacijent.IdPacijenta);
                String detaljiOPregledu = napomenaLab.Text;
                string datum = NadjiDatum();
                tipUputa tipUputa = NadjiTipUputa();

                Uput noviUput = new Uput(idUputa, pacijent.IdPacijenta, termin.Lekar.IdLekara, tipUputa);
                noviUput.datumIzdavanja = datum;
                ZdravstveniKartonServis.DodajUput(noviUput);
               
                TerminServisLekar.sacuvajIzmene();
                PacijentiServis.SacuvajIzmenePacijenta();

                this.Close();
            }
        }

        private void postaviDugmeLab()
        {
            if (this.napomenaLab.Text != null)
            {
                izvrsiPostavljanjeLab();
            }
            else
            {
                this.potvrdiLab.IsEnabled = false;
            }
        }
        private void izvrsiPostavljanjeLab()
        {
            if (this.napomenaLab.Text.Trim().Equals(""))
            {
                this.potvrdiLab.IsEnabled = false;
            }
            else if (!this.napomenaLab.Text.Trim().Equals(""))
            {
                this.potvrdiLab.IsEnabled = true;
                popunjenoLab = true;
            }
        }

        private void Tabovi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.L && Keyboard.IsKeyDown(Key.LeftCtrl)) //laboratorija
            {
                laboratorija.IsSelected = true;
            }
            else if (e.Key == Key.B && Keyboard.IsKeyDown(Key.LeftCtrl)) //bolnicko lecenje
            {
                bolnickoLecenjeTab.IsSelected = true;
            }
            else if (e.Key == Key.P && Keyboard.IsKeyDown(Key.LeftCtrl)) //specijalisticki pregled
            {
                specijalistickiTab.IsSelected = true;
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) // nazad
            {
                this.Close();
            }
        }

        private void Bolnicko_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl)) //Sacuvaj
            {
                PotvrdiLecenje_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) //Nazad
            {
                Odustani_Click(sender, e);
            }
        }

        
            
        private void datumKraja_PreviewKeyDown(object sender, KeyEventArgs e)
        {
           /* var dp = sender as DatePicker;
            if (dp == null) return;
            if (!dp.SelectedDate.HasValue) return;

            var date = dp.SelectedDate.Value;
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                dp.SetValue(DatePicker.SelectedDateProperty, date.AddDays(1));
            }
            else if (e.Key == Key.Down)
            {
                e.Handled = true;
                dp.SetValue(DatePicker.SelectedDateProperty, date.AddDays(-1));
            }*/
        }

        private void laboratorija_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void lekar_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void napomena_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void postaviDugme()
        {
            if (this.specijalista.Text != null || this.napomena.Text != null)
            {
                izvrsiPostavljanje();
            }
            else
            {
                this.potvrdiSpec.IsEnabled = false;
            }
        }
        private void izvrsiPostavljanje()
        {
            if (this.specijalista.Text.Trim().Equals("") || this.napomena.Text.Trim().Equals(""))
            {
                this.potvrdiSpec.IsEnabled = false;
            }
            else if (!this.specijalista.Text.Trim().Equals("") && !this.napomena.Text.Trim().Equals(""))
            {
                this.potvrdiSpec.IsEnabled = true;
                popunjeno = true;
            }
        }

        private void datumKraja_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            postaviDugmeBolnicko();
        }

        private void napomenaPregelda_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugmeBolnicko();
        }

        private void napomenaLab_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugmeLab();
        }
    }
}
