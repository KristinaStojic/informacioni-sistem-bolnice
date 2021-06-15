using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmeniPacijenta.xaml
    /// </summary>
    public partial class IzmeniPacijenta : Window
    {
        public bool flag1 = false;
        public bool flag2 = false;
        public bool flag3 = false;
        public bool flag4 = false;
        public bool flag5 = false;
        public bool flag6 = false;
        public bool flag7 = false;
        public bool flag8 = true;
        public Pacijent pacijent;
        PacijentiServis servis = new PacijentiServis();
        public IzmeniPacijenta(Pacijent izabraniNalog)
        {
            InitializeComponent();
            this.pacijent = izabraniNalog;
            this.DataContext = this;

            if (izabraniNalog != null) 
            {
                PopuniPoljaForme(izabraniNalog);
            }
        }

        private void PopuniPoljaForme(Pacijent izabraniNalog)
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

            OnemoguciPoljaZaGuestNalog(izabraniNalog);
            OmoguciPoljaZaMaloletnika(izabraniNalog);

            validacijaJmbg.Visibility = Visibility.Hidden;
            validacijaBrojTelefona.Visibility = Visibility.Hidden;
            validacijaJmbgStaratelja.Visibility = Visibility.Hidden;
            potvrdi.IsEnabled = true;
        }

        public void OmoguciPoljaZaMaloletnika(Pacijent izabraniNalog)
        {
            if (izabraniNalog.Maloletnik == true)
            {
                maloletnik.IsChecked = true;
                jmbgStaratelja.IsEnabled = true;
                jmbgStaratelja.Text = izabraniNalog.JmbgStaratelja.ToString();
            }
            else
            {
                maloletnik.IsChecked = false;
                jmbgStaratelja.IsEnabled = false;
            }
        }

        public void OnemoguciPoljaZaGuestNalog(Pacijent izabraniNalog)
        {
            if (izabraniNalog.StatusNaloga.Equals(statusNaloga.Stalni))
            {
                statusPacijenta.SelectedIndex = 0;
                statusPacijenta.IsEnabled = false;
                brojTelefona.IsEnabled = true;
                email.IsEnabled = true;
                adresa.IsEnabled = true;
                bracnoStanjePacijenta.IsEnabled = true;
                zanimanje.IsEnabled = true;
            }
            else if (izabraniNalog.StatusNaloga.Equals(statusNaloga.Guest))
            {
                statusPacijenta.SelectedIndex = 1;
                statusPacijenta.IsEnabled = true;
                brojTelefona.IsEnabled = false;
                email.IsEnabled = false;
                adresa.IsEnabled = false;
                bracnoStanjePacijenta.IsEnabled = false;
                zanimanje.IsEnabled = false;
            }
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            statusNaloga status = PacijentiServis.OdrediStatusNaloga(statusPacijenta.Text);
            pol pol = PacijentiServis.OdreditiPolPacijenta(polPacijenta.Text);
            bracnoStanje brStanje = PacijentiServis.OdreditiBracnoStanje(bracnoStanjePacijenta.SelectedIndex, polPacijenta.Text);
            bool maloletnoLice = PacijentiServis.MaloletnoLice((bool)maloletnik.IsChecked);
            long staratelj = PacijentiServis.OdrediJmbgStaratelja(jmbgStaratelja.Text);

            if (statusPacijenta.SelectedIndex == 0) // stalan nalog
            {
                Pacijent noviPacijent = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, long.Parse(jmbg.Text), pol, long.Parse(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje, maloletnoLice, staratelj);
                servis.IzmeniNalog(pacijent, noviPacijent);
            }
            else
            {
                Pacijent guestPacijent = new Pacijent(pacijent.IdPacijenta, ime.Text, prezime.Text, long.Parse(jmbg.Text), pol, status);
                servis.IzmeniNalog(pacijent, guestPacijent);    
            }

            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Combo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (statusPacijenta.Text.Equals("GUEST"))
            {
                brojTelefona.IsEnabled = false;
                email.IsEnabled = false;
                adresa.IsEnabled = false; 
                zanimanje.IsEnabled = false;
                bracnoStanjePacijenta.IsEnabled = false;
                maloletnik.IsEnabled = false;
                jmbgStaratelja.IsEnabled = false;

                if (flag1 == true && flag2 == true && flag6 == true)
                {
                    potvrdi.IsEnabled = true;
                }
                else
                {
                    potvrdi.IsEnabled = false;
                }
            }
            else if (statusPacijenta.Text.Equals("STALAN"))
            {
                brojTelefona.IsEnabled = true;
                email.IsEnabled = true;
                adresa.IsEnabled = true;
                zanimanje.IsEnabled = true;
                bracnoStanjePacijenta.IsEnabled = true;
                maloletnik.IsEnabled = true;

                if ((bool)maloletnik.IsChecked)
                {
                    jmbgStaratelja.IsEnabled = true;

                    if (jmbgStaratelja.Text.Length > 0)
                    {
                        flag8 = true;
                    }
                }
                else
                {
                    jmbgStaratelja.IsEnabled = false;
                    flag8 = true;
                }

                if (flag1 == true && flag2 == true && flag3 == true && flag4 == true 
                    && flag5 == true && flag6 == true && flag7 == true && flag8 == true)
                {
                    potvrdi.IsEnabled = true;
                }
                else
                {
                    potvrdi.IsEnabled = false;
                }
            }
        }

        private void Maloletnik_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((bool)maloletnik.IsChecked)
            {
                jmbgStaratelja.IsEnabled = true;

                if (jmbgStaratelja.Text.Length == 0)
                {
                    flag8 = false;
                }
                else
                {
                    flag8 = true;
                }

                if (flag1 == true && flag2 == true && flag3 == true && flag4 == true
                    && flag5 == true && flag6 == true && flag7 == true && flag8 == true)
                {
                    potvrdi.IsEnabled = true;
                }
                else
                {
                    potvrdi.IsEnabled = false;
                }
            }
            else
            {
                jmbgStaratelja.IsEnabled = false;
                flag8 = true;

                if (flag1 == true && flag2 == true && flag3 == true && flag4 == true
                    && flag5 == true && flag6 == true && flag7 == true && flag8 == true)
                {
                    potvrdi.IsEnabled = true;
                }
                else
                {
                    potvrdi.IsEnabled = false;
                }
            }
        }

        private void Ime_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flag1 = false;
                potvrdi.IsEnabled = false;
            }
            else
            {
                flag1 = true;
                if ((flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true 
                    && flag6 == true && flag7 == true && flag8 == true && statusPacijenta.Text.Equals("STALAN"))
                    || (flag1 == true && flag2 == true && flag6 == true && statusPacijenta.Text.Equals("GUEST")))
                {
                    potvrdi.IsEnabled = true;
                }
            }
        }

        private void Prezime_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flag2 = false;
                potvrdi.IsEnabled = false;
            }
            else
            {
                flag2 = true;
                if ((flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true 
                    && flag6 == true && flag7 == true && flag8 == true && statusPacijenta.Text.Equals("STALAN"))
                    || (flag1 == true && flag2 == true && flag6 == true && statusPacijenta.Text.Equals("GUEST")))
                {
                    potvrdi.IsEnabled = true;
                }
            }
        }

        private void Adresa_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flag3 = false;
                potvrdi.IsEnabled = false;
            }
            else
            {
                flag3 = true;
                if (flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true 
                    && flag6 == true && flag7 == true && flag8 == true && statusPacijenta.Text.Equals("STALAN"))
                {
                    potvrdi.IsEnabled = true;
                }
            }
        }

        private void Email_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flag4 = false;
                potvrdi.IsEnabled = false;
            }
            else
            {
                flag4 = true;
                if (flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true 
                    && flag6 == true && flag7 == true && flag8 == true && statusPacijenta.Text.Equals("STALAN"))
                {
                    potvrdi.IsEnabled = true;
                }
            }
        }

        private void Zanimanje_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flag5 = false;
                potvrdi.IsEnabled = false;
            }
            else
            {
                flag5 = true;
                if (flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true 
                    && flag6 == true && flag7 == true && flag8 == true && statusPacijenta.Text.Equals("STALAN"))
                {
                    potvrdi.IsEnabled = true;
                }
            }
        }

        private void Jmbg_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flag6 = false;
                potvrdi.IsEnabled = false;
            }
            else
            {
                long result;
                if (long.TryParse(jmbg.Text, out result))
                {
                    if (jmbg.Text.Length >= 9 && jmbg.Text.Length <= 13)
                    {
                        validacijaJmbg.Visibility = Visibility.Hidden;
                        flag6 = true;
                        if ((flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true 
                            && flag6 == true && flag7 == true && flag8 == true && statusPacijenta.Text.Equals("STALAN"))
                            || (flag1 == true && flag2 == true && flag6 == true && statusPacijenta.Text.Equals("GUEST")))
                        {
                            potvrdi.IsEnabled = true;
                        }
                    }
                    else
                    {
                        flag6 = false;
                        validacijaJmbg.Visibility = Visibility.Visible;
                        potvrdi.IsEnabled = false;
                    }
                }
                else
                {
                    flag6 = false;
                    validacijaJmbg.Visibility = Visibility.Visible;
                    potvrdi.IsEnabled = false;
                }
            }
        }

        private void BrojTelefona_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flag7 = false;
                potvrdi.IsEnabled = false;
            }
            else
            {
                long result;
                if (long.TryParse(brojTelefona.Text, out result))
                {
                    if (brojTelefona.Text.Length >= 6 && brojTelefona.Text.Length <= 10)
                    {
                        validacijaBrojTelefona.Visibility = Visibility.Hidden;
                        flag7 = true;
                        if (flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true 
                            && flag6 == true && flag7 == true && flag8 == true && statusPacijenta.Text.Equals("STALAN"))
                        {
                            potvrdi.IsEnabled = true;
                        }
                    }
                    else
                    {
                        flag7 = false;
                        validacijaBrojTelefona.Visibility = Visibility.Visible;
                        potvrdi.IsEnabled = false;
                    }
                }
                else
                {
                    flag7 = false;
                    validacijaBrojTelefona.Visibility = Visibility.Visible;
                    potvrdi.IsEnabled = false;
                }
            }
        }

        private void JmbgStaratelja_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flag8 = false;
                potvrdi.IsEnabled = false;
            }
            else
            {
                long result;
                if (long.TryParse(jmbgStaratelja.Text, out result))
                {
                    if (jmbgStaratelja.Text.Length >= 9 && jmbgStaratelja.Text.Length <= 13)
                    {
                        validacijaJmbgStaratelja.Visibility = Visibility.Hidden;
                        flag8 = true;
                        if (flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true 
                            && flag6 == true && flag7 == true && flag8 == true && statusPacijenta.Text.Equals("STALAN"))
                        {
                            potvrdi.IsEnabled = true;
                        }
                    }
                    else
                    {
                        flag8 = false;
                        validacijaJmbgStaratelja.Visibility = Visibility.Visible;
                        potvrdi.IsEnabled = false;
                    }
                }
                else
                {
                    flag8 = false;
                    validacijaJmbgStaratelja.Visibility = Visibility.Visible;
                    potvrdi.IsEnabled = false;
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.M && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (!(bool)maloletnik.IsChecked) // ako nije cekiran, cekiraj check box
                {
                    maloletnik.IsChecked = true;
                    jmbgStaratelja.IsEnabled = true;

                    if (jmbgStaratelja.Text.Length == 0)
                    {
                        flag8 = false;
                    }
                    else
                    {
                        flag8 = true;
                    }

                    if (flag1 == true && flag2 == true && flag3 == true && flag4 == true
                        && flag5 == true && flag6 == true && flag7 == true && flag8 == true)
                    {
                        potvrdi.IsEnabled = true;
                    }
                    else
                    {
                        potvrdi.IsEnabled = false;
                    }
                }
                else
                {
                    maloletnik.IsChecked = false;
                    jmbgStaratelja.IsEnabled = false;
                    flag8 = true;

                    if (flag1 == true && flag2 == true && flag3 == true && flag4 == true
                       && flag5 == true && flag6 == true && flag7 == true && flag8 == true)
                    {
                        potvrdi.IsEnabled = true;
                    }
                    else
                    {
                        potvrdi.IsEnabled = false;
                    }
                }
            }
            else if (e.Key == Key.M && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if(!(bool)maloletnik.IsChecked)
                {
                    maloletnik.IsChecked = true;
                    jmbgStaratelja.IsEnabled = true;

                    if (jmbgStaratelja.Text.Length == 0)
                    {
                        flag8 = false;
                    }
                    else
                    {
                        flag8 = true;
                    }

                    if (flag1 == true && flag2 == true && flag3 == true && flag4 == true
                        && flag5 == true && flag6 == true && flag7 == true && flag8 == true)
                    {
                        potvrdi.IsEnabled = true;
                    }
                    else
                    {
                        potvrdi.IsEnabled = false;
                    }
                }
                else
                {
                    maloletnik.IsChecked = false;
                    jmbgStaratelja.IsEnabled = false;
                    flag8 = true;

                    if (flag1 == true && flag2 == true && flag3 == true && flag4 == true
                       && flag5 == true && flag6 == true && flag7 == true && flag8 == true)
                    {
                        potvrdi.IsEnabled = true;
                    }
                    else
                    {
                        potvrdi.IsEnabled = false;
                    }
                }
            }
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.RightCtrl))
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