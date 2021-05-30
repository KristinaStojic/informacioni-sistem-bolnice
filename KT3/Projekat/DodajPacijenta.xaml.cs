﻿using System;
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
using Projekat.Model;
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for DodajPacijenta.xaml
    /// </summary>

    public partial class DodajPacijenta : Window //, INotifyPropertyChanged
    {
        public bool flag1 = false;
        public bool flag2 = false;
        public bool flag3 = false;
        public bool flag4 = false;
        public bool flag5 = false;
        public bool flag6 = false;
        public bool flag7 = false;
        public bool flag8 = true;

      /*  public string validacijaJmbg;
        public string validacijaJmbgStaratelja;
        public string validacijaBrojTelefona;  */

        public DodajPacijenta()
        {
            InitializeComponent();
            this.DataContext = this;
            jmbgStaratelja.IsEnabled = false;
            potvrdi.IsEnabled = false;
            validacijaJmbg.Visibility = Visibility.Hidden;
            validacijaBrojTelefona.Visibility = Visibility.Hidden; 
            validacijaJmbgStaratelja.Visibility = Visibility.Hidden;
        }

    /*    public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name, bool flag_)
        {
            if (PropertyChanged != null)
            {
               // flag_ = true;
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
            else
            {
                flag_ = false;
            }
            Console.WriteLine(flag_);
        }

       public string ValidacijaJmbg
        {
            get
            {
                return validacijaJmbg;
            }
            set
            {
                if (value != validacijaJmbg)
                {
                    validacijaJmbg = value;
                    OnPropertyChanged("ValidacijaJmbg"/*, flag6);
                }
            }
        }

        public string ValidacijaBrojTelefona
        {
            get
            {
                return validacijaBrojTelefona;
            }
            set
            {
                if (value != validacijaBrojTelefona)
                {
                    validacijaBrojTelefona = value;
                    OnPropertyChanged("ValidacijaBrojTelefona"/*, flag7);
                }
            }
        }

        public string ValidacijaJmbgStaratelja
        {
            get
            {
                return validacijaJmbgStaratelja;
            }
            set
            {
                if (value != validacijaJmbgStaratelja)
                {
                    validacijaJmbgStaratelja = value;
                    OnPropertyChanged("ValidacijaJmbgStaratelja"/*, flag8);
                }
            }
        } */

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        { 
            statusNaloga status = PacijentiServis.OdrediStatusNaloga(combo.Text);
            pol pol = PacijentiServis.OdreditiPolPacijenta(polPacijenta.Text);
            bracnoStanje brStanje = PacijentiServis.OdreditiBracnoStanje(bracnoStanjePacijenta.SelectedIndex, polPacijenta.Text);
            bool maloletnoLice = PacijentiServis.MaloletnoLice((bool)maloletnik.IsChecked);
            long staratelj = PacijentiServis.OdrediJmbgStaratelja(jmbgStaratelja.Text);

            if (status.Equals(statusNaloga.Guest))
            {
                Pacijent guestPacijent = new Pacijent(PacijentiServis.GenerisanjeIdPacijenta(), ime.Text, prezime.Text, long.Parse(jmbg.Text), pol, status);
                PacijentiServis.DodajNalog(guestPacijent);
            }
            else
            {
                Pacijent pacijent = new Pacijent(PacijentiServis.GenerisanjeIdPacijenta(), ime.Text, prezime.Text, long.Parse(jmbg.Text), pol, long.Parse(brojTelefona.Text), email.Text, adresa.Text, status, zanimanje.Text, brStanje, maloletnoLice, staratelj);
                PacijentiServis.DodajNalog(pacijent);
            }    

            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Combo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (combo.Text.Equals("GUEST")) 
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
            else if (combo.Text.Equals("STALAN"))
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
                }
                else
                {
                    jmbgStaratelja.IsEnabled = false;
                }

                if (flag1 == true && flag2 == true && flag3 == true && flag4 == true &&
                    flag5 == true && flag6 == true && flag7 == true && flag8 == true)
                {
                    potvrdi.IsEnabled = true;
                }
                else
                {
                    potvrdi.IsEnabled = false;
                }
            }
        }

        private void Jmbg_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!(jmbg.Text.Equals("")))
            {
                long result;
                if (long.TryParse(jmbg.Text, out result))
                {
                    if ((!PacijentiServis.JedinstvenJmbg(long.Parse(jmbg.Text))))
                    {
                        MessageBox.Show("Uneseni JMBG vec postoji!");
                        jmbg.Text = "";
                    }
                }               
            }
        }

        private void Maloletnik_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((bool)maloletnik.IsChecked)
            {
                jmbgStaratelja.IsEnabled = true;
                jmbgStaratelja.Focusable = true;
                flag8 = false;
                potvrdi.IsEnabled = false;
            }
            else 
            {
                jmbgStaratelja.IsEnabled = false;
                flag8 = true;
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
                if ( (flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true && flag6 == true 
                    && flag7 == true && flag8 == true && combo.Text.Equals("STALAN"))
                    || (flag1 == true && flag2 == true && flag6 == true && combo.Text.Equals("GUEST")) )
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
                if ( (flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true && flag6 == true 
                    && flag7 == true && flag8 == true && combo.Text.Equals("STALAN"))
                    || (flag1 == true && flag2 == true && flag6 == true && combo.Text.Equals("GUEST")) )
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
                if (flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true && flag6 == true 
                    && flag7 == true && flag8 == true && combo.Text.Equals("STALAN"))
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
                if (flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true && flag6 == true 
                    && flag7 == true && flag8 == true && combo.Text.Equals("STALAN"))
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
                if (flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true && flag6 == true 
                    && flag7 == true && flag8 == true && combo.Text.Equals("STALAN"))
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
                        if ( (flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true && flag6 == true 
                                && flag7 == true && flag8 == true && combo.Text.Equals("STALAN"))
                            || (flag1 == true && flag2 == true && flag6 == true && combo.Text.Equals("GUEST")) )
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
                        if (flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true && flag6 == true 
                            && flag7 == true && flag8 == true && combo.Text.Equals("STALAN"))
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
                        if (flag1 == true && flag2 == true && flag3 == true && flag4 == true && flag5 == true && flag6 == true 
                            && flag7 == true && flag8 == true && combo.Text.Equals("STALAN"))
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

    }
}

