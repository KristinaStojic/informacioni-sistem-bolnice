using System;
using System.Collections.Generic;
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
    /// Interaction logic for Recept.xaml
    /// </summary>
    public partial class Recept : Window
    {
        public Pacijent pacijent;
        public LekarskiRecept lekRec;
        public Recept(Pacijent izabraniPacijent)
        {
            InitializeComponent();
            this.pacijent = izabraniPacijent;

            ime.Text = izabraniPacijent.ImePacijenta;
            prezime.Text = izabraniPacijent.PrezimePacijenta;
            id.Text = izabraniPacijent.IdPacijenta.ToString();
        }

        // Sanja
        public Recept(LekarskiRecept lp, Pacijent izabraniPacijent)
        {
            InitializeComponent();
            this.lekRec = lp;
            this.naziv.Text = lp.NazivLeka;
            this.datum.Text = lp.DatumPropisivanjaLeka;
            this.dani.Text = lp.BrojDanaKoriscenja.ToString();
            this.brojUzimanja.Text = lp.BrojDanaKoriscenja.ToString();
            this.sati.Text = lp.PocetakKoriscenja.Substring(0, 2);
            this.min.Text = lp.PocetakKoriscenja.Substring(3);

            this.naziv.IsEnabled = false;
            this.datum.IsEnabled = false;
            this.dani.IsEnabled = false;
            this.brojUzimanja.IsEnabled = false;
            this.sati.IsEnabled = false;
            this.min.IsEnabled = false;
            this.sacuvaj.Visibility = Visibility.Hidden;

            this.pacijent = izabraniPacijent;
            ime.Text = izabraniPacijent.ImePacijenta;
            prezime.Text = izabraniPacijent.PrezimePacijenta;
            id.Text = izabraniPacijent.IdPacijenta.ToString();

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                int brojRecepta = ZdravstveniKartonMenadzer.GenerisanjeIdRecepta(pacijent.IdPacijenta);
                String nazivLeka = naziv.Text;
                String formatirano = null;
                DateTime? selectedDate = datum.SelectedDate;
                //Console.WriteLine(selectedDate);
                if (selectedDate.HasValue)
                {
                    formatirano = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                }
                int kolicinaNaDan = int.Parse(brojUzimanja.Text);
                int kolikoDana = int.Parse(dani.Text);
                String pocetakKoriscenja = sati.Text + ":" + min.Text;


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
                    DateTime dt = datumVreme.AddHours(i*x);
                    Console.WriteLine(dt.ToString()); ;
                    uzimanjeTerapije.Add(dt);
                }


                LekarskiRecept recept = new LekarskiRecept(pacijent, brojRecepta, nazivLeka, formatirano, kolikoDana, kolicinaNaDan, pocetakKoriscenja, uzimanjeTerapije);

                foreach(DateTime dt in recept.UzimanjeTerapije)
                {
                    Obavestenja ob = new Obavestenja(dt.ToString(), "Terapija", "Uzmite terapiju: " + recept.NazivLeka);
                    ObavestenjaMenadzer.obavestenja.Add(ob);
                }
                //LekarskiRecept recept = new LekarskiRecept(pacijent, brojRecepta, nazivLeka, formatirano, kolikoDana, kolicinaNaDan, pocetakKoriscenja);
                //pacijent.Karton.LekarskiRecepti.Add(recept);
                /*foreach(Pacijent p in PacijentiMenadzer.pacijenti)
                 {
                     if(pacijent.IdPacijenta == p.IdPacijenta)
                     {
                         p.Karton.LekarskiRecepti.Add(recept);
                     }
                 }*/
                ZdravstveniKartonMenadzer.DodajRecept(recept);
                //TabelaRecepata.PrikazRecepata.Add(recept);
               // ObavestenjaMenadzer.sacuvajIzmene();
                this.Close();

            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
