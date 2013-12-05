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

using CalExcel = Microsoft.Office.Interop.Excel;

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
            
            //引用Excel对象 excel.
            CalExcel.Application excel = new CalExcel.Application();
            //引用Excel工作簿 
            excel.Workbooks.Add(true);

            for (int i = 0; i < ViewModel.CalculationItems.Count; i++)
            {
                excel.Cells[i+1, 1] = ViewModel.CalculationItems[i].CalculationName;
                excel.Cells[i+1, 2] = ViewModel.CalculationItems[i].CalculationResult;
            }



            //使Excel可视 
            //excel.Visible = true;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\demo.xlsx";
            CalExcel.Worksheet xSheet = (CalExcel.Worksheet)excel.ActiveSheet;
            
            xSheet.SaveAs(path);
            xSheet = null;

            excel.Quit();
            excel = null;
        }
    }
}
