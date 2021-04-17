using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Projekat.Model;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for ObavestenjaPacijent.xaml
    /// </summary>
    public partial class ObavestenjaPacijent : Window
    {
        public static ObservableCollection<Obavestenja> obavestenjaPacijent
        {
            get;
            set;
        }
        public ObavestenjaPacijent()
        {
            InitializeComponent();
            this.DataContext = this;
            obavestenjaPacijent = new ObservableCollection<Obavestenja>();
            foreach (Obavestenja o in ObavestenjaMenadzer.obavestenja)
            {
                obavestenjaPacijent.Add(o);
            }
        }
    }
}
