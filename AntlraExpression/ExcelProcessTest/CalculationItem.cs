using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;

namespace ExcelProcessTest
{
    public class CalculationItem : NotificationObject
    {
        public CalculationItem()
        {
            
        }

        public string CalculationName { get; set; }
        public double CalculationResult { get; set; }
    }
}
