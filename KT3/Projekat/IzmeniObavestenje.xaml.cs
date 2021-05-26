using System;
using System.Collections.Generic;
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
    /// Interaction logic for IzmeniObavestenje.xaml
    /// </summary>
    public partial class IzmeniObavestenje : Window
    {
        bool flag1 = false;
        bool flag2 = false;
        public Obavestenja obavestenje;

        public IzmeniObavestenje(Obavestenja selektovanoObavestenje)
        {
            InitializeComponent();
            obavestenje = selektovanoObavestenje;

            this.listaPacijenata.ItemsSource = PacijentiMenadzer.pacijenti;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource);
            view.Filter = UserFilter;

            if (selektovanoObavestenje != null)
            {
                PopuniFormu(selektovanoObavestenje);
            }
        }

        private void PopuniFormu(Obavestenja selektovanoObavestenje)
        {
            naslov.Text = selektovanoObavestenje.TipObavestenja;
            sadrzaj.Text = selektovanoObavestenje.SadrzajObavestenja;
            namena.SelectedIndex = ObavestenjaServis.OdrediIndeksIzabranogObavestenja(selektovanoObavestenje);

            if (!selektovanoObavestenje.Oznaka.Equals("specificni pacijenti"))
            {
                pretraga.IsEnabled = false;
                listaPacijenata.IsEnabled = false;
                pacijenti.IsEnabled = false;
            }
            else
            {
                pretraga.IsEnabled = true;
                listaPacijenata.IsEnabled = true;
                pacijenti.IsEnabled = true;

                foreach (int id in obavestenje.ListaIdPacijenata)
                {
                    listaPacijenata.SelectedItem = PacijentiServis.PronadjiPoId(id);
                }
            }
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            int idLekara = 0;
            String datum = DateTime.Now.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string oznaka = ObavestenjaServis.OdrediOznakuObavestenja(namena.Text);
            List<int> selektovaniPacijentiId = ObavestenjaServis.DodajSelektovanePacijente(oznaka, listaPacijenata);

            Obavestenja novoObavestenje = new Obavestenja(obavestenje.IdObavestenja, datum, naslov.Text, sadrzaj.Text, selektovaniPacijentiId, idLekara, false, oznaka);
            ObavestenjaServis.IzmeniObavestenjeSekretar(obavestenje, novoObavestenje);
            ObavestenjaServis.sacuvajIzmene();

            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        private void Pretraga_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource).Refresh();
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
    }
}
