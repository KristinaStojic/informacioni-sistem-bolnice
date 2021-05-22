using Projekat.Model;
using Projekat.Pomoc;
using Projekat.Servis;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for Lijekovi.xaml
    /// </summary>
    public partial class Lijekovi : Window
    {

        public Lijekovi()
        {
            InitializeComponent();
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            LijekoviPomoc lijekoviPomoc = new LijekoviPomoc();
            lijekoviPomoc.Show();
        }

    }
}
