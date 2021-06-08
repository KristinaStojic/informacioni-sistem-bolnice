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
        public LicniPodaciLekar(int idLekara)
        {
            InitializeComponent();
            this.IDLekara = idLekara;
            PopuniPodatkeLekara();

            
            
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
            Lekar stariLekar = null; 
            foreach (Lekar lekar in LekariMenadzer.lekari)
            {
                if(IDLekara == lekar.IdLekara)
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
            Lekar noviLekar = new Lekar(IDLekara,imeLekara,prezimeLekara,jmbgLekara, brTelefon, emailLekara,adresaLekara, stariLekar.specijalizacija, korisnickoIme, sifra);

            if (specijalizacija.Text.Equals(Specijalizacija.Opsta_praksa.ToString()))
            {
                noviLekar.specijalizacija = Specijalizacija.Opsta_praksa;
            }
            else if (specijalizacija.Text.Equals(Specijalizacija.Akuserstvo.ToString()))
            {
                noviLekar.specijalizacija = Specijalizacija.Akuserstvo;
            }
            else if(specijalizacija.Text.Equals(Specijalizacija.Hirurgija.ToString()))
            {
                noviLekar.specijalizacija = Specijalizacija.Hirurgija;
            }
            else if(specijalizacija.Text.Equals(Specijalizacija.Ortopedija.ToString()))
            {
                noviLekar.specijalizacija = Specijalizacija.Ortopedija;
            }
            else if(specijalizacija.Text.Equals(Specijalizacija.Specijalista.ToString()))
            {
                noviLekar.specijalizacija = Specijalizacija.Specijalista;
            }

            LekariServis.IzmeniLekara(stariLekar, noviLekar);

            

            this.Close();

            PocetnaStrana p = new PocetnaStrana(IDLekara);
            p.Show();
            
        }
    }
}
