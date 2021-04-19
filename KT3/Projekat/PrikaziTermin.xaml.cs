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
        public static int idPacijent = 1;
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
            pacijentProzor = true;
            thread = new Thread(izvrsiNit);
            thread.Start();
            foreach (Termin t in TerminMenadzer.termini)
            {
                /*if (t.Pacijent.IdPacijenta == idPacijent)
                {
                    Termini.Add(t);
                }*/
                Termini.Add(t);
            }
            Pacijent p = PacijentiMenadzer.PronadjiPoId(idPacijent);  // TODO: promeniti kada uradimo prijavljivanje
            /*foreach (LekarskiRecept lr in p.Karton.LekarskiRecepti)
            {
                foreach (DateTime dt in lr.UzimanjeTerapije)
                {
                    if (dt.Date <= DateTime.Now.Date)
                    {
                        // fali za vreme
                        Obavestenja ob = new Obavestenja(dt.ToString(), "Terapija", "Uzmite terapiju: " + lr.NazivLeka);
                        ObavestenjaMenadzer.obavestenja.Add(ob);
                        Obavestenja.Add(ob);
                    }
                }
            }*/
            foreach(Obavestenja o in ObavestenjaMenadzer.obavestenja)
            {
                if(o.TipObavestenja.Equals("Terapija")) 
                {
                   
                    DateTime dt = DateTime.Parse(o.Datum);
                    if (dt.Date <= DateTime.Now.Date)
                    {
                        string vreme = dt.ToString("HH:mm");
                        string trenutnoVreme = DateTime.Now.ToString("HH:mm");
                        if (dt.TimeOfDay <= DateTime.Now.TimeOfDay)
                        {
                            Obavestenja.Add(o);
                        }
                    }
                } else
                {
                    Obavestenja.Add(o);
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
            Pacijent p = PacijentiMenadzer.PronadjiPoId(idPacijent);  // TODO: promeniti kada uradimo prijavljivanje
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
                            //MessageBox.Show(vremeLp + " " + vremeTrenStr);
                            if (vremeLp.Equals(vremeTrenStr))
                            {
                                //ako ne postoji 
                                if (flag == false)
                                {
                                    Obavestenja.Add(new Obavestenja(d.ToString("MM/dd/yyyy HH:mm"), "Terapija", "Uzmite terapiju: " + lp.NazivLeka));
                                    //ObavestenjaMenadzer.obavestenja.Add(new Obavestenja(d.ToString(), "Terapija", "Uzmite terapiju: " + lp.NazivLeka)); 
                                }
                            }
                        }
                    }
                }
            });
        }


        public static bool postojiObavestenje(string nazivLeka, DateTime datumUzimanjaTerapije) 
        {
            foreach(Obavestenja o in Obavestenja)
            {
                if (o.TipObavestenja.Equals("Terapija"))
                {
                    //MessageBox.Show("Metoda: " + datumUzimanjaTerapije + " " + o.Datum + "\n" + split[2] + " " + nazivLeka);
                    /*if (split[2].Equals(nazivLeka) && datumUzimanjaTerapije.ToString().Equals(o.Datum))
                    {
                        Console.WriteLine(split[1] + " " + nazivLeka + " " + datumUzimanjaTerapije.TimeOfDay.ToString().Substring(0, 5));
                        //return true;
                    }*/
                    string sadrzaj = "Uzmite terapiju: " + nazivLeka;
                    string datum = datumUzimanjaTerapije.ToString();
                    if (!Obavestenja.Any(x => x.SadrzajObavestenja == sadrzaj) && !Obavestenja.Any(y => y.Datum == datum))
                    {
                        return false;
                    }
                    
                }
            }
            return true;
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
}
