using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (!s.Namjena.Equals("Skladiste"))
                {
                    Sale.Add(s);
                }
            }
        }
        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 3)
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DodajSalu ds = new DodajSalu();
            ds.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Sala izabranaSala = (Sala)dataGridSale.SelectedItem;
            if (izabranaSala != null)
            {
                IzmjeniSalu iss = new IzmjeniSalu(izabranaSala);
                iss.ShowDialog();
            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SaleMenadzer.sacuvajIzmjene();
            Upravnik u = new Upravnik();
            u.Show();
            this.Hide();
            //MainWindow mw = new MainWindow();
            //mw.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
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
            Console.WriteLine(izabranaSala.zauzetiTermini.Count());
            foreach(ZauzeceSale zauzeceSale in izabranaSala.zauzetiTermini)
            {
                if (zauzeceSale.idTermina == 0 && datumProsao(zauzeceSale.datumKrajaTermina) && vrijemeProslo(zauzeceSale.krajTermina))
                {
                    izabranaSala.zauzetiTermini.Remove(zauzeceSale);
                    return false;
                }else if(zauzeceSale.idTermina == 0 && (!datumProsao(zauzeceSale.datumKrajaTermina) || !vrijemeProslo(zauzeceSale.krajTermina)))
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
                try
                {
                    PrikazStaticke.otvoren = true;
                    PrikazStaticke ps = new PrikazStaticke(izabranaSala);
                    PremjestajMenadzer.odradiZakazanePremjestaje();
                    ps.ShowDialog();
                }catch(Exception ex) { }
            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Sala izabranaSala = (Sala)dataGridSale.SelectedItem;
            if (izabranaSala != null)
            {
                try
                {
                    PrikazDinamicke pd = new PrikazDinamicke(izabranaSala);
                    pd.ShowDialog();
                }catch(Exception ex) { }
            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //prikazi osoblje
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Zahtjevi z = new Zahtjevi();
            z.Show();
            this.Close();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //prikazi komunikaciju
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            //prikazi izvjestaj
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            //pomoc
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            //O aplikaciji
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
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
                    Button_Click(sender, e);
                } else if (e.Key == Key.Z || e.Key == Key.N)
                {
                    Button_Click_3(sender, e);
                }else if(e.Key == Key.I)
                {
                    Button_Click_2(sender, e);
                }else if(e.Key == Key.R)
                {
                    Button_Click_4(sender, e);
                }else if(e.Key == Key.O)
                {
                    Button_Click_1(sender, e);
                }else if(e.Key == Key.P)
                {
                    this.Pretraga.Focus();
                }else if(e.Key == Key.S)
                {
                    MenuItem_Click(sender, e);
                }else if(e.Key == Key.H)
                {
                    MenuItem_Click_1(sender, e);
                }
                else if (e.Key == Key.K)
                {
                    MenuItem_Click_2(sender, e);
                }
                else if (e.Key == Key.V)
                {
                    MenuItem_Click_3(sender, e);
                }
            }
        }
    }
}
