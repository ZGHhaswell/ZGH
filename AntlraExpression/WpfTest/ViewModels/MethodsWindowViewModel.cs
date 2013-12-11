using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Prism.ViewModel;

namespace WpfTest.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MethodsWindowViewModel : NotificationObject
    {
        [Import]
        public QdItemsViewModel QdItemsViewModel { get; set; }


        public MethodsWindowViewModel()
        {
            //MessageBox.Show("MethodsWindowViewModel");
        }
    }
}
