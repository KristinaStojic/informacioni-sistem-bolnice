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
            List<Oprema> opremaStaticka1 = new List<Oprema>();
            OpremaDinamicka = new ObservableCollection<Oprema>();
            foreach(Sala s in SaleMenadzer.sale)
            {
                if (s.Namjena.Equals("Skladiste"))
                {
                    foreach(Oprema o in OpremaMenadzer.oprema)
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
            OpremaStaticka = new ObservableCollectionEx<Oprema>(opremaStaticka1);
            Thread th = new Thread(izvrsi);
            th.Start();
            
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
            OpremaMenadzer.sacuvajIzmjene();
            Skladiste.otvoren = false;
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (T1.IsSelected)
            {
                var izabranaOprema = dataGridT1.SelectedItem;
                if(izabranaOprema != null)
                {
                    OpremaMenadzer.ObrisiOpremu((Oprema)izabranaOprema);
                }
            }
            else if(T2.IsSelected)
            {
                var izabranaOprema = dataGridT2.SelectedItem;
                if (izabranaOprema != null)
                {
                    OpremaMenadzer.ObrisiOpremu((Oprema)izabranaOprema);
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (T1.IsSelected)
            {
                Oprema izabranaOprema = (Oprema)dataGridT1.SelectedItem;
                if (izabranaOprema != null)
                {
                    IzmjeniOpremu iop = new IzmjeniOpremu(izabranaOprema);
                    iop.ShowDialog();
                }
            }
            else if(T2.IsSelected)
            {
                Oprema izabranaOprema = (Oprema)dataGridT2.SelectedItem;
                if (izabranaOprema != null)
                {
                    IzmjeniOpremu iop = new IzmjeniOpremu(izabranaOprema);
                    iop.ShowDialog();
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OpremaMenadzer.sacuvajIzmjene();
            Skladiste.otvoren = false;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (T1.IsSelected)
            {
                Oprema izabranaOprema = (Oprema)dataGridT1.SelectedItem;
                if (izabranaOprema != null)
                {
                    PrebaciStaticku ps = new PrebaciStaticku(izabranaOprema);
                    PrebaciStaticku.aktivan = true;
                    ps.ShowDialog();
                }
            }
            else
            {
                Oprema izabranaOprema = (Oprema)dataGridT2.SelectedItem;
                if (izabranaOprema != null)
                {
                    PrebaciDinamicku pd = new PrebaciDinamicku(izabranaOprema);
                    PrebaciDinamicku.aktivan = true;
                    pd.ShowDialog();
                }
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
                if (izabranaOprema != null)
                {
                    ObrisiOpremu oo = new ObrisiOpremu(izabranaOprema);
                    oo.Show();
                }
            }
            else if (T2.IsSelected)
            {
                Oprema izabranaOprema = (Oprema)dataGridT2.SelectedItem;
                if (izabranaOprema != null)
                {
                    ObrisiOpremu oo = new ObrisiOpremu(izabranaOprema);
                    oo.Show();
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
