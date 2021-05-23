using Projekat.Pomoc;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Projekat.ViewModel
{
    class ZahtjeviViewModel : BindableBase
    {
        #region ZahtjeviViewModel
        public MyICommand LijekoviProzor { get; set; }
        public MyICommand SkladisteKomanda { get; set; }
        public MyICommand ZatvoriZahtjeveKomanda { get; set; }
        public MyICommand OtvoriSale { get; set; }
        public MyICommand OtvoriKomunikaciju { get; set; }
        public MyICommand OtvoriOAplikaciji { get; set; }
        public static Window ZahtjeviProzor { get; set; }//zatvori prozor sa kog si dosla(kad sve bude uvezano...)
        
        public ZahtjeviViewModel()
        {
            LijekoviProzor = new MyICommand(OtvoriLijekove);
            SkladisteKomanda = new MyICommand(OtvoriSkladiste);
            ZatvoriZahtjeveKomanda = new MyICommand(ZatvoriZahtjeve);
            OtvoriSale = new MyICommand(PrikaziSale);
            OtvoriKomunikaciju = new MyICommand(PrikaziKomunikaciju);
            OtvoriOAplikaciji = new MyICommand(PrikaziOpis);
        }
        private void PrikaziOpis()
        {
            OAplikacijiViewModel.OAplikacijiProzor = new OAplikaciji();
            OAplikacijiViewModel.OAplikacijiProzor.Show();
            OAplikacijiViewModel.OAplikacijiProzor.DataContext = new OAplikacijiViewModel();
        }
        private void PrikaziKomunikaciju()
        {
            KomunikacijaViewModel.KomunikacijaProzor = new Komunikacija();
            KomunikacijaViewModel.KomunikacijaProzor.Show();
            KomunikacijaViewModel.KomunikacijaProzor.DataContext = new Komunikacija();
            ZahtjeviProzor.Close();
        }
        private void PrikaziSale()
        {
            SaleViewModel.SaleProzor = new PrikaziSalu();
            SaleViewModel.SaleProzor.Show();
            SaleViewModel.SaleProzor.DataContext = new SaleViewModel();
            ZahtjeviProzor.Close();
        }
        private void ZatvoriZahtjeve()
        {
            UpravnikViewModel.UpravnikProzor = new Upravnik();
            UpravnikViewModel.UpravnikProzor.Show();
            UpravnikViewModel.UpravnikProzor.DataContext = new UpravnikViewModel();
            ZahtjeviProzor.Close();
        }
        #endregion
        #region LijekoviViewModel
        private void OtvoriLijekove()
        {
            LijekoviViewModel.LijekoviProzor = new Lijekovi();
            LijekoviViewModel.LijekoviProzor.Show();
            LijekoviViewModel.LijekoviProzor.DataContext = new LijekoviViewModel();
            ZahtjeviProzor.Close();
        }
        #endregion
        #region SkladisteViewModel
        private void OtvoriSkladiste()
        {
            try
            {
                SkladisteViewModel.SkladisteProzor = new Skladiste();
                SkladisteViewModel.SkladisteProzor.Show();
                SkladisteViewModel.otvoren = true;
                PremjestajServis.odradiZakazanePremjestaje();
                SkladisteViewModel.SkladisteProzor.DataContext = new SkladisteViewModel();
            }
            catch (Exception ex) { Console.WriteLine(ex.Data); }
        }
        #endregion
    }
}
