using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Data;

namespace Projekat.Lokalizacija
{
    class IzvorPrevoda : INotifyPropertyChanged
    {
        private static readonly IzvorPrevoda instance = new IzvorPrevoda();
        public static IzvorPrevoda Instance
        {
            get { return instance; }
        }

        private readonly ResourceManager menadzerResursa = Properties.Resources.ResourceManager;
        private CultureInfo trenutnaKultura = null;

        public string this[string key]
        {
            get
            {
                string retVal = this.menadzerResursa.GetString(key, this.trenutnaKultura);
                return retVal;
            }
        }

        public CultureInfo CurrentCulture
        {
            get { return this.trenutnaKultura; }
            set
            {
                if (this.trenutnaKultura != value)
                {
                    this.trenutnaKultura = value;
                    var @event = this.PropertyChanged;
                    if (@event != null)
                    {
                        @event.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class LocExtension : Binding
    {
        public LocExtension(string name)
            : base("[" + name + "]")
        {
            this.Mode = BindingMode.OneWay;
            this.Source = IzvorPrevoda.Instance;
        }
    }
}
