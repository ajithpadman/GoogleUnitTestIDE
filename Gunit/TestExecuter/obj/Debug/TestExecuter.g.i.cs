﻿#pragma checksum "..\..\TestExecuter.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "29217C613D866FEA809D9320605525DC"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
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
using TestExecuter;


namespace TestExecuter {
    
    
    /// <summary>
    /// TestExecuter
    /// </summary>
    public partial class TestExecuter : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 35 "..\..\TestExecuter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBrowseExe;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\TestExecuter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBrowseGcov;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\TestExecuter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBrowserObj;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\TestExecuter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBrowseTestReport;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\TestExecuter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBrowseCoverageReport;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\TestExecuter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid myDataGrid;
        
        #line default
        #line hidden
        
        
        #line 132 "..\..\TestExecuter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.MetroProgressBar progess;
        
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
            System.Uri resourceLocater = new System.Uri("/TestExecuter;component/testexecuter.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\TestExecuter.xaml"
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
            this.btnBrowseExe = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\TestExecuter.xaml"
            this.btnBrowseExe.Click += new System.Windows.RoutedEventHandler(this.btnBrowseExe_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnBrowseGcov = ((System.Windows.Controls.Button)(target));
            
            #line 43 "..\..\TestExecuter.xaml"
            this.btnBrowseGcov.Click += new System.Windows.RoutedEventHandler(this.btnBrowseGcov_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnBrowserObj = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\TestExecuter.xaml"
            this.btnBrowserObj.Click += new System.Windows.RoutedEventHandler(this.btnBrowserObj_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnBrowseTestReport = ((System.Windows.Controls.Button)(target));
            
            #line 57 "..\..\TestExecuter.xaml"
            this.btnBrowseTestReport.Click += new System.Windows.RoutedEventHandler(this.btnBrowseTestReport_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnBrowseCoverageReport = ((System.Windows.Controls.Button)(target));
            
            #line 64 "..\..\TestExecuter.xaml"
            this.btnBrowseCoverageReport.Click += new System.Windows.RoutedEventHandler(this.btnBrowseCoverageReport_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.myDataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 8:
            this.progess = ((MahApps.Metro.Controls.MetroProgressBar)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 7:
            
            #line 75 "..\..\TestExecuter.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnRunAll_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

