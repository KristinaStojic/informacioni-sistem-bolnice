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
    /// Interaction logic for LicniPodaciLekar.xaml
    /// </summary>
    public partial class LicniPodaciLekar : Window
    {
        int IDLekara;
        public bool popunjeno = false;
        public LicniPodaciLekar(int idLekara)
        {
            InitializeComponent();
            this.IDLekara = idLekara;
            PopuniPodatkeLekara();
            validacijaJMBG.Visibility = Visibility.Hidden;
            validacijaTel.Visibility = Visibility.Hidden;
            
            
             specijalizacija.Items.Add(Specijalizacija.Akuserstvo);
             specijalizacija.Items.Add(Specijalizacija.Hirurgija);
             specijalizacija.Items.Add(Specijalizacija.Opsta_praksa);
             specijalizacija.Items.Add(Specijalizacija.Ortopedija);
             specijalizacija.Items.Add(Specijalizacija.Specijalista);
            
        }

        private void PopuniPodatkeLekara()
        {
            foreach (Lekar lekar in LekariMenadzer.lekari)
            {
                if (lekar.IdLekara == IDLekara)
                {
                    this.korIme.Text = lekar.korisnickoIme;
                    this.sifra.Text = lekar.lozinka;
                    this.ime.Text = lekar.ImeLek;
                    this.prezime.Text = lekar.PrezimeLek;
                    this.jmbg.Text = lekar.Jmbg.ToString();
                    this.telefon.Text = lekar.BrojTelefona.ToString();
                    this.email.Text = lekar.Email;
                    this.adresa.Text = lekar.AdresaStanovanja;
                    this.specijalizacija.SelectedItem = lekar.specijalizacija;
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) //Sacuvaj
            {
                Button_Click(sender, e);
            }
            else if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl)) //Nazad
            {
                Button_Click_1(sender, e);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PocetnaStrana p = new PocetnaStrana(IDLekara);
            p.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (popunjeno)
            {
                Lekar stariLekar = null;
                foreach (Lekar lekar in LekariMenadzer.lekari)
                {
                    if (IDLekara == lekar.IdLekara)
                    {
                        stariLekar = lekar;
                    }
                }
                string korisnickoIme = korIme.Text;
                string sifra = this.sifra.Text;
                string imeLekara = ime.Text;
                string prezimeLekara = prezime.Text;
                int jmbgLekara = Int32.Parse(jmbg.Text);
                int brTelefon = Int32.Parse(telefon.Text);
                string emailLekara = email.Text;
                string adresaLekara = adresa.Text;

                string specijalizacijaLekara = specijalizacija.Text;
                /**NAPRAVI IZMENU KOR.IMENA I LOZINKE I ISPRAVI SPECIJALIZACIJU - DODAJ COMBOBOX*/
                Lekar noviLekar = new Lekar(IDLekara, imeLekara, prezimeLekara, jmbgLekara, brTelefon, emailLekara, adresaLekara, stariLekar.specijalizacija, korisnickoIme, sifra);

                if (specijalizacija.Text.Equals(Specijalizacija.Opsta_praksa.ToString()))
                {
                    noviLekar.specijalizacija = Specijalizacija.Opsta_praksa;
                }
                else if (specijalizacija.Text.Equals(Specijalizacija.Akuserstvo.ToString()))
                {
                    noviLekar.specijalizacija = Specijalizacija.Akuserstvo;
                }
                else if (specijalizacija.Text.Equals(Specijalizacija.Hirurgija.ToString()))
                {
                    noviLekar.specijalizacija = Specijalizacija.Hirurgija;
                }
                else if (specijalizacija.Text.Equals(Specijalizacija.Ortopedija.ToString()))
                {
                    noviLekar.specijalizacija = Specijalizacija.Ortopedija;
                }
                else if (specijalizacija.Text.Equals(Specijalizacija.Specijalista.ToString()))
                {
                    noviLekar.specijalizacija = Specijalizacija.Specijalista;
                }

                LekariServis.IzmeniLekara(stariLekar, noviLekar);



                this.Close();

                PocetnaStrana p = new PocetnaStrana(IDLekara);
                p.Show();
            }
            else
            {
                MessageBox.Show("Niste popunili sve podatke!");
            }
            
            
        }

        private void korIme_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void sifra_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void ime_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void prezime_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void jmbg_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNumeric(this.jmbg.Text))
            {
                postaviDugme();
                validacijaJMBG.Visibility = Visibility.Hidden;
            }
            else
            {
                this.potvrdi.IsEnabled = false;
                validacijaJMBG.Visibility = Visibility.Visible;
                this.popunjeno = false;
            }

            
        }

        private void telefon_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNumeric(this.telefon.Text))
            {
                postaviDugme();
                validacijaTel.Visibility = Visibility.Hidden;
            }
            else
            {
                this.potvrdi.IsEnabled = false;
                validacijaTel.Visibility = Visibility.Visible;
                this.popunjeno = false;
            }
           
        }

        private void email_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        private void adresa_TextChanged(object sender, TextChangedEventArgs e)
        {
            postaviDugme();
        }

        public bool IsNumeric(string input)
        {
            long test;
            return long.TryParse(input, out test);
        }


        

        private void postaviDugme()
        {
            if (IsNumeric(this.jmbg.Text) && IsNumeric(this.telefon.Text))
            {
                izvrsiPostavljanje();
            }
            else
            {
                this.potvrdi.IsEnabled = false;
                popunjeno = false;
            }
        }

        private void izvrsiPostavljanje()
        {
            if (this.ime.Text.Trim().Equals("") || this.prezime.Text.Trim().Equals("") || this.korIme.Text.Trim().Equals("") || this.sifra.Text.Trim().Equals("") || this.jmbg.Text.Trim().Equals("") || this.telefon.Text.Trim().Equals("") || this.email.Text.Trim().Equals("") || this.adresa.Text.Trim().Equals("") || this.specijalizacija.Text.Trim().Equals(""))
            {
                this.potvrdi.IsEnabled = false;
                popunjeno = false;
            }
            else if (!this.ime.Text.Trim().Equals("") && !this.prezime.Text.Trim().Equals("") && !this.korIme.Text.Trim().Equals("") && !this.sifra.Text.Trim().Equals("") && !this.jmbg.Text.Trim().Equals("") && !this.telefon.Text.Trim().Equals("") && !this.email.Text.Trim().Equals("") && !this.adresa.Text.Trim().Equals("") && !this.specijalizacija.Text.Trim().Equals(""))
            {
                this.potvrdi.IsEnabled = true;
                popunjeno = true;
            }
        }

    }
}
