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
using System.Windows.Shapes;
using WpfTest.ViewModels;

namespace WpfTest
{
    /// <summary>
    /// MethodsWindow.xaml 的交互逻辑
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MethodsWindow : Window
    {
        [Import]
        public MethodsWindowViewModel ViewModel
        {
            get { return DataContext as MethodsWindowViewModel; }
            set { DataContext = value; }
        }

        public event Action<string> ActionEvent;

        public MethodsWindow()
        {
            InitializeComponent();
            
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            e.Cancel = true;

            base.OnClosing(e);
        }

        //private void button1_Click(object sender, RoutedEventArgs e)
        //{
        //    var str = this.textBox1.Text;
        //    ActionEvent(str);
        //}
    }
}
