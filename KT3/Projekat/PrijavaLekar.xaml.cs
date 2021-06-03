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
        public PrijavaLekar()
        {
            InitializeComponent();
           
        }

        

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (popunjeno)
            {
                foreach (Lekar lekar in LekariServis.NadjiSveLekare())
                {
                    
                    if (lekar.korisnickoIme.Equals(ime.Text) && lekar.lozinka.Equals(sifra.Text))
                    {
                        int idLekara = lekar.IdLekara;
                        PocetnaStrana ps = new PocetnaStrana(idLekara);
                        ps.Show();
                        this.Close();
                    }
                    else if (lekar.korisnickoIme.Equals(ime.Text) && !lekar.lozinka.Equals(sifra.Text))
                    {
                        MessageBox.Show("Neispravno korisnicko ime i/ili lozinka");
                    }
                    else if (!lekar.korisnickoIme.Equals(ime.Text) && lekar.lozinka.Equals(sifra.Text))
                    {
                        MessageBox.Show("Neispravno korisnicko ime i/ili lozinka");
                    }

                }
                
                MessageBox.Show("Uneli ste nepostojece podatke!");
                
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
    }
}
