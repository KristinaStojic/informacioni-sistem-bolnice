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
using System.ComponentModel;

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
        public static ObservableCollection<Obavestenja> Obavestenja { get; set; }
        public Thread thread;
        public PrikaziTermin()
        {
            InitializeComponent();
            this.DataContext = this;
            Termini = new ObservableCollection<Termin>();
            Obavestenja = new ObservableCollection<Obavestenja>();
            // Teodora
            foreach (Obavestenja o in ObavestenjaMenadzer.obavestenja)
            {
                Obavestenja.Add(o);
            }


            pacijentProzor = true;
            thread = new Thread(izvrsiNit);
            thread.Start();
            foreach (Termin t in TerminMenadzer.termini)
            {
                /*if (t.Pacijent.IdPacijenta == 1)
                {
                    Termini.Add(t);
                }*/
                Termini.Add(t);
            }
            Pacijent p = PacijentiMenadzer.PronadjiPoId(1);  // TODO: promeniti kada uradimo prijavljivanje
            foreach (LekarskiRecept lr in p.Karton.LekarskiRecepti)
            {
                foreach (DateTime dt in lr.UzimanjeTerapije)
                {
                    if (dt <= DateTime.Now.Date)
                    {
                        // fali za vreme
                        Obavestenja ob = new Obavestenja(dt.ToString(), "Terapija", "Uzmite terapiju: " + lr.NazivLeka);
                        ObavestenjaMenadzer.obavestenja.Add(ob);
                    }
                }
            }
        }

        public void izvrsiNit()
        {
            while (pacijentProzor == true)
            {
                Thread.Sleep(1000);  //30000
                ProveriRecepte();
            }
        }

        public static void ProveriRecepte()
        {
            Pacijent p = PacijentiMenadzer.PronadjiPoId(1);  // TODO: promeniti kada uradimo prijavljivanje
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                foreach (LekarskiRecept lp in p.Karton.LekarskiRecepti)
                {
                    foreach (DateTime d in lp.UzimanjeTerapije)
                    {
                        string sadasnjeVremeStr = DateTime.Now.Date.ToString("HH:mm:00");
                        DateTime now = DateTime.Parse(sadasnjeVremeStr);
                        if (d.Date == DateTime.Now.Date)
                        {
                            bool flag = postojiObavestenje(lp.NazivLeka, d);
                            string vremeTrenStr = DateTime.Now.TimeOfDay.ToString().Substring(0, 5);
                            string vremeLp = d.TimeOfDay.ToString().Substring(0, 5);
                            if (vremeLp.Equals(vremeTrenStr)) 
                            {
                                // TODO: dodati uslov da obavestenje 
                                
                               //if (flag == true)
                                //{
                                    Obavestenja.Add(new Obavestenja(d.ToString(), "Terapija", "Uzmite terapiju: " + lp.NazivLeka));
                                //}   
                            }
                        }
                    }
                }
            });
        }


        public static bool postojiObavestenje(string nazivLeka, DateTime datumUzimanjaTerapije) 
        {
            foreach(Obavestenja o in ObavestenjaMenadzer.NadjiSvaObavestenja())
            {
                if (o.TipObavestenja.Equals("Terapija"))
                {
                    string[] split = o.SadrzajObavestenja.Split(' ');
                    if (split[1].Equals(nazivLeka) && datumUzimanjaTerapije.ToString().Equals(o.Datum))
                    {
                        Console.WriteLine(split[1] + " " + nazivLeka + " " + " " + " " + datumUzimanjaTerapije.TimeOfDay.ToString().Substring(0, 5));
                        return true;
                    }
                }
            }
            return false;  
        }

        private void generateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            colNum++;
            if (colNum == 8) // **
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
                Sala s = SaleMenadzer.NadjiSaluPoId(zaBrisanje.Prostorija.Id);
                foreach (ZauzeceSale zs in s.zauzetiTermini)
                {
                    //MessageBox.Show(zs.idTermina.ToString());
                    if (zs.idTermina.Equals(zaBrisanje.IdTermin))
                    {
                        s.zauzetiTermini.Remove(zs);
                        break;
                    }
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // nazad
            TerminMenadzer.sacuvajIzmene();
            SaleMenadzer.sacuvajIzmjene();
            PacijentiMenadzer.SacuvajIzmenePacijenta();
            ObavestenjaMenadzer.sacuvajIzmene();
            pacijentProzor = false;
            this.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TerminMenadzer.sacuvajIzmene();
            SaleMenadzer.sacuvajIzmjene();
            PacijentiMenadzer.SacuvajIzmenePacijenta();
            pacijentProzor = false;
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

        private void obavestenja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
    
    /*OBAVESTENJE klasa */
    /*public partial class Obavestenje : INotifyPropertyChanged
    {
        public string TipObavestenja { get; set; } // za preglede, za recepte
        public string SadrzajObavestenja { get; set; }      // popijte lek
        public DateTime Datum { get; set; }
        public bool Procitano { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    
        public Obavestenje() { }

        public Obavestenje(DateTime datum, string TipOb, string SadrzajOb)
        {
            this.Datum = datum;
            this.TipObavestenja = TipOb;
            this.SadrzajObavestenja = SadrzajOb;
        } 

        // obavestenja
        private void obavestenja_Click(object sender, RoutedEventArgs e)
        {
            ObavestenjaPacijent o = new ObavestenjaPacijent();
            o.Show();
        }
    }*/
}
