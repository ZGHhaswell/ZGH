using System;
using System.Collections.Generic;
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
using ExcelProcessTest.Tools;


namespace ExcelProcessTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public CalculationViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new CalculationViewModel();
            this.DataContext = ViewModel;

        }

        private void button_Write_Click(object sender, RoutedEventArgs e)
        {

            ExcelUtils.WriteToExcel(ViewModel.CalculationItems);
        }
    }
}
