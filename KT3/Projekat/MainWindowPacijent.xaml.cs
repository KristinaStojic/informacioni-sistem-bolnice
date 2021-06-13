using Model;
using Projekat.Model;
using Projekat.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projekat
{
    public partial class MainWindowPacijent : Window
    {
        public MainWindowPacijent()
        {
            InitializeComponent();
            PrikaziTermin.pacijentProzor = true;
            AnketaServis anketaServis = new AnketaServis();
            anketaServis.NadjiSveAnkete();
            ProxyMalicioznoPonasanjeServis proxy = new ProxyMalicioznoPonasanjeServis();
            proxy.NadjiSvaMalicioznaPonasanja();
            var app = (App)Application.Current;
            app.ChangeLanguage("sr-LATN");  
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //TerminServis.sacuvajIzmene();
            //ObavestenjaServis.sacuvajIzmene();
            ProxyMalicioznoPonasanjeServis proxy = new ProxyMalicioznoPonasanjeServis();
            proxy.sacuvajIzmene();
            //TerminServisLekar.sacuvajIzmene();
            SaleServis.sacuvajIzmjene();
            //PacijentiMenadzer.SacuvajIzmene();
            AnketaMenadzer.sacuvajIzmene();
            
            PrikaziTermin.pacijentProzor = false;  // nit
            LekariMenadzer.SacuvajIzmeneLekara();
        }
    }
}
