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
        public static Window LijekProzor { get; set; }
        public static Window SkladisteProzor { get; set; }
        public static Window ZahtjeviProzor { get; set; }//zatvori prozor sa kog si dosla(kad sve bude uvezano...)
        
        public ZahtjeviViewModel()
        {
            LijekoviProzor = new MyICommand(OtvoriLijekove);
            SkladisteKomanda = new MyICommand(OtvoriSkladiste);
        }
        #endregion
        #region LijekoviViewModel
        private void OtvoriLijekove()
        {
            LijekProzor = new Lijekovi();
            LijekProzor.Show();
            LijekProzor.DataContext = new LijekoviViewModel();
            //ZahtjeviProzor.Close();
        }
        #endregion
        #region SkladisteViewModel
        private void OtvoriSkladiste()
        {
            try
            {
                SkladisteProzor = new Skladiste();
                SkladisteProzor.Show();
                SkladisteViewModel.otvoren = true;
                PremjestajServis.odradiZakazanePremjestaje();
                SkladisteProzor.DataContext = new SkladisteViewModel();
            }
            catch (Exception ex) { Console.WriteLine(ex.Data); }
        }
        #endregion
    }
}
