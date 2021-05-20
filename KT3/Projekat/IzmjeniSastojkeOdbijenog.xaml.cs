using Projekat.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmjeniSastojkeOdbijenog.xaml
    /// </summary>
    public partial class IzmjeniSastojkeOdbijenog : Window
    {
        Lek izabraniLijek;
        /*private int colNum = 0;

        public static ObservableCollection<Sastojak> SastojciLijeka
        {
            get;
            set;
        }*/
        
        public IzmjeniSastojkeOdbijenog()
        {
            InitializeComponent();
            //inicijalizujElemente(izabraniLijek);
            //postaviTekst();
            //dodajSastojke();
        }

        /*private void inicijalizujElemente(Lek izabraniLijek)
        {
            this.izabraniLijek = izabraniLijek;
            this.DataContext = this;
        }

        private void postaviTekst()
        {
            this.tekst.Text = "Sastojci za lijek: " + izabraniLijek.nazivLeka;
        }*/

       /* private void dodajSastojke()
        {
            SastojciLijeka = new ObservableCollection<Sastojak>();

            foreach (ZahtevZaLekove zahtjev in LekoviMenadzer.zahteviZaLekove)
            {
                if(zahtjev.lek.sifraLeka == izabraniLijek.sifraLeka)
                {
                    dodajSastojkeZahtjevu(zahtjev);
                }
            }
        }

        private void dodajSastojkeZahtjevu(ZahtevZaLekove zahtjev)
        {
            foreach (Sastojak sastojak in zahtjev.lek.sastojci)
            {
                SastojciLijeka.Add(sastojak);
            }
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }*/

        /*private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.N)
                {
                    Odustani_Click(sender, e);
                }
            }
        }*/

        /*private void IzmjeniSastojak_Click(object sender, RoutedEventArgs e)
        {
            Sastojak izabraniSastojak = (Sastojak)dataGridSastojci.SelectedItem;
            if(izabraniSastojak != null)
            {
                IzmjeniSastojakOdbijenog izmjeniSastojak = new IzmjeniSastojakOdbijenog(izabraniLijek, izabraniSastojak);
                izmjeniSastojak.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati sastojak!");
            }
        }*/
    }
}
