using Model;
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
    /// Interaction logic for DodajRecept.xaml
    /// </summary>
    public partial class DodajRecept : Window
    {
        Pacijent pacijent;
        Termin termin;
        public DodajRecept(Pacijent izabraniPacijent, Termin izabraniTermin)
        {
            InitializeComponent();
            this.pacijent = izabraniPacijent;
            this.termin = izabraniTermin;
            this.nadjiLek.ItemsSource = ZdravstveniKartonMenadzer.NadjiPacijentuDozvoljeneLekove(pacijent.IdPacijenta);
            this.pacijentIme.Text = izabraniPacijent.ImePacijenta + " " + izabraniPacijent.PrezimePacijenta;
            this.jmbg.Text = izabraniPacijent.Jmbg.ToString();
            this.lekar.Text = izabraniTermin.Lekar.ImeLek + " " + izabraniTermin.Lekar.PrezimeLek;
            datum.SelectedDate = DateTime.Parse(izabraniTermin.Datum);


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
                int brojRecepta = ZdravstveniKartonMenadzer.GenerisanjeIdRecepta(pacijent.IdPacijenta);
                String nazivLeka = nazivSifra.Text;
                
                int kolicinaNaDan = int.Parse(kolicina.Text);
                int kolikoDana = int.Parse(dani.Text);
                String pocetakKoriscenja = sati.Text + ":" + min.Text;

                /* Sanja */
                String formatirano = null;
                DateTime? selectedDate = datum.SelectedDate;
                if (selectedDate.HasValue)
                {
                    formatirano = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                }
                List<DateTime> uzimanjeTerapije = new List<DateTime>();
                int x = 24 / kolicinaNaDan;
                int ukupno = kolicinaNaDan * kolikoDana;

                int godina = int.Parse(formatirano.Substring(6));
                int mesec = int.Parse(formatirano.Substring(0, 2));
                int dan = int.Parse(formatirano.Substring(3, 2));
                int sat = int.Parse(sati.Text);
                int mm = int.Parse(min.Text);
                DateTime datumVreme = new DateTime(godina, mesec, dan, sat, mm, 00);

                for (int i = 0; i <= ukupno; i++)
                {
                    DateTime dt = datumVreme.AddHours(i * x);
                    Console.WriteLine(dt.ToString()); ;
                    uzimanjeTerapije.Add(dt);
                }
                /* --- */

                LekarskiRecept recept = new LekarskiRecept(pacijent, brojRecepta, nazivLeka, formatirano, kolikoDana, kolicinaNaDan, pocetakKoriscenja, uzimanjeTerapije);
                ZdravstveniKartonMenadzer.DodajRecept(recept);

                foreach (DateTime dt in recept.UzimanjeTerapije)
                {
                    int idObavestenja = ObavestenjaMenadzer.GenerisanjeIdObavestenja();
                    Obavestenja ob = new Obavestenja(idObavestenja, dt.ToString("MM/dd/yyyy HH:mm"), "Terapija", "Uzmite terapiju: " + recept.NazivLeka, true);  // dodat flag da je notifikacija
                    ObavestenjaMenadzer.obavestenja.Add(ob);
                }

                TerminMenadzer.sacuvajIzmene();
                PacijentiMenadzer.SacuvajIzmenePacijenta();
                SaleMenadzer.sacuvajIzmjene();
                ObavestenjaMenadzer.sacuvajIzmene();

                this.Close();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
