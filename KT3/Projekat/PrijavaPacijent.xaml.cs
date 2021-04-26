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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrijavaPacijent.xaml
    /// </summary>
    public partial class PrijavaPacijent : Page
    {
        public PrijavaPacijent()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void prijava_Click(object sender, RoutedEventArgs e)
        {
            int IdPacijent = Int32.Parse(this.korisnickoIme.Text);
            Page pocetna = new PrikaziTermin(IdPacijent);
            this.NavigationService.Navigate(pocetna);

        }
    }
}
