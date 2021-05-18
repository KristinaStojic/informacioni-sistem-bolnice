using Projekat.Model;
using Projekat.Servis;
using System.Windows;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for BrisanjeLijeka.xaml
    /// </summary>
    public partial class BrisanjeLijeka : Window
    {
        Lek izabraniLijek;
        public BrisanjeLijeka(Lek izabraniLijek)
        {
            InitializeComponent();
            this.izabraniLijek = izabraniLijek;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            LekoviServis.obrisiLijek(izabraniLijek);
            Lijekovi.Lekovi.Remove(izabraniLijek);
            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
