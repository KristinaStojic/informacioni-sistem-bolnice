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
        private void OtvoriLijekove()
        {
            LijekProzor = new Lijekovi();
            LijekProzor.Show();
            LijekProzor.DataContext = new LijekoviViewModel();
            //ZahtjeviProzor.Close();
        }

        private void OtvoriSkladiste()
        {
            SkladisteProzor = new Skladiste();
            SkladisteProzor.Show();
            SkladisteProzor.DataContext = new SkladistaViewModel();
        }
        #endregion
    }
}
