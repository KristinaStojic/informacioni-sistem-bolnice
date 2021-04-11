using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ZakaziTerminPreferenca.xaml
    /// </summary>
    public partial class ZakaziTerminPreferenca : Window
    {
        public List<string> sviSlobodni2 { get; set; }
        public ObservableCollection<Termin> Termini2 { get; set; }
        public static List<Termin> lista;
        public Termin t;
        public ZakaziTerminPreferenca()
        {
            InitializeComponent();
            this.DataContext = this;
            Termini2 = new ObservableCollection<Termin>();
            sviSlobodni2 = new List<string>() { "07:00", "07:30", "08:00", "08:30",
                                                                "09:00", "09:30",  "10:00", "10:30",
                                                                "11:00", "11:30", "12:00", "12:30",
                                                                "13:00", "13:30", "14:00", "14:30",
                                                                "15:00", "15:30", "16:00", "16:30",
                                                                "17:00", "17:30", "18:00", "18:30",
                                                                "19:00", "19:30", "20:00"};

           /* List<Termin> termini = pronadjiSlobodanTermin();
            foreach(Termin t in termini) {
                
                termins.Add(t);
            }*/
            /*Termin t1 = termins.ElementAt(1);
            this.vreme1.Text = t1.VremePocetka;
            this.datum1.Text = t1.Datum;
            this.brSale1.Text = t1.Prostorija.Id.ToString();

            Termin t2 = termins.ElementAt(1);
            this.vreme2.Text = t2.VremePocetka;
            this.datum2.Text = t2.Datum;
            this.brSale2.Text = t2.Prostorija.Id.ToString();

            Termin t3 = termins.ElementAt(2);
            this.vreme3.Text = t3.VremePocetka;
            this.datum3.Text = t3.Datum;
            this.brSale3.Text = t3.Prostorija.Id.ToString();*/
       
            int count = 0;
            lista = new List<Termin>();
            bool jeTri = false;
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.TipSale.Equals(tipSale.SalaZaPregled))
                {
                    foreach (ZauzeceSale zs in s.zauzetiTermini)
                    {
                        DateTime zsDatum = DateTime.Parse(zs.datumTermina);
                        DateTime noviDatum = DateTime.Now.AddDays(3); // tri dana unapred
                        //MessageBox.Show("Novi datum: " + noviDatum.ToString() + " trenutni datum: " + DateTime.Now.ToString() );
                        if (DateTime.Compare(zsDatum, noviDatum) < 0 && jeTri == false)
                        {
                            foreach (string slot in sviSlobodni2)
                            {
                                if (!s.zauzetiTermini.Exists(x => x.pocetakTermina.Equals(slot)) && jeTri == false)
                                {
                                    t = new Termin();
                                    t.IdTermin = TerminMenadzer.GenerisanjeIdTermina();
                                    t.Datum = zs.datumTermina;
                                    t.VremePocetka = slot;
                                    t.VremeKraja = ZakaziTermin.IzracunajVremeKraja(slot);
                                    t.Prostorija = s;
                                    t.tipTermina = TipTermina.Pregled;
                                    foreach (Lekar l in MainWindow.lekari)
                                    {
                                        if (l.IdLekara.Equals(1))
                                        {
                                            t.Lekar = l;
                                            //MessageBox.Show(l.ToString());
                                            break;
                                        }
                                        break;
                                    }

                                    // TODO: isparivi kada uradimo prijavljivanje
                                    t.Pacijent = PacijentiMenadzer.PronadjiPoId(1);
                                    count++;
                                    Termini2.Add(t);
                                    if (count == 3)
                                    {
                                        jeTri = true;
                                    }
                                }
                            }
                        }
                    }
                    /*if (count == 3)
                    {
                        break;
                    }*/
                }
            }
       }
        private void preferencaGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void zakazi_Click(object sender, RoutedEventArgs e)
        {
            Termin t = (Termin)preferencaGrid.SelectedItem;
            MessageBox.Show(t.Datum);
            // TODO: sacuvati u listu zauzetih termina, srediti id termina
            TerminMenadzer.ZakaziTermin(t);
            TerminMenadzer.sacuvajIzmene();
            this.Close();
        }

        private void nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
