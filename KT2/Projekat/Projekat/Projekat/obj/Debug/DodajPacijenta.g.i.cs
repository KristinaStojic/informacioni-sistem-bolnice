﻿#pragma checksum "..\..\DodajPacijenta.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "56589A07B4DAA63E325282CBDFA6B64C5E62996456A7BE30EDF82B8FF6FDDB5A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Projekat.Model;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Projekat.Model {
    
    
    /// <summary>
    /// DodajPacijenta
    /// </summary>
    public partial class DodajPacijenta : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 33 "..\..\DodajPacijenta.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ime;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\DodajPacijenta.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox prezime;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\DodajPacijenta.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox jmbg;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\DodajPacijenta.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox combo;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\DodajPacijenta.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox brojTelefona;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\DodajPacijenta.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox email;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\DodajPacijenta.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox adresa;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Projekat;component/dodajpacijenta.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\DodajPacijenta.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ime = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.prezime = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.jmbg = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.combo = ((System.Windows.Controls.ComboBox)(target));
            
            #line 36 "..\..\DodajPacijenta.xaml"
            this.combo.LostFocus += new System.Windows.RoutedEventHandler(this.combo_LostFocus);
            
            #line default
            #line hidden
            return;
            case 5:
            this.brojTelefona = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.email = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.adresa = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            
            #line 43 "..\..\DodajPacijenta.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Potvrdi_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 44 "..\..\DodajPacijenta.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Odustani_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

