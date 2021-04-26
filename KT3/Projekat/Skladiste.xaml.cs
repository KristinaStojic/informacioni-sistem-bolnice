using Caliburn.Micro;
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
using System.Collections.Concurrent;
using System.Collections.Specialized;
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
        public static ObservableCollectionEx<Oprema> OpremaStaticka//Sredi kako raditi sa colection pomocu niti
        {
            get;
            set;
        }
        public static ObservableCollection<Oprema> OpremaDinamicka
        {
            get;
            set;
        }
        public Skladiste()
        {
            InitializeComponent();
            this.DataContext = this;
            
            OpremaDinamicka = new ObservableCollection<Oprema>();
            opremaStaticka1 = new List<Oprema>();
            dodajOpremu();
            OpremaStaticka = new ObservableCollectionEx<Oprema>(opremaStaticka1);
            Thread th = new Thread(izvrsi);
            th.Start();
            
        }

        private void dodajOpremu()
        {
            
            foreach (Sala s in SaleMenadzer.sale)
            {
                if (s.Namjena.Equals("Skladiste"))
                {
                    foreach (Oprema o in OpremaMenadzer.oprema)
                    {
                        if (o.Staticka)
                        {
                            opremaStaticka1.Add(o);
                        }
                        else
                        {
                            OpremaDinamicka.Add(o);
                        }
                    }
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
                    foreach (Oprema o in OpremaMenadzer.oprema)
                    {
                        if (o.Staticka)
                        {
                            OpremaStaticka.Add(o);
                        }
                        else
                        {
                            OpremaDinamicka.Add(o);
                        }
                    }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpremaMenadzer.sacuvajIzmjene();
            Skladiste.otvoren = false;
            Zahtjevi zahtjeviProzor = new Zahtjevi();
            zahtjeviProzor.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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


        private void Button_Click_3(object sender, RoutedEventArgs e)
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
            Zahtjevi zahtjeviProzor = new Zahtjevi();
            zahtjeviProzor.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
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
            else
            {
                MessageBox.Show("Morate izabrati opremu!");
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Pomoc
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //O aplikaciji
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //Osoblje
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            PrikaziSalu ps = new PrikaziSalu();
            ps.Show();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            //Komunikacija
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            //Izvjestaj
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
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
            else
            {
                MessageBox.Show("Morate izabrati opremu!");
            }
        }

        private bool provjeriPreostalo(Oprema izabranaOprema)
        {
            int dozvoljenaKolicina = izabranaOprema.Kolicina;
            foreach (Premjestaj pm in PremjestajMenadzer.premjestaji)
            {
                if (pm.izSale.Id == 4 && pm.oprema.IdOpreme == izabranaOprema.IdOpreme)
                {
                    dozvoljenaKolicina -= pm.kolicina;
                }
            }
            if (dozvoljenaKolicina == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
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
