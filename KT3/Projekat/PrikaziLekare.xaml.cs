using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Model;
using Projekat.Pomoc;
using Projekat.Servis;
using System.IO;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Tables;
//using System.Windows.Documents;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrikaziLekare.xaml
    /// </summary>
    public partial class PrikaziLekare : Window
    {
        public PrikaziLekare()
        {
            InitializeComponent();
        }

        private void Pacijenti_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziPacijenta prikazPacijenata = new PrikaziPacijenta();
            prikazPacijenata.Show();
        }

        private void Termini_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            PrikaziTerminSekretar prikazTermina = new PrikaziTerminSekretar();
            prikazTermina.Show();
        }

        private void Oglasna_tabla_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            OglasnaTabla oglasnaTabla = new OglasnaTabla();
            oglasnaTabla.Show();
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            PrikaziLekarePomoc pomoc = new PrikaziLekarePomoc();
            pomoc.Show();
        }

        private void Napusti_uvid_Click(object sender, RoutedEventArgs e)
        {
            canvas2.Visibility = Visibility.Hidden;
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            LekariServis.SacuvajIzmeneLekara();
            this.Close();
            Sekretar pocetnaStrana = new Sekretar();
            pocetnaStrana.Show();
        }
     
        private void TabelaLekara_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            canvas2.Visibility = Visibility.Visible;
            Lekar selektovaniLekar = (Lekar)TabelaLekara.SelectedItem;
        }
       
        private void Radno_vreme_Click(object sender, RoutedEventArgs e)
        {
            Lekar selektovaniLekar = (Lekar)TabelaLekara.SelectedItem;

            if (selektovaniLekar == null)
            {
                MessageBox.Show("Izaberite lekara cije radno vreme zelite da odredite.");
            }
            else
            {
                OdrediRadnoVreme radnoVreme = new OdrediRadnoVreme(selektovaniLekar);
                radnoVreme.Show();
            }        
        }

        private void Godisnji_odmor_Click(object sender, RoutedEventArgs e)
        {
            OdobravanjeGodisnjegOdmora odobravanje = new OdobravanjeGodisnjegOdmora();
            odobravanje.Show();
        }

        private void Komunikacija_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            KomunikacijaSekretar komunikacija = new KomunikacijaSekretar();
            komunikacija.Show();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Z && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Godisnji_odmor_Click(sender, e);
            }
            else if (e.Key == Key.Z && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Godisnji_odmor_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Napusti_uvid_Click(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Napusti_uvid_Click(sender, e);
            }
            else if (e.Key == Key.R && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Radno_vreme_Click(sender, e);
            }
            else if (e.Key == Key.R && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Radno_vreme_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Nazad_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Nazad_Click(sender, e);
            }
            else if (e.Key == Key.G && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Izvestaj_Click(sender, e);
            }
            else if (e.Key == Key.G && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Izvestaj_Click(sender, e);
            }
        }     
       
        private void Izvestaj_Click(object sender, RoutedEventArgs e)
        {
            if (TabelaLekara.SelectedItem != null)
            {
                Lekar selektovaniLekar = (Lekar)TabelaLekara.SelectedItem;

                var pdfDoc = new Document(PageSize.LETTER, 40f, 40f, 60f, 60f);
                string path = $"d:\\Documents\\Teodora\\FAKS\\6. semestar\\SIMS\\Projekat\\informacioni-sistem-bolnice\\KT3\\IzvestajLekara.pdf";
                PdfWriter.GetInstance(pdfDoc, new FileStream(path, FileMode.OpenOrCreate));
                pdfDoc.Open();

                var spacer = new Paragraph("")
                {
                    SpacingBefore = 10f,
                    SpacingAfter = 10f,
                };

                pdfDoc.Add(spacer);

                var tabelaOpisaDokumenta = new PdfPTable(new[] { .75f, 1f }) { };
                tabelaOpisaDokumenta.AddCell("Datum: ");
                tabelaOpisaDokumenta.AddCell(DateTime.Now.Date.ToString().Split(' ')[0]);
                tabelaOpisaDokumenta.AddCell("Vreme: ");
                tabelaOpisaDokumenta.AddCell(DateTime.Now.Hour + ":" + DateTime.Now.Minute);
                tabelaOpisaDokumenta.AddCell("Opis dokumenta: ");
                tabelaOpisaDokumenta.AddCell("Izveštaj o zauzetosti lekara");

                pdfDoc.Add(tabelaOpisaDokumenta);
                pdfDoc.Add(spacer);
                pdfDoc.Add(spacer);

                var sirinaKolona = new[] { 0.75f, 1.5f, 1.5f };

                int brojTermina = 0;
                foreach (Termin termin in TerminMenadzer.termini)
                {
                    if (termin.Lekar.IdLekara == selektovaniLekar.IdLekara)
                    {
                        brojTermina++;
                    }
                }

                if (brojTermina != 0)
                {
                    var tabelaTermina = new PdfPTable(sirinaKolona) { };
                    var zaglavlje = new PdfPCell(new Phrase("Zauzetost lekara" + " " + selektovaniLekar.ImeLek + " " + selektovaniLekar.PrezimeLek))
                    {
                        Colspan = 3,
                        HorizontalAlignment = 1,
                        MinimumHeight = 3
                    };

                    tabelaTermina.AddCell(zaglavlje);

                    tabelaTermina.AddCell("Datum termina");
                    tabelaTermina.AddCell("Vreme pocetka");
                    tabelaTermina.AddCell("Vreme kraja");

                    foreach (Termin termin in TerminMenadzer.termini)
                    {
                        if (termin.Lekar.IdLekara == selektovaniLekar.IdLekara)
                        {
                            tabelaTermina.AddCell(termin.Datum);
                            tabelaTermina.AddCell(termin.VremePocetka);
                            tabelaTermina.AddCell(termin.VremeKraja);
                        }
                    }

                    pdfDoc.Add(tabelaTermina);
                }
                else
                {
                    var nemaTermina = new Paragraph("                Nema zauzetih termina za lekara" + " " +selektovaniLekar.ImeLek + " " + selektovaniLekar.PrezimeLek)
                    {
                        SpacingBefore = 10f,
                        SpacingAfter = 10f,
                    };

                    pdfDoc.Add(nemaTermina);
                }

                pdfDoc.Add(spacer);
                pdfDoc.Add(spacer);
                pdfDoc.Close();

                MessageBox.Show("PDF fajl je uspešno izgenerisan!");
            }
            else
            {
                MessageBox.Show("Izaberite lekara za koga želite da izgenerišete izveštaj.");
            }
        }   
    }
}
