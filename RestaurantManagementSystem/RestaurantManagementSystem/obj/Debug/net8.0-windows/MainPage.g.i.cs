// Updated by XamlIntelliSenseFileGenerator 29-12-2023 23:31:52
#pragma checksum "..\..\..\MainPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "FE39B6414A502DF215FE23A76AADA952BDE9F375"
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
using RestaurantManagementSystem;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace RestaurantManagementSystem
{


    /// <summary>
    /// DashboardWindow
    /// </summary>
    public partial class DashboardWindow : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector
    {

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/RestaurantManagementSystem;component/mainpage.xaml", System.UriKind.Relative);

#line 1 "..\..\..\MainPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:

#line 26 "..\..\..\MainPage.xaml"
                    ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ImageAsButton_Click);

#line default
#line hidden
                    return;
                case 2:

#line 36 "..\..\..\MainPage.xaml"
                    ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuButton_Click);

#line default
#line hidden
                    return;
                case 3:

#line 37 "..\..\..\MainPage.xaml"
                    ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BillsButton_Click);

#line default
#line hidden
                    return;
                case 4:

#line 38 "..\..\..\MainPage.xaml"
                    ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.TablesButton_Click);

#line default
#line hidden
                    return;
                case 5:

#line 39 "..\..\..\MainPage.xaml"
                    ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SalesButton_Click);

#line default
#line hidden
                    return;
                case 6:
                    this.dashboardBorder = ((System.Windows.Controls.Border)(target));
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

