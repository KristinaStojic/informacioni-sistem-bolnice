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
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for HitanSlucajDodajGuest.xaml
    /// </summary>
    public partial class HitanSlucajDodajGuest : Window
    {
        public bool flag1 = false;
        public bool flag2 = false;
        public bool flag3 = false;
        HitanSlucaj hitanSlucaj;
        PacijentiServis servis = new PacijentiServis();

        public HitanSlucajDodajGuest(HitanSlucaj hitanSlucaj)
        {
            InitializeComponent();
            this.hitanSlucaj = hitanSlucaj;
            potvrdi.IsEnabled = false;
            validacija.Visibility = Visibility.Hidden;
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            pol pol = servis.OdreditiPolPacijenta(polPacijenta.Text);
            Pacijent guestPacijent = new Pacijent(servis.GenerisanjeIdPacijenta(), ime.Text, prezime.Text, long.Parse(jmbg.Text), pol, statusNaloga.Guest);
            List<Pacijent> pacijenti = servis.PronadjiSve();
            pacijenti.Add(guestPacijent);
            //PacijentiServis.SacuvajIzmenePacijenta();

            hitanSlucaj.pacijenti.Text = guestPacijent.ImePacijenta + " " + guestPacijent.PrezimePacijenta;
            hitanSlucaj.AzurirajListuPacijenata();
            hitanSlucaj.Pacijent = guestPacijent;

            this.Close();
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Jmbg_LostFocus(object sender, RoutedEventArgs e)
        {
            long result;
            if (long.TryParse(jmbg.Text, out result))
            {
                if (!servis.JedinstvenJmbg(long.Parse(jmbg.Text)))
                {
                    MessageBox.Show("JMBG vec postoji");
                    jmbg.Text = "";
                }
            }
        }

        private void Ime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flag1 = false;
                potvrdi.IsEnabled = false;
            }
            else
            {
                flag1 = true;
                if (flag1 == true && flag2 == true && flag3 == true)
                {
                    potvrdi.IsEnabled = true;
                }
            }
        }

        private void Prezime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flag2 = false;
                potvrdi.IsEnabled = false;
            }
            else
            {
                flag2 = true;
                if (flag1 == true && flag2 == true  && flag3 == true)
                {
                    potvrdi.IsEnabled = true;
                }
            }
        }

        private void Jmbg_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                flag3 = false;
                potvrdi.IsEnabled = false;
            }
            else
            {
                long result;
                if (long.TryParse(jmbg.Text, out result))
                {
                    if (jmbg.Text.Length >= 9 && jmbg.Text.Length <= 13)
                    {
                        validacija.Visibility = Visibility.Hidden;
                        flag3 = true;
                        if (flag1 == true && flag2 == true && flag3 == true)
                        {
                            potvrdi.IsEnabled = true;
                        }
                    }
                    else
                    {
                        flag3 = false;
                        validacija.Visibility = Visibility.Visible;
                        potvrdi.IsEnabled = false;
                    }
                }
                else
                {
                    flag3 = false;
                    validacija.Visibility = Visibility.Visible;
                    potvrdi.IsEnabled = false;
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.O && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Nazad_Click(sender, e);
            }
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Nazad_Click(sender, e);
            }
        }
    }
}
