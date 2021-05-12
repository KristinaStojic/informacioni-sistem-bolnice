using Model;
using Projekat.Model;
using Projekat.Pomoc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for Skladiste.xaml
    /// </summary>
    public partial class Skladiste : Window
    {
        private int colNum = 0;
        public static bool otvoren;
        List<Oprema> opremaStaticka1;
        public static ObservableCollectionEx<Oprema> OpremaStaticka{get; set;}
        public static ObservableCollection<Oprema> OpremaDinamicka{get; set;}

        public Skladiste()
        {
            InitializeComponent();
            inicijaliujElemente();
            dodajOpremu();
            OpremaStaticka = new ObservableCollectionEx<Oprema>(opremaStaticka1);
            Thread th = new Thread(izvrsi);
            th.Start();   
        }

        private void inicijaliujElemente()
        {
            this.DataContext = this;
            OpremaDinamicka = new ObservableCollection<Oprema>();
            opremaStaticka1 = new List<Oprema>();
        }

        private void dodajOpremu()
        {
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.Namjena.Equals("Skladiste"))
                {
                    pronadjiOpremuZaDodavanje();
                }
            }
        }

        private void pronadjiOpremuZaDodavanje()
        {
            foreach (Oprema oprema in OpremaMenadzer.oprema)
            {
                if (oprema.Staticka)
                {
                    opremaStaticka1.Add(oprema);
                }
                else
                {
                    OpremaDinamicka.Add(oprema);
                }
            }
        }

        public static void azurirajOpremu()
        {
            OpremaDinamicka.Clear();
            OpremaStaticka.Clear();
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.Namjena.Equals("Skladiste"))
                {
                    azurirajPrikazOpreme();
                }
            }
        }

        private static void azurirajPrikazOpreme()
        {
            foreach (Oprema oprema in OpremaMenadzer.oprema)
            {
                if (oprema.Staticka)
                {
                    OpremaStaticka.Add(oprema);
                }
                else
                {
                    OpremaDinamicka.Add(oprema);
                }
            }
        }

        public void izvrsi()
        {
            while (otvoren)
            {
                Thread.Sleep(1000);
                PremjestajMenadzer.odradiZakazanePremjestaje();
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
            OpremaMenadzer.sacuvajIzmjene();
            Zahtjevi zahtjevi = new Zahtjevi();
            zahtjevi.Show();
            Skladiste.otvoren = false;
            this.Close();
        }

        private void DodajOpremu_Click(object sender, RoutedEventArgs e)
        {
            if (T1.IsSelected)
            {
                DodajOpremu w1 = new DodajOpremu(true);
                w1.ShowDialog();
            }
            else if(T2.IsSelected)
            {
                DodajOpremu w1 = new DodajOpremu(false);
                w1.ShowDialog();
            }
        }

        private void IzmjeniOpreum_Click(object sender, RoutedEventArgs e)
        {
            if (T1.IsSelected)
            {
                Oprema izabranaOprema = (Oprema)dataGridT1.SelectedItem;
                izmjeniOpremu(izabranaOprema);
            }
            else if(T2.IsSelected)
            {
                Oprema izabranaOprema = (Oprema)dataGridT2.SelectedItem;
                izmjeniOpremu(izabranaOprema);
            }
        }

        private void izmjeniOpremu(Oprema izabranaOprema)
        {
            if (izabranaOprema != null)
            {
                IzmjeniOpremu iop = new IzmjeniOpremu(izabranaOprema);
                iop.ShowDialog();
            }
            else
            {
                MessageBox.Show("Morate izabrati opremu!");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpremaMenadzer.sacuvajIzmjene();
            Skladiste.otvoren = false;
        }

        private void PrebaciStaticku_Click(object sender, RoutedEventArgs e)
        {
            if (T1.IsSelected)
            {
                Oprema izabranaOprema = (Oprema)dataGridT1.SelectedItem;
                prebaciStaticku(izabranaOprema);
            }
            else
            {
                Oprema izabranaOprema = (Oprema)dataGridT2.SelectedItem;
                prebaciDinamicku(izabranaOprema);
            }
        }

        private void prebaciStaticku(Oprema izabranaOprema)
        {
            if (izabranaOprema != null)
            {
                prebaciStatickuOpremu(izabranaOprema);
            }
            else
            {
                MessageBox.Show("Morate izabrati opremu!");
            }
        }

        private void prebaciStatickuOpremu(Oprema izabranaOprema)
        {
            if (provjeriPreostalo(izabranaOprema))
            {
                PrebaciStaticku ps = new PrebaciStaticku(izabranaOprema);
                PrebaciStaticku.aktivan = true;
                ps.ShowDialog();
            }
            else
            {
                MessageBox.Show("Sva preostala oprema je vec zakazana za transfer");
            }
        }

        private void prebaciDinamicku(Oprema izabranaOprema)
        {
            if (izabranaOprema != null)
            {
                PrebaciDinamicku pd = new PrebaciDinamicku(izabranaOprema);
                PrebaciDinamicku.aktivan = true;
                pd.ShowDialog();
            }
            else
            {
                MessageBox.Show("Morate izabrati opremu!");
            }
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            //Pomoc
        }

        private void OAplikaciji_Click(object sender, RoutedEventArgs e)
        {
            //O aplikaciji
        }

        private void Osoblje_Click(object sender, RoutedEventArgs e)
        {
            //Osoblje
        }

        private void Sale_Click(object sender, RoutedEventArgs e)
        {
            PrikaziSalu ps = new PrikaziSalu();
            this.Hide();
            ps.Show();
        }

        private void Komunikacija_Click(object sender, RoutedEventArgs e)
        {
            Komunikacija komunikacija = new Komunikacija();
            komunikacija.Show();
            this.Close();
        }

        private void Izvjestaj_Click(object sender, RoutedEventArgs e)
        {
            //Izvjestaj
        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
            if (T1.IsSelected)
            {
                Oprema izabranaOprema = (Oprema)dataGridT1.SelectedItem;
                obrisiOpremu(izabranaOprema);
            }
            else if (T2.IsSelected)
            {
                Oprema izabranaOprema = (Oprema)dataGridT2.SelectedItem;
                obrisiOpremu(izabranaOprema);
            }
            
        }

        private void obrisiOpremu(Oprema izabranaOprema)
        {
            if (izabranaOprema != null)
            {
                obrisiOpremuSkladista(izabranaOprema);
            }
            else
            {
                MessageBox.Show("Morate izabrati opremu!");
            }
        }

        private void obrisiOpremuSkladista(Oprema izabranaOprema)
        {
            if (provjeriPreostalo(izabranaOprema))
            {
                ObrisiOpremu oo = new ObrisiOpremu(izabranaOprema);
                oo.Show();
            }
            else
            {
                MessageBox.Show("Nije moguce obrisati opremu, oprema je vec zakazana za transfer");
            }
        }

        private bool provjeriPreostalo(Oprema izabranaOprema)
        {
            if (nadjiDozvoljenuKolicinu(izabranaOprema) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int nadjiDozvoljenuKolicinu(Oprema izabranaOprema)
        {
            int dozvoljenaKolicina = izabranaOprema.Kolicina;
            foreach (Premjestaj pm in PremjestajMenadzer.premjestaji)
            {
                if (pm.izSale.Id == 4 && pm.oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    dozvoljenaKolicina -= pm.kolicina;
                }
            }
            return dozvoljenaKolicina;
        }

        private void Pretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (T1.IsSelected)
            {
                nadjiStaticku();
            }else if (T2.IsSelected)
            {
                nadjiDinamicku();
            }
        }

        private void nadjiStaticku()
        {
            OpremaStaticka.Clear();
            foreach (Oprema oprema in OpremaMenadzer.oprema)
            {
                if (oprema.NazivOpreme.StartsWith(this.Pretraga.Text) && oprema.Staticka)
                {
                    OpremaStaticka.Add(oprema);
                }
            }
        }

        private void nadjiDinamicku()
        {
            OpremaDinamicka.Clear();
            foreach (Oprema oprema in OpremaMenadzer.oprema)
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
                }else if (e.Key == Key.D)
                {
                    DodajOpremu_Click(sender, e);
                }
                else if (e.Key == Key.I)
                {
                    IzmjeniOpreum_Click(sender, e);
                }
                else if (e.Key == Key.O)
                {
                    Obrisi_Click(sender, e);
                }
                else if (e.Key == Key.R)
                {
                    PrebaciStaticku_Click(sender, e);
                }
                else if (e.Key == Key.P)
                {
                    this.Pretraga.Focus();
                }
                else if (e.Key == Key.S)
                {
                    Osoblje_Click(sender, e);
                }
                else if (e.Key == Key.T)
                {
                    Sale_Click(sender, e);
                }
                else if (e.Key == Key.H)
                {
                    MenuItem_Click_6(sender, e);
                }else if(e.Key == Key.K)
                {
                    Komunikacija_Click(sender, e);
                }
            }
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            SkladistePomoc skladistePomoc = new SkladistePomoc();
            skladistePomoc.Show();
        }

    }



    public class ObservableCollectionEx<t> : ObservableCollection<t>
    {
        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        public ObservableCollectionEx(IEnumerable<t> collection) : base(collection) { }
        public ObservableCollectionEx(List<t> collection) : base(collection) { }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            using (BlockReentrancy())
            {
                var eventHandler = CollectionChanged;
                if (eventHandler != null)
                {
                    Delegate[] delegates = eventHandler.GetInvocationList();
                    
                    foreach (NotifyCollectionChangedEventHandler handler in delegates)
                    {
                        var dispatcherObject = handler.Target as DispatcherObject;
                    
                        if (dispatcherObject != null && dispatcherObject.CheckAccess() == false)
                    
                            dispatcherObject.Dispatcher.Invoke(DispatcherPriority.DataBind,
                                          handler, this, e);
                        else 
                            handler(this, e);
                    }
                }
            }
        }
    }
}
