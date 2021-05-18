using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projekat.ViewModel
{
    
    class BrisanjeLijeka : ObservableObject
    {
        public RelayCommand ObrisiLijekCommand { get; private set; }
        public BrisanjeLijeka()
        {
            ObrisiLijekCommand = new RelayCommand(ObrisiLijek);
        }
        private void ObrisiLijek()
        {
            LekoviServis.obrisiLijek(izabraniLijek);
            Lijekovi.Lekovi.Remove(izabraniLijek);
            this.Close();
        }
    }

   
}
