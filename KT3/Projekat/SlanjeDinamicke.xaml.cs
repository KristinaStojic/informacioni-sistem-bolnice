using Model;
using Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SlanjeDinamicke.xaml
    /// </summary>
    public partial class SlanjeDinamicke : Window
    {
        public ObservableCollection<Sala> sale { get; set; }
        Oprema opremaZaSlanje;
        Sala salaIzKojeSaljem;
        public SlanjeDinamicke(Sala izabranaSala, Oprema kojuSaljem)
        {
            InitializeComponent();
            this.opremaZaSlanje = kojuSaljem;
            this.salaIzKojeSaljem = izabranaSala;
            this.tekst.Text = kojuSaljem.NazivOpreme;
            this.DataContext = this;
            sale = new ObservableCollection<Sala>();
            foreach(Sala s in SaleMenadzer.sale)
            {
                if (s.Id != izabranaSala.Id)
                {
                    sale.Add(s);
                }
            }
            this.maks.Text = "MAX: " + kojuSaljem.Kolicina.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Sala salaUKojuSaljem = (Sala)kombo.SelectedItem;
            int kolicina = int.Parse(KOlicina.Text);
            int x = 0;
            foreach(Sala s in SaleMenadzer.sale)
            {
                if(s.Id == salaIzKojeSaljem.Id)
                {
                    foreach(Oprema o in s.Oprema)
                    {
                        if(o.IdOpreme == opremaZaSlanje.IdOpreme)
                        {   
                            if (o.Kolicina - kolicina == 0)
                            {
                                s.Oprema.Remove(o);
                                PrikazDinamicke.OpremaDinamicka.Remove(o);
                                break;
                            }
                            else
                            {
                                o.Kolicina -= kolicina;
                                int idx = PrikazDinamicke.OpremaDinamicka.IndexOf(o);
                                PrikazDinamicke.OpremaDinamicka.RemoveAt(idx);
                                PrikazDinamicke.OpremaDinamicka.Insert(idx, o);
                            }
                        
                        }
                    }
                }
                if(s.Id == salaUKojuSaljem.Id)
                {
                    foreach(Oprema o in s.Oprema)
                    {
                        if(o.IdOpreme == opremaZaSlanje.IdOpreme)
                        {
                            o.Kolicina += kolicina;
                            x += 1;
                        }
                    }
                    if(x == 0)
                    {
                        Oprema op = new Oprema(opremaZaSlanje.NazivOpreme, kolicina, false);
                        op.IdOpreme = opremaZaSlanje.IdOpreme;
                        s.Oprema.Add(op);
                    }
                }
            }
            this.Close();
        }
    }
}
