﻿#pragma checksum "C:\Users\Rien\Documents\Artesis\Lessen\semester 4\Project ICT 4\Project-ICT\src\Project ICT 4 - Medicine Planner (C#)\MedAgent 0.1\MedAgent 0.1\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "AB85D2E2A6C71C85DF59E8D56CA0F5E0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace MedAgent_0_1 {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Button PatientButton;
        
        internal System.Windows.Controls.Primitives.Popup LogInPopup;
        
        internal System.Windows.Controls.TextBox EmailBox;
        
        internal System.Windows.Controls.Button LogInButtonPatient;
        
        internal System.Windows.Controls.Button CancelButton;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/MedAgent_0_1;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.PatientButton = ((System.Windows.Controls.Button)(this.FindName("PatientButton")));
            this.LogInPopup = ((System.Windows.Controls.Primitives.Popup)(this.FindName("LogInPopup")));
            this.EmailBox = ((System.Windows.Controls.TextBox)(this.FindName("EmailBox")));
            this.LogInButtonPatient = ((System.Windows.Controls.Button)(this.FindName("LogInButtonPatient")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
        }
    }
}

