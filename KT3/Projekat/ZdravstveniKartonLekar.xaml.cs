﻿using Model;
using Projekat.Model;
using Projekat.Pomoc;
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
using LiveCharts;
using LiveCharts.Wpf;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Tables;
using System.Data;
using System.Drawing;
using System.ComponentModel;
using Projekat.Servis;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for ZdravstveniKartonLekar.xaml
    /// </summary>
    public partial class ZdravstveniKartonLekar : Window, INotifyPropertyChanged
    {
        public Pacijent pacijent;
        public Termin termin;
        public event PropertyChangedEventHandler PropertyChanged;
        ReceptiIzvestaj receptiIzvestaj = new ReceptiIzvestaj();
        AnamnezaIzvestaj anamnezaIzvestaj = new AnamnezaIzvestaj();
        PacijentiServis servis = new PacijentiServis();
        List<Pacijent> sviPacijenti;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
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
        
        public static ObservableCollection<Uput> TabelaUputa
        {
            get;
            set;
        }
        public  ChartValues<int> ukupnoLaboratorija;
        public  ChartValues<int> UkupnoLaboratorija
        {
            get { return ukupnoLaboratorija; }
            set
            {
                ukupnoLaboratorija = value;
                OnPropertyChanged("UkupnoLaboratorija");
            }
        }
        public  ChartValues<int> ukupnoSpecijalisticki;
        public  ChartValues<int> UkupnoSpecijalisticki
        {
            get { return ukupnoSpecijalisticki; }
            set
            {
                ukupnoSpecijalisticki = value;
                OnPropertyChanged("UkupnoSpecijalisticki");
            }
        }
        public ChartValues<int> ukupnoStacionarno;
        public  ChartValues<int> UkupnoStacionarno
        {
            get { return ukupnoStacionarno; }
            set
            {
                ukupnoStacionarno = value;
                OnPropertyChanged("UkupnoStacionarno");
            }
        }

        public Func<ChartPoint, string> LabelPoint { get; set; }
        public ZdravstveniKartonLekar(Pacijent izabraniNalog, Termin termin)
        {
            InitializeComponent();
            LabelPoint = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            
            this.pacijent = izabraniNalog;
            this.termin = termin;
            this.DataContext = this;

            sviPacijenti = servis.PronadjiSve();
            
            if (izabraniNalog != null)
            {
                PopuniLicnePodatkePacijenta(izabraniNalog);
                PopuniTabelePodacima();
            }

        }
        public void PostaviDeloveGrafika(Pacijent izabraniNalog)
        {
            foreach (Pacijent pacijent in sviPacijenti)
            {
                if (pacijent.IdPacijenta == izabraniNalog.IdPacijenta)
                {
                    this.UkupnoLaboratorija = new ChartValues<int>() { pacijent.Karton.brojLaboratorijskihUputa };
                    this.UkupnoSpecijalisticki = new ChartValues<int>() { pacijent.Karton.brojSpecijalistickihUputa };
                    this.UkupnoStacionarno = new ChartValues<int>() { pacijent.Karton.brojBolnickihUputa };
                }
            }

        }
        private void PopuniLicnePodatkePacijenta(Pacijent izabraniNalog)
        {

            PostaviDeloveGrafika(izabraniNalog);

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

            if (izabraniNalog.IzabraniLekar != null)
            {
                lekar.Text = izabraniNalog.IzabraniLekar.ImeLek + " " + izabraniNalog.IzabraniLekar.PrezimeLek;
            }


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

            //Lekar
            if (izabraniNalog.IzabraniLekar != null)
            {
                lekar.Text = izabraniNalog.IzabraniLekar.ImeLek + " " + izabraniNalog.IzabraniLekar.PrezimeLek;
            }

            
        }

        private void PopuniTabelePodacima()
        {
            PrikazRecepata = new ObservableCollection<LekarskiRecept>();
            foreach (Pacijent p in sviPacijenti)
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
            foreach (Pacijent p in sviPacijenti)
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
            foreach (Pacijent p in sviPacijenti)
            {
                if (p.IdPacijenta == pacijent.IdPacijenta)
                {
                    foreach (Alergeni an in p.Karton.Alergeni)
                    {
                        TabelaAlergena.Add(an);
                    }
                }
            }

            TabelaUputa = new ObservableCollection<Uput>();
            foreach (Pacijent p in sviPacijenti)
            {
                if (p.IdPacijenta == pacijent.IdPacijenta)
                {
                    foreach (Uput uput in p.Karton.Uputi)
                    {
                        TabelaUputa.Add(uput);
                    }
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DodajAnamnezu da = new DodajAnamnezu(pacijent, termin);
            da.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            PrikazTerminaLekar pt = new PrikazTerminaLekar(termin.Lekar.IdLekara);
            pt.Show();
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DodajRecept rec = new DodajRecept(pacijent, termin);
            rec.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            DodajAlergene da = new DodajAlergene(pacijent, termin);
            da.Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Alergeni izabraniAlergen = (Alergeni)dataGridAlergeni.SelectedItem;

            if (izabraniAlergen != null)
            {

                DetaljiAlergena da = new DetaljiAlergena(izabraniAlergen, termin);
                da.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali nijedan alergen!");
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            DodajSpecijalistickiUput dodajUput = new DodajSpecijalistickiUput(pacijent,termin);
            dodajUput.Show();
        }

        private void Button_Detalji(object sender, RoutedEventArgs e)
        {
            Uput izabraniUput = (Uput)dataGridUputi.SelectedItem;

            if (izabraniUput != null)
            {

                DetaljiUputa detalji = new DetaljiUputa(izabraniUput);
                detalji.Show();
            }
            else
            {
                MessageBox.Show("Niste selektovali nijedan alergen!");
            }
           
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            ZdravstveniKartonPomoc pomoc = new ZdravstveniKartonPomoc();
            pomoc.Show();
        }

        private void Tabovi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A && Keyboard.IsKeyDown(Key.LeftCtrl)) //Anamneza
            {
                anamneze.IsSelected = true;
            }
            else if (e.Key == Key.R && Keyboard.IsKeyDown(Key.LeftCtrl)) //Recepti
            {
                recepti.IsSelected = true;
            }
            else if (e.Key == Key.U && Keyboard.IsKeyDown(Key.LeftCtrl)) //Uputi
            {
                uputi.IsSelected = true;
            }
            else if (e.Key == Key.P && Keyboard.IsKeyDown(Key.LeftCtrl)) //Podaci
            {
                podaci.IsSelected = true;
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) //Nazad
            {
                Button_Click_2(sender, e);
            }
            else if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl)) //Pomoc
            {
                //Hyperlink_Click(sender, e);
            }
            else if (e.Key == Key.M && Keyboard.IsKeyDown(Key.LeftCtrl)) // Alergeni
            {
                alergeni.IsSelected = true;
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) // Alergeni
            {
                this.Close();
            }
        }

        private void anamneze_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D && Keyboard.IsKeyDown(Key.LeftCtrl)) //Detalji
            {
                Button_Click_1(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl)) //Nova anamneza
            {
                Button_Click(sender, e);
            }
            else if (e.Key == Key.I && Keyboard.IsKeyDown(Key.LeftCtrl)) //Izvestaj
            {
                Izvestaj_Anamneze(sender, e);
            }
            else if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl)) //Nazad
            {
                //Button_Click_2(sender, e);
            }
            else if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl)) 
            {
                Anamneza_Click(sender, e);
            }
        }
        private void Anamneza_Click(object sender, RoutedEventArgs e)
        {
            AnamnezaPomoc ap = new AnamnezaPomoc();
            ap.Show();
        }
        
        private void Alergeni_Click(object sender, RoutedEventArgs e)
        {
            AlergeniPomoc ap = new AlergeniPomoc();
            ap.Show();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                this.Close();
            }
            else if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl)) 
            {
                //Hyperlink_Click(sender, e);
            }
        }

        private void podaci_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl)) //Pomoc
            {
                Hyperlink_Click(sender, e);
            }
        }

        private void recepti_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl)) 
            {
                Button_Click_3(sender, e);
            }
            else if (e.Key == Key.I && Keyboard.IsKeyDown(Key.LeftCtrl)) 
            {
                Izvestaj_Recepti(sender, e);
            }
            else if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Recepti_Pomoc(sender, e);
            }
        }

        private void Recepti_Pomoc(object sender, RoutedEventArgs e)
        {
            ReceptiPomoc rp = new ReceptiPomoc();
            rp.Show();
        }
        private void Uputi_Pomoc(object sender, RoutedEventArgs e)
        {
            UputiPomoc rp = new UputiPomoc();
            rp.Show();
        }

        private void uputi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl)) 
            {
                Button_Click_6(sender, e);
            }
            else if (e.Key == Key.D && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Detalji(sender, e);
            }
            else if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Uputi_Pomoc(sender, e);
            }
        }

        /*private void Izvestaj_Recepti(object sender, RoutedEventArgs e)
        {
            using (PdfDocument doc = new PdfDocument())
            {
                //Add a page to the document
                PdfPage page = doc.Pages.Add();

                // Create a PdfLightTable.
                PdfLightTable pdfLightTable = new PdfLightTable();

                DataTable tabelaNaslov = new DataTable();

               
                tabelaNaslov.Columns.Add("Datum");
                tabelaNaslov.Columns.Add("Vreme");
                tabelaNaslov.Columns.Add("Vrsta");
                tabelaNaslov.Rows.Add(new string[] { "Datum izdavanja izvestaja", "Vreme izdavanja izvestaja", "Vrsta izvestaja"});
                tabelaNaslov.Rows.Add(new string[] { DateTime.Now.Date.ToString().Split(' ')[0], DateTime.Now.Hour + ":" + DateTime.Now.Minute, "Izvestaj recepata pacijenta"});
                // Initialize DataTable to assign as DateSource to the light table.
                DataTable table = new DataTable();

                //Include columns to the DataTable.
                table.Columns.Add("Pacijent");

                table.Columns.Add("Datum izdavanja");

                table.Columns.Add("Naziv leka");

                table.Columns.Add("Broj dana koriscenja");

                table.Columns.Add("Dnevna kolicina");
                table.Rows.Add(new string[] { "Pacijent", "Datum izdavanja recepta", "Naziv leka", "Broj dana koriscenja", "Dnevna kolicina" });
                //Include rows to the DataTable.
                foreach (Pacijent p in PacijentiServis.pacijenti())
                {
                    if(p.IdPacijenta == pacijent.IdPacijenta)
                    {
                        if (pacijent.Karton.LekarskiRecepti != null)
                        {
                            foreach (LekarskiRecept recept in p.Karton.LekarskiRecepti)
                            {
                                table.Rows.Add(new string[] { pacijent.ImePacijenta + " " + pacijent.PrezimePacijenta, recept.DatumPropisivanjaLeka, recept.NazivLeka, recept.BrojDanaKoriscenja.ToString(), recept.DnevnaKolicina.ToString()});
                            }
                        }
                    }
                    

                }

                pdfLightTable.DataSource = tabelaNaslov;
                //Assign data source.
                pdfLightTable.DataSource = table;
                

                //Draw PdfLightTable.
                pdfLightTable.Draw(page, new PointF(0, 0));

                //Save the document
                doc.Save("C:\\SIMS projekat bolnica\\informacioni-sistem-bolnice\\KT3\\IzvestajRecepata" + pacijent.ImePacijenta + pacijent.PrezimePacijenta + ".pdf");

                doc.Close();
            }
            MessageBox.Show("PDF fajl uspesno izgenerisan!");
        }*/

        private void Izvestaj_Recepti(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dtbl = receptiIzvestaj.MakeDataTable(termin);
                String naslov = "Izveštaj recepata@@Pacijent " + pacijent.ImePacijenta + " " + pacijent.PrezimePacijenta;
                naslov = naslov.Replace("@", System.Environment.NewLine);
                String datum = DateTime.Now.ToString("_dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                String putanja = "C:\\SIMS projekat bolnica\\informacioni-sistem-bolnice\\KT3\\IzvestajRecepata" + pacijent.ImePacijenta + pacijent.PrezimePacijenta + ".pdf";
                receptiIzvestaj.ExportDataTableToPdf(dtbl, putanja, naslov);

                if (System.Windows.MessageBox.Show("Izveštaj izgenerisan! Pogledati ga?", "Izveštaj recepata", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start(putanja);
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error Message");
            }

        }

        private void Izvestaj_Anamneze(object sender, RoutedEventArgs e)
        {
            /*using (PdfDocument doc = new PdfDocument())
            {
                //Add a page to the document
                PdfPage page = doc.Pages.Add();

                // Create a PdfLightTable.
                PdfLightTable pdfLightTable = new PdfLightTable();

                // Initialize DataTable to assign as DateSource to the light table.
                DataTable table = new DataTable();

                //Include columns to the DataTable.
                table.Columns.Add("Pacijent");

                table.Columns.Add("Datum izdavanja anamneze");

                table.Columns.Add("Opis bolesti");

                table.Columns.Add("Terapija");

                table.Columns.Add("Lekar koji je izdao anamnezu");
                table.Rows.Add(new string[] { "Pacijent", "Datum izdavanja anamneze", "Opis bolesti", "Terapija",  "Lekar koji je izdao anamnezu" });
                //Include rows to the DataTable.
                foreach (Pacijent p in PacijentiServis.pacijenti())
                {
                    if(p.IdPacijenta == pacijent.IdPacijenta)
                    {
                        if (pacijent.Karton.Anamneze != null)
                        {
                            foreach (Anamneza anamneza in p.Karton.Anamneze)
                            {
                                table.Rows.Add(new string[] { pacijent.ImePacijenta + " " + pacijent.PrezimePacijenta, anamneza.Datum, anamneza.OpisBolesti, anamneza.Terapija, anamneza.ImePrezimeLekara });
                            }
                        }
                    }
                   


                }


                //Assign data source.
                pdfLightTable.DataSource = table;

                //Draw PdfLightTable.
                pdfLightTable.Draw(page, new PointF(0, 0));

                //Save the document
                doc.Save("C:\\SIMS projekat bolnica\\informacioni-sistem-bolnice\\KT3\\IzvestajAnamneza" + pacijent.ImePacijenta + pacijent.PrezimePacijenta + ".pdf");

                doc.Close();
            }
            MessageBox.Show("PDF fajl uspesno izgenerisan!");*/


            try
            {
                DataTable dtbl = anamnezaIzvestaj.MakeDataTable(termin);
                String naslov = "Izveštaj anamneza@@Pacijent " + pacijent.ImePacijenta + " " + pacijent.PrezimePacijenta;
                naslov = naslov.Replace("@", System.Environment.NewLine);
                String datum = DateTime.Now.ToString("_dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                String putanja = "C:\\SIMS projekat bolnica\\informacioni-sistem-bolnice\\KT3\\IzvestajAnamneza" + pacijent.ImePacijenta + pacijent.PrezimePacijenta + ".pdf";
                anamnezaIzvestaj.ExportDataTableToPdf(dtbl, putanja, naslov);

                if (System.Windows.MessageBox.Show("Izveštaj izgenerisan! Pogledati ga?", "Izveštaj recepata", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start(putanja);
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error Message");
            }
        }

        private void alergeni_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_4(sender, e);
            }
            else if (e.Key == Key.D && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Button_Click_5(sender, e);
            }
            else if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Alergeni_Click(sender, e);
            }
        }
    }
}
