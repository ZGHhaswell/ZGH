using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelProcessTest.Tools;
using Microsoft.Practices.Prism.ViewModel;

namespace ExcelProcessTest
{
    public class CalculationItem : NotificationObject, IBridgeToExcel
    {
        public CalculationItem()
        {
            
        }

        public string CalculationName { get; set; }
        public double CalculationResult { get; set; }

        public IEnumerable<string> GetData()
        {
            return new List<string>()
            {
                CalculationName,
                CalculationResult.ToString()
            };
        }
       
    }
}
