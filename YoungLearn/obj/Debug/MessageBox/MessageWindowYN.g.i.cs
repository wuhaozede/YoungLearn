﻿#pragma checksum "..\..\..\MessageBox\MessageWindowYN.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "39D1AA51511FA9E8428C0B208F859EA4044FC159652061840EE810E109647CF8"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace fristArcGisEngineDome {
    
    
    /// <summary>
    /// MessageWindowYN
    /// </summary>
    public partial class MessageWindowYN : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\MessageBox\MessageWindowYN.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_exit;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\MessageBox\MessageWindowYN.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock C1;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\MessageBox\MessageWindowYN.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_ConfirmN;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\MessageBox\MessageWindowYN.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_ConfirmY;
        
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
            System.Uri resourceLocater = new System.Uri("/YoungLearn;component/messagebox/messagewindowyn.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MessageBox\MessageWindowYN.xaml"
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
            
            #line 8 "..\..\..\MessageBox\MessageWindowYN.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.Rectangle_MouseMove);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btn_exit = ((System.Windows.Controls.Button)(target));
            
            #line 10 "..\..\..\MessageBox\MessageWindowYN.xaml"
            this.btn_exit.Click += new System.Windows.RoutedEventHandler(this.btn_exit_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.C1 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.btn_ConfirmN = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\..\MessageBox\MessageWindowYN.xaml"
            this.btn_ConfirmN.Click += new System.Windows.RoutedEventHandler(this.btn_ConfirmN_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btn_ConfirmY = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\MessageBox\MessageWindowYN.xaml"
            this.btn_ConfirmY.Click += new System.Windows.RoutedEventHandler(this.btn_ConfirmY_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

