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
        bool flag1 = false;
        bool flag2 = false;
        ObavestenjaServis servis = new ObavestenjaServis();
        public DodajObavestenje()
        {
            InitializeComponent();

            potvrdi.IsEnabled = false;
            pretraga.IsEnabled = false;
            listaPacijenata.IsEnabled = false;
            pacijenti.IsEnabled = false;

            List<Pacijent> pacijentiLista = PacijentiServis.PronadjiSve();
            this.listaPacijenata.ItemsSource = pacijentiLista;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource);
            view.Filter = PretragaPacijenata;
        }

        private bool PretragaPacijenata(object item)
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

        private void Pretraga_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource).Refresh();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            int idLekara = 0;
            String datum = DateTime.Now.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string oznaka = servis.OdrediOznakuObavestenja(namena.Text);
            List<int> selektovaniPacijentiId = servis.DodajSelektovanePacijente(oznaka, listaPacijenata);

            Obavestenja novoObavestenje = new Obavestenja(servis.GenerisanjeIdObavestenja(), datum, naslov.Text, sadrzaj.Text, selektovaniPacijentiId, idLekara, false, oznaka);
            servis.DodajObavestenjeSekretar(novoObavestenje);
            //ObavestenjaServis.sacuvajIzmene();   
            
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

        private void Naslov_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flag1 = false;
                potvrdi.IsEnabled = false;
            }
            else
            {
                flag1 = true;
                if (flag1 == true && flag2 == true)
                {
                    potvrdi.IsEnabled = true;
                }
            }
        }

        private void Sadrzaj_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flag2 = false;
                potvrdi.IsEnabled = false;
            }
            else
            {
                flag2 = true;
                if (flag1 == true && flag2 == true)
                {
                    potvrdi.IsEnabled = true;
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.O && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Odustani_Click(sender, e);
            }
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Odustani_Click(sender, e);
            }
        }

    }
}
