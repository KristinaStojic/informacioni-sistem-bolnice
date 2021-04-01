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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmeniTerminLekara.xaml
    /// </summary>
    public partial class IzmeniTerminLekara : Window
    {
        public IzmeniTerminLekara()
        {
            InitializeComponent();
        }
        public Termin termin;
        public IzmeniTerminLekara(Termin izabraniTermin)
        {
            InitializeComponent();
            this.termin = izabraniTermin;
            if (izabraniTermin != null)
            {

                this.vpp.Text = izabraniTermin.VremePocetka;
                this.vkk.Text = izabraniTermin.VremeKraja;


                this.IDpacijenta.Text = izabraniTermin.Pacijent.Jmbg.ToString();
                TipTermina tp;
                if (izabraniTermin.tipTermina.Equals(TipTermina.Operacija))
                {
                    this.tipPregleda.SelectedIndex = 0;
                }
                else if (izabraniTermin.tipTermina.Equals(TipTermina.Pregled))
                {
                    this.tipPregleda.SelectedIndex = 1;
                }


                tp = izabraniTermin.tipTermina;
                dp.SelectedDate = DateTime.Parse(izabraniTermin.Datum);
                this.prostorije.SelectedIndex = izabraniTermin.Prostorija.Id;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //potvrdi
            try
            {


                String formatted = null;
                DateTime? selectedDate = dp.SelectedDate;
                Console.WriteLine(selectedDate);
                if (selectedDate.HasValue)
                {
                    formatted = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                }
                String vp = vpp.Text;
                String vk = vkk.Text;
                TipTermina tp;
                if (tipPregleda.Text.Equals("Pregled"))
                {
                    tp = TipTermina.Pregled;
                }
                else
                {
                    tp = TipTermina.Operacija;
                }
                // int idLek = int.Parse(idlekara.Text);
                // Lekar l = new Lekar(5, "Filip", "Filipovic");

                List<Pacijent> pacijenti = PacijentiMenadzer.PronadjiSve();
                int idPac = int.Parse(IDpacijenta.Text);
                Pacijent p = PacijentiMenadzer.PronadjiPoId(idPac);

                List<Sala> sale = SaleMenadzer.NadjiSveSale();
                int idSale = int.Parse(prostorije.Text);
                Sala sala = SaleMenadzer.NadjiSaluPoId(idSale);

                Termin t = new Termin(termin.IdTermin, formatted, vp, vk, tp, termin.Lekar, sala, p);
                TerminMenadzer.IzmeniTermin(termin, t);
                this.Close();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //odustani
            this.Close();
        }
    }
}

