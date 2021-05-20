using Projekat.Model;
using System.Windows;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for BrisanjeOdbijenogLijeka.xaml
    /// </summary>
    public partial class BrisanjeOdbijenogLijeka : Window
    {
        //public Lek izabraniLijek;
        public BrisanjeOdbijenogLijeka()
        {
            InitializeComponent();
            //this.izabraniLijek = izabraniLijek;
        }

       /* private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }*/

        /*private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {   
            ukloniZahtjevZaLijek(nadjiIzabraniZahtjev());
            this.Close();
        }

        private ZahtevZaLekove nadjiIzabraniZahtjev()
        {
            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if (zahtjev.lek.sifraLeka.Equals(izabraniLijek.sifraLeka))
                {
                    return zahtjev;
                }
            }
            return null;
        }

        private void ukloniZahtjevZaLijek(ZahtevZaLekove izabraniZahtjev)
        {
            LekoviMenadzer.zahteviZaLekove.Remove(izabraniZahtjev);
            //OdbijeniLijekovi.azurirajPrikaz();
            LekoviMenadzer.sacuvajIzmeneZahteva();
        }*/
    }
}
