using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    /// Interaction logic for DodajPacijenta.xaml
    /// </summary>
  
    public partial class DodajPacijenta : Window
    {
        public DodajPacijenta()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            statusNaloga status;
            pol pol;
            bool maloletnoLice;
            int staratelj;

            if (combo.Text.Equals("STALAN"))
            {
                status = statusNaloga.Stalni;
            }
            else
            {
                status = statusNaloga.Guest;
            }

            if (combo2.Text.Equals("M"))
            {
                pol = pol.M;
            }
            else
            {
                pol = pol.Z;
            }

            if ((bool)maloletnik.IsChecked)
            {
                Console.WriteLine("cekirano");
                maloletnoLice = true;
            }
            else
            {
                maloletnoLice = false;
            }

            if (jmbgStaratelja.Text.Equals(""))
            {
                staratelj = 0;
            }
            else
            {
                staratelj = Convert.ToInt32(jmbgStaratelja.Text);
            }

            // ukoliko je guest nalog
            if (brojTelefona.Text.Equals("") || adresa.Text.Equals("") || email.Text.Equals("") || zanimanje.Text.Equals(""))  // bice izmene ?
            {
                int idP1 = PacijentiMenadzer.GenerisanjeIdPacijenta();
                Pacijent p1 = new Pacijent(idP1, ime.Text, prezime.Text, Convert.ToInt32(jmbg.Text), pol, status);
                PacijentiMenadzer.DodajNalog(p1);
            }
            else  // ukoliko je stalan nalog
            {
                bracnoStanje brStanje;
                int idP = PacijentiMenadzer.GenerisanjeIdPacijenta();

                if (combo3.Text.Equals("Neozenjen/Neudata") && combo2.Text.Equals("Z"))
                {
                    brStanje = bracnoStanje.Neudata;
                    Pacijent p = new Pacijent(idP, ime.Text, prezime.Text, Convert.ToInt32(jmbg.Text), pol, Convert.ToInt64(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje, maloletnoLice, staratelj);
                    ZdravstveniKarton karton = new ZdravstveniKarton(idP);
                    p.Karton = karton;
                    List<LekarskiRecept> lr = new List<LekarskiRecept>();
                    p.Karton.LekarskiRecepti = lr;
                    List<Anamneza> an = new List<Anamneza>();
                    p.Karton.Anamneze = an;
                    PacijentiMenadzer.DodajNalog(p);     
                }
                else if (combo3.Text.Equals("Ozenjen/Udata") && combo2.Text.Equals("Z"))
                {
                    brStanje = bracnoStanje.Udata;
                    Pacijent p = new Pacijent(idP, ime.Text, prezime.Text, Convert.ToInt32(jmbg.Text), pol, Convert.ToInt64(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje, maloletnoLice, staratelj);
                    ZdravstveniKarton karton = new ZdravstveniKarton(idP);
                    p.Karton = karton;
                    List<LekarskiRecept> lr = new List<LekarskiRecept>();
                    p.Karton.LekarskiRecepti = lr;
                    List<Anamneza> an = new List<Anamneza>();
                    p.Karton.Anamneze = an;
                    PacijentiMenadzer.DodajNalog(p);
                }
                else if (combo3.Text.Equals("Udovac/Udovica") && combo2.Text.Equals("Z"))
                {
                    brStanje = bracnoStanje.Udovica;
                    Pacijent p = new Pacijent(idP, ime.Text, prezime.Text, Convert.ToInt32(jmbg.Text), pol, Convert.ToInt64(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje, maloletnoLice, staratelj);
                    ZdravstveniKarton karton = new ZdravstveniKarton(idP);
                    p.Karton = karton;
                    List<LekarskiRecept> lr = new List<LekarskiRecept>();
                    p.Karton.LekarskiRecepti = lr;
                    List<Anamneza> an = new List<Anamneza>();
                    p.Karton.Anamneze = an;
                    PacijentiMenadzer.DodajNalog(p);

                }
                else if (combo3.Text.Equals("Razveden/Razvedena") && combo2.Text.Equals("Z"))
                {
                    brStanje = bracnoStanje.Razvedena;
                    Pacijent p = new Pacijent(idP, ime.Text, prezime.Text, Convert.ToInt32(jmbg.Text), pol, Convert.ToInt64(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje, maloletnoLice, staratelj);
                    ZdravstveniKarton karton = new ZdravstveniKarton(idP);
                    p.Karton = karton;
                    List<LekarskiRecept> lr = new List<LekarskiRecept>();
                    p.Karton.LekarskiRecepti = lr;
                    List<Anamneza> an = new List<Anamneza>();
                    p.Karton.Anamneze = an;
                    PacijentiMenadzer.DodajNalog(p);
                }

                if (combo3.Text.Equals("Neozenjen/Neudata") && combo2.Text.Equals("M"))
                {
                    brStanje = bracnoStanje.Neozenjen;
                    Pacijent p = new Pacijent(idP, ime.Text, prezime.Text, Convert.ToInt32(jmbg.Text), pol, Convert.ToInt64(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje, maloletnoLice, staratelj);
                    ZdravstveniKarton karton = new ZdravstveniKarton(idP);
                    p.Karton = karton;
                    List<LekarskiRecept> lr = new List<LekarskiRecept>();
                    p.Karton.LekarskiRecepti = lr;
                    List<Anamneza> an = new List<Anamneza>();
                    p.Karton.Anamneze = an;
                    PacijentiMenadzer.DodajNalog(p);
                }
                else if (combo3.Text.Equals("Ozenjen/Udata") && combo2.Text.Equals("M"))
                {
                    brStanje = bracnoStanje.Ozenjen;
                    Pacijent p = new Pacijent(idP, ime.Text, prezime.Text, Convert.ToInt32(jmbg.Text), pol, Convert.ToInt64(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje, maloletnoLice, staratelj);
                    ZdravstveniKarton karton = new ZdravstveniKarton(idP);
                    p.Karton = karton;
                    List<LekarskiRecept> lr = new List<LekarskiRecept>();
                    p.Karton.LekarskiRecepti = lr;
                    List<Anamneza> an = new List<Anamneza>();
                    p.Karton.Anamneze = an;
                    PacijentiMenadzer.DodajNalog(p);
                }
                else if (combo3.Text.Equals("Udovac/Udovica") && combo2.Text.Equals("M"))
                {
                    brStanje = bracnoStanje.Udovac;
                    Pacijent p = new Pacijent(idP, ime.Text, prezime.Text, Convert.ToInt32(jmbg.Text), pol, Convert.ToInt64(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje, maloletnoLice, staratelj);
                    ZdravstveniKarton karton = new ZdravstveniKarton(idP);
                    p.Karton = karton;
                    List<LekarskiRecept> lr = new List<LekarskiRecept>();
                    p.Karton.LekarskiRecepti = lr;
                    List<Anamneza> an = new List<Anamneza>();
                    p.Karton.Anamneze = an;
                    PacijentiMenadzer.DodajNalog(p);
                }
                else if (combo3.Text.Equals("Razveden/Razvedena") && combo2.Text.Equals("M"))
                {
                    brStanje = bracnoStanje.Razveden;
                    Pacijent p = new Pacijent(idP, ime.Text, prezime.Text, Convert.ToInt32(jmbg.Text), pol, Convert.ToInt64(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje, maloletnoLice, staratelj);
                    ZdravstveniKarton karton = new ZdravstveniKarton(idP);
                    p.Karton = karton;
                    List<LekarskiRecept> lr = new List<LekarskiRecept>();
                    p.Karton.LekarskiRecepti = lr;
                    List<Anamneza> an = new List<Anamneza>();
                    p.Karton.Anamneze = an;
                    PacijentiMenadzer.DodajNalog(p);
                }
            }

            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void combo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (combo.Text.Equals("GUEST")) 
            {
                brojTelefona.IsEnabled = false;
                email.IsEnabled = false;
                adresa.IsEnabled = false;
                zanimanje.IsEnabled = false;
                combo3.IsEnabled = false;
                maloletnik.IsEnabled = false;
                jmbgStaratelja.IsEnabled = false;
            }
            else if (combo.Text.Equals("STALAN"))
            {
                brojTelefona.IsEnabled = true;
                email.IsEnabled = true;
                adresa.IsEnabled = true;
                zanimanje.IsEnabled = true;
                combo3.IsEnabled = true;
                
                if ((bool)maloletnik.IsChecked)
                {
                    jmbgStaratelja.IsEnabled = true;
                }
                else
                {
                    jmbgStaratelja.IsEnabled = false;
                }
            }
        }

        private void jmbg_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(jmbg.Text.Equals("")))
            {
                if ((!PacijentiMenadzer.JedinstvenJmbg(Convert.ToInt32(jmbg.Text))))
                {
                    MessageBox.Show("JMBG vec postoji");
                    jmbg.Text = "";
                }
            }
        }

        private void maloletnik_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((bool)maloletnik.IsChecked)
            {
                jmbgStaratelja.IsEnabled = true;
            }
            else 
            {
                jmbgStaratelja.IsEnabled = false;
            }
        }

        private void ime_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((bool)maloletnik.IsChecked)
            {
                jmbgStaratelja.IsEnabled = true;
            }
            else
            {
                jmbgStaratelja.IsEnabled = false;
            }
        }
    }
}

