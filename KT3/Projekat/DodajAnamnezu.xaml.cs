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
    /// Interaction logic for DodajAnamnezu.xaml
    /// </summary>
    public partial class DodajAnamnezu : Window
    {
        public Pacijent pacijent;
        public DodajAnamnezu(Pacijent izabraniPacijent)
        {
            this.pacijent = izabraniPacijent;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int brojAnamneze = ZdravstveniKartonMenadzer.GenerisanjeIdAnamneze(pacijent.IdPacijenta);
                
                String formatirano = null;
                DateTime? selectedDate = datum.SelectedDate;
                Console.WriteLine(selectedDate);
                if (selectedDate.HasValue)
                {
                    formatirano = selectedDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                }
                //TO DO: DODAJ ZA DOKTORA
                string bolest = bol.Text;
                string terapija = terap.Text;
                Anamneza anamneza = new Anamneza(brojAnamneze, pacijent, formatirano, bolest, terapija);
               
                ZdravstveniKartonMenadzer.DodajAnamnezu(anamneza);
                this.Close();

            }
            catch (System.Exception)
            {
                MessageBox.Show("Niste uneli ispravne podatke", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
