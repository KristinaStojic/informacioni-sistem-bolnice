using Model;
using Projekat.Model;
using Projekat.Pomoc;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikaziSalu.xaml
    /// </summary>
    public partial class PrikaziSalu : Window
    {
        private int colNum = 0;

        public static ObservableCollection<Sala> Sale
        {
            get;
            set;
        }

        public PrikaziSalu()
        {
            InitializeComponent();
            this.DataContext = this;
            dodajSale();
        }

        private void dodajSale()
        {
            Sale = new ObservableCollection<Sala>();
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (!sala.Namjena.Equals("Skladiste"))
                {
                    Sale.Add(sala);
                }
            }
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void DodajSalu_Click(object sender, RoutedEventArgs e)
        {
            DodajSalu dodajSalu = new DodajSalu();
            dodajSalu.ShowDialog();
        }

        private void ObrisiSalu_Click(object sender, RoutedEventArgs e)
        {
            var izabranaSala = (Sala)dataGridSale.SelectedItem;
            if(izabranaSala != null) {
                BrisanjeSale brisanjeSale = new BrisanjeSale(izabranaSala);
                brisanjeSale.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
            
        }

        private void IzmjeniSalu_Click(object sender, RoutedEventArgs e)
        {
            Sala izabranaSala = (Sala)dataGridSale.SelectedItem;
            if (izabranaSala != null)
            {
                IzmjeniSalu izmjeniSalu = new IzmjeniSalu(izabranaSala);
                izmjeniSalu.ShowDialog();
            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
        }

        private void Upravnik_Click(object sender, RoutedEventArgs e)
        {
            SaleMenadzer.sacuvajIzmjene();
            Upravnik u = new Upravnik();
            u.Show();
            this.Hide();
        }

        private void Renoviranje_Click(object sender, RoutedEventArgs e)
        {
            Sala izabranaSala = (Sala)dataGridSale.SelectedItem;
            if(izabranaSala != null && !salaZakazanaZaRenoviranje(izabranaSala))
            {
                Renoviranje renoviranje = new Renoviranje(izabranaSala);
                renoviranje.Show();
            }
            else if (izabranaSala != null && salaZakazanaZaRenoviranje(izabranaSala))
            {
                MessageBox.Show("Izabrana sala je vec zakazana za renoviranje!");
            }else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
        }

        private bool salaZakazanaZaRenoviranje(Sala izabranaSala)
        {
            foreach(ZauzeceSale zauzeceSale in izabranaSala.zauzetiTermini)
            {
                if (zauzeceSale.idTermina == 0 && datumProsao(zauzeceSale.datumKrajaTermina) && vrijemeProslo(zauzeceSale.krajTermina))
                {
                    izabranaSala.zauzetiTermini.Remove(zauzeceSale);
                    return false;
                }
                else if(zauzeceSale.idTermina == 0 && (!datumProsao(zauzeceSale.datumKrajaTermina) || !vrijemeProslo(zauzeceSale.krajTermina)))
                {
                    return true;
                }
            }
            return false;
        }

        private bool datumProsao(string datum)
        {
            datum = datum.Replace('/', '-');
            DateTime datumKraja = DateTime.Parse(datum);
            return datumKraja <= DateTime.Now.Date;
        }

        private bool vrijemeProslo(string vrijeme)
        {
            int vrijemeKraja = int.Parse(vrijeme.Split(':')[0]);
            int sadasnjeVrijeme = int.Parse(DateTime.Now.TimeOfDay.ToString().Split(':')[0]);
            return vrijemeKraja <= sadasnjeVrijeme;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaleMenadzer.sacuvajIzmjene();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Sala izabranaSala = (Sala)dataGridSale.SelectedItem;
            if (izabranaSala != null)
            {
                prikaziStaticku(izabranaSala);
            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
        }

        private void prikaziStaticku(Sala izabranaSala)
        {
            try
            {
                PrikazStaticke.otvoren = true;
                PrikazStaticke ps = new PrikazStaticke(izabranaSala);
                PremjestajMenadzer.odradiZakazanePremjestaje();
                ps.ShowDialog();
            }
            catch (Exception ex) { Console.WriteLine(ex.Data); }
        }

        private void PrikazDinamicke_Click(object sender, RoutedEventArgs e)
        {
            Sala izabranaSala = (Sala)dataGridSale.SelectedItem;
            if (izabranaSala != null)
            {
                prikaziDinamicku(izabranaSala);
            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
        }

        private void prikaziDinamicku(Sala izabranaSala)
        {
            try
            {
                PrikazDinamicke pd = new PrikazDinamicke(izabranaSala);
                pd.ShowDialog();
            }
            catch (Exception ex) { Console.WriteLine(ex.Data); }
        }

        private void Osoblje_Click(object sender, RoutedEventArgs e)
        {
            //prikazi osoblje
        }

        private void Zahtjevi_Click(object sender, RoutedEventArgs e)
        {
            Zahtjevi zahtjev = new Zahtjevi();
            zahtjev.Show();
            this.Close();
        }

        private void Komunikacija_Click(object sender, RoutedEventArgs e)
        {
            Komunikacija komunikacija = new Komunikacija();
            komunikacija.Show();
            this.Close();
        }

        private void Izvjestaj_Click(object sender, RoutedEventArgs e)
        {
            //prikazi izvjestaj
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            SalePomoc salePomoc = new SalePomoc();
            salePomoc.Show();
        }

        private void OAplikaciji_Click(object sender, RoutedEventArgs e)
        {
            //O aplikaciji
        }

        private void Pretraga_Click(object sender, TextChangedEventArgs e)
        {
            Sale.Clear();
            foreach (Sala sala in SaleMenadzer.sale)
            {
                if (sala.Namjena.Equals("Skladiste"))
                {
                    continue;
                }
                if (sala.Namjena.StartsWith(this.Pretraga.Text))
                {
                    Sale.Add(sala);
                }
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))) {
                if (e.Key == Key.D) {
                    DodajSalu_Click(sender, e);
                } else if (e.Key == Key.Z || e.Key == Key.N)
                {
                    Upravnik_Click(sender, e);
                }else if(e.Key == Key.I)
                {
                    IzmjeniSalu_Click(sender, e);
                }else if(e.Key == Key.R)
                {
                    Renoviranje_Click(sender, e);
                }else if(e.Key == Key.O)
                {
                    ObrisiSalu_Click(sender, e);
                }else if(e.Key == Key.P)
                {
                    this.Pretraga.Focus();
                }else if(e.Key == Key.S)
                {
                    Osoblje_Click(sender, e);
                }else if(e.Key == Key.E)
                {
                    Zahtjevi_Click(sender, e);
                }
                else if (e.Key == Key.K)
                {
                    Komunikacija_Click(sender, e);
                }
                else if (e.Key == Key.V)
                {
                    Izvjestaj_Click(sender, e);
                }
                else if (e.Key == Key.H)
                {
                    Pomoc_Click(sender, e);
                }
            }
        }
    }
}
