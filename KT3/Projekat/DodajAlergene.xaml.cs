using Model;
using Projekat.Model;
using Projekat.Servis;
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
    /// Interaction logic for DodajAlergene.xaml
    /// </summary>
    public partial class DodajAlergene : Window
    {

        Pacijent pacijent;
        Termin termin;
        bool popunjeno = false;

        ZdravstveniKartonServis servis = new ZdravstveniKartonServis();
        public DodajAlergene(Pacijent izabraniPacijent, Termin izabraniTermin)
        {
            InitializeComponent();
            this.pacijent = izabraniPacijent;
            this.termin = izabraniTermin;
            this.potvrdi.IsEnabled = false;
            PopuniPodatkePacijenta();
        }

        private void PopuniPodatkePacijenta()
        {
            this.nadjiAlergen.ItemsSource = LekoviServis.NadjiSveSastojke();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(nadjiAlergen.ItemsSource);
            view.Filter = UserFilter;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nadjiAlergen.SelectedItems.Count > 0)
            {
                Sastojak item = (Sastojak)nadjiAlergen.SelectedItems[0];
                naziv.Text = item.naziv;
            }
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(pretraga.Text))
                return true;
            else
                return ((item as Sastojak).naziv.IndexOf(pretraga.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void pretraga_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(nadjiAlergen.ItemsSource).Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (popunjeno == true)
            {
                int idAlergena = servis.GenerisanjeIdAlergena(pacijent.IdPacijenta);
                String nazivLeka = naziv.Text;
                String Nuspojava = nuspojava.Text;
                String vremeNuspojave = vreme.Text;


                Alergeni alergen = new Alergeni(idAlergena, pacijent.IdPacijenta, nazivLeka,Nuspojava, vremeNuspojave);
                servis.DodajAlergen(alergen);

                TerminServisLekar.sacuvajIzmene();
               // PacijentiServis.SacuvajIzmenePacijenta();
                SaleServis.sacuvajIzmjene();

                this.Close();
            }
            else
            {
                MessageBox.Show("Popunite sva polja!");
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            
                if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl)) //Detalji
                {
                    Button_Click(sender, e);
                }
                else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) //Nova anamneza
                {
                    this.Close();
                }
            
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void naziv_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();

        }

        private void nuspojava_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();

        }

        private void vreme_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();

        }


        private void postaviDugme()
        {
            if (this.naziv.Text != null && this.nuspojava.Text != null && this.vreme.Text != null)
            {
                izvrsiPostavljanje();
            }
            else
            {
                this.potvrdi.IsEnabled = false;
            }
        }
        private void izvrsiPostavljanje()
        {
            if (this.naziv.Text.Trim().Equals("") || this.nuspojava.Text.Trim().Equals("") || this.vreme.Text.Trim().Equals(""))
            {
                this.potvrdi.IsEnabled = false;
                popunjeno = false;
            }
            else if (!this.naziv.Text.Trim().Equals("") && !this.nuspojava.Text.Trim().Equals("") && !this.vreme.Text.Trim().Equals(""))
            {
                this.potvrdi.IsEnabled = true;
                popunjeno = true;
            }
        }
    }
}
