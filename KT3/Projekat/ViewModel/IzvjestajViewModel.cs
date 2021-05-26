using DocumentFormat.OpenXml.Drawing.Charts;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Model;
using Projekat.Model;
using Projekat.Pomoc;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Projekat.ViewModel
{
    public class IzvjestajViewModel : BindableBase
    {
        #region Promjenljive
        public Window PomocProzor { get; set; }
        public MyICommand OtvoriPomoc { get; set; }
        public MyICommand OtvoriZahtjeve { get; set; }
        public MyICommand OtvoriProstorije { get; set; }
        public MyICommand ZatvoriIzvjestaj { get; set; }
        public MyICommand OtvoriOAplikaciji { get; set; }
        public MyICommand OtvoriKomunikaciju { get; set; }
        public static Window IzvjestajProzor { get; set; }
        public MyICommand GenerisiIzvjestajKomanda { get; set; }

        private ObservableCollection<Sala> slobodneSale;

        private ObservableCollection<Sala> zauzeteSale;
        public ObservableCollection<Sala> SlobodneSale { get { return slobodneSale; } set { slobodneSale = value; OnPropertyChanged("SlobodneSale"); } }
        public ObservableCollection<Sala> ZauzeteSale { get { return zauzeteSale; } set { zauzeteSale = value; OnPropertyChanged("ZauzeteSale"); } }
        #endregion

        #region Konstruktor
        public IzvjestajViewModel()
        {
            ZatvoriIzvjestaj = new MyICommand(ZatvoriProzor);
            GenerisiIzvjestajKomanda = new MyICommand(GenerisiIzvjestaj);
            OtvoriProstorije = new MyICommand(OtvoriSale);
            OtvoriZahtjeve = new MyICommand(OtvoriZahtjev);
            OtvoriKomunikaciju = new MyICommand(OtvoriProzorKomunikacije);
            OtvoriPomoc = new MyICommand(OtvoriPomocKorisniku);
            OtvoriOAplikaciji = new MyICommand(OtvoriOpis);
            SlobodneSale = new ObservableCollection<Sala>();
            ZauzeteSale = new ObservableCollection<Sala>();
            NadjiSlobodneSale();
            NadjiZauzeteSale();
        }
        #endregion

        #region IzvjestajViewModel
        private void NadjiSlobodneSale()
        {
            foreach(Sala sala in SaleServis.Sale())
            {
                if (!SalaZauzetaTrenutno(sala) && !sala.Namjena.Equals("Skladiste"))
                {
                    SlobodneSale.Add(sala);
                }
            }
        }
        private void NadjiZauzeteSale()
        {
            foreach (Sala sala in SaleServis.Sale())
            {
                if (SalaZauzetaTrenutno(sala) && !sala.Namjena.Equals("Skladiste"))
                {
                    ZauzeteSale.Add(sala);
                }
            }
        }

        private bool SalaZauzetaTrenutno(Sala sala)
        {
            foreach(ZauzeceSale zauzece in sala.zauzetiTermini)
            {
                string PocetakZauzeca = zauzece.datumPocetkaTermina + " " + zauzece.pocetakTermina;
                string KrajZauzeca = zauzece.datumKrajaTermina + " " + zauzece.krajTermina;
                DateTime Pocetak;
                DateTime Kraj;
                DateTime.TryParse(PocetakZauzeca, out Pocetak);
                DateTime.TryParse(KrajZauzeca, out Kraj);
                if(DateTime.Now >= Pocetak && DateTime.Now < Kraj)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region ZatvoriIzvjestajViewModel
        
        private void ZatvoriProzor()
        {
            IzvjestajProzor.Close();
            UpravnikViewModel.UpravnikProzor = new Upravnik();
            UpravnikViewModel.UpravnikProzor.Show();
            UpravnikViewModel.UpravnikProzor.DataContext = new UpravnikViewModel();
        }
        #endregion

        #region GenerisanjeIzvjestajaViewModel

        private void GenerisiIzvjestaj()
        {
            var pdfDoc = new Document(PageSize.LETTER, 40f, 40f, 60f, 60f);
            string path = $"C:\\Users\\pc\\Desktop\\SIMS\\informacioni-sistem-bolnice\\KT3\\Izvjestaj.pdf";
            PdfWriter.GetInstance(pdfDoc, new FileStream(path, FileMode.OpenOrCreate));
            pdfDoc.Open();

            var spacer = new Paragraph("")
            {
                SpacingBefore = 10f,
                SpacingAfter = 10f,
            };

            pdfDoc.Add(spacer);

            var tabelaOpisaDokumenta = new PdfPTable(new[] { .75f, 1f }){};
            tabelaOpisaDokumenta.AddCell("Datum: ");
            tabelaOpisaDokumenta.AddCell(DateTime.Now.Date.ToString().Split(' ')[0]);
            tabelaOpisaDokumenta.AddCell("Vrijeme: ");
            tabelaOpisaDokumenta.AddCell(DateTime.Now.Hour + ":" + DateTime.Now.Minute);
            tabelaOpisaDokumenta.AddCell("Opis dokumenta: ");
            tabelaOpisaDokumenta.AddCell("Izvjestaj o zauzecu sala");
            tabelaOpisaDokumenta.AddCell("Sifra dokumenta: ");
            tabelaOpisaDokumenta.AddCell("AX1"); 
            
            pdfDoc.Add(tabelaOpisaDokumenta);

            pdfDoc.Add(spacer);
            pdfDoc.Add(spacer);

            var sirinaKolona = new[] { 0.75f, 1.5f, 1.5f};

            if (SlobodneSale.Count != 0)
            {
                var tabelaSlobodnih = new PdfPTable(sirinaKolona) { };
                var zaglavlje = new PdfPCell(new Phrase("Slobodne sale"))
                {
                    Colspan = 3,
                    HorizontalAlignment = 1,
                    MinimumHeight = 3
                };

                tabelaSlobodnih.AddCell(zaglavlje);

                tabelaSlobodnih.AddCell("Broj sale");
                tabelaSlobodnih.AddCell("Namjena sale");
                tabelaSlobodnih.AddCell("Tip sale");

                foreach (Sala sala in SlobodneSale)
                {
                    tabelaSlobodnih.AddCell(sala.brojSale.ToString());
                    tabelaSlobodnih.AddCell(sala.Namjena);
                    tabelaSlobodnih.AddCell(sala.TipSale.ToString());
                }

                pdfDoc.Add(tabelaSlobodnih);
            }
            else
            {
                var nemaSala = new Paragraph("Nema slobodnih sala");
                pdfDoc.Add(nemaSala);
            }

            pdfDoc.Add(spacer);
            pdfDoc.Add(spacer);

            if (ZauzeteSale.Count != 0)
            {
                var tabelaZauzetih = new PdfPTable(sirinaKolona) { };
                var zaglavlje = new PdfPCell(new Phrase("Zauzete sale"))
                {
                    Colspan = 3,
                    HorizontalAlignment = 1,
                    MinimumHeight = 3
                };

                tabelaZauzetih.AddCell(zaglavlje);

                tabelaZauzetih.AddCell("Broj sale");
                tabelaZauzetih.AddCell("Namjena sale");
                tabelaZauzetih.AddCell("Tip sale");

                foreach (Sala sala in ZauzeteSale)
                {
                    tabelaZauzetih.AddCell(sala.brojSale.ToString());
                    tabelaZauzetih.AddCell(sala.Namjena);
                    tabelaZauzetih.AddCell(sala.TipSale.ToString());
                }

                pdfDoc.Add(tabelaZauzetih);
            }
            else
            {
                var nemaSala = new Paragraph("Nema zauzetih sala");
                pdfDoc.Add(nemaSala);
            }
            pdfDoc.Close();
        }

        #endregion

        #region OtvoriProstorijeViewModel
        
        private void OtvoriSale()
        {
            SaleViewModel.SaleProzor = new PrikaziSalu();
            SaleViewModel.SaleProzor.Show();
            SaleViewModel.SaleProzor.DataContext = new SaleViewModel();
            IzvjestajProzor.Close();
        }

        #endregion

        #region OtvoriZahtjeveViewModel
        private void OtvoriZahtjev()
        {
            ZahtjeviViewModel.ZahtjeviProzor = new Zahtjevi();
            ZahtjeviViewModel.ZahtjeviProzor.Show();
            ZahtjeviViewModel.ZahtjeviProzor.DataContext = new ZahtjeviViewModel();
            IzvjestajProzor.Close();
        }

        #endregion

        #region OtvoriKomunikacijuViewModel
        private void OtvoriProzorKomunikacije()
        {
            KomunikacijaViewModel.KomunikacijaProzor = new Komunikacija();
            KomunikacijaViewModel.KomunikacijaProzor.Show();
            KomunikacijaViewModel.KomunikacijaProzor.DataContext = new KomunikacijaViewModel();
            IzvjestajProzor.Close();
        }
        #endregion

        #region OAplikacijiViewModel

        private void OtvoriOpis()
        {
            OAplikacijiViewModel.OAplikacijiProzor = new OAplikaciji();
            OAplikacijiViewModel.OAplikacijiProzor.Show();
            OAplikacijiViewModel.OAplikacijiProzor.DataContext = new OAplikacijiViewModel();
        }

        #endregion

        #region PomocViewModel

        private void OtvoriPomocKorisniku()
        {
            PomocProzor = new IzvjestajPomoc();
            PomocProzor.Show();
            PomocProzor.DataContext = this;
        }

        #endregion
    }
}
