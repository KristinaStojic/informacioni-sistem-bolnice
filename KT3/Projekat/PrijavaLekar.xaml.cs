using Model;
using Projekat.Servis;
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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrijavaLekar.xaml
    /// </summary>
    public partial class PrijavaLekar : Window
    {
        public bool popunjeno = false;
        LekariServis servis = new LekariServis();

        public PrijavaLekar()
        {
            InitializeComponent();
           
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (popunjeno)
            {
                foreach (Lekar lekar in servis.NadjiSveLekare())
                {
                    
                    if (lekar.korisnickoIme.Equals(ime.Text) && lekar.lozinka.Equals(sifra.Text))
                    {
                        int idLekara = lekar.IdLekara;
                        PocetnaStrana ps = new PocetnaStrana(idLekara);
                        ps.Show();
                        this.Close();
                        return;
                    }
                    else if (lekar.korisnickoIme.Equals(ime.Text) && !lekar.lozinka.Equals(sifra.Text))
                    {
                        //MessageBox.Show("Neispravno korisnicko ime i/ili lozinka");
                        //MessageBox.Show("Uneli ste nepostojece podatke!");
                    }
                    else if (!lekar.korisnickoIme.Equals(ime.Text) && lekar.lozinka.Equals(sifra.Text))
                    {
                        //MessageBox.Show("Neispravno korisnicko ime i/ili lozinka");
                        //MessageBox.Show("Uneli ste neispravne podatke!");
                    }

                }
                
                MessageBox.Show("Uneli ste neispravne podatke!");
                
            }
            
        }

        private void ime_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void sifra_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void postaviDugme()
        {
            if (this.sifra.Text != null && this.ime.Text != null)
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
            if (this.sifra.Text.Trim().Equals("") || this.ime.Text.Trim().Equals(""))
            {
                this.potvrdi.IsEnabled = false;
            }
            else if (!this.sifra.Text.Trim().Equals("") && !this.ime.Text.Trim().Equals(""))
            {
                this.potvrdi.IsEnabled = true;
                popunjeno = true;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Potvrdi_Click(sender, e);
            }else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Nazad_Click(sender, e);
            }
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
