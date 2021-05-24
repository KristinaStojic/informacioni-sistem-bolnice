using Model;
using Projekat.Model;
using Projekat.Pomoc;
using Projekat.Servis;
using Projekat.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for Skladiste.xaml
    /// </summary>
    public partial class Skladiste : Window
    {
        public Skladiste()
        {
            InitializeComponent();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            SkladistePomoc skladistePomoc = new SkladistePomoc();
            skladistePomoc.Show();
        }

    }



    public class ObservableCollectionEx<t> : ObservableCollection<t>
    {
        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        public ObservableCollectionEx(IEnumerable<t> collection) : base(collection) { }
        public ObservableCollectionEx(List<t> collection) : base(collection) { }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            using (BlockReentrancy())
            {
                var eventHandler = CollectionChanged;
                if (eventHandler != null)
                {
                    Delegate[] delegates = eventHandler.GetInvocationList();
                    
                    foreach (NotifyCollectionChangedEventHandler handler in delegates)
                    {
                        var dispatcherObject = handler.Target as DispatcherObject;
                    
                        if (dispatcherObject != null && dispatcherObject.CheckAccess() == false)
                    
                            dispatcherObject.Dispatcher.Invoke(DispatcherPriority.DataBind,
                                          handler, this, e);
                        else 
                            handler(this, e);
                    }
                }
            }
        }
    }
}
