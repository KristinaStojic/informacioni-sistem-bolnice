using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Tables;
using System.Drawing;
using System.Data;
using Model;
using Projekat.Model;
using LiveCharts;
using System.Collections.ObjectModel;
using Projekat.Servis;

namespace Projekat.ViewModel
{
    class ZdravstveniKartonViewModel : BindableBase
    {
        private ObservableCollection<LekarskiRecept> recepti;
        public ObservableCollection<LekarskiRecept> PrikazRecepata { get { return recepti; } set { recepti = value; OnPropertyChanged("PrikazRecepata"); } }
        private ObservableCollection<Anamneza> anamneze;
        public ObservableCollection<Anamneza> TabelaAnamneza { get { return anamneze; } set { anamneze = value; OnPropertyChanged("TabelaAnamneza"); } }
        private ObservableCollection<Alergeni> alergeni;
        public ObservableCollection<Alergeni> TabelaAlergena { get { return alergeni; } set { alergeni = value; OnPropertyChanged("TabelaAlergena"); } }
        private ObservableCollection<Uput> uputi;
        public ObservableCollection<Uput> TabelaUputa { get { return uputi; } set { uputi = value; OnPropertyChanged("TabelaUputa"); } }

        public MyICommand GenerisiIzvestajRecepataKomanda { get; set; }
        public MyICommand GenerisiIzvestajAnamnezeKomanda { get; set; }

        public static Window GenerisiIzvestajLekar { get; set; }

        private static Pacijent izabraniPacijent;
        public Pacijent IzabraniPacijent
        {
            get { return izabraniPacijent; }
            set
            {
                izabraniPacijent = value;
            }
        }
        public ZdravstveniKartonViewModel()
        {
            inicijalizujTabele();
            GenerisiIzvestajRecepataKomanda = new MyICommand(GenerisiIzvestajRecepata);
            GenerisiIzvestajAnamnezeKomanda = new MyICommand(GenerisiIzvestajAnamneze);
            PieChart();
        }
        private void inicijalizujTabele()
        {
            foreach(Pacijent p in PacijentiServis.pacijenti())
            {
                if (p.ImePacijenta.Equals("Kristina") && p.PrezimePacijenta.Equals("Draskovic")){
                    izabraniPacijent = p;
                }
            }

            PrikazRecepata = new ObservableCollection<LekarskiRecept>();
            foreach (Pacijent p in PacijentiServis.pacijenti())
            {
                if (p.IdPacijenta == izabraniPacijent.IdPacijenta)
                {
                    foreach (LekarskiRecept lr in p.Karton.LekarskiRecepti)
                    {
                        PrikazRecepata.Add(lr);
                    }
                }
            }


            TabelaAnamneza = new ObservableCollection<Anamneza>();
            foreach (Pacijent p in PacijentiServis.pacijenti())
            {
                if (p.IdPacijenta == izabraniPacijent.IdPacijenta)
                {
                    foreach (Anamneza an in p.Karton.Anamneze)
                    {
                        TabelaAnamneza.Add(an);
                    }
                }
            }

            TabelaAlergena = new ObservableCollection<Alergeni>();
            foreach (Pacijent p in PacijentiServis.pacijenti())
            {
                if (p.IdPacijenta == izabraniPacijent.IdPacijenta)
                {
                    foreach (Alergeni an in p.Karton.Alergeni)
                    {
                        TabelaAlergena.Add(an);
                    }
                }
            }

            TabelaUputa = new ObservableCollection<Uput>();
            foreach (Pacijent p in PacijentiServis.pacijenti())
            {
                if (p.IdPacijenta == izabraniPacijent.IdPacijenta)
                {
                    foreach (Uput uput in p.Karton.Uputi)
                    {
                        TabelaUputa.Add(uput);
                    }
                }
            }

        }
        private ChartValues<int> _ukupnoLabUputa;
        private ChartValues<int> _ukupnoSpecUputa;
        private ChartValues<int> _ukupnoBolUputa;


        public ChartValues<int> UkupnoLabUputa
        {
            get { return _ukupnoLabUputa; }
            set { _ukupnoLabUputa = value; OnPropertyChanged("UkupnoSobaZaPacijente"); }
        }

        public ChartValues<int> UkupnoSpecUputa
        {
            get { return _ukupnoSpecUputa; }
            set { _ukupnoSpecUputa = value; OnPropertyChanged("UkupnoSobaZaPreglede"); }
        }

        public ChartValues<int> UkupnoBolUputa
        {
            get { return _ukupnoBolUputa; }
            set { _ukupnoBolUputa = value; OnPropertyChanged("UkupnoOperacionihSala"); }
        }

        public Func<ChartPoint, string> LabelPoint { get; set; }

