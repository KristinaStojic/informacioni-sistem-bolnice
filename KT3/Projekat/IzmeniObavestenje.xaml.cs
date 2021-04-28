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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmeniObavestenje.xaml
    /// </summary>
    public partial class IzmeniObavestenje : Window
    {
        public Obavestenja obavestenje;
        public string oznaka;

        public IzmeniObavestenje(Obavestenja selektovanoObavestenje)
        {
            InitializeComponent();
            obavestenje = selektovanoObavestenje;

            this.listaPacijenata.ItemsSource = PacijentiMenadzer.pacijenti;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listaPacijenata.ItemsSource);
            view.Filter = UserFilter;

            if (selektovanoObavestenje != null)
            {
                naslov.Text = selektovanoObavestenje.TipObavestenja;
                sadrzaj.Text = selektovanoObavestenje.SadrzajObavestenja;

                if (selektovanoObavestenje.Oznaka.Equals("svi"))
                {
                    namena.SelectedIndex = 0;
                }
                else if (selektovanoObavestenje.Oznaka.Equals("lekari"))
                {
                    namena.SelectedIndex = 1;
                }
                else if (selektovanoObavestenje.Oznaka.Equals("upravnici"))
                {
                    namena.SelectedIndex = 2;
                }
                else if (selektovanoObavestenje.Oznaka.Equals("pacijenti"))
                {
                    namena.SelectedIndex = 3;
                }
                else if (selektovanoObavestenje.Oznaka.Equals("specificni pacijenti"))
                {
                    namena.SelectedIndex = 4;
                }

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
                        listaPacijenata.SelectedItem = PacijentiMenadzer.PronadjiPoId(id);
                    }
                }
            }
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

        private void namena_LostFocus(object sender, RoutedEventArgs e)
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

        // potvrdi
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int idLekara = 0;
            List<int> selektovaniPacijentiId = new List<int>();     // za slucaj kad se obavestenja salju specificnim pacijentima
            String datum = DateTime.Now.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            if (namena.Text.Equals("sve zaposlene"))
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

            int idObavestenja = ObavestenjaMenadzer.GenerisanjeIdObavestenja();

            if (selektovaniPacijentiId.Count > 0)
            {
                Obavestenja novoObavestenje = new Obavestenja(idObavestenja, datum, naslov.Text, sadrzaj.Text, selektovaniPacijentiId, idLekara, false, oznaka);
                ObavestenjaMenadzer.IzmeniObavestenje(obavestenje, novoObavestenje);
                ObavestenjaMenadzer.sacuvajIzmene();
            }
            else
            {
                Obavestenja novoObavestenje = new Obavestenja(idObavestenja, datum, naslov.Text, sadrzaj.Text, selektovaniPacijentiId, idLekara, false, oznaka);
                ObavestenjaMenadzer.IzmeniObavestenje(obavestenje, novoObavestenje);
                ObavestenjaMenadzer.sacuvajIzmene();
            }

            this.Close();
        }

        // otkazi
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
