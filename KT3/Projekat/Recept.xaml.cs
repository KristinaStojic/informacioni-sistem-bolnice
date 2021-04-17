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
                Console.WriteLine(selectedDate);
                if (selectedDate.HasValue)
                {
                    formatirano = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                }
                int kolicinaNaDan = int.Parse(brojUzimanja.Text);
                int kolikoDana = int.Parse(dani.Text);
                String pocetakKoriscenja = sati.Text + ":" + min.Text;
                
                //LekarskiRecept recept = new LekarskiRecept(pacijent, brojRecepta, nazivLeka, formatirano, kolikoDana, kolicinaNaDan, pocetakKoriscenja);
                //pacijent.Karton.LekarskiRecepti.Add(recept);
               /*foreach(Pacijent p in PacijentiMenadzer.pacijenti)
                {
                    if(pacijent.IdPacijenta == p.IdPacijenta)
                    {
                        p.Karton.LekarskiRecepti.Add(recept);
                    }
                }*/
                //ZdravstveniKartonMenadzer.DodajRecept(recept);
                //TabelaRecepata.PrikazRecepata.Add(recept);
                this.Close();

            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