        private void PieChart()
        {
            LabelPoint = chartPoint => string.Format("{0}({1:P})", chartPoint.Y, chartPoint.Participation);
        }
        private void GenerisiIzvestajRecepata()
        {
            using (PdfDocument doc = new PdfDocument())
            {
                //Add a page to the document
                PdfPage page = doc.Pages.Add();

                // Create a PdfLightTable.
                PdfLightTable pdfLightTable = new PdfLightTable();

                // Initialize DataTable to assign as DateSource to the light table.
                DataTable table = new DataTable();

                //Include columns to the DataTable.
                table.Columns.Add("Pacijent");

                table.Columns.Add("Datum izdavanja");

                table.Columns.Add("Naziv leka");
                table.Rows.Add(new string[] { "Pacijent", "Datum izdavanja recepta", "Naziv leka" });
                //Include rows to the DataTable.
                foreach (Pacijent pacijent in PacijentiServis.pacijenti())
                {
                    if (pacijent.Karton.LekarskiRecepti != null)
                    {
                        foreach (LekarskiRecept recept in pacijent.Karton.LekarskiRecepti)
                        {
                            table.Rows.Add(new string[] { pacijent.ImePacijenta + " " + pacijent.PrezimePacijenta, recept.DatumPropisivanjaLeka, recept.NazivLeka });
                        }
                    }
                    
                    
                }


                //Assign data source.
                pdfLightTable.DataSource = table;

                //Draw PdfLightTable.
                pdfLightTable.Draw(page, new PointF(0, 0));

                //Save the document
                doc.Save("C:\\SIMS projekat bolnica\\informacioni-sistem-bolnice\\KT3\\IzvestajRecepata.pdf");

                doc.Close();
            }
            MessageBox.Show("PDF fajl uspesno izgenerisan!");
        }
        
        private void GenerisiIzvestajAnamneze()
        {
            using (PdfDocument doc = new PdfDocument())
            {
                //Add a page to the document
                PdfPage page = doc.Pages.Add();

                // Create a PdfLightTable.
                PdfLightTable pdfLightTable = new PdfLightTable();

                // Initialize DataTable to assign as DateSource to the light table.
                DataTable table = new DataTable();

                //Include columns to the DataTable.
                table.Columns.Add("Pacijent");

                table.Columns.Add("Datum izdavanja");

                table.Columns.Add("Naziv leka");
                table.Rows.Add(new string[] { "Pacijent", "Datum izdavanja anamneze", "Lekar koji je izdao anamnezu" });
                //Include rows to the DataTable.
                foreach (Pacijent pacijent in PacijentiServis.pacijenti())
                {
                    if (pacijent.Karton.Anamneze != null)
                    {
                        foreach (Anamneza anamneza in pacijent.Karton.Anamneze)
                        {
                            table.Rows.Add(new string[] { pacijent.ImePacijenta + " " + pacijent.PrezimePacijenta, anamneza.Datum, anamneza.ImePrezimeLekara });
                        }
                    }
                    
                    
                }


                //Assign data source.
                pdfLightTable.DataSource = table;

                //Draw PdfLightTable.
                pdfLightTable.Draw(page, new PointF(0, 0));

                //Save the document
                doc.Save("C:\\SIMS projekat bolnica\\informacioni-sistem-bolnice\\KT3\\IzvestajAnamneza.pdf");

                doc.Close();
            }
            MessageBox.Show("PDF fajl uspesno izgenerisan!");
        }

        private int _brojacSpecijastickihUputa;
        private int _brojacLaboratorijskihUputa;
        private int _brojacBolnickoLecenjeUputa;


        public int BrojacSpecijastickihUputa
        {
            get { return _brojacSpecijastickihUputa; }
            set { _brojacSpecijastickihUputa = value; OnPropertyChanged("BrojacSpecijastickihUputa"); }
        }

        public int BrojacLaboratorijskihUputa
        {
            get { return _brojacLaboratorijskihUputa; }
            set { _brojacLaboratorijskihUputa = value; OnPropertyChanged("BrojacLaboratorijskihUputa"); }
        }

        public int BrojacBolnickoLecenjeUputa
        {
            get { return _brojacBolnickoLecenjeUputa; }
            set { _brojacBolnickoLecenjeUputa = value; OnPropertyChanged("BrojacBolnickoLecenjeUputa"); }
        }


        private void podesiBrojOdredjenihProstorija(Uput uput, int koeficijentPravca)
        {
            if (uput.TipUputa.Equals(tipUputa.Laboratorija))
            {
                if (this.UkupnoLabUputa is null)
                    BrojacLaboratorijskihUputa = 1;
                else
                    BrojacLaboratorijskihUputa += koeficijentPravca;
                this.UkupnoLabUputa = new ChartValues<int>() { BrojacLaboratorijskihUputa };

            }
            else if (uput.TipUputa.Equals(tipUputa.SpecijalistickiPregled))
            {
                if (this.UkupnoSpecUputa is null)
                    BrojacSpecijastickihUputa = 1;
                else
                    BrojacSpecijastickihUputa += koeficijentPravca;
                this.UkupnoSpecUputa = new ChartValues<int>() { BrojacSpecijastickihUputa };
            }
            else if (uput.TipUputa.Equals(tipUputa.StacionarnoLecenje))
            {
                if (this.UkupnoBolUputa is null)
                    BrojacBolnickoLecenjeUputa = 1;
                else
                    BrojacBolnickoLecenjeUputa += koeficijentPravca;
                this.UkupnoBolUputa = new ChartValues<int>() { BrojacBolnickoLecenjeUputa };
            }
        }
    }
}
