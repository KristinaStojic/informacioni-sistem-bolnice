using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Model;
using Projekat.Model;
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for UvidZdravstveniKarton.xaml
    /// </summary>
    public partial class UvidZdravstveniKarton : Window
    {
        public Pacijent pacijent;
        public static ObservableCollection<LekarskiRecept> PrikazRecepata
        {
            get;
            set;
        }
        public static ObservableCollection<Anamneza> TabelaAnamneza
        {
            get;
            set;
        }

        public static ObservableCollection<Alergeni> TabelaAlergena
        {
            get;
            set;
        }

        public static ObservableCollection<Uput> TabelaUputa
        {
            get;
            set;
        }

        public UvidZdravstveniKarton(Pacijent izabraniNalog)
        {
            InitializeComponent();
            this.pacijent = izabraniNalog;
            this.DataContext = this;

            if (izabraniNalog != null)
            {
                PopuniPoljaZdravstvenogKartona(izabraniNalog);
                OnemoguciIzmenuPolja(izabraniNalog);
            }

            PopuniTabeluRecepata();
            PopuniTabeluAnamneza();
            PopuniTabeluAlergena();
            PopuniTabeluUputa();           
        }

        private void PopuniPoljaZdravstvenogKartona(Pacijent izabraniNalog)
        {
            ime.Text = izabraniNalog.ImePacijenta;
            prezime.Text = izabraniNalog.PrezimePacijenta;
            jmbg.Text = izabraniNalog.Jmbg.ToString();
            brojTelefona.Text = izabraniNalog.BrojTelefona.ToString();
            email.Text = izabraniNalog.Email;
            adresa.Text = izabraniNalog.AdresaStanovanja;
            zanimanje.Text = izabraniNalog.Zanimanje;
            polPacijenta.SelectedIndex = PacijentiServis.UcitajIndeksPola(izabraniNalog);
            bracnoStanjePacijenta.SelectedIndex = PacijentiServis.UcitajIndeksBracnogStanja(izabraniNalog);
            statusPacijenta.SelectedIndex = PacijentiServis.UcitajIndeksStatusaNaloga(izabraniNalog);

            if (izabraniNalog.IzabraniLekar != null)
            {
                lekar.Text = izabraniNalog.IzabraniLekar.ImeLek + " " + izabraniNalog.IzabraniLekar.PrezimeLek;
            }
        }

        private void PopuniTabeluRecepata() 
        {
            PrikazRecepata = new ObservableCollection<LekarskiRecept>();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                if (p.IdPacijenta == pacijent.IdPacijenta)
                {
                    foreach (LekarskiRecept lr in p.Karton.LekarskiRecepti)
                    {
                        PrikazRecepata.Add(lr);
                    }
                }
            }
        }

        private void PopuniTabeluAnamneza()
        {
            TabelaAnamneza = new ObservableCollection<Anamneza>();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                if (p.IdPacijenta == pacijent.IdPacijenta)
                {
                    foreach (Anamneza an in p.Karton.Anamneze)
                    {
                        TabelaAnamneza.Add(an);
                    }
                }
            }
        }

        private void PopuniTabeluAlergena()
        {
            TabelaAlergena = new ObservableCollection<Alergeni>();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                if (p.IdPacijenta == pacijent.IdPacijenta)
                {
                    foreach (Alergeni an in p.Karton.Alergeni)
                    {
                        TabelaAlergena.Add(an);
                    }
                }
            }
        }

        private void PopuniTabeluUputa() 
        {
            TabelaUputa = new ObservableCollection<Uput>();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                if (p.IdPacijenta == pacijent.IdPacijenta)
                {
                    foreach (Uput uput in p.Karton.Uputi)
                    {
                        TabelaUputa.Add(uput);
                    }
                }
            }
        }

        private void OnemoguciIzmenuPolja(Pacijent izabraniNalog)
        {
            ime.IsEnabled = false;
            prezime.IsEnabled = false;
            jmbg.IsEnabled = false;
            statusPacijenta.IsEnabled = false;
            polPacijenta.IsEnabled = false;
            brojTelefona.IsEnabled = false;
            email.IsEnabled = false;
            adresa.IsEnabled = false;
            bracnoStanjePacijenta.IsEnabled = false;
            zanimanje.IsEnabled = false;

            if (izabraniNalog.Maloletnik == true)
            {
                maloletnik.IsChecked = true;
            }
            else
            {
                maloletnik.IsChecked = false;
            }
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Detalji_alergena_Click(object sender, RoutedEventArgs e)
        {
            Alergeni izabraniAlergen = (Alergeni)dataGridAlergeni.SelectedItem;
            if (izabraniAlergen != null)
            {

                DetaljiAlergenaSekretar detaljiAlergena = new DetaljiAlergenaSekretar(izabraniAlergen, pacijent);
                detaljiAlergena.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali nijedan alergen!");
            }
        }

        private void Detalji_anamneze_Click(object sender, RoutedEventArgs e)
        {
            Anamneza izabranaAnamneza = (Anamneza)dataGridAnamneze.SelectedItem;
            if (izabranaAnamneza != null)
            {
                DetaljiAnamnezeSekretar detaljiAnamneze = new DetaljiAnamnezeSekretar(izabranaAnamneza, pacijent);
                detaljiAnamneze.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali nijednu anamnezu!");
            }
        }

        private void Detalji_uputa_Click(object sender, RoutedEventArgs e)
        {
            Uput izabraniUput = (Uput)dataGridUputi.SelectedItem;
            if (izabraniUput != null)
            {
                DetaljiUputaSekretar detaljiUputa = new DetaljiUputaSekretar(izabraniUput);
                detaljiUputa.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali nijedan uput!");
            }
        }

    }
}
