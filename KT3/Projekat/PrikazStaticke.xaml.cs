using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for PrikazStaticke.xaml
    /// </summary>
    public partial class PrikazStaticke : Window
    {
        public static Sala izabranaSala;
        
        private int colNum = 0;
        public static bool otvoren;
        
        public static ObservableCollectionEx<Oprema> OpremaStaticka
        {
            get;
            set;
        }

        public PrikazStaticke(Sala izabranaSala)
        {
            InitializeComponent();
            PrikazStaticke.izabranaSala = izabranaSala;
            this.DataContext = this;
            if(izabranaSala != null)
            {
                if (izabranaSala.TipSale == tipSale.SalaZaPregled)
                {
                    this.tekst.Text = "Sala za pregled (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
                else
                {
                    this.tekst.Text = "Operaciona sala (" + izabranaSala.Namjena + "), broj " + izabranaSala.brojSale;
                }
            }
            List<Oprema> opremaStaticka1 = new List<Oprema>();
            if (izabranaSala.Oprema != null)
            {
                foreach (Sala s in SaleMenadzer.sale)
                {
                    if (s.Id == izabranaSala.Id)
                    {
                        foreach (Oprema o in s.Oprema)
                        {
                            if (o.Staticka)
                            {
                                opremaStaticka1.Add(o);
                            }
                        }
                    }
                }
            }
            OpremaStaticka = new ObservableCollectionEx<Oprema>(opremaStaticka1);
            Thread th = new Thread(izvrsi);
            th.Start();
        }

        public static void azurirajPrikaz()
        {
            OpremaStaticka.Clear();
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.Id == PrikazStaticke.izabranaSala.Id)
                {
                    foreach (Oprema o in s.Oprema)
                    {
                        if (o.Staticka)
                        {
                            OpremaStaticka.Add(o);
                        }
                    }
                }
            }
            
        }
        public void izvrsi()
        {
            while (otvoren)
            {
                Thread.Sleep(10);
                PremjestajMenadzer.odradiZakazano();
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
            PrikazStaticke.otvoren = false;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PreraspodjelaStaticke.aktivna = true;
            PreraspodjelaStaticke ps = new PreraspodjelaStaticke(izabranaSala);
            ps.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Oprema opremaZaSlanje = (Oprema)dataGridStaticka.SelectedItem;
            if(opremaZaSlanje != null)
            {
                if (provjeriPreostalo(opremaZaSlanje))
                {
                    SlanjeStaticke ss = new SlanjeStaticke(izabranaSala, opremaZaSlanje);
                    SlanjeStaticke.aktivan = true;
                    ss.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Preostala oprema je vec zakazana za transfer");
                }
            }
            else
            {
                MessageBox.Show("Morate izabrati opremu");
            }
        }

        private bool provjeriPreostalo(Oprema opremaZaSlanje)
        {
            int kolicina = opremaZaSlanje.Kolicina;
            foreach (Premjestaj pm in PremjestajMenadzer.premjestaji)
            {
                kolicina = opremaZaSlanje.Kolicina;
                if (pm.izSale.Id == izabranaSala.Id && pm.oprema.IdOpreme == opremaZaSlanje.IdOpreme)
                {
                    kolicina -= pm.kolicina;
                }
            }
            if(kolicina == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PrikazStaticke.otvoren = false;
        }

        private void Pretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            OpremaStaticka.Clear();
            foreach(Oprema oprema in izabranaSala.Oprema)
            {
                if (oprema.NazivOpreme.StartsWith(this.Pretraga.Text) && oprema.Staticka)
                {
                    OpremaStaticka.Add(oprema);
                }
            }
        }
    }

}
