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
    /// Interaction logic for DodajAnamnezu.xaml
    /// </summary>
    public partial class DodajAnamnezu : Window
    {
        public Pacijent pacijent;
        public Termin termin;
        public DodajAnamnezu(Pacijent izabraniPacijent, Termin Ntermin)
        {
            InitializeComponent();
            //PopuniPodatkePacijenta();
            this.pacijent = izabraniPacijent;
            this.termin = Ntermin;
            this.lekar.Text = termin.Lekar.ImeLek + " " + termin.Lekar.PrezimeLek;
            this.datum.SelectedDate = DateTime.Parse(termin.Datum);
        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
                int brojAnamneze = ZdravstveniKartonServis.GenerisanjeIdAnamneze(pacijent.IdPacijenta);          
                String datum = NadjiDatumPregleda();
                string bolest = bol.Text;
                string terapija = terap.Text;

                Anamneza anamneza = new Anamneza(brojAnamneze, pacijent.IdPacijenta, datum, bolest, terapija,termin.Lekar.IdLekara, termin.IdTermin); 
                ZdravstveniKartonServis.DodajAnamnezu(anamneza);


                this.Close();
        }

        private string NadjiDatumPregleda()
        {
            String datumPregleda = null;
            DateTime? selectedDate = datum.SelectedDate;
            Console.WriteLine(selectedDate);
            if (selectedDate.HasValue)
            {
                datumPregleda = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            }

            return datumPregleda;
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
    }
}
