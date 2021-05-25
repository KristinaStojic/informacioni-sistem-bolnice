using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Projekat.ViewModel
{
    public class PrikaziTerminViewModel : BindableBase
    {
        private static int idPacijent = 1;
        private ObservableCollection<Obavestenja> obavestenja;
        public ObservableCollection<Obavestenja> Obavestenja { get { return obavestenja; } set { obavestenja = value; OnPropertyChanged("Obavestenja"); } }

        public MyICommand OdjavaKomanda { get; set; }

        public PrikaziTerminViewModel()
        {
            OdjavaKomanda = new MyICommand(ZatvoriAplikaciju);
            DodajObavestenja();
        }

        private void DodajObavestenja()
        {
            Obavestenja = new ObservableCollection<Obavestenja>();
            Obavestenja = ObavestenjaServis.DodajObavestenja(idPacijent);
        }
        private void ZatvoriAplikaciju()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
