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
            var izabranaSala = dataGridSale.SelectedItem;
            if (izabranaSala != null)
            {
                
                SaleMenadzer.ObrisiSalu((Sala)izabranaSala);
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
            if(izabranaSala != null)
            {
                Renoviranje renoviranje = new Renoviranje(izabranaSala);
                renoviranje.Show();
            }
            else
            {
                MessageBox.Show("Morate izabrati salu!");
            }
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
            foreach(Sala sala in SaleMenadzer.sale)
            {
                if(sala.Namjena.Equals("Skladiste"))
                {
                    break;
                }
                foreach(Oprema oprema in sala.Oprema)
                {
                    if (oprema.NazivOpreme.StartsWith(this.Pretraga.Text))
                    {
                        Sale.Add(sala);
                        break;
                    }
                }
            }
        }
    }
}
