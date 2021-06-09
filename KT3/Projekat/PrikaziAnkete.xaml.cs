using Model;
using Projekat.Model;
using Projekat.Servis;
using Projekat.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat
{
    public partial class PrikaziAnkete : Page
    {
        private static int idPacijent;
        public PrikaziAnkete(int idPrijavljenogPacijenta)
        {
            InitializeComponent();
            idPacijent = idPrijavljenogPacijenta;
            Pacijent prijavljeniPacijent = PacijentiServis.PronadjiPoId(idPacijent);
            this.podaci.Header = prijavljeniPacijent.ImePacijenta.Substring(0, 1) + ". " + prijavljeniPacijent.PrezimePacijenta;
            PacijentWebStranice.AktivnaTema(this.zaglavlje, this.SvetlaTema, this.tamnaTema); 
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new AnketeViewModel(this.NavigationService, idPacijent);
        }
    }
}
