using System;
using System.Data;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using System.Text;
using System.Linq;
using Model;

namespace Projekat.Model
{
    public class ReceptiIzvestaj
    {
        
        Lekar izabranLekar = null;

        public void ExportDataTableToPdf(DataTable dtblTable, String strPdfPath, string strHeader)
        {
            System.IO.FileStream fs = new FileStream(strPdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntHead = new Font(bfntHead, 16, 1, BaseColor.BLACK);
            Paragraph prgHeading = new Paragraph();
            prgHeading.Alignment = Element.ALIGN_CENTER;
            prgHeading.Add(new Chunk(strHeader.ToUpper(), fntHead));
            document.Add(prgHeading);

            //Author
            Paragraph prgAuthor = new Paragraph();
            BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntAuthor = new Font(btnAuthor, 8, 2, BaseColor.BLACK);
            prgAuthor.Alignment = Element.ALIGN_RIGHT;
            prgAuthor.Add(new Chunk("Lekar : " + izabranLekar.ImeLek + " " + izabranLekar.PrezimeLek, fntAuthor));
            prgAuthor.Add(new Chunk("\nDatum izdavanja : " + DateTime.Now.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture), fntAuthor));
            document.Add(prgAuthor);

            //Add a line seperation
            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(p);

            //Add line break
            document.Add(new Chunk("\n", fntHead));

            //Write the table
            PdfPTable table = new PdfPTable(dtblTable.Columns.Count);
            //Table header
            BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntColumnHeader = new Font(btnColumnHeader, 10, 1, BaseColor.DARK_GRAY);
            for (int i = 0; i < dtblTable.Columns.Count; i++)
            {
                PdfPCell cell = new PdfPCell();
                cell.BackgroundColor = new BaseColor(153, 204, 255);
                cell.AddElement(new Chunk(dtblTable.Columns[i].ColumnName.ToUpper(), fntColumnHeader));
                table.AddCell(cell);
            }
            //table Data
            for (int i = 0; i < dtblTable.Rows.Count; i++)
            {
                for (int j = 0; j < dtblTable.Columns.Count; j++)
                {
                    table.AddCell(dtblTable.Rows[i][j].ToString());
                }
            }

            document.Add(table);
            document.Close();
            writer.Close();
            fs.Close();
        }

        public DataTable MakeDataTable(Termin izabranTermin)
        {
            this.izabranLekar = LekariMenadzer.NadjiPoId(izabranTermin.Lekar.IdLekara);

            //Create friend table object
            DataTable tabela = new DataTable();

            //Define columns
            tabela.Columns.Add("Datum izdavanja recepta");
            tabela.Columns.Add("Naziv leka");
            string dani = "Broj dana koriscenja";
            string jacina = "Dnevna kolicina leka";
            byte[] bytes = Encoding.Default.GetBytes(dani);
            dani = Encoding.UTF8.GetString(bytes);
            tabela.Columns.Add(dani); 
            
            byte[] bytes2 = Encoding.Default.GetBytes(jacina);
            jacina = Encoding.UTF8.GetString(bytes2);
            tabela.Columns.Add(jacina);

            Pacijent p = PacijentiMenadzer.PronadjiPoId(izabranTermin.Pacijent.IdPacijenta);

            


            foreach(LekarskiRecept r in p.Karton.LekarskiRecepti)
            {
                tabela.Rows.Add(new string[] { r.DatumPropisivanjaLeka,
                        r.NazivLeka, r.BrojDanaKoriscenja.ToString(), r.DnevnaKolicina.ToString() });
            }


            return tabela;
        }
    }
}
