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
    /// Interaction logic for PreraspodjelaDinamicke.xaml
    /// </summary>
    public partial class PreraspodjelaDinamicke : Window
    {
        public Oprema izabranaOprema;
        public ObservableCollection<Sala> sale { get; set; }
        public ObservableCollection<Oprema> dinamicka { get; set; }
        public PreraspodjelaDinamicke()
        {
            InitializeComponent();
            this.DataContext = this;
            dinamicka = new ObservableCollection<Oprema>();
            sale = new ObservableCollection<Sala>();
            foreach (Oprema o in OpremaMenadzer.NadjiSvuOpremu())
            {
                if (!o.Staticka)
                {
                    dinamicka.Add(o);
                }
            }
            Sala s = new Sala();
            s.Id = SaleMenadzer.GenerisanjeIdSale();
            s.Namjena = "Skladiste";
            sale.Add(s);
        }

        private void kombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.izabranaOprema = (Oprema)kombo.SelectedItem;
            azurirajSale(izabranaOprema);            
        }

        private void azurirajSale(Oprema izabranaOprema)
        {
            foreach (Sala s in SaleMenadzer.NadjiSveSale())
            {
                if (s.Oprema.Contains(izabranaOprema))
                {
                    sale.Add(s);
                }
            }
        }

    }
}
