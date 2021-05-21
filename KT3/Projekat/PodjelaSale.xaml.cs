using Model;
using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PodjelaSale.xaml
    /// </summary>
    public partial class PodjelaSale : Window
    {
        /*Sala staraSala;
        Sala novaSala;
        private int colNum = 0;
        public static ObservableCollection<Oprema> OpremaStaraSala{get; set;}
        public static ObservableCollection<Oprema> OpremaNovaSala { get; set; }
        private List<Oprema> opremaZaPrebacivanje = new List<Oprema>();
        */
        public PodjelaSale(Sala staraSala, Sala novaSala)
        {
            InitializeComponent();
           // inicijalizujElemente(staraSala, novaSala);
            //dodajOpremu();
        }

        /*private void dodajOpremu()
        {
            OpremaStaraSala = new ObservableCollection<Oprema>();
            OpremaNovaSala = new ObservableCollection<Oprema>(); 
            foreach(Oprema oprema in staraSala.Oprema)
            {
                OpremaStaraSala.Add(oprema);
                OpremaNovaSala.Add(oprema);
            }
        }*/

        /*private void inicijalizujElemente(Sala staraSala, Sala novaSala)
        {
            this.DataContext = this;
            this.staraSala = staraSala;
            this.novaSala = novaSala;
            this.staraSalaTekst.Text = "Sala " + staraSala.Namjena + ", br. " + staraSala.brojSale.ToString();
            this.novaSalaTekst.Text = "Sala " + novaSala.Namjena + ", br. " + novaSala.brojSale.ToString();
        }*/

        /*private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }*/

        /* private void Potvrdi_Click(object sender, RoutedEventArgs e)
         {
             Renoviranje.opremaZaPrebacivanje = opremaZaPrebacivanje;
             Renoviranje.salaZaSpajanje = null;
             Renoviranje.novaSala = novaSala;
             this.Close();
         }*/

        /*private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int kolonaKolicine = 1;
            ContentPresenter prezenter = dataGridNova.Columns[kolonaKolicine].GetCellContent(dataGridNova.SelectedItem) as ContentPresenter;
            var templejt = prezenter.ContentTemplate;
            TextBox tekstBoks = templejt.FindName("kolicina", prezenter) as TextBox;
            //SaleViewModel.podesiDugme((Oprema)dataGridNova.SelectedItem, tekstBoks.Text);
        }
        */
        /*private void dodajPrebacivanje(Oprema oprema, int kolicina)
        {
            if (OpremaServis.portojiOpremaZaPrebacivanje(oprema, opremaZaPrebacivanje))
            {
                postaviKolicinuPrebacivanja(oprema, kolicina);
            }
            else
            {
                Oprema opremaZaDodavanje = new Oprema(oprema.NazivOpreme, kolicina, oprema.Staticka);
                opremaZaDodavanje.IdOpreme = oprema.IdOpreme;
                opremaZaPrebacivanje.Add(opremaZaDodavanje);
            }
        }

        private void postaviKolicinuPrebacivanja(Oprema oprema, int kolicina)
        {
            foreach(Oprema opremaPrebacivanje in opremaZaPrebacivanje)
            {
                if(opremaPrebacivanje.IdOpreme == oprema.IdOpreme)
                {
                    opremaPrebacivanje.Kolicina = kolicina;
                }
            }
        }

        private void podesiDugme(Oprema oprema, String kolicina)
        {
            if (!jeBroj(kolicina) && !kolicina.Equals(""))
            {
                this.Potvrdi.IsEnabled = false;
            }
            else if(!kolicina.Equals(""))
            {
                provjeriOpremu(oprema, int.Parse(kolicina));
                dodajPrebacivanje(oprema, int.Parse(kolicina));
            }
        }

        private void provjeriOpremu(Oprema oprema, int kolicina)
        {
            foreach(Oprema opremaSale in staraSala.Oprema) { 
                if(oprema.IdOpreme == opremaSale.IdOpreme)
                {
                    if (opremaSale.Kolicina < kolicina || kolicina < 0)
                    {
                        this.Potvrdi.IsEnabled = false;
                    }
                    else
                    {
                        this.Potvrdi.IsEnabled = true;
                    }
                }
            }
        }
        */
        /*public bool jeBroj(string tekst)
        {
            int test;
            return int.TryParse(tekst, out test);
        }*/
    }
}
