using Model;
using Projekat.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikazDinamicke.xaml
    /// </summary>
    public partial class PrikazDinamicke : Window
    {
        Sala izabranaSala;
        private int colNum = 0;

        public static ObservableCollection<Oprema> OpremaDinamicka
        {
            get;
            set;
        }

        public PrikazDinamicke(Sala izabranaSala)
        {
            InitializeComponent();
            inicijalizujElemente(izabranaSala);
            postaviTekst();
            dodajDinamickuOpremu();
        }

        private void inicijalizujElemente(Sala izabranaSala)
        {
            this.izabranaSala = izabranaSala;
            this.DataContext = this;
        }

        private void dodajDinamickuOpremu()
        {
            OpremaDinamicka = new ObservableCollection<Oprema>();
            if (izabranaSala.Oprema != null)
            {
                foreach (Oprema oprema in izabranaSala.Oprema)
                {
                    if (!oprema.Staticka)
                    {
                        OpremaDinamicka.Add(oprema);
                    }
                }
            }
        }

        private void postaviTekst()
        {
            if (izabranaSala != null)
            {
                if (izabranaSala.TipSale == tipSale.SalaZaPregled)
                {
                    this.tekst.Text = "Sala za pregled (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
                else if (izabranaSala.TipSale == tipSale.OperacionaSala)
                {
                    this.tekst.Text = "Operaciona sala (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
                else
                {
                    this.tekst.Text = "Sala za odmor (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
            }
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SlanjeDinamicke_Click(object sender, RoutedEventArgs e)
        {
            PreraspodjelaDinamicke.aktivna = true;
            PreraspodjelaDinamicke preraspodjelaDinamicke = new PreraspodjelaDinamicke(izabranaSala);
            preraspodjelaDinamicke.ShowDialog();
        }

        private void SlanjeStaticke_Click(object sender, RoutedEventArgs e)
        {
            Oprema opremaZaSlanje = (Oprema)dataGrid.SelectedItem;
            if (opremaZaSlanje != null) {
                SlanjeDinamicke sd = new SlanjeDinamicke(izabranaSala, opremaZaSlanje);
                SlanjeDinamicke.aktivan = true;
                sd.ShowDialog();
            }
            else
            {
                MessageBox.Show("Morate izabrati opremu");
            }
        }

        private void Pretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            OpremaDinamicka.Clear();
            foreach(Oprema oprema in izabranaSala.Oprema)
            {
                if (oprema.NazivOpreme.StartsWith(this.Pretraga.Text) && !oprema.Staticka)
                {
                    OpremaDinamicka.Add(oprema);
                }
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (e.Key == Key.N)
                {
                    Odustani_Click(sender, e);
                }
                else if (e.Key == Key.P)
                {
                    this.Pretraga.Focus();
                }
            }
        }
    }
}
