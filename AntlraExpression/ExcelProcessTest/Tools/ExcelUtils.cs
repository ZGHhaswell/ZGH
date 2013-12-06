using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CalExcel = Microsoft.Office.Interop.Excel;

namespace ExcelProcessTest.Tools
{
    public static class ExcelUtils
    {
        public static void WriteToExcel(IEnumerable<IBridgeToExcel> source)
        {
            
            

            //引用Excel对象 excel.
            CalExcel.Application excel = new CalExcel.Application();
            //引用Excel工作簿 
            excel.Workbooks.Add(true);


            int i = 0;
            foreach (var bridgeToExcel in source)
            {
                var data = bridgeToExcel.GetData();
                i++;
                int j = 0;
                foreach (var da in data)
                {
                    j++;
                    excel.Cells[i, j] = da;
                }
            }

            //for (int i = 0; i < ViewModel.CalculationItems.Count; i++)
            //{
            //    excel.Cells[i + 1, 1] = ViewModel.CalculationItems[i].CalculationName;
            //    excel.Cells[i + 1, 2] = ViewModel.CalculationItems[i].CalculationResult;
            //}

            //使Excel可视 
            excel.Visible = true;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\demo.xlsx";
            CalExcel.Worksheet xSheet = (CalExcel.Worksheet)excel.ActiveSheet;

            xSheet.SaveAs(path);
            xSheet = null;

            excel.Quit();
            excel = null;

        }
        

    }
}
