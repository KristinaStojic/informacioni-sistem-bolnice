using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
using Projekat.Servis;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Tables;

namespace Projekat
{
    public partial class Recept : Page
    {
        public Pacijent prijavljeniPacijent;
        public LekarskiRecept lekRec;
        public static int idPacijent;
        public Recept(LekarskiRecept recept, Pacijent izabraniPacijent)
        {
            InitializeComponent();
            this.DataContext = this;
            idPacijent = izabraniPacijent.IdPacijenta;
            InicijalizujPodatkeRecepta(recept, izabraniPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema);
        }

        private void InicijalizujPodatkeRecepta(LekarskiRecept recept, Pacijent izabraniPacijent)
        {
            this.lekRec = recept;
            this.naziv.Text = recept.NazivLeka;
            this.datum.Text = recept.DatumPropisivanjaLeka;
            this.dani.Text = recept.BrojDanaKoriscenja.ToString();
            this.brojUzimanja.Text = recept.DnevnaKolicina.ToString();
            this.sati.Text = recept.PocetakKoriscenja.Substring(0, 2);
            this.min.Text = recept.PocetakKoriscenja.Substring(3);

            this.naziv.IsEnabled = false;
            this.datum.IsEnabled = false;
            this.dani.IsEnabled = false;
            this.brojUzimanja.IsEnabled = false;
            this.sati.IsEnabled = false;
            this.min.IsEnabled = false;

            this.prijavljeniPacijent = izabraniPacijent;
            ime.Text = izabraniPacijent.ImePacijenta;
            prezime.Text = izabraniPacijent.PrezimePacijenta;
            id.Text = izabraniPacijent.Jmbg.ToString();
 
            Lekar lekar = LekariServis.NadjiPoId(recept.IdLekara);
            podaciLekara.Text = lekar.ToString();
        }

        private void odjava_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.odjava_Click(this);
        }

        public void karton_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.karton_Click(this, idPacijent);
        }

        public void zakazi_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.zakazi_Click(this, idPacijent);
        }
        public void uvid_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.uvid_Click(this, idPacijent);
        }

        private void pocetna_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.pocetna_Click(this, idPacijent);
        }

        private void anketa_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.anketa_Click(this, idPacijent);
        }

        private void PromeniTemu(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.PromeniTemu(SvetlaTema, tamnaTema);
        }

        private void Korisnik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Korisnik_Click(this, idPacijent);

        }

        private void Jezik_Click(object sender, RoutedEventArgs e)
        {
            PacijentWebStranice.Jezik_Click(Jezik);
        }

        private void Izvestaj_Click(object sender, RoutedEventArgs e)
        {
            using (PdfDocument doc = new PdfDocument())
            {
              
                PdfPage page = doc.Pages.Add();
                PdfLightTable pdfLightTable = new PdfLightTable();
                DataTable table = new DataTable();
                Lekar lekar = LekariServis.NadjiPoId(lekRec.IdLekara);

                RectangleF boundsHeader = new RectangleF(0, 0, doc.Pages[0].GetClientSize().Width, 150);
                PdfPageTemplateElement header = new PdfPageTemplateElement(boundsHeader);
                doc.Template.Top = header;

                PdfBrush brush = new PdfSolidBrush(System.Drawing.Color.FromArgb(255, 0, 0, 0));
                PdfPen pen = new PdfPen(brush, 2F);
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Regular);

                String naslov = " IZVESTAJ ZA LEKARSKI RECEPT: " + lekRec.NazivLeka +  " - PACIJENT: " + prijavljeniPacijent.ImePacijenta + " " + prijavljeniPacijent.PrezimePacijenta  + " - LEKAR:" + lekar.ToString();
                header.Graphics.DrawString(naslov, font, brush, 0, 125);
                header.Graphics.DrawLine(pen, 0, 140, doc.Pages[0].GetClientSize().Width, 140);

                table.Columns.Add("  Naziv leka");
                table.Columns.Add("  Datum izdavanja");
                if (lekRec.UzimanjeTerapije != null)
                {
                    foreach (DateTime datumUzimanja in lekRec.UzimanjeTerapije)
                    {
                        table.Rows.Add(new string[] {"   " +  lekRec.NazivLeka,  "    " + datumUzimanja.ToString("dd/MM/yyyy HH:mm")});
                    }
                }

                pdfLightTable.DataSource = table;
                pdfLightTable.Style.ShowHeader = true;
                PdfCellStyle headerStyle = new PdfCellStyle(new PdfStandardFont(PdfFontFamily.Helvetica, 10), PdfBrushes.OrangeRed, PdfPens.Black);
                pdfLightTable.Style.HeaderStyle = headerStyle;

                pdfLightTable.Draw(page, new PointF(0, 0));

                string imePrezimePacijenta = prijavljeniPacijent.ImePacijenta + prijavljeniPacijent.PrezimePacijenta;
                doc.Save("//mac/Home/Desktop/informacioni-sistem-bolnice/KT3/IzvestajTerapija-" + imePrezimePacijenta +  ".pdf");

                doc.Close();
            }
            if (Jezik.Header.Equals("_en-US"))
            {
                MessageBox.Show("Uspešno ste preuzeli pdf izveštaj o uzimanju terapije", "Informacija");
            }
            else
            {
                MessageBox.Show("You have successfully downloaded the pdf report on taking the therapy", "Information");
            }
        }

    }
}
