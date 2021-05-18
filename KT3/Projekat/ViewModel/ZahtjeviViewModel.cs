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
        public static Window LijekProzor { get; set; }
        public static Window ZahtjeviProzor { get; set; }//zatvori prozor sa kog si dosla(kad sve bude uvezano...)
        public ZahtjeviViewModel()
        {
            LijekoviProzor = new MyICommand(OtvoriLijekove);
        }
        private void OtvoriLijekove()
        {
            LijekProzor = new Lijekovi();
            LijekProzor.DataContext = new LijekoviViewModel();
            LijekProzor.Show();
        }
        #endregion
    }
}
