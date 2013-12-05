using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;

namespace ExcelProcessTest
{
    public class CalculationViewModel : NotificationObject
    {
        public ObservableCollection<CalculationItem> CalculationItems { get; set; }

        public CalculationViewModel()
        {
            DataInit();
        }

        private void DataInit()
        {
            CalculationItems = new ObservableCollection<CalculationItem>()
                {
                    new CalculationItem(){CalculationName = "底面积", CalculationResult = 10.0},
                    new CalculationItem(){CalculationName = "体积", CalculationResult = 20.0},
                    new CalculationItem(){CalculationName = "侧面积", CalculationResult = 20.0},
                };
        }


    }
}
