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
using Model;
using Projekat.Model;
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for DodajObavestenje.xaml
    /// </summary>
    public partial class DodajObavestenje : Window
    {
        public string oznaka;
        public DodajObavestenje()
        {
            InitializeComponent();

            pretraga.IsEnabled = false;
            listaPacijenata.IsEnabled = false;
            pacijenti.IsEnabled = false;

            this.listaPacijenata.ItemsSource = PacijentiMenadzer.pacijenti;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource);
            view.Filter = UserFilter;
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(pretraga.Text))
            {
                return true;
            }
            else
            {
                return ((item as Pacijent).ImePacijenta.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    || ((item as Pacijent).PrezimePacijenta.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                       || ((item as Pacijent).Jmbg.ToString().IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void pretraga_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource).Refresh();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            int idLekara = 0;
            List<int> selektovaniPacijentiId = new List<int>();
            String datum = DateTime.Now.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            
            if (namena.Text.Equals("sve"))
            {
                oznaka = "svi";
            }
            else if (namena.Text.Equals("sve lekare"))
            {
                oznaka = "lekari";
            }
            else if (namena.Text.Equals("sve upravnike"))
            {
                oznaka = "upravnici";
            }
            else if (namena.Text.Equals("sve pacijente"))
            {
                oznaka = "pacijenti";
            }
            else  // specificni pacijenti
            {
                oznaka = "specificni pacijenti";
                foreach (Pacijent p in listaPacijenata.SelectedItems)
                {
                    selektovaniPacijentiId.Add(p.IdPacijenta);
                }
            }
            
            Obavestenja novoObavestenje = new Obavestenja(ObavestenjaServis.GenerisanjeIdObavestenja(), datum, naslov.Text, sadrzaj.Text, selektovaniPacijentiId, idLekara, false, oznaka);
            ObavestenjaServis.DodajObavestenjeSekretar(novoObavestenje);
            ObavestenjaServis.sacuvajIzmene();   
            
            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Namena_LostFocus(object sender, RoutedEventArgs e)
        {
            if (namena.Text.Equals("izabrane pacijente"))
            {
                pretraga.IsEnabled = true;
                listaPacijenata.IsEnabled = true;
                pacijenti.IsEnabled = true;
            }
            else
            {
                pretraga.IsEnabled = false;
                listaPacijenata.IsEnabled = false;
                pacijenti.IsEnabled = false;
            }
        }
    }
}
