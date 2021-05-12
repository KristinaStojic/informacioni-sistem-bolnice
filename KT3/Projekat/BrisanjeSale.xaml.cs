using Model;
using System.Windows;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for BrisanjeSale.xaml
    /// </summary>
    public partial class BrisanjeSale : Window
    {

        Sala izabranaSala;

        public BrisanjeSale(Sala izabranaSala)
        {
            InitializeComponent();
            this.izabranaSala = izabranaSala;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            SaleMenadzer.ObrisiSalu((Sala)izabranaSala);
            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
