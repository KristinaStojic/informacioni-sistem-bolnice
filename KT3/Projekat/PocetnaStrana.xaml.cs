﻿using Model;
using Projekat.Pomoc;
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
    /// Interaction logic for PocetnaStrana.xaml
    /// </summary>
    public partial class PocetnaStrana : Window
    {
        int IDLekara;
        public PocetnaStrana(int idLekara)
        {
            InitializeComponent();
            this.IDLekara = idLekara;
            PopuniPodatkeLekara();
        }

        private void PopuniPodatkeLekara()
        {
            foreach(Lekar lekar in LekariMenadzer.lekari)
            {
                if(lekar.IdLekara == IDLekara)
                {
                    this.ime.Text = lekar.ImeLek;
                    this.prezime.Text = lekar.PrezimeLek;
                    this.jmbg.Text = lekar.Jmbg.ToString();
                    this.telefon.Text = lekar.BrojTelefona.ToString();
                    this.email.Text = lekar.Email;
                    this.adresa.Text = lekar.AdresaStanovanja;
                    this.specijalizacija.Text = lekar.specijalizacija.ToString();
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrikazTerminaLekar pl = new PrikazTerminaLekar(IDLekara);
            pl.Show();
            this.Close();
        }

        private void Button_Zahtevi(object sender, RoutedEventArgs e)
        {

            SpisakZahtevaZaLekove zahtevi = new SpisakZahtevaZaLekove();
            zahtevi.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ObavestenjaLekar o = new ObavestenjaLekar();
            o.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //godisnji odmor
            ZahteviZaGodisnjiLekar zahtev = new ZahteviZaGodisnjiLekar(IDLekara);
            zahtev.Show();
        }

        private void Aplikacija_Click(object sender, RoutedEventArgs e)
        {
            OAplikacijiLekar apl = new OAplikacijiLekar();
            apl.Show();
           
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.P && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click(sender, e);
            }
            else if (e.Key == Key.L && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Zahtevi(sender, e);
            }
            else if (e.Key == Key.G && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_3(sender, e);
            } 
            else if (e.Key == Key.M && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_1(sender, e);
            }
            else if (e.Key == Key.O && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_2(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                this.Close();
            }
            else if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Pomoc_Click(sender, e);
            }else if (e.Key == Key.A && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Aplikacija_Click(sender, e);
            }else if (e.Key == Key.I && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                MenuItem_Click_1(sender, e);
            }

            


        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            PocetnaStranaLekarPomoc pomoc = new PocetnaStranaLekarPomoc();
            pomoc.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            LicniPodaciLekar podaci = new LicniPodaciLekar(IDLekara);
            podaci.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EvidencijaUtrosenogMaterijala em = new EvidencijaUtrosenogMaterijala(IDLekara);
            em.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
            LicniPodaciLekar podaci = new LicniPodaciLekar(IDLekara);
            podaci.Show();
        }
    }
}
