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
using Projekat.Pomoc;
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
            List<Pacijent> pacijenti = PacijentiServis.PronadjiSve();
            foreach (Pacijent p in pacijenti)
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
            List<Pacijent> pacijenti = PacijentiServis.PronadjiSve();
            foreach (Pacijent p in pacijenti)
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
            List<Pacijent> pacijenti = PacijentiServis.PronadjiSve();
            foreach (Pacijent p in pacijenti)
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
            List<Pacijent> pacijenti = PacijentiServis.PronadjiSve();
            foreach (Pacijent p in pacijenti)
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

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            ZdravstveniKartonSekretarPomoc pomoc = new ZdravstveniKartonSekretarPomoc();
            pomoc.Show();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.L && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                licniPodaci.IsSelected = true;
            }
            else if (e.Key == Key.L && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                licniPodaci.IsSelected = true;
            }
            else if (e.Key == Key.M && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                anamneza.IsSelected = true;
            }
            else if (e.Key == Key.M && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                anamneza.IsSelected = true;
            }
            else if (e.Key == Key.A && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                alergeni.IsSelected = true;
            }
            else if (e.Key == Key.A && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                alergeni.IsSelected = true;
            }
            else if (e.Key == Key.R && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                recepti.IsSelected = true;
            }
            else if (e.Key == Key.R && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                recepti.IsSelected = true;
            }
            else if (e.Key == Key.U && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                uputi.IsSelected = true;
            }
            else if (e.Key == Key.U && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                uputi.IsSelected = true;
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Nazad_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Nazad_Click(sender, e);
            }
            else if (e.Key == Key.P && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Pomoc_Click(sender, e);
            }
            else if (e.Key == Key.P && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Pomoc_Click(sender, e);
            }
        }
    }
}
