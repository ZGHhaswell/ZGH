using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.ServiceLocation;

namespace WpfTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            var window = ServiceLocator.Current.GetInstance<MethodsWindow>();




            
            if (!window.IsActive)
            {
                window.Top = this.RenderSize.Height * 0.7;
                window.Left = 0;
                window.Width = this.RenderSize.Width;
                window.Height = this.RenderSize.Height * 0.3;


                window.Owner = this;
                window.ActionEvent -= WindowOnActionEvent;
                window.ActionEvent += WindowOnActionEvent;
                window.Visibility = Visibility.Visible;
            }

            

        }

        private void WindowOnActionEvent(string s)
        {
            this.label1.Content = s;
        }

       
    }
}
