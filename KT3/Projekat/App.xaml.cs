using Projekat.Lokalizacija;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ResourceDictionary ThemeDictionary
        {
            // You could probably get it via its name with some query logic as well.
            get { return Resources.MergedDictionaries[0]; }
        }

        public void ChangeTheme(Uri uri)
        {
            ThemeDictionary.MergedDictionaries.Clear();
            ThemeDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });
        }

        public void ChangeLanguage(string currLang)
        {
            if (currLang.Equals("en-US"))
            {
                IzvorPrevoda.Instance.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            }
            else
            {
                IzvorPrevoda.Instance.CurrentCulture = new System.Globalization.CultureInfo("sr-LATN");
            }

        }
    }
}
