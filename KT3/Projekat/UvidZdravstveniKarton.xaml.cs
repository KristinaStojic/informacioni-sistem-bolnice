﻿using System;
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

        public UvidZdravstveniKarton(Pacijent izabraniNalog)
        {
            InitializeComponent();
            this.pacijent = izabraniNalog;
            this.DataContext = this;

            if (izabraniNalog != null)
            {
                ime.Text = izabraniNalog.ImePacijenta;
                prezime.Text = izabraniNalog.PrezimePacijenta;
                jmbg.Text = izabraniNalog.Jmbg.ToString();

                if (izabraniNalog.Pol.Equals(pol.M))
                {
                    combo2.SelectedIndex = 0;
                }
                else if (izabraniNalog.Pol.Equals(pol.Z))
                {
                    combo2.SelectedIndex = 1;
                }

                if (izabraniNalog.StatusNaloga.Equals(statusNaloga.Stalni))
                {
                    combo.SelectedIndex = 0;
                }
                else if (izabraniNalog.StatusNaloga.Equals(statusNaloga.Guest))
                {
                    combo.SelectedIndex = 1;
                }

                brojTelefona.Text = izabraniNalog.BrojTelefona.ToString();
                email.Text = izabraniNalog.Email;
                adresa.Text = izabraniNalog.AdresaStanovanja;
                zanimanje.Text = izabraniNalog.Zanimanje;

                if (izabraniNalog.BracnoStanje.Equals(bracnoStanje.Neozenjen) || izabraniNalog.BracnoStanje.Equals(bracnoStanje.Neudata))
                {
                    combo3.SelectedIndex = 0;
                }
                else if (izabraniNalog.BracnoStanje.Equals(bracnoStanje.Ozenjen) || izabraniNalog.BracnoStanje.Equals(bracnoStanje.Udata))
                {
                    combo3.SelectedIndex = 1;
                }
                else if (izabraniNalog.BracnoStanje.Equals(bracnoStanje.Udovac) || izabraniNalog.BracnoStanje.Equals(bracnoStanje.Udovica))
                {
                    combo3.SelectedIndex = 2;
                }
                else if (izabraniNalog.BracnoStanje.Equals(bracnoStanje.Razveden) || izabraniNalog.BracnoStanje.Equals(bracnoStanje.Razvedena))
                {
                    combo3.SelectedIndex = 3;
                }

                ime.IsEnabled = false;
                prezime.IsEnabled = false;
                jmbg.IsEnabled = false;
                combo.IsEnabled = false;
                combo2.IsEnabled = false;
                brojTelefona.IsEnabled = false;
                email.IsEnabled = false;
                adresa.IsEnabled = false;
                combo3.IsEnabled = false;
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

            PrikazRecepata = new ObservableCollection<LekarskiRecept>();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                if (p.IdPacijenta == pacijent.IdPacijenta)
                {
                    foreach (LekarskiRecept lr in p.Karton.LekarskiRecepti)
                    {
                        PrikazRecepata.Add(lr);
                    }
                }
            }

            TabelaAnamneza = new ObservableCollection<Anamneza>();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                if (p.IdPacijenta == pacijent.IdPacijenta)
                {
                    foreach (Anamneza an in p.Karton.Anamneze)
                    {
                        TabelaAnamneza.Add(an);
                    }
                }
            }

            TabelaAlergena = new ObservableCollection<Alergeni>();
            foreach (Pacijent p in PacijentiMenadzer.pacijenti)
            {
                if (p.IdPacijenta == pacijent.IdPacijenta)
                {
                    foreach (Alergeni an in p.Karton.Alergeni)
                    {
                        TabelaAlergena.Add(an);
                    }
                }
            }

            // izabrani lekar
            if (izabraniNalog.IzabraniLekar != null)
            {
                lekar.Text = izabraniNalog.IzabraniLekar.ImeLek + " " + izabraniNalog.IzabraniLekar.PrezimeLek;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        // detalji anamneze
        /*private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Anamneza izabranaAnamneza = (Anamneza)dataGridAnamneze.SelectedItem;

            if (izabranaAnamneza != null)
            {

                DetaljiAnamneze da = new DetaljiAnamneze(izabranaAnamneza, termin);
                da.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali nijednu anamnezu!");
            }
        }*/
    }
}
