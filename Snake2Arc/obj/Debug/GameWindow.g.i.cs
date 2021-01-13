﻿#pragma checksum "..\..\GameWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "A53E12E9ED7142360CCCE2DBE9BD201E7B101D363E5B1267DAF58F5D694E3E13"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Snake2Arc;
using System;
using System.ComponentModel;
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


namespace Snake2Arc {
    
    
    /// <summary>
    /// GameWindow
    /// </summary>
    public partial class GameWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 2 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Snake2Arc.GameWindow MainWindow;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Viewbox mainViewBox;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock scoreBoard;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Viewbox viewBoxCanvas;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas paintCanvas;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border mainMenu;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border leaderBoard;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border addNewScore;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox playerNameAdded;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button_add_new_score;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border gameOver1Player;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock scoreWin1Player;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border gameOver2Player;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock whoLose2Player;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border pauseMenu;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border optionMenu;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider thickSlider;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\GameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox checkMusic;
        
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
            System.Uri resourceLocater = new System.Uri("/Snake2Arc;component/gamewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\GameWindow.xaml"
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
            this.MainWindow = ((Snake2Arc.GameWindow)(target));
            return;
            case 2:
            this.mainViewBox = ((System.Windows.Controls.Viewbox)(target));
            return;
            case 3:
            this.scoreBoard = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.viewBoxCanvas = ((System.Windows.Controls.Viewbox)(target));
            return;
            case 5:
            this.paintCanvas = ((System.Windows.Controls.Canvas)(target));
            return;
            case 6:
            this.mainMenu = ((System.Windows.Controls.Border)(target));
            return;
            case 7:
            
            #line 39 "..\..\GameWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Play_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 40 "..\..\GameWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Play_Double_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 41 "..\..\GameWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_LeaderBoard_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 42 "..\..\GameWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Options_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 43 "..\..\GameWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BtnCloseClick);
            
            #line default
            #line hidden
            return;
            case 12:
            this.leaderBoard = ((System.Windows.Controls.Border)(target));
            return;
            case 13:
            
            #line 59 "..\..\GameWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_return_menu_click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.addNewScore = ((System.Windows.Controls.Border)(target));
            return;
            case 15:
            this.playerNameAdded = ((System.Windows.Controls.TextBox)(target));
            return;
            case 16:
            this.button_add_new_score = ((System.Windows.Controls.Button)(target));
            
            #line 66 "..\..\GameWindow.xaml"
            this.button_add_new_score.Click += new System.Windows.RoutedEventHandler(this.Button_add_new_score);
            
            #line default
            #line hidden
            return;
            case 17:
            this.gameOver1Player = ((System.Windows.Controls.Border)(target));
            return;
            case 18:
            this.scoreWin1Player = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 19:
            
            #line 73 "..\..\GameWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_return_menu_click_after_game);
            
            #line default
            #line hidden
            return;
            case 20:
            this.gameOver2Player = ((System.Windows.Controls.Border)(target));
            return;
            case 21:
            this.whoLose2Player = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 22:
            
            #line 79 "..\..\GameWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_return_menu_click);
            
            #line default
            #line hidden
            return;
            case 23:
            this.pauseMenu = ((System.Windows.Controls.Border)(target));
            return;
            case 24:
            
            #line 85 "..\..\GameWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_resume_click);
            
            #line default
            #line hidden
            return;
            case 25:
            
            #line 86 "..\..\GameWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_leave_click);
            
            #line default
            #line hidden
            return;
            case 26:
            this.optionMenu = ((System.Windows.Controls.Border)(target));
            return;
            case 27:
            this.thickSlider = ((System.Windows.Controls.Slider)(target));
            
            #line 93 "..\..\GameWindow.xaml"
            this.thickSlider.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.Change_Snake_Thick);
            
            #line default
            #line hidden
            return;
            case 28:
            this.checkMusic = ((System.Windows.Controls.CheckBox)(target));
            
            #line 94 "..\..\GameWindow.xaml"
            this.checkMusic.Unchecked += new System.Windows.RoutedEventHandler(this.Change_Check_Music);
            
            #line default
            #line hidden
            
            #line 94 "..\..\GameWindow.xaml"
            this.checkMusic.Checked += new System.Windows.RoutedEventHandler(this.Change_Check_Music);
            
            #line default
            #line hidden
            return;
            case 29:
            
            #line 95 "..\..\GameWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_return_menu_click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

