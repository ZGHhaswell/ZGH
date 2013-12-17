using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Timers;
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
using WpfTest.Model;
using Timer = C1.Util.Timer;

namespace WpfTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class MainWindow : Window
    {

        public ObservableCollection<ProjInfoData> ItemsPool { get; set; }

        public MainWindow()
        {
           
            InitializeComponent();

            this.c1FlexGrid1.ItemsSource = ItemsPool= new ObservableCollection<ProjInfoData>();

            Timer timer = new Timer();
            timer.Interval = new TimeSpan(1000);
            timer.Tick += TimerOnTick;
           
            timer.Start();

        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            label.Content = this.Left + "  " + this.Top + "   " + this.RenderSize.Width + "  " + this.RenderSize.Height;
            
        }

     

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            var window = ServiceLocator.Current.GetInstance<MethodsWindow>();

            if (window.Visibility != Visibility.Visible) 
            {
                //window.Top = this.RenderSize.Height * 0.7;
                //window.Left = this.Left;
                //window.Width = this.RenderSize.Width;
                //window.Height = this.RenderSize.Height * 0.3;

                if (this.WindowState == WindowState.Maximized)
                {
                    window.Top = this.RenderSize.Height*0.7;
                    window.Left = 0;
                }
                else
                {
                    window.Top = this.Top + this.RenderSize.Height * 0.7;
                    window.Left = this.Left;
                }
                
                window.Width = this.RenderSize.Width;
                window.Height = this.RenderSize.Height * 0.3;

                
                window.Owner = this;
                window.ViewModel.QdItemsViewModel.UserDoubleClickEvent -= WindowOnActionEvent;
                window.ViewModel.QdItemsViewModel.UserDoubleClickEvent += WindowOnActionEvent;
                window.Visibility = Visibility.Visible;
            }

            

        }

        private void WindowOnActionEvent(ProjInfoData data)
        {
            ItemsPool.Add(data);
        }

       
    }
}
