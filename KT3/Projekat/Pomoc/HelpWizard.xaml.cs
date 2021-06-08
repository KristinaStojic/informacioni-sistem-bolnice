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

namespace Projekat.Pomoc
{
    /// <summary>
    /// Interaction logic for HelpWizard.xaml
    /// </summary>
    public partial class HelpWizard : Window
    {
        public HelpWizard()
        {
            InitializeComponent();
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Nastavi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            WizardPrikazTermina termini = new WizardPrikazTermina();
            termini.ShowDialog();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Nazad_Click(sender, e);
            }
            else if (e.Key == Key.N && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Nazad_Click(sender, e);
            }
            else if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Nastavi_Click(sender, e);
            }
            else if (e.Key == Key.S && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Nastavi_Click(sender, e);
            }
        }
    }
}
