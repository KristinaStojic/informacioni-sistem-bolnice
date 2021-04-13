using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using Model;
using Projekat.Model;
using System.Threading;
using System.Globalization;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikaziTermin.xaml
    /// </summary>
    public partial class PrikaziTermin : Window
    {
        public static bool pacijentProzor;
        private int colNum = 0;
        public static ObservableCollection<Termin> Termini
        {
            get;
            set;
        }
        public PrikaziTermin()
        {
            InitializeComponent();
            this.DataContext = this;
            Termini = new ObservableCollection<Termin>();

            Thread thread = new Thread(izvrsiNit);
            thread.Start();
            pacijentProzor = true;
            foreach (Termin t in TerminMenadzer.termini)
            {
                /*if (t.Pacijent.IdPacijenta == 1)
                {
                    Termini.Add(t);
                }*/
                Termini.Add(t);
            }
        }

        public void izvrsiNit()
        {
            while (pacijentProzor)
            {
                Thread.Sleep(100); 
                ProveriRecepte();
            }
        }

        public static void ProveriRecepte()
        {
            Pacijent p = PacijentiMenadzer.PronadjiPoId(1);  // TODO: promeniti kada uradimo prijavljivanje
            foreach (LekarskiRecept lp in p.Karton.LekarskiRecepti)
            {
                /*DateTime datum = DateTime.Parse(lp.DatumPropisivanjaLeka);
               
                if (datum.Date == DateTime.Now.Date)
                {
                    DateTime vremePocetka = DateTime.Parse(lp.PocetakKoriscenja);
                    //DateTime vreme = DateTime.Now.TimeOfDay;
                    MessageBox.Show("Vreme pocetka " + vremePocetka.TimeOfDay + " , sada: " + DateTime.Now.TimeOfDay);
                    /*if (DateTime.Now.TimeOfDay == vremePocetka.TimeOfDay)
                    {
                        //MessageBox.Show("Uzmite lek (" + lp.NazivLeka + ")", "Podsetnik", MessageBoxButton.OK, MessageBoxImage.Information);
                        //Console.Beep();
                        //break;
                    }
                    int godina = int.Parse(lp.DatumPropisivanjaLeka.Substring(6));
                    int mesec = int.Parse(lp.DatumPropisivanjaLeka.Substring(0, 2));
                    int dan = int.Parse(lp.DatumPropisivanjaLeka.Substring(3, 2));
                    int sat = int.Parse(lp.PocetakKoriscenja.Substring(0, 2));
                    int min = int.Parse(lp.PocetakKoriscenja.Substring(3));
                    DateTime datumVreme = new DateTime(godina, mesec, dan, sat, min, 00);
                    Console.WriteLine(datumVreme.ToString());
                    MessageBox.Show("Datum " + datumVreme.ToString() + " , sada: " + DateTime.Now.Date);*/
                //MessageBox.Show(DateTime.Now.Date.ToString());
                MessageBox.Show("ovdee");
                foreach (DateTime d in lp.UzimanjeTerapije)
                {
                    if (d <= DateTime.Now.Date)
                    {
                        MessageBox.Show("Uzmite terapiju - " + lp.NazivLeka, "Podsetnik", MessageBoxButton.OK, MessageBoxImage.Information);
                        lp.UzimanjeTerapije.Remove(d);
                    }
                }
            }

        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 12) // **
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // zakazi
            ZakaziTermin zt = new ZakaziTermin();
            zt.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // izmeni
            Termin izabraniTermin = (Termin)dataGridTermini.SelectedItem;
            if (izabraniTermin != null)
            {
                IzmeniTermin it = new IzmeniTermin(izabraniTermin);
                //TerminMenadzer.sacuvajIzmene();
                it.Show();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // brisanje
            Termin zaBrisanje = (Termin)dataGridTermini.SelectedItem;
            if (zaBrisanje != null)
            {
                TerminMenadzer.OtkaziTermin(zaBrisanje);
                //TerminMenadzer.sacuvajIzmene();
            }
            Sala s = SaleMenadzer.NadjiSaluPoId(zaBrisanje.Prostorija.Id);
            foreach(ZauzeceSale zs in s.zauzetiTermini)
            {
                //MessageBox.Show(zs.idTermina.ToString());
                if (zs.idTermina.Equals(zaBrisanje.IdTermin))
                {
                    s.zauzetiTermini.Remove(zs);
                    break;
                }
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // nazad
            TerminMenadzer.sacuvajIzmene();
            SaleMenadzer.sacuvajIzmjene();
            PacijentiMenadzer.SacuvajIzmenePacijenta();
            pacijentProzor = false;
            this.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TerminMenadzer.sacuvajIzmene();
            SaleMenadzer.sacuvajIzmjene();
            PacijentiMenadzer.SacuvajIzmenePacijenta();
        }

        private void zdravstveniKarton_Click(object sender, RoutedEventArgs e)
        {
            Termin izabraniTermin = (Termin)dataGridTermini.SelectedItem;
            if (izabraniTermin != null)
            {
                ZdravstveniKartonPacijent it = new ZdravstveniKartonPacijent(izabraniTermin.Pacijent);
                //TerminMenadzer.sacuvajIzmene();
                it.Show();
            }
        }

        private void dataGridTermini_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}
